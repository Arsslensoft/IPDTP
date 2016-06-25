using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace IPDTP.Client
{

    public static class AppClient
    {
      internal  static PipeManager Server;
     internal static List<string> ChatHistory;
   public static string serverupl = "upl://";
   public static string ServerName = Process.GetCurrentProcess().ProcessName;
   /// <summary>
   /// Initialize All classes, Connect to the IPDTP server and Start new Server
   /// </summary>
   /// <param name="encoding">Content Encoding</param>
   /// <param name="enc">Content Encryption</param>
  public static void Initialize(Encoding encoding, EncryptionType enc)
 {
      try
      {
     PacketBuilder.Init();
     Sender.Init(encoding, enc);
     serverupl = GenerateUPL(Process.GetCurrentProcess().Id, Process.GetCurrentProcess().ProcessName.Length);
     ChatHistory = new List<string>();
     StartServer();
      }
      catch (Exception ex)
      {
          Log.Write(ex);
      }
 }

        /// <summary>
        /// Initialize All classes, Connect to the IPDTP server and Start new Server
        /// </summary>
        /// <param name="encoding">Content Encoding</param>
        /// <param name="enc">Content Encryption</param>
        /// <param name="NetworkName">The network name or machine name. the default value for the local machine is '.'</param>
  public static void Initialize(Encoding encoding, EncryptionType enc, string NetworkName)
  {
      try
      {
      PacketBuilder.Init();
      Sender.Init(encoding, enc);
      serverupl = GenerateUPL(Process.GetCurrentProcess().Id, Process.GetCurrentProcess().ProcessName.Length);
      ChatHistory = new List<string>();
      StartServer();
        }
     catch (Exception ex)
     {
         Log.Write(ex);
     }
  }

  /// <summary>
  /// Initialize All classes, Connect to the IPDTP server and Start new Server
  /// </summary>
  /// <param name="encoding">Content Encoding</param>
  /// <param name="enc">Content Encryption</param>
  /// <param name="NetworkName">The network name or machine name. the default value for the local machine is '.'</param>
  /// <param name="upl">Your UPL</param>
  public static void Initialize(Encoding encoding, EncryptionType enc, string NetworkName, string upl)
  {
      try
      {
          PacketBuilder.Init();
          Sender.Init(encoding, enc);
          serverupl = upl;
          ChatHistory = new List<string>();
          StartServer();
      }
      catch (Exception ex)
      {
          Log.Write(ex);
      }
  }

 public static string GenerateUPL(int PID, int suffix)
 {
     return "upl://128." + PID.ToString() + "." + suffix.ToString();
 }

 public static void StartServer()
 {
     try
     {
         Server = new PipeManager();
         Server.PipeName = serverupl;
         Server.PacketReceived += new PacketReceivedHandler(Server_PacketReceived);
         Server.Initialize();

         Sender.SendREAD(ServerName, "upl://128.228.52", ".");

         if (Server.Listen)
         {
             OnCON(EventArgs.Empty);
         }
         else
         {
             OnDISCON(EventArgs.Empty);
         }
     }
     catch (Exception ex)
     {
         Log.Write(ex);
     }
 }
 public static event MES Connected;
 public static event MES Disconnected;
 static void OnCON(EventArgs e)
 {
     if (Connected != null)
         Connected(e);
 }
 static void OnDISCON(EventArgs e)
 {
     if (Disconnected != null)
         Disconnected(e);
 }

 public static event PacketReceivedHandler PacketReceived;
 static void OnReceived(PacketReceivedArgs e)
 {
     if (PacketReceived != null)
         PacketReceived(e);
 }
 static void Server_PacketReceived(PacketReceivedArgs e)
 {
     OnReceived(e);
 }
 public static void StopServer()
 {
     try{
         
         Server.Stop();

         OnDISCON(EventArgs.Empty);
       }
     catch (Exception ex)
     {
         Log.Write(ex);
     }
 }
    }
}
