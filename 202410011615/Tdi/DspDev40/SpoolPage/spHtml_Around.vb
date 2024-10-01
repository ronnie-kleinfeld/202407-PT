#Const AVOIDBACK = False 'vk 06.04
'#Const SHOWBROWSER = False 'vk 08.04
Friend Class Around

    'FormStyle():
    '<html>
    '<head>
    '<link />
    '<script> ... </script>
    '<style> ... </style>

    'FormHeader():
    '<script /> (js files)
    '<meta />
    '<title> ... </title>
    '</head>
    '<body>
    '<script> ... </script> (for kiosk)
    '<form>

    'FormForms():
    '</form>
    '<form /> (for iframes)
    '</body>
    '</html>

    'FormFunction():
    '<script> ... </script> (after html)

    Private qStyle As New StyleModule()
    Private qMulti As New Multi()

    Sub FormStyle(ByRef bp As BuildPage, ByRef sOut As System.Text.StringBuilder, _
            ByRef oNodeScreen As Node, ByVal bHideOnScreen As Boolean, ByVal bEmpty As Boolean, _
            ByVal bPrintOnly As Boolean, ByVal bPdf As Boolean)

        Dim bckg, forg, borderclr As String
        Dim iCol As Integer

        If bPdf Then
            sOut.Append("<html xmlns:v='urn:schemas-microsoft-com:vml' xmlns:w='urn:schemas-microsoft-com:office:word'>").Append(vbCrLf)
        Else
            sOut.Append("<!DOCTYPE html>").Append(vbCrLf)
            sOut.Append("<html xmlns ='http://www.w3.org/1999/xhtml'>").Append(vbCrLf)
        End If
        sOut.Append("<head>").Append(vbCrLf)

        If bp.getProperty("AdditionalHtml_Header") > "" Then
            sOut.Append(bp.getProperty("AdditionalHtml_Header")).Append(vbCrLf) 'vk 02.16
        End If
        sOut.Append("<!-- job: ").Append(bp.m_sJob).Append(" -->").Append(vbCrLf)
        sOut.Append("<!-- user: ").Append(bp.m_sUser).Append(" -->").Append(vbCrLf)
        sOut.Append("<!-- session: ").Append(bp.m_sSession).Append(" -->").Append(vbCrLf)
        sOut.Append("<!-- site: ").Append(bp.getProperty("Site")).Append(" -->").Append(vbCrLf)
        sOut.Append("<!-- now: ").Append(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")).Append(" -->").Append(vbCrLf)
        If Not oNodeScreen Is Nothing Then
            sOut.Append("<!-- database: ").Append(oNodeScreen.Param("flib")).Append(" -->").Append(vbCrLf)
            sOut.Append(oNodeScreen.FullCode(bp.getProperty("IncludeXml"), "main")) 'vk 07.04
        End If

        sOut.Append("<meta charset = 'utf-8' />").Append(vbCrLf)

        sOut.Append("<link href='css/dds.css' rel='stylesheet'>").Append(vbCrLf)
        sOut.Append("<link href = 'bootstrap/css/bootstrap.min.css' rel='stylesheet' />").Append(vbCrLf)
        sOut.Append("<script src = 'jquery/jquery-1.10.2.min.js'></script>").Append(vbCrLf)
        sOut.Append("<script src = 'bootstrap/js/bootstrap.min.js'></script>").Append(vbCrLf)

        sOut.Append("<link href = 'Datetimepicker/css/bootstrap-datetimepicker.min.css' rel='stylesheet'/>").Append(vbCrLf)
        sOut.Append("<script src = 'Datetimepicker/js/bootstrap-datetimepicker.js'></script>").Append(vbCrLf)
        sOut.Append("<script src = 'Datetimepicker/js/locales/bootstrap-datetimepicker.he.js'></script>").Append(vbCrLf)


        If oNodeScreen.Param("wait") = "S" AndAlso oNodeScreen.ParamVal("cli") > 0 Then 'vk 02.06
            sOut.Append("<link href='css/cli").Append(oNodeScreen.Param("cli")).Append(".css' rel='stylesheet'>").Append(vbCrLf)
        End If
        If bp.getProperty("Design") = "lex" AndAlso Not bPrintOnly Then 'vk 05.09
            sOut.Append("<link href='css/lex.css' rel='stylesheet'>").Append(vbCrLf)
        End If

        sOut.Append("<SCRIPT>").Append(vbCrLf)
        If bp.getProperty("BlockSelect") = "true" AndAlso oNodeScreen.Param("wait") = "S" Then
            sOut.Append("document.onselectstart=new Function('return false');").Append(vbCrLf)
        End If

        'sOut.Append("window.menubar.visible = false;").Append(vbCrLf)
        'sOut.Append("window.toolbar.visible = false;").Append(vbCrLf)
        'sOut.Append("window.history.forward(1);").Append(vbCrLf)

        sOut.Append("$(document).ready(function() {").Append(vbCrLf)

        sOut.Append("$(window).bind('keydown', function (e) {").Append(vbCrLf)
        sOut.Append("var keyCode = e.keyCode || e.which;").Append(vbCrLf)
        sOut.Append("if (keyCode >= 112 && keyCode <= 123) {").Append(vbCrLf)
        sOut.Append("window.onhelp = function() { return false; }").Append(vbCrLf)
        sOut.Append("e.cancelable = true;").Append(vbCrLf)
        sOut.Append("e.stopPropagation();").Append(vbCrLf)
        sOut.Append("e.preventDefault();").Append(vbCrLf)
        sOut.Append("e.returnValue = false;").Append(vbCrLf)
        sOut.Append("}});").Append(vbCrLf)

        'sOut.Append("$(window).on('beforeunload', function() {").Append(vbCrLf)
        'sOut.Append("return 4;").Append(vbCrLf)
        'sOut.Append("});").Append(vbCrLf)

        'sOut.Append("$(window).bind('beforeunload', function(e) {").Append(vbCrLf)
        'sOut.Append("var message = 'Why are you leaving?';").Append(vbCrLf)
        'sOut.Append("e.returnValue = message;").Append(vbCrLf)
        'sOut.Append("return message;").Append(vbCrLf)
        'sOut.Append("});").Append(vbCrLf)

        'sOut.Append("$(window).bind('beforeunload', function () {return 'Are you sure';};").Append(vbCrLf)
        'sOut.Append("window.onbeforeunload = function () {return 'Are you sure';};").Append(vbCrLf)
        'sOut.Append("window.onbeforeunload = function () {return CloseMsgText();};").Append(vbCrLf)
        'sOut.Append("window.onbeforeunload = CloseMsg;").Append(vbCrLf)

        'sOut.Append("window.addEventListener('beforeunload', function (event) {").Append(vbCrLf)
        ''sOut.Append("$(window).bind('beforeunload', function (event) {").Append(vbCrLf)
        'sOut.Append("event.preventDefault();").Append(vbCrLf)
        ''sOut.Append("event.returnValue = msg(12);").Append(vbCrLf)
        'sOut.Append("if ($('#Hx').val() == 'x') {event.returnValue=msg(12);}").Append(vbCrLf)
        'sOut.Append("else try {$('#Hwait', opener.document).val('');} catch(e){}").Append(vbCrLf)
        'sOut.Append("});").Append(vbCrLf)

        sOut.Append("});").Append(vbCrLf)

        sOut.Append("</SCRIPT>").Append(vbCrLf)
        sOut.Append("<STYLE>").Append(vbCrLf)

        If Not bEmpty Then 'vk 12.07
            bckg = bp.m_colColor.Item("inp_bckg")
            forg = bp.m_colColor.Item("inp_forg")
            borderclr = bp.m_colColor.Item("pf4_bckg")
            bp.m_inp_bkg = bckg
            bp.m_inp_fgd = forg

            ' Input field without focus
            sOut.Append(".InpCom{").Append(vbCrLf)
            sOut.Append(oNodeScreen.FontColor(bckg, forg, bp, "", bPdf))
            sOut.Append(oNodeScreen.HeightPos(bp, "", bPdf))
            sOut.Append("}").Append(vbCrLf)

            ' Open Table for Select
            sOut.Append(".TblSel").Append(vbCrLf)
            sOut.Append("{").Append(vbCrLf)
            sOut.Append(oNodeScreen.FontColor(bckg, forg, bp, "", bPdf))
            sOut.Append(oNodeScreen.HeightPos(bp, "1100", "", bPdf))
            sOut.Append("}").Append(vbCrLf)

            ' Input field without focus with pf4
            sOut.Append(".InpCom4").Append(vbCrLf)
            sOut.Append("{").Append(vbCrLf)
            sOut.Append(oNodeScreen.FontColor(bckg, forg, bp, "", bPdf))
            sOut.Append(oNodeScreen.HeightPos(bp, "", bPdf))
            sOut.Append("border:      1px solid ").Append(borderclr).Append(";").Append(vbCrLf)
            sOut.Append("}").Append(vbCrLf)

            '/Input field without focus with psl
            sOut.Append(".InpComSL").Append(vbCrLf)
            sOut.Append("{").Append(vbCrLf)
            sOut.Append(oNodeScreen.HeightPos(bp, "", bPdf))
            sOut.Append("}").Append(vbCrLf)

            '/input field with focus
            sOut.Append(".InpCom:focus").Append(vbCrLf)
            sOut.Append("{").Append(vbCrLf)
            sOut.Append(oNodeScreen.FontColor(bckg, forg, bp, "", bPdf))
            sOut.Append(oNodeScreen.HeightPos(bp, "", bPdf))
            sOut.Append("}").Append(vbCrLf)

            ' input field with focus with pf4
            sOut.Append(".InpCom4:focus").Append(vbCrLf)
            sOut.Append("{").Append(vbCrLf)
            sOut.Append(oNodeScreen.FontColor(bckg, forg, bp, "", bPdf))
            sOut.Append(oNodeScreen.HeightPos(bp, "", bPdf))
            sOut.Append("}").Append(vbCrLf)

            'sOut.Append(".InpFocCheck").Append(vbCrLf)
            'sOut.Append("{").Append(vbCrLf)
            'sOut.Append(oNodeScreen.FontColor(bckg, forg, bp, "", bPdf, oNodeScreen))
            'sOut.Append(oNodeScreen.HeightPos(bp, "", bPdf, oNodeScreen))
            'sOut.Append("border: 2px solid #FF0000;").Append(vbCrLf)
            'sOut.Append("}").Append(vbCrLf)

            bckg = "#FF0000"
            forg = "black"

            ' input field with focus with psl
            sOut.Append(".InpComSL:focus").Append(vbCrLf)
            sOut.Append("{").Append(vbCrLf)
            sOut.Append(oNodeScreen.FontColor(bckg, forg, bp, "1100", "", bPdf))
            sOut.Append(oNodeScreen.HeightPos(bp, "", bPdf))
            sOut.Append("}").Append(vbCrLf)

            bckg = bp.m_colColor.Item("procond_bckg")
            forg = bp.m_colColor.Item("procond_forg")

            ' Protected (conditional) field
            sOut.Append(".InpProtCond").Append(vbCrLf)
            sOut.Append("{").Append(vbCrLf)
            sOut.Append(oNodeScreen.FontColor(bckg, forg, bp, "", bPdf))
            sOut.Append(oNodeScreen.HeightPos(bp, "", bPdf))
            sOut.Append("}").Append(vbCrLf)

            bckg = bp.m_colColor.Item("pro_bckg")
            forg = bp.m_colColor.Item("pro_forg")

            ' Protected field
            sOut.Append(".InpProt").Append(vbCrLf)
            sOut.Append("{").Append(vbCrLf)
            sOut.Append(oNodeScreen.FontColor(bckg, forg, bp, "", bPdf))
            sOut.Append(oNodeScreen.HeightPos(bp, "", bPdf))
            sOut.Append("}").Append(vbCrLf)

            ' Protected field without bound
            sOut.Append(".InpProtNB").Append(vbCrLf)
            sOut.Append("{").Append(vbCrLf)
            sOut.Append(oNodeScreen.FontColor(bckg, forg, bp, "", bPdf))
            sOut.Append(oNodeScreen.HeightPos(bp, "", bPdf))
            sOut.Append("}").Append(vbCrLf)

            ' Protected combo
            bckg = bp.m_colColor.Item("procmb_bckg")
            forg = bp.m_colColor.Item("procmb_forg")

            sOut.Append(".InpProtCombo").Append(vbCrLf)
            sOut.Append("{").Append(vbCrLf)
            sOut.Append(oNodeScreen.FontColor(bckg, forg, bp, "", bPdf))
            sOut.Append(oNodeScreen.HeightPos(bp, "", bPdf))
            sOut.Append("}").Append(vbCrLf)

            ' Grid
            Dim v, vv As String
            For Each vv In AllStyleSuffixes_Grid1
                For Each v In AllStyleSuffixes_Grid2
                    sOut.Append(".grid").Append(vv).Append(v).Append(vbCrLf)
                    sOut.Append("{").Append(vbCrLf)
                    sOut.Append(oNodeScreen.HeightPos(bp, "1000", "", bPdf))
                    sOut.Append("BACKGROUND-COLOR: ").Append(bp.m_colColor.Item("grid" + vv + v + "_bckg").ToString).Append(";").Append(vbCrLf)
                    sOut.Append("border: ").Append(bp.getProperty("GridBorder" + vv)).Append(";").Append(vbCrLf)
                    sOut.Append("z-index: -1;").Append(vbCrLf)
                    sOut.Append("}").Append(vbCrLf) '0 vk 03.10
                Next
            Next

            If oNodeScreen.Param("wait") = "S" Then
                bckg = "transparent" 'vk 01.06
            Else
                bckg = bp.m_colColor.Item("out_bckg")
            End If
            forg = bp.m_colColor.Item("out_forg")

            ' Output field
            Dim iDelta As Integer = 0
            Dim iColPlusDelta As Integer
            Dim oNode As New Node()
            oNode.Param("col") = "1"
            oNode.Param("lin") = "2"
            iCol = qStyle.icol_fun(bp, oNode, oNodeScreen)
            iDelta = Val(bp.getPropertyPset("SpoolShiftRight", oNodeScreen)) 'vk 09.06
            iColPlusDelta = iCol + iDelta
            For Each v In AllFonts
                sOut.Append(".OutCom").Append(v).Append(vbCrLf)
                sOut.Append("{").Append(vbCrLf)
                If Not bPdf Then 'vk 03.09
                    sOut.Append("position: absolute;").Append(vbCrLf)
                End If
                Dim sCompon As String
                sCompon = IIf(oNodeScreen.ParamExists("pset") OrElse oNodeScreen.Param("wait") = "S", "1110", "1111").ToString
                If v = "" AndAlso oNodeScreen.ParamYes("spo") Then 'vk 01.06
                    sOut.Append(oNodeScreen.FontColor(bckg, forg, bp, sCompon, "Usual", bPdf))
                Else
                    sOut.Append(oNodeScreen.FontColor(bckg, forg, bp, sCompon, v, bPdf))
                End If
                sCompon = IIf(oNodeScreen.Param("wait") = "S", "1100", "1101").ToString
                If bp.getPropertyPset("FixedLineHeight", oNodeScreen) = "true" Then
                    sOut.Append(oNodeScreen.HeightPos(bp, sCompon, "Usual", bPdf)) 'vk 01.20
                Else
                    sOut.Append(oNodeScreen.HeightPos(bp, sCompon, v, bPdf))
                End If
                If bPdf Then
                    'vk 03.09
                    If oNodeScreen.Param("wait") = "S" OrElse oNodeScreen.ParamExists("pset") Then
                        sOut.Append("font-size:").Append(bp.SizeByClass(bp.getPropertyPset("SpoolFontSize", oNodeScreen), v, True, oNodeScreen).ToString).Append("px;").Append(vbCrLf)
                    End If
                    sOut.Append("unicode-bidi:bidi-override;").Append(vbCrLf)
                    sOut.Append("text-align:").Append(IIf(oNodeScreen.ParamVal("flr") = 1 OrElse oNodeScreen.ParamVal("flang") <> 0, "left", "right").ToString).Append(";").Append(vbCrLf)
                    sOut.Append("direction:ltr;").Append(vbCrLf)
                    sOut.Append("}").Append(vbCrLf)
                Else
                    sOut.Append("}").Append(vbCrLf)
                    'vk 05.10, 06.10
                    Dim s As String = bp.getPropertyPset("SpoolFontSize", oNodeScreen)
                    If oNodeScreen.Param("wait") = "S" OrElse oNodeScreen.ParamExists("pset") Then
                        sOut.Append("@media print{.OutCom").Append(v).Append("{").Append(vbCrLf)
                        sOut.Append("left:").Append(iColPlusDelta.ToString).Append("px;").Append(vbCrLf)
                        sOut.Append("font-size:").Append(bp.SizeByClass(s, v, False, oNodeScreen).ToString).Append("px;").Append(vbCrLf)
                        sOut.Append("}}").Append(vbCrLf)
                        sOut.Append("@media screen{.OutCom").Append(v).Append("{").Append(vbCrLf)
                        If bHideOnScreen Then
                            sOut.Append("visibility:hidden;").Append(vbCrLf)
                        End If
                        sOut.Append("left:").Append(iCol.ToString).Append("px;").Append(vbCrLf)
                        If v = "" OrElse Not oNodeScreen.SpoolFontFixed(bp) Then
                            If oNodeScreen.Param("wait") <> "S" Then
                                s = bp.sFontSize
                            End If
                            sOut.Append("font-size:").Append(bp.SizeByClass(s, v, False, oNodeScreen).ToString).Append("px;").Append(vbCrLf)
                        Else
                            sOut.Append("font-size:").Append(bp.SizeByClass(bp.sFontSizeFixed, v, False, oNodeScreen).ToString).Append("px;").Append(vbCrLf)
                        End If
                        sOut.Append("}}").Append(vbCrLf)
                    End If
                End If
            Next

            If oNodeScreen.Param("wait") = "S" AndAlso Not bPdf Then 'vk 01.06, 03.09
                'sOut.Append(".OutTable").Append(vbCrLf)
                'sOut.Append("{").Append(vbCrLf)
                'sOut.Append(oNodeScreen.HeightPos(bp, "0001", "", bPdf, oNodeScreen))
                'sOut.Append("}").Append(vbCrLf)
                sOut.Append("@media screen{.OutTable{").Append(vbCrLf)
                If bHideOnScreen Then
                    sOut.Append("visibility:hidden;").Append(vbCrLf) 'vk 10.07
                End If
                'If oNodeScreen.Param("wait") = "S" Then 'vk 01.06
                sOut.Append("left:").Append(iCol.ToString).Append("px;").Append(vbCrLf)
                'Else
                'sOut.Append("left:" + iColPlusDelta.ToString + "px;" + vbCrLf)
                'End If
                sOut.Append("}}").Append(vbCrLf)
                sOut.Append("@media print{.OutTable{").Append(vbCrLf)
                'If oNodeScreen.ParamExists("pset") Then
                sOut.Append("left:").Append(iColPlusDelta.ToString).Append("px;").Append(vbCrLf)
                'Else
                '    sOut.Append("left:" + iCol.ToString + "px;" + vbCrLf) 'vk 02.06
                'End If
                sOut.Append("}}").Append(vbCrLf)
            End If
            If Not oNode Is Nothing Then oNode.Dispose()
            oNode = Nothing
            'vk 01.10
            If bPdf Then
                sOut.Append(".Section1 {position:static;}").Append(vbCrLf)
            Else
                sOut.Append(".Section1 {position:absolute;}").Append(vbCrLf)
            End If

            'Stamps
            Dim stp As String = oNodeScreen.Param("stp")
            If bp.getPropertyStamp("Count", oNodeScreen.Param("stp")) > "" AndAlso oNodeScreen.Param("wait") = "S" _
                    AndAlso stp <> " " AndAlso Not bPdf Then 'vk 07.05, 11.09
                Dim i As Integer
                For i = 1 To bp.getPropertyStampVal("Count", stp)
                    Dim sStyleScreen As String = bp.getPropertyStamp("StyleExternal" + i.ToString, stp)
                    sOut.Append("@media screen{.Stamp").Append(i.ToString).Append("{").Append(sStyleScreen)
                    If bHideOnScreen Then
                        'vk 10.07
                        If Not (sStyleScreen Like "*;") Then
                            sOut.Append(";")
                        End If
                        sOut.Append("visibility:hidden;")
                    End If
                    sOut.Append("}}").Append(vbCrLf)
                    Dim sStylePrint As String = ""
                    If oNodeScreen.ParamExists("pset") Then
                        Dim aa As String, bb() As String
                        For Each aa In sStyleScreen.Split(";")
                            bb = aa.Split(":")
                            If aa.Trim = "" Then
#If JAVA_MAINSOFT Then
                        ElseIf aa.IndexOf(":") >= 0 AndAlso bb(0).Trim.ToLower = "left" Then
#Else
                            ElseIf aa Like "*:*" AndAlso bb(0).Trim.ToLower = "left" Then
#End If
                                sStylePrint += bb(0) + ":" + (CInt(Val(bb(1))) + iDelta).ToString + "px;"
                            Else
                                sStylePrint += aa + ";"
                            End If
                        Next
                    Else
                        sStylePrint = sStyleScreen
                    End If
                    sOut.Append("@media print{.Stamp").Append(i.ToString).Append("{").Append(sStylePrint).Append("}}").Append(vbCrLf)
                Next
            End If

        End If
        ' --------------------------
        ' body + topFrame
        ' --------------------------

        'vk 12.04
        If bPdf Then
            bckg = "transparent" 'vk 04.09
        Else
            bckg = bp.m_colColor.Item("body_bckg")
        End If
        forg = bp.m_colColor.Item("body_forg")
        sOut.Append("body").Append(vbCrLf)
        sOut.Append("{").Append(vbCrLf)
        sOut.Append(oNodeScreen.FontColor(bckg, forg, bp, "1000", "", bPdf))
        If oNodeScreen.Param("plgc") = "P" Then
            sOut.Append("overflow:hidden;").Append(vbCrLf) 'vk 04.09
        End If
        sOut.Append("}").Append(vbCrLf)

        If Not bEmpty Then 'vk 12.07

            'vk 06.07
            Dim nRightButtons, nRightButtons_Print, iTmp As Integer
            iTmp = Convert.ToInt32(IIf(oNodeScreen.ParamVal("flr") = 1, 10, 0)) _
                    + bp.m_nResolutionW - bp.iMarginLeftCom
            iTmp -= 5 'vk 06.09
            If oNodeScreen.Param("wait") = "S" Then
                If oNodeScreen.Param("plgc") <> "P" Then iTmp -= 10 'vk 06.09
                If iTmp <bp.m_nSpoolWidth Then
                    nRightButtons= bp.m_nSpoolWidth
                Else
                    nRightButtons= iTmp
                End If
                nRightButtons_Print = bp.m_nSpoolWidth
            Else
                nRightButtons = iTmp
                nRightButtons_Print = iTmp
            End If

            If bp.m_bKiosk Then
                sOut.Append(".BtnFrame {}").Append(vbCrLf)
                sOut.Append(".BtnFrameOppozite {}").Append(vbCrLf)
            Else
                sOut.Append(".BtnFrame").Append(vbCrLf)
                sOut.Append("{").Append(vbCrLf)
                sOut.Append("background:linear-gradient(to right,").Append(forg).Append(",").Append(bckg).Append(");").Append(vbCrLf)
                sOut.Append("}").Append(vbCrLf)

                sOut.Append(".BtnFrameOppozite").Append(vbCrLf)
                sOut.Append("{").Append(vbCrLf)
                sOut.Append("background:linear-gradient(to right,").Append(bckg).Append(",").Append(forg).Append(");").Append(vbCrLf)
                sOut.Append("}").Append(vbCrLf)
            End If
            'vk 06.07
            sOut.Append(".BtnFrameTransp{left:0px;}").Append(vbCrLf)
            sOut.Append("@media screen").Append(vbCrLf)
            sOut.Append("{").Append(vbCrLf)
            If bHideOnScreen Then
                sOut.Append("visibility:hidden;").Append(vbCrLf)
            End If
            'vk 07.09
            Dim nLeft As Integer = bp.m_nResolutionW - bp.iMarginLeftCom 'vk 06.10
            If oNodeScreen.Param("wait") = "S" AndAlso oNodeScreen.Param("plgc") <> "P" Then 'vk 11.19
                Dim nLeft_Spool As Integer = bp.iScaleWidth * (bp.m_nPartLength - 7) + 1 + iCol
                If nLeft_Spool > nLeft Then nLeft = nLeft_Spool
            End If
            sOut.Append(".BtnFrameOppozite{left:").Append(CStr(nLeft)).Append("px;}").Append(vbCrLf)
            sOut.Append(".BtnFrameTranspOppozite{left:").Append(CStr(nLeft)).Append("px;}").Append(vbCrLf)
            sOut.Append("}").Append(vbCrLf)
            sOut.Append("@media print{").Append(vbCrLf)
            sOut.Append(".BtnFrameOppozite{left:0px;}").Append(vbCrLf)
            sOut.Append(".BtnFrameTranspOppozite{left:0px;}").Append(vbCrLf)
            sOut.Append("}").Append(vbCrLf)

        End If

        If bp.getProperty("Design") = "lex" AndAlso Not bPrintOnly Then 'vk 05.09
            'vk 05.07
            sOut.Append(".MsgFrame").Append(vbCrLf)
            sOut.Append("{").Append(vbCrLf)
            sOut.Append("background-color:transparent;").Append(vbCrLf)
            sOut.Append("}").Append(vbCrLf)
        Else
            sOut.Append(".MsgFrame").Append(vbCrLf)
            sOut.Append("{").Append(vbCrLf)
            sOut.Append("background:linear-gradient(").Append(bckg).Append(",").Append(forg).Append(");").Append(vbCrLf)
            sOut.Append("}").Append(vbCrLf)
        End If

        Dim a As Attrib
        For Each a In bp.m_cCss 'vk 05.05, 05.10
            Dim sName As String = a.sName
            Select Case sName
                Case "BtnComFoc" : sName = "BtnCom:hover"
                Case "BtnFieldFoc" : sName = "BtnField:hover"
                Case "BtnFolderFoc" : sName = "BtnFolder:hover"
                Case "BtnFolderFocE" : sName = "BtnFolderE:hover"
                Case "BtnFolderFocO" : sName = "BtnFolderO:hover"
                Case "BtnFolderFocB" : sName = "BtnFolderB:hover"
            End Select
            sOut.Append(".").Append(sName).Append(vbCrLf).Append("{").Append(vbCrLf)
            AppendStyle(bp, sOut, a.sValue, a.sName, _
                IIf(oNodeScreen.ParamVal("flr") = 1, "ltr", "rtl"), oNodeScreen.TopLength = 132)
            sOut.Append("}").Append(vbCrLf)
        Next
        'vk 11.06
        sOut.Append(".menuhover").Append(vbCrLf).Append("{").Append(vbCrLf)
        Dim v0 As String
        For Each v0 In AllStyles_Menu
            a = bp.m_cCss(v0)
            AppendStyle(bp, sOut, a.sValue, "menuhover", "", False)
        Next
        sOut.Append("cursor:pointer;").Append(vbCrLf)
        sOut.Append("}").Append(vbCrLf)
        sOut.Append("</STYLE>").Append(vbCrLf)

    End Sub

    Private Sub AppendStyle(ByRef bp As BuildPage, ByRef sOut As System.Text.StringBuilder, _
            ByVal sFrom As String, ByVal sName As String, ByVal sDir As String, ByVal bTop132 As Boolean) 'vk 05.10
        Dim s As String, t As Integer, u, v As String
        Dim bDirFound As Boolean = False
        For Each s In sFrom.Split(";")
            If s.Trim > "" Then
                t = InStr(s, ":")
                If t > 0 Then
                    u = Left(s, t - 1)
                    v = Mid(s, t + 1)
                    Select Case True
                        Case u.Trim.ToLower = "font-size" AndAlso Left(sName, 4) <> "menu"
                            sOut.Append(u).Append(":")
                            sOut.Append(CStr(bp.ResizeFont(Val(v), Left(sName, 3) = "Btn", Left(sName, 6) = "TopOut", bTop132)))
                            sOut.Append("px;").Append(vbCrLf)
                        Case Else
                            sOut.Append(s).Append(";").Append(vbCrLf)
                    End Select
                    If u.ToLower.Trim = "direction" Then
                        bDirFound = True
                    End If
                End If
            End If
        Next
        If Left(sName, 3) = "Btn" AndAlso Not bDirFound Then
            sOut.Append("direction:").Append(sDir).Append(";").Append(vbCrLf)
        End If
    End Sub

    Sub FormHeader(ByRef bp As BuildPage, ByRef sOut As System.Text.StringBuilder, _
            ByRef oNodeScreenI As Node, ByRef oNodeScreenO As Node, ByVal bPrint As Boolean, _
            ByVal nPage As Integer, ByVal nPages As Integer, ByVal bSmallWindow As Boolean, _
            ByVal bEmpty As Boolean, ByVal bPdf As Boolean)

        Dim oControlT As Control, v As String
        If bp.getProperty("Jscript") = "include" Then sOut.Append("<script>") 'vk 04.06
        For Each v In AllFiles_Js
            If bp.getProperty("Jscript") = "link" Then
                sOut.Append("<SCRIPT language='JAVASCRIPT' src='js/dds").Append(v).Append(".js'></SCRIPT>").Append(vbCrLf)
            Else
                sOut.Append(qMulti.FromFile(bp.m_sMapPath + "\js\dds" + v + ".js")).Append(vbCrLf) 'vk 04.06
            End If
        Next
        v = "Msg" + IIf(oNodeScreenI.ParamVal("flang") = 1, "Eng", "Heb")
        If bp.getProperty("Jscript") = "link" Then
            sOut.Append("<SCRIPT language='JAVASCRIPT' src='js/dds").Append(v).Append(".js'></SCRIPT>").Append(vbCrLf)
        Else
            sOut.Append(qMulti.FromFile(bp.m_sMapPath + "\js\dds" + v + ".js")).Append(vbCrLf) 'vk 04.06
        End If
        If bp.getProperty("Jscript") = "include" Then sOut.Append("</script>") 'vk 04.06
#If Not AVOIDBACK Then
        sOut.Append("<META HTTP-EQUIV='Expires' CONTENT='-1'>").Append(vbCrLf)
        sOut.Append("<META HTTP-EQUIV='Pragma' CONTENT='no-cache'>").Append(vbCrLf)
#End If
        sOut.Append("<META HTTP-EQUIV='Content-Type' CONTENT='text/html; charset=UTF-8'>").Append(vbCrLf)
        If bp.getProperty("VisualFilter") > "" Then
            sOut.Append("<META HTTP-EQUIV='Page-Exit' CONTENT='").Append(bp.getProperty("VisualFilter")).Append("'>").Append(vbCrLf)
        End If
        'If bp.getProperty("AdditionalHtml_Header") > "" Then
        '    sOut.Append(bp.getProperty("AdditionalHtml_Header") + vbCrLf) 'vk 01.14
        'End If
        If (oNodeScreenI.Param("fil").Trim = bp.MenuFile OrElse oNodeScreenO.Param("fil").Trim = bp.MenuFile) _
                AndAlso bp.getProperty("MenuStyle") = "control" Then
            sOut.Append("<script src='tree/xtree.js'></script>").Append(vbCrLf)
            sOut.Append("<link type='text/css' rel='stylesheet' href='tree/xtree.css'>").Append(vbCrLf)
        End If
        sOut.Append("<title>")
        If bPdf Then 'vk 03.09
        ElseIf oNodeScreenO.Modal AndAlso Not bp.m_bDrawModal Then
            'vk 07.06
            Dim i As Integer
            For i = 1 To 50
                sOut.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;").Append(vbCrLf)
            Next
        ElseIf bPrint Then
            sOut.Append("Printing page ").Append(CStr(nPage)).Append(" of ").Append(CStr(nPages)) 'vk 09.05, 03.06, 05.06
        ElseIf oNodeScreenO.Param("fil").Trim = bp.HelpFile Then
            sOut.Append("Help") 'vk 07.04
        Else
            sOut.Append(bp.m_sVersion)
        End If
        sOut.Append("</title>").Append(vbCrLf)
#If AVOIDBACK Then
        sOut.Append("<SCRIPT language='JavaScript'>window.history.go(1);</SCRIPT>")
#End If
        sOut.Append("</head>").Append(vbCrLf)

        If bEmpty Then Exit Sub 'vk 12.07

        Dim oStyleT As New Style 'vk 05.10
        oStyleT.Add("overflow-x", "hidden")
        If oNodeScreenI.Param("wait") = "S" AndAlso Not bPdf Then
            oStyleT.Add("overflow-y", "scroll")
        Else
            oStyleT.Add("overflow-y", "hidden")
        End If
        oStyleT.Add("margin", 0)
        oStyleT.Add("direction", "ltr")
        oControlT = New Control(oStyleT.FullCode)
        If Not bPdf Then 'vk 03.09
            If Not bp.m_bKiosk Then
                oControlT.Add("onhelp", "IgnoreKey();", True)
            End If
            oControlT.Add("onkeyup", "IgnoreKey();") ', True) 'vk 11.07, 11.15
            '!!! oControlT.Add("onkeypress", "IgnoreKey();", True) 'vk 11.07
            'vk 02.05, 05.05
            oControlT.Add("onkeydown", "fnWindowKeyDown('" +
                bp.m_sCardField + "'," +
                IIf(bp.m_sCardPer = "Y", "true", "false") + ",'" +
                bp.m_sCardPfk + "',0);")
            If oNodeScreenO.Param("wait") = "S" AndAlso oNodeScreenO.Param("plgc") <> "P" Then 'vk 02.11
                'vk 09.07
                Dim s As String = bp.getProperty(oNodeScreenO.Param("pset") + "_Margins")
                'If s = "" Or bSmallWindow Then s = "'','','',''"
                If s = "" Then s = "'','','',''"
                'vk 10.10
                oControlT.Add("onbeforeprint", "BeforePrint(" + s + ");")
                oControlT.Add("onafterprint", "AfterPrint();")
            End If
            If (oNodeScreenI.Param("fil").Trim <> bp.MenuFile OrElse bp.getProperty("SameWindow").ToLower = "true") _
                    AndAlso oNodeScreenI.Param("fil").Trim <> bp.StopFile _
                    AndAlso Not (oNodeScreenI.Modal AndAlso Not bp.m_bDrawModal) Then 'SameWindow vk 05.09
                'stable version
                oControlT.Add("onbeforeunload", "CloseMsg();") 'vk 07.05
            End If
            oControlT.Add("onfocus", bp.getProperty("FocusAction")) 'vk 03.06
        End If
        If bp.getProperty("DontSelectOnFocus") = "true" Then
            oControlT.Add("onfocus", "bDontSelect=true;") 'vk 09.11
        End If
        If Not bPdf Then 'vk 05.16
            oControlT.Add("disabled", "true") 'vk 01.16
        End If
        sOut.Append(oControlT.FullCode("body"))

        sOut.Append("<script>").Append(vbCrLf)

        '/test(bp, sOut, oNodeScreenI)

        If bp.cKbdFields.Count > 0 Then
            sOut.Append("function CopyFromKbd()").Append(vbCrLf)
            sOut.Append("{").Append(vbCrLf)
            sOut.Append("var s=document.getElementById('Hkbd').value;").Append(vbCrLf)
            sOut.Append("var s0;").Append(vbCrLf)
            Dim i As Integer, o As KbdField
            For i = 1 To bp.cKbdFields.Count
                o = bp.cKbdFields(i.ToString)
                With o
                    sOut.Append("s0=s.substr(").Append(.nFrom.ToString).Append(",").Append(.nLength.ToString).Append(");").Append(vbCrLf)
                    sOut.Append("document.getElementById(""" + .sName + """).value=s0;").Append(vbCrLf)
                    sOut.Append("document.getElementById(""S" + .sName + """).value=s0;").Append(vbCrLf)
                End With
            Next
            o = Nothing
            sOut.Append("}").Append(vbCrLf)
        End If
        If oNodeScreenI.ParamVal("sec") > 0 AndAlso oNodeScreenI.ParamVal("fwt") > 0 Then
            'vk 03.05
            sOut.Append("function PrepareKioskDialog()").Append(vbCrLf)
            sOut.Append("{").Append(vbCrLf)
            Dim sFile As String = "press" + oNodeScreenI.Param("fru") + ".htm"
            If qMulti.FromFile(bp.m_sMapPath + "\" + sFile) = "" Then sFile = "press000.htm"
            'vk 12.06
            sOut.Append("var arrArgs = new Array();").Append(vbCrLf)
            sOut.Append("arrArgs[0] = ").Append(oNodeScreenI.ParamVal("fwt").ToString).Append(";").Append(vbCrLf)
            sOut.Append("FillForModalPing(arrArgs);").Append(vbCrLf)
            'vk 12.06 till here
            sOut.Append("var rv = window.showModalDialog('").Append(sFile)
            sOut.Append("',arrArgs,'scroll:no;center:yes;status:no;help:no;dialogHeight:' + XP(400) + 'px;dialogWidth:700px;');").Append(vbCrLf)
            sOut.Append("if (rv) {document.getElementById('Hfcmd').value='").Append(oNodeScreenI.Param("esc")).Append("';doSubmit();}").Append(vbCrLf)
            sOut.Append("else setTimeout(""PrepareKioskDialog();"",")
            sOut.Append(Val(oNodeScreenI.Param("sec")).ToString).Append("000);").Append(vbCrLf)
            sOut.Append("}").Append(vbCrLf)
        End If
        sOut.Append("</script>").Append(vbCrLf)
        sOut.Append("<form method='post' action='screen.aspx'>").Append(vbCrLf)
        If bp.getProperty("AdditionalHtml_Form") > "" AndAlso Not bPdf Then
            sOut.Append(bp.getProperty("AdditionalHtml_Form")).Append(vbCrLf) 'vk 02.20
            '<button id='msbug' name='msbug' type='submit' style='height:0;width:0;' onclick='return bInSubmit;'></button>
        End If

    End Sub

    Sub FormForms(ByRef bp As BuildPage, ByRef sOut As System.Text.StringBuilder, _
            ByRef oNodeScreen As Node, ByVal sGuidForWnet As String, ByVal sUrlForWnet As String) 'vk 12.04

        sOut.Append("</form>").Append(vbCrLf).Append("<DIV style='display:none;'>").Append(vbCrLf)
        If oNodeScreen.Param("fgr") = "C" Then
            'vk 08.04
            sOut.Append("<form id='frmModal_Calc'>").Append(vbCrLf)
            Dim v As String
            For Each v In bp.CALC_COLUMNS
                sOut.Append(qMulti.Hidden("Modal_" + v))
            Next
            sOut.Append(qMulti.Hidden("Modal_sDb", oNodeScreen.Param("flib").Trim))
            sOut.Append(qMulti.Hidden("Modal_sClass"))
            sOut.Append(qMulti.Hidden("Modal_sLine"))
            sOut.Append(qMulti.Hidden("Modal_sLang", IIf(oNodeScreen.ParamVal("flang") = 1, "Eng", "Heb").ToString))
            sOut.Append("</form>").Append(vbCrLf)
        ElseIf oNodeScreen.Param("adr") = "R" Then 'elseif vk 11.04
            'vk 05.04
            sOut.Append("<FORM id='frmModal_Adr' target='ifr100' method='post' action='CityStreet.aspx'>").Append(vbCrLf) 'vk 05.09
            Dim v As String
            For Each v In AllFields_CityStreet
                sOut.Append(qMulti.Hidden("Modal_" + v))
            Next
            sOut.Append(qMulti.Hidden("Modal_sColor", bp.m_inp_bkg))
            sOut.Append(qMulti.Hidden("Modal_sDb", oNodeScreen.Param("flib").Trim))
            sOut.Append("</form>").Append(vbCrLf)
        End If
        If bp.m_sModelCombosFill > "" Then
            'vk 05.05
            sOut.Append("<IFRAME id='ifrModelsCombo' name='ifrModelsCombo' src='Empty.htm' width='0' height='0'></IFRAME>").Append(vbCrLf)
            sOut.Append("<FORM id='frmModelsCombo' target='ifrModelsCombo' method='post' action='BringM.aspx'>").Append(vbCrLf)
            sOut.Append(qMulti.Hidden("hDb", oNodeScreen.Param("flib").Trim))
            Dim v As String
            For Each v In AllFields_Model1
                sOut.Append(qMulti.Hidden(v))
            Next
            sOut.Append("</form>").Append(vbCrLf)
            sOut.Append(qMulti.Hidden("hResult", "")) ', "onpropertychange", "modellist2();"))
            For Each v In AllFields_Model2
                sOut.Append(qMulti.Hidden(v))
            Next
        End If
        If sUrlForWnet > "" Then
            'vk 01.08
            sOut.Append("<IFRAME id='ifrWnet' name='ifrWnet' src='Empty.htm' width='0' height='0'></IFRAME>").Append(vbCrLf)
            sOut.Append("<form id='frmWnet' target='ifrWnet' method='post' action='CheckMq.aspx'>").Append(vbCrLf)
            sOut.Append(qMulti.Hidden("hMq", bp.getProperty("MqForWnet")))
            sOut.Append(qMulti.Hidden("hKey", sGuidForWnet))
            sOut.Append(qMulti.Hidden("hTimeOut", bp.getProperty("LocalTimeoutForWnet")))
            sOut.Append(qMulti.Hidden("hMqComment"))
            sOut.Append(qMulti.Hidden("hBefore", "v"))
            sOut.Append("</form>").Append(vbCrLf)
            sOut.Append(qMulti.Hidden("hResult", "")) ', "onpropertychange", "wnet_answer();"))
        End If
        'vk 07.07
        'Dim oFlexCombo As FlexCombo
        'For Each oFlexCombo In bp.cFlexCombos
        '    If Not (oFlexCombo.bBoss OrElse oFlexCombo.bDouble) Then
        '        sOut.Append("<IFRAME id='ifrFlexCombo_").Append(oFlexCombo.sN).Append("' name='ifrFlexCombo_").Append(oFlexCombo.sN).Append("' src='Empty.htm' width='0' height='0'></IFRAME>").Append(vbCrLf)
        '        sOut.Append("<form id='frmFlexCombo_").Append(oFlexCombo.sN).Append("' target='ifrFlexCombo_").Append(oFlexCombo.sN).Append("' method='post' action='BringF.aspx'>").Append(vbCrLf)
        '        sOut.Append(qMulti.Hidden("hDb", oNodeScreen.Param("flib").Trim))
        '        sOut.Append(qMulti.Hidden("hKod", oFlexCombo.sKod))
        '        Dim v As String
        '        For Each v In AllFields_FlexCombo1
        '            sOut.Append(qMulti.Hidden(v))
        '        Next
        '        sOut.Append("</form>").Append(vbCrLf)
        '        sOut.Append(qMulti.Hidden("hResult_" + oFlexCombo.sN, "")) ', "onpropertychange", "flexlist2('" + oFlexCombo.sN + "');"))
        '        For Each v In AllFields_FlexCombo2
        '            sOut.Append(qMulti.Hidden(v + "_" + oFlexCombo.sN))
        '        Next
        '    End If
        'Next
        'vk 03.09, 12.09
        If bp.m_sImageMethod = "IF" OrElse bp.m_sImageMethod = "IP" Then
            Dim a, b(), sUrl, sParams As String
            sParams = bp.m_sCommand
            If bp.m_sImageMethod = "IF" Then
                sUrl = bp.getPropertyHttp("ImagePath")
            Else
                Dim j As Integer = InStr(sParams, "?")
                If j = 0 Then
                    sUrl = sParams
                    sParams = ""
                Else
                    sUrl = Left(sParams, j - 1)
                    sParams = Mid(sParams, j + 1)
                End If
            End If
            sOut.Append("<form id='frmImage' target='IFN' method='post' action='")
            sOut.Append(sUrl)
            sOut.Append("' onsubmit='window.open("""",""IFN"")'>").Append(vbCrLf)
            For Each a In sParams.Split("&")
                b = a.Split("=")
                Select Case b.Length
                    Case 1
                        sOut.Append(qMulti.Hidden(b(0).Trim))
                    Case Is >= 2
                        sOut.Append(qMulti.Hidden(b(0).Trim, b(1).Trim))
                End Select
            Next
            sOut.Append("</form>").Append(vbCrLf)
        End If
        'vk 11.09
        Dim i As Integer, s As String
        For i = 1 To 9
            Try
                s = bp.cAdrPer(i.ToString)
                sOut.Append(qMulti.Hidden("Hadr" + i.ToString, s))
            Catch
            End Try
        Next
        If bp.m_sArchiveDescr > "" Then
            'vk 06.13
            sOut.Append("<IFRAME id='ifrArchive' name='ifrArchive' src='Upload.aspx' width='0' height='0'></IFRAME>").Append(vbCrLf)
            sOut.Append(qMulti.Hidden("HarchiveResult", "")) ', "onpropertychange", "ArchiveAfterSubmit();"))
            sOut.Append(qMulti.Hidden("HarchiveKey", bp.ArchiveKey))
            'sOut.Append(qMulti.Hidden("HarchiveWhat", bp.m_sArchiveWhat)) 'vk 03.20
            sOut.Append(qMulti.Hidden("HarchiveDescr", bp.m_sArchiveDescr))
            sOut.Append(qMulti.Hidden("HarchiveDB", oNodeScreen.Param("flib"))) 'vk 07.17
            sOut.Append(qMulti.Hidden("HarchiveDocType")) 'vk 07.17
            sOut.Append(qMulti.Hidden("HarchiveSystem", bp.m_sSystem)) 'vk 12.18
            'vk 07.13
            sOut.Append("<IFRAME id='ifrArchiveCheck' name='ifrArchiveCheck' src='Empty.htm' width='0' height='0'></IFRAME>").Append(vbCrLf)
            sOut.Append("<FORM id='frmArchiveCheck' target='ifrArchiveCheck' method='post' action='CheckFile.aspx'>").Append(vbCrLf)
            sOut.Append(qMulti.Hidden("HarchiveKeyAndName"))
            sOut.Append(qMulti.Hidden("HarchiveWhat", bp.m_sArchiveWhat)) 'vk 03.20
            sOut.Append("</FORM>").Append(vbCrLf)
            sOut.Append(qMulti.Hidden("HarchiveCheckResult", "")) ', "onpropertychange", "ArchiveAfterCheck();"))
        End If
        If bp.m_sCG > "" Then
            'vk 07.13
            sOut.Append("<IFRAME id='ifrCG' name='ifrCG' src='Empty.htm' width='0' height='0'></IFRAME>")
            sOut.Append("<FORM id='frmCG' name='frmCG' target='ifrCG' method='post' action='Call.aspx'>")
            If bp.getProperty("WsApi") = "ESB" Then
                sOut.Append(qMulti.Hidden("txtWhatCG", "ESB1")) 'vk 04.21
            Else
                sOut.Append(qMulti.Hidden("txtWhatCG"))
            End If
            For i = 1 To 9
                'vk 08.13
                'sOut.Append(qMulti.Hidden("Hv" + CStr(i) + "_value"))
                sOut.Append(qMulti.Hidden("Hi" + CStr(i) + "_value"))
                sOut.Append(qMulti.Hidden("Hj" + CStr(i) + "_value"))
            Next
            sOut.Append("</FORM>")
            sOut.Append(qMulti.Hidden("txtResultCG", "")) ', "onpropertychange", "CG2();"))
        End If
        If oNodeScreen.Param("fgr") = "T" Then
            'vk 11.15
            sOut.Append("<IFRAME id='ifrSql' name='ifrSql' src='Empty.htm' width='0' height='0'></IFRAME>")
            sOut.Append("<form id='frmSql' name='frmSql' target='ifrSql' method='post' action='Sql.aspx'>")
            sOut.Append(qMulti.Hidden("txtRecordsTotal", "0"))
            sOut.Append("</form>")
            sOut.Append(qMulti.Hidden("txtWhatSql"))
            sOut.Append(qMulti.Hidden("txtResultSql", "")) ', "onpropertychange", "Sql2();"))
        End If

        'vk 12.06
        'If oNodeScreen.ParamExists("xxx") Then
        '    sOut.Append("<IFRAME id='ifrMailScreen' name='ifrMailScreen' src='Empty.htm' width='0' height='0'></IFRAME>" + vbCrLf)
        '    sOut.Append("<FORM id='frmMailScreen' target='ifrMailScreen' method='post' action='MailScreen.aspx'>" + vbCrLf)
        '    sOut.Append(qMulti.Hidden("hTo", oNodeScreen.Param("xxx").Trim))
        '    sOut.Append("</FORM>" + vbCrLf)
        '    sOut.Append(qMulti.Hidden("hResult", ""))', "onpropertychange", "modellist2();"))
        'End If
        'vk 12.04
        sOut.Append(qMulti.Hidden("PingNumber", "")) ', "onpropertychange", "bInPing=false;")) 'vk 11.06
        sOut.Append("<IFRAME id='ifrPing' name='ifrPing' src='Empty.htm' width='0' height='0'></IFRAME>").Append(vbCrLf)
        sOut.Append("<form id='frmPing' action='Ping.aspx' method='post' target='ifrPing'></form>").Append(vbCrLf)
        'vk 01.06
        sOut.Append("<IFRAME id='ifrRefresh' name='ifrRefresh' src='Empty.htm' width='0' height='0'></IFRAME>").Append(vbCrLf)
        sOut.Append("</DIV></body></html>")

    End Sub

    Sub FormFunction(ByRef bp As BuildPage, ByRef sOut As System.Text.StringBuilder, _
            ByVal winI As String, ByVal wlinI As String, ByVal wlstI As String, _
            ByVal wcolI As String, ByVal wcstI As String, _
            ByRef oNodeScreenI As Node, ByRef oNodeScreenO As Node, _
            ByVal bSmallWindow As Boolean, ByVal bPrint As Boolean, _
            ByVal sUrlForWnet As String, ByVal bFirstScreen As Boolean, ByVal bToken As Boolean)

        If (winI = "Y" OrElse winI = "S" OrElse winI = "C") AndAlso bp.getProperty("ModalWindows") = "dialog" Then 'vk 01.09
            ' Modal Window
            Dim iwlstI As Integer = qStyle.ilin_fun_netto(bp, Int32.Parse(wlstI) + 1, oNodeScreenI) 'ms+vk 06.05
            Dim iwcstI As Integer = Int32.Parse(wcstI) * (bp.iScaleWidth + 1)
            Dim iwlinI As Integer
            Dim iwcolI As Integer
            If oNodeScreenI.Ortogonal(bp) Then 'ms+vk 06.05
                iwlinI = (Int32.Parse(wlinI) + 1) * bp.iScaleLin + bp.iScaleLin_Btn * 2 + 30 '30 vk 07.05
                iwcolI = Int32.Parse(wcolI) * bp.iScaleWidth
                If oNodeScreenI.ParamIn("fsid", "LMOP") Then iwcstI += bp.iMarginLeftCom 'vk 05.07
            Else
                iwlinI = (Int32.Parse(wlinI) + 1) * bp.iScaleLin
                iwcolI = Int32.Parse(wcolI) * bp.iScaleWidth + bp.iMarginRightMod
                If Not (bp.m_bHelp AndAlso bp.m_nCall = 2) Then iwcolI += bp.iMarginLeftCom 'vk 07.04
            End If

            Dim swStat As String = "scroll : no; center : no; status : no; help : no; " + _
                "dialogTop : " + iwlstI.ToString + "px; dialogLeft : " + iwcstI.ToString + "px; " + _
                "dialogHeight : ' + XP(" + iwlinI.ToString + ") + 'px; dialogWidth : " + iwcolI.ToString + "px; "
            sOut.Append(bp.SCRIPT_START_1 + vbCrLf)
            If bp.m_bHelp AndAlso bp.m_nCall = 2 Then 'vk 07.04
                sOut.Append("body_load('" + bp.getProperty("Copyright") + "');" + vbCrLf)
            Else
                FormFunction_Init(bp, sOut, oNodeScreenI)
                sOut.Append("DisableButtons();").Append(vbCrLf)
            End If
            sOut.Append("document.getElementById('Hx').value = '';").Append(vbCrLf)
            sOut.Append("var swStat='").Append(swStat).Append("';").Append(vbCrLf)
            If bp.m_bHelp AndAlso bp.m_nCall = 2 Then
                'vk 07.04
                sOut.Append("var rv = window.showModalDialog('screenMod.aspx','',swStat);").Append(vbCrLf)
                FormFunction_Common(bp, sOut, oNodeScreenI, oNodeScreenO, bSmallWindow, sUrlForWnet, _
                    bFirstScreen, bToken)
            Else
                sOut.Append("ReplyToModal('").Append(swStat).Append("');").Append(vbCrLf) 'vk 02.06
            End If
            sOut.Append(bp.SCRIPT_FINISH_1).Append(vbCrLf)

        Else
            ' Common window
            sOut.Append(bp.SCRIPT_START_1).Append(vbCrLf)
            FormFunction_Common(bp, sOut, oNodeScreenI, oNodeScreenO, bSmallWindow, sUrlForWnet, _
                bFirstScreen, bToken) 'vk 07.04
            sOut.Append(bp.SCRIPT_FINISH_1).Append(vbCrLf)
        End If
        'vk 03.07
        'sOut.Append(bp.SCRIPT_START_2).Append(vbCrLf)
        'If oNodeScreenI.Param("fil").Trim <> bp.StopFile _
        '        AndAlso oNodeScreenO.Param("fil").Trim <> bp.HelpFile _
        '        AndAlso Not bPrint AndAlso Not oNodeScreenO.Modal Then 'AndAlso bp.getProperty("ModalWindows") = "draw" 'vk 08.09, 09.09
        '    'stable version
        '    sOut.Append("OnClose();").Append(vbCrLf)
        '    'debug for chrome
        '    'sOut.Append("DdsDebug('window.unload');").Append(vbCrLf)
        'End If
        'sOut.Append(bp.SCRIPT_FINISH_2).Append(vbCrLf)

    End Sub

    Private Sub FormFunction_Common(ByRef bp As BuildPage, ByRef sOut As System.Text.StringBuilder, _
            ByRef oNodeScreenI As Node, ByRef oNodeScreenO As Node, _
            ByVal bSmallWindow As Boolean, ByVal sUrlForWnet As String, _
            ByVal bFirstScreen As Boolean, ByVal bToken As Boolean) 'vk 07.04

        sOut.Append("if (InGet()) {document.body.disabled=false;return;}").Append(vbCrLf) 'vk 06.10, 01.16
        sOut.Append("AdjustW(")
        sOut.Append(IIf(bp.getProperty("AdjustW").ToLower.Trim = "true", "true", "false").ToString).Append(",")
        sOut.Append(IIf(bp.m_sTextPrl = "Y" OrElse oNodeScreenI.ParamVal("flr") <> 1, "true", "false").ToString).Append(");").Append(vbCrLf) 'vk 06.10
        'If bp.getProperty("AvoidMargins").ToLower = "true" Then
        '    sOut.Append("MaxWidth=" + CStr(bp.m_nResolutionW) _
        '        + ";MaxHeight=" + CStr(bp.m_nResolutionH) _
        '        + ";ResizeWindow();" + vbCrLf) 'vk 10.11
        'Else
        If bFirstScreen AndAlso (bp.getProperty("SameWindow").ToLower <> "true" OrElse bToken) Then 'vk 05.12
            'bp.getProperty("ResizeWindow").ToLower <> "false" _
            'AndAlso bp.getProperty("ForceWinSize").ToLower <> "true" Then 'vk 06.10
            'sOut.Append("GetSize();ResizeWindow();" + vbCrLf) 'vk 05.10
            sOut.Append("ResizeWindow(").Append(CStr(bp.m_nResolutionW)).Append(",").Append(CStr(bp.m_nResolutionH)).Append(");").Append(vbCrLf) 'vk 01.12
        End If
        'If bp.getProperty("ModalWindows") <> "dialog" OrElse Not oNodeScreenI.Modal Then
        '    sOut.Append("ShowScroll(" + CStr(bp.m_nResolutionW) + "," + CStr(bp.m_nResolutionH) + ");" + vbCrLf) 'vk 05.10
        'End If
        'If oNodeScreenI.Param("wait") = "S" AndAlso oNodeScreenI.ParamVal("flr") = 0 Then
        '    sOut.Append("with (document.body) scrollLeft=scrollWidth-clientWidth;" + vbCrLf) 'vk 01.04
        'End If
        sOut.Append("ShowScroll(").Append(CStr(bp.m_nResolutionW)).Append(",").Append(CStr(bp.m_nResolutionH)).AppendFormat(",")
        sOut.Append(IIf(oNodeScreenI.ParamVal("flr") = 0, "true", "false")).Append(");").Append(vbCrLf)
        sOut.Append(bp.m_sMoveSmth) 'vk 07.07
        sOut.Append("if (document.getElementById('leftbuttons')) document.getElementById('leftbuttons').style.visibility='visible';").Append(vbCrLf)
        If bp.m_bTray Then
            sOut.Append("DrawTrays();").Append(vbCrLf)
        End If
        If bp.getProperty("Focus") <> "false" Then 'vk 10.07
            sOut.Append("focus();").Append(vbCrLf)
        End If
        FormFunction_Init(bp, sOut, oNodeScreenI)
        If bp.m_sClickLine <> "" AndAlso bp.cLinChecked.Count = 0 Then '=0 ms+vk 06.05
            sOut.Append("fnClickGridByLine('").Append(bp.m_sClickLine).Append("');").Append(vbCrLf)
        End If
        If bp.cKbdFields.Count > 0 Then
            sOut.Append("document.getElementById('Kbd1').focus();").Append(vbCrLf)
        ElseIf bp.m_sNameTab > "" Then
            sOut.Append("fnFocus_tab(document.getElementById('").Append(bp.m_sNameTab).Append("'),false,'',true);").Append(vbCrLf)
        ElseIf bp.m_sNameFocus > "" Then
            sOut.Append("fnCur('").Append(bp.m_sNameFocus).Append("');").Append(vbCrLf)
        ElseIf bp.cTabs.Count = 0 Then
            If bp.getProperty("Focus") <> "false" Then 'vk 10.07
                sOut.Append("try{document.body.focus();}catch(e){;}").Append(vbCrLf)
            End If
        Else
            'vk 09.06
            Dim oTabIndex As TabIndex, nTab As Integer
            oTabIndex = bp.cTabs(1)
            nTab = oTabIndex.Tab
            For Each oTabIndex In bp.cTabs
                If oTabIndex.Tab <= nTab Then
                    nTab = oTabIndex.Tab
                    bp.m_sNameFocus = oTabIndex.ControlName
                End If
            Next
            If bp.m_sNameFocus = "" Then
                sOut.Append("document.body.focus();").Append(vbCrLf)
            Else
                sOut.Append("fnCur('").Append(bp.m_sNameFocus).Append("');").Append(vbCrLf)
            End If
        End If
        If bp.m_sCardField > "" Then
            'vk 02.05
            sOut.Append("sCard='';").Append(vbCrLf)
            sOut.Append("bNewCard=true;").Append(vbCrLf)
        End If
        If Not bp.m_bKiosk Then
            sOut.Append("body_load_tab();").Append(vbCrLf)
            sOut.Append("setMsgDir();").Append(vbCrLf)
        End If
        sOut.Append("body_load('").Append(bp.getProperty("Copyright")).Append("');").Append(vbCrLf)
        If bp.getProperty("TouchButtons") = "kiosk" Then
            sOut.Append("SetButtonCaptions(true);").Append(vbCrLf)
        Else
            sOut.Append("SetButtonCaptions(false);").Append(vbCrLf)
        End If

        '/ sOut.Append("gIsEnable=true;").Append(vbCrLf) 'vk 05.04

        Select Case oNodeScreenI.Param("fil").Trim
            Case bp.StopFile
                sOut.Append("CloseWindow(").Append(bp.getProperty("CloseMsgSeconds")).Append(");").Append(vbCrLf)
                bp.m_bStopSession = True 'vk 12.04
            Case bp.TestFile
                sOut.Append("setTimeout(""TestSubmit();"",250);").Append(vbCrLf)
        End Select
        If bp.cKbdFields.Count > 0 Then
            sOut.Append("KbdButtons_lang('").Append(bp.m_sKbdRus).Append("');").Append(vbCrLf)
        End If
        If bp.getProperty("PingMaxCount") <> "" Then 'vk 04.05
            sOut.Append("StartPing();").Append(vbCrLf)
        End If

        If oNodeScreenI.Param("fps") = "P" Then
            sOut.Append("window.print();").Append(vbCrLf)
        End If
        Select Case oNodeScreenI.Param("plgc")
            Case "1" 'vk 08.05, 08.10
                sOut.Append("try{").Append(IIf(bp.getProperty("Frame") = "true", "parent.", ""))
                sOut.Append("document.getElementById(""WebBrowser1"").ExecWB(6,2);}catch(e){MyMsgBox(e.message);}").Append(vbCrLf)
                sOut.Append("document.getElementById('NotValid').style.display='block';").Append(vbCrLf)
            Case "2", "A" 'vk 09.05, 05.06
                If bSmallWindow Then
                    sOut.Append("PrintAndClose();").Append(vbCrLf)
                End If
        End Select
        sOut.Append(bp.m_sModelCombosFill) 'vk 05.05
        'vk 07.07
        'Dim oFlexCombo, oFlexComboBoss As FlexCombo
        'For Each oFlexCombo In bp.cFlexCombos
        '    If Not (oFlexCombo.bBoss OrElse oFlexCombo.bDouble) Then
        '        For Each oFlexComboBoss In bp.cFlexCombos
        '            If oFlexComboBoss.sGroup = oFlexCombo.sGroup AndAlso oFlexComboBoss.bBoss Then
        '                sOut.Append("flexlist1('").Append(oFlexComboBoss.sN).Append("','").Append(oFlexCombo.sN).Append("',false);").Append(vbCrLf)
        '            End If
        '        Next
        '    End If
        'Next
        'vk 03.06
        If bp.m_sCommand > "" Then
            Select Case bp.m_sImageMethod
                Case "II" : sOut.Append("window.open('" + bp.getPropertyHttp("ImagePath") _
                    + "?" + bp.m_sCommand + "');" + vbCrLf)
                Case "IF", "IP" : sOut.Append("document.getElementById('frmImage').submit();").Append(vbCrLf)
                Case "IU" : sOut.Append("window.open('" + bp.m_sCommand + "');" + vbCrLf) 'vk 10.09
                Case "SI" : sOut.Append("window.open('" + bp.getPropertyHttp("ScanPath") _
                    + "?" + bp.m_sCommand + "');" + vbCrLf) 'vk 12.12
                Case "M" : sOut.Append("window.open('" + bp.getPropertyHttp("ImagePath_M") _
                    + "?mask=" + bp.m_sCommand + "*.*');" + vbCrLf) 'vk 06.12
            End Select
        End If
        If sUrlForWnet > "" Then 'vk 03.08
            sOut.Append("window.open('" + sUrlForWnet.Replace("\", "\\") + "');" + vbCrLf)
            sOut.Append("wnet_ask(" + bp.getProperty("TimeOutForWnet") + ",'" + _
                sUrlForWnet.Replace("\", "\\").Replace("'", "\'") + "');" + vbCrLf)
        ElseIf oNodeScreenI.Param("dll") <> " " Then
            sOut.Append(bp.getProperty("DllAction_" + oNodeScreenI.Param("dll")) + vbCrLf)
        End If
        'vk 10.07
        'If oNodeScreenI.ParamExists("eps") Then
        '    sOut.Append("eps('" + oNodeScreenI.Param("eps") + "');" + vbCrLf)
        'ElseIf oNodeScreenI.Param("eps") > "" Then
        '    sOut.Append("MyMsgBox(msg(21));" + vbCrLf)
        'End If
        'vk 12.12
        If oNodeScreenI.Param("sic") = "S" AndAlso oNodeScreenI.Param("img") = "K" Then
            sOut.Append("artis('" + bp.getProperty("ArtisFile") + _
                "', '" + bp.getProperty("ArtisExe") + "');" + vbCrLf)
        End If
        If bp.m_sErrorForClient > "" Then
            sOut.Append("MyMsgBox('Error in received data: " + bp.m_sErrorForClient + "');" + vbCrLf)
        End If
        'vk 04.20
        If bp.m_sCG > "" Then
            If bp.getProperty("WsApi") = "ESB" Then
                sOut.Append("CG_ESB();" + vbCrLf) 'vk 04.21
            Else
                sOut.Append("CG1();" + vbCrLf)
            End If
        End If
        'moved here vk 10.07
        If bp.m_bPrintServer AndAlso bp.m_nPagesToSend > 0 Then 'bPrintServer vk 01.06
            sOut.Append("DisableButtons();" + vbCrLf)
            sOut.Append("bPrintServerFromButton=false;" + vbCrLf)
            sOut.Append("PrintServer(1," + bp.m_nPagesToSend.ToString + _
                "," + oNodeScreenI.ParamVal("sec").ToString + _
                ",'" + oNodeScreenI.Param("esc").ToString + "');" + vbCrLf) 'vk 09.05
        ElseIf oNodeScreenI.ParamVal("sec") > 0 Then
            sOut.Append("setTimeout(""")
            If oNodeScreenI.ParamVal("fwt") > 0 Then
                sOut.Append("PrepareKioskDialog();")
            Else
                sOut.Append("document.getElementById('Hfcmd').value='").Append(oNodeScreenI.Param("esc")).Append("';doSubmit();")
            End If
            sOut.Append("""," + Val(oNodeScreenI.Param("sec")).ToString + "000);" + vbCrLf)
        End If

    End Sub

    Private Sub FormFunction_Init(ByRef bp As BuildPage, ByRef sOut As System.Text.StringBuilder, _
            ByRef oNodeScreen As Node) 'vk 07.04

        sOut.Append("InitConst(" + bp.numL.ToString + "," + _
            bp.indL.ToString + "," + bp.typL.ToString + "," + _
            bp.lenL.ToString + "," + bp.decL.ToString + "," + _
            bp.numS.ToString + ");" + vbCrLf)
        sOut.Append("if (document.body.disabled) document.body.disabled=false;" + vbCrLf) 'vk 03.06
        sOut.Append("if (document.getElementById('logo')) document.getElementById('logo').disabled=false;" + vbCrLf) 'vk 03.06
        sOut.Append("fnFocusQfk('');" + vbCrLf)
        sOut.Append("SetQflx(" + _
            IIf(oNodeScreen.Param("qflx") Is Nothing, "false", "true") + ");" + vbCrLf)
        'If oNodeScreen.ParamExists("xxx") Then
        '    sOut.Append("document.forms.frmMailScreen.submit();" + vbCrLf) 'vk 12.06
        'End If

    End Sub

    Sub Dispose()
        qStyle = Nothing
        qMulti.Dispose()
        qMulti = Nothing
    End Sub

End Class
