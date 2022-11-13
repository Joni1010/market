using System.Drawing;
using System.Windows.Forms;

public static class DataGridViewCellExtension
{
    public static void ColorMarket(this DataGridViewCell cell, decimal value)
    {
        if (value > 0)
        {
            cell.Style.BackColor = Color.LightGreen;
        }
        else if (value < 0)
        {
            cell.Style.BackColor = Color.LightCoral;
        }
        else if (value == 0)
        {
            cell.Style.BackColor = Color.White;
        }
    }
}
