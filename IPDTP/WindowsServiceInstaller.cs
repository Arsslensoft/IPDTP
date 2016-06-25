using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace IPDTP
{
    [RunInstaller(true)]
    public class WindowsServiceInstaller : Installer
    {
        /// <summary>
        /// Public Constructor for WindowsServiceInstaller.
        /// - Put all of your Initialization code here.
        /// </summary>
        public WindowsServiceInstaller()
        {
            ServiceProcessInstaller serviceProcessInstaller = new ServiceProcessInstaller();
            ServiceInstaller serviceInstaller = new ServiceInstaller();

            //# Service Account Information
            serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
            serviceProcessInstaller.Username = null;
            serviceProcessInstaller.Password = null;
            

            //# Service Information
            serviceInstaller.DisplayName = "Inter-Process Data Transmission Protocol";
            serviceInstaller.StartType = ServiceStartMode.Automatic;
            serviceInstaller.Description = "This Service Implements The IPDTP Protocol (Inter-Process Data Transmission Protocol).This Service Simplify and Extend the IPC, monitor and manage all connection between processes executed in this machine or in other machines.";
            
            // This must be identical to the WindowsService.ServiceBase name
            // set in the constructor of WindowsService.cs
            serviceInstaller.ServiceName = "IPDTPService";

            this.Installers.Add(serviceProcessInstaller);
            this.Installers.Add(serviceInstaller);
        }
    }
}
