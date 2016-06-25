using IPDTP.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace IPDTPConsole
{
    class Program
    {
        static void Main(string[] args)
        {
             Console.Write("SNPS Name :");
             AppClient.ServerName = Console.ReadLine();
            
            AppClient.Initialize(Encoding.UTF8, EncryptionType.DPL128, ".","upl://128.256."+Process.GetCurrentProcess().Id.ToString());
            AppClient.PacketReceived += new PacketReceivedHandler(AppClient_PacketReceived);
           
            Console.WriteLine("UPL :");
            string destupl = Console.ReadLine();
            while (true)
            {
                string data = Console.ReadLine();
                Sender.SendAnswer(data, destupl, ".", 0);
            }
        }

      static  void AppClient_PacketReceived(PacketReceivedArgs e)
        {
            Connection REQ = PacketBuilder.GetConnectionContentFormByte(e.Packet.Content, e.Packet.ContentEncoding);
      
            if (REQ.Method == IPDTPRules.AnswerMethod)
            {           
                Console.WriteLine("Received from " + e.Packet.SourceAdress + " data: " + REQ.Content + " Session Id "+e.Packet.SessionID);
                Sender.SendResponse("ECHO " + REQ.Content, e.Packet.SourceAdress, ".", e.Packet.SessionID);
            }
           else if (REQ.Method == "RESPONSE")
                Console.WriteLine("Received from " + e.Packet.SourceAdress + " data: " + REQ.Content);
            
         

        }

    }
}
