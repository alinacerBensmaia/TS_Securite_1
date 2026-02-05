using Rrq.InfrastructureCommune.Parametres;
using System;
using System.Security.Cryptography.X509Certificates;

namespace Rrq.Securite.Certificat
{
    public static class TsCaCertificatSecurite
    {

        public static X509Certificate2 RecupererCertificatSecurite(string codeCertificat)
        {

            TsCdCertificatSecurite uad = new TsCdCertificatSecurite();
            string motPasse = uad.RecupererMotPasseCertificat(codeCertificat);

            if (string.IsNullOrWhiteSpace(motPasse))
                throw new Exception("Aucun certificat trouvé pour le code de certificat fourni.");

            // The path to the certificate.
            string cheminCertificat = string.Empty;

            if (codeCertificat.Contains(".cer") || codeCertificat.Contains(".pfx"))
            {
                cheminCertificat = XuCuPolitiqueConfig.ConfigDomaine.get_ValeurSysteme("TS1", "TS1\\TS1N241\\cheminCertificat") + codeCertificat;
                if (!System.IO.File.Exists(cheminCertificat))
                    throw new System.IO.FileNotFoundException("Le fichier " + cheminCertificat + " n'existe pas.");
            }
            else
            {
                if (codeCertificat.Contains("-PUB"))
                {
                    cheminCertificat = XuCuPolitiqueConfig.ConfigDomaine.get_ValeurSysteme("TS1", "TS1\\TS1N241\\cheminCertificat") + codeCertificat + ".cer";
                    if (!System.IO.File.Exists(cheminCertificat))
                        throw new System.IO.FileNotFoundException("Le fichier " + cheminCertificat + " n'existe pas.");
                }
                else
                {
                    cheminCertificat = XuCuPolitiqueConfig.ConfigDomaine.get_ValeurSysteme("TS1", "TS1\\TS1N241\\cheminCertificat") + codeCertificat + ".pfx";
                    if (!System.IO.File.Exists(cheminCertificat))
                        throw new System.IO.FileNotFoundException("Le fichier " + cheminCertificat + " n'existe pas.");
                }
            }


            // Load the certificate into an X509Certificate object.
            X509Certificate2 cert = new X509Certificate2(cheminCertificat, motPasse, X509KeyStorageFlags.MachineKeySet);

            return cert;

        }

        public static X509Certificate2 RecupererCertificatSecurite(string codeCertificat, string cheminCertificat)
        {

            TsCdCertificatSecurite uad = new TsCdCertificatSecurite();
            string motPasse = uad.RecupererMotPasseCertificat(codeCertificat);

            if (string.IsNullOrWhiteSpace(motPasse))
                throw new Exception("Aucun certificat trouvé pour le code de certificat fourni.");

            // The path to the certificate.
            string cheminCompletCertificat = string.Empty;

            if (codeCertificat.Contains(".cer") || codeCertificat.Contains(".pfx"))
                cheminCompletCertificat = cheminCertificat + codeCertificat;
            else
                if (codeCertificat.Contains("-PUB"))
                cheminCompletCertificat = cheminCertificat + codeCertificat + ".cer";
            else
                cheminCompletCertificat = cheminCertificat + codeCertificat + ".pfx";

            if (!System.IO.File.Exists(cheminCompletCertificat))
                throw new System.IO.FileNotFoundException("Le fichier " + cheminCompletCertificat + " n'existe pas.");

            // Load the certificate into an X509Certificate object.
            X509Certificate2 cert = new X509Certificate2(cheminCompletCertificat, motPasse, X509KeyStorageFlags.MachineKeySet);

            return cert;

        }

    }
}
