using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace IPDTP.Client
{
    public static class Log
    {
        public static void Write(Exception ex)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(Application.StartupPath + @"\Log.txt", true))
                {
                    sw.WriteLine(ex.Message);
                    sw.WriteLine(ex.StackTrace);
                    sw.WriteLine(ex.Source);
                    sw.WriteLine("---------------------------------------------------");
                }
            }
            catch (Exception sdex)
            {

            }
            finally
            {

            }
                    }
    }
}
