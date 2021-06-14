using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace hw.cts
{
    public static class aes_cts
    {

        public static byte[] Encrypt(byte[] plain_text, byte[] key, byte[] iv)
        {
            int pad_size = 16 - (plain_text.Length % 16);
            if (plain_text.Length < 16) return plain_text;

            if (pad_size == 16)
            {
                var encrypted = Encrypt_Internal(plain_text, key, plain_text.Length > 16 ? iv : new byte[iv.Length]);


                if (plain_text.Length >= 32) Swap_Blocks(encrypted);


                return encrypted;

            }
            else
            {
                byte[] copy = new byte[plain_text.Length + pad_size];
                Buffer.BlockCopy(plain_text, 0, copy, 0, plain_text.Length);

                // Pad latest plain text block with 0s.
                for (int i = 0; i < pad_size; i++)
                {
                    copy[copy.Length - pad_size + i] = 0;
                }

                var encrypted = Encrypt_Internal(copy, key, iv);


                if (encrypted.Length >= 32) Swap_Blocks(encrypted);


                return encrypted.Take(plain_text.Length).ToArray();
            }
        }

        public static byte[] Decrypt(byte[] cipher_text, byte[] key, byte[] iv)
        {
            int key_size = key.Length;
            int pad_size = 16 - (cipher_text.Length % 16);


            if (cipher_text.Length < 16) return cipher_text;


            if (pad_size == 16)
            {
                byte[] copy = new byte[cipher_text.Length];
                Buffer.BlockCopy(cipher_text, 0, copy, 0, cipher_text.Length);


                if (cipher_text.Length >= 32) Swap_Blocks(copy);


                return Decrypt_Internal(copy, key, copy.Length == 16 ? new byte[iv.Length] : iv);

            }
            else
            {
                byte[] dn = Decrypt_Internal(cipher_text.Skip(cipher_text.Length - 32 + pad_size).Take(16).ToArray(), key, new byte[iv.Length]);

                byte[] copy = new byte[cipher_text.Length + pad_size];
                Buffer.BlockCopy(cipher_text, 0, copy, 0, cipher_text.Length);

                Buffer.BlockCopy(dn, dn.Length - pad_size, copy, cipher_text.Length, pad_size);


                Swap_Blocks(copy);

                return Decrypt_Internal(copy, key, iv).Take(cipher_text.Length).ToArray();
            }
        }

        private static byte[] Decrypt_Internal(byte[] data, byte[] key, byte[] iv)
        {
            using (AesManaged aes = new AesManaged())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.None;
                using (ICryptoTransform decryptor = aes.CreateDecryptor(key, iv))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        using (Stream s = new CryptoStream(m, decryptor, CryptoStreamMode.Write))
                        {
                            s.Write(data, 0, data.Length);
                        }

                        return m.ToArray();
                    }
                }
            }
        }

        private static byte[] Encrypt_Internal(byte[] data, byte[] key, byte[] iv)
        {
            using (Aes algorithm = Aes.Create())
            {
                algorithm.Padding = PaddingMode.None;
                algorithm.Mode = CipherMode.CBC;
                using (ICryptoTransform encryptor = algorithm.CreateEncryptor(key, iv))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        using (Stream s = new CryptoStream(m, encryptor, CryptoStreamMode.Write))
                        {
                            s.Write(data, 0, data.Length);
                        }
                        return m.ToArray();
                    }
                }
            }
        }

        private static void Swap_Blocks(byte[] data)
        {
            for (int i = 0; i < 16; i++)
            {
                byte temp = data[i + data.Length - 32];
                data[i + data.Length - 32] = data[i + data.Length - 16];
                data[i + data.Length - 16] = temp;
            }
        }

    }
}
