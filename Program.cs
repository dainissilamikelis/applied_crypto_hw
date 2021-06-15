using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;

namespace hw_try_2
{
    class Program
    {
      
        static byte[] Task_1_Key()
        {
            Console.WriteLine("Press K to enter 32 character key!");
            Console.WriteLine("Press P to enter a path to 32 character key!");

            string key_text = String.Empty;
            var key_p = Console.ReadLine();
            if (key_p == "K")
            {
                key_text = Console.ReadLine();
                if (key_text.Length != 32)
                {
                    Console.WriteLine("Key has to be 32 charters long!");
                    Task_1_Key();
                }
            }
            else if (key_p == "P")
            {
                string path = String.Format("{0}", key_p);
                string[] lines = System.IO.File.ReadAllLines(@path);
                key_text = lines[0];
            }
            else if (key_p == "Q")
            {
                Main_Dialog();
            }
            else if (key_p == "DS")
            {
                key_text = "12345678901234567890123456789012";
            }
            else
            {
                Task_1_Key();
            }
            var key = Encoding.UTF8.GetBytes(key_text);
            return key;  
        }


        static string Task_1_Text()
        {
            Console.WriteLine("Press T to enter text");
            Console.WriteLine("Press P to enter a path to text!");

            string text = String.Empty;
            var key_p = Console.ReadLine();
            if (key_p == "T")
            {
                text = Console.ReadLine();
                if (text.Length >= 8)
                {
                    Console.WriteLine("Text has to be  atleast 8 charters long!");
                    Task_1_Text();
                }
            }
            else if (key_p == "P")
            {
                string path = String.Format("{0}", key_p);
                string[] lines = System.IO.File.ReadAllLines(@path);
                text = lines[0];
            }
            else if (key_p == "Q")
            {
                Main_Dialog();
            }
            else if (key_p == "B-1")
            {
                text = "this should be o";
            }
            else if (key_p == "B-1.5")
            {
                text = "this should be one block not";
            }
            else if (key_p == "B-2")
            {
                text = "this should be one block not two";
            }
            else if (key_p == "B-3")
            {
                text = "this should be one block not two or almost three";
            }
            else if(key_p == "B-3.5")
            {
                text = "this should be one block not two or almost three or more";
            }
            else
            {
                Task_1_Text();
            }
            Console.WriteLine("Entered text is - {0}", text);
            return text;
        }

        static bool Task_1_Encrypt()
        {
            Console.WriteLine("Press 1 to encrypt !");
            Console.WriteLine("Press 2 to decrypt !");

            string text = String.Empty;
            var key_p = Console.ReadLine();
            if (key_p == "1")
            {
                return true;
            }
            else if (key_p == "2")
            {
                return false;
            }
            else
            {
                Task_1_Encrypt();
                return false;
            }
        }

        static void Task_1()
        {
            var choice = Task_1_Encrypt();

        

            var key = Task_1_Key();
            var text = Task_1_Text();

            if (choice)
            {
                byte[] iv = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                var encrypted = hw_try_2.CBC.CBC_ENCRYPTION(text, key, iv);

                // Console.WriteLine("BASE 64 cipher text {0}", Convert.ToBase64String(encrypted));
                Console.WriteLine("Binary stored in file hw_1_1_cipher", Convert.ToBase64String(encrypted));

                hw_try_2.helper.helper.CreateBinaryFile(encrypted, "hw_1_1_cipher");
            } else
            {
                var encrypted_bytes = 
                var decoded = hw_try_2.CBC.CBC_DECRYPTION(encrypted_bytes, key, iv);
                hw_try_2.helper.helper.CreateBinaryFile(decoded, "hw_1_1_plain");
            }
            Main_Dialog();
        }

        static void Task_2()
        {
            var key = Task_1_Key();
            var text = Task_1_Text();



            var encrypted = hw_try_2.CFB.CFB_ENCRYPTION(text, key, iv);


            var decoded = hw_try_2.CFB.CFB_DECRYPTION(encrypted, key, iv);
            string encrypted_text = System.Text.Encoding.UTF8.GetString(decoded);


            Console.WriteLine("BASE 64 cipher text {0}", Convert.ToBase64String(encrypted));
            Console.WriteLine("Binary stored in file hw_1_1_cipher", Convert.ToBase64String(encrypted));

            hw_try_2.helper.helper.CreateBinaryFile(decoded, "hw_1_1_plain");
            hw_try_2.helper.helper.CreateBinaryFile(encrypted, "hw_1_1_cipher");

            Console.WriteLine("Decrypted text is  - {0}", encrypted_text);

            Main_Dialog();

        }


        static void Main_Dialog()
        {
            Console.WriteLine("Press 1 to start Task #1");
            Console.WriteLine("Press 2 to start Task #2");
            Console.WriteLine("Press R to restart");
            Console.WriteLine("Press Q to quite");
            var task = Console.ReadLine();

            if (task == "1")
            {
                Task_1();
            }
        }
            

        static void Main(string[] args)
        {
            Main_Dialog();
        }
    }
}
