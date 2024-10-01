Imports System.Data.SqlClient
Imports System.IO
Imports Comtec.TIS
Imports System.Xml.Linq
Imports System.Linq

Friend Class Combo

    Private qConv As New Conv()
    Private qStyle As New StyleModule()
    Private oInfo As New PocketKnife.Info()
    Private oConv As New ConvCom()
    'Private qBoo As New Boo()

    Function TextOfCombo_ByXml(ByRef bp As BuildPage, ByVal sXML As String,
            ByRef oNode As Node, ByRef oNodeScreen As Node) As String 'vk 03.03

        Dim sr As New StringReader(sXML)
        Dim rd As New Xml.XmlTextReader(sr)
        Dim nm As String = ""
        Dim val As String

        Try
            nm = "..."
            val = oNode.ValModified

            Do While rd.Read
                If rd.NodeType = Xml.XmlNodeType.Element _
                        AndAlso rd.Name = "b" _
                        AndAlso ItIs(rd.GetAttribute("op"), val, oNode.Param("typ")) Then
                    Return GetNm(rd, bp, oNode, oNodeScreen, False) 'vk 05.13
                End If
            Loop
            Return nm

        Catch e1 As Exception
            Dim sAddNote As String = "b," + nm
            Throw New Exception("XML from AS400 is wrong. " + sAddNote + vbCrLf + e1.Message, e1)
            Return ""
        End Try

    End Function

    Function HelpText(ByRef bp As BuildPage, ByVal oNode As Node,
            ByRef oNodeScreen As Node, ByVal rd As Xml.XmlTextReader,
            ByVal bSearch As Boolean) As String 'vk 02.04, 06.04, 02.14

        Dim hi As String, i As Integer
        Dim h As String = ""

        If bSearch Then
            Dim sValue As String
            Dim bFound As Boolean = False
            Do While rd.Read
                If rd.NodeType = Xml.XmlNodeType.Element Then
                    Select Case rd.Name
                        Case "f"
                            sValue = rd.GetAttribute("val").Trim
                        Case "b"
                            If rd.GetAttribute("op").Trim = sValue Then 'vk 11.08.24 ComboBoxWithANonNumericValue
                                bFound = True
                                Exit Do
                            End If
                    End Select
                End If
            Loop
            If Not bFound Then
                Return "."
            End If
        End If

        For i = 1 To 3
            hi = rd.GetAttribute("h" + i.ToString("0"))
            If hi > "" Then
                hi = hi.Replace("\", "\\")
                qConv.MakeOk(bp, oNodeScreen, hi)
                If i > 1 Then h += "\n"
                h += hi
            End If
        Next
        If h = "" Then Return "." Else Return h

    End Function

    Private Function CurExists(ByVal sXML As String, ByRef oNode As Node) As Boolean 'vk 01.07

        Dim sr As New StringReader(sXML)
        Dim rd As New Xml.XmlTextReader(sr)
        Do While rd.Read
            If rd.NodeType = Xml.XmlNodeType.Element AndAlso rd.Name = "b" Then
                If ItIs(rd.GetAttribute("op"), oNode.Param("val"), "N") Then Return True
            End If
        Loop
        Return False

    End Function

    Sub BuildOption_AsGrid(ByRef bp As BuildPage, ByVal sXML As String,
            ByRef oNode As Node, ByRef oNodeScreen As Node,
            ByRef sOptionT As String, ByRef sN As String, ByVal bReadOnly As Boolean,
            ByVal bWithRadio As Boolean, ByVal bBkg As Boolean)

        Dim iRow As Integer = CountLinesInCombo_ByXml(sXML, oNode, False)
        If iRow = 0 Then Exit Sub
        Dim bDiv As Boolean = iRow > oNode.ParamVal("pht")
        Dim nCurrentLineTop As Integer = 0 'vk 10.06

        Dim nSelectFrom, nSelectTill, nDeletedFrom, nDeletedTill, nValueFrom, nValueTill As Integer
        Dim nWidthPos, nLeft, nTop, i As Integer
        Dim sAlign, sDir As String
        If bDiv Then
            nWidthPos = oNode.ParamVal("wd") - IIf(oNodeScreen.ParamVal("fdsp") = 1, 3, 2) 'vk 02.10
            nLeft = 0
            nTop = 0
        Else
            nWidthPos = oNode.ParamVal("wd")
            nLeft = qStyle.icol_fun(bp, oNode, oNodeScreen)
            nTop = qStyle.ilin_fun(bp, oNode, oNodeScreen)
        End If

        Dim nWidth1 As Integer
        Dim nWidth2 As Integer
        If oNode.Param("cry") = "I" Then
            nWidth1 = bp.getPropertyVal("SignWidth")
            nWidth2 = bp.getPropertyVal("RadioWidth")
        Else
            nWidth1 = bp.getPropertyVal("DeletedWidth")
            nWidth2 = bp.getPropertyVal("SelectWidth")
        End If

        If oNodeScreen.ScrollSide(bp) = "left" Then
            If bReadOnly Then
                nDeletedFrom = 1
                nDeletedTill = nWidth1
            Else
                nSelectFrom = 1
                nSelectTill = nWidth2
                nDeletedFrom = nSelectTill + 1
                nDeletedTill = nSelectTill + nWidth1
            End If
            nValueFrom = nDeletedTill + 1
            nValueTill = nWidthPos
        Else
            If bReadOnly Then
                nDeletedTill = nWidthPos
                nValueTill = nWidthPos - nWidth1
            Else
                nSelectTill = nWidthPos
                nDeletedTill = nWidthPos - nWidth2
                nValueTill = nDeletedTill - nWidth1
                nSelectFrom = nDeletedTill + 1
            End If
            nDeletedFrom = nValueTill + 1
            nValueFrom = 1
        End If
        If oNode.ParamYes("prl") Then
            sAlign = "right"
            sDir = "rtl"
        Else
            sAlign = "left"
            sDir = "ltr"
        End If

        Dim sOut As New Text.StringBuilder()
        Dim sr As New StringReader(sXML)
        Dim rd As New Xml.XmlTextReader(sr)
        Dim bCurrent As Boolean, sControl As String
        Dim bCurExists As Boolean = CurExists(sXML, oNode) 'vk 01.07
        i = 0
        sOut.Append("<!-- table-responsive start -->" + vbCrLf)

        sOut.Append("<div class=""table-responsive"">" + vbCrLf)
        sOut.Append($"<table class=""table {If(bp.getProperty("SortTables") = "true", "sortable", "")} table-bordered table-hover"">" + vbCrLf)

        sOut.Append("<tbody>" + vbCrLf)

        Do While rd.Read
            If rd.NodeType = Xml.XmlNodeType.Element AndAlso rd.Name = "b" Then

                sOut.Append("<tr>" + vbCrLf)

                bCurrent = ItIs(rd.GetAttribute("op"), oNode.Param("val"), "N")

                If Not bReadOnly Then
                    If oNode.Param("cry") = "I" Then

                        ' Build radio button
                        sControl = OneGridCell(bp, oNode, oNodeScreen, nSelectFrom, nSelectTill, nLeft, nTop, i, iRow, bCurrent, IIf(bCurExists, bCurrent, i = 0),
                                   "ListClick('" + sN + "'," + Int64.Parse(rd.GetAttribute("op")).ToString + ")", "", "center", sDir, True, bDiv, True, bBkg, False)

                        bp.m_sCurClass = "form-control"

                        sOut.Append("<td id='193'>" + vbCrLf).Append(sControl).Append("</td>" + vbCrLf)


                        sControl = OneGridCell(bp, oNode, oNodeScreen, nSelectFrom, nSelectTill, nLeft, nTop,
                                    i, iRow, bCurrent, IIf(bCurExists, bCurrent, i = 0), "", "", "center", sDir, True, bDiv, False, bBkg, False)

                        sOut.Append("<td id='199'>" + vbCrLf).Append(sControl).Append("</td>" + vbCrLf)

                    Else
                        sControl = OneGridCell(bp, oNode, oNodeScreen, nSelectFrom, nSelectTill, nLeft, nTop, i, iRow, bCurrent, IIf(bCurExists, bCurrent, i = 0),
                                 "ListClick('" + sN + "'," + Int64.Parse(rd.GetAttribute("op")).ToString + ")", bp.getProperty("SelectText"), "center", sDir, True, bDiv, False, bBkg, False)

                        sOut.Append("<td id='205'>" + vbCrLf).Append(sControl).Append("</td>" + vbCrLf)
                    End If
                End If

                sControl = OneGridCell(bp, oNode, oNodeScreen, nValueFrom, nValueTill, nLeft, nTop, i, iRow, bCurrent, False,
                                       "", rd.GetAttribute("nm"), sAlign, sDir, False, bDiv, False, bBkg, rd.GetAttribute("heb"))

                sOut.Append("<td id='212'>" + vbCrLf).Append(sControl).Append("</td>" + vbCrLf)

                Dim sText As String
                If oNode.Param("cry") = "I" Then
                    sText = rd.GetAttribute("ex")
                ElseIf rd.GetAttribute("dl") = "0" Then
                    sText = ""
                Else
                    sText = bp.getProperty("DeletedText")
                End If

                If sText.Trim <> "" Then
                    sControl = OneGridCell(bp, oNode, oNodeScreen, nDeletedFrom, nDeletedTill, nLeft, nTop,
                   i, iRow,
                   bCurrent, False, "", sText,
                   "center", sDir, False, bDiv, False, bBkg, False)
                    sOut.Append("<td id='228'>" + vbCrLf).Append(sControl).Append("</td>" + vbCrLf)

                End If

                If bCurrent AndAlso bDiv Then
                    'vk 10.06
                    nCurrentLineTop = nTop + bp.iScaleLin * i + 2
                End If
                i += 1
                sOut.Append("</tr>" + vbCrLf)

            End If
        Loop

        sOut.Append("</tbody>" + vbCrLf)

        sOut.Append("</table>" + vbCrLf)
        'sOut.Append("<select id ='mySelect'></select>" + vbCrLf)
        sOut.Append("</div>" + vbCrLf)

        Dim oStyle As Style, oControl As Control
        oStyle = New Style()

        oStyle.Add("background-color", "transparent") 'vk 02.10
        oStyle.Add("border", bp.getProperty("GridFrame0")) 'vk 05.10
        If bDiv Then
            oStyle.Add("left", qStyle.icol_fun(bp, oNode, oNodeScreen))
            oStyle.Add("width", oNode.ParamVal("wd") * bp.iScaleWidth + 4) 'vk 10.06
            oStyle.Add("top", qStyle.ilin_fun(bp, oNode, oNodeScreen))
            oStyle.Add("height", oNode.ParamVal("pht") * bp.iScaleLin)
            oStyle.Add("padding", 5)
            oStyle.Add("overflow-y", "auto") 'vk 02.10
            oStyle.Add("visibility", "hidden") 'vk 10.06
            oControl = New Control(oStyle.FullCode)
            oControl.AddId("scroll_" + oNode.Param("num").Trim) 'vk 10.06, 01.10

            qStyle.SetDir(bp, oNodeScreen, oControl)
            sOptionT = oControl.FullCode(bp, "div") + sOut.ToString + "</div>" + vbCrLf
            bp.m_nDeltaForScrollFields += (oNode.ParamVal("pht") - 1) * bp.iScaleLin
            'vk 10.06
            nCurrentLineTop -= (oNode.ParamVal("pht") - 1) * bp.iScaleLin
            If nCurrentLineTop < 0 Then nCurrentLineTop = 0
            sOptionT += "<script>" + vbCrLf
            sOptionT += "document.getElementById('scroll_" + oNode.Param("num").Trim + "').scrollTop=" + nCurrentLineTop.ToString + ";" + vbCrLf
            sOptionT += "document.getElementById('scroll_" + oNode.Param("num").Trim + "').style.visibility='visible';" + vbCrLf
            sOptionT += "</script>" + vbCrLf
        Else
            sOptionT = sOut.ToString
            If iRow > 1 Then
                bp.m_nDeltaForScrollFields += (iRow - 1) * bp.iScaleLin
            End If
        End If
    End Sub

    Private Function OneGridCell(
            ByRef bp As BuildPage,
            ByRef oNode As Node,
            ByRef oNodeScreen As Node,
            ByVal nFrom As Integer,
            ByVal nTill As Integer,
            ByVal nLeft As Integer,
            ByVal nTop As Integer,
            ByVal nLine As Integer,
            ByVal nLines As Integer,
            ByVal bCurrent As Boolean,
            ByVal bCurrentForTab As Boolean,
            ByVal sOnClick As String,
            ByVal sText As String,
            ByVal sAlign As String,
            ByVal sDir As String,
            ByVal bSelect As Boolean,
            ByVal bDiv As Boolean,
            ByVal bRadio As Boolean,
            ByVal bBkg As Boolean,
            ByVal bHeb As Boolean) As String

        Dim oStyle As New Style()
        'vk 03.10 from here
        Dim iCol As Integer = nLeft + (nFrom - 1) * bp.iScaleWidth + CInt(IIf(bDiv, 1, 3))
        Dim iLin As Integer = nTop + bp.iScaleLin * nLine + CInt(IIf(bDiv, 1, 2))
        Dim iWidth As Integer = (nTill - nFrom + 1) * bp.iScaleWidth - 1
        Dim iHeight As Integer = bp.iScaleLin - 1
        If bRadio Then
            qStyle.SetCheckRadioSize(bp, "radio", iLin, iCol, iWidth, iHeight)
        Else
            oStyle.Add("text-align", sAlign)
        End If

        'vk 03.10 till here
        If sOnClick > "" Then
            oStyle.Add("text-decoration", "underline")
            oStyle.Add("cursor", "pointer")
        End If

        Dim oControl As New Control(oStyle.FullCode, True)

        If bRadio Then
            oControl.Add("type", "radio")
            If bCurrent Then
                oControl.Add("checked", "true")
            End If
        Else
            oControl.Add("value", sText)
        End If

        oControl.Add("dir", sDir)
        'oControl.Add("data-id", sValFor400)
        'oControl.Add("data-sn", sN)
        If sOnClick > "" Then
            oControl.Add("onclick", sOnClick)
        End If
        If bSelect Then 'vk 03.07
            If Not bBkg Then 'vk 09.11
                If bCurrentForTab Then
                    oControl.AddId("Gfocus") 'vk 01.07
                ElseIf nLine = 0 Then
                    oControl.AddId("Gfirst") 'vk 02.07
                ElseIf nLine = nLines - 1 Then
                    oControl.AddId("Glast") 'vk 02.07
                End If
            End If
        End If
        Return oControl.FullCode(bp)

    End Function

    Function CountLinesInCombo_ByXml(ByVal sXML As String, ByRef oNode As Node,
            ByVal bLookForDefault As Boolean) As Integer 'vk 05.04

        Dim sr As New StringReader(sXML)
        Dim rd As New Xml.XmlTextReader(sr)
        Dim op As String = ""
        Dim opDefault As String = ""
        Dim i As Integer = 0

        Try
            If bLookForDefault Then
                If oNode.ParamIn("typ", "NS") Then
                    opDefault = "0".PadLeft(oNode.ParamVal("len"), "0")
                Else
                    opDefault = "".PadLeft(oNode.ParamVal("len"), " ")
                End If
            End If
            Do While rd.Read
                If rd.NodeType = Xml.XmlNodeType.Element AndAlso rd.Name = "b" Then
                    If bLookForDefault Then
                        op = rd.GetAttribute("op").PadLeft(oNode.ParamVal("len"), "0")
                        op = Right(op, oNode.ParamVal("len"))
                        If Not ItIs(op, opDefault, oNode.Param("typ")) Then i += 1
                    Else
                        i += 1 'vk 09.06
                    End If
                End If
            Loop
            Return i
        Catch e1 As Exception
            Dim sAddNote As String = "b," + op
            Throw New Exception("XML from AS400 is wrong. " + sAddNote + vbCrLf + e1.Message, e1)
        End Try

    End Function

    Sub BuildOption_AsRadio(ByRef bp As BuildPage, ByVal sXML As String,
            ByRef oNode As Node, ByRef oNodeScreen As Node,
            ByRef sOptionT As String, ByRef sN As String,
            ByVal bReadOnly As Boolean, ByVal bBkg As Boolean)

        Dim sOut As New Text.StringBuilder()
        Dim sr As New StringReader(sXML)
        Dim Xdoc As XDocument = XDocument.Parse(sXML)
        Dim rd As New Xml.XmlTextReader(sr)
        Dim val, sper As String
        Dim op As String = ""
        Dim nm As String = ""
        Dim ImageFileId As Integer = 0
        Dim i As Integer = 0
        'Dim oControlT As Control, oStyleT As Style, oNodeT As Node
        Dim bFound As Boolean = False

        If oNode.ParamYes("per") Then
            sper = "1"
        Else
            sper = "0"
        End If

        Dim iLin As Integer = qStyle.ilin_fun(bp, oNode, oNodeScreen)
        val = oNode.ValModified

        Try
            Dim ImagesInGroup As Boolean = False

            If Xdoc.Descendants("b").Any(Function(x) x.Attribute("ImageFileId") IsNot Nothing AndAlso x.Attribute("ImageFileId").Value <> "0") AndAlso bp.singleFieldInLine Then
                ImagesInGroup = True
            End If

            If sXML.Contains("<b") Then
                'vk 09.24 pkv
                sOut.AppendLine($"<div class='form-control list-group-checkable {If(oNode.ParamVal("psz") > 0, "row", "")} GroupRadioButtonsNoImages{If(oNode.ParamVal("pkv") = 1, "V", "")} {If(bp.singleFieldInLine, "singleFieldInLine", "")}' style='position: relative;display: inline-flex !important;
    vertical-align: middle;'> ") 'ntg 22.06.23 - creating a new class for radio btn with no images
                'sOut.AppendLine($"<div class='form-control list-group-checkable {If(oNode.ParamVal("psz") > 0, "row", "")} {If(ImagesInGroup, "GroupImagesRadioButtons", "")} {If(bp.singleFieldInLine, "singleFieldInLine", "")}'> ")
            End If
            Do While rd.Read
                If rd.NodeType = Xml.XmlNodeType.Element AndAlso rd.Name = "b" Then
                    op = rd.GetAttribute("op")
                    'nm = GetNm(rd, bp, oNode, oNodeScreen, True) 'vk 05.13
                    nm = GetNm(rd, bp, oNode, oNodeScreen, False) 'ntg 19.06.23 - the quote mark didnt appear right

                    If bp.singleFieldInLine Then ' שמים אייקונים בכפתורי רדיו רק במידה וזה השדה היחיד בשורה
                        ImageFileId = rd.GetAttribute("ImageFileId")
                    Else
                        If rd.GetAttribute("ImageFileId") <> 0 Then
                            bp.DebugWarningList.Add("שמים אייקונים בכפתורי רדיו רק במידה וזה השדה היחיד בשורה")
                        End If
                    End If

                    sOut.Append(OneRadio(bp, oNode, oNodeScreen, iLin, i, sper, op, nm, HelpText(bp, oNode, oNodeScreen, rd, False), sN,
                                         bFound, bReadOnly OrElse rd.GetAttribute("ro") = "Y", ItIs(op, val, oNode.Param("typ")), bBkg, ImageFileId))
                    i = i + 1
                    If oNode.ParamVal("pkv") = 1 Then
                        'vk 02.05
                        If oNode.ParamVal("tch") <> 1 Then
                            iLin += bp.iScaleLin
                        ElseIf oNode.ParamVal("lin") = 0 Then
                            iLin += Int(bp.iScaleLin * 2 * bp.getPropertyValSng("FlexLineHeight")) 'vk 06.10
                        Else
                            iLin += bp.iScaleLin * 3
                        End If
                    End If
                End If
            Loop
            If sXML.Contains("<b") Then sOut.AppendLine("</div>")

            If oNode.Param("crd") Is Nothing Then
            Else
                sOut.Append(OneRadio(bp, oNode, oNodeScreen, iLin, i, sper, "0", oNode.Param("crd"), ".", sN, bFound, bReadOnly, Not bFound, bBkg, ImageFileId))
            End If
            sOptionT = sOut.ToString

        Catch e1 As Exception
            Dim sAddNote As String = "b," + op + "," + nm
            Throw New Exception("XML from AS400 is wrong. " + sAddNote + vbCrLf + e1.Message, e1)
        End Try
    End Sub

    Sub BuildOption_AsCheck(ByRef bp As BuildPage, ByVal sXML As String,
            ByRef oNode As Node, ByRef oNodeScreen As Node,
            ByRef sOptionT As String, ByRef sN As String, ByVal bReadOnly As Boolean, ByVal bBkg As Boolean) 'vk 01.04

        Dim sr As New StringReader(sXML)
        Dim rd As New Xml.XmlTextReader(sr)
        Dim opYes, opNo, val, sper As String
        Dim hYes As String = ""
        Dim hNo As String = ""
        Dim i As Integer = 0
        Dim oControlT As Control, oStyleT As Style, oNodeT As Node

        If oNode.ParamYes("per") Then
            sper = "1"
        Else
            sper = "0"
        End If

        Dim iLin As Integer = qStyle.ilin_fun(bp, oNode, oNodeScreen)
        val = oNode.ValModified

        Try
            opYes = "1"
            opNo = "0"
            Do While rd.Read
                If rd.NodeType = Xml.XmlNodeType.Element AndAlso rd.Name = "b" Then
                    Select Case i
                        Case 0
                            opYes = rd.GetAttribute("op")
                            hYes = HelpText(bp, oNode, oNodeScreen, rd, False)
                        Case 1
                            opNo = rd.GetAttribute("op")
                            hNo = HelpText(bp, oNode, oNodeScreen, rd, False)
                    End Select
                    i = i + 1
                End If
            Loop
            oNodeT = New Node()
            oNodeT.Param("lin") = oNode.Param("lin")
            oNodeT.Param("col") = oNode.ParamVal("col")
            oNodeT.Param("wd") = "1"

            'vk 11.05
            Dim iCol As Integer = qStyle.icol_fun(bp, oNodeT, oNodeScreen) - bp.iScaleWidth \ 2
            Dim iWidth As Integer = qStyle.iwid_fun(bp, oNodeT, oNodeScreen) + bp.iScaleWidth
            Dim iHeight As Integer = bp.iScaleLin - 1
            qStyle.SetCheckRadioSize(bp, "check", iLin, iCol, iWidth, iHeight)
            oStyleT = New Style()
            oStyleT.Add("top", iLin)
            oStyleT.Add("left", iCol)
            oStyleT.Add("width", iWidth)
            oStyleT.Add("height", iHeight)
            oStyleT.Add("line-height", iHeight)

            oControlT = New Control(oStyleT.FullCode)
            oControlT.Add("type", "checkbox")
            'oControlT.Add("class", "InpComSL")
            If Not bBkg Then 'vk 09.11
                oControlT.AddId("S" + sN)
            End If
            'oControlT.Add("tabindex", qStyle.TabShort(bp, oNode, oNodeScreen))
            'If Not bp.m_bKiosk Then
            oControlT.Add("onmouseenter", "help_check(this,true);") 'vk 02.04

            'End If
            oControlT.Add("helptext", "-") 'vk 02.04
            oControlT.Add("helptexttrue", hYes) 'vk 02.04
            oControlT.Add("helptextfalse", hNo) 'vk 02.04
            oControlT.Add("q", IIf(oNode.ParamVal("lin") = 0, "true", "false")) 'vk 02.04
            If ItIs(opYes, val, oNode.Param("typ")) Then
                oControlT.Add("checked", "true")
            End If
            If bReadOnly Then
                oControlT.Add("disabled", "true")
                oControlT.Unsel()
            Else
                oControlT.Add("onclick", "trS_click_check(this,'" + opYes + "','" + opNo + "'," + sper + ",'" + oNode.Param("pfk") + "');") 'pfk vk 02.05
                oControlT.Add("onclick", "help_check(this,false);") 'vk 02.04
                oControlT.Add("onfocus", "fnFocusQfk('" + LCase(oNode.Param("qfk")) + "');")
                oControlT.Add("onfocus", "fnFocusCombo(this,true);")

            End If
            oControlT.Add("onfocus", "bDontSelect=false;") 'vk 09.11
            sOptionT = oControlT.FullCode(bp)

        Catch e1 As Exception
            Dim sAddNote As String = "b," + i.ToString
            Throw New Exception("XML from AS400 is wrong. " + sAddNote + vbCrLf + e1.Message, e1)
        End Try
    End Sub

    Sub BuildOption_AsSelectOptions(ByRef bp As BuildPage, ByVal sXML As String,
            ByRef oNode As Node, ByRef oNodeScreen As Node, ByRef sOptionT As String) 'vk 12.03

        Dim sOut As New Text.StringBuilder()
        Dim sr As New StringReader(sXML)
        Dim rd As New Xml.XmlTextReader(sr)
        Dim val As String
        Dim op As String = ""
        Dim nm As String = ""
        Dim IsLangEnglish As Boolean

        Try
            If oNode.ParamIn("typ", "NS") Then
                op = "0".PadLeft(oNode.ParamVal("len"), "0")
            Else
                op = "".PadLeft(oNode.ParamVal("len"), " ") 'vk 05.20
            End If

            '  op = ""

            nm = "יש לבחור"
            If oNodeScreen.ParamVal("flang") = 1 Then IsLangEnglish = True


            val = oNode.ValModified
            'sOut.Append("<option class='firstOption' selected disabled='true' value='" + op + "'>" + $"{If(IsLangEnglish, "Select", "יש לבחור")}" + "</option>" + vbCrLf)
            sOut.Append("<option class='firstOption' selected value=''" + op + ">" + $"{If(IsLangEnglish, "Select", "יש לבחור")}" + "</option>" + vbCrLf) 'ntg 20.06.23 there should be an option to revert a choise

            Do While rd.Read
                If rd.NodeType = Xml.XmlNodeType.Element AndAlso rd.Name = "b" Then
                    op = rd.GetAttribute("op").PadLeft(oNode.ParamVal("len"), "0")
                    op = Right(op, oNode.ParamVal("len"))
                    nm = GetNm(rd, bp, oNode, oNodeScreen, True) 'vk 05.13
                    sOut.Append("<option ")
                    If ItIs(op, val, oNode.Param("typ")) Then
                        sOut.Append("selected ")
                    End If
                    sOut.Append("value='" + op + "'>" + nm + "</option>" + vbCrLf)
                End If
            Loop

            sOut.Append("</select>" + vbCrLf)
            sOptionT = sOut.ToString

        Catch e1 As Exception
            Dim sAddNote As String = "b," + op + "," + nm
            Throw New Exception("XML from AS400 is wrong. " + sAddNote + vbCrLf + e1.Message, e1)
        End Try

    End Sub

    Function CountLinesInCombo_ByTbl(ByRef bp As BuildPage, ByRef oNode As Node, ByRef oNodeScreen As Node) As Integer 'vk 07.03

        If oNode.ParamIn("pf4", "AB") Then
            Dim m As PocketKnife.Memory = bp.Memory
            Return m.CountByTwoKeys(oNode.ParamVal("pxk"), oNode.ParamVal("pxn"))
        End If

        Dim sSql As String
        Dim conn As SqlConnection = Nothing

        Dim dsRecords As New DataSet()
        Try
            sSql = "select Count(*)" + SqlMiddle(bp, oNode) '-sql-
            If oNodeScreen.ParamExists("dtk") Then
                'vk 07.05
                sSql += " and fltar=" + GetYear_ByTbl(bp, oNode, oNodeScreen)
                sSql += " and flfrdt<=" + oNodeScreen.Param("dtr")
                sSql += " and fltodt>=" + oNodeScreen.Param("dtr")
            End If
            conn = New SqlConnection(bp.m_sConnectionString)
            conn.Open()
            Dim dtAdapter As New SqlDataAdapter(sSql, conn)
            dtAdapter.SelectCommand.CommandType = CommandType.Text
            dtAdapter.Fill(dsRecords)
            With dsRecords.Tables(0).Rows
                If .Count = 0 Then
                    Return 0
                Else
                    Return .Item(0)(0)
                End If
            End With
        Catch e As Exception
            Throw New Exception("1020 DB error. " + vbCrLf + e.Message, e) 'vk 07.06
            Return 0 'vk 01.04
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

    Private Function GetYear_ByTbl(ByRef bp As BuildPage, ByRef oNode As Node, ByRef oNodeScreen As Node) As String 'vk 07.05

        If oNode.ParamIn("pf4", "AB") Then
            Return "0"
        End If

        Dim sSql As String
        Dim conn As SqlConnection = Nothing
        Dim dsRecords As New DataSet()
        Try
            sSql = "select Min(fltar)" + SqlMiddle(bp, oNode) + " and fltar>=" + oNodeScreen.Param("dtk") '-sql-
            conn = New SqlConnection(bp.m_sConnectionString)
            conn.Open()
            Dim dtAdapter As New SqlDataAdapter(sSql, conn)
            dtAdapter.SelectCommand.CommandType = CommandType.Text
            dtAdapter.Fill(dsRecords)
            With dsRecords.Tables(0).Rows
                If .Count = 0 OrElse Convert.IsDBNull(.Item(0)(0)) Then 'vk 08.06
                    Return "0"
                Else
                    Return .Item(0)(0).ToString()
                End If
            End With
        Catch e As Exception
            Throw New Exception("1020 DB error. " + vbCrLf + e.Message, e) 'vk 07.06
            Return "0"
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
    'ntg 10.07.24 new function- vladi's change regarding city-street screen
    Function BuildXml_CityStreet(ByRef bp As BuildPage, ByRef oNode As Node, ByRef oNodeScreen As Node) As String 'vk 06.24+

        Const Quo As String = """"
        Dim sOut As New Text.StringBuilder()
        Dim op, nm, sSql As String
        Dim conn As SqlConnection = Nothing
        Dim dsRecords As New DataSet()
        Dim dr As DataRow
        Try
            If Mid(oNode.Param("apr"), 2, 1) = "C" Then
                sSql = "select * from tblCities order by sText"
            Else
                Dim sCity As String
                If bp.cApr.Contains(Mid(oNode.Param("apr"), 1, 1) & "C") Then
                    sCity = bp.cApr(Mid(oNode.Param("apr"), 1, 1) & "C")
                End If
                sSql = "select * from tblStreets where sCity='" & sCity & "' order by sText"
            End If
            sOut.Append("<f val=" + Quo + oNode.Param("val") + Quo + " >")
            conn = New SqlConnection(bp.m_sConnectionString)
            conn.Open()
            Dim dtAdapter As New SqlDataAdapter(sSql, conn)
            dtAdapter.SelectCommand.CommandType = CommandType.Text
            dtAdapter.Fill(dsRecords)
            For Each dr In dsRecords.Tables(0).Rows
                op = dr("sCode").ToString '.Trim
                nm = dr("sText").ToString.Trim
                qConv.MakeOk(nm)
                sOut.Append("<b" _
                    + " op=" + Quo + op + Quo _
                    + " nm=" + Quo + nm + Quo _
                    + " ImageFileId=" + Quo + Quo _
                    + " h1=" + Quo + Quo _
                    + " h2=" + Quo + Quo _
                    + " h3=" + Quo + Quo)
                sOut.Append(" />")
            Next
            sOut.Append("</f>")
            Return sOut.ToString
        Catch e As Exception
            Throw New Exception("1020 DB error. " + vbCrLf + e.Message, e)
            Return "<f val=" + Quo + oNode.Param("val") + Quo + " ></f>"
        Finally
            Try
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            Catch
            End Try
            Try
                If conn IsNot Nothing Then
                    conn.Dispose()
                End If
            Catch
            End Try
            conn = Nothing
        End Try

    End Function

    Function BuildXml(ByRef bp As BuildPage, ByRef oNode As Node, ByRef oNodeScreen As Node,
            ByVal iRow As Integer) As String

        Const Quo As String = """"
        Dim sOut As New Text.StringBuilder()
        If oNode.ParamIn("pf4", "AB") Then
            'vk 09.06
            Dim m As PocketKnife.Memory = bp.Memory
            Dim l As SortedList = m.ListByTwoKeys(oNode.ParamVal("pxk"), oNode.ParamVal("pxn"), oNode.ParamExists("pxm"))
            Dim r As PocketKnife.Memory.Record
            Dim o As Object
            sOut.Append("<f val=" + Quo + oNode.Param("val") + Quo + " >")
            For Each o In l
                r = o.Value
                Dim s As String = r.sMtxt
                s = qConv.ConvAlf(
                    bp, s, "", "AS2PC", IIf(oNode.ParamYes("prl"), "rtl", "ltr").ToString, False,
                    Not (oNodeScreen.Param("wait") = "S" OrElse oNode.ParamVal("pfn") = 8),
                    oNode.ParamYes("uni"),
                    oNodeScreen.Param("wait") = "S", oNodeScreen.Param("flr"), True) 'pfn deleted vk 12.09, True vk 01.10
                sOut.Append("<b" _
                    + " op=" + Quo + r.nNoin.ToString + Quo _
                    + " nm=" + Quo + s + Quo _
                    + " heb=" + Quo + IIf(r.bHebrew, "1", "0") + Quo _
                    + " ex=" + Quo + r.sExcl + Quo _
                    + " h1=" + Quo + Quo _
                    + " h2=" + Quo + Quo _
                    + " h3=" + Quo + Quo _
                    + " dl=" + Quo + r.nDel.ToString + Quo _
                    + " />")

            Next
            sOut.Append("</f>")
            Return sOut.ToString
        End If

        Dim sField As String = IIf(oNode.ParamIn("pf4", "AB"), "flmtxt", IIf(
                                   MayAsRadio(oNode, iRow) AndAlso
                                   oNode.ParamVal("psz") = 0 AndAlso
                                   oNode.ParamVal("pkv") = 0 AndAlso
                                   bp.getProperty("AlwaysLongText").ToLower <> "true", "flstxt", "flltxt")) 'vk 09.24 AlwaysLongText

        Dim op, nm, h1, h2, h3 As String 'vk 01.04
        h1 = ""
        h2 = ""
        h3 = ""
        Dim sSql As String

        Dim conn As SqlConnection = Nothing

        Dim dsRecords As New DataSet()
        Dim dr As DataRow ', dc As DataColumn
        Dim bHasHelp As Boolean = False 'vk 02.04
        Try
            sSql = "select *" + SqlMiddle(bp, oNode) '-sql-
            If oNodeScreen.ParamExists("dtk") Then
                sSql += " and fltar=" + GetYear_ByTbl(bp, oNode, oNodeScreen)
                sSql += " and flfrdt<=" + oNodeScreen.Param("dtr")
                sSql += " and fltodt>=" + oNodeScreen.Param("dtr")
            End If
            If MayAsCheck(oNode, iRow) OrElse oNode.ParamExists("pxm") Then
                sSql += " order by flnoin"
            Else
                sSql += " order by " + sField
            End If
            sOut.Append("<f val=" + Quo + oNode.Param("val") + Quo + " >")
            conn = New SqlConnection(bp.m_sConnectionString)
            conn.Open()
            Dim dtAdapter As New SqlDataAdapter(sSql, conn)
            dtAdapter.SelectCommand.CommandType = CommandType.Text
            dtAdapter.Fill(dsRecords)
            bHasHelp = Not (dsRecords.Tables(0).Columns("flkyml") Is Nothing) 'vk 12.07
            For Each dr In dsRecords.Tables(0).Rows
                Dim ImageFileId = dr("iconid").ToString
                op = dr("flnoin").ToString
                If bHasHelp Then
                    If dr("flkyml").ToString = "1" Then
                        h1 = dr("flmll1").ToString.Trim : qConv.MakeOk(bp, oNodeScreen, h1)
                        h2 = dr("flmll2").ToString.Trim : qConv.MakeOk(bp, oNodeScreen, h2)
                        h3 = dr("flmll3").ToString.Trim : qConv.MakeOk(bp, oNodeScreen, h3)
                    Else
                        h1 = ""
                        h2 = ""
                        h3 = ""
                    End If
                    If dr("flkyml").ToString = "2" Then
                        'vk 02.05
                        nm = dr("flmll1").ToString +
                            dr("flmll2").ToString +
                            dr("flmll3").ToString
                    Else
                        nm = dr(sField).ToString
                    End If
                Else
                    nm = dr(sField).ToString 'vk 02.05
                End If
                nm = nm.Trim 'vk 02.05

                qConv.MakeOk(nm) 'vk 05.13
                sOut.Append("<b" _
                    + " op=" + Quo + op + Quo _
                    + " nm=" + Quo + nm + Quo _
                    + " ImageFileId=" + Quo + ImageFileId + Quo _
                    + " h1=" + Quo + h1 + Quo _
                    + " h2=" + Quo + h2 + Quo _
                    + " h3=" + Quo + h3 + Quo)
                If oNode.ParamIn("cry", "GI") Then
                    If oNode.ParamIn("pf4", "AB") Then
                        sOut.Append(" dl=" + Quo + dr("fldel").ToString + Quo)
                    Else
                        sOut.Append(" dl=" + Quo + "0" + Quo)
                    End If
                End If
                sOut.Append(" />")
            Next
            sOut.Append("</f>")
            Return sOut.ToString
        Catch e As Exception
            Throw New Exception("1020 DB error. " + vbCrLf + e.Message, e) 'vk 07.06
            Return "<f val=" + Quo + oNode.Param("val") + Quo + " ></f>" 'vk 01.04
        Finally
            Try
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            Catch
            End Try
            Try
                If conn IsNot Nothing Then
                    conn.Dispose()
                End If
            Catch
            End Try
            conn = Nothing
        End Try

    End Function

    Private Function ItIs(ByVal op As String, ByVal val As String, ByVal typ As String) As Boolean 'vk 06.03
        Select Case typ
            Case "N", "S" '"S" vk 09.03
                If val = "" Then val = "0"
                Return Int32.Parse(op) = Int32.Parse(val)
            Case Else
                Return op = val
        End Select
    End Function

    Function ComboComment(ByRef bp As BuildPage, ByVal sXML As String, ByVal bBkg As Boolean) As String 'vk 08.06

        Dim sr As New StringReader(sXML)
        Dim rd As New Xml.XmlTextReader(sr)
        Dim oNode As Node = Nothing
        Dim s As String = ""

        Try
            If sXML = "" Then Return ""
            Do While rd.Read
                If rd.NodeType = Xml.XmlNodeType.Element AndAlso rd.Name = "b" Then
                    oNode = New Node(rd, bp.numL, bp.NOTSPACE)
                    s += oNode.FullCode(bp.getProperty("IncludeXml"), IIf(bBkg, "main", "window").ToString) 'vk 09.11
                End If
            Loop
            Return s
        Catch e1 As Exception
            Return "<!-- " + e1.Message + "-->"
        Finally
            If Not sr Is Nothing Then sr.Dispose()
            If Not oNode Is Nothing Then oNode.Dispose()
        End Try

    End Function

    Private Function OneRadio(ByRef bp As BuildPage, ByRef oNode As Node, ByRef oNodeScreen As Node, ByVal iLin As Integer, ByVal i As Integer, ByVal sper As String, ByVal op As String, ByVal nm As String, ByVal h As String, ByVal sN As String,
            ByRef bFound As Boolean, ByVal bReadOnly As Boolean, ByVal bCurrent As Boolean, ByVal bBkg As Boolean, ByVal ImageFileId As Integer) As String

        Dim oNodeT As Node, oStyleT As Style, oControlT As Control, s As String
        'vk 02.05
        Dim bHoriz As Boolean = oNode.ParamVal("pkv") = 0
        Dim bButton As Boolean = oNode.ParamVal("tch") = 1
        Dim bEnglish As Boolean = oNodeScreen.ParamVal("flr") = 1
        Dim bShGamish As Boolean = oNode.ParamVal("lin") = 0
        Dim bMan As Boolean = oNode.Param("man") = "Y"  'ntg 18.12.23 added bMan to add bRequired:=bMan becaues radio btns were not shown as mandatory fields
        'vk 07.10
        Dim bGray As Boolean = bp.getProperty("GrayDisabledTouchButtons") = "true"
        Dim ImageFileName As String = If(ImageFileId = 0, "", bp.m_colColor.Item($"ImageFile_{ImageFileId}"))

        'radio button
        oNodeT = New Node()

        If bHoriz Then
            oNodeT.Param("lin") = oNode.Param("lin")
        ElseIf bShGamish Then
            oNodeT.Param("ind") = (oNode.ParamVal("ind") + i).ToString
        Else
            oNodeT.Param("lin") = (oNode.ParamVal("lin") + i).ToString
        End If

        If bHoriz Then
            Dim n1, n2 As Integer
            If Not bButton Then
                n1 = 11
            ElseIf bp.getProperty("TouchButtons") = "kiosk" Then
                n1 = 20
            Else
                n1 = oNode.Param("wd") + 1 'vk 05.06
            End If
            If bEnglish Then
                n2 = 1
            Else
                n2 = -1
            End If
            oNodeT.Param("col") = (oNode.ParamVal("col") + n1 * i * n2).ToString
        Else
            oNodeT.Param("col") = oNode.Param("col")
        End If

        If Not bButton Then
            oNodeT.Param("wd") = "1"
        ElseIf bHoriz AndAlso bp.getProperty("TouchButtons") = "kiosk" Then 'vk 05.06
            oNodeT.Param("wd") = "11"
        Else
            oNodeT.Param("wd") = oNode.Param("wd")
        End If

        oStyleT = New Style()

        If bButton Then
            If bReadOnly AndAlso bGray Then
                oStyleT.Add("color", "gray")
                oStyleT.Add("border-style", "outset")
                oStyleT.Add("text-align", "center")
            ElseIf oNode.ParamExists("pcl") Then
                oStyleT.Add("color", bp.m_colColor.Item("clr_" + oNode.Param("pcl")).ToString)
            End If
        End If

        If oNode.ParamVal("pkv") = 1 Then
            oStyleT.Add("width", "100% !important") 'vk 09.24
        End If

        oControlT = New Control(oStyleT.FullCode)
        Dim RadioId As String = "S" + sN + i.ToString()
        oControlT.Add("id", RadioId)
        oControlT.Add("name", "S" + sN)

        If bButton Then
            oControlT.Add("type", "button")
            If bp.getProperty("TouchButtons") = "kiosk" Then
            Else
                oControlT.Add("onmouseenter", "fnBtnEnter_Field(this," + IIf(oNode.Param("val").Trim.Length > oNode.ParamVal("wd"), "true", "false").ToString + ");")
            End If
            oControlT.Add("value", nm)
            oControlT.Add("cap", nm)
            oControlT.Add("optval", op.PadLeft(oNode.ParamVal("len"), "0"))
        Else
            oControlT.Add("type", "radio")

            oControlT.Add("value", op.PadLeft(oNode.ParamVal("len"), "0"))
        End If
        oControlT.Add("onmouseenter", "help_radio(this);") 'vk 02.04
        oControlT.Add("helptext", h) 'vk 02.04
        oControlT.Add("q", IIf(bShGamish, "true", "false")) 'vk 02.04
        If bCurrent Then
            If bButton Then
                oControlT.Add("chck", "true")
            Else
                oControlT.Add("checked", "true")
            End If
            bFound = True
        End If
        If bReadOnly Then
            oControlT.Add("disabled", "true")
            oControlT.Unsel()
        Else
            If bButton Then
                If bp.getProperty("TouchButtons") = "kiosk" Then
                    oControlT.Add("onclick", "fnKeyboard_Radio(this, true);")
                Else
                    oControlT.Add("onclick", "fnKeyboard_Radio(this, false);") 'vk 05.06, 08.06
                End If
                oControlT.Add("per", sper)
                oControlT.Add("cod", oNode.Param("pfk"))
            Else
                If bp.getProperty("TouchButtons") = "kiosk" Then
                    oControlT.Add("onclick", "trS_click_radio(this," + sper + ",'" + oNode.Param("pfk") + "',true);") 'pfk vk 02.05
                Else
                    oControlT.Add("onclick", "trS_click_radio(this," + sper + ",'" + oNode.Param("pfk") + "',false);changeBorderColor(event);removeInvalidMsg(event);") 'vk 08.06
                End If
            End If

            If oNode.ParamVal("psz") > 0 AndAlso oNode.ParamExists("pxr") Then ' Radio with big Image
                oControlT.Add("onclick", "fnBtnClick(this);")
                oControlT.Add("pch", oNode.Param("pxr"))
            End If

            oControlT.Add("onfocus", "fnFocusQfk('" + LCase(oNode.Param("qfk")) + "');")
            If Not bButton Then
                oControlT.Add("onfocus", "fnFocusCombo(this,false);")
            End If
        End If
        oControlT.Add("onfocus", "bDontSelect=false;") 'vk 09.11
        oControlT.Add("grouplevel", oNode.Param("clv"))
        If bButton AndAlso bReadOnly AndAlso bGray Then
            s = oControlT.FullCodePseudoButton(bp)
        Else
            's = oControlT.FullCode(bp ImageFileName:=ImageFileName)
            s = oControlT.FullCode(bp, bRequired:=bMan, ImageFileName:=ImageFileName) 'ntg 18.12.23 added  bRequired:=bMan becaues radio btns were not shown as mandatory fields
        End If
        If bButton Then
            If Not oNodeT Is Nothing Then oNodeT.Dispose()
            Return s
        End If

        'label
        oNodeT = New Node()

        If bHoriz Then
            oNodeT.Param("lin") = oNode.Param("lin")
        ElseIf bShGamish Then
            oNodeT.Param("ind") = (oNode.ParamVal("ind") + i).ToString
        Else
            oNodeT.Param("lin") = (oNode.ParamVal("lin") + i).ToString
        End If

        If Not bHoriz Then
            oNodeT.Param("col") = oNode.Param("col")
        ElseIf bEnglish Then
            oNodeT.Param("col") = (oNode.ParamVal("col") + (11 * i + 1)).ToString
        Else
            oNodeT.Param("col") = (oNode.ParamVal("col") - (11 * i + 10)).ToString
        End If

        oNodeT.Param("wd") = "10"
        oStyleT = New Style()
        oStyleT.Add("position", "absolute")
        oStyleT.Add("background-color", "transparent")

        oControlT = New Control(oStyleT.FullCode)
        oControlT.Add("dir", IIf(bEnglish, "ltr", "rtl"))
        oControlT.Add("for", RadioId)
        oControlT.Add("value", nm)
        If oNodeT IsNot Nothing Then oNodeT.Dispose()

        s = s + oControlT.FullCodeSpan(bp, ImageFileName:=ImageFileName)
        Dim BigImageInRadio = oNode.ParamVal("psz") > 0
        If ImageFileName = "" Or BigImageInRadio Then
            s = $"<div class=' {If(BigImageInRadio, " BigImageDiv col ", "")} form-check'>" + Environment.NewLine + s + "</div>"
        End If

        Return s
    End Function

    Function MayAsRadio(ByRef oNode As Node, ByVal iRow As Integer) As Boolean
        If oNode.ParamIn("cry", "GI") Then Return False
        If oNode.Param("psl") = "G" Then Return True
        If Not (oNode.ParamYes("cry") OrElse oNode.ParamVal("tch") = 1) Then Return False
        If oNode.ParamVal("pkv") = 1 Then Return True
        If oNode.ParamIn("cry", "Y") Then Return iRow <= 3 'ntg 05.03.24  for case of 3 radio buttons

        Return iRow <= 2
    End Function
    Function MayAsCheck(ByRef oNode As Node, ByVal iRow As Integer) As Boolean 'vk 02.05
        If oNode.Param("cry") <> "C" Then Return False
        Return iRow <= 2 AndAlso iRow > 0
    End Function
    Private Function SqlMiddle(ByRef bp As BuildPage, ByRef oNode As Node) As String 'vk 09.06
        Dim s As String
        If oNode.ParamIn("pf4", "AB") Then
            '-sql-
            s = " from tblMemory where flguid='" + bp.m_sGuid + "' and"
            If Not oNode.ParamIn("cry", "GI") Then
                s += " fldel=0 and"
            End If
        Else
            s = " from tblFlex where"
        End If
        s += " flkod=" + oNode.Param("pxk") + " and fltbno=" + oNode.Param("pxn")
        Return s
    End Function
    Private Function GetNm(ByVal rd As Xml.XmlTextReader, ByRef bp As BuildPage,
            ByRef oNode As Node, ByRef oNodeScreen As Node, ByVal bMakeOk As Boolean) As String 'vk 05.13
        Dim nm As String
        nm = rd.GetAttribute("nm")
        qConv.MakeNotOk(nm)
        Select Case oNode.Param("pf4")
            Case "R", "A", "B"
            Case "S"
            Case "C", "D"
                If oNode.ParamYes("uni") Then
                    nm = oInfo.FromUni(nm)
                Else
                    nm = qConv.ConvAlf_Simple(bp, nm, oNodeScreen, "")
                End If
        End Select
        If bMakeOk Then
            qConv.MakeOk(nm)
        End If
        Return nm.Trim
    End Function

    Sub Dispose()
        qConv = Nothing
        qStyle = Nothing
        oInfo = Nothing
        oConv = Nothing
    End Sub
End Class
