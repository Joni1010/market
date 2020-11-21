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
    public abstract class Settings
    {
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
            this.filename = fileName;
            if (!filename.Empty())
            {
                this.Load();
            }
            this.init();
        }
        /// <summary>
        /// 
        /// </summary>
        protected abstract void init();

        /// <summary>
        /// Сериализация и загрузка объекта в файл.
        /// </summary>
        public void Save()
        {
            Qlog.CatchException(() =>
            {
                if (this.data.NotIsNull())
                {
                    Stream stream = File.Open(this.filename, FileMode.Create);
                    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    lock (syncLock)
                    {
                        binaryFormatter.Serialize(stream, this.data);
                    }
                    stream.Close();
                    return true;
                }
                return false;
            }, "");
        }
        /// <summary>
        /// Загрузка и сериализация объекта из файла.
        /// </summary>
        public void Load()
        {
            Qlog.CatchException(() =>
            {
                if (File.Exists(filename))
                {
                    Stream stream = File.Open(this.filename, FileMode.Open);
                    stream.Position = 0;
                    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    lock (syncLock)
                    {
                        this.data = binaryFormatter.Deserialize(stream);
                    }
                    stream.Close();
                    return true;
                }
                return false;
            }, "");
        }
    }
}
