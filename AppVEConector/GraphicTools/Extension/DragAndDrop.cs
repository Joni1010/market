
using System;
using System.Drawing;

namespace GraphicTools.Extension
{
    public class DragAndDrop
    {
        /// <summary> Первая активная свеча </summary>
        public Point? PointFirst = null;
        /// <summary> Вторая активная свеча </summary>
        public Point? PointSecond = null;
        /// <summary>
        /// Начало перемещения
        /// </summary>
        private bool isDrag = false;

        public delegate void moveMouse(Point? first, Point? second);

        public moveMouse OnDrag;
        public moveMouse OnDrop;

        public void startDrag(Point coord)
        {
            isDrag = true;
            PointFirst = coord;
        }
        public void endDrag()
        {
            isDrag = false;
            if (OnDrop.NotIsNull())
            {
                OnDrop(PointFirst, PointSecond);
            }
            PointFirst = null;
            PointSecond = null;
        }
        /// <summary>
        /// Статус перемещения мыши.
        /// true - перемещение не закончено, false - закончено
        /// </summary>
        /// <returns></returns>
        public bool statusDrag()
        {
            return isDrag;
        }

        public void Check(Point coord)
        {
            PointSecond = coord;
            if (statusDrag())
            {
                if (OnDrag.NotIsNull())
                {
                    OnDrag(PointFirst, PointSecond);
                }
            }
        }
    }
}
