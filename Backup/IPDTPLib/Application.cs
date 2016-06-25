using System;
using System.Collections.Generic;
using System.Text;
using IPDTP.IPC;
using System.Windows.Forms;

namespace IPDTPLib
{
   public static class IPDTPApplication
    {
            internal static List<string> ChatHistory;
      internal static PipeManager PipeManager;
      public static Dictionary<string, string> SPNS;
       public static void Initialize()
       {
           SPNS = new Dictionary<string, string>();
           PacketBuilder.Init();
           PipeManager = new PipeManager();
           PipeManager.Initialize();
           ChatHistory = new List<string>();
           PipeManager.PacketReceived += new PacketReceivedHandler(PipeManager_PacketReceived);
          
       }
       static void PipeManager_PacketReceived(PacketReceivedArgs e)
       {
           Log.LWrite("Packet Received From " + e.Packet.SourceAdress + " || Delivered To " + e.Packet.DestinationAdress + " || In Machine (" + e.Packet.NetworkName + ") || Packet Size : " + e.PacketCapsule.Length.ToString() + " || Content Type : " + e.Packet.DataType + " || Encryption : " + e.Packet.DataEncryptionType.ToString());
           Connection con = PacketBuilder.GetConnectionContentFormByte(e.Packet.Content, e.Packet.ContentEncoding);

           if (e.Packet.DestinationAdress == "upl://128.228.52")
              Router.Register(con.Content, e.Packet.SourceAdress);
           else
               Router.SendMessage(e.PacketCapsule, e.Packet.DestinationAdress, e.Packet.NetworkName);

       }
       public static void Stop()
       {
           try
           {
               PipeManager.Stop();
           }
           catch(Exception ex)
           {
               Log.Write(ex);
           }
       }
      }
}
