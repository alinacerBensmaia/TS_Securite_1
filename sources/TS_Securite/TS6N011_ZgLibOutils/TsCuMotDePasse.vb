Imports System.Security.Cryptography

''' --------------------------------------------------------------------------------
''' Project:	TS6N011_ZgLibOutils
''' Class:	TsCuMotDePasse
''' <summary>
''' Classe de génération de mot de passe.
''' </summary>
''' <remarks><para><pre>
''' Historique des modifications: 
''' 
''' --------------------------------------------------------------------------------
''' Date		Nom			Description
''' 
''' --------------------------------------------------------------------------------
''' 2005-04-21	t209376		Création initiale
''' 
''' </pre></para>
''' </remarks>
''' --------------------------------------------------------------------------------
Public Class TsCuMotDePasse
#Region " Constantes et variables "
    Private Const MINUS As Integer = 1
    Private Const MAJUS As Integer = 2
    Private Const CHIFR As Integer = 3
    Private Const SYMBO As Integer = 4

    Private strMdpMinuscules As String = "abcdefghijklmnopqrstuwxyz"
    Private strMdpMajuscules As String = "ABCDEFGHIJKLMNOPQRSTUWXYZ"
    Private strMdpChiffres As String = "0123456789"
    Private strMdpSymboles As String = "!@#$%?&*()-'^+/;:,." 'Les caractères suivant ~|{}[] sont supportés par les normes mais nous les avons retirés pour des facilités de saisie au clavier
#End Region

#Region " Propriétés privées "
    Private Property MdpMinuscules() As String
        Get
            MdpMinuscules = strMdpMinuscules
        End Get
        Set(ByVal Value As String)
            strMdpMinuscules = Value
        End Set
    End Property

    Private Property MdpMajuscules() As String
        Get
            MdpMajuscules = strMdpMajuscules
        End Get
        Set(ByVal Value As String)
            strMdpMajuscules = Value
        End Set
    End Property

    Private Property MdpChiffres() As String
        Get
            MdpChiffres = strMdpChiffres
        End Get
        Set(ByVal Value As String)
            strMdpChiffres = Value
        End Set
    End Property

    Private Property MdpSymboles() As String
        Get
            MdpSymboles = strMdpSymboles
        End Get
        Set(ByVal Value As String)
            strMdpSymboles = Value
        End Set
    End Property
#End Region

#Region " Fonctions publiques "
    ''' --------------------------------------------------------------------------------
    ''' Class.Method:	TsCuMotDePasse.GenererMotDePasse
    ''' <summary>
    ''' Fonction de génération de mot de passe.  La génération est faite en fonction
    ''' des paramètres passés à la fonction.
    ''' </summary>
    ''' <param name="intLongueur">
    ''' 	Longueur désirée du mot de passe. 
    ''' 	Value Type: <see cref="Int32" />	(System.Int32)
    ''' </param>
    ''' <param name="blnUtiliseMinuscules">
    ''' 	Inclure l'alphabet minuscule pour la génération. 
    ''' 	Value Type: <see cref="Boolean" />	(System.Boolean)
    ''' </param>
    ''' <param name="blnUtiliseMajuscules">
    ''' 	Inclure l'alphabet majuscule pour la génération. 
    ''' 	Value Type: <see cref="Boolean" />	(System.Boolean)
    ''' </param>
    ''' <param name="blnUtiliseChiffres">
    ''' 	Inclure les chiffres pour la génération. 
    ''' 	Value Type: <see cref="Boolean" />	(System.Boolean)
    ''' </param>
    ''' <param name="blnUtiliseSymboles">
    ''' 	Inclure certains symboles pour la génération. 
    ''' 	Value Type: <see cref="Boolean" />	(System.Boolean)
    ''' </param>    
    ''' <returns><see cref="String" />	(System.String)</returns>
    ''' <remarks><para><pre>
    ''' Historique des modifications: 
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' 2005-04-21	t209376		Création initiale
    ''' 
    ''' </pre></para>
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    Public Function GenererMotDePasse(ByVal intLongueur As Integer, ByVal blnUtiliseMinuscules As Boolean, _
                                    ByVal blnUtiliseMajuscules As Boolean, ByVal blnUtiliseChiffres As Boolean, _
                                    ByVal blnUtiliseSymboles As Boolean) As String

        Dim motDePasse As String = String.Empty

        'On vérifie la validité de longueur de mot de passe.  Les règles de la Régie spécifient
        'qu'un mot de passe n'aura jamais moins de 6 caractères.
        If (intLongueur < 6) Then            
            Throw New Exception("La longueur minimum du mot de passe doit être d'au moins 6 caractères.")
            'Exit Function
        End If

        Dim intTypeMDP As Integer
        If blnUtiliseMinuscules Then
            intTypeMDP = MINUS
        Else
            If Not blnUtiliseMajuscules Then                
                Throw New Exception("Un mot de passe doit au moins contenir des minuscules ou des majuscules ou les deux.")
                'Exit Function
            End If
            intTypeMDP = 10 'Pour éviter les chevauchements de possibilités
        End If
        If blnUtiliseMajuscules Then intTypeMDP += MAJUS
        If blnUtiliseChiffres Then intTypeMDP += CHIFR
        If blnUtiliseSymboles Then intTypeMDP += SYMBO

        'Tableau des types de caractères admis dans le mot de passe.
        Dim charGroups As Char()() = Nothing
        Select Case intTypeMDP
            Case 1 'MINUS
                charGroups = New Char()() {strMdpMinuscules.ToCharArray()}
            Case 3 'MINUS + MAJUS
                charGroups = New Char()() {strMdpMinuscules.ToCharArray(), strMdpMajuscules.ToCharArray()}
            Case 4 'MINUS + CHIFR
                charGroups = New Char()() {strMdpMinuscules.ToCharArray(), strMdpChiffres.ToCharArray()}
            Case 5 'MINUS + SYMBO
                charGroups = New Char()() {strMdpMinuscules.ToCharArray(), strMdpSymboles.ToCharArray()}
            Case 6 'MINUS + MAJUS + CHIFR
                charGroups = New Char()() {strMdpMinuscules.ToCharArray(), strMdpMajuscules.ToCharArray(), strMdpChiffres.ToCharArray()}
            Case 7 'MINUS + MAJUS + SYMBO
                charGroups = New Char()() {strMdpMinuscules.ToCharArray(), strMdpMajuscules.ToCharArray(), strMdpSymboles.ToCharArray()}
            Case 8 'MINUS + CHIFR + SYMBO
                charGroups = New Char()() {strMdpMinuscules.ToCharArray(), strMdpChiffres.ToCharArray(), strMdpSymboles.ToCharArray()}
            Case 10 'MINUS + MAJUS + CHIFR + SYMBO
                charGroups = New Char()() {strMdpMinuscules.ToCharArray(), strMdpMajuscules.ToCharArray(), strMdpChiffres.ToCharArray(), strMdpSymboles.ToCharArray()}
            Case 12 'MAJUS
                charGroups = New Char()() {strMdpMajuscules.ToCharArray()}
            Case 15 'MAJUS + CHIFR
                charGroups = New Char()() {strMdpMajuscules.ToCharArray(), strMdpChiffres.ToCharArray()}
            Case 16 'MAJUS + SYMBO
                charGroups = New Char()() {strMdpMajuscules.ToCharArray(), strMdpSymboles.ToCharArray()}
            Case 19 'MAJUS + CHIFR + SYMBO
                charGroups = New Char()() {strMdpMajuscules.ToCharArray(), strMdpChiffres.ToCharArray(), strMdpSymboles.ToCharArray()}
        End Select

        'Ce tableau sert à comptabiliser le nombre de caractères non-utilisés dans chaque groupe.
        Dim charsLeftInGroup As Integer() = New Integer(charGroups.Length - 1) {}

        'Initialement, tous les caractères de chaque groupe sont non-utilisés.
        For I As Integer = 0 To charsLeftInGroup.Length - 1
            charsLeftInGroup(I) = charGroups(I).Length
        Next

        'Ce tableau sert à comptabiliser le nombre de groupes non-utilisés.
        Dim leftGroupsOrder As Integer() = New Integer(charGroups.Length - 1) {}

        'Initialement, tous les groupes sont non-utilisés.
        For I As Integer = 0 To leftGroupsOrder.Length - 1
            leftGroupsOrder(I) = I
        Next

        'Parce qu'on ne peut utiliser le randomizer natif (il est basé sur l'heure courante et il pourrais produire
        'le même nombre aléatoire en une seconde), nous utiliserons un générateur de nombres pour alimenter le randomizer.

        'On remplis un tableau de bytes de 4 lignes.
        Dim randomBytes As Byte() = New Byte(3) {}

        'On génère des bytes aléatoires.
        Dim rng As RNGCryptoServiceProvider = New RNGCryptoServiceProvider
        rng.GetBytes(randomBytes)

        'On convertis les bytes en integer.
        Dim seed As Integer = ((randomBytes(0) And &H7F) << 24 Or randomBytes(1) << 16 Or randomBytes(2) << 8 Or randomBytes(3))

        'Et voilà!  Un vrai random!
        Dim random As Random = New Random(seed)

        'Ce tableau contiendra le mot de passe généré.
        Dim password As Char() = Nothing

        'Allocation de mémoire pour le mot de passe.
        password = New Char(intLongueur - 1) {}

        'Index du prochain caractère à ajouter au mot de passe.
        Dim nextCharIdx As Integer

        'Index du prochain groupe à processer.
        Dim nextGroupIdx As Integer

        'Index qui comptabilise les groupes non-processés.
        Dim nextLeftGroupsOrderIdx As Integer

        'Index du dernier caractère non-processé dans un groupe.
        Dim lastCharIdx As Integer

        'Index du dernier groupe non-processé.  Initialement, on omet les caractères spéciaux.
        Dim lastLeftGroupsOrderIdx As Integer = leftGroupsOrder.Length - 1

        'Générer le mot de passe un caractère à la fois.
        For I As Integer = 0 To password.Length - 1
            'Si il reste juste un groupe, on le processe.
            'Sinon, on prend un groupe au hasard dans la liste des groupes.
            If (lastLeftGroupsOrderIdx = 0) Then
                nextLeftGroupsOrderIdx = 0
            Else
                nextLeftGroupsOrderIdx = random.Next(0, lastLeftGroupsOrderIdx)
            End If

            'On prend l'index actuel du groupe.
            nextGroupIdx = leftGroupsOrder(nextLeftGroupsOrderIdx)

            'On prend l'index du dernier caractère non-processé dans ce groupe.
            lastCharIdx = charsLeftInGroup(nextGroupIdx) - 1

            'Si il reste seulement un caractère non-processé, on le prend.
            'Sinon, on prend un caractère au hasard dans la liste des caractères non-utilisés.
            If (lastCharIdx = 0) Then
                nextCharIdx = 0
            Else
                nextCharIdx = random.Next(0, lastCharIdx + 1)
            End If

            'Ajouter ce caractère au mot de passe.
            password(I) = charGroups(nextGroupIdx)(nextCharIdx)

            'Si on processe le dernier caractère dans ce groupe, on recommence.
            If (lastCharIdx = 0) Then
                charsLeftInGroup(nextGroupIdx) = charGroups(nextGroupIdx).Length 'Il reste des caractères non-processé.
            Else
                'Changer le caractère processé par le dernier caractère non-processé.
                'Grâce à ça on s'assure de passer à travers la collection de caractères de ce groupe.
                If (lastCharIdx <> nextCharIdx) Then
                    Dim temp As Char = charGroups(nextGroupIdx)(lastCharIdx)
                    charGroups(nextGroupIdx)(lastCharIdx) = charGroups(nextGroupIdx)(nextCharIdx)
                    charGroups(nextGroupIdx)(nextCharIdx) = temp
                End If

                'Décrémenter le nombre de caractères non-processé dans ce groupe.
                charsLeftInGroup(nextGroupIdx) = charsLeftInGroup(nextGroupIdx) - 1
            End If

            'Si on processe le dernier groupe, on recommence.
            If lastLeftGroupsOrderIdx = 0 Then
                lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1 'Il reste des groupes non-processé.
            Else
                'Changer le groupe processé par le dernier groupe non-processé.
                'Grâce à ça on s'assure de prendre un élément de chaque groupe.
                If lastLeftGroupsOrderIdx <> nextLeftGroupsOrderIdx Then
                    Dim intTempo As Integer = leftGroupsOrder(lastLeftGroupsOrderIdx)
                    leftGroupsOrder(lastLeftGroupsOrderIdx) = leftGroupsOrder(nextLeftGroupsOrderIdx)
                    leftGroupsOrder(nextLeftGroupsOrderIdx) = intTempo
                End If

                'Décrémenter le nombre de groupes non-processé.
                lastLeftGroupsOrderIdx -= 1
            End If
        Next

        'Convertir le tableau de caractères du mot de passe en string.
        Dim strNouvMDP As String
        strNouvMDP = New String(password)

        'Les normes d'injections de code ne permettent pas que les symboles & et # se suivent.
        'Si le cas se présente on relance la génération.
        If InStr(strNouvMDP, "&#", CompareMethod.Text) > 0 Then
            GenererMotDePasse(intLongueur, blnUtiliseMinuscules, blnUtiliseMajuscules, blnUtiliseChiffres, blnUtiliseSymboles)
        Else
            motDePasse = strNouvMDP
        End If

        Return motDePasse

    End Function
#End Region
End Class
