﻿using System.Collections.Generic;
using MarketObjects.Charts;

namespace GraphicTools.Extension
{
    ////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Базовый класс для уровней
    /// </summary>
    public class BaseLevels
    {
        /// <summary> Коллекция для отрисовки уровней </summary>
        public List<Chart> CollectionLevels = new List<Chart>();
        /// <summary> Минимальное значение </summary>
        public decimal Min = 100000000;
        /// <summary> Максимальное значение </summary>
        public decimal Max = -100000000;
        /// <summary>
        /// Сброс значений мин макс
        /// </summary>
        public void ResetMinMax()
        {
            Min = 100000000;
            Max = -100000000;
        }
    }
}
