Imports System.IO 'vk 07.13

Friend Class Once

    Private qStyle As New StyleModule() 'vk 03.04
    Private qSecurity As New Security() 'vk 06.04
    Private qMulti As New Multi() 'vk 02.05
    Private qConv As New Conv() 'vk 11.06

    Sub FormHiddens(ByRef bp As BuildPage, ByRef sOut As System.Text.StringBuilder, _
            ByRef oNodeScreen As Node, ByVal sScreenNum As String, _
            ByVal sLastEntry As String, ByVal sWarning As String, ByVal sCommand As String, _
            ByVal bLockEnter As Boolean, ByVal bFirstScreen As Boolean) 'vk 12.07, 03.13

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
        bp.m_sXmlFil = oNodeScreen.Param("fil") 'vk 05.09
        bp.m_sXmlRec = oNodeScreen.Param("rec") 'vk 05.09
        If oNodeScreen.Param("qpxl") Is Nothing Then
        Else
            'vk 05.06
            sOut.Append(qMulti.Hidden("Hqpxl", oNodeScreen.ParamVal("qpxl").ToString))
        End If
        sOut.Append(qMulti.Hidden("Hx", "x"))
        sOut.Append(qMulti.Hidden("Hforbidden", qSecurity.ForbiddenList(bp, oNodeScreen)))
        sOut.Append(qMulti.Hidden("Hscreen", sScreenNum))
        sOut.Append(qMulti.Hidden("Hspool", IIf(oNodeScreen.Param("wait") = "S", "true", "false"))) 'vk 01.07
        'vk 12.07
        'For Each v In New String() {"Webf", "Eps", "Comtec"}
        sOut.Append(qMulti.Hidden("Hpath_Comtec", bp.getPropertyHttp(v + "Path")))
        'Next
        sOut.Append(qMulti.Hidden("Hfnn", oNodeScreen.Param("fnn")))
        'vk 01.08
        sOut.Append(qMulti.Hidden("Henter", IIf(bLockEnter, "false", "true")))
        'vk 03.08
        sOut.Append(qMulti.Hidden("Hsynchr", bp.m_sSynchr))
        'sOut.Append(qMulti.Hidden("Hf5", IIf(bp.m_bF5, "true", "false")))
        'vk 01.09
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
        'vk 05.09, 06.09
        sOut.Append(qMulti.Hidden("HresMethod", _
            IIf(bp.getProperty("Flexibility") <> "", bp.getProperty("Flexibility"), _
            IIf(bp.getProperty("SameWindow").ToLower = "true", "flexible", "hard")))) 'vk 01.12
        sOut.Append(qMulti.Hidden("Hpopups", bp.getProperty("Popups")))

        sOut.Append(qMulti.Hidden("Hwidescreen", bp.getProperty("WideScreenSupport"))) 'vk 05.10

        If bp.getProperty("PingMaxCount") <> "" Then
            sOut.Append(qMulti.Hidden("PingMaxCount", bp.getProperty("PingMaxCount")))
        Else
            sOut.Append(qMulti.Hidden("PingMaxCount", "-1"))
        End If

        If bp.getProperty("PingSeconds") <> "" Then
            sOut.Append(qMulti.Hidden("PingSeconds", bp.getProperty("PingSeconds") + "000;"))
        Else
            sOut.Append(qMulti.Hidden("PingSeconds", "-1"))
        End If
        sOut.Append(qMulti.Hidden("gIsEnable", "1"))
        sOut.Append(qMulti.Hidden("PingAlertCount", bp.getPropertyVal("PingAlertCount").ToString))
        sOut.Append(qMulti.Hidden("PingAlertText", bp.getProperty("PingAlertText").Replace("\n", " ")))

        sOut.Append(qMulti.FromFile(bp.m_sMapPath + "\color\NoPopups.htm"))
        sOut.Append(qMulti.Hidden("HlockClick", bp.getProperty("LockClick"))) 'vk 06.10
        sOut.Append(qMulti.Hidden("HexitMsg", bp.getProperty("ExitMsg"))) 'vk 10.11
        sOut.Append(qMulti.Hidden("Hgear", bp.getProperty("GearDefault"))) 'vk 02.13
        sOut.Append(qMulti.Hidden("Hurl", bp.m_sUrl)) 'vk 02.13
        sOut.Append(qMulti.Hidden("HIP", bp.m_sIP)) 'vk 12.13
        sOut.Append(qMulti.Hidden("HtifTop", bp.getPropertyPset("Margin_Top", oNodeScreen))) 'vk 03.13
        sOut.Append(qMulti.Hidden("HtifLeft", bp.getPropertyPset("Margin_Left", oNodeScreen))) 'vk 03.13
        sOut.Append(qMulti.Hidden("HarchiveMask", bp.getProperty("ArchiveMask"))) 'vk 10.17
        'vk 12.13
        Dim sBuffer As String = bp.getProperty("Buffer")
        If Right(sBuffer, 1) <> "\" Then
            sBuffer &= "\"
        End If
        sOut.Append(qMulti.Hidden("Hbuffer", sBuffer))
        If oNodeScreen.Param("fgr") = "T" Then
            sOut.Append(qMulti.Hidden("HdontAnswer", "")) 'vk 11.15
        End If
        'vk 12.12, 02.13, 03.13
        If oNodeScreen.Param("sic") = "S" AndAlso oNodeScreen.Param("img") = "K" Then
            sOut.Append("<object id='DspControls' name='DspControls' classid='DspControls.dll#DspControls.DspControls.DspRun' style='display:none;'></object>").Append(vbCrLf)
            Dim s, sKey, sValue As String, i1, i2, j As Integer ', c As Collection
            s = qMulti.FromFile(bp.m_sMapPath + "\color\artis.config")
            Do
                i1 = InStr(s, "{@")
                If i1 <= 0 Then Exit Do
                i2 = InStr(i1, s, "@}")
                If i2 > 0 Then
                    sKey = Mid(s, i1 + 2, i2 - i1 - 2)
                    sValue = ""
                    Select Case sKey
                        Case "F1"
                            Select Case Mid(sCommand, 3, 1)
                                Case "8" 'policy
                                    sValue = Mid(sCommand, 16, 12) & ";" & Mid(sCommand, 12, 3)
                                Case "9" 'claim
                                    sValue = Mid(sCommand, 29, 13) & ";" & Mid(sCommand, 12, 3)
                                Case "1" 'client
                                    sValue = Mid(sCommand, 47, 15)
                            End Select
                        Case Else
                            j = InStr(sKey, ",")
                            If j > 0 Then
                                Try
                                    'sValue = c.Item(sKey)
                                    sValue = Mid(sCommand, Val(Left(sKey, j - 1)), Val(Mid(sKey, j + 1)))
                                Catch
                                End Try
                            End If
                    End Select
                    'Do While Len(sValue) > 1 AndAlso Left(sValue, 1) = "0"
                    '    sValue = Mid(sValue, 2)
                    'Loop
                    s = Left(s, i1 - 1) + sValue + Mid(s, i2 + 2)
                End If
            Loop
            sOut.Append(qMulti.Hidden("Hartis", s))
        End If
        'vk 12.20
        If bp.getProperty("Iframe").ToLower = "true" Then
            sOut.Append(qMulti.Hidden("Hiframe", "iframe"))
        End If

    End Sub

    Sub FormStamp(ByRef bp As BuildPage,
            ByRef sOut As System.Text.StringBuilder, ByRef oNodeScreen As Node) 'vk 06.05, 11.09

        Dim stp As String = oNodeScreen.Param("stp")
        If stp = " " Then Return
        If bp.getPropertyStamp("Count", stp) = "" Then Return
        If oNodeScreen.Param("wait") <> "S" Then Return
        If oNodeScreen.Param("plgc") = "P" Then Return 'vk 04.09

        Dim sInternal As String, i As Integer, oControl As Control ', oStyle As Style
        sInternal = qMulti.FromFile(bp.m_sMapPath + "\Color\" + bp.getPropertyStamp("Image", stp)) + vbCrLf
        For i = 1 To 9
            If oNodeScreen.ParamExists("sp" + i.ToString) Then
                oControl = New Control("style='" + bp.getPropertyStamp("StyleInternal" + i.ToString, stp) + "'")
                oControl.Add("value", oNodeScreen.Param("sp" + i.ToString))
                sInternal += oControl.FullCodeSpan
            End If
        Next
        For i = 1 To bp.getPropertyStampVal("Count", stp)
            oControl = New Control()
            oControl.Add("dir", "ltr")
            oControl.Add("class", "Stamp" + i.ToString) 'vk 07.05
            sOut.Append(oControl.FullCode("div") + sInternal + "</div>" + vbCrLf)
        Next

    End Sub

    Sub Dispose()
        qStyle = Nothing
        qSecurity = Nothing
        qMulti.Dispose()
        qMulti = Nothing
        qConv = Nothing
    End Sub

End Class
