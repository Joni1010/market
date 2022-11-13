using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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