using Market.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Volumes
{
    public class HVolumesTF : ElementTF<HVolumeBlock, HVolume>
    {
        const string POSTFIX_FILE_DUMP = "_hvol.dat";

        /// <summary> Конструктор </summary>
        /// <param name="timeFrame">Кол-во минут</param>
        public HVolumesTF(int period, string dirKeep)
        {
            lock (syncLock)
            {
                periodTimeFrame = period;
                dirKeepHistory = dirKeep;
            }
        }

        /// <summary>
        /// Удаляет свечу
        /// </summary>
        /// <param name="datetime"></param>
        public void Delete(DateTime datetime)
        {
            base.Delete(datetime, (block, candle) =>
            {
                if (block.DeleteElement(candle))
                {
                    lastUseBlock = null;
                }
            });
        }

        /// <summary>
        /// Получает блок по свечам-объемам
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        protected override HVolumeBlock getBlock(DateTime time)
        {
            lock (syncLock)
            {
                BlockTime timeBlock = BlockTime.ConvertForDay(time);
                if (lastUseBlock.NotIsNull())
                {
                    if (lastUseBlock.IdTime == timeBlock)
                    {
                        return lastUseBlock;
                    }
                    lastUseBlock = null;
                }
                if (Blocks.Count > 0)
                {
                    var block = Blocks.FirstOrDefault(b => b.IdTime == timeBlock);
                    if (block.NotIsNull() && block.IdTime == timeBlock)
                    {
                        lastUseBlock = block;
                        return block;
                    }
                }
                if (lastUseBlock.IsNull())
                {
                    HVolumeBlock newBlock = loadByIdTime(timeBlock, POSTFIX_FILE_DUMP);
                    if (newBlock.IsNull())
                    {
                        newBlock = new HVolumeBlock(periodTimeFrame, timeBlock);
                    }
                    Blocks.Add(newBlock);
                    lastUseBlock = newBlock;
                }
                return lastUseBlock;
            }
        }

        /// <summary>
        /// Возвращает дату начала хранения
        /// </summary>
        /// <returns></returns>
        private BlockTime getDateBeginKeep()
        {
            var date = DateTime.Now.AddDays(daysPeriodKeep * -1);
            return BlockTime.ConvertForDay(date);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="saveForce"></param>
        public override bool Save()
        {
            lock (syncLock)
            {
                if (Blocks.Count > 0)
                {
                    var dateBeginKeep = getDateBeginKeep();
                    var blocksSave = Blocks.Where(b => b.IdTime.Index >= dateBeginKeep.Index);
                    if (blocksSave.Count() > 0)
                    {
                        foreach (var block in blocksSave)
                        {
                            var filename = getFileNameDump(block.IdTime, POSTFIX_FILE_DUMP);
                            block.Save(filename);
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <param name="addNew"></param>
        /// <returns></returns>
        public override HVolume GetCandle(DateTime time, bool addNew = false)
        {
            var block = getBlock(time);
            lock (syncLock)
            {
                if (block.NotIsNull())
                {
                    var candle = block.GetElement(time, addNew);
                    if (candle.NotIsNull())
                    {
                        return candle;
                    }
                }
                return null;
            }
        }
    }
}
