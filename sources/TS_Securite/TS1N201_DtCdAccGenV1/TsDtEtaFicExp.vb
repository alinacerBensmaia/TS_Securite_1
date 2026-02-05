Imports Rrq.InfrastructureCommune.ScenarioTransactionnel
Imports System.Collections.Generic
Imports System.Runtime.Serialization
Imports System.Data.Objects.DataClasses
Imports System.Linq

''' <summary>
''' Informations sur l'état du fichier d'exportation
''' </summary>
<EdmEntityTypeAttribute(NamespaceName:="TS1N201_DtCdAccGenV1", Name:="TsDtEtaFicExp")> _
<DataContractAttribute(IsReference:=True)> _
<Serializable(), CLSCompliant(True)> _
<XuCuRRQDescription("Informations sur l'état du fichier d'exportation")> _
Public Class TsDtEtaFicExp
	Inherits XuCuDonneesTravailResultat
	
#Region "Constructeurs"
	
	Public Sub New()
		'Initialisation des valeurs par défaut.
		Me.mInFicJou = False
	End Sub
	
#End Region
	
#Region "InFicJou"
	
	Private mInFicJou as Boolean
	
	''' <summary>
	''' Indicateur de fichier à jour
	''' </summary>
	<EdmScalarPropertyAttribute(IsNullable:=False)> _ 
	<DataMemberAttribute()> _
	<XuCuRRQValeurParDefaut("False")> _ 
	<XuCuRRQDescription("Indicateur de fichier à jour")> _
	Public Property InFicJou() As Boolean
		Get
			Return Me.mInFicJou
		End Get
		Set (Byval value as Boolean)
			If Not Me.mInFicJou.Equals(value) Then
				Me.OnInFicJouChanging(value)
				Me.ReportPropertyChanging("InFicJou")
				Me.mInFicJou = StructuralObject.SetValidValue(value)
				Me.ReportPropertyChanged("InFicJou")
				Me.OnInFicJouChanged()
			End If
		End Set
	End Property
	
	Partial Private Sub OnInFicJouChanging(ByVal value As Boolean)
	End Sub
	
	Partial Private Sub OnInFicJouChanged()
	End Sub
	
#End Region
	
#Region "DtDerFicExp"
	
	Private mDtDerFicExp as DateTime
	
	''' <summary>
	''' Date de dernière exportation du fichier
	''' </summary>
	<EdmScalarPropertyAttribute(IsNullable:=False)> _ 
	<DataMemberAttribute()> _
	<XuCuRRQDescription("Date de dernière exportation du fichier")> _
	Public Property DtDerFicExp() As DateTime
		Get
			Return Me.mDtDerFicExp
		End Get
		Set (Byval value as DateTime)
			If Not Me.mDtDerFicExp.Equals(value) Then
				Me.OnDtDerFicExpChanging(value)
				Me.ReportPropertyChanging("DtDerFicExp")
				Me.mDtDerFicExp = StructuralObject.SetValidValue(value)
				Me.ReportPropertyChanged("DtDerFicExp")
				Me.OnDtDerFicExpChanged()
			End If
		End Set
	End Property
	
	Partial Private Sub OnDtDerFicExpChanging(ByVal value As DateTime)
	End Sub
	
	Partial Private Sub OnDtDerFicExpChanged()
	End Sub
	
#End Region
	
#Region "DtDerMajBd"
	
	Private mDtDerMajBd as DateTime
	
	''' <summary>
	''' Date de dernière mise à jour dans la base de données
	''' </summary>
	<EdmScalarPropertyAttribute(IsNullable:=False)> _ 
	<DataMemberAttribute()> _
	<XuCuRRQDescription("Date de dernière mise à jour dans la base de données")> _
	Public Property DtDerMajBd() As DateTime
		Get
			Return Me.mDtDerMajBd
		End Get
		Set (Byval value as DateTime)
			If Not Me.mDtDerMajBd.Equals(value) Then
				Me.OnDtDerMajBdChanging(value)
				Me.ReportPropertyChanging("DtDerMajBd")
				Me.mDtDerMajBd = StructuralObject.SetValidValue(value)
				Me.ReportPropertyChanged("DtDerMajBd")
				Me.OnDtDerMajBdChanged()
			End If
		End Set
	End Property
	
	Partial Private Sub OnDtDerMajBdChanging(ByVal value As DateTime)
	End Sub
	
	Partial Private Sub OnDtDerMajBdChanged()
	End Sub
	
#End Region
	
End Class

