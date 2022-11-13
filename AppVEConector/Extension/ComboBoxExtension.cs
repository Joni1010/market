using AppVEConector.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    /// <summary>
    /// Создает поисковик и наполняет содержимым из поиска
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="search"></param>
    public static void InitSearcher(this ComboBox obj, Func<string, string[]> search, Action after = null)
    {
        if (search.IsNull())
        {
            return;
        }
        obj.TextChanged += (s, e) =>
        {
            var text = obj.Text;

            if (text.Length < 2 || obj.SelectedIndex >= 0)
            {
                return;
            }
            var listSec = search(text);
            if (listSec.Count() > 0)
            {
                obj.Clear();
                obj.SetListValues(listSec);
                obj.SelectedIndex = -1;
            }
            obj.Select(text.Length, 0);
            obj.DroppedDown = true;
            Cursor.Current = Cursors.Default;
            if (after.NotIsNull())
            {
                after();
            }
        };
    }
}
