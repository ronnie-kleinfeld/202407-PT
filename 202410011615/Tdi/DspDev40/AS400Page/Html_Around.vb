#Const AVOIDBACK = False 'vk 06.04
'#Const SHOWBROWSER = False 'vk 08.04
Friend Class Around

    Private qStyle As New StyleModule()
    Private qMulti As New Multi()

    Sub FormForms(ByRef bp As BuildPage, ByRef sOut As Text.StringBuilder, ByRef oNodeScreen As Node)

        sOut.Append("<DIV style='display:none;'>").Append(vbCrLf)

        If oNodeScreen.Param("adr") = "R" Then 'elseif vk 11.04
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

        If bp.m_bStreetCombos Then 'ntg 10.07.24 vladi's change regarding city-street screen
            sOut.Append("<IFRAME id='ifrStreetsCombo' name='ifrStreetsCombo' src='Empty.htm' width='0' height='0'></IFRAME>").Append(vbCrLf)
            sOut.Append("<FORM id='frmStreetsCombo' target='ifrStreetsCombo' method='post' action='BringS.aspx'>").Append(vbCrLf)
            sOut.Append(qMulti.Hidden("hDb", oNodeScreen.Param("flib").Trim))
            sOut.Append(qMulti.Hidden("hCity"))
            sOut.Append(qMulti.Hidden("hTarget"))
            sOut.Append("</form>").Append(vbCrLf)
            sOut.Append(qMulti.Hidden("hResult", ""))
        End If

        Dim oFlexCombo As FlexCombo
        If bp.cFlexCombos IsNot Nothing Then
            For Each oFlexCombo In bp.cFlexCombos
                If Not (oFlexCombo.bBoss OrElse oFlexCombo.bDouble) Then
                    sOut.Append("<IFRAME id='ifrFlexCombo_").Append(oFlexCombo.sN).Append("' name='ifrFlexCombo_").Append(oFlexCombo.sN).Append("' src='Empty.htm' width='0' height='0'></IFRAME>").Append(vbCrLf)
                    sOut.Append("<form id='frmFlexCombo_").Append(oFlexCombo.sN).Append("' target='ifrFlexCombo_").Append(oFlexCombo.sN).Append("' method='post' action='BringF.aspx'>").Append(vbCrLf)
                    sOut.Append(qMulti.Hidden("hDb", oNodeScreen.Param("flib").Trim))
                    sOut.Append(qMulti.Hidden("hKod", oFlexCombo.sKod))
                    Dim v As String
                    For Each v In AllFields_FlexCombo1
                        sOut.Append(qMulti.Hidden(v))
                    Next
                    sOut.Append("</form>").Append(vbCrLf)
                    sOut.Append(qMulti.Hidden("hResult_" + oFlexCombo.sN, "")) ', "onpropertychange", "flexlist2('" + oFlexCombo.sN + "');"))
                    For Each v In AllFields_FlexCombo2
                        sOut.Append(qMulti.Hidden(v + "_" + oFlexCombo.sN))
                    Next
                End If
            Next
        End If

        ' Open external URl 

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
        ''

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
            sOut.Append(qMulti.Hidden("HarchiveDescr", bp.m_sArchiveDescr))
            sOut.Append(qMulti.Hidden("HarchiveDB", oNodeScreen.Param("flib"))) 'vk 07.17
            sOut.Append(qMulti.Hidden("HarchiveDocType")) 'vk 07.17
            sOut.Append(qMulti.Hidden("HarchiveSystem", bp.m_sSystem)) 'vk 12.18

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

        sOut.Append(qMulti.Hidden("PingNumber", "")) ', "onpropertychange", "bInPing=false;")) 'vk 11.06
        sOut.Append("<IFRAME id='ifrPing' name='ifrPing' src='Empty.htm' width='0' height='0'></IFRAME>").Append(vbCrLf)
        sOut.Append("<form id='frmPing' action='Ping.aspx' method='post' target='ifrPing'></form>").Append(vbCrLf)
        sOut.Append("<IFRAME id='ifrRefresh' name='ifrRefresh' src='Empty.htm' width='0' height='0'></IFRAME>").Append(vbCrLf)
        sOut.Append("</DIV>").Append(vbCrLf)
    End Sub

    Sub Dispose()
        qStyle = Nothing
        qMulti.Dispose()
        qMulti = Nothing
    End Sub
End Class
