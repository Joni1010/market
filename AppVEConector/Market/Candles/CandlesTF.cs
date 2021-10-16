using Market.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Candles
{
    public class CandlesTF : ElementTF<CandlesBlock, CandleData>
    {
        const string POSTFIX_FILE_DUMP = "_charts.dat";

        private bool KeepDay = true;
        private bool KeepMonth = false;

        public event Action<int> OnSave = null;

        public override CandleData[] Collection
        {
            get
            {
                lock (syncLock)
                {
                    var countCandlesInBlock = Blocks.Sum(b => b.Count);
                    if (MergeCollection.IsNull() || countCandlesInBlock != MergeCollection.Length)
                    {
                        updateMergeCollection();
                    }
                    if (MergeCollection.NotIsNull())
                    {
                        return MergeCollection.ToArray();
                    }
                }
                return null;
            }
        }

        /// <summary>  Получить первую свечку по порядку. [0] элемент, если пустая то возвращается null. </summary>
        public CandleData FirstCandle
        {
            get
            {
                if (Count > 0)
                {
                    lock (syncLock)
                    {
                        var col = Collection;
                        if (col.Length > 0)
                        {
                            return Collection[0];
                        }
                        return null;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Получить последнюю свечку в колеккции. MAX-индекс в коллекциит, если пустая то возвращается null.
        /// </summary>
        /*public CandleData LastCandle
        {
            get
            {
                lock (syncLock)
                {
                    var col = Collection;
                    if (col.Length > 0)
                    {
                        return Collection[col.Length - 1];
                    }
                    return null;
                }
            }
        }*/

        /// <summary> Конструктор </summary>
        /// <param name="timeFrame">Кол-во минут</param>
        public CandlesTF(int period, bool keepDay, string dirKeep)
        {
            lock (syncLock)
            {
                periodTimeFrame = period;
                KeepDay = keepDay;
                KeepMonth = !keepDay;
                dirKeepHistory = dirKeep;
            }
            updateHistoryStruct(POSTFIX_FILE_DUMP);
        }
        /// <summary>
        /// Удаляет свечу
        /// </summary>
        /// <param name="datetime"></param>
        public void Delete(DateTime datetime)
        {
            base.Delete(datetime, (block, candle) =>
            {
                if (block.DeleteElement(candle))
                {
                    lastUseBlock = null;
                }
            });
        }

        /// <summary>
        /// Ищет нужный блок свечек по времени
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        protected override CandlesBlock getBlock(DateTime time)
        {
            lock (syncLock)
            {
                BlockTime timeBlock;
                if (KeepDay)
                {
                    timeBlock = BlockTime.ConvertForDay(time);
                }
                else
                {
                    timeBlock = BlockTime.ConvertForMonth(time);
                }
                if (lastUseBlock.NotIsNull())
                {
                    if (lastUseBlock.IdTime == timeBlock)
                    {
                        return lastUseBlock;
                    }
                    lastUseBlock = null;
                }
                if (Blocks.Count > 0)
                {
                    var block = Blocks.FirstOrDefault(b => b.IdTime == timeBlock);
                    if (block.NotIsNull() && block.IdTime == timeBlock)
                    {
                        lastUseBlock = block;
                        return block;
                    }
                }
                if (lastUseBlock.IsNull())
                {
                    var newBlock = loadByIdTime(timeBlock, POSTFIX_FILE_DUMP);
                    if (newBlock.IsNull())
                    {
                        newBlock = new CandlesBlock(periodTimeFrame, timeBlock);
                    }
                    Blocks.Add(newBlock);
                    lastUseBlock = newBlock;
                }
                return lastUseBlock;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void BeforeNewCandle()
        {
            lock (syncLock)
            {
                updateMergeCollection();
            }
        }
        /// <summary>
        /// Возвращает дату начала хранения
        /// </summary>
        /// <returns></returns>
        private BlockTime getDateBeginKeep()
        {
            var date = DateTime.Now.AddDays(daysPeriodKeep * -1);
            return KeepDay ? BlockTime.ConvertForDay(date) : BlockTime.ConvertForMonth(date);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool Save()
        {
            lock (syncLock)
            {
                if (Blocks.Count > 0)
                {
                    var dateBeginKeep = getDateBeginKeep();
                    var blocksSave = Blocks.Where(b => b.IdTime.Index >= dateBeginKeep.Index);
                    if (blocksSave.Count() > 0)
                    {
                        foreach (var block in blocksSave.ToArray())
                        {
                            var filename = getFileNameDump(block.IdTime, POSTFIX_FILE_DUMP);
                            block.Save(filename);
                            if (OnSave.NotIsNull())
                            {
                                OnSave(periodTimeFrame);
                            }
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Load()
        {
            updateHistoryStruct(POSTFIX_FILE_DUMP);
            lock (syncLock)
            {
                foreach (var itemHist in HistoryStruct)
                {
                    var filename = itemHist.Filename;// getFileNameDump(itemHist.Filename);
                    if (File.Exists(filename))
                    {
                        CandlesBlock block = null;
                        bool isLoad = false;

                        block = CandlesBlock.Load(filename);
                        if (block.NotIsNull())
                        {
                            var existBlock = Blocks.FirstOrDefault(b => b.IdTime == block.IdTime);
                            if (existBlock.IsNull())
                            {
                                Blocks.Add(block);
                                isLoad = true;
                            }
                        }
                        if (isLoad)
                        {
                            updateMergeCollection();
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Возвращает свечку по времени
        /// </summary>
        /// <param name="time"></param>
        /// <param name="addNew"></param>
        /// <returns></returns>
        public override CandleData GetCandle(DateTime time, bool addNew = false)
        {
            var block = getBlock(time);
            lock (syncLock)
            {
                if (block.NotIsNull())
                {
                    var candle = block.GetElement(time, addNew);
                    if (candle.NotIsNull())
                    {
                        return candle;
                    }
                }
                return null;
            }
        }
        /// <summary>
        /// Обновляет коллекцию обьединенных элементов блока
        /// </summary>
        private void updateMergeCollection()
        {
            MergeCollection = null;
            Blocks = Blocks.OrderByDescending(b => b.IdTime.Index).ToList();
            foreach (var block in Blocks)
            {
                var col = block.GetAllCollection();
                if (col.Length > 0)
                {
                    if (MergeCollection.NotIsNull())
                    {
                        MergeCollection = MergeCollection.Union(col).ToArray();
                    }
                    else
                    {
                        MergeCollection = col;
                    }
                }
            }
        }
    }
}
