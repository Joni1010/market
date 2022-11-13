using AppVEConector.libs;
using MarketObjects;
using QuikConnector.MarketObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AppVEConector
{
    public partial class Form_GraphicDepth : Form
    {

        static int[] PERCENT = { 30, 50, 70, 100 };
        /// <summary>
        /// Инициализация панели с контроля сделки
        /// </summary>
        private void InitPanelControl()
        {
            numericUpDownMCPriceLastTrade.InitSecurity(Securities);
            numericUpDownMCStopTiks.InitWheelDecimal(0, 1000, 1, 0);
            numericUpDownMCTakeProfitTiks.InitWheelDecimal(0, 1000, 1, 0);

            //Получаем сохраненные данные
            numericUpDownMCStopTiks.Value = SettingsDepth.Get("TiksControlStop");
            numericUpDownMCTakeProfitTiks.Value = SettingsDepth.Get("TiksControlTake");

            numericUpDownMCStopTiks.ValueChanged += (s, e) =>
            {
                SettingsDepth.Set("TiksControlStop", (int)numericUpDownMCStopTiks.Value);
            };
            numericUpDownMCTakeProfitTiks.ValueChanged += (s, e) =>
            {
                SettingsDepth.Set("TiksControlTake", (int)numericUpDownMCTakeProfitTiks.Value);
            };
            //ВЫставляем флажок на сохраненном проценте
            foreach (var percent in PERCENT)
            {
                var objRadio = GetRadioButtonByPercent(percent);
                if (objRadio.NotIsNull())
                {
                    if (percent == SettingsDepth.Get("PercentTakeOut"))
                    {
                        objRadio.Checked = true;
                    }
                    objRadio.CheckedChanged += (s, e) =>
                    {
                        SettingsDepth.Set("PercentTakeOut", percent);
                    };
                }
            }

            buttonMCClose.Click += (s, e) =>
            {
                var sOrders = GetStopOrdersForControl();
                foreach (var sOrd in sOrders)
                {
                    Quik.Trader.CancelStopOrder(sOrd);
                }
            };
            buttonMCReset.Click += (s, e) =>
            {
                LoopControl(true);
            };

            buttonACPriceUp.Click += (s, e) =>
            {
                numericUpDownMCStopTiks.Value++;
                numericUpDownMCTakeProfitTiks.Value = numericUpDownMCTakeProfitTiks.Value > 0 ? numericUpDownMCTakeProfitTiks.Value - 1 : 0;
            };
            buttonACPriceDown.Click += (s, e) =>
            {
                numericUpDownMCStopTiks.Value = numericUpDownMCStopTiks.Value > 0 ? numericUpDownMCStopTiks.Value - 1 : 0;
                numericUpDownMCTakeProfitTiks.Value++;
            };
        }

        public void LoopControl(bool ReCalculate = false)
        {
            if (checkBoxMCActivate.Checked)
            {
                var pos = GetPositionForControl();
                if (pos.NotIsNull() && pos.CurrentVolume > 0)
                {
                    var sOrders = GetStopOrdersForControl();
                    if (ReCalculate || !CheckStopOrdersForControls(sOrders))
                    {
                        foreach (var sOrd in sOrders)
                        {
                            Quik.Trader.CancelStopOrder(sOrd);
                        }
                        return;
                    }
                    if (sOrders.NotIsNull() && sOrders.Count() == 0) ///*&& (!CheckStopOrdersForControls(sOrders)*/)
                    {
                        //Защита от большого числа заявок
                        if (sOrders.Count() > 2)
                        {
                            ShowTransReply("Контроль риска. Стоп-заявки не снимаются.");
                            return;
                        }
                        var lastMyTrade = GetLastMyTradeForControl(pos.Data.CurrentNet);
                        decimal Price = 0;
                        if (lastMyTrade.NotIsNull())
                        {
                            //Если выбран, то берем цену из поля цены
                            if (checkBoxMCSetPrice.Checked)
                            {
                                Price = numericUpDownMCPriceLastTrade.Value;
                            }
                            else
                            {
                                Price = numericUpDownMCPriceLastTrade.Value = lastMyTrade.Trade.Price;
                            }
                        }
                        else
                        {
                            ShowTransReply("Контроль риска. Не удалось получить последнюю сделку.");
                            return;
                        }
                        if (Price > 0 && numericUpDownMCStopTiks.Value > 0 && numericUpDownMCTakeProfitTiks.Value > 0)
                        {
                            var newStopOrder = GetStopOrderControl(pos.Data.CurrentNet, Price);
                            var takePos = getCurrectPosByPercent(pos);
                            var newTakeProfit = GetTakeProfitOrderControl(pos.Data.CurrentNet, takePos, Price);
                            if (newStopOrder.NotIsNull() && checkBoxMCStopLoss.Checked)
                            {
                                Quik.Trader.CreateStopOrder(newStopOrder, StopOrderType.StopLimit);
                            }
                            if (newTakeProfit.NotIsNull() && checkBoxMCTakeProfit.Checked)
                            {
                                Quik.Trader.CreateStopOrder(newTakeProfit, StopOrderType.TakeProfit);
                            }
                        }
                        else
                        {
                            ShowTransReply("Контроль риска. Одно из значений равно 0.");
                            return;
                        }
                    }
                }
                else
                {
                    var sOrders = GetStopOrdersForControl();
                    foreach (var sOrd in sOrders)
                    {
                        Quik.Trader.CancelStopOrder(sOrd);
                        Quik.Trader.CancelAllOrder(Securities);
                        Quik.Trader.CancelAllStopOrder(Securities);
                    }
                    //Снимаем флаг фиксации цены, чтобы при последующих ордерах не выставлялось не корректно.
                    checkBoxMCSetPrice.Checked = false;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="Price"></param>
        /// <returns></returns>
        private StopOrder GetStopOrderControl(int position, decimal Price)
        {
            decimal stepStop = Securities.Params.MinPriceStep * 20;
            var stopOrder = new StopOrder()
            {
                Sec = Securities,
                Volume = position < 0 ? position * -1 : position,
                DateExpiry = DateMarket.ExtractDateTime(DateTime.Now.AddDays(1)),
                ClientCode = ClientCode.Value,
                Comment = Define.STOP_CONTROL
            };
            if (position > 0)
            {
                Price = Price - numericUpDownMCStopTiks.Value * Securities.Params.MinPriceStep;
                stopOrder.ConditionPrice = Price;
                stopOrder.Direction = OrderDirection.Sell;
                stopOrder.Price = Price - stepStop;
            }
            else if (position < 0)
            {
                Price = Price + numericUpDownMCStopTiks.Value * Securities.Params.MinPriceStep;
                stopOrder.ConditionPrice = Price;
                stopOrder.Direction = OrderDirection.Buy;
                stopOrder.Price = Price + stepStop;
            }
            return stopOrder;
        }

        private StopOrder GetTakeProfitOrderControl(int position, int percentPosition, decimal Price)
        {
            bool isBuy = position < 0 ? true : false;
            var sOrder = new StopOrder()
            {
                Sec = Securities,
                Volume = percentPosition,
                Comment = Define.STOP_CONTROL,
                ClientCode = ClientCode.Value,
                DateExpiry = DateMarket.ExtractDateTime(DateTime.Now.AddDays(1))
            };
            if (isBuy)
            {
                Price = Price - numericUpDownMCTakeProfitTiks.Value * Securities.Params.MinPriceStep;
                sOrder.Price = Price;
                sOrder.Direction = OrderDirection.Buy;
                if (Securities.LastPrice > Price)
                {
                    sOrder.ConditionPrice = Price + Securities.Params.MinPriceStep;
                    sOrder.Offset = Securities.Params.MinPriceStep;
                    sOrder.Spread = Securities.Params.MinPriceStep;
                    return sOrder;
                }
            }
            else
            {
                sOrder.Direction = OrderDirection.Sell;
                Price = Price + numericUpDownMCTakeProfitTiks.Value * Securities.Params.MinPriceStep;
                sOrder.Price = Price;
                if (Securities.LastPrice < Price)
                {
                    sOrder.ConditionPrice = Price - Securities.Params.MinPriceStep;
                    sOrder.Offset = Securities.Params.MinPriceStep;
                    sOrder.Spread = Securities.Params.MinPriceStep;
                    return sOrder;
                }
            }
            return null;
        }

        /// <summary>
        /// Проверка текущих выставленых контролирующих оредов.
        /// </summary>
        /// <param name="orders"></param>
        /// <returns>true - если ореда корректные, false - надо пересчитать</returns>
        private bool CheckStopOrdersForControls(IEnumerable<StopOrder> orders)
        {

            var expectCountOrder = 0;
            if (orders.Count() > 0)
            {
                if (checkBoxMCStopLoss.Checked)
                {
                    expectCountOrder++;
                }
                if (checkBoxMCTakeProfit.Checked)
                {
                    expectCountOrder++;
                }
                if (orders.Count() != expectCountOrder)
                {
                    return false;
                }
            }
            /*bool takeExists = false;
            bool stopExists = false;
            foreach (var ord in orders)
            {
                if (ord.Status == OrderStatus.ACTIVE && ord.TypeStopOrder == StopOrderType.TakeProfit)
                {
                    takeExists = true;
                }
                if (ord.Status == OrderStatus.ACTIVE && ord.TypeStopOrder == StopOrderType.StopLimit)
                {
                    stopExists = true;
                }
            }
            if (stopExists && takeExists)
            {
                return true;
            }*/
            return true;
        }

        private MyTrade GetLastMyTradeForControl(int position)
        {
            if (position != 0)
            {
                return Quik.Trader.Objects.tMyTrades.ToArray().LastOrDefault(t => t.Trade.Sec == Securities);
            }
            /*if (position > 0)
            {
                return Quik.Trader.Objects.MyTrades.LastOrDefault(t => t.Trade.Sec == Securities && t.Trade.Direction == OrderDirection.Buy);
            }
            else if (position < 0)
            {
                return Quik.Trader.Objects.MyTrades.LastOrDefault(t => t.Trade.Sec == Securities && t.Trade.Direction == OrderDirection.Sell);
            }*/
            return null;
        }
        private IEnumerable<StopOrder> GetStopOrdersForControl()
        {
            return Quik.Trader.Objects.tStopOrders.SearchAll(o => o.Sec == Securities
                && o.IsActive() && o.Comment.Contains(Define.STOP_CONTROL)
            );
        }

        private Position GetPositionForControl()
        {
            return Quik.Trader.Objects.tPositions.SearchFirst(p => p.Sec == Securities);
        }
        /// <summary>
        /// Получение процент позиции, от общей позиции
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="allPosition"></param>
        /// <returns></returns>
        private int getCurrectPosByPercent(Position pos, bool allPosition = false)
        {
            if (pos.NotIsNull())
            {
                if (pos.CurrentVolume > 0)
                {
                    var modPos = pos.CurrentVolume;
                    if (allPosition)
                    {
                        return modPos;
                    }
                    var percent = GetPercentOutPosition();
                    if (percent > 0)
                    {
                        double calculPos = (double)modPos * (double)percent / 100;
                        var result = (int)(Math.Round(calculPos));
                        return result == 0 ? 1 : result;
                    }
                    else
                    {
                        return modPos;
                    }
                }
            }
            return 0;
        }

        private RadioButton GetRadioButtonByPercent(int percent)
        {
            var obj = this.Controls.Find("radioButtonPercentTake" + percent, true)
                .FirstOrDefault(o => o.Name == "radioButtonPercentTake" + percent);
            if (obj is RadioButton)
            {
                return (RadioButton)obj;
            }
            return null;
        }
        /// <summary>
        /// Получить процент выхода из позиции
        /// </summary>
        /// <returns></returns>
        private int GetPercentOutPosition()
        {
            foreach (var percent in PERCENT)
            {
                var objRadio = GetRadioButtonByPercent(percent);
                if (objRadio.NotIsNull() && objRadio.Checked)
                {
                    return percent;
                }
            }
            return 100;
        }

    }
}
