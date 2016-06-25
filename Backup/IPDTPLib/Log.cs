using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace IPDTPLib
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
                    sw.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch (Exception sdex)
            {

            }
            finally
            {

            }
                    }

        public static void LWrite(string log)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(Application.StartupPath + @"\Log.txt", true))
                {
                    sw.WriteLine(log);
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
