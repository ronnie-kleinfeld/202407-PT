Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.IO   ' NET20

Friend Class Color
    Private qConv As New Conv()

    Private Function ColorName(ByVal s As String) As String 'vk 04.03
        Select Case Left(s, 1)
            Case "0" To "9", "A" To "F"
                If Len(s) = 6 Then Return "#" + s
                'Case Else
                'Return s
        End Select
        Return s
    End Function

    Function GetColorTable(ByRef bp As BuildPage, ByVal sXMLColor As String) As Hashtable

        Dim sr As New StringReader(sXMLColor)
        Dim rd As New Xml.XmlTextReader(sr)
        Dim id As String
        Dim colColor As New Hashtable()

        Try
            Do While rd.Read
                If rd.NodeType = Xml.XmlNodeType.Element Then
                    id = rd.GetAttribute("id") 'moved here by vk 02.05
                    Select Case rd.Name
                        Case "font", "scale" '03.11
                            colColor.Add(rd.Name + "_" + id, rd.GetAttribute("name"))
                        Case "opt_color"
                            colColor.Add("c" + id + "_normal_forg", rd.GetAttribute("normal"))
                            colColor.Add("c" + id + "_blink_forg", rd.GetAttribute("blink"))
                        Case "color"
                            colColor.Add("clr_" + id, rd.GetAttribute("name"))
                        Case "bg" 'vk 09.03
                            colColor.Add("bg_" + id + "_bckg", ColorName(rd.GetAttribute("bck_ground")))
                            colColor.Add("bg_" + id + "_code", rd.GetAttribute("code"))
                        Case "element"
                            colColor.Add("element_" + id, rd.GetAttribute("tag"))
                        Case "graphics" 'vk 04.09
                            colColor.Add("graphics_" + id, rd.GetAttribute("name"))
                            colColor.Add("graphicsH_" + id, rd.GetAttribute("h"))
                            colColor.Add("graphicsW_" + id, rd.GetAttribute("w"))
                            colColor.Add("graphicsT_" + id, rd.GetAttribute("t"))
                            colColor.Add("graphicsL_" + id, rd.GetAttribute("l"))
                            colColor.Add("graphicsPage_" + id, rd.GetAttribute("page"))
                        Case "Icon"
                            colColor.Add("Icon_" + id, rd.GetAttribute("name"))
                            If rd.GetAttribute("fk") IsNot Nothing Then
                                colColor.Add("Icon__" + rd.GetAttribute("fk"), id)
                            End If
                        Case "IconColor"
                            colColor.Add("IconColor_" + id, rd.GetAttribute("name"))
                        Case "ImageFile"
                            colColor.Add("ImageFile_" + id, rd.GetAttribute("name"))
                    End Select
                End If
            Loop
        Catch e1 As Exception
            Throw New Exception("XML of Colors is wrong. " + e1.Message, e1)
        End Try

        Return colColor

    End Function

    Sub AddColors(ByRef colColor As Hashtable, ByVal sParam As String, ByVal s1 As String, ByVal s2 As String)
        Dim a As String, b() As String

        For Each a In sParam.Split(";")
            If a Like "*:*" Then
                b = a.Split(":")
                colColor.Add(s1 + b(0) + s2, ColorName(b(1)))
            End If
        Next
    End Sub

    Function GetHelp_Simple(ByRef bp As BuildPage, ByRef oNode As Node, ByRef oNodeScreen As Node, ByVal sParam As String) As String 'vk 01.12

        Dim s As String = "", s0 As String, i As Integer
        s0 = oNode.Param(sParam).PadRight(75)
        For i = 1 To 51 Step 25
            s += qConv.ConvAlf_Simple(bp, Mid(s0, i, 25).Trim, oNodeScreen, "").Replace("\", "\\")
            If i < 51 Then s += "\n"
        Next
        Return s.Replace("'", "&#39;").Replace("""", "&quot;")

    End Function

    Function GetHelp(ByRef bp As BuildPage, ByRef oNode As Node, ByRef oNodeScreen As Node, sType As String) As String 'vk 09.03

        'vk 07.04
        Dim s As String = "", s0 As String
        If oNode.ParamExists("bua") Then
            Return GetHelp_Simple(bp, oNode, oNodeScreen, "bua")
        End If

        Dim sSql, hky As String
        Dim conn As SqlConnection = Nothing
        Dim dsRecords As New DataSet
        Dim dr As DataRow
        Try
            hky = oNode.Param("hky").PadLeft(9, "0")
            sSql = $"select * from tblHelp 
                     where hpanaf= {hky.Substring(0, 3)}
                           and hpvers= {hky.Substring(3, 4)} 
                           and hpfield= {hky.Substring(7, 9)} 
                           and hptype= {sType} 
                     order by hpnumb"
            conn = New SqlConnection(bp.m_sConnectionString)
            conn.Open()
            Dim dtAdapter As New SqlDataAdapter(sSql, conn)
            dtAdapter.SelectCommand.CommandType = CommandType.Text
            dtAdapter.Fill(dsRecords)
            For Each dr In dsRecords.Tables(0).Rows
                s0 = dr("hptext").ToString.Trim
                If s > "" Then s += "\n"
                s += s0.Replace("\", "\\")
            Next
            Return s.Replace("'", "&#39;").Replace("""", "&quot;")
        Catch e As Exception
            Throw New Exception("1020 DB error. " + vbCrLf + e.Message, e) 'vk 07.06
            Return "" 'vk 01.04
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

    Sub Dispose()
        qConv = Nothing
    End Sub
End Class
