Imports Rrq.InfrastructureCommune.ScenarioTransactionnel
Imports System.Collections.Generic
Imports System.Runtime.Serialization
Imports System.Data.Objects.DataClasses
Imports System.Linq

''' <summary>
''' Donnée de travail représentant un groupe dans l'active directory.
''' </summary>
<EdmEntityTypeAttribute(NamespaceName:="Rrq.Securite.Applicative", Name:="TsDtGroupe")> _
<DataContractAttribute(IsReference:=True)> _
<Serializable(), CLSCompliant(True)> _
<XuCuRRQDescription("Donnée de travail représentant un groupe dans l'active directory.")> _
Public Class TsDtGroupe
	Inherits XuCuBase
	
#Region "NmGrpSec"
	
	Private mNmGrpSec as String
	
	''' <summary>
	''' Nom du groupe de sécurité
	''' </summary>
	<EdmScalarPropertyAttribute(IsNullable:=False)> _ 
	<DataMemberAttribute()> _
	<XuCuRRQDescription("Nom du groupe de sécurité")> _
	Public Property NmGrpSec() As String
		Get
			Return Me.mNmGrpSec
		End Get
		Set (Byval value as String)
			If Me.mNmGrpSec Is Nothing OrElse Not Me.mNmGrpSec.Equals(value) Then
				Me.OnNmGrpSecChanging(value)
				Me.ReportPropertyChanging("NmGrpSec")
				Me.mNmGrpSec = StructuralObject.SetValidValue(value, True)
				Me.ReportPropertyChanged("NmGrpSec")
				Me.OnNmGrpSecChanged()
			End If
		End Set
	End Property
	
	Partial Private Sub OnNmGrpSecChanging(ByVal value As String)
	End Sub
	
	Partial Private Sub OnNmGrpSecChanged()
	End Sub
	
#End Region
	
#Region "DsGrpSec"
	
	Private mDsGrpSec as String
	
	''' <summary>
	''' Description du groupe de sécurité
	''' </summary>
	<EdmScalarPropertyAttribute(IsNullable:=False)> _ 
	<DataMemberAttribute()> _
	<XuCuRRQDescription("Description du groupe de sécurité")> _
	Public Property DsGrpSec() As String
		Get
			Return Me.mDsGrpSec
		End Get
		Set (Byval value as String)
			If Me.mDsGrpSec Is Nothing OrElse Not Me.mDsGrpSec.Equals(value) Then
				Me.OnDsGrpSecChanging(value)
				Me.ReportPropertyChanging("DsGrpSec")
				Me.mDsGrpSec = StructuralObject.SetValidValue(value, True)
				Me.ReportPropertyChanged("DsGrpSec")
				Me.OnDsGrpSecChanged()
			End If
		End Set
	End Property
	
	Partial Private Sub OnDsGrpSecChanging(ByVal value As String)
	End Sub
	
	Partial Private Sub OnDsGrpSecChanged()
	End Sub
	
#End Region
	
End Class

