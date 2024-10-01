Imports System.IO
Imports System.Threading

Public Class Emergency

    Private Shared mut As New Mutex()
    Public Sub WriteFile(ByVal sInput As String, ByVal pSes As System.Web.SessionState.HttpSessionState)
        Try
            Dim sFileName = pSes("LogPath")
            Dim finfo As FileInfo = New FileInfo(sFileName)
            mut.WaitOne(400, True)
            Dim fs As FileStream = finfo.OpenWrite()
            Dim w As StreamWriter = New StreamWriter(fs)
            w.BaseStream.Seek(0, SeekOrigin.End)
            w.WriteLine(Now.ToString("dd/MM/yy HH:mm:ss:fff") + " " + pSes("Job") + " " + sInput + " " + pSes.SessionID)
            w.Flush()
            w.Dispose()
            w.Close()
        Catch ex As Exception
        Finally
            Try
                mut.ReleaseMutex()
            Catch
            End Try
        End Try
    End Sub

End Class
