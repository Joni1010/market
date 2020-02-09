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
    public class AutoControls<T>
    {
        protected string FullFileName = "";
        private readonly object objSync = new object();

        public AutoControls(string pathFile, string namefile)
        {
            DinamicFileName(pathFile, namefile);
        }
        /// <summary>
        /// Изменить название файла
        /// </summary>
        /// <param name="namefile"></param>
        public void DinamicFileName(string pathFile, string namefile, string subdir = "")
        {
            FullFileName = pathFile + (subdir.Length > 0 ? subdir + "\\" : "") + namefile;
        }

        public delegate void EventAutoOrder(T objControl);
        public EventAutoOrder OnAdd;
        public EventAutoOrder OnDelete;
        public EventAutoOrder OnEdit;
        /// <summary>
        /// Список
        /// </summary>
        protected List<T> Collection = new List<T>();

        /// <summary>
        /// Count
        /// </summary>
        public int Count
        {
            get
            {
                lock (objSync)
                {
                    return Collection.Count;
                }
            }
        }
        /// <summary>
        /// Массив всех элементов
        /// </summary>
        public T[] ToArray
        {
            get
            {
                lock (objSync)
                {
                    return Collection.ToArray();
                }
            }
        }
        public object ObjectCollection
        {
            get
            {
                lock (objSync)
                {
                    return Collection;
                }
            }
        }
        /// <summary>
        /// Получает элемент из коллекции, если он есть, иначе null.
        /// </summary>
        /// <param name="predic"></param>
        /// <returns></returns>
        public T GetItem(Func<T, bool> predic)
        {
            return ToArray.FirstOrDefault(predic);
        }
        /// <summary>
        /// Добавить элемент
        /// </summary>
        /// <param name="order"></param>
        public void Add(T objControl)
        {
            lock (objSync)
            {
                Collection.Add(objControl);
            }
            Save();
            if (OnAdd.NotIsNull())
            {
                OnAdd(objControl);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objControl"></param>
        public void Edit(T objControl)
        {
            Save();
            if (OnEdit.NotIsNull())
            {
                OnEdit(objControl);
            }
        }
        /// <summary>
        /// Удалить указанный ордер
        /// </summary>
        /// <param name="order"></param>
        public void Delete(T item)
        {
            if (item.NotIsNull())
            {
                lock (objSync)
                {
                    Collection.Remove(item);
                }
                Save();
                if (OnDelete.NotIsNull())
                {
                    OnDelete(item);
                }
            }
        }
        /// <summary>
        /// Удалить все элементы по инструменту
        /// </summary>
        /// <param name="sec"></param>
        public void Delete(Securities sec, Func<T, bool> predicat)
        {
            var foundObjs = Collection.Where(predicat).ToArray();
            if (foundObjs.NotIsNull() && foundObjs.Count() > 0)
            {
                foreach (var delObj in foundObjs)
                {
                    Delete(delObj);
                }
            }
        }
        /// <summary>
        /// Сохраняет коллекцию
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private bool Save()
        {
            object obj = null;
            lock (objSync)
            {
                obj = Collection.Clone();
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
                Collection = (List<T>)binaryFormatter.Deserialize(stream);
            }
            stream.Close();
            return true;
        }
    }
}
