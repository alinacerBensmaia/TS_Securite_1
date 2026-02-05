Imports Rrq.InfrastructureCommune.ScenarioTransactionnel
Imports System.Collections.Generic
Imports System.Runtime.Serialization
Imports System.Data.Objects.DataClasses
Imports System.Linq

''' <summary>
''' Groupe "active directory"
''' </summary>
<EdmEntityTypeAttribute(NamespaceName:="TS1N201_DtCdAccGenV1", Name:="TsDtGroAd")> _
<DataContractAttribute(IsReference:=True)> _
<Serializable(), CLSCompliant(True)> _
<XuCuRRQDescription("Groupe ""active directory""")> _
Public Class TsDtGroAd
	Inherits XuCuBase
	
#Region "NmGroActDirTs"
	
	Private mNmGroActDirTs as String
	
	''' <summary>
	''' Inscrire une description
	''' </summary>
	<EdmScalarPropertyAttribute()> _ 
	<DataMemberAttribute()> _
	<XuCuRRQDescription("Inscrire une description")> _
	Public Property NmGroActDirTs() As String
		Get
			Return Me.mNmGroActDirTs
		End Get
		Set (Byval value as String)
			If Me.mNmGroActDirTs Is Nothing OrElse Not Me.mNmGroActDirTs.Equals(value) Then
				Me.OnNmGroActDirTsChanging(value)
				Me.ReportPropertyChanging("NmGroActDirTs")
				Me.mNmGroActDirTs = StructuralObject.SetValidValue(value, True)
				Me.ReportPropertyChanged("NmGroActDirTs")
				Me.OnNmGroActDirTsChanged()
			End If
		End Set
	End Property
	
	Partial Private Sub OnNmGroActDirTsChanging(ByVal value As String)
	End Sub
	
	Partial Private Sub OnNmGroActDirTsChanged()
	End Sub
	
#End Region
	
#Region "LsCleSym"
	
	''' <summary>
	''' Liste des clés symboliques
	''' </summary>
	<DataMember(), XuCuRRQCapaciteListe(1)> _
	Public LsCleSym As IList(Of TsDtCleSym) = New List(Of TsDtCleSym)
	
#End Region
	
	<OnDeserialized()> _
	Private Sub OnDeserializedMethod(ByVal Context As StreamingContext)
		If LsCleSym IsNot Nothing Then
			LsCleSym = LsCleSym.ToList()
		End If
	End Sub
	
End Class

