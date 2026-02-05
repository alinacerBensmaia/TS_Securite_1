using System;
using System.Data;
using Rrq.InfrastructureCommune.ScenarioTransactionnel;


namespace TS6N626_IGererFichierChiffr
{
    public class TsCuProxyGererFichierChiffr : XuCuProxyInfra<TsIGererFichierChiffr>, IDisposable
    {
        public TsCuProxyGererFichierChiffr() : base()
        {
            _Url = "TS/exec/TS6N626_IGererFichierChiffr.TS6N626_IGererFichierChiffr.TsIGererFichierChiffr.svc/TS6N626_IGererFichierChiffr.TsIGererFichierChiffr";
        }

        public void InscrireFichierChiffre(string strNomFichier, string nomTypeDataset, string contenuFichier)
        {
            CreateChannel();

            Channel.InscrireFichierChiffre(strNomFichier, nomTypeDataset, contenuFichier);
        }

        public string ObtenirFichierChiffre(string strNomFichier, string nomTypeDataset)
        {
            CreateChannel();

            return Channel.ObtenirFichierChiffre(strNomFichier, nomTypeDataset);
        }


    }
}
