Public Class TsCuCommuns

    Public Shared Function ObtenirTimeStamp() As String
        Dim now As Date = Date.Now
        Dim timestamp As String = String.Concat(now.Year.ToString("00"), now.Month.ToString("00"), now.Day.ToString("00"), now.Hour.ToString("00"),
                                                now.Minute.ToString("00"), now.Second.ToString("00"), now.Millisecond.ToString("000"))

        Return timestamp
    End Function
End Class
