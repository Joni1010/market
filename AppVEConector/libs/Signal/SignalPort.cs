using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppVEConector.libs.Signal
{
	public class SignalPort
	{
		/// <summary>
		/// Порт
		/// </summary>
		public SerialPort Port = null;
		/// <summary>
		/// Функция обработчик принимаемых с порта данных.
		/// </summary>
		public Action<string, SerialPort, SerialDataReceivedEventArgs> OnReceived = null;

		/// <summary>
		/// Инициализация объекта порта. По умолчанию "COM1"
		/// </summary>
		/// <param name="namePort"></param>
		public SignalPort(string namePort = "COM1", int speed = 9600, Parity parity = Parity.None)
		{
			this.Port = new SerialPort(namePort, speed, parity, 8, StopBits.One);
		}
		/// <summary>
		/// Открывает порт для работы. Иначе возвращает false.
		/// </summary>
		public bool Open()
		{
			try
			{
				Port.Open();
				Port.DataReceived += port_DataReceived;
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		/// <summary>
		/// Обработчик получения данных с порта
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			var objPort = (SerialPort)sender;
			byte[] buf = new byte[1000];
			//this.Wait();
			Thread.Sleep(100);
			var data = objPort.ReadExisting();

			if (OnReceived.NotIsNull())
				OnReceived(data, objPort, e);
		}

		/// <summary>
		/// Записывает данные в открытый порт c переходом на след. строку.
		/// </summary>
		/// <returns></returns>
		public int WriteLine(string data)
		{
			if (this.Port != null)
			{
				try
				{
					this.Port.WriteLine(data);
					return data.Length;
				}
				catch (Exception)
				{
					return -1;
				}
			}
			return -1;
		}
		/// <summary>
		/// Записывает данные в открытый порт
		/// </summary>
		/// <returns></returns>
		public int Write(string data)
		{
			if (this.Port != null)
			{
				try
				{
					this.Port.Write(data);
					return data.Length;
				}
				catch (Exception)
				{
					return -1;
				}
			}
			return -1;
		}
		/// <summary>
		/// Ожидает пока будут полученны все данные
		/// </summary>
		public void Wait()
		{
			int timeout = 100;
			int countBytesRead = -1;
			while (true)
			{
				if (countBytesRead == this.Port.BytesToRead) break;
				countBytesRead = this.Port.BytesToRead;
				Thread.Sleep(10);
				if (timeout <= 0) break;
				timeout--;
			}
		}
		/// <summary>
		/// Получает список портов
		/// </summary>
		public static string[] GetListPorts()
		{
			// Get a list of serial port names.
			return SerialPort.GetPortNames();
		}
	}
}
