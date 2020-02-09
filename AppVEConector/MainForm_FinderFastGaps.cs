using GraphicTools.Extension;
using Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppVEConector
{
    public partial class MainForm : Form
    {
        public string FastGapLog = "";
        private Strategy.FastGap FastGapSettings = new Strategy.FastGap();
        
        private void InitPanelFastGap()
        {
            numericUpDownFGSizeGap.InitWheelDecimal(0, 10000, 1);
            FastGapSettings.Option_1 = (int)numericUpDownFGSizeGap.Value;
            numericUpDownFGSizeGap.ValueChanged += (s, e) =>
            {
                FastGapSettings.Option_1 = (int)numericUpDownFGSizeGap.Value;
            };
    
            numericUpDownFGTimeStep.InitWheelDecimal(0, 10000000, 1);
            numericUpDownFGTimeStep.Value = FastGapSettings.StepTime;
            numericUpDownFGTimeStep.ValueChanged += (s, e) =>
            {
                FastGapSettings.StepTime = (int)numericUpDownFGTimeStep.Value;
            };

            comboBoxFGTimeFrame.DataSource = SelectorTimeFrame.GetAll();
            comboBoxFGTimeFrame.SelectedIndex = SelectorTimeFrame.GetIndex(FastGapSettings.TimeFrame);
            comboBoxFGTimeFrame.SelectedIndexChanged += (s, e) => {
                var tf = ((SelectorTimeFrame)comboBoxFGTimeFrame.SelectedItem).TimeFrame;
                FastGapSettings.TimeFrame = tf;
            };
        }


        Thread threadStrategy = null;
        bool showLog = false;
        private void EventStartegy()
        {
            if (checkBoxFGActivate.Checked)
            {
                if (threadStrategy.IsNull() || threadStrategy.ThreadState == ThreadState.Stopped)
                {
                    if (DataTrading.Collection.Count() == 0)
                    {
                        return;
                    }
                    if (FastGapSettings.StepTime == 0)
                    {
                        return;
                    }
                    threadStrategy = MThread.InitThread(() =>
                    {
                        var allStocks = DataTrading.Collection.ToArray();
                        foreach (var elem in allStocks)
                        {
                            if (elem.ListStrategy.Count > 0)
                            {
                                foreach (var stg in elem.ListStrategy)
                                {
                                    if (stg is Strategy.Strategy)
                                    {
                                        var strategy = (Strategy.Strategy)stg;

                                        strategy.StepTime = FastGapSettings.StepTime;
                                        strategy.TimeFrame = FastGapSettings.TimeFrame;
                                        strategy.IndexStartCandle = FastGapSettings.IndexStartCandle;
                                        strategy.Option_1 = FastGapSettings.Option_1;
                                        strategy.Option_2 = FastGapSettings.Option_2;
                                        strategy.Option_3 = FastGapSettings.Option_3;

                                        var now = DateTime.Now;
                                        if (strategy.TimeLastAction < now.AddSeconds(strategy.StepTime * -1))
                                        {
                                            var tf = elem.CollectionTimeFrames.FirstOrDefault(t => t.TimeFrame == strategy.TimeFrame);
                                            strategy.Security = elem.Security;
                                            strategy.BeforeAction(() =>
                                            {

                                            });
                                            var log = strategy.ActionCollection(tf.CollectionArray.ToArray());
                                            strategy.TimeLastAction = now;
                                            if (!log.Empty())
                                            {
                                                showLog = true;
                                                this.FastGapLog = log + this.FastGapLog;
                                            }
                                        }
                                    }
                                    Thread.Sleep(1);
                                }
                            }
                        }
                        threadStrategy = null;
                    });
                    if (showLog)
                    {
                        showLog = false;
                        Form_MessageSignal.Show(this.FastGapLog);
                        textBoxFGLog.Text = this.FastGapLog;
                    }
                }
            }
        }
    }
}
