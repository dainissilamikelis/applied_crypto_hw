using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace hw_try_2
{
    public class CFB
    {

        static byte[] Encrypt_Block(byte[] block, byte[] Key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Mode = CipherMode.CFB;
                aesAlg.Key = Key;
                aesAlg.IV = iv;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                var bytes = encryptor.TransformFinalBlock(block, 0, block.Length);
                return bytes;
            }

        }

        public static byte[] Decrypt_Block(byte[] block, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Mode = CipherMode.CFB;
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform encryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                var bytes = encryptor.TransformFinalBlock(block, 0, block.Length);
                string encrypted_text = System.Text.Encoding.UTF8.GetString(bytes);
                return bytes;
            }
        }

        public static byte[] CFB_DECRYPTION(byte[] cipher_text, byte[] key, byte[] iv)
        {


            var blocks = hw_try_2.helper.helper.Buffer_Split(cipher_text, 16);

            var decrypted = new List<byte[]>();

            foreach (var block in blocks)
            {
                dynamic iv_new = iv;
                var data = Decrypt_Block(block, key, iv_new);
                decrypted.Add(data);
               // iv_new = block;
            }

            var array_1 = decrypted.ToArray();
            var array_2 = array_1.SelectMany(i => i).ToArray();

            var result = array_2.Take(cipher_text.Length).ToArray();

            return result;

        }


        public static byte[] CFB_ENCRYPTION(string plain_text, byte[] key, byte[] iv)
        {
            var plain_text_bytes = Encoding.UTF8.GetBytes(plain_text);

            var blocks = hw_try_2.helper.helper.Buffer_Split(plain_text_bytes, 16);

            var encrypted = new List<byte[]>();


            foreach (var block in blocks)
            {
                dynamic iv_new = iv;

                var data = Encrypt_Block(block, key, iv_new);
                //iv_new = data;
                encrypted.Add(data);
            }


            var array_1 = encrypted.ToArray();
            var array_2 = array_1.SelectMany(i => i).ToArray();
            var result = array_2.Take(plain_text.Length).ToArray();

            return result;
        }
    }
}
