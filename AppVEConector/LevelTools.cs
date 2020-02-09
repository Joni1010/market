using AppVEConector.libs;
using GraphicTools;
using Libs;
using MarketObjects;
using QuikConnector.MarketObjects;
using System.Collections.Generic;
using System.IO;

namespace AppVEConector
{
    /// <summary> Класс горизонтальных уровней </summary>
    class LevelTools : AutoControls<LevelsFree.DoubleLevel>
    {
        const string POSTFIX_FILENAME = "levels.dat";
        const string SUBDIR = "levels";
        /// <summary>
        /// 
        /// </summary>
        private Securities Security;

        public LevelTools(Securities sec, string pathSaveFile)
            : base(pathSaveFile, "")
        {
            Security = sec;
            DinamicFileName(pathSaveFile, Security.Code + "." + Security.ClassCode + "." + POSTFIX_FILENAME, SUBDIR);
            Load();
        }

        public const char SPLITTER = '|';
    }
}
