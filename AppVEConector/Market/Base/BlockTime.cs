using System;

namespace Market.Base
{
    [Serializable]
    public class BlockTime
    {
        public int Year = 0;
        public int Month = 0;
        public int Day = 0;
        private long numId = 0;

        public long Index
        {
            get { return getNumId(); }
        }

        private long getNumId()
        {
            if (numId == 0)
            {
                numId = (Year.ToString()
                + (Month < 10 ? "0" + Month.ToString() : Month.ToString())
                + (Day < 10 ? "0" + Day.ToString() : Day.ToString())).ToInt64();
            }
            return numId;
        }

        public static BlockTime ConvertForDay(DateTime time)
        {
            return new BlockTime() { Year = time.Year, Month = time.Month, Day = time.Day };
        }
        public static BlockTime ConvertForMonth(DateTime time)
        {
            return new BlockTime() { Year = time.Year, Month = time.Month, Day = 0 };
        }
        public static bool operator ==(BlockTime d1, BlockTime d2)
        {
            if (d1.Year == d2.Year && d1.Month == d2.Month && d1.Day == d2.Day)
            {
                return true;
            }
            return false;
        }
        public static bool operator !=(BlockTime d1, BlockTime d2)
        {
            if (d1.Year != d2.Year || d1.Month != d2.Month || d1.Day != d2.Day)
            {
                return true;
            }
            return false;
        }

        public override bool Equals(object d2)
        {
            return this == (BlockTime)d2;
        }

        public override int GetHashCode()
        {
            return Year+Month+Day;
        }

        public override string ToString()
        {
            return getNumId().ToString();
        }
    }
}
