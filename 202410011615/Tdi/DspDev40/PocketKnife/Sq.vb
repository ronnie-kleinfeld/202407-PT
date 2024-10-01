Imports System.Data.SqlClient
Imports System.Data.OleDb

Public Class Sq

    Dim sError As String = ""
    Dim conn As SqlConnection, adapter As SqlDataAdapter, ds As DataSet
    Dim bui As SqlCommandBuilder, t As DataTable

    Public Function RowCount() As Integer
        Try
            Return t.Rows.Count
        Catch
            Return 0
        End Try
    End Function

    Public Function HasField(ByVal sField As String) As Boolean 'vk 03.08
        Try
            Dim c As DataColumn
            Dim b As Boolean = False
            For Each c In t.Columns
                If c.ColumnName = sField Then b = True
            Next
            Return b
        Catch ee As Exception
            sError = ee.Message + Chr(13) + Chr(10) + ee.StackTrace
            Return False
        End Try
    End Function

    Public Function Receive(ByVal sField As String) As Object
        Try
            If t.Rows.Count = 0 Then
                Return Nothing
            Else
                Return t.Rows(0)(sField)
            End If
        Catch ee As Exception
            sError = ee.Message + Chr(13) + Chr(10) + ee.StackTrace
            Return Nothing
        End Try
    End Function

    Public Sub Send(ByVal sField As String, ByVal sData As Object)
        Try
            If t.Rows.Count = 0 Then
                sError = "NO DATA"
            Else
                t.Rows(0)(sField) = sData
                adapter.Update(t)
            End If
        Catch ee As Exception
            sError = ee.Message + Chr(13) + Chr(10) + ee.StackTrace
        End Try
    End Sub

    Public Sub Run(ByVal sCommand As String) 'vk 02.07
        Try
            Dim cmd As New SqlCommand(sCommand, conn)
            cmd.CommandType = CommandType.Text
            Do 'vk 09.12
                Try
                    cmd.ExecuteNonQuery()
                    Exit Do
                Catch ee As System.Threading.ThreadAbortException
                    System.Threading.Thread.ResetAbort()
                Catch ee As System.InvalidOperationException
                    'vk 09.14
                    If conn.State <> ConnectionState.Open Then
                        conn.Open()
                    Else
                        sError = ee.Message + Chr(13) + Chr(10) + ee.StackTrace
                        Exit Do
                    End If
                End Try
            Loop
        Catch ee As Exception
            sError = ee.Message + Chr(13) + Chr(10) + ee.StackTrace
        End Try
    End Sub

    Public Sub New(ByVal sSqlServer As String, ByVal sDataBase As String,
            ByVal sSelect As String, ByVal sUser As String, ByVal sPass As String, ByVal sConnStrAddition As String)
        MyBase.New()
        Try
            Dim sConnStr As String

            If sUser = "" Then 'ntg 10.07.24 vladi's change regarding sql migration for bituah haklai
                sConnStr = "Data Source=" + sSqlServer + ";Initial Catalog=" + sDataBase +
                ";Integrated Security=true;" + sConnStrAddition
            Else
                sConnStr = "Data Source=" + sSqlServer + ";Initial Catalog=" + sDataBase +
                ";User ID=" + sUser + ";Password=" + sPass + ";" + sConnStrAddition
            End If
            conn = New SqlConnection(sConnStr) 'vk 03.08
            conn.Open()
            If sSelect > "" Then SetSelect(sSelect)
        Catch ee As Exception
            sError = ee.Message + Chr(13) + Chr(10) + ee.StackTrace
        End Try
    End Sub
    Public Sub SetSelect(ByVal sSelect As String)
        Try
            adapter = New SqlDataAdapter(sSelect, conn)
            adapter.SelectCommand.CommandType = CommandType.Text
            ds = New DataSet()
            adapter.Fill(ds)
            t = ds.Tables(0)
            bui = New SqlCommandBuilder(adapter)
        Catch ee As Exception
            sError = ee.Message + Chr(13) + Chr(10) + ee.StackTrace
        End Try
    End Sub

    Public Sub Dispose()
        Try
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        Catch
        End Try
        Try
            If Not conn Is Nothing Then
                conn.Dispose()
            End If
        Catch
        End Try
        Try
            t.Dispose()
            t = Nothing
        Catch
        End Try
        Try
            bui.Dispose()
            bui = Nothing
        Catch
        End Try
        Try
            ds.Dispose()
            ds = Nothing
        Catch
        End Try
        Try
            adapter.Dispose()
            adapter = Nothing
        Catch
        End Try
        Try
            If Not conn Is Nothing Then
                conn.Dispose()
            End If
            conn = Nothing
        Catch
        End Try
    End Sub

    Public Function ErrorText() As String
        ErrorText = sError
    End Function

End Class
