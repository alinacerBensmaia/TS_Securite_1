using Rrq.InfrastructureCommune.Parametres;

namespace Rq.Infrastructure.Securite.SignatureDigital.Internes
{
    internal class AppConfiguration
    {
        public static string UtilisateursOU
        {
            get { return getValeurSysteme("OU_Utilisateurs"); }
        }

        public static string ProprietaireGenerique
        {
            get { return getValeurSysteme("Proprietaire"); }
        }

        public static string RepertoireSignatureProduction
        {
            get { return getValeurSysteme("RepertoireSignatureProduction"); }
        }

        public static string RepertoireSignatureDepot
        {
            get { return getValeurSysteme("RepertoireSignatureDepot"); }
        }

        public static string RepertoireProfilQuebec
        {
            get { return getValeurSysteme("RepertoireProfilQuebec"); }
        }
        public static string RepertoireProfilMontreal
        {
            get { return getValeurSysteme("RepertoireProfilMontreal"); }
        }
        public static string RepertoireProfilTroisRivieres
        {
            get { return getValeurSysteme("RepertoireProfilTroisRivieres"); }
        }

        public static string RepertoireCoba
        {
            get { return getValeurSysteme("RepertoireCoba"); }
        }


        private static string getValeurSysteme(string clef)
        {
            return XuCuConfiguration.get_ValeurSysteme("TS1", string.Format(@"TS1\TS1N151\{0}", clef));
        }
    }
}
