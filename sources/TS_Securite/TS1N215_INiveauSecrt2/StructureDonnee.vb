Imports Rrq.InfrastructureCommune.ScenarioTransactionnel
Imports System.Collections.Generic
Imports System.Runtime.Serialization
Imports System.Data.Objects.DataClasses
Imports System.Linq

''' <summary>
''' Description du verrou actif
''' </summary>
<EdmEntityTypeAttribute(NamespaceName:="TS1N215_INiveauSecrt2", Name:="TsDtVerrou")> _
<DataContractAttribute(IsReference:=True)> _
<Serializable(), CLSCompliant(True)> _
<XuCuRRQDescription("Description du verrou actif")> _
Public Class TsDtVerrou
	Inherits XuCuBase
	
#Region "CoUtlPro"
	
	Private mCoUtlPro as String
	
	''' <summary>
	''' Code de l'utilisateur propriétaire du verrou
	''' </summary>
	<EdmScalarPropertyAttribute(IsNullable:=False)> _ 
	<DataMemberAttribute()> _
	<XuCuRRQDescription("Code de l'utilisateur propriétaire du verrou")> _
	Public Property CoUtlPro() As String
		Get
			Return Me.mCoUtlPro
		End Get
		Set (Byval value as String)
			If Me.mCoUtlPro Is Nothing OrElse Not Me.mCoUtlPro.Equals(value) Then
				Me.OnCoUtlProChanging(value)
				Me.ReportPropertyChanging("CoUtlPro")
				Me.mCoUtlPro = StructuralObject.SetValidValue(value, True)
				Me.ReportPropertyChanged("CoUtlPro")
				Me.OnCoUtlProChanged()
			End If
		End Set
	End Property
	
	Partial Private Sub OnCoUtlProChanging(ByVal value As String)
	End Sub
	
	Partial Private Sub OnCoUtlProChanged()
	End Sub
	
#End Region
	
#Region "InVerObt"
	
	Private mInVerObt as Boolean
	
	''' <summary>
	''' Indicateur de verrou obtenu
	''' </summary>
	<EdmScalarPropertyAttribute(IsNullable:=False)> _ 
	<DataMemberAttribute()> _
	<XuCuRRQDescription("Indicateur de verrou obtenu")> _
	Public Property InVerObt() As Boolean
		Get
			Return Me.mInVerObt
		End Get
		Set (Byval value as Boolean)
			If Not Me.mInVerObt.Equals(value) Then
				Me.OnInVerObtChanging(value)
				Me.ReportPropertyChanging("InVerObt")
				Me.mInVerObt = StructuralObject.SetValidValue(value)
				Me.ReportPropertyChanged("InVerObt")
				Me.OnInVerObtChanged()
			End If
		End Set
	End Property
	
	Partial Private Sub OnInVerObtChanging(ByVal value As Boolean)
	End Sub
	
	Partial Private Sub OnInVerObtChanged()
	End Sub
	
#End Region
	
End Class

