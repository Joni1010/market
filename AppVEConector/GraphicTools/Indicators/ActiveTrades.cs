using GraphicTools.Base;
using GraphicTools.Shapes;
using Market.Candles;
using MarketObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GraphicTools.GCandles;

namespace AppVEConector.GraphicTools.Indicators
{
    public class ActiveTrades : Indicator
    {
        public int MinVolumeShow = 0;
        /// <summary>
        /// Список сделок
        /// </summary>
        public IEnumerable<Trade> ListTrades = null;

        public ActiveTrades(ViewPanel mainPanel, bool enable = true) :
            base(mainPanel)
        {
            FastRedraw = true;
            Enable = enable;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool IsEnable()
        {
            if (Enable && MinVolumeShow > 0)
            {
                return true;
            }
            return false;
        }

        public override void EventInitStartIndicator()
        {

        }

        public override void FastUpdate()
        {
            Panel.Clear();
            if (!IsEnable())
            {
                return;
            }
            if (ListTrades.NotIsNull() && ListTrades.Count() > 0)
            {
                var canvas = Panel.GetGraphics;
                //Устанавливаем первую уоординату + смещение
                float XlastTrade = Panel.Rect.Width - 40;
                var textVol = new TextDraw();
                textVol.SetFontSize(6);
                textVol.Color = Color.Black;

                foreach (var trade in ListTrades)
                {
                    int width = 4;
                    int Height = 4;
                    SizeF sizeText;
                    float xPlusText = 0;
                    float yPlusText = 0;
                    if (MinVolumeShow <= trade.Volume)
                    {                      
                        sizeText = textVol.GetSizeText(canvas, trade.Volume.ToString());
                        width = Height = (int)(sizeText.Width * 2);
                        xPlusText = (width - sizeText.Width) / 2;
                        yPlusText = (Height - sizeText.Height) / 2;
                    }
                    XlastTrade = XlastTrade - width;

                    float yPrice = (float)GetYByPrice(trade.Price);
                    var rect = new RectDraw();
                    var colorCandle = trade.Direction == OrderDirection.Buy ? Color.LightGreen : Color.LightCoral;
                    var colorBorder = Color.Black;
                    rect.Paint(canvas, XlastTrade, yPrice - Height / 2, width, Height, colorBorder, colorCandle);

                    if (MinVolumeShow <= trade.Volume)
                    {                        
                        textVol.Paint(canvas, trade.Volume.ToString(), 
                            XlastTrade + xPlusText, (yPrice - Height / 2) + yPlusText);
                    }                   
                }
            }
        }

        public override void EventInitEndIndicator()
        {

        }
        /// <summary>
        /// Обработка по каждой свечке в текущем тайм-фрейме
        /// </summary>
        /// <param name="candle"></param>
        public override void EachCandle(int index, CandleData can, int count)
        {

        }

        public override void EachFullCandle(CandleInfo candle)
        {

        }
        public override void NewCandleInTimeFrame(int timeFrame, CandleData candle)
        {
        }
    }
}
