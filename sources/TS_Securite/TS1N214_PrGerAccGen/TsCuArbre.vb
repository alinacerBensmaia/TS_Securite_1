Imports System.Globalization
Imports System.Text

Public NotInheritable Class TsCuArbre
    Implements IDisposable
    Private mDtArbreCleSousSys As DataTable
    Private mDtArbreCle As DataTable
    Private mDtArbreCleEnv As DataTable

    Public Property DtArbreCleSousSys() As DataTable
        Get
            Return mDtArbreCleSousSys
        End Get
        Set(ByVal value As DataTable)
            mDtArbreCleSousSys = value
        End Set
    End Property

    Private Property DtArbreCle() As DataTable
        Get
            Return mDtArbreCle
        End Get
        Set(ByVal value As DataTable)
            mDtArbreCle = value
        End Set
    End Property

    Private Property DtArbreCleEnv() As DataTable
        Get
            Return mDtArbreCleEnv
        End Get
        Set(ByVal value As DataTable)
            mDtArbreCleEnv = value
        End Set
    End Property

    Friend Sub CreerArbreSousSys(ByVal pSysRow As DataRow,
                                 ByVal pCle As TS1N201_DtCdAccGenV1.TsDtCleSym)

        If pSysRow Is Nothing Then
            DtArbreCleSousSys = New DataTable
            DtArbreCleSousSys.Locale = CultureInfo.InvariantCulture
            DtArbreCleSousSys.Columns.Add(New DataColumn(My.Resources.NomSousSysteme))
            DtArbreCleSousSys.Columns.Add(New DataColumn(My.Resources.TagCle, GetType(DataTable)))
        Else
            DtArbreCleSousSys = DirectCast(pSysRow(My.Resources.TagSousSysteme), DataTable)
        End If

        If DtArbreCleSousSys.Rows.Count = 0 OrElse DtArbreCleSousSys.Rows(DtArbreCleSousSys.Rows.Count - 1)(My.Resources.NomSousSysteme).ToString <> pCle.CoSouCleSymTs Then
            Dim r As DataRow = DtArbreCleSousSys.NewRow
            r(My.Resources.NomSousSysteme) = pCle.CoSouCleSymTs
            CreerArbreCle(Nothing, pCle)
            r(My.Resources.TagCle) = DtArbreCle
            DtArbreCleSousSys.Rows.Add(r)
        Else
            Dim r As DataRow = DtArbreCleSousSys.Rows(DtArbreCleSousSys.Rows.Count - 1)
            CreerArbreCle(r, pCle)
            r(My.Resources.TagCle) = DtArbreCle
        End If
    End Sub

    Private Sub CreerArbreCle(ByVal pSousSysRow As DataRow,
                              ByVal pCle As TS1N201_DtCdAccGenV1.TsDtCleSym)
        Dim reg As RegularExpressions.Regex = New RegularExpressions.Regex("[EUIABPQS]", RegularExpressions.RegexOptions.Compiled)

        If pSousSysRow Is Nothing Then
            DtArbreCle = New DataTable
            DtArbreCle.Locale = CultureInfo.InvariantCulture
            DtArbreCle.Columns.Add(New DataColumn(My.Resources.NomCle))
            DtArbreCle.Columns.Add(New DataColumn(My.Resources.TagCleEnv, GetType(DataTable)))
        Else
            DtArbreCle = DirectCast(pSousSysRow(My.Resources.TagCle), DataTable)
        End If

        If DtArbreCle.Rows.Count = 0 OrElse
           (DtArbreCle.Rows(DtArbreCle.Rows.Count - 1)(My.Resources.NomCle).ToString.Substring(0, DtArbreCle.Rows(DtArbreCle.Rows.Count - 1)(My.Resources.NomCle).ToString.Length - 1) <> pCle.CoIdnCleSymTs.Substring(0, pCle.CoIdnCleSymTs.Length - 1) OrElse
           Not reg.IsMatch(pCle.CoIdnCleSymTs.Substring(pCle.CoIdnCleSymTs.Length - 1))) Then
            Dim r As DataRow = DtArbreCle.NewRow
            r(My.Resources.NomCle) = pCle.CoIdnCleSymTs
            CreerArbreCleEnv(Nothing, pCle)
            r(My.Resources.TagCleEnv) = DtArbreCleEnv
            DtArbreCle.Rows.Add(r)
        Else
            Dim r As DataRow = DtArbreCle.Rows(DtArbreCle.Rows.Count - 1)
            CreerArbreCleEnv(r, pCle)
            r(My.Resources.TagCleEnv) = DtArbreCleEnv
        End If
    End Sub

    Private Sub CreerArbreCleEnv(ByVal pCleRow As DataRow,
                                 ByVal pCle As TS1N201_DtCdAccGenV1.TsDtCleSym)
        If pCleRow Is Nothing Then
            DtArbreCleEnv = New DataTable
            DtArbreCleEnv.Locale = CultureInfo.InvariantCulture
            DtArbreCleEnv.Columns.Add(New DataColumn(My.Resources.NomCle))
        Else
            DtArbreCleEnv = DirectCast(pCleRow(My.Resources.TagCleEnv), DataTable)
        End If
        Dim r As DataRow = DtArbreCleEnv.NewRow
        r(My.Resources.NomCle) = pCle.CoIdnCleSymTs
        DtArbreCleEnv.Rows.Add(r)
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Using mDtArbreCleSousSys : End Using
        Using mDtArbreCle : End Using
        Using mDtArbreCleEnv : End Using
    End Sub

End Class
