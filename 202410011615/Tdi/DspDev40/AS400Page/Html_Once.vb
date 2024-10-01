Friend Class Once

    Private qSecurity As New Security()
    Private qMulti As New Multi()
    Private qConv As New Conv()
    Private qStyle As New StyleModule()

    Sub FormTextarea(ByRef bp As BuildPage, ByRef sOut As System.Text.StringBuilder, ByRef oNodeScreen As Node)

        Dim i As Integer
        For i = 1 To oNodeScreen.ParamVal("tmx")
            sOut.Append(qMulti.Hidden("W_" + i.ToString("0000")))
        Next
        sOut.Append(qMulti.Hidden("Htextlen", bp.m_nTextLen.ToString("00000")))
        sOut.Append(qMulti.Hidden("Htextcoumax", oNodeScreen.ParamVal("tmx").ToString))
        sOut.Append(qMulti.Hidden("Htextused", "0"))

        Dim oNode As New Node()

        oNode.Param("pht") = (oNodeScreen.ParamVal("wlin") - 2).ToString("000")
        oNode.Param("inp") = "I"
        oNode.Param("typ") = "A"
        If bp.m_sTextPrl > "" Then
            oNode.Param("prl") = bp.m_sTextPrl
        Else
            oNode.Param("prl") = IIf(oNodeScreen.ParamVal("flr") = 1, "N", "Y").ToString()
        End If
        oNode.Param("pfn") = "008"

        Dim oControl As New Control()

        oControl.Add("rows", oNodeScreen.ParamVal("tmx"))

        bp.m_sCurClass = "form-control col"

        Select Case bp.m_sTextPrl
            Case "Y" : oControl.Add("dir", "rtl")
            Case "N" : oControl.Add("dir", "ltr")
            Case Else
                oControl.Add("dir", IIf(oNodeScreen.ParamVal("flr") = 1, "ltr", "rtl").ToString())
        End Select

        oControl.AddId("W")
        oControl.Add("onfocus", "fnFocus(this,0);")
        oControl.Add("onfocus", "bDontSelect=false;") 'vk 09.11
        sOut.Append(oControl.FullCode(bp, "textarea", False))

        Dim sAccum As String = ""
        For i = 1 To bp.cText.Count
            Dim s As String = bp.cText(i) + " "
            Dim sSymbol As String
            If i = bp.cText.Count Then 'OrElse Len(s) = Len(RTrim(s)) Then
                sSymbol = ""
            ElseIf Trim(s) = "" OrElse InStr(bp.getProperty("ParagraphSymbols"), Right(Trim(s), 1)) > 0 Then
                sSymbol = vbCrLf
            Else
                Dim sNext As String = bp.cText(i + 1) + " "
                If Trim(sNext) = "" Then
                    sSymbol = vbCrLf
                Else
                    Select Case InStr(Len(sNext) - Len(LTrim(sNext)) + 1, sNext, " ")
                        Case 0, Is > Len(s) - Len(RTrim(s))
                            sSymbol = " "
                        Case Else
                            sSymbol = vbCrLf
                    End Select
                End If
            End If
            qConv.MakeOk(s)
            sAccum += RTrim(s) + sSymbol 'RTrim vk 03.08
        Next
        Do
            If sAccum Like "*" + vbCrLf Then
                sAccum = Left(sAccum, Len(sAccum) - 2)
            ElseIf sAccum Like "* " Then
                sAccum = Left(sAccum, Len(sAccum) - 1)
            Else
                Exit Do
            End If
        Loop

        sOut.AppendLine(sAccum + "</textarea>")
        bp.m_sNameFocus = "W"
    End Sub

    Function IsFlexiblePage(fil As String, rec As String) As Boolean
        Return (fil = "HFFLNAVFM" AndAlso rec = "RLISTDDS")
    End Function


    Sub FormHiddens(ByRef bp As BuildPage, ByRef sOut As Text.StringBuilder, ByRef oNodeScreen As Node, ByVal sScreenNum As String,
            ByVal sLastEntry As String, ByVal sWarning As String, ByVal sCommand As String, ByVal bFirstScreen As Boolean, ByVal m_ModalScreen As String, ByVal IsModalWindow As Boolean) 'vk 12.07, 03.13, 02.23

        Dim v As String = ""
        For Each v In AllHiddens
            sOut.Append(qMulti.Hidden("H" + v))
        Next
        For Each v In AllHiddens_Value
            sOut.Append(qMulti.Hidden("H" + v, oNodeScreen.Param(v)))
        Next
        If bp.getProperty("FilRec") = "hidden" Then 'vk 05.09
            For Each v In AllHiddens_FilRec
                sOut.Append(qMulti.Hidden("H" + v, oNodeScreen.Param(v)))
            Next
        Else
            For Each v In AllHiddens_FilRec
                sOut.Append(qMulti.Hidden("H" + v, "x"))
            Next
        End If
        bp.m_sXmlFil = oNodeScreen.Param("fil")
        bp.m_sXmlRec = oNodeScreen.Param("rec")
        'vk 08.22
        If bp.m_sXmlFil Is Nothing Then bp.m_sXmlFil = ""
        If bp.m_sXmlRec Is Nothing Then bp.m_sXmlRec = ""

        If oNodeScreen.Param("qpxl") Is Nothing OrElse m_ModalScreen > "" Then 'ntg 07.05.23 fixing teufa
            sOut.Append(qMulti.Hidden("Hqpxl", ""))
        Else
            sOut.Append(qMulti.Hidden("HInputQpxl", oNodeScreen.ParamVal("qpxl").ToString))
            sOut.Append(qMulti.Hidden("Hqpxl", oNodeScreen.ParamVal("qpxl").ToString))
        End If
        sOut.Append(qMulti.Hidden("Haccords", "")) 'vk 09.24

        sOut.Append(qMulti.Hidden("HFlexible", If(IsFlexiblePage(bp.m_sXmlFil.Trim(), bp.m_sXmlRec.Trim()), 1, 0)))

        sOut.Append(qMulti.Hidden("Hx", "x"))
        sOut.Append(qMulti.Hidden("Hforbidden", qSecurity.ForbiddenList(bp, oNodeScreen)))
        sOut.Append(qMulti.Hidden("Hscreen", sScreenNum))
        sOut.Append(qMulti.Hidden("Hspool", IIf(oNodeScreen.Param("wait") = "S", "true", "false"))) 'vk 01.07
        sOut.Append(qMulti.Hidden("Hpath_Comtec", bp.getPropertyHttp(v + "Path")))
        sOut.Append(qMulti.Hidden("Hfnn", oNodeScreen.Param("fnn")))
        sOut.Append(qMulti.Hidden("Hsynchr", bp.m_sSynchr))

        sOut.Append(qMulti.Hidden("HModalWindow", If(m_ModalScreen <> "", "true", "false"))) 'vk 02.23 'ntg 28.02.24 removed from comment (was needed for מסך הצהרת סוכן)
        'sOut.Append(qMulti.Hidden("HModalWindow", If(oNodeScreen.Param("fil").Trim = "HFMELELFM" And oNodeScreen.Param("rec").Trim = "RISURMVTH", "true", "false"))) 'vk 02.23 'ntg 28.02.24 add comment (was problemtaic for מסך הצהרת סוכן)

        If oNodeScreen.Param("win") <> "N" Then
            sOut.Append(qMulti.Hidden("Hgridpos", "leave"))
        Else
            sOut.Append(qMulti.Hidden("Hgridpos", "delete"))
        End If
        If oNodeScreen.Param("agr") = "P" Then
            sOut.Append(qMulti.Hidden("Hrdf", oNodeScreen.Param("rdf")))
        Else
            sOut.Append(qMulti.Hidden("Hrdf", " "))
        End If

        If sWarning > "" Then
            sOut.Append(qMulti.Hidden("Hlastentry", sWarning)) 'vk 05.12
        ElseIf sLastEntry = "" OrElse oNodeScreen.Param("fil").Trim = bp.StopFile Then
            sOut.Append(qMulti.Hidden("Hlastentry"))
        Else
            Dim s As String
            Try
                Dim d As Date = DateSerial(Val(Mid(sLastEntry, 1, 4)), Val(Mid(sLastEntry, 5, 2)), Val(Mid(sLastEntry, 7, 2)))
                d = DateAdd(DateInterval.Hour, Val(Mid(sLastEntry, 9, 2)), d)
                d = DateAdd(DateInterval.Minute, Val(Mid(sLastEntry, 11, 2)), d)
                d = DateAdd(DateInterval.Second, Val(Mid(sLastEntry, 13, 2)), d)
                s = d.ToString(bp.getProperty("DateTimeFormat"))
            Catch
                s = sLastEntry
            End Try
            sOut.Append(qMulti.Hidden("Hlastentry", bp.getProperty("LastEntryText") + " " + s))
        End If

        If bp.getProperty("PingMaxCount") <> "" Then
            sOut.Append(qMulti.Hidden("PingMaxCount", bp.getProperty("PingMaxCount")))
        Else
            sOut.Append(qMulti.Hidden("PingMaxCount", "-1"))
        End If

        If bp.getProperty("PingSeconds") <> "" Then
            sOut.Append(qMulti.Hidden("PingSeconds", bp.getPropertyVal("PingSeconds").ToString()))
        Else
            sOut.Append(qMulti.Hidden("PingSeconds", "-1"))
        End If
        sOut.Append(qMulti.Hidden("gIsEnable", "1"))
        sOut.Append(qMulti.Hidden("PingAlertCount", bp.getPropertyVal("PingAlertCount").ToString))
        sOut.Append(qMulti.Hidden("PingAlertText", bp.getProperty("PingAlertText")))

        sOut.Append(qMulti.FromFile(bp.m_sMapPath + "\color\NoPopups.htm"))

        sOut.Append(qMulti.Hidden("HlockClick", bp.getProperty("LockClick"))) 'vk 06.10
        sOut.Append(qMulti.Hidden("HexitMsg", bp.getProperty("ExitMsg"))) 'vk 10.11
        sOut.Append(qMulti.Hidden("Hgear", bp.getProperty("GearDefault"))) 'vk 02.13
        'sOut.Append(qMulti.Hidden("Hurl", bp.m_sUrl)) 'vk 02.13   'cancelled ntg 05.03.23
        'sOut.Append(qMulti.Hidden("HIP", bp.m_sIP)) 'vk 12.13    'cancelled ntg 05.03.23 
        sOut.Append(qMulti.Hidden("HtifTop", bp.getPropertyPset("Margin_Top", oNodeScreen))) 'vk 03.13
        sOut.Append(qMulti.Hidden("HtifLeft", bp.getPropertyPset("Margin_Left", oNodeScreen))) 'vk 03.13
        sOut.Append(qMulti.Hidden("HarchiveMask", bp.getProperty("ArchiveMask"))) 'vk 10.17

        'vk 12.13
        'Dim sBuffer As String = bp.getProperty("Buffer")  cancelled ntg 05.03.23
        'If Right(sBuffer, 1) <> "\" Then
        '    sBuffer &= "\"
        'End If
        'sOut.Append(qMulti.Hidden("Hbuffer", sBuffer))   cancelled ntg 05.03.23
        If oNodeScreen.Param("fgr") = "T" Then
            sOut.Append(qMulti.Hidden("HdontAnswer", "")) 'vk 11.15
        End If

        'sOut.Append("<script>").Append(vbCrLf)
        'sOut.Append("if (window.NodeList && !NodeList.prototype.forEach) {").Append(vbCrLf)
        'sOut.Append("NodeList.prototype.forEach = Array.prototype.forEach;").Append(vbCrLf)
        'sOut.Append("}").Append(vbCrLf)
        'sOut.Append("try {").Append(vbCrLf)
        'sOut.Append("document.getElementById('Hclient').value=navigator.userAgent;").Append(vbCrLf)
        'sOut.Append("} catch(e) {}").Append(vbCrLf)
        'sOut.Append("let charts = new Map();").Append(vbCrLf) 'vk 06.21
        'sOut.Append("</script>").Append(vbCrLf)

        If oNodeScreen.Param("sic") = "S" AndAlso oNodeScreen.Param("img") = "K" Then
            sOut.Append("<object id='DspControls' name='DspControls' classid='DspControls.dll#DspControls.DspControls.DspRun' style='display:none;'></object>").Append(vbCrLf)
        End If
        sOut.Append(bp.m_sPrebuilt) 'ntg 10.07.24 vladi's change regarding city-street screen
    End Sub

    Sub Dispose()
        qSecurity = Nothing
        qMulti.Dispose()
        qMulti = Nothing
    End Sub

End Class
