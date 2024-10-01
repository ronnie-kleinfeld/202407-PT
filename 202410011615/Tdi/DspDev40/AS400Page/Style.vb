Friend Class StyleModule

    Function GetStyleOption(ByRef oNodeScreen As Node, ByVal iwidth As Integer) As String
        Dim oStyle As New Style()
        oStyle.Add("display", "none")
        oStyle.Add("cursor", "default")
        oStyle.Add("width", iwidth)
        Return oStyle.FullCode
    End Function

    Function GetStyleV(ByRef bp As BuildPage,
            ByRef oNode As Node, ByRef oNodeScreen As Node) As String
        Dim oStyle As New Style()
        oStyle.Add("position", "absolute")
        oStyle.Add("left", icol_fun(bp, oNode, oNodeScreen))
        oStyle.Add("top", ilin_fun(bp, oNode, oNodeScreen))
        oStyle.Add("cursor", "pointer")
        oStyle.Add("height", bp.iScaleLin - 2)
        Return oStyle.FullCode
    End Function

    Function GetStylePos(ByRef bp As BuildPage,
            ByRef oNode As Node, ByRef oNodeScreen As Node) As String 'vk 02.05
        Dim oStyle As New Style()
        oStyle.Add("position", "absolute")
        oStyle.Add("left", icol_fun(bp, oNode, oNodeScreen))
        oStyle.Add("top", ilin_fun(bp, oNode, oNodeScreen))
        Return oStyle.FullCode
    End Function

    Function GetStyleLvl(ByRef bp As BuildPage,
            ByRef oNode As Node, ByRef oNodeScreen As Node, ByVal bBkg As Boolean) As String
        Dim oStyle As New Style()
        oStyle.Add("position", "absolute")
        oStyle.Add("left", icol_fun(bp, oNode, oNodeScreen, True)) 'vk 01.06
        oStyle.Add("top", ilin_fun(bp, oNode, oNodeScreen))
        'vk 07.06
        'Dim s As String = bp.getProperty("AsteriskColor")
        'If s = "" Then
        '    Dim bckg As String = ""
        '    Dim forg As String = ""
        '    Dim isColorChangeF, isColorChangeB, bTransparent As Boolean
        '    SetColor(bp, oNode, bckg, forg, isColorChangeF, isColorChangeB, bTransparent)
        '    oStyle.Add("color", forg)
        'Else
        '    oStyle.Add("color", s)
        'End If
        Return oStyle.FullCode
    End Function

    Function GetStyleBtn(ByRef bp As BuildPage, ByRef oNode As Node, ByRef oNodeScreen As Node,
        ByVal sPlace As String, ByVal sSpecial As String, ByVal bDisabled As Boolean) As String

        Dim iLin, iCol As Integer

        Dim oStyle As New Style()
        If sPlace = "field" Then
            iLin = ilin_fun(bp, oNode, oNodeScreen)
            iCol = icol_fun(bp, oNode, oNodeScreen)
        Else
            iLin = (oNode.ParamVal("lin") - 1) * bp.iScaleLin_Btn
            iCol = (oNode.ParamVal("col") - 1) * bp.iScaleCol_Btn '+ bp.iMarginLeft
        End If

        ' oStyle.Add("font-family", bp.getProperty("BodyFont"))

        If oNode.ParamExists("fbg") Then
            oStyle.Add("background-color", bp.m_colColor.Item("bg_" + oNode.Param("fbg") + "_bckg").ToString)
        End If
        If oNode.ParamExists("fcl") Then
            oStyle.Add("color", bp.m_colColor.Item("clr_" + oNode.Param("fcl")).ToString)
        End If

        If oNode.ParamExists("pcl") Then
            oStyle.Add("color", bp.m_colColor.Item("clr_" + oNode.Param("pcl")).ToString)
        End If

        Return oStyle.FullCode
    End Function

    Function GetStyleTxt(ByRef bp As BuildPage, ByRef oNode As Node, ByRef oNodeScreen As Node) As String

        Dim iLin, iCol, iWidth, iHeight As Integer

        iLin = ilin_fun(bp, oNode, oNodeScreen)
        iCol = icol_fun(bp, oNode, oNodeScreen)

        iWidth = iwid_fun(bp, oNode, oNodeScreen)
        iHeight = ihei_fun(bp, oNode) 'vk 02.05
        If oNode.Param("psl") = "G" Then
            SetCheckRadioSize(bp, "radio", iLin, iCol, iWidth, iHeight) 'vk 11.05
        ElseIf oNode.ParamExists("chb") Then
            SetCheckRadioSize(bp, "check", iLin, iCol, iWidth, iHeight) 'vk 11.05
        End If
        If oNode.ParamIn("pf4", "CRADSB") Then
            iWidth += bp.iScaleWidth * 2
        End If

        Dim oStyle As New Style()
        If oNode.Param("num") = "placeholder" Then
            'vk 09.24
            oStyle.Add("display", "none")
        End If
        If oNode.ParamVal("lin") = 1 AndAlso Not oNodeScreen.Modal Then
            oStyle.Add("width", iwid_fun(bp, oNode, oNodeScreen, True) - 1)
            oStyle.Add("text-align", "center")
            oStyle.Add("direction", IIf(oNodeScreen.ParamVal("flr") = 1, "ltr", "rtl").ToString) 'vk 06.10
            Return oStyle.FullCode
        End If

        If oNode.Param("inp") <> "O" OrElse Not oNodeScreen.ParamExists("pset") Then 'vk 07.05
            oStyle.Add("left", iCol)
        End If

        oStyle.Add("line-height", CInt(iHeight * 0.8)) 'vk 06.10
        If oNode.ParamVal("pht") > 1 Then 'vk 11.06
            oStyle.Add("vertical-align", "middle") 'vk 02.05
        End If

        If oNode.ParamIn("pf4", "CRADSB") OrElse
            (bp.getProperty("CityStreet") = "inline" AndAlso oNode.ParamIn("apr", "CS", 2) AndAlso oNodeScreen.Param("fgr") <> "P") Then 'ntg 10.07.24 vladi's change regarding city-street screen
            If oNodeScreen.ParamVal("flr") = 1 Then
                oStyle.Add("text-align", "left") 'English screen
            Else
                oStyle.Add("text-align", "right") 'Hebrew screen
                oStyle.Add("padding-right", 2) 'vk 11.06
            End If
        Else 'not combo
            If (oNode.Param("typ") <> "A" AndAlso oNode.Param("typ") <> "U" AndAlso oNode.Param("typ") <> "T") OrElse oNode.ParamYes("prl") Then
                If oNodeScreen.ParamVal("flr") = 1 Then
                    oStyle.Add("text-align", "left") 'ntg 14.06.23 added English option for GIC (input data now appears on the left)
                Else
                    oStyle.Add("text-align", "right") 'number, date, etc., Hebrew text
                End If
                If oNode.ParamIn("inp", "IKR") Then 'vk 02.07
                    oStyle.Add("padding-right", 2) 'vk 11.06
                End If
            Else
                oStyle.Add("text-align", "left") 'English text
            End If
        End If

        If oNode.ParamYes("phi") Then ' high intensity
            oStyle.Add("font-weight", "bold")
        End If
        If oNode.ParamYes("pul") Then ' underline
            oStyle.Add("text-decoration", "underline")
        End If

        Dim pfnP As String
        If oNode.ParamExists("pfn") Then
            pfnP = bp.m_colColor.Item("font_" + oNode.Param("pfn"))
            oStyle.Add("font-family", pfnP)
        End If
        If oNode.ParamExists("psz") Then
            'oStyle.Add("font-size", oNode.FontSize(oNodeScreen, bp, False))
            oStyle.Add("font-size", oNode.ParamVal("psz")) 'vk 09.24
        ElseIf oNode.ParamVal("pfn") = 8 AndAlso Not oNodeScreen.ParamExists("pset") Then
            oStyle.Add("font-size", bp.sFontSizeFixed)
        End If

        If oNode.Param("forcewrap") = "true" Then
            oStyle.Add("white-space", "normal")
        End If

        If oNode.ParamYes("heb") AndAlso Not oNode.ParamIn("inp", "IKR") Then
            oStyle.Add("unicode-bidi", "bidi-override")
            oStyle.Add("direction", "ltr")
        End If

        Return oStyle.FullCode

    End Function

    Function ilin_fun(ByRef bp As BuildPage, ByRef oNode As Node, ByRef oNodeScreen As Node) As Integer

        Dim l, r As Integer

        If oNode.ParamVal("lin") = 0 Then
            l = oNode.ParamVal("ind") - 1 '- oNodeScreen.ParamVal("qstr") + 2
            Dim flh As Single = bp.getPropertyValSng("FlexLineHeight") 'vk 07.07
            r = l * bp.iScaleLin * flh + 8 + bp.m_nDeltaForScrollFields '+ bp.iMarginTop + 8 - bp.iScaleLin
        Else
            l = oNode.ParamVal("lin")
            If bp.m_nGridSearch > 0 AndAlso bp.m_nGridTitleFrom > 0 Then
                Select Case l 'vk 05.05
                    Case bp.m_nGridSearch
                        l = bp.m_nGridTitleFrom
                    Case bp.m_nGridTitleFrom To bp.m_nGridTitleTill
                        l += 1
                End Select
            End If
            r = ilin_fun_netto(bp, l, oNodeScreen, True)
        End If
        Return r
    End Function
    Function ilin_fun_netto(ByRef bp As BuildPage, ByVal l As Integer, ByRef oNodeScreen As Node,
            Optional ByVal bField As Boolean = False) As Integer 'vk 05.05
        Dim bUnderGrid As Boolean = bField AndAlso bp.getProperty("Design") = "lex" _
            AndAlso bp.m_nGridTill > 0 AndAlso l > bp.m_nGridTill 'vk 08.07
        If Not oNodeScreen.Modal Then
            l -= 2
        End If
        Dim r As Integer = l * bp.iScaleLin
        If bUnderGrid Then
            r += 17
        End If
        Return r
    End Function

    Function icol_fun(ByRef bp As BuildPage, ByRef oNode As Node, ByRef oNodeScreen As Node,
            Optional ByVal bAsterisk As Boolean = False) As Integer
        Dim c As Integer = oNode.ParamVal("col")
        If bAsterisk Then
            If oNodeScreen.ParamVal("flr") = 0 Then
                c += oNode.ParamVal("wd")
            Else
                c -= 1
            End If
        End If
        If oNode.ParamVal("lin") = 0 Then
            Return (c - 1) * bp.iScaleWidth
        Else
            Return icol_fun_netto(bp, c, oNodeScreen, False, oNode.ParamVal("lin") = 1 AndAlso Not oNodeScreen.Modal) 'vk 03.08, 01.12, 01.15
        End If
    End Function
    Function icol_fun_netto(ByRef bp As BuildPage, ByVal l As Integer, ByRef oNodeScreen As Node,
            ByVal bInShiftedFirstLine As Boolean, Optional ByVal bTop As Boolean = False) As Integer 'vk 05.05, 07.07, 03.08
        Dim r As Integer
        Dim nScaleWidth As Integer = ScaleWid(bp, oNodeScreen, bTop)

        r = (l - 1) * nScaleWidth

        Return r
    End Function

    Function iwid_fun(ByRef bp As BuildPage, ByRef oNode As Node, ByRef oNodeScreen As Node,
            Optional ByVal bTop As Boolean = False) As Integer

        Return oNode.ParamVal("wd") * ScaleWid(bp, oNodeScreen, bTop) + 1
    End Function
    Function ihei_fun(ByRef bp As BuildPage, ByRef oNode As Node) As Integer 'vk 02.05
        Return oNode.ParamVal("pht") * bp.iScaleLin - 1
    End Function
    Private Function ScaleWid(ByRef bp As BuildPage, ByRef oNodeScreen As Node, ByVal bTop As Boolean) As Integer
        Return IIf(bTop AndAlso oNodeScreen.ParamYes("dspa"), bp.ResolValue(8, 10, False, "hor"), bp.iScaleWidth)
    End Function

    Sub SetCheckRadioSize(ByRef bp As BuildPage, ByVal sKind As String, ByRef iLin As Integer, ByRef iCol As Integer, ByRef iWidth As Integer, ByRef iHeight As Integer)

        Dim n As Integer
        Select Case sKind
            Case "check"
                n = bp.getPropertyVal("CheckSize")
                If n = 0 Then n = 17
            Case "radio"
                n = bp.getPropertyVal("RadioSize")
                If n = 0 Then n = 11
        End Select
        n *= bp.ResolValue(100, 122, False, "hor") / 100

        iLin += (iHeight - n) / 2
        iCol += (iWidth - n) / 2
        iWidth = n
        iHeight = n

    End Sub

    Sub SetDir(ByRef bp As BuildPage, ByRef oNodeScreen As Node, ByRef oControl As Control) 'vk 08.06
        Select Case oNodeScreen.ScrollSide(bp)
            Case "left" : oControl.Add("dir", "rtl")
            Case "right" : oControl.Add("dir", "ltr")
        End Select
    End Sub
End Class
