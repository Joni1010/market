using System.Collections.Generic;
using System.Linq;
using MarketObjects;

namespace AppVEConector.Components
{
    public class ActiveStocks
    {
        private readonly List<Securities> list = new List<Securities>();
        public void Add(Securities sec)
        {
            if (!Exists(sec))
            {
                list.Add(sec);
            }
        }

        public bool Remove(Securities sec)
        {
            return list.Remove(sec);
        }

        public bool Exists(Securities sec)
        {
            return Find(sec) != null;
        }

        public Securities Find(string code, string classCode)
        {
            return list.FirstOrDefault(s => s.Code == code && s.Class.Code == classCode);
        }

        public Securities Find(Securities sec)
        {
            return list.FirstOrDefault(s => s.Code == sec.Code && s == sec);
        }
        /// <summary>
        /// Ввиде списка строк CODE:CLASS
        /// </summary>
        /// <returns></returns>
        public string[] ToArray()
        {
            return list.Select(s => s.ToString()).ToArray();
        }
        /// <summary>
        /// Поиск списка
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public Securities[] Search(string content)
        {
            return Searcher.Stock(list, content).ToArray();
        }

        public Securities SearchFirst(string content)
        {
            return Searcher.StockFirst(list, content);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="codeAndClass"></param>
        /// <returns></returns>
        public Securities SearchByCode(string codeAndClass)
        {
            return Searcher.StockByCode(list, codeAndClass);
        }

        public void Clear()
        {
            list.Clear();
        }
    }
}
