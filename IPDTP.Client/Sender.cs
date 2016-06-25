using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using IPDTP.IPC;
using IPDTP.COM;
using System.IO;
using System.Windows.Forms;

namespace IPDTP.Client
{
    public enum SessionState
    {
        CANCELLED,
        TIMEOUT,
        OK,
        WAITINGResponse
    }
  public static  class Sender
    {
      public static Encoding ServerEncoding;
      public static EncryptionType Encryption;
      public static Dictionary<int, SessionState> Connections;
     internal static int sessions = 0;
     public static string Networkname = ".";
     public static void SetSID(int sid, SessionState state)
     {
         try
         {
             if (Connections.ContainsKey(sid))
             {
                 Connections[sid] = state;
             }
             else
             {
                 Connections.Add(sid, state);
             }
         }
         catch (Exception ex)
         {

         }
         finally
         {

         }
     }
     public static void Init(Encoding encoding, EncryptionType encryption, string netname)
  {
      Connections = new Dictionary<int, SessionState>();
      ServerEncoding = encoding;
      Encryption = encryption;
      Networkname = netname;
  }
      public static void Init(Encoding encoding,EncryptionType encryption)
      {
          Connections = new Dictionary<int, SessionState>();
          ServerEncoding = encoding;
          Encryption = encryption;
      }
      public static Image byteArrayToImage(byte[] byteArrayIn)
      {
          MemoryStream ms = new MemoryStream(byteArrayIn);
          Image returnImage = Image.FromStream(ms);
          return returnImage;
      }
      public static byte[] imageToByteArray(System.Drawing.Image imageIn)
      {
          MemoryStream ms = new MemoryStream();
          imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
          return ms.ToArray();
      }


      public static bool SendAccess(string file, string DestinationAdress, string destnetname)
       {
           string request = PacketBuilder.RequestBuilder(Process.GetCurrentProcess(), ConnectionDataType.STRING, "ACCESS", file);
           byte[] data = ServerEncoding.GetBytes(request);
          int sid = sessions + 1;
          string packet = PacketBuilder.BuildPacket(AppClient.serverupl, DestinationAdress, "STRING", sid, Process.GetCurrentProcess().Id, Encryption, ServerEncoding, data, destnetname);
          if (packet != "")
          {
              for (int i = 0; i < 1; i++)
              {
                  IInterProcessConnection clientConnection = null;
                  try
                  {
                      clientConnection = new ClientPipeConnection("upl://128.215.52",Networkname);
                      clientConnection.Connect();
                      clientConnection.Write(packet);
                     
                      clientConnection.Close();
                      SetSID(sid, SessionState.WAITINGResponse);
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
      public static bool SendREAD(string message, string DestinationAdress, string destnetname)
      {
          string request = PacketBuilder.RequestBuilder(Process.GetCurrentProcess(), ConnectionDataType.STRING, "READ", message);
          byte[] data = ServerEncoding.GetBytes(request);
          int sid = sessions + 1;
          string packet = PacketBuilder.BuildPacket(AppClient.serverupl, DestinationAdress, "STRING", sid, Process.GetCurrentProcess().Id, Encryption, ServerEncoding, data, destnetname);
          if (packet != "")
          {
              for (int i = 0; i < 1; i++)
              {
                  IInterProcessConnection clientConnection = null;
                  try
                  {
                      clientConnection = new ClientPipeConnection("upl://128.215.52", Networkname);
                      clientConnection.Connect();
                      clientConnection.Write(packet);
                   
                      clientConnection.Close();
                      SetSID(sid, SessionState.WAITINGResponse);
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
      public static bool SendGIVEImage(string imagename, string DestinationAdress, string destnetname)
      {
          string request = PacketBuilder.RequestBuilder(Process.GetCurrentProcess(), ConnectionDataType.IMAGE, "GIVE", imagename);
          byte[] data = ServerEncoding.GetBytes(request);
          int sid = sessions + 1;
          string packet = PacketBuilder.BuildPacket(AppClient.serverupl, DestinationAdress, "IMAGE", sid, Process.GetCurrentProcess().Id, Encryption, ServerEncoding, data, destnetname);
          if (packet != "")
          {
              for (int i = 0; i < 1; i++)
              {
                  IInterProcessConnection clientConnection = null;
                  try
                  {
                      clientConnection = new ClientPipeConnection("upl://128.215.52", Networkname);
                      clientConnection.Connect();
                      clientConnection.Write(packet);

                      clientConnection.Close();
                      SetSID(sid, SessionState.WAITINGResponse);
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
      public static bool SendGIVERessource(string ressource, string DestinationAdress, string destnetname)
      {
          string request = PacketBuilder.RequestBuilder(Process.GetCurrentProcess(), ConnectionDataType.RESX, "GIVE", ressource);
          byte[] data = ServerEncoding.GetBytes(request);
          int sid = sessions + 1;
          string packet = PacketBuilder.BuildPacket(AppClient.serverupl, DestinationAdress, "RESX", sid, Process.GetCurrentProcess().Id, Encryption, ServerEncoding, data, destnetname);
          if (packet != "")
          {
              for (int i = 0; i < 1; i++)
              {
                  IInterProcessConnection clientConnection = null;
                  try
                  {
                      clientConnection = new ClientPipeConnection("upl://128.215.52", Networkname);
                      clientConnection.Connect();
                      clientConnection.Write(packet);

                      clientConnection.Close();
                      SetSID(sid, SessionState.WAITINGResponse);
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
      public static bool SendProcess(object information, string DestinationAdress, string destnetname)
      {
          string request = PacketBuilder.RequestBuilder(Process.GetCurrentProcess(), ConnectionDataType.INFORMATION, "PROCESS", information.ToString());
          byte[] data = ServerEncoding.GetBytes(request);
          int sid = sessions + 1;
          string packet = PacketBuilder.BuildPacket(AppClient.serverupl, DestinationAdress, "INFORMATION", sid, Process.GetCurrentProcess().Id, Encryption, ServerEncoding, data, destnetname);
          if (packet != "")
          {
              for (int i = 0; i < 1; i++)
              {
                  IInterProcessConnection clientConnection = null;
                  try
                  {
                      clientConnection = new ClientPipeConnection("upl://128.215.52", Networkname);
                      clientConnection.Connect();
                      clientConnection.Write(packet);

                      clientConnection.Close();
                      SetSID(sid, SessionState.WAITINGResponse);
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
      public static bool SendResponse(string message, string DestinationAdress, string destnetname, int sid)
      {
          string response = PacketBuilder.RequestBuilder(Process.GetCurrentProcess(), ConnectionDataType.STRING, "RESPONSE", message);
          byte[] data = ServerEncoding.GetBytes(response);

          string packet = PacketBuilder.BuildPacket(AppClient.serverupl, DestinationAdress, "STRING", sid, Process.GetCurrentProcess().Id, Encryption, ServerEncoding, data, destnetname);
          if (packet != "")
          {
              for (int i = 0; i < 1; i++)
              {
                  IInterProcessConnection clientConnection = null;
                  try
                  {
                      clientConnection = new ClientPipeConnection("upl://128.215.52", Networkname);
                      clientConnection.Connect();
                      clientConnection.Write(packet);

                      clientConnection.Close();
                      SetSID(sid, SessionState.OK);
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
      public static bool SendAnswer(string message, string DestinationAdress, string destnetname, int sid)
      {
          string response = PacketBuilder.RequestBuilder(Process.GetCurrentProcess(), ConnectionDataType.STRING, "ANSWER", message);
          byte[] data = ServerEncoding.GetBytes(response);

          string packet = PacketBuilder.BuildPacket(AppClient.serverupl, DestinationAdress, "STRING", sid, Process.GetCurrentProcess().Id, Encryption, ServerEncoding, data, destnetname);
          if (packet != "")
          {
              for (int i = 0; i < 1; i++)
              {
                  IInterProcessConnection clientConnection = null;
                  try
                  {
                      clientConnection = new ClientPipeConnection("upl://128.215.52", Networkname);
                      clientConnection.Connect();
                      clientConnection.Write(packet);

                      clientConnection.Close();
                      SetSID(sid, SessionState.OK);
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
      public static bool SendTakeImage(Image img, string DestinationAdress, string destnetname, int sid)
       {
          // get the image base64 string

           byte[] imagebytes = imageToByteArray(img);

       string imageb64 = System.Convert.ToBase64String(imagebytes);

          // begin response
           string response = PacketBuilder.RequestBuilder(Process.GetCurrentProcess(), ConnectionDataType.IMAGE, "TAKE", imageb64);
           byte[] data = ServerEncoding.GetBytes(response);

           string packet = PacketBuilder.BuildPacket(AppClient.serverupl, DestinationAdress, "IMAGE", sid, Process.GetCurrentProcess().Id, Encryption, ServerEncoding, data, destnetname);
           if (packet != "")
           {
               for (int i = 0; i < 1; i++)
               {
                   IInterProcessConnection clientConnection = null;
                   try
                   {
                       clientConnection = new ClientPipeConnection("upl://128.215.52", Networkname);
                       clientConnection.Connect();
                       clientConnection.Write(packet);

                       clientConnection.Close();
                       SetSID(sid, SessionState.OK);
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
      public static bool SendTakeRessource(byte[] resx, string DestinationAdress, string destnetname, int sid)
      {
          string resxb64 = System.Convert.ToBase64String(resx);

          // begin response
          string response = PacketBuilder.RequestBuilder(Process.GetCurrentProcess(), ConnectionDataType.RESX, "TAKE", resxb64);
          byte[] data = ServerEncoding.GetBytes(response);

          string packet = PacketBuilder.BuildPacket(AppClient.serverupl, DestinationAdress, "RESX", sid, Process.GetCurrentProcess().Id, Encryption, ServerEncoding, data, destnetname);
          if (packet != "")
          {
              for (int i = 0; i < 1; i++)
              {
                  IInterProcessConnection clientConnection = null;
                  try
                  {
                      clientConnection = new ClientPipeConnection("upl://128.215.52", Networkname);
                      clientConnection.Connect();
                      clientConnection.Write(packet);

                      clientConnection.Close();
                      SetSID(sid, SessionState.OK);
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
      public static bool SendSET(Type type, string propertyname, object value, string DestinationAdress, string destnetname)
      {
          string content = type.ToString() + "|"+ propertyname + "|"+ value.ToString(); 
          string request = PacketBuilder.RequestBuilder(Process.GetCurrentProcess(), ConnectionDataType.VAR, "SET", content);
          byte[] data = ServerEncoding.GetBytes(request);
          int sid = sessions + 1;
          string packet = PacketBuilder.BuildPacket(AppClient.serverupl, DestinationAdress, "VAR", sid, Process.GetCurrentProcess().Id, Encryption, ServerEncoding, data, destnetname);
          if (packet != "")
          {
              for (int i = 0; i < 1; i++)
              {
                  IInterProcessConnection clientConnection = null;
                  try
                  {
                      clientConnection = new ClientPipeConnection("upl://128.215.52", Networkname);
                      clientConnection.Connect();
                      clientConnection.Write(packet);

                      clientConnection.Close();
                      SetSID(sid, SessionState.OK);
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
      public static bool SendRProcess(string information, string DestinationAdress, string destnetname, int sid)
      {
          string response = PacketBuilder.RequestBuilder(Process.GetCurrentProcess(), ConnectionDataType.INFORMATION, "RPROCESS", information);
          byte[] data = ServerEncoding.GetBytes(response);

          string packet = PacketBuilder.BuildPacket(AppClient.serverupl, DestinationAdress, "INFORMATION", sid, Process.GetCurrentProcess().Id, Encryption, ServerEncoding, data, destnetname);
          if (packet != "")
          {
              for (int i = 0; i < 1; i++)
              {
                  IInterProcessConnection clientConnection = null;
                  try
                  {
                      clientConnection = new ClientPipeConnection("upl://128.215.52", Networkname);
                      clientConnection.Connect();
                      clientConnection.Write(packet);

                      clientConnection.Close();
                      SetSID(sid, SessionState.OK);
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
      public static bool SendRAccess(string state, string DestinationAdress, string destnetname, int sid)
      {
          string response = PacketBuilder.RequestBuilder(Process.GetCurrentProcess(), ConnectionDataType.STRING, "RACCESS", state);
          byte[] data = ServerEncoding.GetBytes(response);

          string packet = PacketBuilder.BuildPacket(AppClient.serverupl, DestinationAdress, "STRING", sid, Process.GetCurrentProcess().Id, Encryption, ServerEncoding, data, destnetname);
          if (packet != "")
          {
              for (int i = 0; i < 1; i++)
              {
                  IInterProcessConnection clientConnection = null;
                  try
                  {
                      clientConnection = new ClientPipeConnection("upl://128.215.52", Networkname);
                      clientConnection.Connect();
                      clientConnection.Write(packet);

                      clientConnection.Close();
                      SetSID(sid, SessionState.OK);
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

    }
}
