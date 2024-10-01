Option Explicit On 
Imports System.IO
Imports System.Text

Public Class Fq

    Dim fs As FileStream, sr As StreamReader, sw As StreamWriter
    Dim sPath As String, bWrite As FileType, sEncoding As String, bEveryTime As Boolean 'vk 05.16
    Public Enum FileType
        Read = 0
        Write = -1
        Append = 1
    End Enum

    Public Function Receive() As String
        Return sr.ReadToEnd
    End Function
    Public Function ReceiveLine() As String 'vk 07.11
        Return sr.ReadLine
    End Function
    Public Function EOF() As Boolean 'vk 07.11
        Return sr.EndOfStream
    End Function
    Public Sub Send(ByVal sData As String)
        If bEveryTime Then Init()
        sw.WriteLine(sData)
        If bEveryTime Then Commit()
    End Sub
    Public Sub SendExact(ByVal sData As String) 'vk 07.11
        If bEveryTime Then Init()
        sw.Write(sData)
        If bEveryTime Then Commit()
    End Sub
    Private Sub Commit() 'vk 05.16
        Try
            sw.Close()
        Catch
        End Try
        Try
            fs.Close()
        Catch
        End Try
    End Sub
    Private Sub Init() 'vk 05.16
        Select Case bWrite
            Case FileType.Read
                fs = New FileStream(sPath, FileMode.Open, FileAccess.Read, FileShare.Read)
                If sEncoding = "" Then sr = New StreamReader(fs) Else sr = New StreamReader(fs, Encoding.GetEncoding(sEncoding))
            Case FileType.Write
                fs = New FileStream(sPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite)
                If sEncoding = "" Then sw = New StreamWriter(fs) Else sw = New StreamWriter(fs, Encoding.GetEncoding(sEncoding))
            Case FileType.Append
                fs = New FileStream(sPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite)
                If sEncoding = "" Then sw = New StreamWriter(fs) Else sw = New StreamWriter(fs, Encoding.GetEncoding(sEncoding))
        End Select
    End Sub

    Public Sub New(ByVal psPath As String, ByVal pbWrite As FileType, Optional ByVal psEncoding As String = "", Optional ByVal pbEveryTime As Boolean = False) 'vk 01.08
        MyBase.New()
        sPath = psPath
        bWrite = pbWrite
        sEncoding = psEncoding
        bEveryTime = pbEveryTime
        If Not bEveryTime Then Init()
    End Sub
    Public Sub Dispose()
        Try
            If sr IsNot Nothing Then
                sr.Close()
            End If
        Catch
        End Try
        Try
            If sw IsNot Nothing Then
                sw.Close()
            End If
        Catch
        End Try
        Try
            fs.Close()
        Catch
        End Try
        Try
            If sr IsNot Nothing Then
                sr.Dispose()
            End If
        Catch
        End Try
        Try
            If sw IsNot Nothing Then
                sw.Dispose()
            End If
        Catch
        End Try
        Try
            fs.Dispose()
        Catch
        End Try
        Try
            fs = Nothing
        Catch
        End Try
        Try
            sr = Nothing
        Catch
        End Try
        Try
            sw = Nothing
        Catch
        End Try
    End Sub

End Class
