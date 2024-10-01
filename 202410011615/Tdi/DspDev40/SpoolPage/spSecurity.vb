Imports System.Data.SqlClient
Imports System.Data.OleDb

Friend Class Security 'vk 06.04

    Function ForbiddenList(ByRef bp As BuildPage, ByRef oNodeScreen As Node) As String

        If bp.getProperty("Forbid").ToLower <> "true" Then Return ""
        Dim sSql, s As String
#If Not JAVA_MAINSOFT Then
        Dim conn As SqlConnection = Nothing
#Else
        Dim conn As OleDbConnection
#End If
        Dim dsRecords As New DataSet()
        Dim dr As DataRow
        Try
            sSql = "select * from tblForbidden where UserName='" + bp.m_sUser +
                "' and Fil='" + oNodeScreen.Param("fil").Trim +
                "' and Rec='" + oNodeScreen.Param("rec").Trim +
                "' and Code like '__'"
#If Not JAVA_MAINSOFT Then
            conn = New SqlConnection(bp.m_sConnectionString)
            conn.Open()
            Dim dtAdapter As New SqlDataAdapter(sSql, conn)
#Else
            conn = New OleDbConnection(bp.m_sConnectionString)
            conn.Open()
            Dim dtAdapter As New OleDbDataAdapter(sSql, conn)
#End If
            dtAdapter.SelectCommand.CommandType = CommandType.Text
            dtAdapter.Fill(dsRecords)
            With dsRecords.Tables(0).Rows
                If .Count = 0 Then
                    Return ""
                Else
                    s = ";"
                    For Each dr In dsRecords.Tables(0).Rows
                        s += dr("Code").ToString + ";"
                    Next
                    Return s
                End If
            End With
        Catch e As Exception
            'Throw New Exception("1020 DB error. " + vbCrLf + e.Message, e) 'vk 07.06
            Return ";00;01;02;03;04;05;06;07;08;09;10;11;12;13;14;15;16;17;18;19" +
                   ";20;21;22;23;24;25;26;27;28;29;30;31;32;33;34;35;36;37;38;39" +
                   ";40;41;42;43;44;45;46;47;48;49;50;51;52;53;54;55;56;57;58;59" +
                   ";60;61;62;63;64;65;66;67;68;69;70;71;72;73;74;75;76;77;78;79" +
                   ";80;81;82;83;84;85;86;87;88;89;90;91;92;93;94;95;96;97;98;99;"
        Finally
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
            conn = Nothing
        End Try

    End Function

End Class
