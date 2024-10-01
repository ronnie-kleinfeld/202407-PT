Friend Class StyleModule

    Function ilin_fun_netto(ByRef bp As BuildPage, ByVal l As Integer, ByRef oNodeScreen As Node,
            Optional ByVal bField As Boolean = False) As Integer 'vk 05.05
        Dim bUnderGrid As Boolean = bField AndAlso bp.getProperty("Design") = "lex" _
            AndAlso bp.m_nGridTill > 0 AndAlso l > bp.m_nGridTill 'vk 08.07
        If Not oNodeScreen.Modal Then
            If bp.m_bKiosk Then
                l -= 4
            Else
                l -= 2
            End If
            If oNodeScreen.Ortogonal(bp) Then l -= 2 'vk 05.05
        End If
        Dim r As Integer = l * bp.iScaleLin + bp.iMarginTop
        If bUnderGrid Then
            r += 17
        End If
        Return r
    End Function

    Function icol_fun(ByRef bp As BuildPage, ByRef oNode As Node, ByRef oNodeScreen As Node,
            Optional ByVal bAsterisk As Boolean = False) As Integer
        Dim c As Integer = oNode.ParamVal("col")
        If bAsterisk Then
            'vk 01.06, 07.06
            'Select Case dp.getProperty("AsteriskSide")
            '    Case ""
            If oNodeScreen.ParamVal("flr") = 0 Then
                c += oNode.ParamVal("wd")
            Else
                c -= 1
            End If
            '    Case "left" : c -= 1
            '    Case "right" : c += oNode.ParamVal("wd")
            'End Select
        End If
        If oNode.ParamVal("lin") = 0 Then
            If bp.m_bKiosk Then
                'vk 02.05
                Return (c - 1) * bp.iScaleWidth + bp.iMarginLeft
            Else
                Return (c - 1) * bp.iScaleWidth
            End If
        ElseIf bp.m_bKiosk AndAlso
                (oNode.ParamVal("lin") = 2 OrElse oNode.ParamVal("lin") = 3) AndAlso Not oNodeScreen.Modal Then 'win vk 09.05
            'vk 02.05
            Return (c - 1) * bp.iScaleWidthK
        Else
            Return icol_fun_netto(bp, c, oNodeScreen, oNode.ShiftedFirstLine(oNodeScreen, bp), oNode.ParamVal("lin") = 1 AndAlso Not oNodeScreen.Modal) 'vk 03.08, 01.12, 01.15
        End If
    End Function
    Function icol_fun_netto(ByRef bp As BuildPage, ByVal l As Integer, ByRef oNodeScreen As Node,
            ByVal bInShiftedFirstLine As Boolean, Optional ByVal bTop As Boolean = False) As Integer 'vk 05.05, 07.07, 03.08
        Dim r As Integer
        Dim nScaleWidth As Integer = ScaleWid(bp, oNodeScreen, bTop)
        'IIf(bTop AndAlso oNodeScreen.TopLength = 80, bp.ResolValue(6, 7, 11, False, "hor"), bp.iScaleWidth) 'vk 07.09, 06.10, 01.12
        r = (l - 1) * nScaleWidth
        If Not bInShiftedFirstLine Then r += bp.iMarginLeft
        If bp.OrganizedButtons(True) Then 'vk 01.12
            'If bp.getProperty("Design") = "lex" Then
            If oNodeScreen.ParamIn("fsid", "RSDE") AndAlso oNodeScreen.Param("win") <> "C" Then 'vk 01.09
                r += CInt(nScaleWidth * 1.6) 'vk 05.08, 08.08
            End If
            If bp.GridExists AndAlso oNodeScreen.Modal Then
                r += IIf(oNodeScreen.ParamVal("flr") = 1, -9, 16) 'vk 11.08
            End If
        End If
        'AndAlso (bp.GridExists OrElse bp.ResolValue(-1, 0, 0)) 
        Return r
    End Function

    Function iwid_fun(ByRef bp As BuildPage, ByRef oNode As Node, ByRef oNodeScreen As Node,
            Optional ByVal bTop As Boolean = False) As Integer
        If bp.m_bKiosk AndAlso
                (oNode.ParamVal("lin") = 2 OrElse oNode.ParamVal("lin") = 3) AndAlso Not oNodeScreen.Modal Then 'win vk 09.05
            'vk 02.05
            Return oNode.ParamVal("wd") * bp.iScaleWidthK + 1
        Else
            'Dim nScaleWidth As Integer = IIf(bTop AndAlso oNodeScreen.TopLength = 80, bp.ResolValue(8, 10, 14, False, "hor"), bp.iScaleWidth) 'vk 07.09, 06.10
            Return oNode.ParamVal("wd") * ScaleWid(bp, oNodeScreen, bTop) + 1
        End If
    End Function
    Private Function ScaleWid(ByRef bp As BuildPage, ByRef oNodeScreen As Node, ByVal bTop As Boolean) As Integer 'vk 01.12
        Return IIf(bTop AndAlso oNodeScreen.ParamYes("dspa"), bp.ResolValue(8, 10, 14, False, "hor"), bp.iScaleWidth)
        'Return IIf(bTop AndAlso oNodeScreen.TopLength = 80, bp.ResolValue(6, 7, 11, False, "hor"), bp.iScaleWidth)
    End Function

End Class
