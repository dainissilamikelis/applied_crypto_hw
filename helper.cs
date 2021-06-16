using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

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


        public static byte[] Binary_Str_To_Byte(string binary)
        {
            var list = new List<byte>();
            var split = binary.Split(" ");
            foreach(var b in split)
            {
                if (b != String.Empty)
                {
                    var trimmed = b.Trim();
                    var parsed = Convert.ToByte(trimmed, 2);
                    list.Add(parsed);
                }
            }
            var arr = list.ToArray(); 
            var plain_text_bytes = System.Text.Encoding.ASCII.GetString(arr);
            return arr;
        }

        public static void Write_All_Bytes(byte[] encrypted, string file_name)
        {
            File.WriteAllBytes(file_name, encrypted);
        }

        public static void CreateBinaryFile(byte[] encrypted, string file_name)
        {
            try
            {
                var lines = new List<string>();

                foreach (byte b in encrypted)
                {
                    //var bin_str = Convert.ToString(b, 2);
                    var bin_2 = Convert.ToString(b, 2).PadLeft(8, '0');
                    lines.Add(bin_2);
                }

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
