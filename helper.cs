using System;
using System.IO;
using System.Collections.Generic;

namespace hw_try_2.helper
{
    public class helper
    {
        public static byte[][] Buffer_Split(byte[] buffer, int blockSize)
        {
            byte[][] blocks = new byte[(buffer.Length + blockSize - 1) / blockSize][];

            for (int i = 0, j = 0; i < blocks.Length; i++, j += blockSize)
            {
                blocks[i] = new byte[Math.Min(blockSize, buffer.Length - j)];
                Array.Copy(buffer, j, blocks[i], 0, blocks[i].Length);
            }

            return blocks;
        }

        public static void CreateBinaryFile(byte[] encrypted, string file_name)
        {
            try
            {
                var lines = new List<string>();

                foreach (byte b in encrypted)
                {
                    var bin_str = Convert.ToString(b, 2);
                    lines.Add(bin_str);
                }


                var binary = encrypted.ToString();

                using (BinaryWriter bin_writer =
                    new BinaryWriter(File.Open(file_name, FileMode.Create)))
                {
                    lines.ForEach((l) =>
                    {
                        bin_writer.Write(l + " ");
                    });
                }
            }
            catch (IOException ioexp)
            {
                Console.WriteLine("Error: {0}", ioexp.Message);
            }
        }
    }
}
