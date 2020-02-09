using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace Libs
{
    public class WFile
    {
        private string FileName = "";
        public WFile(string fileName)
        {
            this.FileName = fileName;
        }
        /// <summary> Размер файла в байтах </summary>
        /// <returns></returns>
        public long Size()
        {
            if (!this.Exists()) return 0;
            FileInfo file = new FileInfo(this.FileName);
            return file.Length;
        }

        public int Append(string TextWrite)
        {
            //try
            //{
            File.AppendAllText(this.FileName, TextWrite + Environment.NewLine);
            return 0;
            //}
            //catch (Exception e) { return -1; }
        }

        public string[] ReadAllLines()
        {
            if (File.Exists(this.FileName))
                return File.ReadAllLines(this.FileName);
            return null;
        }

        public string ReadAll()
        {
            if (File.Exists(this.FileName))
                return File.ReadAllText(this.FileName);
            return null;
        }

        public string ReadLastString()
        {
            try
            {
                if (File.Exists(this.FileName))
                {
                    string[] tmp = File.ReadAllLines(this.FileName);
                    int countStr = tmp.Count();
                    if (countStr > 0) return tmp[countStr - 1] == "" ? tmp[countStr - 2] : tmp[countStr - 1];
                }
            }
            catch (Exception) { return null; }
            return null;
        }

        public void WriteFileNew(string Text)
        {
            if (File.Exists(this.FileName))
                File.Delete(this.FileName);
            File.WriteAllText(this.FileName, Text);
        }

        public void WriteFileNew(string[] ArrayLines)
        {
            if (File.Exists(this.FileName))
                File.Delete(this.FileName);
            File.WriteAllLines(this.FileName, ArrayLines);
        }

        public void WriteBinary<T>(T obj)
        {
            FileStream fsser = new FileStream(this.FileName, FileMode.OpenOrCreate, FileAccess.Write);
            BinaryFormatter bfser = new BinaryFormatter();
            bfser.Serialize(fsser, obj);
            fsser.Close();
        }

        public T ReadBinary<T>()
        {
            FileStream fsdis = new FileStream(this.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            BinaryFormatter bfdis = new BinaryFormatter();
            T obj = (T)bfdis.Deserialize(fsdis);
            fsdis.Close();

            return obj;
        }

        public bool Exists()
        {
            return File.Exists(this.FileName);
        }

        public bool Delete()
        {
            if (this.FileName == "") return false;
            if (this.Exists())
            {
                File.Delete(this.FileName);
				return true;
            }
            return false;
        }

        public bool ClearFile()
        {
            if (this.FileName == "") return false;
            if (this.Delete())
            {
                File.WriteAllText(this.FileName, "");
                return true;
            }
            return false;
        }
    }
}
