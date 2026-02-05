Imports System.Collections.Generic
Imports TS6N631_ZpTrtParmChif
Imports TS6N631_ZpTrtParmChif.TsCuParamsChiffrement

Public Class TsCuGestFichierChif
    Implements IGestFichierChif

#Region "Propriétés de la classe"

    Public Property Environnement As TsCuParamsChiffrement.Envrn Implements IGestFichierChif.Environnement

    Public Overridable Function ObtenirNomFichierServeurInterne(typeFichier As TypeFichier) As String Implements IGestFichierChif.ObtenirNomFichierServeurInterne
        Return TsCuParamsChiffrement.NomFichierServeurInterne(Environnement, typeFichier)
    End Function

    Public Overridable Function ObtenirNomFichierServeurExtranet(typeFichier As TypeFichier) As String Implements IGestFichierChif.ObtenirNomFichierServeurExtranet
        Return TsCuParamsChiffrement.NomFichierServeurExtranet(Environnement, typeFichier)
    End Function

    Public Overridable Function ObtenirNomFichierServeurInforoute(typeFichier As TypeFichier) As String Implements IGestFichierChif.ObtenirNomFichierServeurInforoute
        Return TsCuParamsChiffrement.NomFichierServeurInforoute(Environnement, typeFichier)
    End Function

    Public Property ListeDependances As List(Of IGestFichierChif) Implements IGestFichierChif.ListeDependances

#End Region

#Region "Instances de classes de gestion des fichiers de chiffrement"


    Private objCuTrtParmtChiff As New TS6N631_ZpTrtParmChif.tsCuTrtParmChif
    Private objCuTrtParmtSel As New TS6N631_ZpTrtParmChif.tsCuTrtParmtSel
    Private objCuTrtParmtCertf As New TS6N631_ZpTrtParmChif.tsCuTrtParmtCertf

    Private Function ObjTrtParmt(typeFichier As TypeFichier) As tsCuTrtParmt
        Dim objParmt As tsCuTrtParmt

        Select Case typeFichier
            Case TsCuParamsChiffrement.TypeFichier.Certificat
                objParmt = objCuTrtParmtCertf
            Case TsCuParamsChiffrement.TypeFichier.Chiffrement
                objParmt = objCuTrtParmtChiff
            Case TsCuParamsChiffrement.TypeFichier.Sel
                objParmt = objCuTrtParmtSel
            Case Else
                Throw New Exception("Type de fichier inconnu")
        End Select

        objParmt.ChargerSource(ObtenirNomFichierServeurInterne(typeFichier))
        Return objParmt

    End Function

#End Region


#Region "Constructeur et variables locales"

    Private _NomFichier As String

    Public Sub New(pEnv As TsCuParamsChiffrement.Envrn)
        Environnement = pEnv
        ListeDependances = New List(Of IGestFichierChif)
    End Sub

#End Region

#Region "Méthodes publiques"

    ''' <summary>
    ''' Permet d'ajouter une dépendance entre deux environnments, par exemple entre Unit et Intg
    ''' Quand on met à jour Unit, il faut aussi faire la modif en Intg, et dans tous les autres environnements dépendants.
    ''' </summary>
    ''' <param name="dependance"></param>
    Public Sub AjouterDependance(dependance As IGestFichierChif) Implements IGestFichierChif.AjouterDependance
        If ListeDependances Is Nothing Then
            ListeDependances = New List(Of IGestFichierChif)
        End If
        ListeDependances.Add(dependance)
    End Sub

    ''' <summary>
    ''' Lire le fichier source selon les paramètres saisis dans la fenêtre principale
    ''' </summary>
    ''' <returns></returns>
    Public Function ObtenirSource(type As TypeCle, typeFichier As TypeFichier) As DataRow() Implements IGestFichierChif.ObtenirSource
        Try
            Return ObjTrtParmt(typeFichier).ObtenirSource(type)
        Catch ex As Exception
            Throw New TsCuExceptionErreur("Erreur dans environnement " & Me.Environnement.ToString & ":" & vbCrLf & ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' Ajouter une entrée dans le fichier courant
    ''' </summary>
    ''' <param name="recordAAjouter"></param>
    ''' <returns></returns>
    Public Function Ajouter(ByVal recordAAjouter As DataRow, typeCle As TypeCle, typeFichier As TypeFichier) As Boolean Implements IGestFichierChif.Ajouter
        Try
            Dim obj As tsCuTrtParmt = ObjTrtParmt(typeFichier)
            Dim retour As Boolean = obj.Ajouter(typeCle, recordAAjouter, ObtenirNomFichierServeurExtranet(typeFichier), ObtenirNomFichierServeurInterne(typeFichier), ObtenirNomFichierServeurInforoute(typeFichier))

            If typeFichier = TypeFichier.Chiffrement AndAlso ListeDependances.Count > 0 Then
                For Each uneDependance As IGestFichierChif In ListeDependances
                    uneDependance.Copier(recordAAjouter, typeCle, typeFichier)
                Next
            End If
        Catch exErr631 As TsCuExceptionErreur
            Throw
        Catch ex As Exception
            Throw New TsCuExceptionErreur("Erreur dans environnement " & Me.Environnement.ToString & ":" & vbCrLf & ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' Obtenir une clé vide selon bon format de fichier courant
    ''' </summary>
    ''' <returns></returns>
    Public Function ObtenirNouvlCleVecteur(typeFichier As TypeFichier) As DataRow Implements IGestFichierChif.ObtenirNouvlCleVecteur
        Select Case typeFichier
            Case TsCuParamsChiffrement.TypeFichier.Certificat
                Return objCuTrtParmtCertf.ObtenirNouvlCertificat
            Case TsCuParamsChiffrement.TypeFichier.Chiffrement
                Return ObjCuTrtParmtChiff.ObtenirNouvlCleVecteur
            Case TsCuParamsChiffrement.TypeFichier.Sel
                Return objCuTrtParmtSel.ObtenirNouveauSel
            Case Else
                Return Nothing
        End Select
    End Function

    ''' <summary>
    ''' Écriture des modifications apportées sur disque.
    ''' </summary>
    ''' <param name="majInterneCommun"></param>
    ''' <param name="majExterneCommun"></param>
    ''' <returns></returns>
    Public Function MettreAJourSource(majInterneCommun As Boolean, majExterneCommun As Boolean, typeFichier As TypeFichier) As Boolean Implements IGestFichierChif.MettreAJourSource
        Try
            Return ObjTrtParmt(typeFichier).MettreAJourSource(majInterneCommun, majExterneCommun, ObtenirNomFichierServeurExtranet(typeFichier), ObtenirNomFichierServeurInterne(typeFichier), ObtenirNomFichierServeurInforoute(typeFichier))
        Catch ex As Exception
            Throw New TsCuExceptionErreur("Erreur dans environnement " & Me.Environnement.ToString & vbCrLf & ":" & ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' Copie d'une clé d'un environnement à un autre.
    ''' </summary>
    ''' <param name="recordAAjouter"></param>
    ''' <returns></returns>
    Public Function Copier(recordAAjouter As DataRow, type As TypeCle, typeFichier As TypeFichier) As Boolean Implements IGestFichierChif.Copier
        'Me.TypeCle = type
        Try
            Return ObjTrtParmt(typeFichier).Ajouter(type, ObjTrtParmt(typeFichier).CopieCle(recordAAjouter), ObtenirNomFichierServeurExtranet(typeFichier), ObtenirNomFichierServeurInterne(typeFichier), ObtenirNomFichierServeurInforoute(typeFichier))
        Catch ex As Exception
            Throw New TsCuExceptionErreur("Erreur dans environnement " & Me.Environnement.ToString & ":" & vbCrLf & ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' Valide si un code existe déjà dans la liste des codes
    ''' </summary>
    ''' <param name="code"></param>
    ''' <returns></returns>
    Public Function CodeExiste(code As String, typeCle As TypeCle, typeFichier As TypeFichier) As Boolean Implements IGestFichierChif.CodeExiste

        Dim liste As New List(Of DataRow)
        liste.AddRange(ObtenirSource(typeCle, typeFichier))

        Dim count As String = Me.Environnement.ToString & " - " & liste.Count

        'Valider que le code n'existe pas déjà dans la liste actuelle
        Return (Not liste.Find(Function(x) String.Equals(x.Item("Code").ToString, code, StringComparison.CurrentCultureIgnoreCase) AndAlso
                                           String.Equals(x.Item("Type").ToString, typeCle.ToString, StringComparison.CurrentCultureIgnoreCase)) Is Nothing)

    End Function


#End Region

End Class
