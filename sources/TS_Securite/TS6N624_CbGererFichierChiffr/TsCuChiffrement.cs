using Rrq.InfrastructureCommune.Parametres;
using Rrq.Securite.Certificat;
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Rrq.Securite.ParametreChiffrement
{
	internal class TsCuChiffrement
    {
		const string cUNIT = "\\UNIT\\";
		const string cINTG = "\\INTG\\";
		const string cACCP = "\\ACCP\\";
		const string cFORA = "\\FORA\\";
		const string cSIMI = "\\SIMI\\";
		const string cSIML = "\\SIML\\";
		const string cPROD = "\\PROD\\";

		protected internal static byte[] DecryptKey(byte[] keyBytes)
		{
			var cert = ObtenirCertificat();
			var privateKey = cert.GetRSAPrivateKey();
			//Decrypt the key with the same padding used to encrypt it.
			return privateKey.Decrypt(keyBytes, RSAEncryptionPadding.OaepSHA512);
		}

		protected internal static X509Certificate2 ObtenirCertificat()
		{
			string Thumbprint = XuCuPolitiqueConfig.ConfigDomaine.get_ObtenirValeurSystemeOptionnelle("TS6", "TS6\\TS6N624\\ThumbPrintCertificat");
			if (Thumbprint != null)
			{
				X509Store x509Store = new X509Store(StoreLocation.LocalMachine);
				x509Store.Open(OpenFlags.ReadOnly);
				X509Certificate2Collection certx509Collection = x509Store.Certificates;

				foreach (var X509Cert in certx509Collection)
				{
					if (X509Cert.Thumbprint.Equals(Thumbprint.Replace(" ", string.Empty), StringComparison.CurrentCultureIgnoreCase))
					{
						return X509Cert;
					}

				}
			}

			//Si on arrive ici, cela veut dire que le certificat n'est pas installé sur la machine, on recupère le certificat qui est sur le réseau
			String NomFichierCertificat = XuCuPolitiqueConfig.ConfigDomaine.get_ValeurSysteme("TS6", "TS6\\TS6N624\\NomFichierCertificat");

			return TsCaCertificatSecurite.RecupererCertificatSecurite(NomFichierCertificat, XuCuPolitiqueConfig.ConfigDomaine.get_ValeurSysteme("TS6", "TS6\\TS6N624\\CheminCertificat"));

		}

		protected internal static X509Certificate2 ObtenirCertificatEnvironnement(string cheminFichier)
		{
			String NomFichierCertificat = String.Empty;
			string chFichierUP = cheminFichier.ToUpper();


			if (chFichierUP.Contains(cPROD))
			{
				NomFichierCertificat = XuCuPolitiqueConfig.ConfigDomaine.get_ValeurSysteme("TS6", "TS6\\TS6N624\\NomFichierCertificatPubliquePROD");
				return TsCaCertificatSecurite.RecupererCertificatSecurite(NomFichierCertificat, XuCuPolitiqueConfig.ConfigDomaine.get_ValeurSysteme("TS6", "TS6\\TS6N624\\CheminCertificat"));
			}

			if (chFichierUP.Contains(cSIML))
			{
				NomFichierCertificat = XuCuPolitiqueConfig.ConfigDomaine.get_ValeurSysteme("TS6", "TS6\\TS6N624\\NomFichierCertificatPubliqueSIML");
				return TsCaCertificatSecurite.RecupererCertificatSecurite(NomFichierCertificat, XuCuPolitiqueConfig.ConfigDomaine.get_ValeurSysteme("TS6", "TS6\\TS6N624\\CheminCertificat"));
			}

			if (chFichierUP.Contains(cUNIT) || chFichierUP.Contains(cINTG) || chFichierUP.Contains(cACCP) || chFichierUP.Contains(cFORA) || chFichierUP.Contains(cSIMI))
			{
				NomFichierCertificat = XuCuPolitiqueConfig.ConfigDomaine.get_ValeurSysteme("TS6", "TS6\\TS6N624\\NomFichierCertificatPubliqueDEV");
				return TsCaCertificatSecurite.RecupererCertificatSecurite(NomFichierCertificat, XuCuPolitiqueConfig.ConfigDomaine.get_ValeurSysteme("TS6", "TS6\\TS6N624\\CheminCertificat"));
			}
			else
				throw new System.ApplicationException("Code d'environnement introuvable dans le chemin réseau " + cheminFichier);

		}

		protected internal static byte[] EncryptKey(byte[] key, string cheminFichier)
		{

			var cert = ObtenirCertificatEnvironnement(cheminFichier);

			var publicKey = cert.GetRSAPublicKey();
			//Encrypt the key with certificate
			return publicKey.Encrypt(key, RSAEncryptionPadding.OaepSHA512);
		}
	}
}
