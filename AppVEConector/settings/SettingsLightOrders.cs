using AppVEConector.libs;
using System;
using System.Collections.Generic;

namespace AppVEConector.settings
{
    public class SettingsLightOrders
    {
        /// <summary>
        /// Класс настроек
        /// </summary>
        [Serializable]
        public class SettingsForm : Settings
        {
            public const int COUNT_PANELS = 10;
            [Serializable]
            public class ItemSec
            {
                public string SecAndClass = "";
                public string Name = "";
            }
            public string[] StructPanels = null;
            public int indexScreen = 0;
            public int numberPosition = 0;
            public List<ItemSec> ItemsSec = null;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="filename"></param>
            public SettingsForm(string filename) : base(filename)
            {
                data = this;
                load();
                if (StructPanels.IsNull())
                {
                    StructPanels = new string[COUNT_PANELS];
                }
                if (ItemsSec.IsNull())
                {
                    ItemsSec = new List<ItemSec>();
                }
            }

        }

        /// <summary>
        /// Сохраняемые настройки
        /// </summary>
        protected SettingsForm Storage = null;

        protected SettingsLightOrders(string filenameSettings)
        {
            Storage = new SettingsForm(filenameSettings);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        public static SettingsLightOrders New(string fileName)
        {
            return new SettingsLightOrders(fileName);
        }


        public dynamic Get(string varName)
        {
            return Storage.Get(varName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void Set(string name, object value)
        {
            Storage.Set(name, value);
        }

        public void Save()
        {
            Storage.save();
        }
    }
}
