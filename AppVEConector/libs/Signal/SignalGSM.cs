using Connector.Logs;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace AppVEConector.libs.Signal
{
    public class SignalGSM
    {
        private readonly object syncObj = new object();
        /// <summary> Объект порта </summary>
        public SignalPort sPort = null;
        /// <summary>  Номер на который отправлять сигналы </summary>
        public string Phone = "+79*********";
        /// <summary> Список сигналов </summary>
        private List<SignalMarket> Signals = new List<SignalMarket>();

        public delegate void EventSignal(SignalMarket objSignal);
        /// <summary> Событие удаления сигнала из списка сигналов  </summary>
        public EventSignal OnRemove = null;
        /// <summary> Событие добавления сигнала в список сигналов  </summary>
        public EventSignal OnAdd = null;
        /// <summary>
        /// Событие загрузки сигналов из файла
        /// </summary>
        public Action OnLoad = null;
        /// <summary>
        /// Определяет что порт принадлежит устройству
        /// </summary>
        public bool PortDevice = false;
        /// <summary>
        /// Директория хранения файла сигналов
        /// </summary>
        private string DirSignals = "\\";

        public SignalGSM()
        {
            this.DirSignals = Global.GetPathData();
        }

        public SignalMarket[] ToArray()
        {
            lock (syncObj)
            {
                return this.Signals.ToArray();
            }
        }
        /// <summary>
        /// Закрыть порт.
        /// </summary>
        public void Close()
        {
            if (this.sPort.NotIsNull())
                this.sPort.Port.Close();
        }
        /// <summary>
        /// Проверяет был ли иницирован порт
        /// </summary>
        /// <returns></returns>
        public bool IsInit()
        {
            if (this.sPort.NotIsNull()) return true;
            return false;
        }

        private string GetFilename()
        {
            return this.DirSignals + "\\signals.dat";
        }

        private bool SaveSignals()
        {
            return (bool)Qlog.CatchException(() =>
            {
                lock (syncObj)
                {
                    Stream stream = File.Open(this.GetFilename(), FileMode.Create);
                    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    binaryFormatter.Serialize(stream, this.Signals);
                    stream.Close();
                    return true;
                }
            });
        }
        /// <summary>
        /// Загрузить сигналы
        /// </summary>
        public void Load()
        {
            this.LoadSignals();
            if (this.OnLoad.NotIsNull())
                this.Load();
        }

        private bool LoadSignals()
        {
            return (bool)Qlog.CatchException(() =>
            {
                if (File.Exists(this.GetFilename()))
                {
                    lock (syncObj)
                    {
                        Stream stream = File.Open(this.GetFilename(), FileMode.Open);
                        var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                        this.Signals = (List<SignalMarket>)binaryFormatter.Deserialize(stream);
                        stream.Close();
                        return true;
                    }                                        
                }
                return false;
            });
        }

        public bool Init(string namePort)
        {
            this.sPort = new SignalPort(namePort);
            if (sPort.NotIsNull())
                return sPort.Open();
            return false;
        }

        public bool SendSignalCall()
        {
            if (sPort.NotIsNull())
            {
                this.SaveSignals();
                return sPort.WriteLine("CAL:" + Phone) > 0 ? true : false;
            }
            return false;
        }
        public bool SendSignalResetCall()
        {
            if (sPort.NotIsNull())
                return sPort.WriteLine("RES") > 0 ? true : false;
            return false;
        }
        public bool SendTestSignal(bool statIndicator = true)
        {
            if (sPort.NotIsNull())
                return sPort.WriteLine("TST" + (statIndicator ? "1" : "0")) > 0 ? true : false;
            return false;
        }

        public bool SendSignalSms(string textMsg)
        {
            if (sPort.NotIsNull())
                return sPort.WriteLine("SMP:" + this.Phone + "PHN:" + textMsg) > 0 ? true : false;
            return false;
        }
        /// <summary>
        /// Получить индекс в коллекции
        /// </summary>
        /// <param name="signal"></param>
        /// <returns></returns>
        public int GetIndex(SignalMarket signal)
        {
            lock (syncObj)
            {
                return Signals.Count > 0 ? Signals.IndexOf(signal) : 0;
            }
        }
        /// <summary>
        /// Перемещает по списку указанный сигнал. (Вверх, к 0)
        /// </summary>
        /// <param name="signal"></param>
        /// <returns></returns>
        public bool MoveUp(SignalMarket signal)
        {
            if (signal.IsNull()) return false;
            var index = this.Signals.IndexOf(signal);
            if (index >= 0 && index - 1 >= 0)
            {
                lock (syncObj)
                {
                    this.Signals.Remove(signal);
                    this.Signals.Insert(index - 1, signal);
                    this.SaveSignals();
                }
                return true;
            }
            return false;
        }

        public bool MoveDown(SignalMarket signal)
        {
            if (signal.IsNull()) return false;
            var index = this.Signals.IndexOf(signal);
            if (index >= 0 && index < this.Signals.Count - 1)
            {
                lock (syncObj)
                {
                    this.Signals.Remove(signal);
                    this.Signals.Insert(index + 1, signal);
                    this.SaveSignals();
                }
                return true;
            }
            return false;
        }


        /// <summary> Добавить сигнал </summary>
        /// <param name="newSignal"></param>
        public void AddSignal(SignalMarket newSignal)
        {
            if (newSignal.IsNull()) return;
            var oldEl = this.Signals.FirstOrDefault(s =>
                s.Comment == newSignal.Comment
                && s.Condition == newSignal.Condition
                && s.DateTime == newSignal.DateTime
                && s.Infinity == newSignal.Infinity
                && s.Price == newSignal.Price
                && s.SecClass == newSignal.SecClass
                && s.TimeFrame == newSignal.TimeFrame
                && s.Type == newSignal.Type
                && s.Volume == newSignal.Volume
                );
            lock (syncObj)
            {
                if (oldEl.NotIsNull()) this.Signals.Remove(oldEl);
                this.Signals.Add(newSignal);
            }
            if (this.OnAdd.NotIsNull())
            {
                this.OnAdd(newSignal);
            }
            this.SaveSignals();
        }

        /// <summary> Удалить сигнал </summary>
        /// <param name="newSignal"></param>
        public bool RemoveSignal(SignalMarket remSignal)
        {
            if (remSignal.IsNull()) return false;
            bool res = false;
            lock (syncObj)
            {
                res = Signals.Remove(remSignal);
            }
            if (this.OnRemove.NotIsNull() && res)
                this.OnRemove(remSignal);
            this.SaveSignals();
            return res;
        }
        /// <summary>
        /// Поиск сигнала по заявке
        /// </summary>
        /// <param name="price"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        public SignalMarket[] GetSignalByOrder(decimal price, string comment)
        {
            lock (syncObj)
            {
                return this.Signals.Where(s => s.Price == price && s.Comment.Contains(comment)).ToArray();
            }
        }

    }
}
