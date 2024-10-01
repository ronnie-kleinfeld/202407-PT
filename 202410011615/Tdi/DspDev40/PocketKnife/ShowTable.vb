'Imports Comtec.TIS
'Imports System.Data
Imports System.Data.OleDb

Public Class ShowTable 'vk 06.16

    Dim c As SortedList

    Public Class Record
        Public sReqType, sMsgId As String
        Public nPhase, nStep, nRDate, nRTime, nNumerator As Integer
        Public sIpAddress As String
        Public nActivity, nHexLob As Integer
        Public sErrorCode, sErrorDesc, sBuffer, sUser As String
    End Class

    Public Sub AddItemFromBuffer(ByVal sData As String)
        Dim r As New Record
        'r...=
        c.Add("", r)
    End Sub

    Public Sub FillFromSql(ByVal sConnString As String, ByVal sSelect As String)
        Dim conn As New OleDbConnection(sConnString)
        conn.Open()
        Dim dsRecords As New DataSet()
        Dim dtAdapter As New OleDbDataAdapter(sSelect, conn)
        dtAdapter.SelectCommand.CommandType = CommandType.Text
        dtAdapter.Fill(dsRecords)
        For Each dr As DataRow In dsRecords.Tables(0).Rows
            Dim r As New Record
            'r...=
            c.Add("", r)
        Next
        Try
            conn.Close()
        Catch ex As Exception
            ex = ex
        End Try
        Try
            conn.Dispose()
        Catch ex As Exception
            ex = ex
        End Try
    End Sub

    Public Sub Clear()
        c = New SortedList
    End Sub

    Public Function Html() As String
        Dim r As Record
        Dim s As New System.Text.StringBuilder()
        s.Append("<table>" & vbCrLf)
        For Each r In c
            s.Append("<th id='56'>" & vbCrLf)
            s.Append("<td id='57'>...</td>" & vbCrLf)
            s.Append("</th>" & vbCrLf)
        Next
        s.Append("</table>" & vbCrLf)
        Return s.ToString
    End Function

    'Private Function KeyString(ByVal nKod As Integer, ByVal nTbno As Integer, ByVal bOrderByCode As Boolean) As String
    '    Return nKod.ToString("00") + nTbno.ToString("000") + IIf(bOrderByCode, "y", "n")
    'End Function

End Class
