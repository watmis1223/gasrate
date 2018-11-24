using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace CalculationOilPrice.Library.Util
{
    public class Zipper
    {
        public static void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }

        public static string Zip(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);

            //var memoryStream = new MemoryStream();
            //using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
            //{
            //    gZipStream.Write(bytes, 0, bytes.Length);
            //}

            //memoryStream.Position = 0;

            //var compressedData = new byte[memoryStream.Length];
            //memoryStream.Read(compressedData, 0, compressedData.Length);

            //var gZipBuffer = new byte[compressedData.Length + 4];
            //Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
            //Buffer.BlockCopy(BitConverter.GetBytes(bytes.Length), 0, gZipBuffer, 0, 4);

            string sCompress = String.Empty;
            using (var msi = new MemoryStream(bytes))
            {
                using (var mso = new MemoryStream())
                {
                    using (var gs = new GZipStream(mso, CompressionMode.Compress))
                    {
                        //msi.CopyTo(gs);
                        CopyTo(msi, gs);
                    }

                    sCompress = Convert.ToBase64String(mso.ToArray());

                    //return mso.ToArray();
                }

            }
            //return Convert.ToBase64String(gZipBuffer);

            return sCompress;
        }

        public static string Unzip(byte[] bytes)
        {            
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    //gs.CopyTo(mso);
                    CopyTo(gs, mso);
                }

                return Encoding.UTF8.GetString(mso.ToArray());
            }
        }
    }
}
