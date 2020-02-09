using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

public static class ComboBoxExtension
{
    /// <summary>
    /// Инициализация обьекта с настройками по умолчанию
    /// </summary>
    /// <param name="obj"></param>
    public static void InitDefault(this ComboBox obj)
    {
        obj.DropDownStyle = ComboBoxStyle.DropDownList;
    }
    public static void SetListValues(this ComboBox obj, IEnumerable<string> list, string currentValue = "")
    {
        SetList(obj, list);
        SetCurrentValues(obj, currentValue);
    }

    private static void SetList(this ComboBox obj, IEnumerable<string> list)
    {
        if (list.Count() > 0)
        {
            obj.Items.Clear();
            foreach (var item in list)
            {
                obj.Items.Add(item);
            }
        }
    }
    public static void SetCurrentValues(this ComboBox obj, string currentValue)
    {
        if (!currentValue.Empty())
        {
            obj.SelectedIndex = obj.FindStringExact(currentValue);
        }
    }
    public static void Clear(this ComboBox obj)
    {
        obj.Items.Clear();
    }
}


public static class CheckBoxExtension
{
    public static void AddGroup(this CheckBox obj)
    {
        obj.Click += (s, e) =>
        {
            var parent = ((CheckBox)s).Parent;
            foreach (var control in parent.Controls)
            {
                if (control is CheckBox)
                {
                    if (obj == control && ((CheckBox)control).Checked)
                    {
                        ((CheckBox)control).BackColor = Color.Red;
                    }
                    else
                    {
                        ((CheckBox)control).BackColor = SystemColors.Control;
                        ((CheckBox)control).Checked = false;
                    }
                }
            }
        };
    }
}