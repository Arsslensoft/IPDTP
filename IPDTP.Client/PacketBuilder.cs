using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.IO.Compression;

namespace IPDTP.Client
{

  public static class PacketBuilder
    {
      internal static string password = "KDF4TGX8";
      internal static Regex Spliter;
     internal static Regex REQS;
      public static Connection GetConnectionContentFormByte(byte[] Content, Encoding enc)
      {
         return new Connection(enc.GetString(Content), enc);
      }
    
      public static void Init()
      {
          REQS = new Regex(@"=>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
          Spliter = new Regex(@"\r\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
      }
      public static string PackageIPDTP(IPDTPacket packet)
      {
          return BuildPacket(packet.SourceAdress, packet.DestinationAdress, packet.DataType, packet.SessionID, packet.ProcessID, packet.DataEncryptionType, packet.ContentEncoding, packet.Content, packet.Networkname);
      }
      public static byte[] Compress(byte[] buffer)
      {
          MemoryStream ms = new MemoryStream();
          using (GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true))
          {
              zip.Write(buffer, 0, buffer.Length);
          }

          ms.Position = 0;
          MemoryStream outStream = new MemoryStream();

          byte[] compressed = new byte[ms.Length];
          ms.Read(compressed, 0, compressed.Length);

          byte[] gzBuffer = new byte[compressed.Length + 4];
          System.Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
          System.Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);
          return gzBuffer;
      }

      public static byte[] Decompress(byte[] gzBuffer)
      {
          using (MemoryStream ms = new MemoryStream())
          {
              int msgLength = BitConverter.ToInt32(gzBuffer, 0);
              ms.Write(gzBuffer, 4, gzBuffer.Length - 4);

              byte[] buffer = new byte[msgLength];

              ms.Position = 0;
              using (GZipStream zip = new GZipStream(ms, CompressionMode.Decompress))
              {
                  zip.Read(buffer, 0, buffer.Length);
              }

              return buffer;
          }
      }
      public static string BuildPacket(string source, string dest, string DataType, int sessionid, int processid, EncryptionType encryptiontype, Encoding encoding, byte[] Data, string NETNAME)
      {
          string contentb64 = Security.BToBase64(Data);
          StringBuilder SB = new StringBuilder();
          // Append Source and Dest = 32 bytes in max
          SB.Append(source + "\r\n");
          SB.Append(dest + "\r\n");
          // Append Hash must be in 32 bytes
          SB.Append(Security.GetMd5Hashofstring(contentb64) + "\r\n");
          // Append Data type, SID and PID
          SB.Append(DataType + "\r\n");
          SB.Append(sessionid.ToString() + "\r\n");
          SB.Append(processid.ToString() + "\r\n");
          // Append Encryption Algorithm, Encoding, Signature
          SB.Append(encryptiontype.ToString() + "\r\n");
          SB.Append(encoding.CodePage + "\r\n");
          SB.Append(NETNAME + "\r\n");
          // Append Data
          if (contentb64.Length > IPDTPRules.MaxContentSize)
          {
              return "FALSE";
          }
          else
          {
              if (encryptiontype == EncryptionType.DECRYP)
              {
                  SB.Append(contentb64);
              }
              else if (encryptiontype == EncryptionType.DPL128)
              {
                  SB.Append(Security.Encrypt(contentb64));
              }
              else
              {
                  SB.Append(Security.EncryptTripleDES(contentb64, password));
              }
          }
          return Security.ToBase64(SB.ToString());

      }
      public static IPDTPacket Open(string packet)
      {
          string decoded = Security.FromBase64(packet);
          string[] pack = Spliter.Split(decoded, 10);
          EncryptionType enct = EncryptionType.DECRYP;
          if (pack[6] == "DECRYPT")
          {
              enct = EncryptionType.DECRYP;
          }
          else if (pack[6] == "DPL128")
          {
              enct = EncryptionType.DPL128;
          }
          else
          {
              enct = EncryptionType.DPL256;
          }
          return new IPDTPacket(pack[0], pack[1], pack[2], pack[3], Int32.Parse(pack[4]), Int32.Parse(pack[5]), enct, Encoding.GetEncoding(Int32.Parse(pack[7])), pack[8], pack[9]);
      }
     public static string RequestBuilder(Process process, ConnectionDataType DataType, string Method, string Content)
      {
          StringBuilder sb = new StringBuilder();
          sb.Append(Method + "\r\n" );
          sb.Append( process.ProcessName + ":" + process.Id.ToString() + "\r\n");
          sb.Append(DataType.ToString() + "\r\n");
          sb.Append(Content);

          return sb.ToString();
      }
    }
}
