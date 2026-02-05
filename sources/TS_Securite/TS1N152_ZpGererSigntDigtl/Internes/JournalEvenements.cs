using Rq.Infrastructure.Securite.SignatureDigital.Internes.Signatures;
using Rrq.InfrastructureCommune.UtilitairesCommuns;

namespace Rq.Infrastructure.Securite.SignatureDigital.Internes
{
    internal class JournalEvenements
    {
        public void JournaliserSuppressionSignature(Signature signature)
        {
            var numeroMessage = 114;
            var nomSignature = signature.Fichier;
            var loi = signature.Loi.Nom;

            XuCuGestionEvent.AjouterEvenmSpecifique(XuGeJournalEvenement.XuGeJeSecuriteRRQ, XuGeTypeEvenement.XuGeTeInformation, numeroMessage, nomSignature, loi);
        }

        public void JournaliserSuppressionRepertoiresProfil(Utilisateur utilisateur)
        {
            var numeroMessage = 116;
            var compte = utilisateur.Identifiant;

            XuCuGestionEvent.AjouterEvenmSpecifique(XuGeJournalEvenement.XuGeJeSecuriteRRQ, XuGeTypeEvenement.XuGeTeInformation, numeroMessage, compte);
        }
    }
}
