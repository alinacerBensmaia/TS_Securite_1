Imports Rrq.InfrastructureCommune.ScenarioTransactionnel
Imports System.Collections.Generic
Imports System.Runtime.Serialization
Imports System.Data.Objects.DataClasses
Imports System.Linq

''' <summary>
''' Information sur une clé symbolique
''' </summary>
<EdmEntityTypeAttribute(NamespaceName:="TS1N621_INiveauSecrt1", Name:="TsDtInfoCleSymbolique")> _
<DataContractAttribute(IsReference:=True)> _
<Serializable(), CLSCompliant(True)> _
<XuCuRRQDescription("Information sur une clé symbolique")> _
Public Class TsDtInfoCleSymbolique
	Inherits XuCuDonneesTravailResultat
	
#Region "Constructeurs"
	
	Public Sub New()
		'Initialisation des valeurs par défaut.
		Me.mInCreCle = False
	End Sub
	
#End Region
	
#Region "CoEnv"
	
	Private mCoEnv as String
	
	''' <summary>
	''' Code de l'environnement
	''' </summary>
	<EdmScalarPropertyAttribute(IsNullable:=False)> _ 
	<DataMemberAttribute()> _
	<XuCuRRQDescription("Code de l'environnement")> _
	Public Property CoEnv() As String
		Get
			Return Me.mCoEnv
		End Get
		Set (Byval value as String)
			If Me.mCoEnv Is Nothing OrElse Not Me.mCoEnv.Equals(value) Then
				Me.OnCoEnvChanging(value)
				Me.ReportPropertyChanging("CoEnv")
				Me.mCoEnv = StructuralObject.SetValidValue(value, True)
				Me.ReportPropertyChanged("CoEnv")
				Me.OnCoEnvChanged()
			End If
		End Set
	End Property
	
	Partial Private Sub OnCoEnvChanging(ByVal value As String)
	End Sub
	
	Partial Private Sub OnCoEnvChanged()
	End Sub
	
#End Region
	
#Region "NmUtl"
	
	Private mNmUtl as String
	
	''' <summary>
	''' Nom de l'utilisateur
	''' </summary>
	<EdmScalarPropertyAttribute(IsNullable:=False)> _ 
	<DataMemberAttribute()> _
	<XuCuRRQDescription("Nom de l'utilisateur")> _
	Public Property NmUtl() As String
		Get
			Return Me.mNmUtl
		End Get
		Set (Byval value as String)
			If Me.mNmUtl Is Nothing OrElse Not Me.mNmUtl.Equals(value) Then
				Me.OnNmUtlChanging(value)
				Me.ReportPropertyChanging("NmUtl")
				Me.mNmUtl = StructuralObject.SetValidValue(value, True)
				Me.ReportPropertyChanged("NmUtl")
				Me.OnNmUtlChanged()
			End If
		End Set
	End Property
	
	Partial Private Sub OnNmUtlChanging(ByVal value As String)
	End Sub
	
	Partial Private Sub OnNmUtlChanged()
	End Sub
	
#End Region
	
#Region "NmCom"
	
	Private mNmCom as String
	
	''' <summary>
	''' Nom du compte
	''' </summary>
	<EdmScalarPropertyAttribute(IsNullable:=False)> _ 
	<DataMemberAttribute()> _
	<XuCuRRQDescription("Nom du compte")> _
	Public Property NmCom() As String
		Get
			Return Me.mNmCom
		End Get
		Set (Byval value as String)
			If Me.mNmCom Is Nothing OrElse Not Me.mNmCom.Equals(value) Then
				Me.OnNmComChanging(value)
				Me.ReportPropertyChanging("NmCom")
				Me.mNmCom = StructuralObject.SetValidValue(value, True)
				Me.ReportPropertyChanged("NmCom")
				Me.OnNmComChanged()
			End If
		End Set
	End Property
	
	Partial Private Sub OnNmComChanging(ByVal value As String)
	End Sub
	
	Partial Private Sub OnNmComChanged()
	End Sub
	
#End Region
	
#Region "VlMotPasCle"
	
	Private mVlMotPasCle as String
	
	''' <summary>
	''' Mode de passe de la clé
	''' </summary>
	<EdmScalarPropertyAttribute(IsNullable:=False)> _ 
	<DataMemberAttribute()> _
	<XuCuRRQDescription("Mode de passe de la clé")> _
	Public Property VlMotPasCle() As String
		Get
			Return Me.mVlMotPasCle
		End Get
		Set (Byval value as String)
			If Me.mVlMotPasCle Is Nothing OrElse Not Me.mVlMotPasCle.Equals(value) Then
				Me.OnVlMotPasCleChanging(value)
				Me.ReportPropertyChanging("VlMotPasCle")
				Me.mVlMotPasCle = StructuralObject.SetValidValue(value, True)
				Me.ReportPropertyChanged("VlMotPasCle")
				Me.OnVlMotPasCleChanged()
			End If
		End Set
	End Property
	
	Partial Private Sub OnVlMotPasCleChanging(ByVal value As String)
	End Sub
	
	Partial Private Sub OnVlMotPasCleChanged()
	End Sub
	
#End Region
	
#Region "DsCle"
	
	Private mDsCle as String
	
	''' <summary>
	''' Description de la clé symbolique
	''' </summary>
	<EdmScalarPropertyAttribute(IsNullable:=False)> _ 
	<DataMemberAttribute()> _
	<XuCuRRQDescription("Description de la clé symbolique")> _
	Public Property DsCle() As String
		Get
			Return Me.mDsCle
		End Get
		Set (Byval value as String)
			If Me.mDsCle Is Nothing OrElse Not Me.mDsCle.Equals(value) Then
				Me.OnDsCleChanging(value)
				Me.ReportPropertyChanging("DsCle")
				Me.mDsCle = StructuralObject.SetValidValue(value, True)
				Me.ReportPropertyChanged("DsCle")
				Me.OnDsCleChanged()
			End If
		End Set
	End Property
	
	Partial Private Sub OnDsCleChanging(ByVal value As String)
	End Sub
	
	Partial Private Sub OnDsCleChanged()
	End Sub
	
#End Region
	
#Region "NmProUtl"
	
	Private mNmProUtl as String
	
	''' <summary>
	''' Nom du profil utilisateur
	''' </summary>
	<EdmScalarPropertyAttribute(IsNullable:=False)> _ 
	<DataMemberAttribute()> _
	<XuCuRRQDescription("Nom du profil utilisateur")> _
	Public Property NmProUtl() As String
		Get
			Return Me.mNmProUtl
		End Get
		Set (Byval value as String)
			If Me.mNmProUtl Is Nothing OrElse Not Me.mNmProUtl.Equals(value) Then
				Me.OnNmProUtlChanging(value)
				Me.ReportPropertyChanging("NmProUtl")
				Me.mNmProUtl = StructuralObject.SetValidValue(value, True)
				Me.ReportPropertyChanged("NmProUtl")
				Me.OnNmProUtlChanged()
			End If
		End Set
	End Property
	
	Partial Private Sub OnNmProUtlChanging(ByVal value As String)
	End Sub
	
	Partial Private Sub OnNmProUtlChanged()
	End Sub
	
#End Region
	
#Region "InCreCle"
	
	Private mInCreCle as Boolean
	
	''' <summary>
	''' Indicateur de création de la clé symbolique
	''' </summary>
	<EdmScalarPropertyAttribute(IsNullable:=False)> _ 
	<DataMemberAttribute()> _
	<XuCuRRQValeurParDefaut("False")> _ 
	<XuCuRRQDescription("Indicateur de création de la clé symbolique")> _
	Public Property InCreCle() As Boolean
		Get
			Return Me.mInCreCle
		End Get
		Set (Byval value as Boolean)
			If Not Me.mInCreCle.Equals(value) Then
				Me.OnInCreCleChanging(value)
				Me.ReportPropertyChanging("InCreCle")
				Me.mInCreCle = StructuralObject.SetValidValue(value)
				Me.ReportPropertyChanged("InCreCle")
				Me.OnInCreCleChanged()
			End If
		End Set
	End Property
	
	Partial Private Sub OnInCreCleChanging(ByVal value As Boolean)
	End Sub
	
	Partial Private Sub OnInCreCleChanged()
	End Sub
	
#End Region
	
End Class

''' <summary>
''' Données pour la création des comptes FTP
''' </summary>
<EdmEntityTypeAttribute(NamespaceName:="TS1N621_INiveauSecrt1", Name:="TsDtCreationComptes")> _
<DataContractAttribute(IsReference:=True)> _
<Serializable(), CLSCompliant(True)> _
<XuCuRRQDescription("Données pour la création des comptes FTP")> _
Public Class TsDtCreationComptes
	Inherits XuCuBase
	
#Region "Constructeurs"
	
	Public Sub New()
		'Initialisation des valeurs par défaut.
		Me.mInCreUni = False
		Me.mInCreInt = False
		Me.mInCreAcp = False
		Me.mInCrePrd = False
	End Sub
	
#End Region
	
#Region "VlPfxUsg"
	
	Private mVlPfxUsg as String
	
	''' <summary>
	''' Préfixe du type d'utilisateur
	''' </summary>
	<EdmScalarPropertyAttribute(IsNullable:=False)> _ 
	<DataMemberAttribute()> _
	<XuCuRRQDescription("Préfixe du type d'utilisateur")> _
	Public Property VlPfxUsg() As String
		Get
			Return Me.mVlPfxUsg
		End Get
		Set (Byval value as String)
			If Me.mVlPfxUsg Is Nothing OrElse Not Me.mVlPfxUsg.Equals(value) Then
				Me.OnVlPfxUsgChanging(value)
				Me.ReportPropertyChanging("VlPfxUsg")
				Me.mVlPfxUsg = StructuralObject.SetValidValue(value, True)
				Me.ReportPropertyChanged("VlPfxUsg")
				Me.OnVlPfxUsgChanged()
			End If
		End Set
	End Property
	
	Partial Private Sub OnVlPfxUsgChanging(ByVal value As String)
	End Sub
	
	Partial Private Sub OnVlPfxUsgChanged()
	End Sub
	
#End Region
	
#Region "VlAbrCli"
	
	Private mVlAbrCli as String
	
	''' <summary>
	''' Abréviation du nom du compte client
	''' </summary>
	<EdmScalarPropertyAttribute(IsNullable:=False)> _ 
	<DataMemberAttribute()> _
	<XuCuRRQDescription("Abréviation du nom du compte client")> _
	Public Property VlAbrCli() As String
		Get
			Return Me.mVlAbrCli
		End Get
		Set (Byval value as String)
			If Me.mVlAbrCli Is Nothing OrElse Not Me.mVlAbrCli.Equals(value) Then
				Me.OnVlAbrCliChanging(value)
				Me.ReportPropertyChanging("VlAbrCli")
				Me.mVlAbrCli = StructuralObject.SetValidValue(value, True)
				Me.ReportPropertyChanged("VlAbrCli")
				Me.OnVlAbrCliChanged()
			End If
		End Set
	End Property
	
	Partial Private Sub OnVlAbrCliChanging(ByVal value As String)
	End Sub
	
	Partial Private Sub OnVlAbrCliChanged()
	End Sub
	
#End Region
	
#Region "VlIpRac"
	
	Private mVlIpRac as String
	
	''' <summary>
	''' Ip du répertoire racine de l'utilisateur
	''' </summary>
	<EdmScalarPropertyAttribute(IsNullable:=False)> _ 
	<DataMemberAttribute()> _
	<XuCuRRQDescription("Ip du répertoire racine de l'utilisateur")> _
	Public Property VlIpRac() As String
		Get
			Return Me.mVlIpRac
		End Get
		Set (Byval value as String)
			If Me.mVlIpRac Is Nothing OrElse Not Me.mVlIpRac.Equals(value) Then
				Me.OnVlIpRacChanging(value)
				Me.ReportPropertyChanging("VlIpRac")
				Me.mVlIpRac = StructuralObject.SetValidValue(value, True)
				Me.ReportPropertyChanged("VlIpRac")
				Me.OnVlIpRacChanged()
			End If
		End Set
	End Property
	
	Partial Private Sub OnVlIpRacChanging(ByVal value As String)
	End Sub
	
	Partial Private Sub OnVlIpRacChanged()
	End Sub
	
#End Region
	
#Region "InConFtpNonSec"
	
	Private mInConFtpNonSec as Boolean
	
	''' <summary>
	''' Indicateur de connexion FTP non-sécurisée
	''' </summary>
	<EdmScalarPropertyAttribute(IsNullable:=False)> _ 
	<DataMemberAttribute()> _
	<XuCuRRQDescription("Indicateur de connexion FTP non-sécurisée")> _
	Public Property InConFtpNonSec() As Boolean
		Get
			Return Me.mInConFtpNonSec
		End Get
		Set (Byval value as Boolean)
			If Not Me.mInConFtpNonSec.Equals(value) Then
				Me.OnInConFtpNonSecChanging(value)
				Me.ReportPropertyChanging("InConFtpNonSec")
				Me.mInConFtpNonSec = StructuralObject.SetValidValue(value)
				Me.ReportPropertyChanged("InConFtpNonSec")
				Me.OnInConFtpNonSecChanged()
			End If
		End Set
	End Property
	
	Partial Private Sub OnInConFtpNonSecChanging(ByVal value As Boolean)
	End Sub
	
	Partial Private Sub OnInConFtpNonSecChanged()
	End Sub

#End Region

#Region "InCreCompteFtp"

	Private mInCreCompteFtp As Boolean

	''' <summary>
	''' Indicateur de création du compte en unitaire
	''' </summary>
	<EdmScalarPropertyAttribute(IsNullable:=False)>
	<DataMemberAttribute()>
	<XuCuRRQValeurParDefaut("False")>
	<XuCuRRQDescription("Indicateur de création du compte FTP")>
	Public Property InCreCompteFtp() As Boolean
		Get
			Return Me.mInCreCompteFtp
		End Get
		Set(ByVal value As Boolean)
			If Not Me.mInCreCompteFtp.Equals(value) Then
				Me.OnInCreCompteFtpChanging(value)
				Me.ReportPropertyChanging("InCreCompteFtp")
				Me.mInCreCompteFtp = StructuralObject.SetValidValue(value)
				Me.ReportPropertyChanged("InCreCompteFtp")
				Me.OnInCreCompteFtpChanged()
			End If
		End Set
	End Property

	Partial Private Sub OnInCreCompteFtpChanging(ByVal value As Boolean)
	End Sub

	Partial Private Sub OnInCreCompteFtpChanged()
	End Sub

#End Region

#Region "InCreUni"

	Private mInCreUni as Boolean
	
	''' <summary>
	''' Indicateur de création du compte en unitaire
	''' </summary>
	<EdmScalarPropertyAttribute(IsNullable:=False)> _ 
	<DataMemberAttribute()> _
	<XuCuRRQValeurParDefaut("False")> _ 
	<XuCuRRQDescription("Indicateur de création du compte en unitaire")> _
	Public Property InCreUni() As Boolean
		Get
			Return Me.mInCreUni
		End Get
		Set (Byval value as Boolean)
			If Not Me.mInCreUni.Equals(value) Then
				Me.OnInCreUniChanging(value)
				Me.ReportPropertyChanging("InCreUni")
				Me.mInCreUni = StructuralObject.SetValidValue(value)
				Me.ReportPropertyChanged("InCreUni")
				Me.OnInCreUniChanged()
			End If
		End Set
	End Property
	
	Partial Private Sub OnInCreUniChanging(ByVal value As Boolean)
	End Sub
	
	Partial Private Sub OnInCreUniChanged()
	End Sub
	
#End Region
	
#Region "InCreInt"
	
	Private mInCreInt as Boolean
	
	''' <summary>
	''' Indicateur de création du compte en intégration
	''' </summary>
	<EdmScalarPropertyAttribute(IsNullable:=False)> _ 
	<DataMemberAttribute()> _
	<XuCuRRQValeurParDefaut("False")> _ 
	<XuCuRRQDescription("Indicateur de création du compte en intégration")> _
	Public Property InCreInt() As Boolean
		Get
			Return Me.mInCreInt
		End Get
		Set (Byval value as Boolean)
			If Not Me.mInCreInt.Equals(value) Then
				Me.OnInCreIntChanging(value)
				Me.ReportPropertyChanging("InCreInt")
				Me.mInCreInt = StructuralObject.SetValidValue(value)
				Me.ReportPropertyChanged("InCreInt")
				Me.OnInCreIntChanged()
			End If
		End Set
	End Property
	
	Partial Private Sub OnInCreIntChanging(ByVal value As Boolean)
	End Sub
	
	Partial Private Sub OnInCreIntChanged()
	End Sub
	
#End Region
	
#Region "InCreAcp"
	
	Private mInCreAcp as Boolean
	
	''' <summary>
	''' Indicateur de création du compte en acceptation
	''' </summary>
	<EdmScalarPropertyAttribute(IsNullable:=False)> _ 
	<DataMemberAttribute()> _
	<XuCuRRQValeurParDefaut("False")> _ 
	<XuCuRRQDescription("Indicateur de création du compte en acceptation")> _
	Public Property InCreAcp() As Boolean
		Get
			Return Me.mInCreAcp
		End Get
		Set (Byval value as Boolean)
			If Not Me.mInCreAcp.Equals(value) Then
				Me.OnInCreAcpChanging(value)
				Me.ReportPropertyChanging("InCreAcp")
				Me.mInCreAcp = StructuralObject.SetValidValue(value)
				Me.ReportPropertyChanged("InCreAcp")
				Me.OnInCreAcpChanged()
			End If
		End Set
	End Property
	
	Partial Private Sub OnInCreAcpChanging(ByVal value As Boolean)
	End Sub
	
	Partial Private Sub OnInCreAcpChanged()
	End Sub
	
#End Region
	
#Region "InCrePrd"
	
	Private mInCrePrd as Boolean
	
	''' <summary>
	''' Indicateur de création du compte en production
	''' </summary>
	<EdmScalarPropertyAttribute(IsNullable:=False)> _ 
	<DataMemberAttribute()> _
	<XuCuRRQValeurParDefaut("False")> _ 
	<XuCuRRQDescription("Indicateur de création du compte en production")> _
	Public Property InCrePrd() As Boolean
		Get
			Return Me.mInCrePrd
		End Get
		Set (Byval value as Boolean)
			If Not Me.mInCrePrd.Equals(value) Then
				Me.OnInCrePrdChanging(value)
				Me.ReportPropertyChanging("InCrePrd")
				Me.mInCrePrd = StructuralObject.SetValidValue(value)
				Me.ReportPropertyChanged("InCrePrd")
				Me.OnInCrePrdChanged()
			End If
		End Set
	End Property
	
	Partial Private Sub OnInCrePrdChanging(ByVal value As Boolean)
	End Sub
	
	Partial Private Sub OnInCrePrdChanged()
	End Sub
	
#End Region
	
End Class

