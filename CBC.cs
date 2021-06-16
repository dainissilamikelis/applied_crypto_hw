using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace hw_try_2
{
    public class CBC
    {
        static void Swap_Last_Two_Blocks(List<byte[]> list)
        {
            int  last = list.Count - 1;
            int second_to_last = list.Count - 2;

            var tmp = list.ElementAt(last);
            list[last] = list[second_to_last];
            list[second_to_last] = tmp;
        }


        static byte[] Encrypt_Block(byte[] block, byte[] Key, byte[] iv)
        {

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Key = Key;
                aesAlg.IV = iv;
                aesAlg.Padding = block.Length < 16 ? PaddingMode.Zeros : PaddingMode.None;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                var bytes = encryptor.TransformFinalBlock(block, 0, block.Length);
                return bytes;
            }

        }

        public static byte[] Decrypt_Block(byte[] block, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Key = key;
                aesAlg.IV = iv;
                aesAlg.Padding = PaddingMode.None;

                ICryptoTransform encryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                var bytes = encryptor.TransformFinalBlock(block, 0, block.Length);
                string encrypted_text = System.Text.Encoding.UTF8.GetString(bytes);
                return bytes;
            }
        }

        public static byte[] CBC_DECRYPTION(byte[] cipher_text, byte[] key, byte[] iv)
        {

            var swaped_last_two_blocks = 16 - (cipher_text.Length % 16) != 16;
      
            if (swaped_last_two_blocks)
            {
                var pad_size = 16 - (cipher_text.Length % 16);
                var blocks = hw_try_2.helper.helper.Buffer_Split(cipher_text, 16);

                var second_to_last_block = blocks[blocks.Length - 2];
                var last_block = blocks[blocks.Length - 1];

                byte[] dn = Decrypt_Block(cipher_text.Skip(cipher_text.Length - 32 + pad_size).Take(16).ToArray(), key, blocks[0]);
                var decrypt_second_to_last = Decrypt_Block(second_to_last_block, key, iv);

                byte[] padded_last_with_tail = new byte[16];

                Buffer.BlockCopy(last_block, 0, padded_last_with_tail, 0, last_block.Length);
                Buffer.BlockCopy(decrypt_second_to_last, 16 - pad_size, padded_last_with_tail, last_block.Length, pad_size);

                blocks[blocks.Length - 1] = second_to_last_block;
                blocks[blocks.Length - 2] = padded_last_with_tail;


                var decrypted = new List<byte[]>();

                foreach (var block in blocks)
                {
                    dynamic iv_new = iv;
                    var data = Decrypt_Block(block, key, iv_new);
                    decrypted.Add(data);
                    iv_new = block;
                }

                var array_1 = decrypted.ToArray();
                var array_2 = array_1.SelectMany(i => i).ToArray();

                return array_2;
            } else
            {
                var blocks = hw_try_2.helper.helper.Buffer_Split(cipher_text, 16);

                var decrypted = new List<byte[]>();

                foreach (var block in blocks)
                {
                    dynamic iv_new = iv;
                    var data = Decrypt_Block(block, key, iv_new);
                    decrypted.Add(data);
                    iv_new = block;
                }

                var array_1 = decrypted.ToArray();
                var array_2 = array_1.SelectMany(i => i).ToArray();

                var result = array_2.Take(cipher_text.Length).ToArray();

                return result;
            }




        }


        public static byte[] CBC_ENCRYPTION(string plain_text, byte[] key, byte[] iv)
        {
            var plain_text_bytes = Encoding.UTF8.GetBytes(plain_text);

            var blocks = hw_try_2.helper.helper.Buffer_Split(plain_text_bytes, 16);

            var encrypted = new List<byte[]>();

            var swap_last_two_blocks = false;

            foreach (var block in blocks)
            {
                dynamic iv_new = iv;

                var data = Encrypt_Block(block, key, iv_new);
                iv_new = data;
                encrypted.Add(data);

                if (block.Length < 16) swap_last_two_blocks = true;
            }

            if (swap_last_two_blocks)
            {
                Swap_Last_Two_Blocks(encrypted);
            } 



            var array_1 = encrypted.ToArray();
            var array_2 = array_1.SelectMany(i => i).ToArray();
            var result = array_2.Take(plain_text.Length).ToArray();

            return result;
        }
    }
}
