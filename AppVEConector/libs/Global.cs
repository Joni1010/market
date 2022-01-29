
using Connector.Logs;
using System;
using System.Collections.Generic;
using System.IO;

namespace AppVEConector.libs
{
    public class Global
    {
        /// <summary>
        /// Возвращает корневой путь к директории с данными.
        /// </summary>
        /// <returns></returns>
        public static string GetPathData()
        {
            var sett = Connector.QuikConnector.ConfSettings.GetParam("Main", "PathData");
            return sett.NotIsNull() ? sett.Value : "";
        }
        /// <summary>
        /// Файл с рабочими инструментами
        /// </summary>
        public const string FILE_WORKING_STOCK = "market.list";


        /// <summary> Список загруженных активных инструметов </summary>
        private static IEnumerable<string> ListLoadSec = null;
        /// <summary> Возвращает список рабочих инструментов </summary>
        /// <returns>Возвращает список инструментов</returns>
        public static IEnumerable<string> GetWorkingListSec(bool reloadList = false)
        {
            try
            {
                if (Global.ListLoadSec.NotIsNull() && !reloadList)
                {
                    return Global.ListLoadSec;
                }
                List<string> list = new List<string>();
                var rootDir = Global.GetPathData();
                var filename = rootDir + "\\" + FILE_WORKING_STOCK;
                if(!File.Exists(filename)){
                    File.Create(filename);
                }
                System.IO.StreamReader openFile = new System.IO.StreamReader(filename, true);
                while (!openFile.EndOfStream)
                {
                    string line = openFile.ReadLine();
                    if (!line.Empty() && line != "")
                    {
                        list.Add(line);
                    }
                }
                Global.ListLoadSec = list;
                return Global.ListLoadSec;
            }
            catch (Exception e)
            {
                Qlog.Write(e.ToString());
            }
            return null;
        }
        /// <summary>
        /// Добавляет рабочий инструмент в конец файла
        /// </summary>
        /// <param name="secCode"></param>
        /// <returns></returns>
        public static IEnumerable<string> AddWorkingListSec(string secCode)
        {
            try
            {
                var rootDir = Global.GetPathData();
                File.AppendAllText(rootDir + "\\" + FILE_WORKING_STOCK, secCode + "\r\n");
            }
            catch (Exception e)
            {
                Qlog.Write(e.ToString());
            }
            return null;
        }
    }
}
