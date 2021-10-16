using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Base
{
    public class ElementTF<Block, T>
    {
        /// <summary>
        /// Директория храннения бинарных файлов истории
        /// </summary>
        protected string dirKeepHistory = "";
        /// <summary>
        /// Период тайм-фрейма
        /// </summary>
        protected int periodTimeFrame = 0;
        /// <summary>
        /// обьект мутекса
        /// </summary>
        protected readonly object syncLock = new object();
        /// <summary>
        /// делегат событий в тайм-фрейме
        /// </summary>
        /// <param name="timeframe"></param>
        /// <param name="element"></param>
        public delegate void eventElementTimeFrame(int timeframe, T element);
        /// <summary> Событие нового элемента в тайм-фрейме </summary>
        public event eventElementTimeFrame OnNew = null;
        /// <summary>
        /// Структура хранения полученных файлов истории в отсуртированному порядке.
        /// </summary>
        protected List<TimeFrame.History> HistoryStruct = new List<TimeFrame.History>();
        /// <summary>
        /// Блоки данных элементов
        /// </summary>
        protected List<Block> Blocks = new List<Block>();
        /// <summary>
        /// Последний используемый блок
        /// </summary>
        protected Block lastUseBlock = default;
        /// <summary>
        /// Коллекция обьединенных элементов из блоков
        /// </summary>
        protected T[] MergeCollection = null;
        /// <summary>
        /// Период хранения истории в днях
        /// </summary>
        protected int daysPeriodKeep = 10;
        /// <summary>
        /// Получить полную коллекцию элементов
        /// </summary>
        public virtual T[] Collection
        {
            get
            {
                lock (syncLock)
                {
                    if (MergeCollection.NotIsNull())
                    {
                        return MergeCollection.ToArray();
                    }
                    return null;
                }
            }
        }
        /// <summary>
        /// Кол-во элементов в коллекции
        /// </summary>
        public virtual int Count
        {
            get
            {
                lock (syncLock)
                {
                    var col = Collection;
                    return col.IsNull() ? 0 : col.Length;
                }
            }
        }

        /// <summary>
        /// Устанавливает период хранения
        /// </summary>
        /// <param name="periodKeep"></param>
        public void SetPeriodKeep(int periodKeep)
        {
            daysPeriodKeep = periodKeep;
        }
        /// <summary>
        /// Получить элемент по индексу
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public T GetElement(int i)
        {
            lock (syncLock)
            {
                var col = Collection;
                return col.Length > 0 && i < col.Length ? col[i] : default;
            }
        }
        /// <summary>
        /// Получить блок
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        protected virtual Block getBlock(DateTime time)
        {
            return default;
        }

        /// <summary>
        /// Удаление блоков и элементов
        /// </summary>
        /// <param name="datetime"></param>
        /// <param name="actionDelete"></param>
        protected virtual void Delete(DateTime datetime, Action<Block, T> actionDelete)
        {
            var candle = GetCandle(datetime);
            lock (syncLock)
            {
                if (candle.NotIsNull())
                {
                    foreach (var block in Blocks)
                    {
                        if (actionDelete.NotIsNull())
                        {
                            actionDelete(block, candle);
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
        public virtual T GetCandle(DateTime time, bool addNew = false)
        {
            return default;
        }
        /// <summary>
        /// Событие нового элемента в коллекции
        /// </summary>
        /// <param name="candle"></param>
        /// <param name="deforeAction"></param>
        /// <param name="afterAction"></param>
        public void EventNewElement(T candle, Action deforeAction = null, Action afterAction = null)
        {
            if (OnNew.NotIsNull())
            {
                if (deforeAction.NotIsNull())
                {
                    deforeAction();
                }
                OnNew(periodTimeFrame, candle);
                if (afterAction.NotIsNull())
                {
                    afterAction();
                }
            }
        }
        /// <summary>
        /// Возврщает название файла хранения блоков с элементами
        /// </summary>
        /// <param name="timeBlock"></param>
        /// <param name="postfix"></param>
        /// <returns></returns>
        protected string getFileNameDump(BlockTime timeBlock, string postfix)
        {
            return dirKeepHistory + timeBlock.ToString() + "_" + periodTimeFrame.ToString() + postfix;
        }
        /// <summary>
        /// Обновляем структуру файлов
        /// </summary>
        /// <param name="postfix"></param>
        protected void updateHistoryStruct(string postfix)
        {
            var files = Directory.GetFiles(dirKeepHistory, "*_" + periodTimeFrame + postfix);
            lock (syncLock)
            {
                HistoryStruct.Clear();
                foreach (var file in files)
                {
                    var onlyName = file.Replace(dirKeepHistory, "");
                    var list = onlyName.Split('_');
                    int period = list[1].ToInt32();
                    if (period == periodTimeFrame)
                    {
                        var idTime = new BlockTime();
                        idTime.Year = list[0].Substring(0, 4).ToInt32();
                        idTime.Month = list[0].Substring(4, 2).ToInt32();
                        idTime.Day = list[0].Substring(6).ToInt32();
                        HistoryStruct.Add(new TimeFrame.History() { Filename = file, TimeFrame = period, IdTime = idTime });
                    }
                }
                HistoryStruct = HistoryStruct.OrderByDescending(h => h.IdTime.Index).ToList();
            }
        }
        /// <summary>
        /// Сохраняет блоки
        /// </summary>
        /// <param name="saveForce"></param>
        /// <returns></returns>
        public virtual bool Save()
        {
            return false;
        }
        /// <summary>
        /// Загружает блок из бинарного файла
        /// </summary>
        /// <param name="timeBlock"></param>
        /// <param name="postfix"></param>
        /// <returns></returns>
        protected Block loadByIdTime(BlockTime timeBlock, string postfix)
        {
            var filename = getFileNameDump(timeBlock, postfix);
            return BaseBlock<Block, T>.Load(filename);
        }
        /// <summary>
        /// Возвращает последний использованный блок
        /// </summary>
        /// <returns></returns>
        public virtual Block GetCurrentBlock()
        {
            lock (syncLock)
            {
                return lastUseBlock;
            }
        }
    }
}
