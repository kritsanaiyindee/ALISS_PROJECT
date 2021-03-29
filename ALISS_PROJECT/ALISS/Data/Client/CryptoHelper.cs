using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ALISS.Data.Client
{
    public class CryptoHelper
    {
        public static string EncryptMD5(string plaintext)
        {
            MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
            byte[] cipher;
            UTF8Encoding encoder = new UTF8Encoding();
            byte[] strArr;
            string ciphertext;

            try
            {
                if (plaintext.Trim().ToString() != "")
                {
                    strArr = encoder.GetBytes(plaintext);
                    cipher = MD5.ComputeHash(strArr);
                    ciphertext = System.BitConverter.ToString(cipher);
                    ciphertext = ciphertext.Replace("-", "");
                }
                else
                {
                    ciphertext = "";
                }
            }
            catch (Exception ex)
            {
                //LOG.Write_Log(ex)
                throw ex;
            }
            return ciphertext;
        }

        public static string ConvertBytesToString(byte[] cipher)
        {
            return System.BitConverter.ToString(cipher);
        }

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

            //'Dim utf8 As New UTF8Encoding
            //'Dim ciphertext As String
            //'If plaintext.Trim.ToString() <> "" Then
            //'    Dim encodedBytes As Byte() = utf8.GetBytes(plaintext)
            //'    ciphertext = ConvertBytesToBase64String(encodedBytes)
            //'Else
            //'    ciphertext = ""
            //'End If

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

            //'Dim utf8 As New UTF8Encoding
            //'Dim plaintext As String
            //'If ciphertext.Trim().ToString() <> "" Then
            //'    Dim decodeBytes As Byte() = ConvertFrom64StringToBytes(ciphertext)
            //'    plaintext = utf8.GetString(decodeBytes)
            //'Else
            //'    plaintext = ""
            //'End If

            return plaintext;
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

        public static byte[] ConvertFrom64StringToBytes(string src)
        {
            byte[] dest = { };
            if (src.Trim().ToString() != "")
            {
                dest = System.Convert.FromBase64String(src);
            }

            return dest;
        }
    }
}
