Option Explicit On
Imports ADODB
Imports System.Threading

Public Class Dq

    Dim gnTimeOutMilliSec As Integer, gnSleepMilliSec As Integer
    Dim gbWithTimeOut As Boolean
    Dim sError As String
    Dim conn As ADODB.Connection
    Dim rs As ADODB.Recordset
    Dim Coll As New Collection() 'vk 05.05
    Dim gnMaxInMemory As Integer
    'Dim cmd As ADODB.Command
    'Dim prm As ADODB.Parameter

    Public Function Receive() As String
        Dim sData As String = ""
        sError = ""
        Try
            If Coll.Count > 0 Then
                sData = Coll(1)
                Coll.Remove(1)
                Exit Try
            End If
            Dim tStart As Date = Now
            sData = ""
            Do
                rs.Requery()
                If Not rs.EOF Then
                    sData = rs.Fields(0).Value
                    Do Until rs.EOF OrElse Coll.Count >= gnMaxInMemory
                        rs.MoveNext()
                        Coll.Add(rs.Fields(0).Value)
                    Loop
                    Exit Try
                End If
                Thread.Sleep(gnSleepMilliSec)
            Loop While (Not gbWithTimeOut) OrElse Now.Ticks - tStart.Ticks < gnTimeOutMilliSec * 10000
        Catch ee As Exception
            sError = ee.Message + Chr(13) + Chr(10) + ee.StackTrace
        End Try
        Return sData
    End Function

    Public Sub Send(ByVal sData As String)
        sError = ""
        Try
            rs.AddNew("Data", sData)
            'cmd.CommandType = CommandTypeEnum.adCmdText
            'cmd.CommandText = "insert into ... (Data) values (?)"
            'prm.Value = sData
            'cmd.Parameters.Append(prm)
            'cmd.Execute()
        Catch ee As Exception
            sError = ee.Message + Chr(13) + Chr(10) + ee.StackTrace
        End Try
    End Sub

    Public Function MayDestroy() As Boolean
        Return Coll.Count = 0
    End Function

    Public Sub New(ByVal sSystem As String, ByVal sLib As String, ByVal sDq As String,
            ByVal nTimeOutMilliSec As Integer, ByVal nSleepMilliSec As Integer, ByVal nLen As Integer,
            ByVal sPurpose As String, ByVal sUser As String, ByVal sPass As String,
            ByVal nMaxInMemory As Integer)
        MyBase.New()
        sError = ""
        Try
            gbWithTimeOut = (nTimeOutMilliSec > 0)
            gnTimeOutMilliSec = nTimeOutMilliSec
            If nSleepMilliSec < 100 Then nSleepMilliSec = 100 'vk 05.05
            gnSleepMilliSec = nSleepMilliSec
            gnMaxInMemory = nMaxInMemory 'vk 05.05
            conn = New ADODB.Connection()
            conn.Open("Provider=IBMDA400;Data Source=" & sSystem & ";", sUser, sPass)
            conn.CommandTimeout = 1
            Dim Rcds As Object = Nothing
            Dim s As String = ""
            s &= "OPEN DATAQUEUE /QSYS.LIB/" & sLib & ".LIB/" & sDq _
                & ".DTAQ(Data CHARACTER(" & nLen.ToString
            s &= ")) FOR " & sPurpose
            rs = conn.Execute(s, Rcds, 1)
            'cmd = New ADODB.Command()
            'prm = New ADODB.Parameter()
        Catch ee As Exception
            sError = ee.Message + Chr(13) + Chr(10) + ee.StackTrace
        End Try
    End Sub
    Public Sub New(ByVal sSystem As String, ByVal sUser As String, ByVal sPass As String)
        'check connection only
        MyBase.New()
        sError = ""
        Try
            conn = New ADODB.Connection()
            conn.Open("Provider=IBMDA400;Data Source=" & sSystem & ";", sUser, sPass)
        Catch ee As Exception
            sError = ee.Message + Chr(13) + Chr(10) + ee.StackTrace
        End Try
    End Sub

    Public Sub Dispose()
        Try
            rs.Close()
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            rs = Nothing
            conn = Nothing
            'cmd = Nothing
            'prm = Nothing
        Catch
        End Try
    End Sub

    Public Function ErrorText() As String
        ErrorText = sError
    End Function

End Class
