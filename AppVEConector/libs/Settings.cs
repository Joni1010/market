using Connector.Logs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppVEConector.libs
{
    [Serializable]
    public abstract class Settings
    {
        /// <summary>
        /// Имя файла храниния настроек
        /// </summary>
        protected string filename = "";
        /// <summary>
        /// Locker
        /// </summary>
        private readonly object syncLock = new object();
        /// <summary>
        /// Объект данных
        /// </summary>
        protected object data = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        protected Settings(string fileName)
        {
            filename = fileName;
            if (!filename.Empty())
            {
                load();
            }
        }

        /// <summary>
        /// Установка значения настройки по ее названию
        /// </summary>
        public void Set(string varName, object value)
        {
            try
            {
                data.GetType().GetField(varName).SetValue(data, value);
                save();
                return;
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// Получение значения настройки по ее названию
        /// </summary>
        public dynamic Get(string varName)
        {
            try
            {
                return data.GetType().GetField(varName).GetValue(data);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Сериализация и загрузка объекта в файл.
        /// </summary>
        public bool save()
        {
            if (data.NotIsNull())
            {
                Stream stream = File.Open(filename, FileMode.Create);
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                lock (syncLock)
                {
                    binaryFormatter.Serialize(stream, data);
                }
                stream.Close();
                return true;
            }
            return false;
        }
        /// <summary>
        /// Загрузка и сериализация объекта из файла.
        /// </summary>
        protected bool load()
        {
            if (File.Exists(filename))
            {
                Stream stream = File.Open(filename, FileMode.Open);
                stream.Position = 0;
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                lock (syncLock)
                {
                    data = binaryFormatter.Deserialize(stream);
                }
                stream.Close();
                return true;
            }
            return false;
        }
    }
}
