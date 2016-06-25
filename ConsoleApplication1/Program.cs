using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string a = "aaaaaaaaaaaaaaaa44444444444444444444452555555555555555BEGIN TRANSACTION;CREATE TABLE Y (blacklistid NUMERIC, hash TEXT, urlid INTEGER PRIMARY KEY);CREATE TABLE Z (blacklistid NUMERIC, hash TEXT, urlid INTEGER PRIMARY KEY);CREATE TABLE ZERO (blacklistid NUMERIC, hash TEXT, urlid INTEGER PRIMARY KEY);CREATE TABLE ONE (blacklistid NUMERIC, hash TEXT, urlid INTEGER PRIMARY KEY);CREATE TABLE TWO (blacklistid NUMERIC, hash TEXT, urlid INTEGER PRIMARY KEY);CREATE TABLE THREE (blacklistid NUMERIC, hash TEXT, urlid INTEGER PRIMARY KEY);CREATE TABLE FOUR (blacklistid NUMERIC, hash TEXT, urlid INTEGER PRIMARY KEY)REATE TABLE FIVE (blacklistid NUMERIC, hash TEXT, urlid INTEGER PRIMARY KEY);CREATE TABLE SIX (blacklistid NUMERIC, hash TEXT, urlid INTEGER PRIMARY KEY);CREATE TABLE SEVEN (blacklistid NUMERIC, hash TEXT, urlid INTEGER PRIMARY KEY);CREATE TABLE EIGHT (blacklistid NUMERIC, hash TEXT, urlid INTEGER PRIMARY KEY);CREATE TABLE NINE (blacklistid NUMERIC, hash TEXT, urlid INTEGER PRIMARY KEY);COMMIT;5555558hhhhhhhhhhhhhhhhhhhhhhh77777777777777777777jk54l4k55p";

            byte[] Real = Encoding.UTF8.GetBytes(a);
            byte[] Compressed = Compress(Real);
            byte[] Decompressed = Decompress(Compressed);

            Console.WriteLine("Real : " +Real.LongLength);
            Console.WriteLine("Compressed : " + Compressed.LongLength);
            Console.WriteLine("Decompressed : " + Decompressed.LongLength);
             Console.Read();
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
    }
}
