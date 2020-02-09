using AppVEConector.libs;
using Connector.Logs;
using GraphicTools;
using MarketObjects;
using QuikConnector.MarketObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppVEConector
{
    public partial class Form_GraphicDepth : Form
    {
        /// <summary>
        /// Редактируемые уровни по инструменту
        /// </summary>
        private LevelTools Levels = null;
        /// <summary>
        /// Инициализация панели с уровнями
        /// </summary>
        private void InitPanelLevels()
        {
            this.Levels = new LevelTools(Securities, Global.GetPathData());

            checkBoxVisibleLevels.Click += (s, e) =>
            {
                if (checkBoxVisibleLevels.Checked)
                {
                    GraphicStock.Levels.Visible = false;
                }
                else
                {
                    GraphicStock.Levels.Visible = true;
                }
            };

            this.buttonAddLevel.Click += (s, e) =>
            {
                var top = numericUpDownPriceLevel.Value;
                var bot = numericUpDownPriceLevel2.Value;
                var datetimeleft = textBoxDateTimeLevel1.Text;
                var datetimeright = textBoxDateTimeLevel2.Text;
                if (top < bot)
                {
                    var tmp = top;
                    top = bot;
                    bot = tmp;
                }
                var value = new LevelsFree.DoubleLevel()
                {
                    Top = top,
                    Bottom = bot,
                    DateLeft = datetimeleft.Empty() ? null : new DateMarket(datetimeleft),
                    DateRight = datetimeright.Empty() ? null : new DateMarket(datetimeright),
                };
                this.Levels.Add(value);
                this.UpdatePanelLevels();
            };

            dataGridViewLList.SelectionChanged += (s, e) =>
            {
                if (dataGridViewLList.SelectedRows.NotIsNull())
                {
                    foreach (var row in dataGridViewLList.SelectedRows)
                    {
                        if (row is DataGridViewRow)
                        {
                            var level = (LevelsFree.DoubleLevel)((DataGridViewRow)row).Tag;
                            if (level.NotIsNull())
                            {
                                numericUpDownPriceLevel.Value = level.Top;
                                numericUpDownPriceLevel2.Value = level.Bottom;
                                textBoxDateTimeLevel1.Text = level.DateLeft.NotIsNull() ? level.DateLeft.ToString() : "";
                                textBoxDateTimeLevel2.Text = level.DateRight.NotIsNull() ? level.DateRight.ToString() : "";
                            }
                        }
                    }
                }
            };

            checkBoxPLAddRectLevlOnGraphic.Click += (s, e) =>
            {
                if (checkBoxPLAddRectLevlOnGraphic.Checked)
                {
                    ResetButtonLevels();
                    checkBoxPLAddRectLevlOnGraphic.Checked = true;
                    GraphicStock.Levels.TypeLevel = LevelsFree.TYPE_LEVELS.Rectangle;
                    checkBoxPLAddRectLevlOnGraphic.BackColor = Color.LightBlue;
                }
                else
                {
                    ResetButtonLevels();
                }
            };
            checkBoxPLAddLevelOnGraphic.Click += (s, e) =>
            {
                if (checkBoxPLAddLevelOnGraphic.Checked)
                {
                    ResetButtonLevels();
                    checkBoxPLAddLevelOnGraphic.Checked = true;
                    GraphicStock.Levels.TypeLevel = LevelsFree.TYPE_LEVELS.Vector;
                    checkBoxPLAddLevelOnGraphic.BackColor = Color.LightBlue;
                }
                else
                {
                    ResetButtonLevels();
                }
            };

            this.buttonDelLevel.Click += (s, e) =>
            {
                foreach (var row in dataGridViewLList.SelectedRows)
                {
                    if (row is DataGridViewRow)
                    {
                        var rowEl = (DataGridViewRow)row;
                        if (rowEl.Tag.NotIsNull())
                        {
                            Levels.Delete((LevelsFree.DoubleLevel)rowEl.Tag);
                        }
                    }
                }
            };

            AutoControls<LevelsFree.DoubleLevel>.EventAutoOrder eventLevels = (item) =>
            {
                this.UpdatePanelLevels();
            };
            Levels.OnDelete += eventLevels;
            Levels.OnAdd += eventLevels;
            Levels.OnEdit += eventLevels;

            buttonEditLevel.Click += (s, e) =>
            {
                foreach (var row in dataGridViewLList.SelectedRows)
                {
                    if (row is DataGridViewRow)
                    {
                        var rowEl = (DataGridViewRow)row;
                        var level = rowEl.GetElementTag<LevelsFree.DoubleLevel>();
                        if (level.NotIsNull())
                        {
                            Qlog.CatchException(() =>
                            {
                                level.Top = numericUpDownPriceLevel.Value;
                                level.Bottom = numericUpDownPriceLevel2.Value;
                                if (!textBoxDateTimeLevel1.Text.Empty())
                                {
                                    level.DateLeft.SetDateTime(Convert.ToDateTime(textBoxDateTimeLevel1.Text));
                                }
                                if (!textBoxDateTimeLevel2.Text.Empty())
                                {
                                    level.DateRight.SetDateTime(Convert.ToDateTime(textBoxDateTimeLevel2.Text));
                                }
                                Levels.Edit(level);
                            }, "",() => { MessageBox.Show("Значение даты не распознано!"); });
                        }
                    }
                }
            };
        }
        /// <summary>
        /// Сброс кнопок рисования уровней
        /// </summary>
        private void ResetButtonLevels()
        {
            checkBoxPLAddLevelOnGraphic.Checked = false;
            checkBoxPLAddRectLevlOnGraphic.Checked = false;
            checkBoxPLAddLevelOnGraphic.BackColor = Color.Transparent;
            checkBoxPLAddRectLevlOnGraphic.BackColor = Color.Transparent;
            GraphicStock.Levels.TypeLevel = LevelsFree.TYPE_LEVELS.Nothing;
        }

        /// <summary> Обновляет список уровней </summary>
        private void UpdatePanelLevels(bool isNew = false)
        {
            Qlog.CatchException(() =>
            {
                if (this.Levels.IsNull() || isNew)
                {
                    numericUpDownPriceLevel.InitSecurity(Securities, -1000000);
                    numericUpDownPriceLevel2.InitSecurity(Securities, -1000000);
                }
                RefrashLGridList();
                UpdateGraphic();
            });
        }

        private void RefrashLGridList()
        {
            LevelsFree.DoubleLevel selectLevel = null;
            if (dataGridViewLList.SelectedRows.Count > 0)
            {
                var row = dataGridViewLList.SelectedRows.GetFirst();
                if (row.NotIsNull())
                {
                    selectLevel = row.GetElementTag<LevelsFree.DoubleLevel>();
                }
            }
            dataGridViewLList.Rows.Clear();

            if (Levels.Count > 0)
            {
                foreach (var el in Levels.ToArray)
                {
                    if (el.NotIsNull())
                    {
                        var newRow = (DataGridViewRow)dataGridViewLList.Rows[0].Clone();
                        newRow.Cells[0].Value = el.Top;
                        newRow.Cells[1].Value = el.Bottom;
                        newRow.Cells[2].Value = el.DateLeft;
                        newRow.Cells[3].Value = el.DateRight;
                        newRow.Tag = el;

                        dataGridViewLList.Rows.Add(newRow);

                        if (selectLevel.NotIsNull())
                        {
                            if (el == selectLevel)
                            {
                                newRow.Selected = true;
                            }
                        }
                    }
                }
            }
        }
    }
}
