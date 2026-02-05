' On a ici dans un même fichier toutes les classes de métadonnées pour jouer avec Sage (paramètres, liste de config...)
' (autant les classes de collection que les objets de dernier niveau)
' Les listes d'attributs de ces classes sont probablement incomplètes, on pourra en ajouter

Imports System.Xml.Serialization

''' <summary>
''' Collection de «configurations» au sens Sage
''' </summary>
<XmlRoot("SageConfigurations")> _
Public Class TsCdSageConfigurationCollection
    Inherits TsCdCollectionSage(Of TsCdSageConfiguration)

    Public ReadOnly Property Configurations() As List(Of TsCdSageConfiguration)
        Get
            Return _list
        End Get
    End Property

End Class

''' <summary>
''' Description d'une «configurations» au sens Sage
''' </summary>
<XmlType("Configuration")> _
Public Class TsCdSageConfiguration
    Public ConfigurationID As Integer
    Public ConfigurationName As String
    Public UDB As String
    Public RDB As String
    Public Owner1 As String
    Public Operation1 As String
    Public ParentConfigName As String
    Public CreateDate As Date
    Public ModifyDate As Date
End Class

Namespace IncoherenceSage
    ' Dans ce namespace on retrouve des classes pour dealer avec des incohérences de Sage... à ne pas trop utiliser
    ' à l'extérieur de ce composant... Elles sont Public pour permettre le sérialisation XML

    ' Pour une raison incompréhensible, il y a un tag bidon autour de la configuration...
    <XmlType("NewDataSet")> _
    Public Class TsCdSageEnveloppeConfigurationSeule
        Inherits TsCdCollectionSage(Of TsCdSageConfigurationSeule)

        <XmlIgnore()> _
        Public ReadOnly Property Config() As TsCdSageConfigurationSeule
            Get
                Return _list(0)
            End Get
        End Property

    End Class

    <XmlType("Configuration")> _
    Public Class TsCdSageConfigurationSeule
        ' La version retournée par cfg_get_databases est un peu différente de data_sourace_get_configurations... pas fort!
        Public ConfigurationID As Integer
        Public ConfigurationName As String
        <XmlElement("UserDatabaseID")> _
        Public UDBID As Integer
        <XmlElement("UserDBName")> _
        Public UDB As String
        <XmlElement("ResourceDatabaseID")> _
        Public RDBID As Integer
        <XmlElement("DatabaseName")> _
        Public RDB As String
        Public CreateDate As Date
        Public ModifyDate As Date
        Public Owner1 As String
        Public Operation1 As String
        Public ParentConfigName As String
        Public IsReadOnly As Integer
        Public IsLogged As Integer
        Public IsCompleted As Integer
    End Class
End Namespace
