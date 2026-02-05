Imports Rrq.InfrastructureCommune.ScenarioTransactionnel
Imports System.Collections.Generic
Imports System.Runtime.Serialization
Imports System.Data.Objects.DataClasses
Imports System.Linq

''' <summary>
''' Structure de retour de l'obtention des indicateurs de création des comptes AD et TSS
''' </summary>
<EdmEntityTypeAttribute(NamespaceName:="TS1N201_DtCdAccGenV1", Name:="TsDtIndCreCpt")> _
<DataContractAttribute(IsReference:=True)> _
<Serializable(), CLSCompliant(True)> _
<XuCuRRQDescription("Structure de retour de l'obtention des indicateurs de création des comptes AD et TSS")> _
Public Class TsDtIndCreCpt
	Inherits XuCuDonneesTravailResultat
	
#Region "Constructeurs"
	
	Public Sub New()
		'Initialisation des valeurs par défaut.
		Me.mInCreCptTssTs = True
		Me.mInCreCptAdTs = True
	End Sub
	
#End Region
	
#Region "InCreCptTssTs"
	
	Private mInCreCptTssTs as Boolean
	
	''' <summary>
	''' Indicateur de création de compte TSS
	''' </summary>
	<EdmScalarPropertyAttribute(IsNullable:=False)> _ 
	<DataMemberAttribute()> _
	<XuCuRRQValeurParDefaut("True")> _ 
	<XuCuRRQDescription("Indicateur de création de compte TSS")> _
	Public Property InCreCptTssTs() As Boolean
		Get
			Return Me.mInCreCptTssTs
		End Get
		Set (Byval value as Boolean)
			If Not Me.mInCreCptTssTs.Equals(value) Then
				Me.OnInCreCptTssTsChanging(value)
				Me.ReportPropertyChanging("InCreCptTssTs")
				Me.mInCreCptTssTs = StructuralObject.SetValidValue(value)
				Me.ReportPropertyChanged("InCreCptTssTs")
				Me.OnInCreCptTssTsChanged()
			End If
		End Set
	End Property
	
	Partial Private Sub OnInCreCptTssTsChanging(ByVal value As Boolean)
	End Sub
	
	Partial Private Sub OnInCreCptTssTsChanged()
	End Sub
	
#End Region
	
#Region "InCreCptAdTs"
	
	Private mInCreCptAdTs as Boolean
	
	''' <summary>
	''' Indicateur de création du compte AD
	''' </summary>
	<EdmScalarPropertyAttribute(IsNullable:=False)> _ 
	<DataMemberAttribute()> _
	<XuCuRRQValeurParDefaut("True")> _ 
	<XuCuRRQDescription("Indicateur de création du compte AD")> _
	Public Property InCreCptAdTs() As Boolean
		Get
			Return Me.mInCreCptAdTs
		End Get
		Set (Byval value as Boolean)
			If Not Me.mInCreCptAdTs.Equals(value) Then
				Me.OnInCreCptAdTsChanging(value)
				Me.ReportPropertyChanging("InCreCptAdTs")
				Me.mInCreCptAdTs = StructuralObject.SetValidValue(value)
				Me.ReportPropertyChanged("InCreCptAdTs")
				Me.OnInCreCptAdTsChanged()
			End If
		End Set
	End Property
	
	Partial Private Sub OnInCreCptAdTsChanging(ByVal value As Boolean)
	End Sub
	
	Partial Private Sub OnInCreCptAdTsChanged()
	End Sub
	
#End Region
	
#Region "InCreCptLdsTs"
	
	Private mInCreCptLdsTs as Boolean
	
	''' <summary>
	''' Indicateur de création du compte AD/LDS
	''' </summary>
	<EdmScalarPropertyAttribute(IsNullable:=False)> _ 
	<DataMemberAttribute()> _
	<XuCuRRQDescription("Indicateur de création du compte AD/LDS")> _
	Public Property InCreCptLdsTs() As Boolean
		Get
			Return Me.mInCreCptLdsTs
		End Get
		Set (Byval value as Boolean)
			If Not Me.mInCreCptLdsTs.Equals(value) Then
				Me.OnInCreCptLdsTsChanging(value)
				Me.ReportPropertyChanging("InCreCptLdsTs")
				Me.mInCreCptLdsTs = StructuralObject.SetValidValue(value)
				Me.ReportPropertyChanged("InCreCptLdsTs")
				Me.OnInCreCptLdsTsChanged()
			End If
		End Set
	End Property
	
	Partial Private Sub OnInCreCptLdsTsChanging(ByVal value As Boolean)
	End Sub
	
	Partial Private Sub OnInCreCptLdsTsChanged()
	End Sub
	
#End Region
	
End Class

