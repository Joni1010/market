using System;
using System.IO;

namespace Libs
{
	public class WorkLog
	{
		private string PrefixFileLog = "log";
		private string AppendPrefixString = "_";
		private string Path = "";
		private DateTime? DateFile = null;
		public WorkLog(string PrefixFile = "log")
		{
			this.PrefixFileLog = PrefixFile;
			this.AppendPrefixString = "_";
		}
		//Получение и установка пути к файлу
		public string PathFile(string Path = null)
		{
			if (Path != null)
			{
				this.Path = Path;
			}
			return this.Path;
		}

		public void SetDate(DateTime Date)
		{
			this.DateFile = Date;
		}
		//Получить имя файла текущего лога
		protected string GetNameCurFileLog()
		{
			DateTime date = DateTime.Now;
			if (this.DateFile != null)
				date = (DateTime)this.DateFile;
			return this.Path + this.PrefixFileLog + this.AppendPrefixString + date.Year + "-" + date.Month + "-" + date.Day + ".txt";
		}
		public void Write(string TextLog)
		{
			string file = this.GetNameCurFileLog();
			DateTime date = DateTime.Now;
			if (this.PathFile() != "" && !Directory.Exists(this.PathFile()))
				Directory.CreateDirectory(this.PathFile());
			File.AppendAllText(file, date.ToString() + ": " + TextLog + Environment.NewLine);
		}

		public void Append(string TextLog, bool appendDate = false)
		{
			string file = this.GetNameCurFileLog();
			DateTime date = DateTime.Now;
			if (this.PathFile() != "" && !Directory.Exists(this.PathFile()))
				Directory.CreateDirectory(this.PathFile());
			File.AppendAllText(file, (appendDate == true ? date.ToString() + ": " : "") + TextLog + Environment.NewLine);
		}
		//Добавить к префиксу какую-либо информацию
		public void AppendPrefix(string appendPrefixName)
		{
			this.AppendPrefixString = appendPrefixName;
		}

		public string ReadAll()
		{
			string file = this.GetNameCurFileLog();
			return this.ReadAll(file);
		}
		public string ReadAll(string FileName)
		{
			string file = FileName;
			if (File.Exists(file))
				return File.ReadAllText(file);
			else return null;
		}

		//Очищает файл
		public bool ClearFile()
		{
			string file = this.GetNameCurFileLog();
			if (this.PathFile() != "" && !Directory.Exists(this.PathFile()))
				Directory.CreateDirectory(this.PathFile());
			File.Delete(file);
			File.WriteAllText(file, "");
			return true;
		}
	}
}
