Public Class Text

    Dim sBuffer As String

    Public Sub New(ByVal nLen As Integer)
        MyBase.New()
        sBuffer = "".PadRight(nLen)
    End Sub

    Public Sub Put(ByVal nPos As Integer, ByVal nLen As Integer, ByVal sValue As String)
        Mid(sBuffer, nPos, nLen) = sValue.PadRight(nLen)
    End Sub
    Public Sub Put(ByVal nPos As Integer, ByVal nLen As Integer, ByVal nValue As Integer)
        Mid(sBuffer, nPos, nLen) = nValue.ToString("".PadLeft(nLen, "0"))
    End Sub
    Public Function FullText() As String
        Return sBuffer
    End Function

End Class
