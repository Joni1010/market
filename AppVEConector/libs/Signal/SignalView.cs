using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppVEConector.libs.Signal
{
    class SignalView
    {
        /// <summary>
        /// 
        /// </summary>
        public static SignalGSM GSMSignaler = new SignalGSM();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="textSecClass"></param>
        /// <param name="lastIndexRow"></param>
        public static void constructGridSignals(DataGridView grid, string textSecClass, Action afterAction)
        {
            grid.GuiAsync(() =>
            {
                if(grid.Rows.Count == 0)
                {
                    return;
                }
                var rowForClone = grid.Rows[0];             

                grid.Rows.Clear();
                if (GSMSignaler.ToArray().Length > 0)
                {
                    int i = 0;
                    foreach (var sig in GSMSignaler.ToArray())
                    {
                        if (sig.SecClass == textSecClass || textSecClass.Empty())
                        {
                            var newRow = (DataGridViewRow)rowForClone.Clone();
                            if (sig.Type == SignalMarket.TypeSignal.ByPrice)
                            {
                                newRow.Cells[0].Value = sig.SecClass;
                                newRow.Cells[1].Value = "Price: " + sig.Price.ToString() + " (" + sig.Comment + ")";

                                if (sig.Condition == SignalMarket.CondSignal.MoreOrEquals)
                                    newRow.Cells[2].Value = ">=";
                                else if (sig.Condition == SignalMarket.CondSignal.More)
                                    newRow.Cells[2].Value = ">";
                                else if (sig.Condition == SignalMarket.CondSignal.LessOrEquals)
                                    newRow.Cells[2].Value = "<=";
                                else if (sig.Condition == SignalMarket.CondSignal.Less)
                                    newRow.Cells[2].Value = "<";
                                else if (sig.Condition == SignalMarket.CondSignal.Equals)
                                    newRow.Cells[2].Value = "==";
                            }
                            else if (sig.Type == SignalMarket.TypeSignal.ByVolume)
                            {
                                newRow.Cells[0].Value = sig.SecClass;
                                newRow.Cells[1].Value = "Volume: " + sig.Volume.ToString() + " tf:" + sig.TimeFrame;
                                newRow.Cells[2].Value = ">=";
                            }
                            else if (sig.Type == SignalMarket.TypeSignal.ByTime)
                            {
                                newRow.Cells[0].Value = sig.Comment;
                                newRow.Cells[1].Value = "Time: " + sig.DateTime.GetDateTime().ToLongTimeString();
                                newRow.Cells[2].Value = "==";
                            }
                            else
                            {
                                newRow.Cells[0].Value = "none";
                                newRow.Cells[1].Value = "none";
                                newRow.Cells[2].Value = "none";
                            }
                            grid.Rows.Add(newRow);
                            newRow.Tag = sig;
                            i++;
                        }
                    }
                    if (afterAction.NotIsNull())
                    {
                        afterAction();
                    }
                }
            });
        }
    }
}
