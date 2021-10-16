using Market.Candles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Base
{
    [Serializable]
    public class BaseBlock <Block, T>
    {
        /// <summary>
        /// ID (время) блока
        /// </summary>
        public BlockTime IdTime = null;
        /// <summary>
        /// Период тайм-фрейма
        /// </summary>
        protected int periodTimeFrame = 0;
        /// <summary>
        /// Обьект мутекса
        /// </summary>
        protected readonly object syncLock = new object();
        /// <summary> Коллекция элементов в блоке </summary>
        protected List<T> Collection = new List<T>();
        /// <summary>
        /// Последний используемый элемент
        /// </summary>
        [field: NonSerialized()]
        protected T lastSearchedElement = default;
        /// <summary>
        /// Флаг определяющий были ли изменения в блоке
        /// </summary>
        protected bool flagWasChanged = false;

        /// <summary>
        /// Кол-во эелементов в блоке
        /// </summary>
        public int Count
        {
            get { lock (syncLock) { return Collection.Count; } }
        }

        /// <summary>
        /// Получить коллекцию элементов
        /// </summary>
        /// <returns></returns>
        public T[] GetAllCollection()
        {
            lock (syncLock)
            {
                return Collection.ToArray();
            }
        }
        /// <summary>
        /// Сохраняет текущий блок в файл(бинарный)
        /// </summary>
        /// <param name="filename"></param>
        public void Save(string filename)
        {
            lock (syncLock)
            {
                if (flagWasChanged)
                {
                    flagWasChanged = false;
                    using (Stream stream = File.Open(filename, FileMode.Create))
                    {
                        var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                        binaryFormatter.Serialize(stream, this);
                        stream.Close();
                    }
                }
            }
        }
        /// <summary>
        /// Загружает из файла блок
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static Block Load(string filename)
        {
            if (!File.Exists(filename))
            {
                return default;
            }
            using (Stream stream = File.Open(filename, FileMode.Open))
            {
                stream.Position = 0;
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                var loadBlock = (Block)binaryFormatter.Deserialize(stream);
                stream.Close();
                return loadBlock;
            }
        }

        /// <summary>
        /// Удаляет элемент из коллекции
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public bool DeleteElement(T element)
        {
            lock (syncLock)
            {
                flagWasChanged = true;
                lastSearchedElement = default;
                return Collection.Remove(element);
            }
        }
        /// <summary>
        /// Возвращает элемент коллекции
        /// </summary>
        /// <param name="time"></param>
        /// <param name="addNew">Добавить принудительно элемент</param>
        /// <returns></returns>
        public virtual T GetElement(DateTime time, bool addNew = false)
        {
            return default;
        }
        /// <summary>
        /// Установить как блок с изменениями
        /// </summary>
        public void SetChanged()
        {
            lock (syncLock)
            {
                flagWasChanged = true;
            }
        }
            
    }
}
