using Libs;
using MarketObjects;
using QuikConnector.Components.Controllers;
using QuikConnector.Components.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Market.Candles
{
    /// <summary>  Класс коллекции данных по свечекам, за определнный тайм-фрейм. </summary>
    public class CandleCollection11
    {
        [Serializable]
        private class KeepCollection
        {
            /// <summary>  Набор значений готовых свечей для отрисовки </summary>
            public List<CandleData> Collection = new List<CandleData>();
        }

        private KeepCollection ObjCollection = new KeepCollection();

        private readonly object syncLock = new object();
        /// <summary> Текущий тайм-фрейм </summary>
        public int TimeFrame = 1;

        public delegate void EventCandle(int timeframe, CandleData candle);
        /// <summary> Событие появления новой свечки </summary>
        public event EventCandle OnNewCandle;
        /// <summary> Событие появления новой исторической свечи </summary>
        //public event EventCandle OnNewOldCandle;

        /// <summary>
        /// Возвращает коллекцию
        /// </summary>
        private List<CandleData> Collection
        {
            get
            {
                return this.ObjCollection.Collection;
            }
        }

        public delegate void DeleteExtra(CandleData candle);
        /// <summary> Событие удаления избыточной свечки</summary>
        public event DeleteExtra OnDeleteExtra;

        /// <summary> Кол-во хранимых свечек в каждом тайм-фрейме </summary>
        public int CountKeepCandle = 1000;
        /// <summary> Время последнего сохранения </summary>
        public DateTime TimeLastSave = DateTime.Now;
        /// <summary> Время последнего изменения коллекции </summary>
        public DateTime TimeLastUpdateCollection = DateTime.Now;
        /// <summary> Флаг определяющий, была ли загружена история.</summary>
        //public bool WasReadHistory = false;

        /// <summary> Флаг определяющий контроль за сделками, для избежания дубликатов </summary>
        //public bool ControlTrades = false;

        /// <summary> Запись коллекции в сериализованном виде. </summary>
        /// <param name="filename"></param>
        public bool WriteCollectionInFile(string filename)
        {
            ThreadsController.Thread(() =>
            {
                object obj = null;
                lock (syncLock)
                {
                    obj = this.ObjCollection.Collection.Clone();
                }
                this.TimeLastSave = DateTime.Now;
                Stream stream = File.Open(filename, FileMode.Create);
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, obj);
                stream.Close();
            });
            return true;
        }

        /// <summary> Чтение коллекции из файла в сериализованном виде. </summary>
        /// <param name="filename"></param>
        public void ReadCollectionFromFile(string filename)
        {
            QLog.CatchException(() =>
            {
                this.TimeLastSave = DateTime.Now;
                WFile file = new WFile(filename);
                if (!file.Exists()) return;
                //this.WasReadHistory = true;
                Stream stream = File.Open(filename, FileMode.Open);
                stream.Position = 0;
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                lock (syncLock)
                {
                    this.ObjCollection.Collection = (List<CandleData>)binaryFormatter.Deserialize(stream);
                }
                stream.Close();

                /*	using (Stream stream = File.Open(filename, FileMode.Open))
				{
					var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
					this.Collection = (List<CandleData>)binaryFormatter.Deserialize(stream);
				}*/
            }, filename);
        }


        /// <summary> Конструктор </summary>
        /// <param name="timeFrame">Кол-во минут</param>
        public CandleCollection11(int timeFrame)
        {
            this.TimeFrame = timeFrame;
        }

        /// <summary>  Получить коллекцию данных по свечкам. </summary>
        public List<CandleData> MainCollection
        {
            get
            {
                lock (syncLock)
                {
                    return this.Collection;
                }
            }
        }
        public CandleData[] CollectionArray
        {
            get
            {
                lock (syncLock)
                {
                    return this.Collection.ToArray();
                }
            }
        }
        /// <summary> Перебор коллекции </summary>
        /// <param name="eachAction"></param>
        public void ForEach(Action<CandleData> eachAction)
        {
            if (this.Count == 0) return;
            if (eachAction.IsNull()) return;
            foreach (var el in this.MainCollection) { eachAction(el); }
        }

        /// <summary> Возвращает кол-во свечек в текущем тайм-фрейме  </summary>
        public int Count
        {
            get
            {
                if (this.Collection.Count == 0) return 0;
                lock (syncLock)
                {
                    return this.Collection.Count;
                }

            }
        }

        /// <summary>  Получить первую свечку по порядку. [0] элемент, если пустая то возвращается null. </summary>
        public CandleData FirstCandle
        {
            get
            {

                if (this.Count > 0)
                {
                    lock (syncLock)
                    {
                        return this.GetElement(0);
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Получить последнюю свечку в колеккции. MAX-индекс в коллекциит, если пустая то возвращается null.
        /// </summary>
        public CandleData LastCandle
        {
            get
            {
                lock (syncLock)
                {
                    return this.GetElement(this.Count - 1);
                }
            }
        }

        /// <summary> Возвращает i-ый элемент в коллекции.  </summary>
        /// <param name="i">Индекс в коллекции</param>
        /// <returns></returns>
        public CandleData GetElement(int i)
        {
            lock (syncLock)
            {
                return this.Collection.Count > 0 ? this.Collection.ElementAt(i) : null;
            }
        }
        /// <summary> Получает i-элемент с конца</summary>
        /// <param name="i"></param>
        /// <returns></returns>
        /*public CandleData GetElementFromEnd(int i)
        {
            lock (syncLock)
            {
                i = this.Collection.Count - i - 1;
                if (i < 0)
                {
                    return null;
                }
                return this.Collection.Count > 0 ? this.Collection.ElementAt(i) : null;
            }
        }*/

        //**************************************************************

        /// <summary> Добавление "первой" свечки в коллекцию, в [0] по индексу.  </summary>
        /// <param name="candle"></param>
        public void InsertFirst(CandleData candle)
        {
            if (candle.IsNull()) return;
            lock (syncLock)
            {
                this.Collection.Insert(0, candle);
            }
        }

        /// <summary> Создает новую свечку и добавляем вначало [0] коллекции. </summary>
        /// <param name="time">Расчитанное граничное время свечки.</param>
        private void AddNewCandle(DateTime time)
        {
            this.InsertFirst(new CandleData(time));
            this.TimeLastUpdateCollection = DateTime.Now;
        }

        /// <summary> Удаление списка свечей </summary>
        /// <param name="listCandles"></param>
        public void RemoveCandles(IEnumerable<CandleData> listCandles)
        {
            if (listCandles.NotIsNull())
            {
                if (listCandles.Count() > 0)
                {
                    foreach (var can in listCandles)
                    {
                        this.RemoveCandle(can);
                    }
                    this.TimeLastUpdateCollection = DateTime.Now;
                }
            }
        }
        /// <summary> Удаление свечи </summary>
        /// <param name="candle">Объект свечи</param>
        /// <returns>true - если свеча удалена, иначе false. </returns>
        public bool RemoveCandle(CandleData candle)
        {
            if (candle.IsNull()) return false;
            lock (syncLock)
            {
                //candle.ClearKeepTrades();
                var res = this.Collection.Remove(candle);
                this.TimeLastUpdateCollection = DateTime.Now;
                return res;
            }
        }

        /*private CandleData lastExistsCandle = null;
        /// <summary> Проверка наличия данной сделки в тайм фрейме. </summary>
        /// <param name="trade"></param>
        /// <returns></returns>
        public bool ExistTradeInTF(Trade trade)
        {
            DateTime time = CandleData.GetTimeCandle(trade.DateTrade.GetDateTime(), this.TimeFrame);
            //Очистка хранимых сделок за прошлое. (в них нет необходимости)
            lock (syncLock)
            {
                if (lastExistsCandle.NotIsNull())
                {
                    if (lastExistsCandle.Time == time)
                    {
                        return lastExistsCandle.ExistsTrade(trade);
                    }
                }
                //Проверка на наличие сделки
                var candle = this.Collection.FirstOrDefault(c => c.Time == time);
                var res = false;
                if (candle.NotIsNull())
                {
                    res = candle.ExistsTrade(trade);
                    lastExistsCandle = candle;
                }
                return res;
            }
        }*/
        /// <summary>
        /// Очистка хранимых сделок. (от дубликатов)
        /// </summary>
        /// <param name="candle"></param>
        /*public void ClearHistoryTrade(CandleData candle)
        {
            lock (syncLock)
            {
                
                var candlesClear = this.Collection.Skip(PERIOD_AFTER_CLEAR_TRADE).Where(c => c.CountKeepTrades > 0 && c._lastUpdate < DateTime.Now.AddDays(-2));
                if (candlesClear.NotIsNull())
                {
                    foreach (var c in candlesClear)
                    {
                        c.ClearKeepTrades();
                    }
                }
                
            }
        }*/

        private CandleData LastFindCandle = null;
        /// <summary> Добавить новую сделку в свечку с соответствущим временем. </summary>
        /// <param name="trade">Сделка</param>
        /// <param name="history"> Флаг загрузки исторических сделок </param>
        public bool AddNewTrade(Trade trade, bool history = false)
        {
            if (trade.IsNull()) return false;
            DateTime time = CandleData.GetTimeCandle(trade.DateTrade.GetDateTime(), this.TimeFrame);
            if (this.Count > 0)
            {
                lock (syncLock)
                {
                    if (LastFindCandle.NotIsNull())
                    {
                        if (LastFindCandle.Time == time)
                        {
                            LastFindCandle.NewTrade(trade);
                            TimeLastUpdateCollection = DateTime.Now;
                            return true;
                        }
                    }
                    LastFindCandle = this.Collection.FirstOrDefault(c => c.Time == time);
                    if (LastFindCandle.NotIsNull())
                    {
                        LastFindCandle.NewTrade(trade);
                        TimeLastUpdateCollection = DateTime.Now;
                        return true;
                    }
                    else
                    {
                        //свеча отсутствует
                        this.AddNewCandle(time);
                        LastFindCandle = this.FirstCandle;
                        LastFindCandle.NewTrade(trade);

                        //Сортируем по времени
                        this.ObjCollection.Collection = this.Collection.OrderByDescending(c => c.Time).ToList();

                        if (!OnNewCandle.IsNull() && !history)
                            OnNewCandle(this.TimeFrame, LastFindCandle);

                        //Удаляем свечки c конца, которые выше допустимого кол-ва хранения
                        if (this.Collection.Count > this.CountKeepCandle)
                        {
                            if (this.OnDeleteExtra != null)
                                OnDeleteExtra(this.LastCandle);
                            this.Collection.Remove(this.LastCandle);
                        }
                    }
                }
            }
            else
            {
                lock (syncLock)
                {
                    //Добавляем первую свечку в коллекцию
                    this.AddNewCandle(time);
                    this.FirstCandle.NewTrade(trade);

                    if (!OnNewCandle.IsNull() && !history)
                        OnNewCandle(this.TimeFrame, this.FirstCandle);
                }
            }
            return true;
        }
    }
}
