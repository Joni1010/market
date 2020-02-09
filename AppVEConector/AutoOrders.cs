using Connector.Logs;
using Libs;
using MarketObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppVEConector
{
    public class AutoOrders
    {
        const string FILENAME = "auto_orders.dat";
        private string FullFileName = "";
        private readonly object objSync = new object();

        public AutoOrders(string pathSaveFile)
        {
            FullFileName = pathSaveFile + FILENAME;
        }
        /// <summary>
        /// 
        /// </summary>
        public enum CondAutoOrder
        {
            MoreOrEquals = 2,
            LessOrEquals = 4,
            Equals = 5
        }
        [Serializable]
        public class ConditionOrder : Order
        {
            public decimal PriceCondition;
            public CondAutoOrder CondAutoOrder;

            public string SecAndCode;
            public string SecName;
        }
        public delegate void EventAutoOrder(ConditionOrder order);
        public EventAutoOrder OnAdd;
        public EventAutoOrder OnDelete;
        /// <summary>
        /// Список заявок на авто срабатывание
        /// </summary>
        private List<ConditionOrder> ListOrders = new List<ConditionOrder>();

        /// <summary>
        /// Массив всех авто-ордеров
        /// </summary>
        public ConditionOrder[] ToArray
        {
            get
            {
                lock (objSync)
                {
                    return ListOrders.ToArray();
                }
            }
        }
        /// <summary>
        /// Добавить авто-ордер
        /// </summary>
        /// <param name="order"></param>
        public void Add(ConditionOrder order)
        {
            lock (objSync)
            {
                ListOrders.Add(order);
            }
            Save();
            if (OnAdd.NotIsNull())
            {
                OnAdd(order);
            }
        }
        /// <summary>
        /// Удалить указанный ордер
        /// </summary>
        /// <param name="order"></param>
        public void Delete(ConditionOrder order)
        {
            var foundOrd = ListOrders.FirstOrDefault(o => o == order);
            if (foundOrd.NotIsNull())
            {
                lock (objSync)
                {
                    ListOrders.Remove(foundOrd);
                }
                Save();
                if (OnDelete.NotIsNull())
                {
                    OnDelete(foundOrd);
                }
            }
        }
        /// <summary>
        /// Удалить все ордера по инструменту
        /// </summary>
        /// <param name="sec"></param>
        public void Delete(Securities sec)
        {
            var foundOrd = ListOrders.Where(o => o.SecAndCode == sec.ToString()).ToArray();
            if (foundOrd.NotIsNull() && foundOrd.Count() > 0)
            {
                foreach (var delOrd in foundOrd)
                {
                    Delete(delOrd);
                }
            }
        }
        /// <summary>
        /// Сохраняет коллекцию ордеров
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private bool Save()
        {
            object obj = null;
            lock (objSync)
            {
                obj = ListOrders.Clone();
            }
            Stream stream = File.Open(FullFileName, FileMode.Create);
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            binaryFormatter.Serialize(stream, obj);
            stream.Close();
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool Load()
        {
            try { 
                WFile file = new WFile(FullFileName);
                if (!file.Exists())
                {
                    return false;
                }
                Stream stream = File.Open(FullFileName, FileMode.Open);
                stream.Position = 0;
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                lock (objSync)
                {
                    ListOrders = (List<ConditionOrder>)binaryFormatter.Deserialize(stream);
                }
                stream.Close();
                return true;
            }
            catch (Exception e)
            {
                Qlog.Write("Ошибка при получении данный из файла FullFileName." + e.ToString());
                return false;
            }
        }
    }
}
