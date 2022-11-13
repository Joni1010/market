using MarketObjects;
using System.Collections.Generic;
using System.Linq;

namespace AppVEConector.Components
{
    class Searcher
    {
        /// <summary>
        /// Ищет удовлетворяющий список
        /// </summary>
        /// <param name="list"></param>
        /// <param name="contents"></param>
        /// <returns></returns>
        public static IEnumerable<Securities> Stock(IEnumerable<Securities> list, string contents)
        {
            return list.Where(
                el => el.Code.ToLower().Contains(contents.ToLower())
                || el.Name.ToLower().Contains(contents.ToLower())
                || el.ToString().ToLower() == contents.ToLower()
                ).ToArray();
        }
        /// <summary>
        /// Первый попавшийся
        /// </summary>
        /// <param name="list"></param>
        /// <param name="contents"></param>
        /// <returns></returns>
        public static Securities StockFirst(IEnumerable<Securities> list, string contents)
        {
            return Stock(list, contents)
                .FirstOrDefault(s => s.ToString().ToLower() == contents.ToLower());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="contents"></param>
        /// <returns></returns>
        public static string[] StockAsArray(IEnumerable<Securities> list, string contents)
        {
            return Stock(list, contents).Select(s => s.ToString()).ToArray();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="codeAndClass"></param>
        /// <returns></returns>
        public static Securities StockByCode(IEnumerable<Securities> list, string codeAndClass)
        {
            return list.FirstOrDefault(el => el.ToString().ToLower() == codeAndClass.ToLower());
        }

    }
}
