Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.IO   ' NET20

Friend Class Color

    'vk 07.04
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

    Function GetColorTable(ByRef bp As BuildPage, ByVal sXMLColor As String) As System.Collections.Hashtable

        Dim sr As New StringReader(sXMLColor)
        Dim rd As New Xml.XmlTextReader(sr)
        Dim id As String
        Dim colColor As New System.Collections.Hashtable()

        Try
            Do While rd.Read
                If rd.NodeType = Xml.XmlNodeType.Element Then
                    id = rd.GetAttribute("id") 'moved here by vk 02.05
                    Select Case rd.Name
                        Case "font", "buttonhtml", "preprinthtml", "scale" '03.11
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
                    End Select
                End If
            Loop
        Catch e1 As Exception
            Throw New Exception("XML of Colors is wrong. " + e1.Message, e1)
        End Try

        Return colColor

    End Function

    Sub AddColors(ByRef colColor As System.Collections.Hashtable, _
            ByVal sParam As String, ByVal s1 As String, ByVal s2 As String) 'vk 05.05


        Dim a As String, b() As String

        For Each a In sParam.Split(";")
#If JAVA_MAINSOFT Then
            If a.IndexOf(":") >= 0 Then
#Else
            If a Like "*:*" Then
#End If
                b = a.Split(":")
                colColor.Add(s1 + b(0) + s2, ColorName(b(1)))
            End If
        Next

    End Sub

    Sub Dispose()
        qConv = Nothing
    End Sub

End Class
