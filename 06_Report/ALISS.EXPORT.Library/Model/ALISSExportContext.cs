using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ALISS.EXPORT.Library.Model
{
    public class ALISSExportContext
    {
        public static string GetConnectionString(string model)
        {
            var stringConnEncrypt = ConfigurationManager.ConnectionStrings[("ALISSExportApiEntities")].ConnectionString;
            //var conStr = stringConnEncrypt;
            //var conStr = Base64Decode(stringConnEncrypt);
            //Decrypt(stringConnEncrypt); 
            //"data source=vpn130843243.softether.net,31433;initial catalog=ALISS;user id=sa;password=#Control001234;MultipleActiveResultSets=True;App=EntityFramework"; 
            var connSplit = stringConnEncrypt.Split(';');
            string connProvString = "";
            for (var i =2;i<= connSplit.Count() - 1; i++)
            {
                connProvString += connSplit[i] + ";";
            }
            connProvString = connProvString.Remove(connProvString.Count() - 2, 1).Replace("provider connection string=","");
            connProvString = connProvString.Remove(0, 1);
             var conStr = connProvString; //"data source=vpn130843243.softether.net,31433;initial catalog=ALISS;user id=sa;password=#Control001234;MultipleActiveResultSets=True;App=EntityFramework";
            //byte[] bytes = System.Text.Encoding.UTF8.GetBytes(conStr);
            //var s = Convert.ToBase64String(bytes);

            var efConnection = new EntityConnectionStringBuilder();
            efConnection.Provider = "System.Data.SqlClient"; 
            efConnection.ProviderConnectionString = conStr;
            // based on whether you choose to supply the app.config connection string to the constructor
            efConnection.Metadata = string.Format("res://*/Model.{0}.csdl|res://*/Model.{0}.ssdl|res://*/Model.{0}.msl", (model));
            // Make sure the "res://*/..." matches what's already in your config file.
            return efConnection.ToString();
        }

        public static string Base64Decode(string cipherText)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(cipherText);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        private static string Decrypt(string cipherText)
        {
            try
            {
                byte[] test = Convert.FromBase64String(cipherText);
            }
            catch (Exception ex)
            {
                return cipherText;
            }
            byte[] initVectorBytes = Encoding.ASCII.GetBytes("1.q;a[z=x-spwl2,");
            byte[] saltValueBytes = Encoding.ASCII.GetBytes("dmsc-aliss");
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

            PasswordDeriveBytes password = new PasswordDeriveBytes
            (
                "dmsc-aliss",
                saltValueBytes,
                "MD5",
                12
            );

            byte[] keyBytes = password.GetBytes(256 / 8);

            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor
            (
                keyBytes,
                initVectorBytes
            );
            const int chunkSize = 64;
            byte[] plainTextBytes;
            string outputstring = cipherText;
            try
            {
                using (MemoryStream memoryStream = new MemoryStream(cipherTextBytes))
                using (MemoryStream dataOut = new MemoryStream())
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                using (var decryptedData = new BinaryReader(cryptoStream))
                {
                    byte[] buffer = new byte[chunkSize];
                    int count;
                    while ((count = decryptedData.Read(buffer, 0, buffer.Length)) != 0)
                        dataOut.Write(buffer, 0, count);

                    plainTextBytes = dataOut.ToArray();
                }
                string plainText = Encoding.UTF8.GetString(plainTextBytes);
                outputstring = plainText;
            }
            catch (CryptographicException ex)
            {
                outputstring = cipherText;
            }

            return outputstring;
        }

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
    }

    public partial class ALISSEntities
    {
        private static string conStr = ALISSExportContext.GetConnectionString("AntimicrobialGraph");
        public ALISSEntities(bool decrypt = false)
            : base(conStr)
        {

        }
    }
}
