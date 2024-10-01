Option Explicit On 
Imports System.Messaging
Imports System.Text
Imports System.IO
Imports System.Runtime.InteropServices

Public Class Mq

    Dim oMq As MessageQueue
    Dim oTimeSpan As TimeSpan
    Dim oTimeSpan0 As TimeSpan = New TimeSpan(0, 0, 0)
    Dim nSeconds As Integer 'vk 12.11
    Dim sError As String
    Dim bTimeOut As Boolean 'vk 02.07
    Dim gsMq As String, gbCreate As Boolean
    Dim oError As Exception 'vk 01.08

    Public Function ReceiveSimple(ByVal sKey As String) As String 'vk 01.08
        sError = ""
        oError = Nothing
        bTimeOut = False
        Do 'vk 09.05
            Dim msg As Message = Nothing
            Try
                If sKey > "" Then
                    msg = oMq.ReceiveByCorrelationId(sKey, oTimeSpan)
                Else
                    msg = oMq.Receive(oTimeSpan)
                End If
                Return msg.Body.ToString 'vk 01.08
            Catch ee As Threading.ThreadAbortException 'vk 01.12
                Threading.Thread.ResetAbort()
            Catch ee As MessageQueueException
                Threading.Thread.Sleep(1)
                Select Case ee.MessageQueueErrorCode
                    Case MessageQueueErrorCode.MessageAlreadyReceived
                    Case MessageQueueErrorCode.IOTimeout
                        bTimeOut = True
                        Return ""
                    Case Else
                        sError = ee.Message + Chr(13) + Chr(10) + ee.StackTrace
                        oError = ee
                        Return ""
                End Select
            Catch ee As Exception
                Threading.Thread.Sleep(1)
                sError = ee.Message + Chr(13) + Chr(10) + ee.StackTrace
                oError = ee
                Return ""
            Finally
                If Not msg Is Nothing Then msg.Dispose()
                msg = Nothing
            End Try
        Loop
    End Function

    Public Function Receive(ByVal sKey As String) As String
        sError = ""
        oError = Nothing
        bTimeOut = False
        Do 'vk 09.05
            Dim msg As Message = Nothing
            Dim sb As StringBuilder
            Try
                If sKey > "" Then
                    msg = oMq.ReceiveByCorrelationId(sKey, oTimeSpan)
                Else
                    msg = oMq.Receive(oTimeSpan)
                End If
                Dim rd As New StreamReader(msg.BodyStream, Encoding.Unicode)
                sb = New StringBuilder()
                Dim c1 As Integer

                Do
                    c1 = rd.Read
                    If c1 < 0 Then Exit Do
                    sb.Append(ChrW(c1))
                Loop
                If Not rd Is Nothing Then rd.Dispose()
                rd = Nothing
                Return sb.ToString
            Catch ee As Threading.ThreadAbortException 'vk 01.12
                Threading.Thread.ResetAbort()
            Catch ee As MessageQueueException
                Threading.Thread.Sleep(1)
                Select Case ee.MessageQueueErrorCode
                    Case MessageQueueErrorCode.MessageAlreadyReceived
                    Case MessageQueueErrorCode.IOTimeout
                        bTimeOut = True
                        Return ""
                    Case Else
                        sError = ee.Message + Chr(13) + Chr(10) + ee.StackTrace
                        oError = ee
                        Return ""
                End Select
            Catch ee As Exception
                Threading.Thread.Sleep(1)
                sError = ee.Message + Chr(13) + Chr(10) + ee.StackTrace
                oError = ee
                Return ""
            Finally
                If Not msg Is Nothing Then msg.Dispose()
                msg = Nothing
                sb = Nothing
            End Try
        Loop
    End Function

    Public Sub Send(ByVal sData As String, ByVal sKey As String)
        sError = ""
        oError = Nothing
        Dim msg As Message = Nothing
        Try
            msg = New Message()
            If sKey > "" Then msg.CorrelationId = sKey
            Dim wr As New StreamWriter(msg.BodyStream, Encoding.Unicode)
            wr.Write(sData)
            wr.Flush()
            oMq.Send(msg)
        Catch ee As Exception
            sError = ee.Message + Chr(13) + Chr(10) + ee.StackTrace
            oError = ee
        Finally
            If Not msg Is Nothing Then
                msg.Dispose()
            End If
            msg = Nothing
        End Try
    End Sub

    Public Function Receive() As String
        Dim tEnd As Date = Now.AddSeconds(nSeconds) 'vk 12.11
        sError = ""
        oError = Nothing
        bTimeOut = False
        Do
            Try
                oMq.Formatter = New XmlMessageFormatter(New Type() {GetType(MyMsg)})
                Return CType(oMq.Receive(oTimeSpan0).Body, MyMsg).s
            Catch ee As Threading.ThreadAbortException 'vk 12.11
                Threading.Thread.ResetAbort()
            Catch ee As MessageQueueException
                Threading.Thread.Sleep(1)
                Select Case ee.MessageQueueErrorCode
                    Case MessageQueueErrorCode.IOTimeout
                    Case Else
                        sError = ee.Message + Chr(13) + Chr(10) + ee.StackTrace
                        oError = ee
                        Return ""
                End Select
            Catch ee As Exception
                Threading.Thread.Sleep(1)
                sError = ee.Message + Chr(13) + Chr(10) + ee.StackTrace
                oError = ee
                Return ""
            Finally
                If Not oMq Is Nothing Then
                    oMq.Formatter = Nothing
                End If
            End Try
            'vk 12.11
            If Now >= tEnd Then Exit Do
            Try
                Threading.Thread.Sleep(10) 'vk 02.12
            Catch ee As Threading.ThreadAbortException
                Threading.Thread.ResetAbort()
            Catch ee As Exception
            End Try
        Loop
        bTimeOut = True
        Return ""
    End Function

    Public Sub Send(ByVal sData As String)
        sError = ""
        oError = Nothing
        Dim mm As MyMsg
        Try
            mm = New MyMsg()
            mm.s = sData
            oMq.Send(mm)
            mm = Nothing
        Catch ee As Exception
            sError = ee.Message + Chr(13) + Chr(10) + ee.StackTrace
            oError = ee
        Finally
            mm = Nothing
        End Try
    End Sub

    Public Sub New(ByVal sMq As String, ByVal sMsgCaption As String, ByVal bCreate As Boolean, _
            ByVal nTimeOut As Integer)
        MyBase.New()
        sError = ""
        oError = Nothing
        gsMq = sMq
        gbCreate = bCreate
        Try
            If bCreate Then
                oMq = MessageQueue.Create(".\PRIVATE$\" + sMq)
                oMq.SetPermissions("Everyone", MessageQueueAccessRights.FullControl) 'vk 06.14
            Else
                oMq = New MessageQueue(".\PRIVATE$\" + sMq)
            End If
            oMq.Formatter = New XmlMessageFormatter(New Type() {GetType(String)})
            oTimeSpan = New System.TimeSpan(0, 0, nTimeOut)
            nSeconds = nTimeOut 'vk 12.11
        Catch ee As Exception
            sError = ee.Message + Chr(13) + Chr(10) + ee.StackTrace
            oError = ee
        End Try
    End Sub
    Public Sub SetTimeOut(ByVal nTimeOut As Integer)
        oTimeSpan = New System.TimeSpan(0, 0, nTimeOut)
        nSeconds = nTimeOut 'vk 12.11
    End Sub
    Public Function Exists() As Boolean 'vk 02.18
        Try
            Return MessageQueue.Exists(".\PRIVATE$\" + gsMq)
        Catch ee As Exception
            sError = ee.Message + Chr(13) + Chr(10) + ee.StackTrace
            oError = ee
            Return False
        End Try
    End Function
    Public Sub Delete() 'vk 02.18
        Try
            MessageQueue.Delete(".\PRIVATE$\" + gsMq)
        Catch ee As Exception
            sError = ee.Message + Chr(13) + Chr(10) + ee.StackTrace
            oError = ee
        End Try
    End Sub

    Public Sub Dispose()
        sError = ""
        oError = Nothing
        Try
            If gbCreate AndAlso gsMq > "" Then 'vk 02.10
                MessageQueue.Delete(".\PRIVATE$\" + gsMq)
            End If
            If Not oMq Is Nothing Then
                oMq.Formatter = Nothing
                oMq.Close()
                oMq.Dispose()
                oMq = Nothing
            End If
            oTimeSpan = Nothing
            oTimeSpan0 = Nothing 'vk 12.11
        Catch ee As Exception
            sError = ee.Message + Chr(13) + Chr(10) + ee.StackTrace 'vk 02.07
            oError = ee
        End Try
    End Sub
    Public Sub Dispose(ByVal bMustDelete As Boolean) 'vk 02.07
        sError = ""
        oError = Nothing
        Try
            If gsMq > "" Then 'vk 02.10
                MessageQueue.Delete(".\PRIVATE$\" + gsMq)
            End If
            If Not oMq Is Nothing Then
                oMq.Formatter = Nothing
                oMq.Close()
                oMq.Dispose()
                oMq = Nothing
            End If
            oTimeSpan = Nothing
        Catch ee As Exception
            sError = ee.Message + Chr(13) + Chr(10) + ee.StackTrace
            oError = ee
        End Try
    End Sub

    Public Function ErrorText() As String
        Return sError
    End Function
    Public Function OriginalException() As Exception
        Return oError
    End Function
    Public Function TimeOut() As Boolean
        Return bTimeOut
    End Function
    Public Class MyMsg 'vk 06.05
        Public s As String
    End Class

End Class
