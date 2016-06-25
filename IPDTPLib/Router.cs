using System;
using System.Collections.Generic;
using System.Text;
using IPDTP.IPC;
using IPDTP.COM;

namespace IPDTPLib
{
  
   public static class Router
    {
       public static void Register(string servername, string upl)
       {
           if (!IPDTPApplication.SPNS.ContainsKey(servername))
           {
               IPDTPApplication.SPNS.Add( servername,upl);
               Log.LWrite("UPL REGISTRED IN SPNS (Simple Process Name Server) ('" + upl + "')  ('" + servername + "')");
           }
        
       }
       public static string ResolveSNPS(string snps)
       {
           if (IPDTPApplication.SPNS.ContainsKey(snps))
               return IPDTPApplication.SPNS[snps];
           else return snps;
       }
       public static bool SendMessage(string packet, string upl, string Networkname)
       {
           if (packet != "")
           {
               for (int i = 0; i < 1; i++)
               {
                   IInterProcessConnection clientConnection = null;
                   try
                   {
                       clientConnection = new ClientPipeConnection(upl, Networkname);
                       clientConnection.Connect();
                       clientConnection.Write(packet);
                       IPDTPApplication.ChatHistory.Add(clientConnection.Read());
                       Log.LWrite("Packet Sent From Default Server (upl://128.215.52)" + " || To " + upl + " || In Machine (" + Networkname + ")" );
                       clientConnection.Close();
                       return true;
                   }
                   catch (Exception ex)
                   {
                       clientConnection.Dispose();
                       throw (ex);
                   }
               }
           }
           else
           {

           }
               return false;
              
       }
       public static string GenerateUPL(int PID, int port)
       {
           return "upl://128." + PID.ToString() + "." + port.ToString();
       }

    }
}
