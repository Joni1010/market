using AppVEConector.libs.Signal;
using MarketObjects;
using System;
using System.Windows.Forms;

namespace AppVEConector
{
    public partial class Form_GraphicDepth : Form
    {

        public delegate void eventTypeHorVol(ToolStripMenuItem elementMenu, int type);
        /// <summary>
        /// Событие выбора типа горизонтального обьема
        /// </summary>
        public eventTypeHorVol OnSelectTypeHorVol;
        private void ContextMenuGraphic_Init()
        {
            this.pictureBoxGraphic.ContextMenuStrip = this.contextMenuGraphic;
            cmgSetSignal.Click += ContextMenuGraphic_cmgSetSignal_Click;
            toolStripGraphicZoomInc.Click += ContextMenuGraphic_toolStripGraphicZoomInc_Click;
            toolStripGraphicZoomDec.Click += ContextMenuGraphic_toolStripGraphicZoomDec_Click;
            toolStripGraphicOrder.Click += ContextMenuGraphic_toolStripGraphicOrder_Click;
            toolStripGraphicStopOrder.Click += ContextMenuGraphic_toolStripGraphicStopOrder_Click;

            //Init context menu
            foreach (ToolStripMenuItem childMenu in MenuItemHorVol.DropDownItems)
            {
                childMenu.Click += (s, e) =>
                {
                    var elem = (ToolStripMenuItem)s;
                    ContextMenuGraphic_UncheckedMenuItemsHorVol();
                    elem.Checked = true;
                    if (OnSelectTypeHorVol.NotIsNull())
                    {
                        OnSelectTypeHorVol(elem, elem.Tag.ToString().ToInt32());
                    }
                };
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void ContextMenuGraphic_UncheckedMenuItemsHorVol()
        {
            foreach (ToolStripMenuItem childMenu in MenuItemHorVol.DropDownItems)
            {
                childMenu.Checked = false;
            }
        }
        /// <summary>
        /// Получить выбранный тип гор. обьема
        /// </summary>
        /// <returns></returns>
        private int ContextMenuGraphic_GetCheckedMenuItemsHorVol()
        {
            foreach (ToolStripMenuItem childMenu in MenuItemHorVol.DropDownItems)
            {
                if (childMenu.Checked)
                {
                    return childMenu.Tag.ToString().ToInt32();
                }
            }
            return 0;
        }

        private void ContextMenuGraphic_toolStripGraphicZoomInc_Click(object s, EventArgs e)
        {
            var obj = new Control();
            obj.Tag = 50;
            this.buttonInc_Click(obj, e);
        }
        private void ContextMenuGraphic_toolStripGraphicZoomDec_Click(object s, EventArgs e)
        {
            var obj = new Control();
            obj.Tag = 50;
            this.buttonDec_Click(obj, e);
        }

        private void ContextMenuGraphic_toolStripGraphicOrder_Click(object s, EventArgs e)
        {
            var cross = this.GraphicStock.GetDataCross();
            var cond = OrderDirection.Sell;
            if (cross.Price < Securities.LastPrice)
            {
                cond = OrderDirection.Buy;
            }
            var order = new Order();
            order.Sec = Securities;
            order.Price = cross.Price;
            order.Volume = (int)this.numericUpDownVolume.Value;
            order.Direction = cond;
            this.Parent.Trader.CreateOrder(order);
        }

        private void ContextMenuGraphic_toolStripGraphicStopOrder_Click(object s, EventArgs e)
        {
            var cross = this.GraphicStock.GetDataCross();
            SetStopOrder(cross.Price, Position.Data.CurrentNet);
        }

        /// <summary>
        /// Установка сигнала
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void ContextMenuGraphic_cmgSetSignal_Click(object s, EventArgs e)
        {
            var cross = this.GraphicStock.GetDataCross();
            var cond = SignalMarket.CondSignal.MoreOrEquals;
            if (cross.Price < Securities.LastPrice)
            {
                cond = SignalMarket.CondSignal.LessOrEquals;
            }
            SignalView.GSMSignaler.AddSignal(new SignalMarket()
            {
                Type = SignalMarket.TypeSignal.ByPrice,
                SecClass = Securities.Code + ":" + Securities.Class.Code,
                Price = cross.Price,
                Condition = cond,
                Comment = "auto signal by graphic"
            });
            this.ShowTransReply("Signal set by price: " + cross.Price.ToString());
        }

        private void ToolStripMenuItemAutoSize_Click(object sender, EventArgs e)
        {
            ToolStripMenuItemAutoSize.Checked = !ToolStripMenuItemAutoSize.Checked;
            GraphicStock.AutoScaling(ToolStripMenuItemAutoSize.Checked);
            checkBoxAutoSizePrice.Checked = ToolStripMenuItemAutoSize.Checked;
        }
    }
}
