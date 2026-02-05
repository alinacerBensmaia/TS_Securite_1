''' <summary>
''' Connecteur de base pour les connecteurs Sage spécialisé.
''' </summary>
Public Class TsCdConnecteurSage
    Inherits TsCuConnecteurCibleIgnorable

#Region "Variables privées"

    '! Élément de l'interface IDiposable
    Protected _disposedValue As Boolean = False

    Protected accesSage As TsBaAccesSage

#End Region

#Region "Constructeurs"

    ''' <summary>
    ''' Constructeur de base.
    ''' </summary>
    ''' <param name="idCible">Identifiant du connecteur.</param>
    ''' <remarks></remarks>
    Sub New(ByVal idCible As String)
        MyBase.New(idCible)

        accesSage = New TsBaAccesSage()
    End Sub

    Sub New(ByVal IdCible As String, ByVal config As String)
        Me.New(IdCible)
        accesSage.Config = config
    End Sub

#End Region

#Region "Fonctions de services"

    ''' <summary>
    ''' Relache les ressources gérées.
    ''' </summary>
    ''' <param name="disposing">Indique si l'on doit disposé des valeurs managées par l'objet.</param>
    ''' <remarks></remarks>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If Not _disposedValue Then
            If disposing Then

            End If
        End If
        _disposedValue = True
    End Sub

    ''' <summary>
    ''' Fonction générique de service. 
    ''' Utilisé pour dépiler une liste d'éléments et faire l'application à chaque élément 
    ''' de la fonction en argument. Si une erreur survient la 2e fonction sera utiliser pour valider l'erreur.
    ''' </summary>
    ''' <typeparam name="T">Type générique d'élément de connection.</typeparam>
    ''' <param name="liste">La liste à dépiler.</param>
    ''' <param name="contnErreur">Si l'on continue en cas d'erreur.</param>
    ''' <param name="fonctionAppSage">La fonction qui traite l'élément.</param>
    ''' <returns>Si les traitement de la liste c'est fait sans problème.</returns>
    Protected Function ApplicationListe(Of T As TsCdElementConnexion)(ByVal liste As IEnumerable(Of T), ByVal contnErreur As Boolean, _
    ByVal fonctionAppSage As FonctionApplication(Of T)) As Boolean
        Dim toutOk As Boolean = True

        For Each elementT As T In liste
            If elementT.CibleAJour < TsECcCibleAJour.ExtractionAJour Then
                Dim erreur As String = fonctionAppSage(elementT)
                If erreur = "" Then
                    elementT.CibleAJour = TsECcCibleAJour.AJour
                Else
                    elementT.DescriptionErreur = erreur
                    elementT.CibleAJour = TsECcCibleAJour.PasAJour
                    toutOk = False
                    If contnErreur = False Then
                        Exit For
                    End If
                End If
            End If
        Next

        accesSage.ViderCache()

        Return toutOk
    End Function

    ''' <summary>
    ''' Fonction générique de service. 
    ''' Utilisé pour dépiler une liste d'éléments et faire une vérification à l'aide de la fonction en argument.
    ''' </summary>
    ''' <typeparam name="T">Type générique d'élément de connection.</typeparam>
    ''' <param name="liste">La liste à dépiler.</param>
    ''' <param name="fonctionSage">La fonction qui vérifie les éléments.</param>
    Protected Sub VerificationListe(Of T As TsCdElementConnexion)(ByVal liste As IEnumerable(Of T), _
    ByVal fonctionSage As FonctionVerification(Of T))

        accesSage.ViderCache()

        For Each elementT As T In liste
            If elementT.CibleAJour < TsECcCibleAJour.ExtractionAJour Then
                If fonctionSage(elementT) = True Then
                    elementT.CibleAJour = TsECcCibleAJour.AJour
                Else
                    elementT.CibleAJour = TsECcCibleAJour.PasAJour
                End If
            End If
        Next
    End Sub

    ''' <summary>
    ''' Fonction générique de service.
    ''' Utilisé pour trouver quel élément sont des modifications. 
    ''' C'est à dire qui dubite une suppression et un ajout.
    ''' </summary>
    ''' <typeparam name="T">Type générique d'élément de connection.</typeparam>
    ''' <param name="ajouts">Liste d'ajout.</param>
    ''' <param name="suppr">Liste de suppression.</param>
    ''' <param name="fonctionCle">Fonction pour obtenir une cle de hashage.</param>
    ''' <remarks></remarks>
    Protected Sub TrouverModification(Of T As TsCdElementConnexion)(ByVal ajouts As IEnumerable(Of T), _
                                                                    ByVal suppr As IEnumerable(Of T), _
                                                                    ByVal fonctionCle As FonctionConstruireCle(Of T))
        Dim tableHasher As New HashSet(Of String)()
        For Each a As T In ajouts
            tableHasher.Add(fonctionCle(a))
        Next

        For Each s As T In suppr
            If tableHasher.Contains(fonctionCle(s)) Then
                s.CibleAJour = TsECcCibleAJour.AJour
            End If
        Next

    End Sub

    ''' <summary>
    ''' Fonction de service. Fonction généraliser du traitment des erreurs Sage.
    ''' </summary>
    ''' <typeparam name="T">Type d'entrée.</typeparam>
    ''' <param name="liste">Une liste de type d'entrée. La liste peut possèder des éléments sans erreur.</param>
    ''' <param name="fonctionTraitante">Une fonction traitant les éléments de la liste d'entrée.</param>
    Protected Sub DepileurErreur(Of T As TsCdElementConnexion)(ByVal liste As IEnumerable(Of T), ByVal modeAjout As Boolean, ByVal fonctionTraitante As TraiterErreur(Of T))
        For Each elmnt As T In liste
            If elmnt.DescriptionErreur <> "" Then
                elmnt.DescriptionErreur = fonctionTraitante(elmnt, modeAjout)
            End If
        Next
    End Sub

    ''' <summary>
    ''' Fonction de service. Fonction généraliser du traitment des erreurs de ressources Sage.
    ''' </summary>
    ''' <typeparam name="T">Type d'entrée.</typeparam>
    ''' <param name="liste">Une liste de type d'entrée. La liste peut possèder des éléments sans erreur.</param>
    ''' <param name="fonctionTraitante">Une fonction traitant les éléments de la liste d'entrée.</param>
    Protected Sub DepileurErreurRessr(Of T As TsCdElementConnexion)(ByVal liste As IEnumerable(Of T), ByVal modeAjout As Boolean, ByVal idCible As String, ByVal fonctionTraitante As TraiterErreurRessr(Of T))
        For Each elmnt As T In liste
            If elmnt.DescriptionErreur <> "" Then
                elmnt.DescriptionErreur = fonctionTraitante(elmnt, modeAjout, idCible)
            End If
        Next
    End Sub

#End Region

#Region "Fonctions déleguées"

    ''' <summary>
    ''' Fonction déléguée. Une fonction généraliser pour traiter les erreurs de Sage.
    ''' </summary>
    ''' <typeparam name="T1">Type d'entrée 1.</typeparam>
    ''' <param name="element1">Élément d'entrée du type d'entrée 1.</param>
    ''' <param name="modeAjout">Est-ce un ajout.</param>
    ''' <returns>un élément du type de sortie.</returns>
    Delegate Function TraiterErreur(Of T1)(ByVal element1 As T1, ByVal modeAjout As Boolean) As String

    ''' <summary>
    ''' Fonction déléguée. Une fonction généraliser pour traiter les erreurs de ressources de Sage.
    ''' </summary>
    ''' <typeparam name="T1">Type d'entrée 1.</typeparam>
    ''' <param name="element1">Élément d'entrée du type d'entrée 1.</param>
    ''' <param name="modeAjout">Est-ce un ajout.</param>
    ''' <param name="idCible">Identificateur des cibles.</param>
    ''' <returns>un élément du type de sortie.</returns>
    Delegate Function TraiterErreurRessr(Of T1)(ByVal element1 As T1, ByVal modeAjout As Boolean, ByVal idCible As String) As String

    ''' <summary>
    ''' Fonction délèguée utilisée qui représente des fonctions qui traîtent des éléments.
    ''' </summary>
    ''' <typeparam name="T">Type de l'élément.</typeparam>
    ''' <param name="elementT">L'élément traiter.</param>
    ''' <returns>
    ''' L'élément a été traitrer correctement le retour sera "",
    ''' Sinon une descritpion de l'erreur sera retournée.
    ''' </returns>
    Delegate Function FonctionApplication(Of T)(ByVal elementT As T) As String

    ''' <summary>
    ''' Fonction délèguée utilisée qui représente des fonctions qui vérifient des éléments.
    ''' </summary>
    ''' <typeparam name="T">Type de l'élément.</typeparam>
    ''' <param name="elementT">L'élément vérifié.</param>
    ''' <returns>Vrai si l'élément est à jour, Faux sinon.</returns>
    Delegate Function FonctionVerification(Of T)(ByVal elementT As T) As Boolean

    ''' <summary>
    ''' Fonction délèguée utilisée pour construire une clé pour un dictionnaire..
    ''' </summary>
    ''' <typeparam name="T">Type de l'élément.</typeparam>
    ''' <param name="element1">L'élément 1.</param>
    ''' <returns>Une vérité.</returns>
    Delegate Function FonctionConstruireCle(Of T)(ByVal element1 As T) As String

#End Region

End Class
