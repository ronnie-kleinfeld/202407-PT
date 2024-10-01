Friend Class Spool

    Private qStyle As New StyleModule()
    Private qMulti As New Multi()
    Private qConv As New Conv()
    Const PREPRINT As String = "~XXXFORMXXX~"
    Private nMaxHeight, nCurTop, nCurTop_Page, nInterline As Integer
    Private sMaxClass As String
    Private oTabPositions As New Collection()
    Private oLinePixels As New Collection()
    Private bBorderBottom As Boolean = False
    Private bBorderTop_Outer As Boolean = False
    Private bBorderTop_Inner As Boolean = False
    Private sBorder1, sBorder4 As String

    Private Function SpoolOneRow(ByRef bp As BuildPage, ByVal sLastSymbol As String,
            ByVal nFullWidth As Integer, ByVal sTableStyle As String,
            ByVal s As String, ByVal sMask As String, ByRef oNodeScreen As Node,
            ByVal bDifferentFonts As Boolean, ByVal bFixedLineHeight As Boolean) As String 'vk 04.09
        Dim sResult As String = ""
        'Dim oStyle As Style, oControl As Control
        Dim j1, j2 As Integer
        Dim nAddToWidth As Single 'vk 04.09
        Dim sClass, sChar As String
        'If bPdf Then
        'sResult += "<!-- debug: len(s)=" + Len(s).ToString + " -->" + vbCrLf
        sResult += "<td colspan=" + s.Length.ToString + " style='width:" _
                + (s.Length * bp.iScaleWidth + 1).ToString _
                + "px;'><table cellspacing=0 cellpadding=0 style='" + sTableStyle + "'><tr>" + vbCrLf
        'End If
        If sMask.Trim = "" Then
            sResult += SpoolField(bp, s, "Usual", 0, 1, sMask.Length, oNodeScreen, True, True, bDifferentFonts, bFixedLineHeight) + vbCrLf
        Else
            Do
                'vk 02.11
                Dim i As Integer = InStr(sMask, "++")
                If i = 0 Then Exit Do
                Mid(sMask, i, 1) = " "
                Mid(s, i, 1) = " "
            Loop
            j1 = 1
            Do
                j2 = InStr(j1, sMask, "+") - 1
                If j2 < 0 Then j2 = sMask.Length
                sChar = " "
                If j1 > 1 Then
                    sChar = Mid(s, j1 - 1, 1)
                End If
                sClass = ClassByChar(bp, sChar)
                nAddToWidth = 0
                'If bPdf Then
                'vk 04.09
                If j1 = 2 Then
                        nAddToWidth = 1
                    End If
                    If j2 - j1 + 1 > 0 Then
                        sResult += SpoolField(bp, Mid(s, j1, j2 - j1 + 1), sClass, nAddToWidth, j1, sMask.Length, oNodeScreen, j1 <= 2, j2 = sMask.Length, bDifferentFonts, bFixedLineHeight) + vbCrLf
                    End If
                'Else
                '    If j1 > 1 Then
                '        nAddToWidth = 1
                '    End If
                '    sResult += SpoolField(bp, Mid(s, j1, j2 - j1 + 1), sClass, nAddToWidth, j1, sMask.Length, oNodeScreen, j1 = 1, j2 = sMask.Length, bDifferentFonts, bFixedLineHeight) + vbCrLf
                'End If
                j1 = j2 + 2
            Loop Until j1 > sMask.Length
        End If
        'If bPdf Then
        sResult += "</tr></table></td>" + vbCrLf
        'End If
        Return sResult
    End Function
    Private Function SpoolField(ByRef bp As BuildPage, ByRef s As String, ByVal sClass As String,
            ByVal nAddToWidth As Integer, ByVal nPos As Integer, ByVal nLineLen As Integer,
            ByRef oNodeScreen As Node, ByVal bEdgeL As Boolean, ByVal bEdgeR As Boolean,
            ByVal bDifferentFonts As Boolean, ByVal bFixedLineHeight As Boolean) As String 'vk 12.05

        Dim oNode As Node, oControl As Control, sResult As String
        Dim oStyle As Style, sStyle As String
        oNode = New Node()
        'oNode.AddIfNo("pht", 1)
        Dim bText As Boolean = False
        Dim bPseudo254 As Boolean = oNodeScreen.ParamVal("flang") = 0 'vk 08.08
        oStyle = New Style()
        'If oNodeScreen.ParamVal("flr") = 1 Then
        'vk 08.07
        Dim i As Integer
        Dim sNumeric As String = bp.getProperty("NumericSymbols") + " 0123456789"
        For i = 1 To Len(s)
            If sNumeric.IndexOf(Mid(s, i, 1)) < 0 Then
                bText = True
                Exit For
            End If
        Next
        If Not bText Then
            If oNodeScreen.ParamVal("flr") = 1 Then 'moved here by vk 12.09
                oStyle.Add("text-align", "right")
            End If
        End If
        'If bPdf Then
        'vk 04.09
        If bDifferentFonts Then 'vk 12.09
            If bFixedLineHeight Then
                oStyle.Add("vertical-align", "top") 'vk 08.10
            Else
                oStyle.Add("vertical-align", "bottom")
            End If
        End If
        Dim nWidth As Single = (s.Length + nAddToWidth) * bp.iScaleWidth 'Single vk 05.15
        If bp.getProperty("PdfProductVersion") = "1" Then
            'vk 05.15
            If Not bEdgeL Then nWidth += bp.iScaleWidth / 2
            If Not bEdgeR Then nWidth += bp.iScaleWidth / 2
        Else
            If bEdgeL Then
                oStyle.Add("padding-left", nAddToWidth * bp.iScaleWidth) 'vk 12.09
            Else
                oStyle.Add("padding-left", nAddToWidth * bp.iScaleWidth + bp.iScaleWidth / 2) 'vk 12.09
                nWidth += bp.iScaleWidth / 2
            End If
            If bEdgeR Then
                oStyle.Add("padding-right", 0)
            Else
                oStyle.Add("padding-right", bp.iScaleWidth / 2)
                nWidth += bp.iScaleWidth / 2
            End If
        End If
        oStyle.Add("width", nWidth)
        If bFixedLineHeight Then
            oStyle.Add("height", bp.HeightByClass("Usual", oNodeScreen) + nInterline) 'vk 08.10
        Else
            oStyle.Add("height", bp.HeightByClass(sClass, oNodeScreen) + nInterline) 'vk 06.09
        End If
        If oTabPositions.Count > 1 Then
            If nPos - 1 >= oTabPositions(1) AndAlso nPos + Len(s) <= oTabPositions(oTabPositions.Count) Then
                If bBorderTop_Inner Then
                    oStyle.Add("border-top", sBorder1)
                End If
                If bBorderTop_Outer Then
                    oStyle.Add("border-top", sBorder4)
                End If
                If bBorderBottom Then
                    oStyle.Add("border-bottom", sBorder4)
                End If
            End If
            If nPos + Len(s) = oTabPositions(oTabPositions.Count) Then
                oStyle.Add("border-right", sBorder4)
            End If
            'Dim i As Integer
            For i = 1 To oTabPositions.Count - 1
                If nPos - 1 = oTabPositions(i) Then
                    If i = 1 Then
                        oStyle.Add("border-left", sBorder4)
                    Else
                        oStyle.Add("border-left", sBorder1)
                    End If
                End If
            Next
        Else
            If bBorderTop_Inner Then
                oStyle.Add("border-top", sBorder1) 'vk 12.09
            End If
            If bBorderTop_Outer Then
                oStyle.Add("border-top", sBorder4) 'vk 12.09
            End If
        End If

        sStyle = oStyle.FullCode
        oStyle = Nothing

        oControl = New Control(sStyle)
        oControl.Add("class", "OutCom" + sClass)
        If s.Trim = "" Then
            s = "&nbsp;"
        Else
            If Not bText AndAlso Not oNodeScreen.Plain(bp) Then
                s = s.Trim
            ElseIf sClass = "Size3" OrElse sClass = "Size2" OrElse sClass = "Size2_Underline" Then
                If oNodeScreen.ParamVal("flr") = 1 Then
                    s = RTrim(s) 'vk 11.11
                Else
                    s = LTrim(s)
                End If
            End If
            s = qConv.ConvAlf(bp, s, "", "AS2PC",
                IIf(oNodeScreen.ParamVal("flr") = 1 OrElse bPseudo254, "ltr", "rtl"),
                False,
                False, False, False, oNodeScreen.Param("flr"))
            qConv.MakeOk(s, True)
            If bFixedLineHeight Then
                s = "<div style='position:relative;unicode-bidi:bidi-override;'>" + s + "</div>"
            End If
        End If
        If bp.getProperty("PdfProductVersion") = "1" Then
            'vk 05.15
            s = s.Replace("&nbsp;", " ")
        End If
        oControl.Add("value", s)
        sResult = oControl.FullCodeSpan(bp, False, "td", False, True)
        If Not oControl Is Nothing Then oControl.Dispose()
        If Not oNode Is Nothing Then oNode.Dispose()
        Return sResult
    End Function

    Private Function ClassByChar(ByRef bp As BuildPage, ByVal sChar As String) As String 'vk 05.07
        Select Case AscW(sChar)
            Case bp.m_nCode_Bold : Return "Bold"
            Case bp.m_nCode_Size2 : Return "Size2"
            Case bp.m_nCode_Size3 : Return "Size3"
            Case bp.m_nCode_Half : Return "Half"
            Case bp.m_nCode_Bold_Underline : Return "Bold_Underline"
            Case bp.m_nCode_Size2_Underline : Return "Size2_Underline"
            Case bp.m_nCode_Underline : Return "Underline"
            Case Else : Return "Usual"
        End Select
    End Function

    Sub Dispose()
        qStyle = Nothing
        qMulti.Dispose()
        qMulti = Nothing
        qConv = Nothing
    End Sub

End Class
