Imports Rrq.InfrastructureCommune.ScenarioTransactionnel
Imports System.Collections.Generic
Imports System.Runtime.Serialization
Imports System.Data.Objects.DataClasses
Imports System.Linq

''' <summary>
''' Donnée de travail représentant un utilisateur dans l'active directory
''' </summary>
<EdmEntityTypeAttribute(NamespaceName:="Rrq.Securite.Applicative", Name:="TsDtUtilisateur")> _
<DataContractAttribute(IsReference:=True)> _
<Serializable(), CLSCompliant(True)> _
<XuCuRRQDescription("Donnée de travail représentant un utilisateur dans l'active directory")> _
Public Class TsDtUtilisateur
	Inherits XuCuBase
	
#Region "CoUtl"
	
	Private mCoUtl as String
	
	''' <summary>
	''' Code de l'utilisateur active directory
	''' </summary>
	<EdmScalarPropertyAttribute(IsNullable:=False)> _ 
	<DataMemberAttribute()> _
	<XuCuRRQDescription("Code de l'utilisateur active directory")> _
	Public Property CoUtl() As String
		Get
			Return Me.mCoUtl
		End Get
		Set (Byval value as String)
			If Me.mCoUtl Is Nothing OrElse Not Me.mCoUtl.Equals(value) Then
				Me.OnCoUtlChanging(value)
				Me.ReportPropertyChanging("CoUtl")
				Me.mCoUtl = StructuralObject.SetValidValue(value, True)
				Me.ReportPropertyChanged("CoUtl")
				Me.OnCoUtlChanged()
			End If
		End Set
	End Property
	
	Partial Private Sub OnCoUtlChanging(ByVal value As String)
	End Sub
	
	Partial Private Sub OnCoUtlChanged()
	End Sub
	
#End Region
	
#Region "NmComUtl"
	
	Private mNmComUtl as String
	
	''' <summary>
	''' Nom complet de l'utilisateur.
	''' </summary>
	<EdmScalarPropertyAttribute(IsNullable:=False)> _ 
	<DataMemberAttribute()> _
	<XuCuRRQDescription("Nom complet de l'utilisateur.")> _
	Public Property NmComUtl() As String
		Get
			Return Me.mNmComUtl
		End Get
		Set (Byval value as String)
			If Me.mNmComUtl Is Nothing OrElse Not Me.mNmComUtl.Equals(value) Then
				Me.OnNmComUtlChanging(value)
				Me.ReportPropertyChanging("NmComUtl")
				Me.mNmComUtl = StructuralObject.SetValidValue(value, True)
				Me.ReportPropertyChanged("NmComUtl")
				Me.OnNmComUtlChanged()
			End If
		End Set
	End Property
	
	Partial Private Sub OnNmComUtlChanging(ByVal value As String)
	End Sub
	
	Partial Private Sub OnNmComUtlChanged()
	End Sub
	
#End Region
	
End Class

