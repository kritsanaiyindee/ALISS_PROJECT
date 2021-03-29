using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.HISUpload.Batch
{
    public class CryptoHelper
    {
        public static string UnicodeEncoding(string plaintext)
        {
            string ciphertext;

            if (plaintext.Trim().ToString() != "")
            {
                byte[] encode = System.Text.Encoding.Unicode.GetBytes(plaintext);
                ciphertext = ConvertBytesToBase64String(encode);
            }
            else
            {
                ciphertext = "";
            }
            return ciphertext;
        }

        public static string UnicodeDecoding(string ciphertext)
        {
            string plaintext;
            if (ciphertext.Trim().ToString() != "")
            {
                byte[] decodeBytes = ConvertFrom64StringToBytes(ciphertext);
                plaintext = System.Text.Encoding.Unicode.GetString(decodeBytes);
            }
            else
            {
                plaintext = "";
            }          
            return plaintext;
        }

        public static byte[] ConvertFrom64StringToBytes(string src)
        {
            byte[] dest = { };
            if (src.Trim().ToString() != "")
            {
                dest = System.Convert.FromBase64String(src);
            }
            return dest;
        }

        public static string ConvertBytesToBase64String(byte[] src)
        {
            string dest;
            if (src.Length > 0)
            {
                dest = System.Convert.ToBase64String(src);
            }
            else
            {
                dest = "";
            }
            return dest;
        }
    }
}
