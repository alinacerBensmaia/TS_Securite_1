using System;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Rrq.Securite.ParametreChiffrement
{
	public class TsCuGererFichrParamChiffr
	{

        public void ChiffrerFichier(string filePath, string nomTypeDataSet, string contenuFichier) 
        {

            DataSet dsContenu = new DataSet();

            switch (nomTypeDataSet)
            {
                case nameof(TS6N628_DtParmChif.TsDsObtnrParmtSel):
                    {
                        dsContenu = TS6N628_DtParmChif.TsCuDataSetSerializer.Deserialize<TS6N628_DtParmChif.TsDsObtnrParmtSel>(contenuFichier);
                        break;
                    }

                case nameof(TS6N628_DtParmChif.tsDsObtParmChif):
                    {
                        dsContenu = TS6N628_DtParmChif.TsCuDataSetSerializer.Deserialize<TS6N628_DtParmChif.tsDsObtParmChif>(contenuFichier);
                        break;
                    }
                case nameof(TS6N628_DtParmChif.TsDsObtnrParmtCertf):
                    {
                        dsContenu = TS6N628_DtParmChif.TsCuDataSetSerializer.Deserialize<TS6N628_DtParmChif.TsDsObtnrParmtCertf>(contenuFichier);
                        break;
                    }
            }

            dsContenu.AcceptChanges();

            //We're using AES encryption to create my key.
            Aes aesKey = Aes.Create();
            aesKey.GenerateKey();
            byte[] ivKey = new byte[aesKey.IV.Length];
            Array.Copy(aesKey.Key, ivKey, aesKey.IV.Length);
            aesKey.IV = ivKey;
            var encryptor = aesKey.CreateEncryptor();
            var encryptedKey = TsCuChiffrement.EncryptKey(aesKey.Key, filePath);

			//We're using truncate mode, so the file opens up and is empty.
			using (var outputStream = new FileStream(filePath, FileMode.Create))
            {
				//Add the encrypted key to the start of file.
				outputStream.Write(encryptedKey, 0, encryptedKey.Length);

				var encryptStream = new CryptoStream(outputStream, encryptor, CryptoStreamMode.Write);
                dsContenu.WriteXml(encryptStream);
				encryptStream.Close();
			}
        }
                
		public String DechiffrerFichier(string filePath, string nomTypeDataSet)
		{

            DataSet dsContenu = new TS6N628_DtParmChif.TsDsObtnrParmtCertf();

            switch (nomTypeDataSet)
            {
                case nameof(TS6N628_DtParmChif.TsDsObtnrParmtSel):
                    {
                        dsContenu = new TS6N628_DtParmChif.TsDsObtnrParmtSel();
                        break;
                    }

                case nameof(TS6N628_DtParmChif.tsDsObtParmChif):
                    {
                        dsContenu = new TS6N628_DtParmChif.tsDsObtParmChif();
                        break;
                    }
            }

            //Read the file in memory so we can overwrite the source with it's original file.
            //We also need it in memory so we can extract the key.
            var file = (File.ReadAllBytes(filePath)).ToList();
			var encryptedKey = new Collection<byte>();
			//Checking the length so we know how much bytes we need to take from the file.
			//Different certificates can create different size of keys.
			var encryptLength = TsCuChiffrement.EncryptKey(Encoding.UTF8.GetBytes("string"), filePath).Length;
			//Extract the key.
			for (var i = 0; i < encryptLength; i++)
				encryptedKey.Add(file[i]);
			file.RemoveRange(0, encryptLength);

			var decryptedKey = TsCuChiffrement.DecryptKey(encryptedKey.ToArray());

			using (var managed = new AesManaged())
			{
				//I'm using AES encryption, but this time we do not generate the key but pass our decrypted key.
				Aes aesKey = Aes.Create();
				aesKey.Key = decryptedKey;
				byte[] ivKey = new byte[aesKey.IV.Length];
				Array.Copy(aesKey.Key, ivKey, aesKey.IV.Length);
				aesKey.IV = ivKey;
				var decryptor = aesKey.CreateDecryptor();

				using (var encryptedFileStream = new MemoryStream(file.ToArray()))
				{
					//We're using truncate mode, so the file opens up and is empty.
					using (var objCrypto = new CryptoStream(encryptedFileStream, decryptor, CryptoStreamMode.Read))
					{
						dsContenu.ReadXml(objCrypto);
						dsContenu.AcceptChanges();
                        return TS6N628_DtParmChif.TsCuDataSetSerializer.Serialize(dsContenu);
					}
				}

			}
		}
    }
}
