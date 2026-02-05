namespace Rq.Infrastructure.Securite.SignatureDigital.Internes.Signatures
{
    internal interface ISignatureFormatter
    {
        string Format(Utilisateur utilisateur);
    }


    internal class EmployeeNumberSignatureFormatter : ISignatureFormatter
    {
        public string Format(Utilisateur utilisateur)
        {
            //02442.tif
            return string.Format("{0}.tif", utilisateur.NumeroEmploye);
        }
    }

    internal class EmployeeNameSignatureFormatter : ISignatureFormatter
    {
        //selon la longueur du nom de l'utilisateur (pas plus de 8 caractères)
        //John Smith      == JSmith.tif
        //John Smitherine == JSmitheri.tif
        public string Format(Utilisateur utilisateur)
        {
            var prenom = utilisateur.Prenom.Substring(0, 1);
            var nom = utilisateur.Nom;
            if (utilisateur.Nom.Length > 8)
                nom = utilisateur.Nom.Substring(0, 8);             

            return string.Format("{0}{1}.tif", prenom, nom);
        }
    }
}
