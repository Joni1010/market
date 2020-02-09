
using MarketObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;


public static class DataGridViewSelectedRowCollectionExtension
{
    /// <summary>
    /// Получает первый элемент из коллекции
    /// </summary>
    /// <param name="rows"></param>
    /// <returns></returns>
    public static DataGridViewRow GetFirst(this DataGridViewSelectedRowCollection rows)
    {
        if (rows.NotIsNull() && rows.Count > 0)
        {
            foreach (var row in rows) {
                if (row is DataGridViewRow)
                {
                    var rowEl = (DataGridViewRow)row;
                    return rowEl;
                }
            }
        }
        return null;
    }
}


public static class DataGridViewRowExtension
{
    public static T GetElementTag<T>(this DataGridViewRow row)
    {
        if (row is DataGridViewRow)
        {
            var rowEl = (DataGridViewRow)row;
            if (rowEl.Tag.NotIsNull())
            {
                return (T)rowEl.Tag;
            }
        }
        return default;
    }
}
public static class ControlExtension
{
    public static void GuiAsync(this Control form, Action action)
    {
        if (form != null)
        {
            MethodInvoker AsyncAction = delegate
            {
                if (action != null) action();
            };
            if (form.InvokeRequired)
                form.BeginInvoke(AsyncAction);
            else AsyncAction();
        }
    }
    public delegate void ActionWithParam(object param);
    public static void GuiAsync(this Control form, ActionWithParam action, object arg)
    {
        if (form != null)
        {
            MethodInvoker AsyncAction = delegate
            {
                if (action != null) action(arg);
            };
            if (form.InvokeRequired)
                form.BeginInvoke(AsyncAction);
            else AsyncAction();
        }
    }
}


public static class ToolStripStatusLabelExtension
{
    public static void GuiAsync(this ToolStripStatusLabel f, Action action)
    {
        /*Control form = (Control)f.;
        if (form != null)
        {
            MethodInvoker AsyncAction = delegate
            {
                if (action != null) action();
            };
            form.BeginInvoke(AsyncAction);
        }*/
    }
}

public class ComboBoxItem
{
    /// <summary>
    /// Текстовое предстваление
    /// </summary>
    public string Text = "";
    /// <summary>
    /// Цыфровое представление
    /// </summary>
    public int Value = -1;

    public ComboBoxItem(int value, string text)
    {
        this.Value = value;
        this.Text = text;
    }

    public override string ToString()
    {
        return this.Text;
    }
}

public static class NumericUpDownExtesion
{
    class ChangeUpDown : Control
    {
        public ChangeUpDown(decimal val) { this.NewValChange = val; }
        /// <summary> Новое значение при вращении колеса мыши </summary>
        public decimal NewValChange = 0;
        public DateTime lastChange = DateTime.Now;
    }
    /// <summary>
    /// Инициализировать форму под рыночный инструмент
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="securities"></param>
    public static void InitSecurity(this NumericUpDown obj, Securities securities, decimal min = 0)
    {
        if (securities.NotIsNull())
        {
            obj.InitWheelDecimal(min, 10000000, securities.MinPriceStep, securities.Scale);
        }
    }

    public static void InitWheelDecimal(this NumericUpDown obj, decimal min, decimal max, decimal inc, int scale)
    {
        obj.DecimalPlaces = scale;
        obj.InitWheelDecimal(min, max, inc);
    }
    public static void InitWheelDecimal(this NumericUpDown obj, decimal min, decimal max, decimal inc)
    {
        obj.Minimum = min;
        obj.Maximum = max;
        obj.Increment = inc;
        obj.InitWheelDecimal();
    }

    public static void InitWheelDecimal(this NumericUpDown obj)
    {
        obj.Controls.Add(new ChangeUpDown(obj.Minimum));
        TextBoxBase txtBase = obj.Controls[1] as TextBoxBase;
        obj.KeyDown += (s, e) =>
        {
            var el = (NumericUpDown)s;
            if (e.KeyValue == 190 || e.KeyValue == 191)
            {
                int currentCaretPosition = txtBase.SelectionStart;
                txtBase.Text = txtBase.Text.Insert(currentCaretPosition, ",");
                txtBase.Text = Regex.Replace(txtBase.Text, "\\,{2,}", ",");
                el.Select(currentCaretPosition + 1, 0);
            }
        };
        obj.ValueChanged += (s, e) =>
        {
            var el = (NumericUpDown)s;
            /*el.Controls.ForEach<Control>((child) =>
			{
				if (child is ChangeUpDown)
				{
					var con = (ChangeUpDown)child;
					if (DateTime.Now.Ticks - con.lastChange.Ticks > 2)
					{
						con.lastChange = DateTime.Now;
						con.NewValChange = el.Value;
					}
					else
					{
						el.Value = con.NewValChange;
					}
				}
			});*/
        };
    }
    /// <summary>
    /// Проверяет попадание мыши только в область текста
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="e"></param>
    public static bool IsMouseOnTextBox(this NumericUpDown obj, MouseEventArgs e)
    {
        if (obj.Controls[1] is TextBox)
        {
            var control = obj.Controls[0] as Control;
            var pMouse = control.PointToScreen(new Point(e.X, e.Y));
            var pUpDown = control.PointToScreen(control.Location);

            if (pMouse.X < pUpDown.X)
            {
                return true;
            }
        }
        return false;
    }
}
namespace AppVEConector
{

}
