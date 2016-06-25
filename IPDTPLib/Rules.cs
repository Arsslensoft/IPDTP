using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace IPDTPLib
{
    public struct UPLAdress
    {
        public int Preffix;
        public int Middle;
        public int Suffix;

        public UPLAdress(string upl)
        {
            Preffix = Int32.Parse(upl.Remove(0, 6).Substring(0, 3));
            Middle = Int32.Parse(upl.Remove(0, 9).Substring(0, 3));
            Suffix = Int32.Parse(upl.Remove(0, 12).Substring(0, 3));

        }
    }
    public enum ConnectionDataType
    {
        IMAGE,
        STRING,
        RESX,
        INFORMATION,
        VAR
    }
    /// <summary>
    /// Represents the Request or Response Contents
    /// </summary>
    public struct Connection
    {
        public string Method;
        public string PROCESS;
        public ConnectionDataType DataType;
        public byte[] RealContent;
        public string Content;

        public Connection(string request, Encoding enc)
        {
            string[] line = PacketBuilder.Spliter.Split(request, 4);
            Method = line[0];
            PROCESS = line[1];
            if (line[2] == "IMAGE")
            {
                DataType = ConnectionDataType.IMAGE;
            }
            else if (line[2] == "STRING")
            {
                DataType = ConnectionDataType.STRING;
            }
            else if (line[2] == "RESX")
            {
                DataType = ConnectionDataType.RESX;
            }
            else if (line[2] == "VAR")
            {
                DataType = ConnectionDataType.VAR;
            }
            else
            {
                DataType = ConnectionDataType.INFORMATION;
            }
            Content = line[3];
            RealContent = enc.GetBytes(Content);
        }

    }

    /// <summary>
    /// Represents the Data Protection Layer
    /// </summary>
    public enum EncryptionType
    {
        DPL128,
        DPL256,
        DECRYP
    }

    /// <summary>
    /// The Inter-Process Data Transmission Protocol Packet Struct
    /// </summary>
    public struct IPDTPacket
    {
        public string SourceAdress;
        public string DestinationAdress;
        public string Hash;
        public string DataType;
        public int SessionID;
        public int ProcessID;
         public EncryptionType DataEncryptionType;
        public Encoding ContentEncoding;
        public string NetworkName; 
        public byte[] Content;
        public string ContentB64;
        public IPDTPacket(string source, string dest, string hash, string dataType, int sessionid, int processid, EncryptionType encryptiontype, Encoding encoding, string NETNAME, string Data)
        {
            SourceAdress = source;
            DestinationAdress = dest;
            Hash = hash;
            DataType = dataType;
            SessionID = sessionid;
            ProcessID = processid;
            DataEncryptionType = encryptiontype;
            ContentEncoding = encoding;
            NetworkName = NETNAME;
           
            if (encryptiontype == EncryptionType.DECRYP)
            {
                Content = Security.BFromBase64(Data, encoding);
                ContentB64 = Convert.ToBase64String(Content);
            }
            else if (encryptiontype == EncryptionType.DPL128)
            {
                Content = Security.BFromBase64(Security.Decrypt(Data), encoding);
                ContentB64 = Convert.ToBase64String(Content);
           
            }
            else
            {
                Content = Security.BFromBase64(Security.DecryptTripleDES(Data, PacketBuilder.password), encoding);
                ContentB64 = Convert.ToBase64String(Content);
          }

        }
    }

    public static class Security
    {
        public static string HexAsciiConvert(string hex)
        {

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i <= hex.Length - 2; i += 2)
            {

                sb.Append(Convert.ToString(Convert.ToChar(Int32.Parse(hex.Substring(i, 2),

                System.Globalization.NumberStyles.HexNumber))));

            }

            return sb.ToString();

        }
           private static string Pad(string s, int len)
        {
            string temp = s;
            for (int i = s.Length; i < len; ++i)
                temp = "0" + temp;
            return temp;
        }
        public static string DumpHex(string filename)
        {

            StringBuilder sb = new StringBuilder();
            FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            using (StreamReader sr = new StreamReader(fileStream))
            {

                string line = "";
                int nCounter = 0;
                int nOffset = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    for (int i = 0; i < line.Length; ++i)
                    {
                        int c = (int)line[i];
                        string fmt = String.Format("{0:x}", c);
                        if (fmt.Length == 1)
                            fmt = Pad(fmt, 2);
                        if (nOffset % 16 == 0)
                        {
                            string offsetFmt = nOffset.ToString();


                        }
                        sb.Append(fmt);

                        if (nCounter == 15)
                        {

                            nCounter = 0;
                        }
                        else
                            nCounter++;
                        nOffset++;
                    }
                }
                sr.Close();
            }
            return sb.ToString();

        }
        public static string ConvertToHex(string input)
        {
            StringBuilder sb = new StringBuilder();
            char[] values = input.ToCharArray();
            foreach (char letter in values)
            {
                // Get the integral value of the character.
                int value = Convert.ToInt32(letter);
                // Convert the decimal value to a hexadecimal value in string form.
                string hexOutput = String.Format("{0:x}", value);
                sb.Append(hexOutput);
            }
            return sb.ToString();
        }
   
        public static string ToBase64(string inputstring)
        {
            byte[] byt = System.Text.Encoding.UTF8.GetBytes(inputstring);

            string result = Convert.ToBase64String(byt);
            return result;
        }

        public static string BToBase64(byte[] byt)
        {
            string result = Convert.ToBase64String(byt);
            return result;
        }
        public static byte[] BFromBase64(string encodedstring, Encoding enc)
        {
            byte[] b = Convert.FromBase64String(encodedstring);


            return b;
        }

        public static string FromBase64(string encodedstring)
        {
            byte[] b = Convert.FromBase64String(encodedstring);

            string result = System.Text.Encoding.UTF8.GetString(b);
            return result;
        }
        internal static string Encrypt(string originalString)
        {

            if (String.IsNullOrEmpty(originalString))
            {
                throw new ArgumentNullException
                       ("The string which needs to be encrypted can not be null.");
            }
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                cryptoProvider.CreateEncryptor(bytes, bytes), CryptoStreamMode.Write);
            StreamWriter writer = new StreamWriter(cryptoStream);
            writer.Write(originalString);
            writer.Flush();
            cryptoStream.FlushFinalBlock();
            writer.Flush();
            return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);

        }
        internal static string Decrypt(string cryptedString)
        {

            if (String.IsNullOrEmpty(cryptedString))
            {
                throw new ArgumentNullException
                   ("The string which needs to be decrypted can not be null.");
            }
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream
                    (Convert.FromBase64String(cryptedString));
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                cryptoProvider.CreateDecryptor(bytes, bytes), CryptoStreamMode.Read);
            StreamReader reader = new StreamReader(cryptoStream);
            return reader.ReadToEnd();


        }
        static byte[] bytes = ASCIIEncoding.ASCII.GetBytes("ipdtpxnT");
        public static string GetMd5Hashofstring(string input)
        {

            // Create a new instance of the MD5CryptoServiceProvider object.
            MD5 md5Hasher = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
        public static string GetMD5HashFromFile(string fileName)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                FileStream file = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();


                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
            }
            finally
            {

            }
            return sb.ToString();

        }
        public static bool Match(string hashOfInput, string VDBhash)
        {

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, VDBhash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        /// <summary>
        /// Encrypts text with Triple DES encryption using the supplied key
        /// </summary>
        /// <param name="plaintext">The text to encrypt</param>
        /// <param name="key">Key to use for encryption</param>
        /// <returns>The encrypted string represented as base 64 text</returns>
        internal static string EncryptTripleDES(string plaintext, string key)
        {
            TripleDESCryptoServiceProvider DES =
                new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider hashMD5 = new MD5CryptoServiceProvider();
            DES.Key = hashMD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(key));
            DES.Mode = CipherMode.ECB;
            ICryptoTransform DESEncrypt = DES.CreateEncryptor();
            byte[] Buffer = ASCIIEncoding.ASCII.GetBytes(plaintext);
            return Convert.ToBase64String(
                DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
        }
        static char[] cDuoDev = new char[] { 'Z', 'Y', 'X', 'W', 'V', 'U', 'T', 'S' };
        static int[] iDuoDevNumeric = new int[] { 10, 11, 12, 13, 14, 15, 16, 17 };
        /// <summary>
        /// Convert String to DUODEV
        /// </summary>
        /// <param name="text">string to convert</param>
        /// <returns>the converted string</returns>
        internal static string ToDuoDev(string text)
        {
            char[] values = text.ToCharArray();
            StringBuilder sb = new StringBuilder();
            foreach (char letter in values)
            {
                string strBin = "";
                int iDec = Convert.ToInt32(letter);

                int[] result = new int[32];
                int MaxBit = 32;
                for (; iDec > 0; iDec /= 18)
                {
                    int rem = iDec % 18;
                    result[--MaxBit] = rem;
                }
                for (int i = 0; i < result.Length; i++)
                    if ((int)result.GetValue(i) >= 10)
                        strBin += cDuoDev[(int)result.GetValue(i) % 10];
                    else
                        strBin += result.GetValue(i);
                strBin = strBin.TrimStart(new char[] { '0' });
                sb.Append(strBin);
            }

            return sb.ToString(); ;
        }

        /// <summary>
        /// Decrypts supplied Triple DES encrypted text using the supplied key
        /// </summary>
        /// <param name="base64Text">Triple DES encrypted base64 text</param>
        /// <param name="key">Decryption Key</param>
        /// <returns>The decrypted string</returns>
        internal static string DecryptTripleDES(string base64Text, string key)
        {
            TripleDESCryptoServiceProvider DES =
                new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider hashMD5 = new MD5CryptoServiceProvider();
            DES.Key = hashMD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(key));
            DES.Mode = CipherMode.ECB;
            ICryptoTransform DESDecrypt = DES.CreateDecryptor();
            byte[] Buffer = Convert.FromBase64String(base64Text);
            return ASCIIEncoding.ASCII.GetString(
                DESDecrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
        }
    }

    /// <summary>
    /// All Basic IPDTP Rules
    /// </summary>
    public static class IPDTPRules
    {
        public const string GIVEMethod = "GIVE";
        public const string SetMethod = "SET";
        public const string ReadMethod = "READ";
        public const string AnswerMethod = "Answer";
        public const string ProcessMethod = "PROCESS";
        public const string AccessMethod = "ACCESS";
        public const string TAKEMethod = "TAKE";
        public const string RPROCESSMethod = "RPROCESS";
        public const string RACCESSMethod = "RACCESS";


        public const long MaxContentSize = 2300000;
        public const int MaxPacketSize = 2300756;



    }
}
