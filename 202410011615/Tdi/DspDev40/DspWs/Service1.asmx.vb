Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Messaging

<System.Web.Services.WebService(Namespace:="http://comtec.co.il/")> _
Public Class Service1
    Inherits System.Web.Services.WebService

    Public sResult As String = ""

    <WebMethod()> _
    Public Function As400(ByVal sCheckCode As String, ByVal sSystem As String, ByVal sUser As String, ByVal sPass As String) As String
        If sCheckCode <> "BD9D2C18-0449-4391-AB4A-E29CB9095598" Then Return "CHECK CODE ERROR"
        Dim conn As ADODB.Connection
        Try
            conn = New ADODB.Connection()
            conn.Open("Provider=IBMDA400;Data Source=" & sSystem & ";", sUser, sPass)
            conn = Nothing
            Return ""
        Catch ee As Exception
            Return ee.Message
        End Try
    End Function

    'vk 09.09
    <WebMethod()> _
    Public Function Result() As String
        Return sResult
    End Function
    <WebMethod()> _
    Public Function ClearMq(ByVal sCheckCode As String, _
            ByVal sComputer As String, ByVal sMask As String, ByVal nDaysToSave As Integer) As String
        Dim i As Integer, c As New Collection, s As String
        If sCheckCode <> "389E9A03-62BE-4fdf-A93A-3F8E5D56B494" Then
            sResult = "CHECK CODE ERROR"
        Else
            sResult = ""
            Try
                For i = MessageQueue.GetPrivateQueuesByMachine(sComputer).Length - 1 To 0 Step -1
                    With MessageQueue.GetPrivateQueuesByMachine(sComputer)(i)
                        If .CreateTime < DateAdd(DateInterval.Day, -nDaysToSave, Now) Then
                            c.Add(.QueueName)
                        End If
                    End With
                Next
                For Each s In c
                    If s Like "private$\" + sMask Then
                        Try
                            System.Threading.Thread.Sleep(100)
                            MessageQueue.Delete(".\" + s)
                        Catch ee As Exception
                            sResult += vbCrLf + ee.Message
                        End Try
                    End If
                Next
                c = Nothing
            Catch e As Exception
                sResult += vbCrLf + e.Message
            End Try
        End If
        Return True
    End Function

End Class