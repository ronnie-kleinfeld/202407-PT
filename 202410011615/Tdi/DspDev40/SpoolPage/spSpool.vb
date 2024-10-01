Friend Class Spool

    Private qStyle As New StyleModule() 'vk 03.04
    Private qMulti As New Multi() 'vk 02.05
    Private qConv As New Conv() 'vk 01.06
    Const PREPRINT As String = "~XXXFORMXXX~"
    Private nMaxHeight, nCurTop, nCurTop_Page, nInterline As Integer
    Private sMaxClass As String
    'vk 08.07
    Private oTabPositions As New Collection()
    Private oLinePixels As New Collection()
    'vk 04.09
    Private bBorderBottom As Boolean = False
    'vk 06.10
    Private bBorderTop_Outer As Boolean = False
    Private bBorderTop_Inner As Boolean = False
    Private sBorder1, sBorder4 As String

    Sub FormSpool(ByRef bp As BuildPage, ByRef sOut As System.Text.StringBuilder,
            ByRef oNodeScreen As Node, ByVal nPage As Integer, ByVal bNetto As Boolean,
            ByVal bBkg As Boolean, ByVal bBrowser As Boolean, ByVal bPdf As Boolean) 'vk 01.06

        Dim i As Integer ', s As String

        If bp.cPrintServer.Count = 0 Then Return 'no preprint and no usual spool

        's = bp.getProperty(oNodeScreen.Param("pset") + "_SpoolTopPixel")
        'If s = "" Then
        '    s = bp.getProperty("SpoolTopPixel")
        'End If
        nCurTop = Val(bp.getPropertyPset("SpoolTopPixel", oNodeScreen))
        's = bp.getProperty(oNodeScreen.Param("pset") + "_SpoolInterline")
        'If s = "" Then
        '    s = bp.getProperty("SpoolInterline")
        'End If
        nInterline = Val(bp.getPropertyPset("SpoolInterline", oNodeScreen))
        'vk 06.09 till here
        nCurTop_Page = nCurTop
        If nPage = 0 Then
            For i = 1 To bp.m_nPages
                If i > 1 AndAlso bp.getProperty("UsualPageHeight") > "" Then
                    'vk 01.07, 02.07
                    nCurTop_Page += bp.getPropertyVal("UsualPageHeight")
                    nCurTop = nCurTop_Page
                End If
                SpoolRows(bp, sOut, oNodeScreen, i, bNetto, bBkg, bBrowser, bPdf, i = bp.m_nPages)
                'vk 05.20
                If bp.getPropertyPset("AddPage", oNodeScreen) > "" Then
                    sOut.Append(bp.getProperty("BetweenTables") + vbCrLf)
                    sOut.Append(bp.getPropertyPset("AddPage", oNodeScreen) + vbCrLf)
                End If
            Next
        Else
            SpoolRows(bp, sOut, oNodeScreen, nPage, bNetto, bBkg, bBrowser, bPdf, True)
        End If

        Dim oNode As Node
        oNode = New Node()
        oNode.AddIfNo("wd", Len(bp.cPrintServer(1)) - 7)
        bp.m_nSpoolWidth = qStyle.iwid_fun(bp, oNode, oNodeScreen)
        bp.m_nSpoolHeight = nCurTop 'vk 01.06
        If Not oNode Is Nothing Then oNode.Dispose()
        oNode = Nothing

    End Sub
    Private Function ValByKey(ByRef bp As BuildPage, ByVal sKey As String) As String 'vk 02.06
        Dim s As String, i As Integer
        Try
            s = bp.cPrintServer(sKey)
            'i = InStr(s, Mid(sKey, 6))
            's = (Mid(s, i + Len(sKey) - 4)).Trim
            'vk 01.08
            i = InStr(s, "~")
            If i = 0 Then Return ""
            i = InStr(i + 1, s, "~")
            If i = 0 Then Return ""
            Return Mid(s, i + 1).Trim
        Catch
            Return ""
        End Try
    End Function
    Private Sub SpoolRows(ByRef bp As BuildPage,
            ByRef sOut As System.Text.StringBuilder, ByRef oNodeScreen As Node,
            ByVal nPage As Integer, ByVal bNetto As Boolean, ByVal bBkg As Boolean,
            ByVal bBrowser As Boolean, ByVal bPdf As Boolean, ByVal bLastPage As Boolean)

        Dim s, s0 As String, i, n As Integer
        Dim nLines As Integer = 0
        Dim nFullWidth As Integer
        Dim i1, i2 As Integer
        Dim sValue As String
        Dim nNum As Integer
        Dim bFixedLineHeight As Boolean = bp.getPropertyPset("FixedLineHeight", oNodeScreen) = "true" 'vk 08.10
        Dim sImgIndic1 As String = "#@&" 'vk 03.11
        Dim sImgIndic2 As String = bp.getProperty("ImgIndic") 'vk 03.11

        'vk 11.07
        If bp.getProperty("IncludeXml") = "true" Then
            For Each s In bp.cPrintServer
                If Val(Left(s, 4)) = nPage Then
                    s0 = s
                    qConv.MakeOk(s0)
                    sOut.Append("<!-- " + s0 + " -->" + vbCrLf)
                End If
            Next
        End If
        'moved here by vk 01.10
        For Each s In bp.cPrintServer
            If Val(Left(s, 4)) = nPage Then
                n = Val(Mid(s, 5, 2))
                If n > nLines Then nLines = n
                nFullWidth = (Len(s) - 7) * bp.iScaleWidth + 1 'vk 03.09
            End If
        Next

        Try
            s = bp.cPrintServerPreprint(nPage.ToString("0000"))
        Catch
            Exit Sub 'vk 05.07, 08.07
        End Try

        If s > "" Then 'If Val(s) > 0 Then
            Dim bExit As Boolean = True 'vk 11.08
            'If Len(s) = 3 Then
            s = "preprinthtml_" + oNodeScreen.Param("cli") + s
            If bp.m_colColor.Item(s) Is Nothing Then
                bExit = oNodeScreen.Param("nfr") <> "P"
            Else
                s = qMulti.FromFile(bp.m_sMapPath + "\Color\" + bp.m_colColor.Item(s).ToString)
                If s > "" Then
                    'clever form - from here
                    If Not oNodeScreen.ParamYes("debug") Then
                        Do 'vk 12.07 - support for loops
                            i1 = InStr(s, "<!-- cycle ")
                            If i1 <= 0 Then Exit Do
                            i2 = InStr(i1, s, "<!-- /cycle -->")
                            If i2 > 0 Then
                                Dim i11, nStep, nIndex, j0, j1, j2, j3, nInPar, nCount As Integer
                                Dim sCaption, sBody, sBodyAfter, sIndexField, sIndex, sResult, sInPar, sCount As String
                                Dim a As String, b() As String
                                i11 = InStr(i1, s, " -->")
                                sCaption = Mid(s, i1 + 11, i11 - i1 - 11)
                                sBody = Mid(s, i11 + 4, i2 - i11 - 4)
                                sIndexField = ""
                                nStep = 1
                                sResult = ""
                                For Each a In sCaption.Split(" ")
                                    b = a.Split("=")
                                    Select Case b(0)
                                        Case "field" : sIndexField = b(1)
                                        Case "step" : nStep = Val(b(1))
                                    End Select
                                Next
                                nIndex = 1
                                Do
                                    sCount = nPage.ToString("0000") + "_" + sIndexField
                                    Try
                                        nCount = bp.cPrintServerCount(sCount)
                                    Catch
                                        nCount = 0
                                    End Try
                                    Select Case nIndex
                                        Case Is > nCount
                                            Exit Do
                                        Case 1
                                            sIndex = ""
                                        Case Else
                                            sIndex = nIndex.ToString("00")
                                    End Select
                                    'If ValByKey(bp, nPage.ToString("0000") + "_" + sIndexField + sIndex) = "" Then Exit Do
                                    sBodyAfter = sBody
                                    Do
                                        j2 = InStr(sBodyAfter, ")@}")
                                        If j2 <= 0 Then Exit Do
                                        j1 = InStrRev(Left(sBodyAfter, j2), "(")
                                        j0 = InStrRev(Left(sBodyAfter, j2), "{@")
                                        sInPar = Mid(sBodyAfter, j1 + 1, j2 - j1 - 1)
                                        'sInPar = Replace(sInPar, "i", nIndex.ToString)
                                        'nInPar = eval(sInPar)
                                        Select Case True
                                            Case sInPar = "i" : nInPar = nIndex
                                            Case sInPar Like "i+*" : nInPar = nIndex + Val(Mid(sInPar, 3))
                                            Case sInPar Like "i-*" : nInPar = nIndex - Val(Mid(sInPar, 3))
                                            Case Else : nInPar = 0
                                        End Select
                                        If j0 + 2 = j1 Then
                                            sBodyAfter = Left(sBodyAfter, j0 - 1) + nInPar.ToString + Mid(sBodyAfter, j2 + 3)
                                        Else
                                            If nInPar = 1 Then
                                                sInPar = ""
                                            Else
                                                sInPar = nInPar.ToString("00")
                                            End If
                                            sBodyAfter = Left(sBodyAfter, j1 - 1) + sInPar + Mid(sBodyAfter, j2 + 1)
                                        End If
                                    Loop
                                    j3 = 1
                                    Do 'indexes in ifs in loop
                                        j0 = InStr(j3, sBodyAfter, "<!-- maybe ")
                                        If j0 = 0 Then Exit Do
                                        j3 = InStr(j0, sBodyAfter, " -->")
                                        Dim sIfCaption As String = Mid(sBodyAfter, j0, j3 - j0 + 4)
                                        Do
                                            j1 = InStr(sIfCaption, "(")
                                            If j1 = 0 Then Exit Do
                                            j2 = InStr(j1, sIfCaption, ")")
                                            sInPar = Mid(sIfCaption, j1 + 1, j2 - j1 - 1)
                                            Select Case True
                                                Case sInPar = "i" : nInPar = nIndex
                                                Case sInPar Like "i+*" : nInPar = nIndex + Val(Mid(sInPar, 3))
                                                Case sInPar Like "i-*" : nInPar = nIndex - Val(Mid(sInPar, 3))
                                                Case Else : nInPar = 0
                                            End Select
                                            If nInPar = 1 Then
                                                sInPar = ""
                                            Else
                                                sInPar = nInPar.ToString("00")
                                            End If
                                            sIfCaption = Left(sIfCaption, j1 - 1) + sInPar + Mid(sIfCaption, j2 + 1)
                                        Loop
                                        sBodyAfter = Left(sBodyAfter, j0 - 1) + sIfCaption + Mid(sBodyAfter, j3 + 4)
                                    Loop
                                    sResult += sBodyAfter
                                    nIndex += nStep
                                Loop
                                s = Left(s, i1 - 1) + sResult + Mid(s, i2 + 15)
                            End If
                        Loop
                        Do 'vk 12.07 - support for ifs
                            i1 = InStr(s, "<!-- maybe ")
                            If i1 <= 0 Then Exit Do
                            i2 = InStr(i1, s, "<!-- /maybe -->")
                            If i2 > 0 Then
                                Dim i11 As Integer, bShow As Boolean
                                Dim sCaption, sBody, sCond, sCondField, sCondValue As String
                                Dim sCondValue2 As String = ""
                                Dim a As String, b() As String
                                i11 = InStr(i1, s, " -->")
                                sCaption = Mid(s, i1 + 11, i11 - i1 - 11)
                                sBody = Mid(s, i11 + 4, i2 - i11 - 4)
                                sCondField = ""
                                sCond = ""
                                For Each a In sCaption.Split(" ")
                                    b = a.Split("=")
                                    Dim b1 As String
                                    If UBound(b) >= 1 Then
                                        b1 = b(1)
                                    Else
                                        b1 = ""
                                    End If
                                    Select Case b(0)
                                        Case "field" : sCondField = b1
                                        Case "cond" : sCond = b1
                                        Case "value" : sCondValue2 = b1
                                    End Select
                                Next
                                Dim sFullFieldName As String = nPage.ToString("0000") + "_" + sCondField
                                sCondValue = ValByKey(bp, sFullFieldName)
                                Select Case sCond
                                    Case "EQ" : bShow = sCondValue = sCondValue2
                                    Case "GT" : bShow = sCondValue > sCondValue2
                                    Case "LT" : bShow = sCondValue < sCondValue2
                                    Case "NE" : bShow = sCondValue <> sCondValue2
                                    Case "GE" : bShow = sCondValue >= sCondValue2
                                    Case "LE" : bShow = sCondValue <= sCondValue2
                                    Case "IN" : bShow = InStr(sCondValue2, sCondValue) > 0
                                    Case "NOTIN" : bShow = InStr(sCondValue2, sCondValue) = 0
                                    Case "INLIST", "NOTINLIST" 'vk 02.08
                                        bShow = False
                                        For Each a In sCondValue2.Split(",")
                                            If a = sCondValue Then bShow = True
                                        Next
                                        If sCond = "NOTINLIST" Then bShow = Not bShow
                                    Case "EXIST", "NOTEXIST" 'vk 08.08
                                        Try
                                            bShow = bp.cPrintServerCount(sFullFieldName) > 0
                                        Catch
                                            bShow = False
                                        End Try
                                        If sCond = "NOTEXIST" Then bShow = Not bShow
                                    Case Else : bShow = False
                                End Select
                                If bShow Then
                                    s = Left(s, i1 - 1) + sBody + Mid(s, i2 + 15)
                                Else
                                    s = Left(s, i1 - 1) + Mid(s, i2 + 15)
                                End If
                            End If
                        Loop
                    End If
                    nNum = 0
                    Do
                        i1 = InStr(s, "{@")
                        If i1 <= 0 Then Exit Do
                        i2 = InStr(i1, s, "@}")
                        If i2 > 0 Then
                            Dim sKey As String = Mid(s, i1 + 2, i2 - i1 - 2)
                            Dim sPrefix As String
                            Dim bAsis As Boolean = False 'vk 01.08
                            'vk 12.07
                            i = InStrRev(sKey, ")")
                            If i = 0 Then
                                sPrefix = ""
                            Else
                                sPrefix = Left(sKey, i)
                                sKey = Mid(sKey, i + 1)
                            End If
                            'vk 01.08 from here
                            If sPrefix Like "*(C)" Then
                                sValue = sKey
                            ElseIf sKey = "XXXI" Then
                                nNum += 1
                                sValue = nNum.ToString
                            ElseIf sKey = "XXXP" Then
                                If bBrowser Then
                                    sValue = "."
                                Else
                                    sValue = bp.m_sMapPath
                                End If
                            ElseIf sKey = "XXX\" OrElse sKey = "XXX/" Then
                                If bBrowser Then
                                    sValue = "/"
                                Else
                                    sValue = "\"
                                End If
                                'vk 01.08 till here
                            Else
                                sValue = ValByKey(bp, nPage.ToString("0000") + "_" + sKey) 'vk 08.07
                            End If
                            If oNodeScreen.ParamYes("debug") AndAlso sValue = "" Then
                                sValue = "{{" + sPrefix + sKey + "}}" 'vk 10.07
                                bAsis = True
                            Else
                                'vk 12.07 from here - support for functions on fields
                                Do Until sPrefix = "" OrElse sPrefix = "(W)"
                                    i = InStrRev(sPrefix, "(")
                                    Dim sFormula, sFunc, sParam1, sParam2 As String
                                    sFormula = Mid(sPrefix, i)
                                    sFormula = Mid(sFormula, 2, Len(sFormula) - 2)
                                    sPrefix = Left(sPrefix, i - 1)
                                    sFunc = Left(sFormula, 1)
                                    i = InStr(sFormula, ",")
                                    If i = 0 Then
                                        sParam1 = Mid(sFormula, 2)
                                        sParam2 = ""
                                    Else
                                        sParam1 = Mid(sFormula, 2, i - 2)
                                        sParam2 = Mid(sFormula, i + 1)
                                    End If
                                    Select Case sFunc
                                        Case "L" : sValue = Left(sValue, Val(sParam1))
                                        Case "R" : sValue = Right(sValue, Val(sParam1))
                                        Case "M" : sValue = Mid(sValue, Val(sParam1))
                                        Case "N" : sValue = Mid(sValue, Val(sParam1), Val(sParam2))
                                        Case "P" : sValue = sValue.PadLeft(Val(sParam1), sParam2)
                                        Case "Q" : sValue = sValue.PadRight(Val(sParam1), sParam2)
                                        Case "K"
                                            'If sParam1 = "2" Then
                                            '    sKeyForWnet2 += sValue
                                            'Else
                                            '    sKeyForWnet += sValue
                                            'End If
                                        Case "C" : bAsis = True 'do nothing, see above
                                        Case "W" 'do nothing, see below
                                        Case "V" : sValue = Val(sValue).ToString 'vk 01.08
                                        Case "D"
                                            If sValue = "" Then
                                                sValue = Mid(sFormula, 2)
                                                bAsis = True
                                            End If
                                    End Select
                                Loop
                                'vk 12.07 till here
                            End If
                            'vk 01.08
                            If Not bAsis Then
                                If Left(sValue, 3) = sImgIndic1 OrElse Left(sValue, 3) = sImgIndic2 Then
                                    'vk 04.11
                                    Dim sFileName As String = Trim(Mid(sValue, 4))
                                    Dim sStyle As String = ScaleStyle(bp, sFileName)
                                    sValue = "<img src=" + GoodFileName(bp, sFileName, oNodeScreen) + " style=" + sStyle + ">"
                                ElseIf sValue Like "*<br>*" Then
                                    Dim sValue0 As String = ""
                                    Do
                                        i = InStr(sValue, "<br>")
                                        If i = 0 Then
                                            sValue0 += qConv.ConvAlf(bp, sValue, "", "AS2PC",
                                                IIf(oNodeScreen.ParamVal("flr") = 1, "ltr", "rtl"),
                                                False, False, False, True, oNodeScreen.Param("flr"))
                                            Exit Do
                                        Else
                                            sValue0 += qConv.ConvAlf(bp, Left(sValue, i - 1), "", "AS2PC",
                                                IIf(oNodeScreen.ParamVal("flr") = 1, "ltr", "rtl"),
                                                False, False, False, True, oNodeScreen.Param("flr")) + "<br>"
                                            sValue = Mid(sValue, i + 4)
                                        End If
                                    Loop
                                    sValue = sValue0
                                Else
                                    sValue = qConv.ConvAlf(bp, sValue, "", "AS2PC",
                                        IIf(oNodeScreen.ParamVal("flr") = 1, "ltr", "rtl"),
                                        False, False, False, True, oNodeScreen.Param("flr"))
                                End If
                            End If
                            'If sPrefix = "(W)" Then
                            '    Dim o As XmlDesc.EncoderDecoder
                            '    Try
                            '        qConv.MakeNotOk(sValue)
                            '        o = New XmlDesc.EncoderDecoder(bp.getProperty("DescrFileForWnet"))
                            '        sValue = o.UpdateDescriptionFile(sValue)
                            '    Catch
                            '        sValue = ""
                            '    Finally
                            '        o = Nothing
                            '    End Try
                            'End If
                            s = Left(s, i1 - 1) + sValue + Mid(s, i2 + 2)
                        End If
                    Loop
                    If bPdf Then
                        'vk 01.10
                        Dim oStyle As Style, oControl As Control
                        oStyle = New Style()
                        oStyle.Add("width", nFullWidth)
                        oStyle.Add("direction", "ltr")
                        oStyle.Add("table-layout", "fixed")
                        oControl = New Control(oStyle.FullCode)
                        'oControl.Add("align", "left") 'rem vk 08.10
                        oControl.Add("cellspacing", "0")
                        oControl.Add("cellpadding", "0")
                        sOut.Append(oControl.FullCode("table"))
                        If Not oControl Is Nothing Then oControl.Dispose()
                        oControl = Nothing
                        oStyle = Nothing
                        sOut.Append("<tr><td id='435'>" + vbCrLf)
                        sOut.Append(s + vbCrLf)
                        sOut.Append("</td></tr></table>" + vbCrLf)
                        If Not bLastPage Then 'vk 05.15
                            sOut.Append(bp.getProperty("BetweenTables") + vbCrLf)
                        End If
                    ElseIf bNetto Then
                        sOut.Append(s + vbCrLf) 'vk 12.07
                    Else
                        sOut.Append("<div class='OutTable' style='top:" + nCurTop.ToString + "px;'>" + vbCrLf)
                        sOut.Append(s + vbCrLf)
                        sOut.Append("</div>" + vbCrLf)
                        nCurTop = nCurTop_Page + bp.getPropertyVal("PreprintHeight") 'vk 12.08, 08.10
                    End If
                    'clever form - till here
                End If
            End If
            'End If
            If bExit Then
                Exit Sub 'preprint done, no usual spool
            End If
        End If

        'vk 03.11
        Dim sHeader As String = bp.getPropertyPset("Header", oNodeScreen)
        Dim sFooter As String = bp.getPropertyPset("Footer", oNodeScreen)
        Dim sBackground As String = bp.getPropertyPset("Background", oNodeScreen)
        Dim sBackgroundPos As String = bp.getPropertyPset("BackgroundPos", oNodeScreen)
        Dim sShiftOnBkg As String = bp.getPropertyPset("ShiftOnBkg", oNodeScreen)
        Dim bHeaderOrFooterOrBack As Boolean = sHeader > "" OrElse sFooter > "" OrElse sBackground > ""

        If bPdf Then
            Dim oStyle As Style, oControl As Control
            If bHeaderOrFooterOrBack Then 'vk 04.11
                oStyle = New Style()
                Dim sTableCss = bp.getPropertyPset("TableCss", oNodeScreen)
                If sTableCss > "" Then
                    'vk 04.20
                    Dim v, vv() As String
                    For Each v In Split(sTableCss, ";")
                        vv = Split(v, ":")
                        If UBound(vv) > 0 Then
                            oStyle.Add(vv(0), vv(1))
                        End If
                    Next
                End If
                oStyle.Add("table-layout", "fixed")
                If sBackground > "" Then
                    oStyle.Add("background-image", "url(pics/" + sBackground + ")")
                    oStyle.Add("background-repeat", "no-repeat")
                    oStyle.Add("background-position", sBackgroundPos)
                End If
                oControl = New Control(oStyle.FullCode)
                oControl.Add("cellspacing", "0")
                oControl.Add("cellpadding", "0")
                oControl.Add("border", "0")
                oControl.Add("width", "100%")
                sOut.Append(oControl.FullCode("table"))
                If Not oControl Is Nothing Then oControl.Dispose()
                oControl = Nothing
                oStyle = Nothing
                If sHeader > "" Then 'vk 04.20
                    sOut.Append("<tr style='height:" + bp.getPropertyPset("HeaderHeight", oNodeScreen) + "'><td id='497'>" + vbCrLf)
                    sOut.Append("<img src='pics/" + sHeader + "' style='" + bp.getPropertyPset("HeaderStyle", oNodeScreen) + "'></td></tr>" + vbCrLf)
                End If
                sOut.Append("<tr style='height:" + bp.getPropertyPset("MiddleHeight", oNodeScreen) + "'><td id='500' valign=top>" + vbCrLf)
            End If
            'vk 03.09
            oStyle = New Style()
            oStyle.Add("width", nFullWidth)
            oStyle.Add("direction", "ltr")
            oStyle.Add("table-layout", "fixed")
            oStyle.Add("margin-left", sShiftOnBkg)
            'oStyle.Add("empty-cells", "show") 'vk 02.11
            oControl = New Control(oStyle.FullCode)
            'oControl.Add("align", "left") 'rem vk 08.10
            oControl.Add("cellspacing", "0")
            oControl.Add("cellpadding", "0")
            sOut.Append(oControl.FullCode("table"))
            If Not oControl Is Nothing Then oControl.Dispose()
            oControl = Nothing
            oStyle = Nothing
        End If

        'Dim bLineTop, bLineBottom, bLineLeft, bLineRight As Boolean 'vk 04.09
        Dim nGraphicsLines As Integer = 0 'vk 04.09
        sBorder1 = bp.getPropertyPset("Border_1", oNodeScreen)
        For i = 1 To nLines
            Dim sLastSymbol As String = "" 'vk 08.07
            Try
                s = bp.cPrintServer(nPage.ToString("0000") + i.ToString("00"))
                If oNodeScreen.Plain(bp) Then
                    sLastSymbol = " "
                Else
                    sLastSymbol = Right(s, 1)
                End If
                s = Mid(s, 7, Len(s) - 7)
            Catch e As Exception
                s = ""
            End Try
            If s > "" Then
                Dim sMask As String = "".PadRight(s.Length)
                Dim j As Integer
                Dim oStyle As Style, oControl As Control

                'vk 08.07
                'Dim nFullWidth As Integer = Len(s) * bp.iScaleWidth + 1
                Select Case sLastSymbol
                    Case "1", "4" 'vk 06.10
                        oTabPositions = New Collection()
                        For j = 1 To s.Length
                            Select Case AscW(Mid(s, j, 1))
                                Case bp.m_nCode_Bold, bp.m_nCode_Size2, bp.m_nCode_Size3, bp.m_nCode_Half, bp.m_nCode_Usual,
                                        bp.m_nCode_Bold_Underline, bp.m_nCode_Size2_Underline, bp.m_nCode_Underline, 32 'vk 04.11
                                Case Else
                                    oTabPositions.Add(j)
                            End Select
                        Next
                        If bPdf Then
                            'vk 04.09
                            bBorderTop_Outer = sLastSymbol = "4"
                            bBorderTop_Inner = sLastSymbol = "1"
                        Else
                            oLinePixels = New Collection()
                            oLinePixels.Add(nCurTop)
                        End If
                        If sLastSymbol = "4" Then
                            sBorder4 = bp.getPropertyPset("Border_4", oNodeScreen)
                        Else
                            sBorder4 = bp.getPropertyPset("Border_1", oNodeScreen)
                        End If
                    Case "2"
                        If bPdf Then
                            'vk 04.09
                            'bBorderTop = True
                        Else
                            oLinePixels.Add(nCurTop)
                            Dim iTab, iLine, jCur, jNext As Integer
                            If oTabPositions.Count > 1 AndAlso oLinePixels.Count > 1 Then
                                For iTab = 1 To oTabPositions.Count - 1
                                    For iLine = 1 To oLinePixels.Count - 1
                                        oStyle = New Style()
                                        oStyle.Add("position", "absolute")
                                        jCur = qStyle.icol_fun_netto(bp, Convert.ToInt32(oTabPositions(iTab)), oNodeScreen, False)
                                        jNext = qStyle.icol_fun_netto(bp, Convert.ToInt32(oTabPositions(iTab + 1)), oNodeScreen, False)
                                        oStyle.Add("left", jCur + bp.iScaleWidth / 2 - 1)
                                        oStyle.Add("width", jNext - jCur + 2)
                                        oStyle.Add("top", Convert.ToInt32(oLinePixels(iLine)) - 1)
                                        oStyle.Add("height", Convert.ToInt32(oLinePixels(iLine + 1) - oLinePixels(iLine)) + 2)
                                        oStyle.Add("background-color", "transparent")
                                        'vk 06.10
                                        If iTab = 1 Then
                                            oStyle.Add("border-left", sBorder4)
                                        Else
                                            oStyle.Add("border-left", sBorder1)
                                        End If
                                        If iTab = oTabPositions.Count - 1 Then
                                            oStyle.Add("border-right", sBorder4)
                                        End If
                                        If iLine = 1 Then
                                            oStyle.Add("border-top", sBorder4)
                                        Else
                                            oStyle.Add("border-top", sBorder1)
                                        End If
                                        If iLine = oLinePixels.Count - 1 Then
                                            oStyle.Add("border-bottom", sBorder4)
                                        End If
                                        oStyle.Add("font-size", "1") 'vk 04.09
                                        oControl = New Control(oStyle.FullCode)
                                        sOut.Append(oControl.FullCodeSpan)
                                    Next
                                Next
                            End If
                        End If
                        Do Until oTabPositions.Count = 0
                            oTabPositions.Remove(1)
                        Loop
                        Do Until oLinePixels.Count = 0
                            oLinePixels.Remove(1)
                        Loop
                    Case "3"
                        If bPdf Then
                            'vk 04.09
                            bBorderTop_Inner = True
                            bBorderTop_Outer = False
                        Else
                            If oLinePixels.Count = 0 Then
                                oStyle = New Style()
                                oStyle.Add("position", "absolute")
                                oStyle.Add("left", bp.iScaleWidth / 2)
                                oStyle.Add("top", nCurTop)
                                oStyle.Add("width", nFullWidth)
                                oStyle.Add("height", 0)
                                oStyle.Add("line-height", 0)
                                oStyle.Add("border-top", sBorder1)
                                oControl = New Control(oStyle.FullCode)
                                sOut.Append(oControl.FullCodeSpan)
                            Else
                                oLinePixels.Add(nCurTop)
                            End If
                        End If
                    Case Else
                        'vk 04.08
                        If bPdf AndAlso i < nLines AndAlso Not oNodeScreen.Plain(bp) Then
                            Dim ii As Integer
                            bBorderBottom = False
                            For ii = i + 1 To nLines
                                Try
                                    bBorderBottom = (Right(bp.cPrintServer(nPage.ToString("0000") + ii.ToString("00")), 1) = "2")
                                    Exit For
                                Catch
                                End Try
                            Next
                        End If

                        For Each j In oTabPositions
                            Mid(s, j, 1) = ChrW(bp.m_nCode_Usual) 'vk 11.11
                        Next

                        For j = 1 To s.Length
                            Select Case AscW(Mid(s, j, 1))
                                Case bp.m_nCode_Bold, bp.m_nCode_Size2, bp.m_nCode_Size3, bp.m_nCode_Half, bp.m_nCode_Usual,
                                        bp.m_nCode_Bold_Underline, bp.m_nCode_Size2_Underline, bp.m_nCode_Underline 'vk 04.11
                                    If oNodeScreen.Plain(bp) Then
                                        Mid(s, j, 1) = " " 'vk 01.06
                                    Else
                                        Mid(sMask, j, 1) = "+"
                                    End If
                            End Select
                        Next

                        'vk 04.09
                        Const NOFILE = "   "
                        Dim sFileName As String = NOFILE
                        Dim nPosOfGrafics As Integer
                        If Not (bPdf AndAlso nGraphicsLines > 0) Then
                            'vk 03.06
                            nPosOfGrafics = InStr(s, "@@001") 'logo
                            If nPosOfGrafics > 0 Then
                                sFileName = "" 'vk 04.09
                                Mid(s, nPosOfGrafics, 5) = Space(5)
                                If bPdf Then
                                Else
                                    oStyle = New Style()
                                    oStyle.Add("position", "absolute")
                                    oStyle.Add("top", nCurTop)
                                    oStyle.Add("z-index", "2")
                                    oControl = New Control(oStyle.FullCode)
                                    oControl.Add("class", "OutTable")
                                    sOut.Append(oControl.FullCode("div"))
                                    oStyle = New Style()
                                    oStyle.Add("position", "absolute")
                                    oStyle.Add("left", (nPosOfGrafics - 1) * bp.iScaleWidth)
                                    sOut.Append(qMulti.Logo(bp, oStyle, bBkg, True))
                                    sOut.Append("</div>" + vbCrLf)
                                    If Not oControl Is Nothing Then oControl.Dispose()
                                    oControl = Nothing
                                    oStyle = Nothing
                                End If
                            End If
                            'vk 08.07
                            Do
                                'vk 04.09
                                Select Case True 'vk 03.11
                                    Case sImgIndic2 = ""
                                        If InStr(s, sImgIndic1) = 0 Then Exit Do
                                        nPosOfGrafics = InStr(s, sImgIndic1)
                                    Case InStr(s, sImgIndic1) = 0 AndAlso InStr(s, sImgIndic2) = 0 : Exit Do
                                    Case InStr(s, sImgIndic1) > 0 : nPosOfGrafics = InStr(s, sImgIndic1)
                                    Case InStr(s, sImgIndic2) > 0 : nPosOfGrafics = InStr(s, sImgIndic2)
                                End Select
                                Dim nPosOfSpace As Integer = InStr(nPosOfGrafics, s, " ")
                                If nPosOfSpace < 1 OrElse nPosOfSpace > nPosOfGrafics + 10 Then
                                    nPosOfSpace = nPosOfGrafics + 10
                                End If
                                If nPosOfSpace > s.Length + 1 Then
                                    nPosOfSpace = s.Length + 1
                                End If
                                If Not (bPdf AndAlso sFileName <> NOFILE) Then
                                    sFileName = Mid(s, nPosOfGrafics + 3, nPosOfSpace - nPosOfGrafics - 3)
                                End If
                                Mid(s, nPosOfGrafics, nPosOfSpace - nPosOfGrafics) = Space(nPosOfSpace - nPosOfGrafics)
                                If bPdf Then
                                Else
                                    oStyle = New Style()
                                    oStyle.Add("position", "absolute")
                                    oStyle.Add("top", nCurTop)
                                    oStyle.Add("z-index", "2")
                                    oControl = New Control(oStyle.FullCode)
                                    oControl.Add("class", "OutTable")
                                    sOut.Append(oControl.FullCode("div"))
                                    oStyle = New Style()
                                    oStyle.Add("position", "absolute")
                                    oStyle.Add("left", (nPosOfGrafics - 1) * bp.iScaleWidth)
                                    Dim oControlT As New Control(oStyle.FullCode + ScaleStyle(bp, sFileName)) 'vk 03.11
                                    oControlT.Add("src", GoodFileName(bp, sFileName, oNodeScreen))
                                    oControlT.Add("disabled", "true")
                                    sOut.Append(oControlT.FullCode("img"))
                                    sOut.Append("</div>" + vbCrLf)
                                    If Not oControl Is Nothing Then oControl.Dispose()
                                    oControl = Nothing
                                    oStyle = Nothing
                                End If
                            Loop
                        End If

                        'vk 08.07
                        Dim j1 As Integer, sClass As String
                        Dim bDifferentFonts As Boolean = False 'vk 12.09
                        If sMask.Trim = "" Then
                            nMaxHeight = bp.HeightByClass("Usual", bPdf, oNodeScreen)
                            sMaxClass = "Usual"
                        Else
                            nMaxHeight = 0
                            For j1 = 1 To sMask.Length
                                If Mid(sMask, j1, 1) = "+" Then
                                    sClass = ClassByChar(bp, Mid(s, j1, 1))
                                    Dim nTmp As Integer = bp.HeightByClass(sClass, bPdf, oNodeScreen)
                                    If nTmp > nMaxHeight Then
                                        nMaxHeight = nTmp
                                        sMaxClass = sClass
                                    End If
                                    Select Case sClass 'vk 12.09
                                        Case "Size2", "Size3", "Half", "Size2_Underline"
                                            bDifferentFonts = True
                                    End Select
                                End If
                            Next
                        End If

                        'vk 04.09
                        If bPdf Then

                            Dim bStamp As Boolean = False
                            Dim j0 As Integer
                            Dim nGraphicsStart, nGraphicsEnd As Integer, sGraphics As String
                            Dim stp As String = oNodeScreen.Param("stp") 'vk 11.09
                            If nPosOfGrafics = 0 AndAlso stp <> " " AndAlso
                                    bp.getPropertyStamp("Count", stp) > "" Then 'vk 11.09
                                For j0 = 1 To bp.getPropertyStampVal("Count", stp)
                                    Dim sKey As String = oNodeScreen.GraphicsKey + "stamp" + j0.ToString + "_" + stp 'vk 05.20
                                    If bp.m_colColor.Item("graphics_" + sKey) Is Nothing Then
                                        sKey = oNodeScreen.GraphicsKey + "stamp" + j0.ToString
                                    End If
                                    If bp.m_colColor.Item("graphics_" + sKey) Is Nothing Then
                                        sKey = oNodeScreen.Param("cli") + "stamp" + j0.ToString + "_" + stp
                                    End If
                                    If bp.m_colColor.Item("graphics_" + sKey) Is Nothing Then
                                        sKey = oNodeScreen.Param("cli") + "stamp" + j0.ToString 'vk 11.09
                                    End If
                                    Try
                                        bStamp = (bp.m_colColor.Item("graphicsT_" + sKey) = i.ToString _
                                            AndAlso bp.m_colColor.Item("graphicsPage_" + sKey) = nPage.ToString)
                                    Catch
                                        'bStamp = False
                                    End Try
                                    If bStamp Then
                                        nPosOfGrafics = bp.m_colColor.Item("graphicsL_" + sKey)
                                        sFileName = "stamp" + j0.ToString
                                        sGraphics = qMulti.FromFile(bp.m_sMapPath + "\Color\" + bp.m_colColor.Item("graphics_" + sKey)) + vbCrLf
                                        Exit For
                                    End If
                                Next
                            End If
                            If bStamp Then
                                For j0 = 1 To 9
                                    sGraphics = sGraphics.Replace("{@" + j0.ToString + "@}", oNodeScreen.Param("sp" + j0.ToString))
                                Next
                            End If

                            If sFileName <> NOFILE Then
                                nGraphicsStart = nPosOfGrafics
                                Dim sScaleStyle As String = ScaleStyle(bp, sFileName) 'vk 03.11
                                Try
                                    Dim sKey As String = oNodeScreen.Param("cli") + sFileName
                                    If stp.Trim > "" Then 'vk 04.11
                                        sKey += "_" + stp
                                    End If
                                    If bp.m_colColor.Item("graphics_" + sKey) = "" Then
                                        sKey = oNodeScreen.Param("cli") + sFileName 'vk 11.09
                                    End If
                                    nGraphicsLines = bp.m_colColor.Item("graphicsH_" + sKey)
                                    nGraphicsEnd = nPosOfGrafics - 1 + bp.m_colColor.Item("graphicsW_" + sKey)
                                    If Not bStamp Then sGraphics = "<img style='" + sScaleStyle + "' src='pics/" + bp.m_colColor.Item("graphics_" + sKey) + "'>"
                                Catch
                                    nGraphicsLines = 5
                                    nGraphicsEnd = nPosOfGrafics + 9
                                    If Not bStamp Then sGraphics = "<img style='" + sScaleStyle + "' src='pics/" + sFileName + ".jpg'>"
                                End Try
                            ElseIf nGraphicsLines > 0 Then
                                nGraphicsLines -= 1
                            End If
                            Dim sTableStyle As String = "table-layout:fixed;"
                            Select Case sLastSymbol
                                Case "*", ChrW(bp.m_nCode_Gray)
                                    sTableStyle += "background-color:silver;"
                            End Select
                            sOut.Append("<tr>" + vbCrLf)
                            If nGraphicsLines = 0 Then
                                sOut.Append(SpoolOneRow(bp, sLastSymbol,
                                        nFullWidth, sTableStyle,
                                        s, sMask,
                                        oNodeScreen, bPdf, bDifferentFonts, bFixedLineHeight))
                            Else
                                If nGraphicsStart > 1 Then
                                    sOut.Append(SpoolOneRow(bp, sLastSymbol,
                                        (nGraphicsStart - 1) * bp.iScaleWidth, sTableStyle,
                                        Left(s, nGraphicsStart - 1), Left(sMask, nGraphicsStart - 1),
                                        oNodeScreen, bPdf, bDifferentFonts, bFixedLineHeight))
                                    'Else
                                    'sOut.Append("<td style='width:0;'></td>" + vbCrLf)
                                End If
                                If sFileName <> NOFILE Then
                                    Dim nColspan As Integer = nGraphicsEnd - nGraphicsStart + 1
                                    sOut.Append("<td colspan=" + nColspan.ToString _
                                        + " rowspan=" + nGraphicsLines.ToString + ">" + sGraphics + "</td>" + vbCrLf)
                                End If
                                If nGraphicsEnd < s.Length Then
                                    sOut.Append(SpoolOneRow(bp, sLastSymbol,
                                        (s.Length - nGraphicsEnd) * bp.iScaleWidth, sTableStyle,
                                        Right(s, s.Length - nGraphicsEnd), Right(sMask, s.Length - nGraphicsEnd),
                                        oNodeScreen, bPdf, bDifferentFonts, bFixedLineHeight))
                                    'Else
                                    'sOut.Append("<td style='width:0;'></td>" + vbCrLf)
                                End If
                            End If
                            sOut.Append("</tr>" + vbCrLf + vbCrLf)
                            bBorderBottom = False
                            bBorderTop_Inner = False
                            bBorderTop_Outer = False
                        Else
                            oStyle = New Style()
                            oStyle.Add("top", nCurTop)
                            oStyle.Add("width", nFullWidth)
                            oStyle.Add("height", nMaxHeight)
                            Select Case sLastSymbol
                                Case "*", ChrW(bp.m_nCode_Gray)
                                    oStyle.Add("background-color", "silver")
                            End Select
                            oControl = New Control(oStyle.FullCode)
                            oControl.Add("class", "OutTable")
                            sOut.Append(oControl.FullCode("div"))
                            If Not oControl Is Nothing Then oControl.Dispose()
                            oControl = Nothing
                            oStyle = Nothing
                            sOut.Append(SpoolOneRow(bp, sLastSymbol,
                                nFullWidth, "",
                                s, sMask,
                                oNodeScreen, bPdf, True, bFixedLineHeight))
                            sOut.Append("</div>" + vbCrLf)
                        End If
                        If bFixedLineHeight Then
                            nMaxHeight = bp.HeightByClass("Usual", bPdf, oNodeScreen) 'vk 08.10
                        End If
                        nCurTop += nMaxHeight - 2 + nInterline 'vk 02.06

                End Select
            End If
        Next

        If bPdf Then
            If bBorderTop_Inner Then
                sOut.Append("<tr><td colspan=" + s.Length.ToString +
                    " style='border-top:" + sBorder1 + ";height:" + sBorder1.Split(" ")(0) + ";'>&nbsp;</td><tr>" + vbCrLf)
                bBorderTop_Inner = False
            End If
            If bBorderTop_Outer Then
                sOut.Append("<tr><td colspan=" + s.Length.ToString +
                    " style='border-top:" + sBorder4 + ";height:" + sBorder4.Split(" ")(0) + ";'>&nbsp;</td><tr>" + vbCrLf)
                bBorderTop_Outer = False
            End If
            sOut.Append("</table>" + vbCrLf)
            If bHeaderOrFooterOrBack Then 'vk 03.11
                sOut.Append("</td></tr>" + vbCrLf)
                If sFooter > "" Then 'vk 04.20
                    sOut.Append("<tr style='height:" + bp.getPropertyPset("FooterHeight", oNodeScreen) + "'><td id='910'>" + vbCrLf)
                    sOut.Append("<img src='pics/" + sFooter + "' style='" + bp.getPropertyPset("FooterStyle", oNodeScreen) + "'></td></tr>" + vbCrLf)
                End If
                sOut.Append("</table>" + vbCrLf)
            End If
            If Not bLastPage Then 'vk 05.15
                sOut.Append(bp.getProperty("BetweenTables") + vbCrLf)
            End If
        End If

    End Sub
    Private Function SpoolOneRow(ByRef bp As BuildPage, ByVal sLastSymbol As String,
            ByVal nFullWidth As Integer, ByVal sTableStyle As String,
            ByVal s As String, ByVal sMask As String,
            ByRef oNodeScreen As Node, ByVal bPdf As Boolean,
            ByVal bDifferentFonts As Boolean, ByVal bFixedLineHeight As Boolean) As String 'vk 04.09
        Dim sResult As String = ""
        'Dim oStyle As Style, oControl As Control
        Dim j1, j2 As Integer
        Dim nAddToWidth As Single 'vk 04.09
        Dim sClass, sChar As String
        If bPdf Then
            'sResult += "<!-- debug: len(s)=" + Len(s).ToString + " -->" + vbCrLf
            sResult += "<td colspan=" + s.Length.ToString + " style='width:" _
                + (s.Length * bp.iScaleWidth + 1).ToString _
                + "px;'><table cellspacing=0 cellpadding=0 style='" + sTableStyle + "'><tr>" + vbCrLf
        End If
        If sMask.Trim = "" Then
            sResult += SpoolField(bp, s, "Usual", 0, 1, sMask.Length, oNodeScreen, bPdf, True, True, bDifferentFonts, bFixedLineHeight) + vbCrLf
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
                If bPdf Then
                    'vk 04.09
                    If j1 = 2 Then
                        nAddToWidth = 1
                    End If
                    If j2 - j1 + 1 > 0 Then
                        sResult += SpoolField(bp, Mid(s, j1, j2 - j1 + 1), sClass, nAddToWidth, j1, sMask.Length, oNodeScreen, bPdf, j1 <= 2, j2 = sMask.Length, bDifferentFonts, bFixedLineHeight) + vbCrLf
                    End If
                Else
                    If j1 > 1 Then
                        nAddToWidth = 1
                    End If
                    sResult += SpoolField(bp, Mid(s, j1, j2 - j1 + 1), sClass, nAddToWidth, j1, sMask.Length, oNodeScreen, bPdf, j1 = 1, j2 = sMask.Length, bDifferentFonts, bFixedLineHeight) + vbCrLf
                End If
                j1 = j2 + 2
            Loop Until j1 > sMask.Length
        End If
        If bPdf Then
            sResult += "</tr></table></td>" + vbCrLf
        End If
        Return sResult
    End Function
    Private Function SpoolField(ByRef bp As BuildPage, ByRef s As String, ByVal sClass As String,
            ByVal nAddToWidth As Integer, ByVal nPos As Integer, ByVal nLineLen As Integer,
            ByRef oNodeScreen As Node, ByVal bPdf As Boolean,
            ByVal bEdgeL As Boolean, ByVal bEdgeR As Boolean,
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
        If bp.getProperty("IgnoreNumbers") = "true" Then
            bText = True 'vk 07.21
        Else
            For i = 1 To Len(s)
                If sNumeric.IndexOf(Mid(s, i, 1)) < 0 Then
                    bText = True
                    Exit For
                End If
            Next
        End If
        If Not bText Then
            If oNodeScreen.ParamVal("flr") = 1 Then 'moved here by vk 12.09
                oStyle.Add("text-align", "right")
            End If
        End If
        If bPdf Then
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
                oStyle.Add("height", bp.HeightByClass("Usual", True, oNodeScreen) + nInterline) 'vk 08.10
            Else
                oStyle.Add("height", bp.HeightByClass(sClass, True, oNodeScreen) + nInterline) 'vk 06.09
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
        Else
            If oNodeScreen.ParamVal("flr") = 1 AndAlso bText Then
                oNode.AddIfNo("wd", nLineLen - nPos + nAddToWidth + 1)
                oNode.AddIfNo("col", nPos - nAddToWidth)
            Else
                oNode.AddIfNo("wd", Len(s) - 1 + nPos)
                oNode.AddIfNo("col", 1)
            End If
            oStyle.Add("width", qStyle.iwid_fun(bp, oNode, oNodeScreen) - 1)
            oStyle.Add("left", qStyle.icol_fun(bp, oNode, oNodeScreen))
            If sMaxClass = sClass OrElse bFixedLineHeight Then 'vk 08.10
                oStyle.Add("top", 0)
            Else
                oStyle.Add("top", nMaxHeight - bp.HeightByClass(sClass, bPdf, oNodeScreen)) 'vk 08.07
            End If
            If bPseudo254 Then
                'vk 08.08
                oStyle.Add("unicode-bidi", "bidi-override")
                oStyle.Add("direction", "ltr")
                'vk 01.09
                If oNodeScreen.ParamVal("flr") = 0 Then
                    oStyle.Add("text-align", "right")
                End If
            End If
        End If
        sStyle = oStyle.FullCode
        oStyle = Nothing

        oControl = New Control(sStyle)
        oControl.Add("class", "OutCom" + sClass)
        If s.Trim = "" Then
            If bPdf Then
                s = "&nbsp;" 'vk 04.09, 02.11
                's = " " 'vk 11.09
            Else
                s = "" 'vk 01.06
            End If
        Else
            'vk 11.10 till End Function
            If Not bText AndAlso Not oNodeScreen.Plain(bp) Then
                s = s.Trim
            ElseIf bPdf AndAlso (sClass = "Size3" OrElse sClass = "Size2" OrElse sClass = "Size2_Underline") Then
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
            If bPdf AndAlso bFixedLineHeight Then
                s = "<div style='position:relative;unicode-bidi:bidi-override;'>" + s + "</div>"
            End If
        End If
        If bp.getProperty("PdfProductVersion") = "1" Then
            'vk 05.15
            s = s.Replace("&nbsp;", " ")
        End If
        oControl.Add("value", s)
        sResult = oControl.FullCodeSpan(False, IIf(bPdf, "td", IIf(bPseudo254, "pre", "span")), False, True)
        If Not oControl Is Nothing Then oControl.Dispose()
        oControl = Nothing
        If Not oNode Is Nothing Then oNode.Dispose()
        oNode = Nothing
        Return sResult

    End Function

    Private Function ScaleStyle(ByRef bp As BuildPage, ByVal sFileName As String) As String
        Try
            Return bp.m_colColor.Item("scale_" + sFileName)
        Catch
            Return ""
        End Try
    End Function
    Private Function GoodFileName(ByRef bp As BuildPage, ByVal sFileName As String, ByRef oNodeScreen As Node) As String
        Dim sSource As String = bp.m_colColor.Item("graphics_" + oNodeScreen.GraphicsKey + sFileName) 'vk 05.20
        If sSource Is Nothing Then
            sSource = bp.m_colColor.Item("graphics_" + oNodeScreen.Param("cli") + sFileName)
        End If
        If sSource Is Nothing Then
            sSource = sFileName + ".jpg"
        End If
        Return "pics/" + sSource
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

    Function PreprintNo(ByVal s As String) As String 'vk 12.06, 12.08
        Try
            Dim i As Integer
            i = InStr(s, PREPRINT)
            If i > 6 Then
                Return Mid(s, i + Len(PREPRINT)).Trim
            Else
                Return "" 'vk 01.09
            End If
        Catch
            Return "" 'vk 01.09
        End Try
    End Function

    Function IsNetto(ByRef bp As BuildPage, ByRef oNodeScreen As Node) As Boolean 'vk 12.07
        Try
            Dim s As String
            If oNodeScreen Is Nothing Then Return False
            If bp.cPrintServer.Count = 0 Then Return False
            s = bp.cPrintServerPreprint("0001")
            If Val(s) = 0 Then Return False
            If Len(s) <> 3 Then Return False
            s = "preprinthtml_" + oNodeScreen.Param("cli") + s
            If bp.m_colColor.Item(s) Is Nothing Then Return False
            s = qMulti.FromFile(bp.m_sMapPath + "\Color\" + bp.m_colColor.Item(s).ToString)
            Return s.ToLower Like "<html*"
        Catch
            Return False
        End Try
    End Function

    Sub Dispose()
        qStyle = Nothing
        qMulti.Dispose()
        qMulti = Nothing
        qConv = Nothing
    End Sub

End Class
