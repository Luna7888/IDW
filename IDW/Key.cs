using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDW
{
    public static class Key
    {

        public static void IntNumber(KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8)
            {
                e.Handled = true;
            }
        }

        public static void IntNumber_Ponto(KeyPressEventArgs e, object sender)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8 && (e.KeyChar != '.' || ((sender as TextBox).Text.Contains('.'))))
            {
                e.Handled = true;
            }
        }





    }
}
