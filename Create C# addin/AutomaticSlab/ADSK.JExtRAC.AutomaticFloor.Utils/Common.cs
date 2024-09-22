using System;
using System.Windows.Forms;

namespace ADSK.JExtRAC.AutomaticFloor.Utils
{
    // Token: 0x02000002 RID: 2
    public partial class Common
    {
        // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        public static void NumberCheck(object sender, KeyPressEventArgs e, bool allowNegativeValue = false)
        {
            if (!allowNegativeValue)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                {
                    e.Handled = true;
                }
                if (sender is TextBox && e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
                {
                    e.Handled = true;
                }
                if (sender is ComboBox && e.KeyChar == '.' && (sender as ComboBox).Text.IndexOf('.') > -1)
                {
                    e.Handled = true;
                    return;
                }
            }
            else
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != '-')
                {
                    e.Handled = true;
                }
                if (sender is TextBox)
                {
                    if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
                    {
                        e.Handled = true;
                    }
                    if (e.KeyChar == '-' && (sender as TextBox).Text.IndexOf('-') > -1)
                    {
                        e.Handled = true;
                    }
                }
                if (sender is ComboBox)
                {
                    if (e.KeyChar == '.' && (sender as ComboBox).Text.IndexOf('.') > -1)
                    {
                        e.Handled = true;
                    }
                    if (e.KeyChar == '-' && (sender as ComboBox).Text.IndexOf('-') > -1)
                    {
                        e.Handled = true;
                    }
                }
            }
        }
    }
}
