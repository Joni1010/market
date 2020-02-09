
namespace GraphicTools.Extension
{
    public class PeriodActCandles
    {
        /// <summary> Первая активная свеча </summary>
        public SelectCandle ActiveCandle1 = null;
        /// <summary> Вторая активная свеча </summary>
        public SelectCandle ActiveCandle2 = null;
        /// <summary>
        /// Начало перемещения
        /// </summary>
        private bool startMove = false;

        public void startSel()
        {
            startMove = true;
        }
        public void endSel()
        {
            startMove = false;
        }
        public bool statusSel()
        {
            return startMove;
        }
    }
}
