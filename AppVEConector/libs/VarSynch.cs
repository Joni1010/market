using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace libs
{
    public class VarSynch<T>
    {

        /// <summary>
        /// 
        /// </summary>
        private T value;

        protected readonly object syncLock = new object();

        public delegate void eventValue(T value);
        public eventValue OnSet = null;

        public VarSynch()
        {
            
        }
        /// <summary>
        /// Значение перменной
        /// </summary>
        public T Value
        {
            set
            {
                lock (syncLock)
                {
                    this.value = value;
                    if (OnSet.NotIsNull())
                    {
                        OnSet(this.value);
                    }
                }
            }
            get
            {
                lock (syncLock)
                {
                    return this.value;
                }
            }
        }
    }
}
