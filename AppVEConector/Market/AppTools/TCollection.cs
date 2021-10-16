using MarketObjects;
using System.Collections.Generic;
using System.Linq;

namespace Market.AppTools
{
    /// <summary> Коллекция активных торгуемых инструментов </summary>
    public class TCollection
    {
        /// <summary>
        /// Collection all trade elements
        /// </summary>
        private List<TElement> _Collection = new List<TElement>();
        /// <summary>
        /// Locker
        /// </summary>
        private readonly object syncLock = new object();

        /// <summary>
        /// Последний найденный инструмент
        /// </summary>
        private TElement lastFoundElem = null;
        public TElement[] Collection
        {
            get
            {
                lock (syncLock)
                {
                    return this._Collection.ToArray();
                }
            }
        }
        /// <summary> Получает торговый элемент</summary>
        /// <param name="sec"></param>
        /// <returns></returns>
        public TElement GetTElement(Securities sec)
        {
            lock (syncLock)
            {
                return this._Collection.FirstOrDefault(t => t.Security == sec);
            }
        }
        /// <summary> Добавляет торговый элемент в коллекцию по указанному инструменту.
        /// Если элемент существует в коллекции, то он возвращается.</summary>
        /// <param name="newElem"></param>
        public TElement AddOrFind(Securities sec)
        {
            if (sec.IsNull())
            {
                return null;
            }
            lock (syncLock)
            {
                if (lastFoundElem.NotIsNull() && lastFoundElem.Security == sec)
                {
                    return lastFoundElem;
                }
                var el = this._Collection.FirstOrDefault(t => t.Security == sec);
                if (el.IsNull())
                {
                    el = new TElement(sec);
                    this._Collection.Add(el);
                    el.Create();
                }
                lastFoundElem = el;
                return el;
            }
        }
    }
}
