Imports System.Collections.Generic
Imports System.Xml

Partial Public Class AfficherRapport
    Inherits System.Web.UI.Page
    Public lstUARapport As New List(Of Integer)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LancerRapport()
    End Sub

    Public Sub LancerRapport()
        'Lancer Rapport
        HttpContext.Current.Response.ContentType = "application/vnd.ms-excel"

        Dim arrString() As String = Request.QueryString("UA").Split(CChar(","))
        For i As Integer = 0 To arrString.Length - 1
            lstUARapport.Add(CInt(arrString(i)))
        Next


        Dim strLstUA As String = String.Empty
        lstUARapport.Sort()

        For Each UA As Integer In lstUARapport
            If strLstUA = String.Empty Then
                strLstUA = CStr(UA)
            Else
                strLstUA = strLstUA & "-" & CStr(UA)
            End If
        Next


        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=Roles UA " & strLstUA & " au " & Date.Now.ToShortDateString & ".xls")


        Dim strRapportBrut As String = String.Empty
        Dim objRapport As New TS7I142_CiRapportRess.TsCaGererRapports


        strRapportBrut = objRapport.ProduireRapportExcel(lstUARapport)

        Response.Clear()

        Using x As New XmlTextWriter(System.Web.HttpContext.Current.Response.OutputStream, System.Text.Encoding.UTF8)
            x.WriteRaw(strRapportBrut)
        End Using

        HttpContext.Current.Response.End()
    End Sub

End Class