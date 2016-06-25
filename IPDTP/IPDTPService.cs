using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using IPDTPLib;

namespace IPDTP
{
    public partial class IPDTPService : ServiceBase
    {
        public IPDTPService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {

                IPDTPApplication.Initialize();
                
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            finally
            {

            }
        }

        protected override void OnStop()
        {
            try
            {
                IPDTPApplication.Stop();
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            finally
            {

            }
        }
    }
}
