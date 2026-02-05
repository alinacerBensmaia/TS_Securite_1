Imports Rrq.InfrastructureCommune.ScenarioTransactionnel
Imports System.Collections.Generic
Imports System.Runtime.Serialization
Imports System.Data.Objects.DataClasses
Imports System.Linq

''' <summary>
''' Clé symbolique
''' </summary>
<EdmEntityTypeAttribute(NamespaceName:="TS1N201_DtCdAccGenV1", Name:="TsDtCleSym")> _
<DataContractAttribute(IsReference:=True)> _
<Serializable(), CLSCompliant(True)> _
<XuCuRRQDescription("Clé symbolique")> _
Public Class TsDtCleSym
	Inherits XuCuBase
	
#Region "Constructeurs"
	
	Public Sub New()
		'Initialisation des valeurs par défaut.
		Me.mInAjtEnv = False
	End Sub
	
#End Region
	
#Region "CoIdnCleSymTs"
	
	Private mCoIdnCleSymTs as String
	
	''' <summary>
	''' Code identifiant
	''' </summary>
	<EdmScalarPropertyAttribute()> _ 
	<DataMemberAttribute()> _
	<XuCuRRQDescription("Code identifiant")> _
	Public Property CoIdnCleSymTs() As String
		Get
			Return Me.mCoIdnCleSymTs
		End Get
		Set (Byval value as String)
			If Me.mCoIdnCleSymTs Is Nothing OrElse Not Me.mCoIdnCleSymTs.Equals(value) Then
				Me.OnCoIdnCleSymTsChanging(value)
				Me.ReportPropertyChanging("CoIdnCleSymTs")
				Me.mCoIdnCleSymTs = StructuralObject.SetValidValue(value, True)
				Me.ReportPropertyChanged("CoIdnCleSymTs")
				Me.OnCoIdnCleSymTsChanged()
			End If
		End Set
	End Property
	
	Partial Private Sub OnCoIdnCleSymTsChanging(ByVal value As String)
	End Sub
	
	Partial Private Sub OnCoIdnCleSymTsChanged()
	End Sub
	
#End Region
	
#Region "CoTypCleSymTs"
	
	Private mCoTypCleSymTs as String
	
	''' <summary>
	''' Code type
	''' </summary>
	<EdmScalarPropertyAttribute()> _ 
	<DataMemberAttribute()> _
	<XuCuRRQDescription("Code type")> _
	Public Property CoTypCleSymTs() As String
		Get
			Return Me.mCoTypCleSymTs
		End Get
		Set (Byval value as String)
			If Me.mCoTypCleSymTs Is Nothing OrElse Not Me.mCoTypCleSymTs.Equals(value) Then
				Me.OnCoTypCleSymTsChanging(value)
				Me.ReportPropertyChanging("CoTypCleSymTs")
				Me.mCoTypCleSymTs = StructuralObject.SetValidValue(value, True)
				Me.ReportPropertyChanged("CoTypCleSymTs")
				Me.OnCoTypCleSymTsChanged()
			End If
		End Set
	End Property
	
	Partial Private Sub OnCoTypCleSymTsChanging(ByVal value As String)
	End Sub
	
	Partial Private Sub OnCoTypCleSymTsChanged()
	End Sub
	
#End Region
	
#Region "CoEnvCleSymTs"
	
	Private mCoEnvCleSymTs as String
	
	''' <summary>
	''' Code environnement
	''' </summary>
	<EdmScalarPropertyAttribute()> _ 
	<DataMemberAttribute()> _
	<XuCuRRQDescription("Code environnement")> _
	Public Property CoEnvCleSymTs() As String
		Get
			Return Me.mCoEnvCleSymTs
		End Get
		Set (Byval value as String)
			If Me.mCoEnvCleSymTs Is Nothing OrElse Not Me.mCoEnvCleSymTs.Equals(value) Then
				Me.OnCoEnvCleSymTsChanging(value)
				Me.ReportPropertyChanging("CoEnvCleSymTs")
				Me.mCoEnvCleSymTs = StructuralObject.SetValidValue(value, True)
				Me.ReportPropertyChanged("CoEnvCleSymTs")
				Me.OnCoEnvCleSymTsChanged()
			End If
		End Set
	End Property
	
	Partial Private Sub OnCoEnvCleSymTsChanging(ByVal value As String)
	End Sub
	
	Partial Private Sub OnCoEnvCleSymTsChanged()
	End Sub
	
#End Region
	
#Region "CoTypDepCleTs"
	
	Private mCoTypDepCleTs as String
	
	''' <summary>
	''' Code type de dépot
	''' </summary>
	<EdmScalarPropertyAttribute()> _ 
	<DataMemberAttribute()> _
	<XuCuRRQDescription("Code type de dépot")> _
	Public Property CoTypDepCleTs() As String
		Get
			Return Me.mCoTypDepCleTs
		End Get
		Set (Byval value as String)
			If Me.mCoTypDepCleTs Is Nothing OrElse Not Me.mCoTypDepCleTs.Equals(value) Then
				Me.OnCoTypDepCleTsChanging(value)
				Me.ReportPropertyChanging("CoTypDepCleTs")
				Me.mCoTypDepCleTs = StructuralObject.SetValidValue(value, True)
				Me.ReportPropertyChanged("CoTypDepCleTs")
				Me.OnCoTypDepCleTsChanged()
			End If
		End Set
	End Property
	
	Partial Private Sub OnCoTypDepCleTsChanging(ByVal value As String)
	End Sub
	
	Partial Private Sub OnCoTypDepCleTsChanged()
	End Sub
	
#End Region
	
#Region "CoSysCleSymTs"
	
	Private mCoSysCleSymTs as String
	
	''' <summary>
	''' Code système
	''' </summary>
	<EdmScalarPropertyAttribute()> _ 
	<DataMemberAttribute()> _
	<XuCuRRQDescription("Code système")> _
	Public Property CoSysCleSymTs() As String
		Get
			Return Me.mCoSysCleSymTs
		End Get
		Set (Byval value as String)
			If Me.mCoSysCleSymTs Is Nothing OrElse Not Me.mCoSysCleSymTs.Equals(value) Then
				Me.OnCoSysCleSymTsChanging(value)
				Me.ReportPropertyChanging("CoSysCleSymTs")
				Me.mCoSysCleSymTs = StructuralObject.SetValidValue(value, True)
				Me.ReportPropertyChanged("CoSysCleSymTs")
				Me.OnCoSysCleSymTsChanged()
			End If
		End Set
	End Property
	
	Partial Private Sub OnCoSysCleSymTsChanging(ByVal value As String)
	End Sub
	
	Partial Private Sub OnCoSysCleSymTsChanged()
	End Sub
	
#End Region
	
#Region "CoSouCleSymTs"
	
	Private mCoSouCleSymTs as String
	
	''' <summary>
	''' Code sous-système
	''' </summary>
	<EdmScalarPropertyAttribute()> _ 
	<DataMemberAttribute()> _
	<XuCuRRQDescription("Code sous-système")> _
	Public Property CoSouCleSymTs() As String
		Get
			Return Me.mCoSouCleSymTs
		End Get
		Set (Byval value as String)
			If Me.mCoSouCleSymTs Is Nothing OrElse Not Me.mCoSouCleSymTs.Equals(value) Then
				Me.OnCoSouCleSymTsChanging(value)
				Me.ReportPropertyChanging("CoSouCleSymTs")
				Me.mCoSouCleSymTs = StructuralObject.SetValidValue(value, True)
				Me.ReportPropertyChanged("CoSouCleSymTs")
				Me.OnCoSouCleSymTsChanged()
			End If
		End Set
	End Property
	
	Partial Private Sub OnCoSouCleSymTsChanging(ByVal value As String)
	End Sub
	
	Partial Private Sub OnCoSouCleSymTsChanged()
	End Sub
	
#End Region
	
#Region "CoUtlGenCleTs"
	
	Private mCoUtlGenCleTs as String
	
	''' <summary>
	''' Code utilisateur générique
	''' </summary>
	<EdmScalarPropertyAttribute()> _ 
	<DataMemberAttribute()> _
	<XuCuRRQDescription("Code utilisateur générique")> _
	Public Property CoUtlGenCleTs() As String
		Get
			Return Me.mCoUtlGenCleTs
		End Get
		Set (Byval value as String)
			If Me.mCoUtlGenCleTs Is Nothing OrElse Not Me.mCoUtlGenCleTs.Equals(value) Then
				Me.OnCoUtlGenCleTsChanging(value)
				Me.ReportPropertyChanging("CoUtlGenCleTs")
				Me.mCoUtlGenCleTs = StructuralObject.SetValidValue(value, True)
				Me.ReportPropertyChanged("CoUtlGenCleTs")
				Me.OnCoUtlGenCleTsChanged()
			End If
		End Set
	End Property
	
	Partial Private Sub OnCoUtlGenCleTsChanging(ByVal value As String)
	End Sub
	
	Partial Private Sub OnCoUtlGenCleTsChanged()
	End Sub
	
#End Region
	
#Region "VlMotPasCleTs"
	
	Private mVlMotPasCleTs as String
	
	''' <summary>
	''' Valeur mot de passe
	''' </summary>
	<EdmScalarPropertyAttribute()> _ 
	<DataMemberAttribute()> _
	<XuCuRRQDescription("Valeur mot de passe")> _
	Public Property VlMotPasCleTs() As String
		Get
			Return Me.mVlMotPasCleTs
		End Get
		Set (Byval value as String)
			If Me.mVlMotPasCleTs Is Nothing OrElse Not Me.mVlMotPasCleTs.Equals(value) Then
				Me.OnVlMotPasCleTsChanging(value)
				Me.ReportPropertyChanging("VlMotPasCleTs")
				Me.mVlMotPasCleTs = StructuralObject.SetValidValue(value, True)
				Me.ReportPropertyChanged("VlMotPasCleTs")
				Me.OnVlMotPasCleTsChanged()
			End If
		End Set
	End Property
	
	Partial Private Sub OnVlMotPasCleTsChanging(ByVal value As String)
	End Sub
	
	Partial Private Sub OnVlMotPasCleTsChanged()
	End Sub
	
#End Region
	
#Region "DsCleSymTs"
	
	Private mDsCleSymTs as String
	
	''' <summary>
	''' description
	''' </summary>
	<EdmScalarPropertyAttribute()> _ 
	<DataMemberAttribute()> _
	<XuCuRRQDescription("description")> _
	Public Property DsCleSymTs() As String
		Get
			Return Me.mDsCleSymTs
		End Get
		Set (Byval value as String)
			If Me.mDsCleSymTs Is Nothing OrElse Not Me.mDsCleSymTs.Equals(value) Then
				Me.OnDsCleSymTsChanging(value)
				Me.ReportPropertyChanging("DsCleSymTs")
				Me.mDsCleSymTs = StructuralObject.SetValidValue(value, True)
				Me.ReportPropertyChanged("DsCleSymTs")
				Me.OnDsCleSymTsChanged()
			End If
		End Set
	End Property
	
	Partial Private Sub OnDsCleSymTsChanging(ByVal value As String)
	End Sub
	
	Partial Private Sub OnDsCleSymTsChanged()
	End Sub
	
#End Region
	
#Region "CmCleSymTs"
	
	Private mCmCleSymTs as String
	
	''' <summary>
	''' commentaire
	''' </summary>
	<EdmScalarPropertyAttribute()> _ 
	<DataMemberAttribute()> _
	<XuCuRRQDescription("commentaire")> _
	Public Property CmCleSymTs() As String
		Get
			Return Me.mCmCleSymTs
		End Get
		Set (Byval value as String)
			If Me.mCmCleSymTs Is Nothing OrElse Not Me.mCmCleSymTs.Equals(value) Then
				Me.OnCmCleSymTsChanging(value)
				Me.ReportPropertyChanging("CmCleSymTs")
				Me.mCmCleSymTs = StructuralObject.SetValidValue(value, True)
				Me.ReportPropertyChanged("CmCleSymTs")
				Me.OnCmCleSymTsChanged()
			End If
		End Set
	End Property
	
	Partial Private Sub OnCmCleSymTsChanging(ByVal value As String)
	End Sub
	
	Partial Private Sub OnCmCleSymTsChanged()
	End Sub
	
#End Region
	
#Region "VlVerCleSymTs"
	
	Private mVlVerCleSymTs as String
	
	''' <summary>
	''' Valeur vérification
	''' </summary>
	<EdmScalarPropertyAttribute()> _ 
	<DataMemberAttribute()> _
	<XuCuRRQDescription("Valeur vérification")> _
	Public Property VlVerCleSymTs() As String
		Get
			Return Me.mVlVerCleSymTs
		End Get
		Set (Byval value as String)
			If Me.mVlVerCleSymTs Is Nothing OrElse Not Me.mVlVerCleSymTs.Equals(value) Then
				Me.OnVlVerCleSymTsChanging(value)
				Me.ReportPropertyChanging("VlVerCleSymTs")
				Me.mVlVerCleSymTs = StructuralObject.SetValidValue(value, True)
				Me.ReportPropertyChanged("VlVerCleSymTs")
				Me.OnVlVerCleSymTsChanged()
			End If
		End Set
	End Property
	
	Partial Private Sub OnVlVerCleSymTsChanging(ByVal value As String)
	End Sub
	
	Partial Private Sub OnVlVerCleSymTsChanged()
	End Sub
	
#End Region
	
#Region "InAjtEnv"
	
	Private mInAjtEnv as Boolean
	
	''' <summary>
	''' Indicateur pour forcer l'ajout du code de l'environnement à la clé dans les cas ou cela n'est normalement pas fait
	''' </summary>
	<EdmScalarPropertyAttribute(IsNullable:=False)> _ 
	<DataMemberAttribute()> _
	<XuCuRRQValeurParDefaut("False")> _ 
	<XuCuRRQDescription("Indicateur pour forcer l'ajout du code de l'environnement à la clé dans les cas ou cela n'est normalement pas fait")> _
	Public Property InAjtEnv() As Boolean
		Get
			Return Me.mInAjtEnv
		End Get
		Set (Byval value as Boolean)
			If Not Me.mInAjtEnv.Equals(value) Then
				Me.OnInAjtEnvChanging(value)
				Me.ReportPropertyChanging("InAjtEnv")
				Me.mInAjtEnv = StructuralObject.SetValidValue(value)
				Me.ReportPropertyChanged("InAjtEnv")
				Me.OnInAjtEnvChanged()
			End If
		End Set
	End Property
	
	Partial Private Sub OnInAjtEnvChanging(ByVal value As Boolean)
	End Sub
	
	Partial Private Sub OnInAjtEnvChanged()
	End Sub
	
#End Region
	
#Region "LsGroAd"
	
	''' <summary>
	''' Liste des groupe de l'AD
	''' </summary>
	<DataMember(), XuCuRRQCapaciteListe(1)> _
	Public LsGroAd As IList(Of TsDtGroAd) = New List(Of TsDtGroAd)
	
#End Region
	
#Region "StIndCreCpt"
	
	Private mStIndCreCpt as TsDtIndCreCpt
	
	''' <summary>
	''' Indicateurs de création de compte
	''' </summary>
	<EdmScalarPropertyAttribute(IsNullable:=False)> _ 
	<DataMemberAttribute()> _
	<XuCuRRQDescription("Indicateurs de création de compte")> _
	Public Property StIndCreCpt() As TsDtIndCreCpt
		Get
			Return Me.mStIndCreCpt
		End Get
		Set (Byval value as TsDtIndCreCpt)
			If Me.mStIndCreCpt Is Nothing OrElse Not Me.mStIndCreCpt.Equals(value) Then
				Me.OnStIndCreCptChanging(value)
				Me.ReportPropertyChanging("StIndCreCpt")
				Me.mStIndCreCpt = value
				Me.ReportPropertyChanged("StIndCreCpt")
				Me.OnStIndCreCptChanged()
			End If
		End Set
	End Property
	
	Partial Private Sub OnStIndCreCptChanging(ByVal value As TsDtIndCreCpt)
	End Sub
	
	Partial Private Sub OnStIndCreCptChanged()
	End Sub
	
#End Region
	
	<OnDeserialized()> _
	Private Sub OnDeserializedMethod(ByVal Context As StreamingContext)
		If LsGroAd IsNot Nothing Then
			LsGroAd = LsGroAd.ToList()
		End If
	End Sub
	
End Class

