Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.IO   ' NET20
Imports System.Text
Imports System.Xml.Linq
Imports System.Linq
Imports Comtec.TIS

Friend Class Multi
    Private qColor As New Color()
    Private qCombo As New Combo()
    Private qConv As New Conv()
    Private qStyle As New StyleModule()
    Private qSecurity As New Security() 'vk 06.04

    Private sN, sStyle As String
    Private dir As String
    Private sfnClearOptions As String
    Private sStyleT As String
    Private ilin, icol, iwidth As Integer

    Private len1 As String ', col1 As String
    Private iLen, iComma, iSign, iPoint, iAdd As Integer

    Private sXML_Combo As String
    Private oControl, oControlT As Control
    Private bNoControl As Boolean
    Private nPosOfLogo As Integer
    Private Const sBigPoint As String = "*" '"&#149;" 'ChrW(9679)
    Private iRow As Integer, sOptionT As String
    Private GridLineNo As Integer = 1
    Private FirstTimeActionButtonsMenu As Boolean = True

    Function Hidden(ByVal sName As String, Optional ByVal sValue As String = "",
            Optional ByVal sParamName As String = "", Optional ByVal sParamVal As String = "") As String 'vk 09.05
        Dim oControlT As New Control()
        oControlT.AddId(sName)
        oControlT.Add("type", "hidden")
        oControlT.Add("value", sValue, True) 'vk 11.07
        If sParamName > "" Then oControlT.Add(sParamName, sParamVal, True)
        Return oControlT.FullCode(Nothing)
    End Function

    Sub BuildField(ByRef bp As BuildPage, ByRef rd As Xml.XmlTextReader,
            ByRef sOut As StringBuilder, ByRef sOut_Local As StringBuilder, ByRef sOutOut As StringBuilder,
            ByRef oNode As Node, ByRef oNodeScreen As Node,
            ByVal sFind As String, ByVal sFil As String, ByVal bBkg As Boolean, ByVal m_ModalScreen As String) 'vk 02.23

        bNoControl = False
        sfnClearOptions = ""

        If oNodeScreen.ParamIn("fgr", "LAPD") Then 'vk 09.24 fgr=D
            sfnClearOptions = "fnClearOptions();"
        End If
        ' ------------------------------------
        ' Take attributes of field
        ' ------------------------------------

        Dim sPrefix As String = "F" 'vk 02.04
        With oNode
            Dim v As String
            For Each v In AllParams_Field
                .AddIfNo(v)
            Next
            .AddIfNo("apr", "  ") 'vk 05.05
            If .ParamIn("apr", "MYG", 2) AndAlso Not .ParamExists("per") AndAlso oNodeScreen.Param("fgr") <> "P" Then
                .Param("per") = "Y"
                .Param("pfk") = "modellist1(" + Left(.Param("apr"), 1) + ",true);"
            End If
            For Each v In AllParams_Field0
                .AddIfNo(v, "0")
            Next
            For Each v In AllParams_FieldN
                .AddIfNo(v, "N")
            Next
            'vk 02.05  
            For Each v In AllParams_Field00
                .AddIfNo(v, "00")
            Next
            For Each v In AllParams_Field000
                .AddIfNo(v, "000")
            Next
            If .ParamIn("cry", "GI") Then
                .AddIfNo("pht", bp.getProperty("DefaultHeight")) 'vk 09.06
            Else
                .AddIfNo("pht", "01")
            End If
            .AddIfNo("num", "".PadRight(bp.numL, bp.NOTSPACE))
            .AddIfNo("col", "1")
            .AddIfNo("qfk", "")
            .AddIfNo("prl", "Y")
            .AddIfNo("tab", "-1")

            '.AddIfNo("inp", If(.ForBoo("label", winVal:=oNodeScreen.Param("win"), fdspVal:=oNodeScreen.ParamVal("fdsp")), bp.inpD, "P"))
            .AddIfNo("inp", If(.ForBoo("label", winVal:=oNodeScreen.Param("win"), fdspVal:=oNodeScreen.ParamVal("fdsp"), filVal:=oNodeScreen.Param("fil")), bp.inpD, "P")) 'vk 05.08.24 for cities and streets screen

            .AddIfNo("dec", bp.decD)
            .AddIfNo("ind", bp.indD)
            .AddIfNo("pxr", "10") 'vk 03.14
            'vk 07.05
            If .ParamYes("heb") Then '.ParamIn("inp", "IKR") AndAlso rem vk 01.06
                .Param("prl") = "Y"
            End If

            If .Param("pfn") Is Nothing Then
                .AddIfNo("pfn")
            Else
                .Param("pfn") = .Param("pfn").PadLeft(3, "0")
            End If
            If .Param("pcl") Is Nothing Then
                .AddIfNo("pcl")
            Else
                .Param("pcl") = .Param("pcl").PadLeft(3, "0")
            End If
            If m_ModalScreen <> "" AndAlso .Param("inp") <> "O" Then 'vk 02.23
                .Param("inp") = "P"
                .Param("f4p") = "N"
            End If
            If .ParamYes("prl") Then 'heb vk 06.05, deleted 01.06
                dir = "rtl"
            Else
                dir = "ltr"
            End If

            If oNode.ParamYes("pf4") AndAlso Not oNode.ParamIn("inp", "IK") Then _
                sPrefix = "E"

            sN = sPrefix + .LongName + oNodeScreen.Param("fdate")

            If bp.m_sCurClass = "form-control" Then
                bp.m_InputId = sN
            End If

            If .ParamIn("apr", "B", 2) AndAlso bp.cFlexCombos IsNot Nothing Then
                Dim s As String = "", oFlexCombo As FlexCombo, bDouble As Boolean = False
                Dim BossSN As String = ""
                For Each oFlexCombo In bp.cFlexCombos
                    If oFlexCombo.sGroup = Left(.Param("apr"), 1) AndAlso oFlexCombo.bBoss AndAlso oFlexCombo.bDouble Then
                        bDouble = True
                    End If

                    If oFlexCombo.sGroup = Left(.Param("apr"), 1) AndAlso oFlexCombo.bBoss Then
                        BossSN = oFlexCombo.sN
                    End If
                Next
                If Not bDouble Then
                    For Each oFlexCombo In bp.cFlexCombos
                        If oFlexCombo.sGroup = Left(.Param("apr"), 1) AndAlso Not oFlexCombo.bBoss Then
                            s += "flexlist1(\'" + BossSN + "\',\'" + oFlexCombo.sN + "\',true);"
                        End If
                    Next
                    .Param("per") = "Y"
                    .Param("pfk") = s
                End If
            End If
            'vk 09.24
            If bp.m_bStatementWindow Then
                .Param("inp") = "O"
            End If
        End With

        If oNode.Param("num") = "SUGM298".PadRight(bp.numL, bp.NOTSPACE) Then
            sOut.Append(Hidden("HarchiveDT", sN))
        End If

        ' ----------------------------------
        ' Change length with regards to type
        ' ----------------------------------

        'idec = oNode.ParamVal("dec")

        With oNode
            iLen = .ParamVal("len")
            Select Case .Param("typ")
                Case "N", "S"
                    iComma = IIf(.Param("edt") = "J", (iLen - .ParamVal("dec") - 1) \ 3, 0)
                    iSign = IIf(.Param("typ") = "S", 1, 0)
                    iPoint = IIf(.ParamVal("dec") > 0, 1, 0)
                    iAdd = iComma + iSign + iPoint
                Case "D", "Y", "T" : iAdd = 2 ' Date DD/MM/YY, DD/MM/YYYY, Time T HH:MM:SS
                Case "H", "U", "Q" : iAdd = 1 ' Date MM/YYYY, Time U HH:MM 'Q vk 07.05
                Case Else : iAdd = 0
            End Select
            len1 = (iLen + iAdd).ToString("00000")
            .AddIfNo("wd", len1)
            .Param("inp") = .Param("inp").Trim
        End With

        nPosOfLogo = 0
        If oNodeScreen.Param("wait") = "S" AndAlso oNode.Param("inp") = "O" Then
            nPosOfLogo = InStr(oNode.Param("val"), "@@001")
            If nPosOfLogo > 0 Then Mid(oNode.Param("val"), nPosOfLogo, 5) = Space(5)
        End If

        sStyle = qStyle.GetStyleTxt(bp, oNode, oNodeScreen)

        ' ----------------------------------
        ' Change value with regards to type
        ' ----------------------------------

        With oNode
            If .Param("tch") = "A" Then
                .Param("val") = "" 'vk 09.18
            Else
                Select Case .Param("typ")
                    Case "S", "N"
                        If .Param("typ") = "S" AndAlso .ParamExists("sgn") Then
                            .Param("val") = .Param("sgn") + .Param("val")
                        End If
                        If .ParamIn("pf4", "RCASDB") Then
                            .Param("val") = Right(.Param("val").PadLeft(.Param("len"), "0"), .Param("len"))
                        Else
                            .Param("val") = qConv.ConvNum(bp, .Param("val"), sN, "AS2PC", .Param("edt"), .Param("ewr"))
                        End If
                    Case "A"
                        If .ParamExists("val") Then
                            If .ParamIn("apr", "CS", 2) AndAlso oNodeScreen.Param("fgr") <> "P" Then
                                'vk 12.09
                                If .ParamYes("prl") Then
                                    .Param("val") = Right(.Param("val"), 5)
                                Else
                                    .Param("val") = Left(.Param("val"), 5)
                                End If
                            ElseIf .ParamYes("heb") AndAlso Not .ParamIn("inp", "IKR") AndAlso Not BOOTSTRAP Then
                                'Stop

                                'Else
                            ElseIf .Param("ewr") <> "H" OrElse .Param("inp") <> "P" Then 'vk 02.24+
                                .Param("val") = qConv.ConvAlf(
                                bp, .Param("val"), sN, "AS2PC", dir,
                                    .ParamYes("heb") AndAlso Not .ParamIn("inp", "IKR"),
                                    Not (oNodeScreen.Param("wait") = "S" OrElse (
                                        .ParamVal("pfn") = 8 AndAlso .ParamIn("inp", "OPQ"))),
                                    .ParamYes("uni"),
                                    oNodeScreen.Param("wait") = "S", oNodeScreen.Param("flr")) 'pfn deleted vk 11.09
                            End If
                        ElseIf Not .ParamYes("pul") Then
                            .Param("val") = ""
                        End If
                    Case "D", "Y", "H", "Q", "B" 'Q vk 07.05, ntg 05.03.24 added "B"
                        Select Case oNode.Param("inp")
                            Case "I", "K", "R"
                                bp.m_sSpecialCtl = "date" 'boo? date
                            Case Else
                                .Param("val") = qConv.ConvDate(bp, .Param("val"), sN, "AS2PC")
                        End Select
                    Case "T", "U"
                        .Param("val") = qConv.ConvTime(bp, .Param("val"), sN, "AS2PC")
                End Select
            End If
        End With

        ' ----------------------------------
        ' Display controls with regards to type
        ' ----------------------------------
        If oNode.ParamIn("apr", "UVWIJ", 2) AndAlso oNodeScreen.Param("fgr") <> "P" Then
            sOut.Append(Hidden("H" + LCase(
                Mid(oNode.Param("apr"), 2, 1)) +
                Mid(oNode.Param("apr"), 1, 1) + "_name", sN))
        End If

        If oNode.ParamExists("pul") AndAlso oNode.Param("pul") = "S" Then
            oControl = New Control(sStyle)
            If (Not oNode.ParamIn("pf4", "CRADSB")) OrElse 'bp.getProperty("ComboStyle") <> "ms" OrElse
                        (Not oNode.ParamIn("inp", "IKR")) Then 'R vk 07.04
                oControl.Add("type", "text")
            End If
        ElseIf oNode.Param("pic") <> "000" Then
            'vk 02.05
            oControlT = New Control(qStyle.GetStylePos(bp, oNode, oNodeScreen))
            Dim s As String = oNodeScreen.Param("cli") + oNode.Param("pic")
            Try
                s = bp.m_colColor.Item("element_" + s)
                If BOOTSTRAP Then
                    bp.m_sOneSomething = oControlT.FullCode(bp, s)
                End If
                sOut_Local.Append(oControlT.FullCode(bp, s))
            Catch
            End Try
            Exit Sub
        ElseIf oNode.ParamVal("tch") = 2 Then
            'vk 02.05
            sOut.Append(Hidden(sN))
            If BOOTSTRAP Then
                bp.m_sOneSomething = sOut.ToString
            End If
            Exit Sub
        ElseIf oNode.Param("pwd") = "P" Then
            'vk 07.13
            sOut.Append(Hidden(sN, oNode.Param("val")))
            If BOOTSTRAP Then
                bp.m_sOneSomething = sOut.ToString
            End If
            Exit Sub
        ElseIf oNode.ParamYes("only") Then
            If bBkg Then Exit Sub 'vk 07.06
            sOut.Append(Hidden("Honly", sN, "len", oNode.ParamVal("len").ToString))
            oControl = New Control()
            oControl.SetStyle("style='display:none;'")
            oControl.Add("type", "hidden")
        ElseIf oNode.ParamIn("inp", "VW") Then 'vk 05.05
            oControlT = New Control(qStyle.GetStyleV(bp, oNode, oNodeScreen))
            Select Case oNode.Param("inp")
                Case "V" : oControlT.Add("src", "pics/V.gif")
                Case "W" : oControlT.Add("src", "pics/W.gif")
            End Select
            'vk 02.04, 03.04
            oControlT.Add("pch", "14")
            oControlT.Add("onclick", "fnPencilClick(this);")
            oControlT.Add("fld", oNode.Param("num")) 'vk 05.05
            oControlT.Add("ind", oNode.Param("ind")) 'vk 05.05
            sOut_Local.Append(oControlT.FullCode(bp, "img"))
            If BOOTSTRAP Then
                bp.m_sOneSomething = oControlT.FullCode(bp, "img")
            End If
            Exit Sub

        ElseIf oNode.Param("inp") = "H" Then
            Dim s As String
            If oNode.ParamExists("hky") OrElse oNode.ParamExists("bua") Then 'bua vk 07.04
                s = qColor.GetHelp(bp, oNode, oNodeScreen, "2")
            Else
                s = ""
            End If
            oControlT = New Control(qStyle.GetStyleV(bp, oNode, oNodeScreen))
            oControlT.Add("src", "pics/H.gif")
            If s > "" Then
                oControlT.Add("helptext", s)
                oControlT.Add("onclick", "ShowHelp(this);")
            End If
            sOut_Local.Append(oControlT.FullCode(bp, "img"))
            If BOOTSTRAP Then
                bp.m_sOneSomething = oControlT.FullCode(bp, "img")
            End If
            Exit Sub
        ElseIf oNode.ParamExists("chb") Then
            oControl = New Control(sStyle)
            oControl.Add("type", "checkbox")
        ElseIf oNode.Param("psl") = "G" Then
            oControl = New Control(sStyle)
            oControl.Add("type", "radio")
            oControl.Add("title", "דאבל-קליק") 'ntg 17.01.24 if its a table with radio buttons, the double click will appear

        ElseIf oNode.ParamYes("pwd") Then
            oControl = New Control(sStyle)
            oControl.Add("type", "password")
        Else
            oControl = New Control(sStyle)
            If (Not oNode.ParamIn("pf4", "CRADSB")) OrElse 'bp.getProperty("ComboStyle") <> "ms" OrElse
                    (Not oNode.ParamIn("inp", "IKR")) Then 'R vk 07.04
                oControl.Add("type", "text")
            End If
        End If

        BuildField_Continue(bp, rd, sOut, sOut_Local, sOutOut, oNode, oNodeScreen, sFind, sFil, False, bBkg)

        If oNode.Param("typ") = "A" AndAlso dir = "ltr" Then
            bp.sListOfLtr += sN + ";"
        End If
        If oNode.ParamYes("uni") Then
            bp.sListOfUni += sN + ";" 'vk 02.05
        End If

        'value text for combo 'boo? F->FF
        If oNode.Param("cry") = "V" Then
            With oNode
                .Param("apr") = " "
                .Param("cry") = " "
                .Param("pf4") = " "
                If oNodeScreen.ParamVal("flr") = 1 Then
                    .Param("col") = (.ParamVal("col") + .ParamVal("wd") + 2).ToString
                Else
                    .Param("col") = (.ParamVal("col") - .ParamVal("len") - 2).ToString
                End If
                .Param("wd") = .Param("len")
                .Param("val") = qConv.ConvNum(bp, .Param("val"), sN, "AS2PC", .Param("edt"), .Param("ewr"))
            End With
            oControl = New Control(qStyle.GetStyleTxt(bp, oNode, oNodeScreen))
            BuildField_Continue(bp, rd, sOut, sOut_Local, sOutOut, oNode, oNodeScreen, "", "", True, bBkg)
        End If
        If Not oControl Is Nothing Then oControl.Dispose()
        oControl = Nothing
        If Not oControlT Is Nothing Then oControlT.Dispose()
        oControlT = Nothing
        If BOOTSTRAP Then
            bp.m_sOneSomething = sOut.ToString & sOut_Local.ToString & sOutOut.ToString
        End If

    End Sub

    Private Sub BuildField_Continue(ByRef bp As BuildPage, ByRef rd As Xml.XmlTextReader,
        ByRef sOut As StringBuilder,
        ByRef sOut_Local As StringBuilder,
        ByRef sOutOut As StringBuilder,
        ByRef oNode As Node, ByRef oNodeScreen As Node,
        ByVal sFind As String, ByVal sFil As String,
        ByVal bAdditionalField As Boolean, ByVal bBkg As Boolean)

        If oNode.ParamVal("lin") = 0 Then
            oControl.Add("onfocus", "fnFocusQfk('" + LCase(oNode.Param("qfk")) + "');")
        End If

        If oNode.ParamYes("hlp") AndAlso Not bBkg Then
            bp.m_bHelpButton = True
            oControl.Add("onfocus", "HelpOff(false);")
            oControl.Add("onblur", "HelpOff(true);") 'onblur vk 05.08
        End If
        If oNode.ParamIn("apr", "MYG", 2) AndAlso oNodeScreen.Param("fgr") <> "P" Then 'vk 05.05
            Dim s As String = "modellist1(" + Left(oNode.Param("apr"), 1) + ",false);"
            If bp.m_sModelCombosFill.IndexOf(s) < 0 Then
                bp.m_sModelCombosFill += s + vbCrLf
            End If
        End If

        'ntg 10.07.24 vladi's change regarding city-street screen
        If bp.getProperty("CityStreet") = "inline" Then
            If oNode.ParamIn("apr", "CNST", 2) AndAlso oNodeScreen.Param("fgr") <> "P" Then
                bp.m_bStreetCombos = True
            End If
        Else
            If oNodeScreen.Param("adr") = "R" AndAlso oNode.ParamIn("apr", "CNST", 2) AndAlso oNode.ParamYes("per") AndAlso Not bBkg AndAlso oNodeScreen.Param("fgr") <> "P" Then 'vk 11.09
                Try
                    bp.cAdrPer.Add("1;" + oNode.Param("pfk"), Left(oNode.Param("apr"), 1))
                Catch
                End Try
            End If
        End If 'ntg 10.07.24 vladi's change regarding city-street screen

        sXML_Combo = "" 'vk 02.14
        Select Case oNode.Param("inp")
            Case "I", "K", "R"
                If oNode.Param("psl") = "G" AndAlso oNode.ParamVal("pxk") = 0 Then ' אם זה כפתור רדיו בודד - ובלי קישור למסד הנתונים
                    BuildInputRadio(bp, rd, sOut, sOut_Local, oNode, oNodeScreen, bBkg)
                ElseIf oNode.ParamExists("chb") Then
                    BuildInputCheckBox(bp, rd, sOut, sOut_Local, oNode, oNodeScreen, bBkg)
                ElseIf oNode.ParamIn("pf4", "CRADSB") OrElse (oNode.Param("psl") = "G" AndAlso oNode.ParamVal("pxk") <> 0) _
                    OrElse (bp.getProperty("CityStreet") = "inline" AndAlso oNode.ParamIn("apr", "CS", 2) AndAlso oNodeScreen.Param("fgr") <> "P") Then 'ntg 10.07.24 vladi's change regarding city-street screen
                    BuildInputCombo(bp, rd, sOut, sOut_Local, sOutOut, oNode, oNodeScreen, bBkg)
                Else
                    BuildInputText(bp, rd, sOut, sOut_Local, oNode, oNodeScreen, bAdditionalField, bBkg)
                End If
                If oNode.Param("inp") = "R" Then
                    oControl.Add("readonly", "true") 'vk 07.04
                End If
            Case "O", "P", "Q"
                If oNode.Param("typ") <> "A" Then 'ntg 25.06.23 makes all numeric fields as ltr
                    dir = "ltr"
                End If

                If AddToTopAndExit(bp, oNode, oNodeScreen, oControl) Then
                    bNoControl = True
                ElseIf oNode.Param("inp") = "O" Then
                    BuildOutput(bp, rd, sOut, sOut_Local, oNode, oNodeScreen, bBkg)
                Else
                    BuildProtect(bp, rd, sOut, sOut_Local, oNode, oNodeScreen, bBkg)
                End If
        End Select

        If bNoControl Then Exit Sub

        If oNode.ParamExists("bua") OrElse oNode.ParamExists("fbua") Then
            oControl.Add("data-bs-toggle", "tooltip")
            oControl.Add("title", qColor.GetHelp_Simple(bp, oNode, oNodeScreen, If(oNode.ParamExists("bua"), "bua", "fbua")).Replace("\n", " ").Trim())
        End If

        If bNoControl Then Exit Sub 'vk 05.05 'boo? F->nothing

        ToolTip(bp, oNode, oNodeScreen, oControl) 'moved here vk 02.14

        If Not bAdditionalField Then
            If oNode.ParamExists("apr") Then 'vk 05.04, 05.05
                sOut_Local.Append(Hidden(oNode.Param("apr"), sN))
            End If
        End If
        If oNode.Param("inp") = "K" Then
            oControl.Clear("type")
            oControl.Add("type", "search") ' Add a small "x" to clear the text
            oControl.Add("oldvalue", oNode.Param("val").Trim)
            oControl.Add("onblur", "fnBlur_DeleteOnly(this);", asFirst:=True)
        End If
        If CurrentField(bp, oNode, oNodeScreen, bAdditionalField, sFind, sFil) Then
            bp.m_sNameFocus = oControl.Param("name")
            If bp.m_sSpecialCtl = "date" Then 'vk 15.07.24 changes to make focus appear in "date" type field
                bp.m_sNameFocus = "X" & bp.m_sNameFocus
            End If

            If oNode.Param("psl") = "G" Then
                bp.m_sClickLine = oNode.ParamVal("lin").ToString("000") 'vk 08.04
            End If
        End If
        oControl.Add("onfocus", "bDontSelect=false;")
        If oNode.ParamIn("pf4", "CRADSB") OrElse
            (bp.getProperty("CityStreet") = "inline" AndAlso oNode.ParamIn("apr", "CS", 2) AndAlso oNodeScreen.Param("fgr") <> "P") Then 'ntg 10.07.24 vladi's change regarding city-street screen
            sOut_Local.Append(oControl.FullCode(bp, "select", bRequired:=oNode.ParamYes("man")))
        ElseIf bp.m_sSpecialCtl = "date" Then
            'boo? date
            bp.m_sCurClass = "duet-date-picker"
            Dim sVal As String = oNode.Param("val") '"23112020"

            Dim d = "", m = "", y = ""
            Dim DateFormat As String = ""
            GetDateValues(sVal, oNode.Param("typ"), DateFormat, d, m, y)
            oControl.Clear("value")
            oControl.Add("value", y & "-" & m & "-" & d)
            oControl.Clear("id")
            oControl.Clear("name")
            oControl.AddId("X" & sN)
            If oNode.ParamExists("max") AndAlso oNode.ParamVal("max") < 100000 Then
                oControl.Add("max", DateTime.Today.AddMonths(oNode.ParamVal("max")).ToString("yyyy-MM-dd")) 'vk 08.24+++
            Else
                Dim sTo As String = bp.getProperty("YearsTo")
                Dim nTo As Integer = 20
                If sTo.Trim > "" Then nTo = Val(sTo)
                nTo += Now.Year
                oControl.Add("max", nTo.ToString("0000") & "-12-31")
            End If
            If oNode.ParamExists("min") AndAlso oNode.ParamVal("min") < 100000 Then
                oControl.Add("min", DateTime.Today.AddMonths(-oNode.ParamVal("min")).ToString("yyyy-MM-dd")) 'vk 08.24+++
            Else
                oControl.Add("min", "1900-01-01")
            End If
            oControl.Add("identifier", "date")
            oControl.Add("first-day-of-week", "0")
            If oNode.ParamIn("typ", "B") Then 'ntg 27.03.24 now to add the function updDateRange, its required to be typ=B
                oControl.Add("onchange", "CopyOut(this);updDateRange(this);") 'ntg 05.03.24 in the special date field- if a date was selected inside the date-picker, update all elements accordingly
            Else
                oControl.Add("onchange", "CopyOut(this);")
            End If
            oControl.Add("DFormat", DateFormat)
            oControl.AddTail("</duet-date-picker>")

            If oNode.ParamIn("typ", "B") Then
                Dim todayDate As Date = DateTime.Today
                Dim formattedDate As String = todayDate.ToString("yyyy-MM-dd")
                Dim formattedNextMonth As String
                Dim formattedNextMonthWithHyphens As String

                ' Check if today is the first day of the month
                If todayDate.Day = 1 Then
                    ' If today is the first day of the month, nextMonth will be the first day of the following month
                    Dim nextMonth As Date = todayDate.AddMonths(1)
                    nextMonth = New Date(nextMonth.Year, nextMonth.Month, 1)
                    formattedNextMonth = nextMonth.ToString("dd/MM/yyyy")
                    formattedNextMonthWithHyphens = nextMonth.ToString("yyyy-MM-dd") '("dd-MM-yyyy") 'vk 09.24
                Else
                    ' If today is not the first day of the month, nextMonth will be the first day of the next month
                    Dim daysUntilNextMonth As Integer = (DateTime.DaysInMonth(todayDate.Year, todayDate.Month) - todayDate.Day) + 1
                    Dim nextMonth As Date = todayDate.AddDays(daysUntilNextMonth)
                    nextMonth = New Date(nextMonth.Year, nextMonth.Month, 1)
                    formattedNextMonth = nextMonth.ToString("dd/MM/yyyy")
                    formattedNextMonthWithHyphens = nextMonth.ToString("yyyy-MM-dd")
                End If

                Dim nextMonthYear As Integer = Integer.Parse(formattedNextMonth.Split("/")(2)) ' Extract the year from formattedNextMonth
                Dim endOfYearDate As New Date(nextMonthYear, 12, 31)  ' Create an end of the year date
                Dim formattedEndOfYear As String = endOfYearDate.ToString("dd/MM/yyyy") ' Format the end of the year date

                sOut_Local.Append("<div class='specialBtnsDatePickerDiv'>")
                sOut_Local.Append("<div class='dateOptonsDiv'>")
                'vk 08.24+++ from here
                Dim sPer As String = ""
                If oNode.ParamYes("per") Then
                    sPer = $"fnBlur_Check(document.getElementById(""X{sN}""),""{oNode.Param("pfk")}"");"
                End If
                Dim sStyle1 As String = "", sStyle2 As String = "", sStyle3 As String = ""
                Select Case y & "-" & m & "-" & d
                    Case "0000-00-00"
                    Case formattedDate
                        sStyle1 = "dateSpecialButtonsSelected"
                    Case formattedNextMonthWithHyphens
                        sStyle2 = "dateSpecialButtonsSelected"
                    Case Else
                        sStyle3 = "dateSpecialButtonsSelected"
                End Select
                sOut_Local.Append($"<button type='button' class='dateSpecialButtons displayDateToday {sStyle1}' onclick='PutDate(""{sN}"",""{formattedDate}"");changeDateColor(event);{sPer}'>היום</button>")
                sOut_Local.Append($"<button type='button' class='dateSpecialButtons displayFirstofNextMonth {sStyle2}' onclick='PutDate(""{sN}"",""{formattedNextMonthWithHyphens}"");changeDateColor(event);{sPer}'>{formattedNextMonth}</button>")
                sOut_Local.Append($"<button type='button' class='dateSpecialButtons displayDateShow {sStyle3}' onclick='document.getElementById(""X{sN}"").show();changeDateColor(event);'>בחירה</button>")
                sOut_Local.Append("</div>")
                '''sOut_Local.Append($"<div id='displayDateRange'>{formattedNextMonth}-{formattedEndOfYear}</div>")
                sOut_Local.Append("</div>") 'end of specialBtnsDatePickerDiv
                'sOut_Local.Append("<div style=""display:none"" class=""datepicker-wrap"">").Append(oControl.FullCode(bp, "duet-date-picker", bRequired:=oNode.ParamYes("man"))).Append("</div>")
                sOut_Local.Append("<div class=""datepicker-wrap"" style=""width:0;height:0;visibility:hidden;"">").Append(oControl.FullCode(bp, "duet-date-picker", bRequired:=oNode.ParamYes("man"))).Append("</div>") 'vk 09.24
            Else
                sOut_Local.Append("<div class=""datepicker-wrap"">").Append(oControl.FullCode(bp, "duet-date-picker", bRequired:=oNode.ParamYes("man"))).Append("</div>")
            End If
            ' sOut_Local.Append(Hidden(sN, y & "-" & m & "-" & d)) 'vk 06.21
        Else
            sOut_Local.Append(oControl.FullCode(bp, bRequired:=oNode.ParamYes("man"), plc:=oNode.Param("plc"), bPer:=oNode.ParamYes("per")))
        End If
        'vk 08.24+++ till here
        If oNode.ParamYes("lvl") AndAlso Not bAdditionalField Then
            oControlT = New Control(qStyle.GetStyleLvl(bp, oNode, oNodeScreen, bBkg))
            oControlT.Add("value", sBigPoint)
            sOut_Local.Append(oControlT.FullCodeSpan(bp))

        End If
    End Sub

    Private Sub GetDateValues(pval As String, ptype As String, ByRef DateFormat As String, ByRef d As String, ByRef m As String, ByRef y As String)
        Select Case ptype
            Case "D" '  DDMMYY 
                d = Left(pval, 2)
                m = Mid(pval, 3, 2)
                y = "20" + Mid(pval, 5, 2)
                DateFormat = "DDMMYY"
            Case "Y", "B" ' DDMMYYYY   ntg 05.03.24 added "B"
                d = Left(pval, 2)
                m = Mid(pval, 3, 2)
                y = Mid(pval, 5)
                DateFormat = "DDMMYYYY"
            Case "H", "Q" 'MMYYYY   
                d = "01"
                m = Left(pval, 2)
                y = Mid(pval, 3)
                DateFormat = "MMYYYY"
        End Select
    End Sub

    Private Sub BuildInputCombo(ByRef bp As BuildPage, ByRef rd As Xml.XmlTextReader, ByRef sOut As StringBuilder, ByRef sOut_Local As StringBuilder,
            ByRef sOutOut As StringBuilder, ByRef oNode As Node, ByRef oNodeScreen As Node, ByVal bBkg As Boolean)


        If StartCombo(bp, rd, sOut_Local, oNode, oNodeScreen, False, bBkg) Then
            'If bp.getProperty("ComboStyle") = "ms" Then
            If Not bBkg Then 'vk 09.11
                oControl.AddId("S" + sN)
            End If
            If oNode.ParamIn("pf4", "DSB") Then 'vk 05.20 
                '
            End If


            'oControl.Add("class", "InpCom")
            If oNodeScreen.ParamVal("flr") = 1 Then
                oControl.Add("dir", "ltr")
            Else
                oControl.Add("dir", "rtl")
            End If
            oControl.Add("value", oNode.ValModified)

            If bp.getProperty("CityStreet") = "inline" AndAlso oNode.ParamIn("apr", "CS", 2) AndAlso oNodeScreen.Param("fgr") <> "P" Then 'ntg 10.07.24 vladi's change regarding city-street screen
                Dim t As String = oNode.Param("apr")
                Dim t2 As String = t
                If Mid(t, 2, 1) = "C" Then
                    Mid(t, 2, 1) = "S"
                    Mid(t2, 2, 1) = "T"
                    oControl.Add("onblur", "streetlist1(this,'" & t & "','" & t2 & "');")
                    oControl.Add("onchange", "streetlist1(this,'" & t & "','" & t2 & "');")
                    Mid(t, 2, 1) = "N"
                Else
                    Mid(t, 2, 1) = "T"
                End If
                oControl.Add("onblur", "CopyOut(this,'" & t & "');")
                oControl.Add("onchange", "CopyOut(this,'" & t & "');")
            Else 'ntg 10.07.24 vladi's change regarding city-street screen
                oControl.Add("onfocus", "fnFocusSelect(this);")
                oControl.Add("onblur", "CopyOut(this);")
            End If
            oControl.Add("onchange", "CopyOut(this);")
            oControl.Add("class", "ComboSelect2")


            If oNode.ParamYes("per") Then
                oControl.Add("onfocus", "fnFocus_Check(this);")
                oControl.Add("onblur", "if(bChange)fnBlur_Check(this,'" + oNode.Param("pfk") + "');")
                'vk 01.20
                oControl.Add("onclick", "if(bChange)fnBlur_Check(this,'" + oNode.Param("pfk") + "');")
                'oControl.Add("onchange", "bChange=true;")
                oControl.Add("onchange", "fnBlur_Check(this,'" + oNode.Param("pfk") + "');")
            End If

            If oNode.ParamYes("man") Then
                oControl.Add("oninput", "ValidateNonZero(this);")
            End If

            oControl.Add("oninput", "SetColorForCombo(this);")
            oControl.Add("oninvalid", "יש להזין ערך תקין") 'ntg 10.04.24 there was a browser automatic tooltip and this customizes the text of it-for combo
            oControl.Add("title", "יש להזין ערך") 'ntg 10.04.24 there was a browser automatic tooltip and this customizes the text of it-for combo

            qCombo.BuildOption_AsSelectOptions(bp, sXML_Combo, oNode, oNodeScreen, sOptionT)
            oControl.AddTail(sOptionT)

        End If

        'hidden input to pass value
        If oNode.Param("typ") = "N" Then
            sOut.Append(Hidden(sN, Right(oNode.Param("val").PadLeft(oNode.ParamVal("len"), "0"), oNode.ParamVal("len"))))
        Else
            sOut.Append(Hidden(sN, oNode.Param("val")))
        End If

    End Sub

    Private Sub CreateHiddenField(ByRef oNode As Node, ByRef sOut As StringBuilder, ByRef sNameShift As String)
        If oNode.Param("inp") = "R" Then
            sOut.Append(Hidden(sN, oNode.Param("val"), "ro", "true"))
        Else
            sOut.Append(Hidden(sN, oNode.Param("val")))
        End If

        oControl.AddId("Y" + sN)
        sNameShift = "1"

        oControl.Add("onblur", "CopyOut(this);")
    End Sub

    Private Sub BuildInputText(ByRef bp As BuildPage, ByRef rd As Xml.XmlTextReader, ByRef sOut As StringBuilder,
                               ByRef sOut_Local As StringBuilder, ByRef oNode As Node, ByRef oNodeScreen As Node, ByVal bAdditionalField As Boolean, ByVal bBkg As Boolean)

        Dim sNameShift As String = "1"
        If oNode.Param("tch") = "A" OrElse bp.getProperty("DontSelectOnFocus") = "true" Then 'ntg 17.06.24 added OrElse bp.getProperty("DontSelectOnFocus") = "true"  - regarding field focus when changing tabs
            oControl.Add("onfocus", "bDontSelect=true;")
        End If

        If bAdditionalField Then
            If Not bBkg Then
                oControl.AddId("C" + sN)
            End If
            If oNode.Param("inp") = "R" Then
                oControl.Add("ro", "true") 'vk 07.04
            End If
            oControl.Add("onfocus", "fnFocus_Check(this);")
            oControl.Add("onblur", "fnBlur_Check(this,'$copy');")
        ElseIf oNode.ParamVal("tch") = 1 Then

            sOut.Append(Hidden(sN, oNode.Param("val")))
            If Not bBkg Then 'vk 09.11
                oControl.AddId("S" + sN)
            End If
            oControl.Add("readonly", "true")
        ElseIf oNode.ParamVal("lin") = 0 Then
            CreateHiddenField(oNode, sOut, sNameShift)
        Else
            If bp.m_sSpecialCtl = "date" Then
                CreateHiddenField(oNode, sOut, sNameShift)
            End If
            ' נטרלתי את הקוד הנל על מנת למנוע מצב של יצירת "אידן" במקרה של תיבת טקסט עם סימון של שלוש נקודות
            'If oNode.ParamIn("typ", "NSA") AndAlso oNode.ParamYes("pf4") Then
            '    'check for SideButton
            '    CreateHiddenField(oNode, sOut, sNameShift)
            'End If

            oControl.AddId(sN)
            Select Case oNode.Param("tch") 'vk 09.18
                Case "F" : sOut.Append(Hidden("HarchiveMail", sN)) 'vk 10.17
                Case "G" : sOut.Append(Hidden("HarchiveWhat", bp.m_sArchiveWhat)) 'vk 03.20
                Case "K" : sOut.Append(Hidden("HarchiveText1", sN))
                Case "L" : sOut.Append(Hidden("HarchiveText2", sN))
                Case "M" : sOut.Append(Hidden("HarchiveText3", sN))
                Case "N" : sOut.Append(Hidden("HarchiveText4", sN))
                Case "O" : sOut.Append(Hidden("HarchiveText5", sN))
            End Select

            If oNode.Param("inp") = "R" Then
                oControl.Add("ro", "true") 'vk 07.04
            End If
            sNameShift = "0"
        End If

        oControl.Add("value", oNode.Param("val"))
        oControl.Add("onfocus", sfnClearOptions)

        If oNode.ParamYes("eml") Then
            oControl.Add("oninput", "document.getElementById('A_" + oNode.Param("num").Trim() + "').href='mai" + "lto:' + this.value;")
            oControl.Add("placeholder", "Enter email")
        Else
            If oNode.Param("placeholder") IsNot Nothing Then
                oControl.Add("placeholder", oNode.Param("placeholder"))
            Else
                oControl.Add("placeholder", " ")
            End If
        End If

        If oNode.Param("typ") = "A" Then
            oControl.Add("dir", dir)
        ElseIf bp.m_sSpecialCtl = "date" Then ' Date element
            oControl.Add("dir", "rtl")
        Else
            oControl.Add("dir", "ltr") 'vk 05.05
        End If

        Select Case oNode.Param("typ")
            Case "D", "Y", "H", "Q", "T", "U" 'Q vk 07.05
                sNameShift = "1"
                oControl.Add("maxLength", iLen.ToString)
                'sNameShift = "1" 'vk 06.21
                oControl.Add("onfocus", "fnFocus(this," + sNameShift + ");")
            Case "N", "S", "A"
                If oNode.Param("tch") = "3" Then
                    bp.m_sNameFocus = oControl.Param("name")
                    bp.m_sCurClass += " px-2 "
                    bp.m_sSideButton = "<i class='far fa-credit-card' style='color:DarkSlateBlue'></i>"
                End If
                If oNode.ParamYes("pf4") Then
                    If oNodeScreen.Param("adr") = "R" AndAlso oNode.ParamIn("apr", "CNST", 2) AndAlso oNodeScreen.Param("fgr") <> "P" Then
                        'bp.m_sSideButton = SideButton(sN)
                        bp.m_sSideButton = SideButton(sN, "fnpf4_dblclick_adr('" + Left(oNode.Param("apr"), 1) + "','" + sN + "');") 'ntg 05.07.23 changes by vladi- for cities and streets screen
                    Else
                        If oNode.ParamYes("eml") Then
                            bp.m_sSideButton = EmailSideButton(bp, oNode.Param("val"), oNode.Param("num").Trim(), bBkg)
                        Else
                            'bp.m_sSideButton = SideButton(sN)
                            bp.m_sSideButton = SideButton(sN, "fnpf4_dblclick();")
                        End If
                    End If
                ElseIf (oNode.Param("tch") = "3" AndAlso oNode.Param("val").Trim > "") OrElse
                        (bp.getProperty("WsApi").Trim > "" AndAlso oNodeScreen.Param("fgr") <> "P" AndAlso (oNode.Param("tch") = "3" OrElse oNode.Param("apr") = "8V")) Then
                    oControl.Add("ReadOnly", "True")
                    oControl.Unsel()
                Else
                    oControl.Add("onfocus", "fnFocus(this," + sNameShift + ");")
                End If
        End Select
        If oNode.ParamYes("per") Then
            oControl.Add("onfocus", "fnFocus_Check(this);")
        End If
        Select Case oNode.Param("typ")
            Case "D", "Y", "H", "Q"
                oControl.Add("onfocus", "fnFocusDate(this);")
            Case "T", "U"
                oControl.Add("data-inputmask-alias", "datetime")
                oControl.Add("data-inputmask-inputformat", $"HH:MM{If(oNode.Param("typ") = "T", ":ss", "")}")
                oControl.Add("data-inputmask-placeholder", $"hh:mm{If(oNode.Param("typ") = "T", ":ss", "")}")
            Case "N", "S"
                Dim FieldLen = iLen + iAdd
                oControl.Add("maxLength", FieldLen.ToString)
                oControl.Add("onbeforedeactivate", "fnBlurNum(this, " + oNode.ParamVal("len").ToString + ", " + oNode.ParamVal("dec").ToString + ",'" + oNode.Param("edt") + "');")
                oControl.Add("onfocus", "fnFocusNum(this);")
                If oControl.Param("type") <> "password" Then 'ntg 21.06.23 password wasnt shown as ***
                    oControl.Clear("type")
                    'oControl.Add("type", "number")
                    oControl.Add("type", "text") 'was changed from "number" to "text" because it didnt enable writing a dot in the text box - ntg 22.02.23
                End If

                oControl.Add("step", "any")

                If oNode.Param("edt") = "J" Then
                    'oControl.Add("data-inputmask", "'alias': 'decimal', 'groupSeparator': ','")
                    oControl.Add("data-inputmask", "'alias': 'numeric', 'autoGroup': ',', 'allowMinus': 'false'")
                    'oControl.Clear("type")
                    'oControl.Add("type", "text") 'ntg 21.06.23 password wasnt shown as ***
                Else
                    oControl.Add("oninput", "this.value=this.value.slice(0,this.maxLength)")
                End If

                If oNode.ParamYes("man") Then
                    oControl.Add("oninput", "ValidateNonZero(this);")
                    oControl.Add("title", "יש להזין ערך")  'ntg 10.04.24 there was a browser automatic tooltip and this customizes the text of it-for regular typing field
                End If
            Case "A"
                oControl.Add("maxLength", oNode.Param("len"))
                Const EV As String = "onkeyup"
                If oNode.Param("inp") <> "R" Then
                    If oNode.Param("tch") = "3" Then
                        oControl.Add("onkeydown", "IgnoreKey();") 'vk 07.13
                    ElseIf oNode.Param("tch") = "F" OrElse oNode.Param("tch") = "G" Then 'vc 10.17, 03.20
                    ElseIf oNodeScreen.ParamVal("ver") >= 1 AndAlso oNode.Param("plc") = " " Then
                        oControl.Add(EV, "MakeUpper(this);") 'vk 03.20
                        oControl.Add("onpaste", "MakeUpper_Paste(this);") 'vk+ib 04.21
                    ElseIf oNode.Param("plc") = "H" Then
                        oControl.Add(EV, "MakeUpper(this);") 'vk 03.20
                        oControl.Add("onpaste", "MakeUpper_Paste(this);") 'vk+ib 04.21
                        oControl.Add("onkeypress", "BlockNonAlphaNum();")
                        oControl.Add("pattern", "[A-Za-z0-9]+")
                    End If
                End If
                oControl.Add("onfocus", "mySelect(this);")
        End Select

        If oNode.Param("tch") = "3" Then 'vk 04.20 Then
            If bp.getProperty("WsApi") = "ESB" Then
                'oControl.Add("onfocus", "if (this.value=='') CG_ESB();") 'vk 04.21
            ElseIf bp.getProperty("WsApi").Trim = "" Then
                oControl.Add("onfocus", "if (this.value=='') CG1();") 'vk 07.13
            End If
            'vk 02.21+06.22 universal window by Mark
            If oNodeScreen.Param("fil").Trim = bp.CgWindow Then
                oControl.Add("cod", "02")
            End If
        End If

        If oNode.ParamYes("per") Then
            oControl.Add("onkeydown", "fnKey_Check(this);")
            oControl.Add("onblur", "fnBlur_Check(this,'" + oNode.Param("pfk") + "');")
            oControl.Add("cod", oNode.Param("pfk")) 'vk 07.13
        End If
    End Sub

    'Private Function SideButton(cntid As String, fn As String) As String 'ntg 05.07.23 added , fn As String & ;{fn}  'ntg 09.07.23 added addHeaderText(); that changes the text of the header

    '    Dim sRtn As String = $"<button class='btn btn-sm btn-primary btn-dots' type='button' onclick=""fnFocusBoo4('{cntid}', 1);{fn}DisableFooterBtns();checkAndToggleSelectDisplay();addHeaderText();"">
    '                                  <i class='fa fa-ellipsis-v'></i> 
    '                           </button>"
    '    Return sRtn
    'End Function
    Friend Function SideButton(cntid As String, fn As String, Optional sStyle As String = "") As String  'ntg 10.04.24 changes for special header split with '|'
        Dim sRtn As String = $"<button style='{sStyle}' class='btn btn-sm btn-primary btn-dots' type='button' onclick=""fnFocusBoo4('{cntid}', 1);{fn}DisableFooterBtns();checkAndToggleSelectDisplay();addHeaderText();"">
                                      <i class='fa fa-ellipsis-v'></i> 
                               </button>"
        Return sRtn
    End Function


    Private Function EmailSideButton(ByRef bp As BuildPage, sVal As String, sNum As String, bBkg As Boolean) As String
        Dim oControl As New Control
        If Not bBkg Then
            oControl.AddId($"A_{sNum}")
        End If
        'oControl.Add("href", $"mailto:{sVal}")
        'oControl.Add("value", "@")

        oControl.Add("target", "_blank")
        oControl.Add("class", "input-group-text")
        Dim SideClass = bp.m_sCurClass
        bp.m_sCurClass = "input-group-text no-text-dec " 'ntg 20.05.24 adding an icom for a mail field

        Dim ControlText As String = oControl.FullCodeSpan(bp, sTag:="a", sMailIcon:="<i Class='fa-regular fa-envelope'></i>") 'ntg 20.05.24 adding an icom for a mail field

        bp.m_sCurClass = SideClass
        Return ControlText
    End Function

    Private Sub BuildInputCheckBox(ByRef bp As BuildPage, ByRef rd As Xml.XmlTextReader, ByRef sOut As StringBuilder,
        ByRef sOut_Local As StringBuilder, ByRef oNode As Node, ByRef oNodeScreen As Node, ByVal bBkg As Boolean)

        Dim bRadioExists As Boolean = False 'vk 03.05
        Dim o As Integer 'Node 'vk 03.05
        If bp.cRadios IsNot Nothing Then 'vk 09.20
            For Each o In bp.cRadios
                If o = oNode.ParamVal("lin") Then bRadioExists = True
            Next
        End If

        If Not bBkg Then 'vk 09.11
            oControl.AddId("X" + sN)
        End If

        If oNode.ParamExists("val") Then
            oControl.Add("checked", "true")
            If Not bRadioExists Then bp.cLinChecked.Add(oNode.ParamVal("lin"))
        End If

        oControl.Add("onfocus", "fnFocusCombo(this,true);")

        If bRadioExists Then
            'oControl.Add("onclick", "CheckBoxClick(this,false);")
        Else
            If Not oNodeScreen.ParamIn("fgr", "LPD") Then 'vk 05.21, 09.24
                oControl.Add("ondblclick", "CheckBoxDblClick(this);")
            End If
        End If
        oControl.Add("mark", oNode.Param("chb")) 'vk 08.04

        sOut.Append(Hidden(sN, oNode.Param("val")))
        If oNodeScreen.ParamIn("fgr", "LPBD") Then 'ntg 23.06.24 fgr=B is the same as fgr=L, but with option to select multiple rows in a table Then
            'vk 09.24 fgr=D
            bp.sListOfChk += sN + ";" 'vk 05.21
        End If

    End Sub

    Private Sub BuildInputRadio(ByRef bp As BuildPage, ByRef rd As Xml.XmlTextReader,
        ByRef sOut As StringBuilder,
        ByRef sOut_Local As StringBuilder,
        ByRef oNode As Node, ByRef oNodeScreen As Node, ByVal bBkg As Boolean)

        If Not bBkg Then
            oControl.AddId("R")
        End If
        oControl.Add("value", sN + "_" + oNodeScreen.Param("fch"))
        oControl.Add("maxLength", oNode.Param("len"))
        If oNode.ParamExists("val") AndAlso bp.cLinChecked.Count = 0 Then '=0 ms+vk 06.05
            oControl.Add("checked", "true")
            bp.cLinChecked.Add(oNode.ParamVal("lin"))
        End If

        oControl.Add("onfocus", "fnFocusSL(this,'" + oNode.Param("lin").PadLeft(3, "0") + "');")

        If oNode.ParamExists("dfs") Then
            oControl.Add("pch", oNode.Param("dfs"))
        End If
        oControl.Add("ondblclick", "fnpsl_dblclick" +
            IIf(oNodeScreen.ParamIn("fgr", "LAPD"), "_folders(this);", "();")) 'vk 09.24 fgr=D
        sOut.Append(Hidden("Z" + sN + "_" + oNodeScreen.Param("fch")))

        ' Add Action Icons
        If bp.ActionNodes.Count() > 0 Then
            oControl.SetStyle("style='display:none;'")
            Dim allActionsHaveIcons As Boolean = bp IsNot Nothing AndAlso bp.ActionNodesIcons.All(Function(kvp) kvp.Value IsNot Nothing) 'ntg 20.05.24 

            Dim isFgrP As Boolean = oNodeScreen.ParamExists("fgr") AndAlso oNodeScreen.ParamIn("fgr", "LBD") AndAlso bp.ActionNodesIcons.Count < 4 AndAlso allActionsHaveIcons 'ntg 20.05.24 changes in appearence regarding screens with fgr=P-showing button instead of list of actions -- 'ntg 23.06.24 fgr=B is the same as fgr=L, but with option to select multiple rows in a table
            'vk 09.24 fgr=D
            If Not isFgrP Then 'ntg 20.05.24 changes in appearence regarding screens with fgr=P-showing button instead of list of actions
                sOut.AppendLine($"<button id='LinksPopOverButton' type='button' onclick = 'CheckTheRow(this)' 
                                   class='LinksPopOver px-0 btn btn-circle btn-sm btnOpenActionButtons' title=''>
                                   <i class='mx-0 fas fa-ellipsis-h'></i>
                                   </button>")
            End If

            'sOut.AppendLine($"<button id='LinksPopOverButton' type='button' onclick = 'CheckTheRow(this)' 
            '                   class='LinksPopOver footerButton px-1 btn btn-sm' title=''>
            '                   פעולות נוספות
            '                   </button>")

            ' Create a popover window with all the relevant buttons
            Dim ActionButtonsString As String = ""

            For Each ActionNodeStr As String In bp.ActionNodes
                'Dim ascombo As String = ActionNodeStr.Replace("button", "option")

                ActionNodeStr = ActionNodeStr.Replace("button", "a") ' PopOver works with a (hyperlink) and not with buttons

                'ntg 29.05.24 code section that adds the  "CheckTheRow(this);" function to a row in the table in which an action was selected (table with icons)
                Dim newFunctionCall As String = "CheckTheRow(this);"
                Dim onclickPosition As Integer = ActionNodeStr.IndexOf("onclick=""")
                If onclickPosition <> -1 Then
                    Dim insertPosition As Integer = onclickPosition + 9
                    ActionNodeStr = ActionNodeStr.Insert(insertPosition, newFunctionCall)
                End If

                Dim ActionNodeElement As XElement = XDocument.Parse(ActionNodeStr).Root

                ActionNodeElement.SetAttributeValue("class", $"{If(isFgrP, "fgrP", "px-1 btn list-group-item popOverItem LinksPopOverItem")} {If(bp.RetIsEnglish, "leftAlign", "")}") 'ntg 06.02.23 'ntg 20.05.24 changes in appearence regarding screens with fgr=P-showing button instead of list of actions
                'ActionNodeElement.SetAttributeValue("onclick", $"{If(isFgrP, "selectTr();", "")}") 'ntg 06.02.23 'ntg 20.05.24 changes in appearence regarding screens with fgr=P-showing button instead of list of actions
                ActionButtonsString += ActionNodeElement.ToString()
            Next

            If FirstTimeActionButtonsMenu Then
                If Not isFgrP Then 'ntg 20.05.24 changes in appearence regarding screens with fgr=P-showing button instead of list of actions
                    sOut.AppendLine($"<ul id='popover-content' class='d-none list-group'>
                                 {ActionButtonsString}
                                </ul>")
                    FirstTimeActionButtonsMenu = False

                Else
                    sOut.AppendLine($"<div class='fgrPactionsWrapper'>{ActionButtonsString}</div>") 'ntg 20.05.24 changes in appearence regarding screens with fgr=P-showing button instead of list of actions
                End If

            End If
        End If
        GridLineNo = GridLineNo + 1

    End Sub

    Private Sub BuildProtect(ByRef bp As BuildPage, ByRef rd As Xml.XmlTextReader,
        ByRef sOut As StringBuilder,
        ByRef sOut_Local As StringBuilder,
        ByRef oNode As Node, ByRef oNodeScreen As Node, ByVal bBkg As Boolean)

        Dim bMsCombo As Boolean = False
        If Not (bBkg OrElse oNode.Param("num") = "placeholder") Then 'vk 09.11, 09.24
            sOut.Append(Hidden(sN, oNode.Param("val"))) 'vk 09.08 'must be before oNode.Param("val") is changed
        End If
        'If oNode.ParamIn("pf4", "CRADSB") Then
        If oNode.ParamIn("pf4", "CRADSB") OrElse (bp.getProperty("CityStreet") = "inline" AndAlso oNode.ParamIn("apr", "CS", 2)) Then 'vk 05.08.24 for cities and streets screen
            If StartCombo(bp, rd, sOut_Local, oNode, oNodeScreen, True, bBkg) Then 'vk 10.06

                oNode.Param("val") = qCombo.TextOfCombo_ByXml(bp, sXML_Combo, oNode, oNodeScreen)
                oControl.Add("disabled", "true")
                oControl.AddTail("<option selected>" + oNode.Param("val") + "</option></select>")
                bMsCombo = True
            End If
        End If

        If bNoControl Then Exit Sub
        If oNodeScreen.ParamYes("msg") AndAlso oNode.IsErrMsg(oNodeScreen) Then
        ElseIf oNode.ParamIn("pf4", "CRADSB") Then
        ElseIf oNode.Param("inp") = "Q" Then
        ElseIf oNode.ParamExists("chb") OrElse oNode.Param("psl") = "G" Then
            oControl.Add("disabled", "true")
        Else
            'oControl.Add("class", "InpProt")
        End If
        oControl.Add("value", oNode.Param("val"))
        oControl.Add("readonly", "true")
        oControl.Add("placeholder", " ")
        If oNode.ParamYes("pf4") Then
            If Not bBkg Then
                oControl.AddId("P" + sN)
            End If
        Else
            oControl.Unsel()
            If Not bBkg Then 'vk 09.11
                oControl.AddId("S" + sN) 'vk 02.09
            End If
        End If
        If bMsCombo Then
            If oNodeScreen.ParamVal("flr") = 1 Then
                oControl.Add("dir", "ltr")
            Else
                oControl.Add("dir", "rtl")
            End If
        Else
            oControl.Add("dir", dir)
        End If
        oControl.Add("tabindex", "-1")
    End Sub

    Private Sub BuildOutput(ByRef bp As BuildPage, ByRef rd As Xml.XmlTextReader,
        ByRef sOut As StringBuilder,
        ByRef sOut_Local As StringBuilder,
        ByRef oNode As Node, ByRef oNodeScreen As Node, ByVal bBkg As Boolean)

        Dim BuaText As String = ""

        If nPosOfLogo > 0 Then
            'vk 03.04
            Dim oStyle As New Style()
            oStyle.Add("position", "absolute")
            oStyle.Add("left", qStyle.icol_fun(bp, oNode, oNodeScreen) + (nPosOfLogo - 1) * bp.iScaleWidth)
            oStyle.Add("top", qStyle.ilin_fun(bp, oNode, oNodeScreen))
            oStyle.Add("z-index", "2")

            sOut_Local.Append(Logo(bp, oStyle, bBkg, False))
        End If
        'vk 09.24
        bp.m_sCurStyle = ""
        If bp.m_bStatementWindow Then
            bp.m_sCurStyle &= "border:none;"
        End If

        oControlT = New Control(sStyle)
        If oNode.Param("forcename") > "" Then
            oControlT.AddId(oNode.Param("forcename")) 'vk 11.15
        End If

        oControlT.Add("value", oNode.Param("val"))

        If oNode.ParamVal("lin") = 0 OrElse oNodeScreen.Param("wait") = "S" Then
            oControlT.Add("dir", IIf(oNodeScreen.ParamVal("flr") = 1, "ltr", "rtl"))
        Else
            'If oNode.Param("sgn") = "-" Then 'ntg 11.06.23 numbers with - sign were shown rtl instead of ltr
            '    oControlT.Add("dir", "ltr")
            'Else
            oControlT.Add("dir", dir)
            'End If
        End If

        oControlT.Unsel()

        Dim HelpText As String = ""
        If oNode.ParamExists("hky") Then
            HelpText = qColor.GetHelp(bp, oNode, oNodeScreen, "2")
        End If

        Dim toolTipType As Integer = 0
        If oNode.ParamExists("tip") Then
            toolTipType = oNode.Param("tip")
        ElseIf HelpText <> "" Then
            toolTipType = 1
        End If

        If oNode.ParamExists("numv") OrElse oNode.ParamExists("numw") Then
            Dim PencilText = If(oNode.ParamExists("numv"), oNode.Param("numv"), oNode.Param("numw"))
            oControlT.AddPencil(PencilText, oNode.ParamExists("numv"))
            oControlT.SetInd(oNode.Param("ind"))
        End If


        If oNode.ParamExists("lnk") Then ' lnk = "Hebrew letter of the action"
            oControlT.Add("pch", oNode.Param("lnk"))
            oControlT.Add("onclick", "CheckTheRow(this);CmdPchClick(this);")
            oControlT.Add("href", "#")
        End If

        If oNode.ParamExists("bua") OrElse oNode.ParamExists("fbua") Then
            Dim oConv As Comtec.TIS.ConvCom = New Comtec.TIS.ConvCom()
            BuaText = oConv.RevHeb(If(oNode.ParamExists("bua"), oNode.Param("bua"), oNode.Param("fbua")), "").Replace("\n", " ").Trim()
        End If

        Select Case oNode.Param("pul") 'ntg 19.06.24 changes for sub-title and accordion inside cards
            Case "S"
                Dim iconName As String = "fa-sharp fa-solid fa-bullhorn fa-flip-horizontal subTitleIcon"
                If oNode.ParamExists("pic") Then
                    Dim picNumber = oNode.Param("pic")
                    iconName = bp.m_colColor.Item($"Icon_{CInt(picNumber)}")
                End If
                sOut_Local.Append($"<button class=""subCardS"" disabled> <i class='{iconName}'></i>{oNode.Param("val")}</button>")
            Case "A" 'vk 09.24 change
                sOut_Local.Append($"<button class=""subCardA"" id=""Accord_{oNode.Param("num")}"" onclick=""toggleCardAccordion(this);"" type='button'> <i class='fa-regular fa-square-plus subTitleIcon'></i>{oNode.Param("val")}</button>")
            Case Else
                sOut_Local.Append(oControlT.FullCodeSpan(bp, sTag:=If(oNode.ParamExists("lnk"), "a", ""), bRequired:=oNode.ParamYes("lvl"), pHelpText:=HelpText, pBua:=BuaText, toolTipType:=toolTipType))
        End Select

        'sOut_Local.Append(oControlT.FullCodeSpan(bp, sTag:=If(oNode.ParamExists("lnk"), "a", ""), bRequired:=oNode.ParamYes("lvl"), pHelpText:=HelpText, pBua:=BuaText))
        If Not oControl Is Nothing Then oControl.Dispose()
        oControl = Nothing
        bNoControl = True
    End Sub

    Sub BuildCmd(ByRef bp As BuildPage, ByRef sOut As StringBuilder, ByRef sOutH As StringBuilder, ByVal sKind As String, ByRef oNode As Node,
            ByRef oNodeScreenI As Node, ByRef oNodeScreenO As Node, ByVal bDisabled As Boolean, ByVal bIgnoreLeft As Boolean, ByVal bBkg As Boolean)

        'Dim icol, ilen, idec As Integer
        Dim sN As String = ""
        Dim sPlace As String = ""
        Dim sCode As String = ""
        Dim nNumber As Integer
        Dim bCaptionAsIs As Boolean = False 'vk 07.13
        Dim bIdAlways As Boolean = False 'vk 07.13

        With oNode
            Dim v As String
            For Each v In AllParams_Cmd
                .AddIfNo(v)
            Next
            For Each v In AllParams_Cmd0
                .AddIfNo(v, "0")
            Next
            .AddIfNo("len", "1")
            .AddIfNo("tab", "-1")
            .AddIfNo("wd", .Param("len"))
            'vk 06.04
            .AddIfNo("cgr", "00")
            'vk 09.06
            .AddIfNo("dec", bp.decD)
            .AddIfNo("ind", bp.indD)
            nNumber = Int32.Parse(Left(.Param("cgr"), 1))
            Select Case sKind
                Case "pch" 'Ilia
                    sCode = .Param("pch")
                    sN = "C_PCH" + sCode
                    sPlace = "folder"
                Case "pchl"
                    sCode = .Param("pchl")
                    sN = "F" + .LongName + oNodeScreenI.Param("fdate") '"C_PCHL" + sCode 'vk 09.06
                    sPlace = "field"
                Case "l" 'folder
                    If .Param("ln") IsNot Nothing Then
                        .Param("val") = .Param("ln")
                    End If

                    sCode = .Param("lk")
                    sN = "C_L" + sCode
                    sPlace = "folder"
                Case "c"
                    .Param("val") = If(bp.getProperty("ShowFCommand") = "true", .Param("ft"), CutFCommand(.Param("ft")))
                    sCode = .Param("fk")
                    sN = "C" + sCode
                    sPlace = "left"
                'bp.sListOfButtons += ";" + sCode
                Case "print" 'vk 01.04
                    .Param("val") = ""
                    sCode = ""
                    sN = "C_Print"
                    sPlace = "left"
                Case "printscreen" 'vk 05.05
                    .Param("val") = "Print Screen"
                    sCode = ""
                    sN = "C_PrintScreen"
                    sPlace = "left"
                Case "printpages" 'vk 05.06
                    .Param("val") = ""
                    sCode = ""
                    sN = "C_PrintPages"
                    sPlace = "left"
                Case "printpdf" ', "printtif", "printtifpdf" 'vk 03.09, 12.10, 02.13, 02.14
                    .Param("val") = ""
                    sCode = ""
                    sN = "C_PrintPdf"
                    sPlace = "left"
                Case "help" 'vk 07.04
                    .Param("val") = ""
                    sCode = "??"
                    sN = "C_Help"
                    sPlace = "left"
                Case "demo" 'vk 08.04
                    .Param("val") = "CM"
                    sCode = ""
                    sN = "C_Demo"
                    sPlace = "left"
                Case "archive_browse" 'vk 06.13
                    .Param("val") = bp.getProperty("ButtonBrowse")
                    bCaptionAsIs = True
                    bIdAlways = True
                    sCode = ""
                    sN = "C_ArchiveBrowse"
                    sPlace = "left"
                Case "archive_submit" 'vk 06.13
                    .Param("val") = bp.getProperty("ButtonSubmit")
                    bCaptionAsIs = True
                    bIdAlways = True
                    sCode = ""
                    sN = "C_ArchiveSubmit"
                    sPlace = "left"
                Case "testpage" 'vk 12.13
                    .Param("val") = bp.getProperty("ButtonTestPage")
                    bCaptionAsIs = True
                    sCode = ""
                    sN = "C_TestPage"
                    sPlace = "left"
                Case "jacket" 'vk 12.17
                    .Param("val") = bp.getProperty("ButtonJacket")
                    bCaptionAsIs = True
                    sCode = ""
                    sN = "C_Jacket"
                    sPlace = "left"
            End Select

            If Not bCaptionAsIs Then
                .Param("val") = qConv.ConvAlf_Simple(bp, .Param("val"), oNodeScreenI, "") 'vk 12.09
            End If
        End With

        'vk 01.04, 07.05
        Dim sCaption As String = oNode.Param("val").Trim
        Dim s As String = ""
        Dim sDir As String = "", sKey As String = "", sEq As String = ""
        Select Case True
            Case sCaption Like "*= F##" : sKey = Right(sCaption, 3) : sEq = " = " : sCaption = Left(sCaption, Len(sCaption) - 5)
            Case sCaption Like "*- F##" : sKey = Right(sCaption, 3) : sEq = " - " : sCaption = Left(sCaption, Len(sCaption) - 5)
            Case sCaption Like "*=  F#" : sKey = Right(sCaption, 2) : sEq = " = " : sCaption = Left(sCaption, Len(sCaption) - 5)
            Case sCaption Like "*-  F#" : sKey = Right(sCaption, 2) : sEq = " - " : sCaption = Left(sCaption, Len(sCaption) - 5)
            Case sCaption Like "F## =*" : sKey = Left(sCaption, 3) : sEq = " = " : sCaption = Mid(sCaption, 6)
            Case sCaption Like "F## -*" : sKey = Left(sCaption, 3) : sEq = " - " : sCaption = Mid(sCaption, 6)
            Case sCaption Like "F#  =*" : sKey = Left(sCaption, 2) : sEq = " = " : sCaption = Mid(sCaption, 6)
            Case sCaption Like "F#  -*" : sKey = Left(sCaption, 2) : sEq = " - " : sCaption = Mid(sCaption, 6)
        End Select
        Select Case True '< and > vk 12.09
            Case sCaption Like "<*" : sDir = "left" : s = Trim(Mid(sCaption, 2))
            Case sCaption Like "*>" : sDir = "right" : s = Trim(Left(sCaption, Len(sCaption) - 1))
        End Select
        If s <> "".PadRight(Len(s), "-") AndAlso s <> "".PadRight(Len(s), "=") Then sDir = ""

        If Not bp.getProperty("CutFLettersFromButton").ToLower() = "true" Then
            If sDir = "" Then
                sCaption = oNode.Param("val").Trim
            Else
                If oNodeScreenI.ParamVal("flr") = 0 Then
                    Select Case sDir
                        Case "left" : sCaption = ChrW(9658) + sEq + sKey
                        Case "right" : sCaption = ChrW(9668) + sEq + sKey
                    End Select
                Else
                    Select Case sDir
                        Case "left" : sCaption = sKey + sEq + ChrW(9668)
                        Case "right" : sCaption = sKey + sEq + ChrW(9658)
                    End Select
                End If
            End If
        End If


        Dim sUpDown As String = oNode.Param("val").ToLower.Replace(" ", "") 'vk 08.08
        'vk 04.07
        If oNodeScreenI.ParamYes("arw") Then 'OrElse bp.OrganizedButtons(True) Then 'vk 05.07
            Select Case sUpDown 'vk 08.08
                Case "pagedown" : sCaption = ChrW(9660)
                Case "pageup" : sCaption = ChrW(9650)
            End Select
            Select Case oNode.ParamVal("fk")
                Case 90 : sCaption = ChrW(9660)
                Case 91 : sCaption = ChrW(9650)
            End Select
        End If

        Dim sSpecial As String = ""

        Dim sStyle As String = qStyle.GetStyleBtn(bp, oNode, oNodeScreenO, sPlace, sSpecial, bDisabled)
        Dim oControl As New Control(sStyle)
        Dim sPrompt As String = "false"
        If bIdAlways OrElse Not bDisabled Then 'vk 05.07, 07.13
            If Not bBkg Then 'vk 09.11
                oControl.AddId(sN)
            End If
        End If
        If sPlace <> "scroll" Then 'vk 07.07
            oControl.Add("type", "button")
            oControl.Add("value", sCaption)
            If oNode.ParamExists("fbua") Then
                sPrompt = "true"
                oControl.Add("helptext", qColor.GetHelp_Simple(bp, oNode, oNodeScreenI, "fbua"))
            Else
                sPrompt = IIf(
                    (sPlace <> "field" AndAlso oNode.Param("val").Trim.Length > 16) OrElse
                    (sPlace = "field" AndAlso oNode.Param("val").Trim.Length > oNode.ParamVal("wd")),
                     "true", "false") 'vk 06.04
            End If
        End If
        oControl.Add("pch", sCode)

        'ntg 23.05.23
        If sKind = "pchl" Then
            oControl.Add("ondblclick", "if(NotReady('dblclick'))return;CmdPchlClick(this);")
        End If

        oControl.Add("spec", sSpecial) 'vk 05.07
        Select Case sPlace
            Case "folder"
                oControl.Add("onmouseenter", "fnBtnEnter_Folder(this," + sPrompt + ");")
            Case "left"
                oControl.Add("onmouseenter", "fnBtnEnter(this," + sPrompt + ");")
            Case "field"
                oControl.Add("onmouseenter", "fnBtnEnter_Field(this," + sPrompt + ");")
        End Select
        If sPlace <> "scroll" Then
            oControl.Add("onfocus", "fnBtnFocus(this);") 'vk 01.04
            oControl.Add("onblur", "fnBtnBlur(this);")
        End If
        If Not oNode.ParamIn("cnf", "0N") Then 'vk 09.05
            sOutH.Append(Hidden("Hcnf_" + sN, oNode.Param("cnf")))
        End If
        If qSecurity.IsForbidden(bp, oNodeScreenI, sCode) Then
            oControl.Add("onclick", "MyMsgBox(msg(7));") 'vk 06.04
        Else
            If oNodeScreenI.Param("win") = "N" Then
                'vk 01.09
                If sDir > "" OrElse Val(sCode) = 4 Then
                    oControl.Add("gridpos", "remember")
                Else
                    oControl.Add("gridpos", "delete")
                End If
                oControl.Add("onclick", "$('#Hgridpos').val($(this).attr('gridpos'));")
            End If
            Select Case sKind
                Case "print", "printscreen" : oControl.Add("onclick", "window.print();") 'vk 01.04, 05.05
                Case "printpdf"
                    If bp.getProperty("PrintPdf") = "Consist" Then
                        oControl.Add("onclick", "PrintPdf_Consist('" _
                            + IIf(bp.m_sPdf Like "http:*" OrElse bp.m_sPdf Like "https:*", "", bp.m_sUrl) + bp.m_sPdf + "'," _
                            + IIf(bp.m_sHova = "H", "true", "false") + ");") 'vk 12.14
                    Else
                        oControl.Add("onclick", "PrintPdf();") 'vk 02.14
                    End If
                Case "printpages"
                    oControl.Add("onclick", "bPrintServerFromButton=true;")
                    oControl.Add("onclick", "DisableButtonsOnly(true);")
                    oControl.Add("onclick", "PrintServer(1," + bp.m_nPagesToSend.ToString + ",0,'0');") 'vk 09.07
                Case "demo" : oControl.Add("onclick", "window.open('" + bp.getProperty("DemoButton") + "');") 'vk 08.04
                Case "pch" : oControl.Add("onclick", "CmdPchClick(this);")
                Case "pchl" : oControl.Add("onclick", "UpdateActionButton(this);")
                Case "archive_browse" : oControl.Add("onclick", "ArchiveBrowse();") 'vk 06.13
                Case "archive_submit"
                    If bp.getProperty("ArchiveLogic") = "1" Then
                        oControl.Add("onclick", "ArchiveSubmit('true');") 'vk 07.17
                    Else
                        oControl.Add("onclick", "ArchiveSubmit('false');") 'vk 07.13
                    End If
                Case "jacket" 'vk 12.17
                    Dim conn As SqlConnection = Nothing
                    Dim dsRecords As New DataSet()
                    Dim sJacket As String = ""
                    Try
                        Dim sSql As String = "select sReport from tblForm where sKind='jacket" &
                                             "' and sForm='" & oNodeScreenI.ParamVal("jacket").ToString &
                                             "' and nClient=" & oNodeScreenI.ParamVal("cli").ToString
                        If bp.getProperty("JacketDB") > "" Then
                            Dim pp As New PocketKnife.Info()

                            If bp.getProperty("SqlUser") = "" Then 'ntg 10.07.24 vladi's change regarding sql migration for bituah haklai
                                conn = New SqlConnection("Data Source=" & bp.getProperty("SqlServer") &
                                                     ";Initial Catalog=" & bp.getProperty("JacketDB") &
                                                     ";Integrated Security=true;" & bp.getProperty("ConnStrAddition"))
                            Else
                                conn = New SqlConnection("Data Source=" & bp.getProperty("SqlServer") &
                                                     ";Initial Catalog=" & bp.getProperty("JacketDB") &
                                                     ";User ID=" & bp.getProperty("SqlUser") &
                                                     ";Password=" & pp.DecryptPassword(bp.getProperty("SqlPass")) & ";" & bp.getProperty("ConnStrAddition"))
                            End If
                            pp = Nothing
                        Else
                            conn = New SqlConnection(bp.m_sConnectionString)
                        End If
                        conn.Open()
                        Dim dtAdapter As New SqlDataAdapter(sSql, conn)
                        dtAdapter.SelectCommand.CommandType = CommandType.Text
                        dtAdapter.Fill(dsRecords)
                        With dsRecords.Tables(0).Rows
                            If .Count > 0 Then
                                sJacket = .Item(0)(0)
                            End If
                        End With
                    Catch e As Exception
                        Throw New Exception("1020 DB error. " + vbCrLf + e.Message, e) 'vk 07.06
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
                    If sJacket = "" Then
                        oControl.Add("onclick", "alert('" & bp.getProperty("JacketMsg") & " " & oNodeScreenI.ParamVal("jacket").ToString & "');")
                    Else
                        oControl.Add("onclick", "window.open('" & bp.getProperty("JacketRoot") & "/" & sJacket & "');")
                    End If
                Case Else
                    If oNode.ParamVal("fk") = 2 AndAlso oNodeScreenI.Param("fgr") = "T" Then
                        oControl.Add("onclick", "Sql1();") 'vk 11.15
                    Else
                        oControl.Add("onclick", "fnBtnClick(this);")
                    End If
            End Select
        End If
        Select Case sKind
            Case "l"
                If oNode.Param("lon") = "N" OrElse oNode.ParamYes("ltr") Then
                    bDisabled = True
                End If
            Case "pchl"
                If Not oNode.ParamExists("val") Then
                    bDisabled = True
                End If
        End Select

        If bDisabled Then
            oControl.Add("disabled", "true")
        End If

        If oNode.ParamExists("fbua") OrElse oNode.ParamExists("bua") Then
            oControl.Add("data-toggle", "tooltip")
            oControl.Add("title", qColor.GetHelp_Simple(bp, oNode, oNodeScreenI, If(oNode.ParamExists("bua"), "bua", "fbua")).Replace("\n", " ").Trim()) 'vk 01.12
        Else
            If sKind = "pchl" Then
                oControl.Add("title", "דאבל-קליק") 'ntg 29.06.23 in the pchl (reshimat peiluyot) the hover is double click
            Else
                oControl.Add("title", sCaption.Replace("""", "&quot;"))
            End If
            'oControl.Add("title", "Double Click")
        End If

        oControl.Add("onfocus", "bDontSelect=false;")


        If bp.IsFoldersScreen(oNodeScreenI) Then
            If oNode.ParamExists("HiddentFieldRef") Then
                oControl.Add("HiddentFieldRef", oNode.Param("HiddentFieldRef"))
                oControl.Add("Mark", oNode.Param("Mark"))

                oControl.ReplaceVal("onclick", "UpdateHiddenMark(this) ;CmdPchClick_Common('', '00', false, false, 0);")
            End If
        End If

        Select Case sKind
            Case "c", "printpdf", "jacket", "archive_browse", "archive_submit"
                'oControl.ReplaceVal("value", oControl.Param("value").Replace("פולדר", "").Trim())
                bp.cButtons.Add(New Button(oControl, oNode))
            Case Else
                Dim IsImage As Boolean = oNode.ParamVal("psz") > 0

                bp.m_sOneSomething = oControl.FullCode(bp, "button", IconFullValue:=If(oNode.ParamExists("icn") AndAlso Not IsImage, $"{oNode.Param("icn")}-{oNode.Param("fcl")}",
                                                                                    If(oNode.ParamExists("pic") AndAlso Not IsImage, $"{oNode.Param("pic")}-{oNode.Param("pcl")}", "")),
                                                       ImageFileName:=If(IsImage, bp.m_colColor.Item($"ImageFile_{oNode.ParamVal("pic")}"), ""), NodeScreen:=oNodeScreenI, OriginalNode:=oNode)
        End Select

    End Sub

    Private Function CutFCommand(value As String) As String
        Dim Index As Integer = value.IndexOf("= ")

        If Index >= 0 Then
            Return value.Substring(Index + 1)
        Else Return value
        End If
    End Function

    Function Logo(ByRef bp As BuildPage, ByRef oStyle As Style, ByVal bBkg As Boolean, ByVal bSpool As Boolean) As String

        Select Case bp.getProperty("LogoClick")
            Case "click", "dblclick" : oStyle.Add("cursor", "pointer")
        End Select
        Dim oControlT As New Control(oStyle.FullCode)
        oControlT.Add("src", "pics/" + bp.getProperty("Logo"))
        oControlT.Add("disabled", "True")
        oControlT.AddId("logo")
        If Not bBkg Then
            Select Case bp.getProperty("LogoClick")
                Case "click", "dblclick" : oControlT.Add("On" + bp.getProperty("LogoClick"), bp.getProperty("LogoAction"))
            End Select
        End If

        Return oControlT.FullCode(bp, "img")
    End Function

    Private Function StartCombo(ByRef bp As BuildPage, ByRef rd As Xml.XmlTextReader,
            ByRef sOut_Local As StringBuilder,
            ByRef oNode As Node, ByRef oNodeScreen As Node,
            ByVal bReadOnly As Boolean, ByVal bBkg As Boolean) As Boolean 'vk 09.06

        Dim bMustContinue As Boolean = False

        If oNode.ParamIn("apr", "P", 2) AndAlso oNodeScreen.Param("fgr") <> "P" Then 'vk 05.05
            iRow = 0
            sXML_Combo = "<f val=""" + oNode.Param("val") + """ ></f>"
        ElseIf oNode.ParamIn("pf4", "RSAB") Then
            If oNode.ParamIn("cry", "GI") Then
                sXML_Combo = qCombo.BuildXml(bp, oNode, oNodeScreen, 0)
            ElseIf oNode.ParamIn("pf4", "AB") Then
                sXML_Combo = qCombo.BuildXml(bp, oNode, oNodeScreen, 0)
                iRow = qCombo.CountLinesInCombo_ByXml(sXML_Combo, oNode, True)
            Else
                iRow = qCombo.CountLinesInCombo_ByTbl(bp, oNode, oNodeScreen)
                sXML_Combo = qCombo.BuildXml(bp, oNode, oNodeScreen, iRow)
            End If
        ElseIf bp.getProperty("CityStreet") = "inline" AndAlso oNode.ParamIn("apr", "CS", 2) AndAlso oNodeScreen.Param("fgr") <> "P" Then 'ntg 10.07.24 vladi's change regarding city-street screen
            sXML_Combo = qCombo.BuildXml_CityStreet(bp, oNode, oNodeScreen)
        Else
            If BOOTSTRAP Then
                sXML_Combo = oNode.Param("OuterXml")
            Else
                sXML_Combo = rd.ReadOuterXml()
            End If
            iRow = qCombo.CountLinesInCombo_ByXml(sXML_Combo, oNode, True)
        End If

        'ntg 03.12.23 removed these lines because they are unnecessary
        'Dim sLocalPathh As String = System.Reflection.Assembly.GetExecutingAssembly.Location.ToString
        'Dim ii As Integer = InStrRev(sLocalPathh, "\")
        'sLocalPathh = Left(sLocalPathh, ii)
        'Dim rDef As Reader = New Reader(sLocalPathh & "defprop.config")
        Dim isGridCombo As Boolean
        Dim isGridComboStr As String = bp.getProperty("ComboAsGrid")
        Boolean.TryParse(isGridComboStr, isGridCombo)

        'If oNode.ParamIn("cry", "GI") Then
        If oNode.ParamIn("cry", "GI") And isGridCombo Then 'ntg 03.12.23 if isGridCombo=True in defprop, then the combo will look like in dds as grid
            qCombo.BuildOption_AsGrid(bp, sXML_Combo, oNode, oNodeScreen, sOptionT, sN, bReadOnly, False, bBkg)
        Else
            ilin = qStyle.ilin_fun(bp, oNode, oNodeScreen)
            icol = qStyle.icol_fun(bp, oNode, oNodeScreen)
            iwidth = qStyle.iwid_fun(bp, oNode, oNodeScreen)
            If Not bReadOnly AndAlso iRow > Val(oNode.Param("pxr")) Then
                iwidth += bp.ResolValue(0, 2, False, "hor")
            End If
            sStyleT = qStyle.GetStyleOption(oNodeScreen, iwidth)

            If qCombo.MayAsRadio(oNode, iRow) Then
                qCombo.BuildOption_AsRadio(bp, sXML_Combo, oNode, oNodeScreen, sOptionT, sN, bReadOnly, bBkg)
            ElseIf qCombo.MayAsCheck(oNode, iRow) Then 'vk 01.04
                qCombo.BuildOption_AsCheck(bp, sXML_Combo, oNode, oNodeScreen, sOptionT, sN, bReadOnly, bBkg)
            Else
                iRow += 1
                bMustContinue = True
                'moved here vk 03.12
                Dim iwidth1 As Integer = iwidth
                Dim iwidth2 As Integer = iwidth + 2 * bp.iScaleWidth
                If iRow > Val(oNode.Param("pxr")) Then
                    iRow = Val(oNode.Param("pxr"))
                    iwidth += 2 * bp.iScaleWidth
                End If
                oControl.Add("sWidth1", iwidth1.ToString)
                oControl.Add("sWidth2", iwidth2.ToString)
                oControl.Add("sHeight1", (bp.iScaleLin - bp.iScaleLinComboDelta).ToString)
                oControl.Add("sMaxRows", oNode.Param("pxr")) 'vk 03.14
            End If
        End If

        If Not bMustContinue Then
            sOut_Local.Append(sOptionT)
            If (Not bReadOnly) AndAlso (Not oNode.ParamIn("cry", "GI")) AndAlso CurrentField(bp, oNode, oNodeScreen, False, "", "") Then
                bp.m_sNameFocus = "S" + sN
            End If
            bNoControl = True
        End If
        Return bMustContinue

    End Function

    Private Function CurrentField(ByRef bp As BuildPage,
            ByRef oNode As Node, ByRef oNodeScreen As Node,
            ByVal bAdditionalField As Boolean, ByVal sFind As String, ByVal sFil As String) As Boolean 'vk 12.04

        Dim fpc As String 'vk 01.09
        If oNodeScreen.Param("agr") = "A" AndAlso oNodeScreen.Param("fil") = sFil AndAlso sFind > "" Then
            fpc = sFind
        ElseIf oNodeScreen.Param("agr") = "P" AndAlso sFind > "" Then
            fpc = sFind
        Else
            fpc = oNodeScreen.Param("fpc")
        End If
        If bAdditionalField Then Return False 'vk 05.05
        If fpc = bp.fpcD Then Return False
        If Not oNode.ParamIn("inp", "IKR") Then Return False
        If oNode.ParamYes("only") Then Return False 'vk 09.06
        If oNode.ParamVal("lin") = 0 Then
            Return fpc = Mid(oNode.Param("tab"), 2)
        Else
            Return fpc = oNode.Param("ind")
        End If

    End Function

    Private Function AddToTopAndExit(ByRef bp As BuildPage,
            ByRef oNode As Node, ByRef oNodeScreen As Node, ByRef oControl As Control) As Boolean

        If oNode.ParamVal("lin") <> 1 OrElse oNodeScreen.Modal Then Return False
        'oNode.Param("val") = oNode.Param("val").Replace(" ", "&nbsp;") 'vk 09.05
        'If oNodeScreen.Param("srg") = "C" AndAlso oNode.InMiddle(oNodeScreen) Then
        '    bp.sTopLine2.Append(oNode.Param("val")) 'vk 05.07
        'Else
        '    Dim s As String = "<span " + sStyle + " Class='TopOut" +
        '        IIf(oNodeScreen.ParamIn("srg", "BA") AndAlso oNode.InMiddle(oNodeScreen), "2", "1").ToString +
        '        "' >" + oNode.Param("val") + "</span>"
        '    If oNodeScreen.Param("srg") = "B" AndAlso oNode.InMiddle(oNodeScreen) Then
        '        bp.sTopLine2.Append("<td id='1646'>" + s + "</td>" + vbCrLf)
        '    Else
        '        bp.sTopLine1.Append(s + vbCrLf)
        '    End If
        'End If
        'If bp.m_sEnvironment = "" Then bp.m_sEnvironment = oNode.Param("val") 'vk 08.08
        'If Not oControl Is Nothing Then oControl.Dispose()
        'oControl = Nothing
        Return True

    End Function

    Private Sub ToolTip(ByRef bp As BuildPage, ByRef oNode As Node, ByRef oNodeScreen As Node,
        ByRef oControl As Control) 'vk 10.03, moved here from Color and modified vk 02.14

        Dim s As String = ""
        If sXML_Combo > "" Then
            Dim sr As New StringReader(sXML_Combo)
            Dim rd As New Xml.XmlTextReader(sr)
            s = qCombo.HelpText(bp, oNode, oNodeScreen, rd, True)
            If Not sr Is Nothing Then sr.Dispose()
            sr = Nothing
            rd = Nothing
        End If
        If s = "" Then
            If oNode.ParamExists("hky") Then 'bua vk 07.04
                s = qColor.GetHelp(bp, oNode, oNodeScreen, "1")
            End If
        End If
        If s = "" Then Exit Sub
        oControl.Add("helptext", s)
    End Sub

    Function FromFile(ByVal sFile As String) As String 'vk 02.05, 03.10

        Dim fs As FileStream = Nothing
        Dim sw As StreamReader = Nothing
        Dim s As String
        Try
            fs = New FileStream(sFile, FileMode.Open, FileAccess.Read, FileShare.Read)
            sw = New StreamReader(fs)
            s = sw.ReadToEnd
            'If sFrom > "" Then s = s.Replace(sFrom, sTo) 'vk 07.07
            Return s
        Catch
            Return ""
        Finally

            If sw IsNot Nothing Then
                sw.Close()
                sw.Dispose()
            End If
            If fs IsNot Nothing Then
                fs.Close()
                fs.Dispose()
            End If
        End Try
    End Function

    Sub Dispose()
        qColor.Dispose()
        qColor = Nothing
        qCombo.Dispose()
        qCombo = Nothing
        qConv = Nothing
        qStyle = Nothing
        qSecurity = Nothing
    End Sub
End Class
