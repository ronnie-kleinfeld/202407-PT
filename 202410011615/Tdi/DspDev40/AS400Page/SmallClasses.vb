'all Collections turned to HashTables by vk 03.08

'vk 04.03
'for grid:
Imports System.Linq
Imports DDSWeb

Friend Class Xzone
    Friend nFrom, nTill As Integer
    Friend Sub New()
        nFrom = 0
        nTill = 0
    End Sub
End Class

Friend Class Yzone
    Friend nFrom, nTill As Integer, sKind As String
    Friend Sub New()
        nFrom = 0
        nTill = 0
        sKind = " "
    End Sub
End Class

'for controls:
Friend Class Attrib
    Friend sName, sValue As String
    Friend Sub New(ByVal pName As String, ByVal pValue As String)
        sName = pName
        sValue = pValue
    End Sub
End Class

Friend Class Control
    Private qConv As New Conv() 'vk 11.09
    Private sStyle, sTail As String, a As Attrib
    Private hAttribs As Hashtable
    Private PencilObj As PencilProperties = Nothing
    Private Ind As String = ""

    Friend Sub New(Optional ByVal pStyle As String = "", Optional ByVal bLocked As Boolean = False)
        sStyle = pStyle
        sTail = ""
        hAttribs = New Hashtable()
        If bLocked Then
            Add("readonly", "true")
            'Add("UNSELECTABLE", "on")
            Add("tabindex", "-1")
            Unsel()
        End If
    End Sub

    Friend Sub SetInd(ByVal pInd As String) 'vk 07.07
        Ind = pInd
    End Sub
    Friend Sub SetStyle(ByVal pStyle As String) 'vk 07.07
        sStyle = pStyle
    End Sub
    Friend Sub Unsel()
        Const U As String = "-webkit-user-select:none;-moz-user-select:none;-ms-user-select:none;user-select:none;"
        If sStyle = "" Then
            sStyle = "style='" & U & "'"
        ElseIf sStyle Like "*'" Then
            sStyle = Left(sStyle, Len(sStyle) - 1) & U & "'"
        Else
            sStyle &= U
        End If
    End Sub

    Friend Sub AddPencil(pPencilText As String, pColor As Boolean)
        PencilObj = New PencilProperties(pPencilText, pColor)
    End Sub
    Friend Sub AddId(ByVal sValue As String)
        If hAttribs.ContainsKey("name") Then hAttribs.Remove("name")
        If hAttribs.ContainsKey("id") Then hAttribs.Remove("id")
        Try
            hAttribs.Add("name", New Attrib("name", sValue))
            hAttribs.Add("id", New Attrib("id", sValue))
        Catch e As Exception
            e = e
        End Try
    End Sub
    Friend Sub AddTail(ByVal sValue As String) 'vk 12.03
        sTail = sValue
    End Sub
    Friend Sub Clear(ByVal sName As String)
        If hAttribs.ContainsKey(sName) Then
            hAttribs.Remove(sName)
        End If
    End Sub
    Friend Sub Add(ByVal sName As String, ByVal sValue As String, Optional ByVal bAlways As Boolean = False, Optional asFirst As Boolean = False)
        If sValue = "" AndAlso Not bAlways Then Exit Sub

        If sName Like "on*" AndAlso Not (sValue Like "*;") Then 'ntg 28.06.23 function call missing a ';', so this adds it
            sValue &= ";" 'vk 06.23
        End If

        Try
            If hAttribs.ContainsKey(sName) Then
                a = hAttribs(sName)
                If asFirst Then
                    a.sValue = sValue + a.sValue
                Else
                    a.sValue += sValue
                End If
            Else
                a = New Attrib(sName, sValue)
                hAttribs.Add(sName, a)
                If bAlways Then Exit Sub
                If sName Like "onkey*" Then
                    a.sValue = "if(NotReady_Key('" + Mid(sName, 3) + "'))return;" + sValue 'vk 11.11
                ElseIf sName Like "on*" Then
                    a.sValue = "if(NotReady('" + Mid(sName, 3) + "'))return;" + sValue
                End If
            End If
        Catch e As Exception
            e = e
        End Try
    End Sub

    Friend Sub ReplaceVal(ByVal sName As String, ByVal sValue As String)
        Try
            If hAttribs.ContainsKey(sName) Then
                a = hAttribs(sName)
                a.sValue = sValue
            Else
                hAttribs.Add(sName, sValue)
            End If
        Catch e As Exception
            e = e
        End Try
    End Sub

    Friend Function Param(ByVal sName As String) As String
        If hAttribs.ContainsKey(sName) Then
            a = hAttribs(sName)
            Return a.sValue
        Else
            Return Nothing
        End If
    End Function
    Friend Function FullCode(ByRef bp As BuildPage, Optional ByVal sType As String = "input",
            Optional ByVal bWithCrLf As Boolean = True, Optional ByVal bMakeOk As Boolean = True,
            Optional ByVal bNbsp As Boolean = False, Optional ByVal IconFullValue As String = "",
            Optional ByVal bRequired As Boolean = False, Optional ByVal plc As String = "", Optional ImageFileName As String = "",
            Optional NodeScreen As Node = Nothing, Optional OriginalNode As Node = Nothing, Optional FooterButtonWithImage As Boolean = False,
            Optional AdditionalClass As String = "", Optional bPer As Boolean = False) As String 'bPer vk 08.24+++

        Dim allActionsHaveIcons As Boolean = bp IsNot Nothing AndAlso bp.ActionNodesIcons.All(Function(kvp) kvp.Value IsNot Nothing) 'ntg 22.05.24 
        Dim aType As Attrib = hAttribs("type")
        Dim bBooCheckbox As Boolean = False 'bBootstrap AndAlso hAttribs.ContainsKey("type") AndAlso hAttribs("type").sValue = "checkbox"
        If hAttribs.ContainsKey("type") Then
            Select Case aType.sValue
                Case "checkbox"
                    If bp IsNot Nothing Then bp.m_sCurClass = "form-check-input"
                    bBooCheckbox = True
                Case "radio"
                    If bp IsNot Nothing Then bp.m_sCurClass = If(ImageFileName = "", "form-check-input", "list-group-item-check")
                    'If bp IsNot Nothing Then bp.m_sCurClass = "list-group-item-check" 'ntg 25.06.23 - changed the appearence of radion btns, so now they can appear as "checked" with blue border
                    sType = "input"
            End Select
        End If

        Dim s As New Text.StringBuilder(), v As Attrib
        Dim val As String = ""
        Dim ToolTipParams = ""

        s.Append("<").Append(sType)

        If bp IsNot Nothing Then
            If bp.m_sCurClass > "" Then s.Append($" class=' {bp.m_sCurClass} {AdditionalClass} '")
            If bp.m_sCurStyle > "" Then
                If sStyle > "" Then
                    sStyle = Left(sStyle, Len(sStyle) - 1) & bp.m_sCurStyle & "'"
                Else
                    sStyle = "'" & bp.m_sCurStyle & "'" 'vk 09.24
                End If
            End If
        Else
            Select Case aType.sValue
                Case "checkbox" : s.Append($" class='custom-switch-input'")
                Case "radio" : s.Append(" class=""form-check-input""")
            End Select
        End If

        If sStyle > "" AndAlso sType <> "duet-date-picker" Then
            s.Append(" ").Append(sStyle)
        End If
        Dim enu As IDictionaryEnumerator = hAttribs.GetEnumerator
        Dim sDblClick As String = ""
        Dim sFocus As String = ""
        Dim sMark As String = ""
        Do While enu.MoveNext
            v = enu.Value
            If v.sName = "value" Then
                val = v.sValue
                If sType <> "button" Then
                    If bMakeOk Then qConv.MakeOk(val, bNbsp)
                    'val = val.Replace(" ", "&nbsp;") 'vk 12.09
                    s.Append(" ").Append(v.sName).Append("=""").Append(val).Append("""")
                End If
            ElseIf bBooCheckbox AndAlso v.sName = "ondblclick" Then
                sDblClick = v.sValue
            ElseIf bBooCheckbox AndAlso v.sName = "onfocus" Then
                sFocus = v.sValue
            ElseIf bBooCheckbox AndAlso v.sName = "mark" Then
                sMark = v.sValue
            Else
                If BOOTSTRAP AndAlso (v.sName = "style" OrElse v.sName = "class") Then
                Else
                    s.Append(" ").Append(v.sName).Append("=""").Append(v.sValue).Append("""")
                End If
            End If
        Loop

        If FooterButtonWithImage Then
            ToolTipParams = $"data-bs-toggle='tooltip' title='{val}'"
            val = ""
        End If

        Dim IconElement As String = ""

        If IconFullValue <> "" Then
            Dim isFgrP As Boolean = NodeScreen IsNot Nothing AndAlso NodeScreen.ParamExists("fgr") AndAlso NodeScreen.ParamIn("fgr", "LBD") AndAlso bp.ActionNodesIcons.Count < 4 AndAlso allActionsHaveIcons 'ntg 21.05.24 ignore "sideIcon" properties in case of fgr=P-showing button instead of list of actions -- 'ntg 23.06.24 fgr=B is the same as fgr=L, but with option to select multiple rows in a table
            'vk 09.24 fgr=D
            Dim IconParam As String() = IconFullValue.Split("-")
            Dim IconNumber As String = IconParam(0)
            Dim IconColorNumber As String = If(IconParam.Length > 1, IconParam(1), "0")

            Dim IconName As Object = bp.m_colColor.Item($"Icon_{CInt(IconNumber)}")

            Dim iconColor As Object = bp.m_colColor.Item($"clr_{IconColorNumber}")

            Dim ColorStyle = ""
            If iconColor IsNot Nothing Then
                ColorStyle = $"style='color:{iconColor};'"
            End If

            If IconName IsNot Nothing Then

                IconElement = $"<i class='{IconName} {If(isFgrP, "", "sideIcon")} {If(AdditionalClass = "cardCmdBtn", "cmdButtonsForCardsIcon", "")} ' {If(isFgrP, "", ColorStyle)}></i>"
            End If

            If OriginalNode IsNot Nothing AndAlso OriginalNode.ParamExists("pch") AndAlso NodeScreen IsNot Nothing AndAlso ((NodeScreen.ParamIn("fgr", "LBD") AndAlso (bp.ActionNodesIcons.Count > 3 OrElse (bp.ActionNodesIcons.Count < 4 AndAlso Not allActionsHaveIcons)))) Then ' Do not show icons in grid with sub-menu of actions -- 'ntg 23.06.24 fgr=B is the same as fgr=L, but with option to select multiple rows in a table
                'vk 09.24 fgr=D
                IconElement = ""
            End If
        Else
            If bp IsNot Nothing AndAlso bp.m_sCurClass IsNot Nothing AndAlso bp.m_sCurClass.Contains("nav-link") Then
                IconElement = $"<i class='fa-solid fa-folder sideIcon '></i>"
            End If
        End If

        If bRequired Then 'OrElse sType = "select"
            s.Append(" required ") 'adding the "required: to the input field itself
        End If

        If bBooCheckbox Then
            'vk 08.24+++
            Dim sPer As String = ""
            If bPer Then
                sPer = "CmdPchClick_Common('', '00', false, false, 0);"
            End If
            s.Append($"onfocus=""{sFocus}"" mark='{sMark}' onclick=""if(NotReady('click'))return;CheckBoxClick(this);{sPer}"" ondblclick=""if(NotReady('dblclick'))return;CheckBoxDblClick(this);"" ") 'ntg 13.04.23 - ondblclick

            '  s.Append("<span Class=""custom-switch-indicator"" ondblclick=""" & sDblClick & """ onfocus=""" & sFocus & """ mark=""" & sMark & """></span></label>") 'boo? check
        End If

        Dim ImageInButton As String = ""
        If ImageFileName <> "" Then
            ImageInButton = $"<img class='w-100 px-2' src='./assets/images/{ImageFileName}'>"
        End If

        If sType = "button" Then
            's.Append($"{ToolTipParams}>{ImageInButton}{IconElement}{If(ImageInButton = "", "", "<span>")}{val}{If(ImageInButton = "", "", "</span>")}</{sType}>{sTail}")
            Dim isFgrP As Boolean = NodeScreen IsNot Nothing AndAlso NodeScreen.ParamExists("fgr") AndAlso NodeScreen.ParamIn("fgr", "LBD") AndAlso bp.ActionNodesIcons.Count < 4 AndAlso allActionsHaveIcons AndAlso IconFullValue > "" 'ntg 20.05.24 changes in appearence regarding screens with fgr=P-showing button instead of list of actions -- 'ntg 23.06.24 fgr=B is the same as fgr=L, but with option to select multiple rows in a table
            'vk 09.24 fgr=D, IconFullValue
            s.Append($"{ToolTipParams}>{ImageInButton}{IconElement}{If(ImageInButton = "", "", "<span>")}{If(isFgrP, "", val)}{If(ImageInButton = "", "", "</span>")}</{sType}>{sTail}") 'ntg 20.05.24 if fgr=P then show only icons without text
        Else
            s.Append(">")
        End If

        If sTail.Trim > "" Then s.Append(vbCrLf).Append(sTail)
        If bWithCrLf Then s.Append(vbCrLf)

        If bRequired Then
            If sType = "select" Then
                s.Append(vbCrLf).Append("<div class='invalid-feedback InvalidManSelect'></div>")
            ElseIf aType.sValue = "number" Then
                s.Append(vbCrLf).Append("<div class='invalid-feedback InvalidManNonZero'></div>")
            Else
                s.Append(vbCrLf).Append("<div class='invalid-feedback InvalidMan'></div>")
            End If
        End If

        If plc <> "" Then
            If plc = "H" Then
                s.Append(vbCrLf).Append("<div class='invalid-feedback InvalidH'></div>")
            End If
        End If

        If bBooCheckbox Then
            Return $"<div class='form-check form-switch'> {s} </div>"
        Else
            Return s.ToString
        End If
    End Function
    Friend Function FullCodeSpan(ByRef bp As BuildPage, Optional ByVal bWithCrLf As Boolean = True,
            Optional ByVal sTag As String = "", Optional ByVal bMakeOk As Boolean = True,
            Optional ByVal bNbsp As Boolean = False, Optional ByVal bRequired As Boolean = False,
            Optional ByVal pHelpText As String = "", Optional ImageFileName As String = "", Optional pBua As String = "", Optional sMailIcon As String = "",
                                 Optional toolTipType As Integer = 1) As String

        If sTag = "" Then
            sTag = "label"
        End If

        Dim s As New Text.StringBuilder(), v As Attrib
        Dim val As String = ""

        If pHelpText <> "" AndAlso (toolTipType = 2 Or toolTipType = 3) Then
            s.Append("<div style=""display:flex;flex-direction:column;"">")
        End If

        s.Append($"<{sTag}")

        If bp IsNot Nothing Then
            If bp.m_sCurClass > "" Then
                If bp.m_sCurClass = "list-group-item-check" AndAlso sTag = "label" Then
                    s.Append(" Class= 'text-center list-group-item py-1'")
                ElseIf bp.m_sCurClass = "form-check-input" AndAlso sTag = "label" Then
                    s.Append($" Class= 'form-check-label'")
                    's.Append(" Class= 'text-center list-group-item py-1'") 'ntg 22.06.23 fixing the class of radio btns 
                Else
                    s.Append(" Class=""").Append(bp.m_sCurClass)
                End If

                If bRequired Then
                    s.Append(" required ") ' Add a red asterix before the text
                End If
                s.Append("""")
            End If
            If bp.m_sCurStyle > "" Then s.Append(" style=""").Append(bp.m_sCurStyle).Append("""")
        End If

        If bp.m_InputId <> "" AndAlso sTag = "label" AndAlso Not hAttribs.ContainsKey("for") Then 'added " AndAlso Not hAttribs.ContainsKey("for") Then - bug 10.05.23
            hAttribs.Add("for", New Attrib("for", bp.m_InputId))
        End If

        Dim enu As IDictionaryEnumerator = hAttribs.GetEnumerator
        Do While enu.MoveNext
            v = enu.Value
            Select Case v.sName
                Case "value"
                    val = v.sValue
                    If bMakeOk Then
                        qConv.MakeOk(val, bNbsp)
                        val = val.Replace(" ", "&nbsp;") 'vk 12.09
                    End If
                Case Else
                    If Not (v.sName = "style" OrElse v.sName = "class") Then
                        s.Append(" " + v.sName + "=""" + v.sValue + """")
                    End If
            End Select
        Loop
        If pBua <> "" Then
            val = val + $"<i class='fas fa-info-circle HelpLink mx-1' data-bs-toggle='tooltip' title='{pBua}' ></i>"
        End If

        If PencilObj IsNot Nothing Then
            Dim PenClass As String = If(PencilObj.Color, "PencilColor", "PencilBlack")

            val = val + $"<i Class=""Pencil fas fa-border fa-pencil-alt {PenClass}"" onclick=""fnPencilClick(this)"" pch=""14"" ind= ""{Ind}"" fld=""{PencilObj.PencilText}""></i>"
        End If

        If ImageFileName <> "" Then
            If ImageFileName = "man.svg" And bp.getProperty("FnxNahal") <> "" Then
                val = $"<img src='./assets/images/{bp.getProperty("FnxNahal")}'>" + val
            ElseIf ImageFileName = "chargingStationComtec.svg" And bp.getProperty("FnxDirot171") <> "" Then
                val = $"<img src='./assets/images/{bp.getProperty("FnxDirot171")}'>" + val
            ElseIf ImageFileName = "car.svg" And bp.getProperty("FnxDirot271") <> "" Then
                val = $"<img src='./assets/images/{bp.getProperty("FnxDirot271")}'>" + val
            Else
                val = $"<img src='./assets/images/{ImageFileName}'>" + val
            End If

        End If

        If sMailIcon <> "" Then 'ntg 20.05.24 adding an icon for a mail field
            val = sMailIcon
        End If

        If s.Chars(s.Length - 1) <> ">" Then
            s.Append(">")
        End If
        s.Append($"{val}")
        'ntg 28.06.23 added DisableFooterBtns
        If pHelpText <> "" AndAlso toolTipType = "1" Then
            s.Append($"<button Class='btn btn-sm btn-primary btn-dots' type='button' style='padding:0px !important;' onclick='ShowHelp(this);DisableFooterBtns(this)' helptext=""{pHelpText}""> 
                                   <i class='fa-regular fa-circle-info' style='padding-right:0.5rem;'></i>
                                 </button>") 'tt+vk 09.24
        End If
        If pHelpText <> "" AndAlso toolTipType = "4" Then
            s.Append($"<i class='fa-regular fa-circle-info tooltipType4' data-bs-toggle='tooltip' data-bs-original-title=""{pHelpText}"" aria-label=""{pHelpText}""></i>")
        End If

        s.Append($"</{sTag}> {sTail}")

        If pHelpText <> "" AndAlso (toolTipType = 2) Then
            s.Append($"<div class=""specialHelpText""> {pHelpText} </div>")
        End If

        If pHelpText <> "" AndAlso (toolTipType = 2 Or toolTipType = 3) Then
            'bp.m_sSideButton = $"<button Class='btn btn-sm btn-primary btn-dots' type='button' onclick='ShowHelp(this);DisableFooterBtns(this)' helptext=""{pHelpText}""> 
            '                       <i class='fas fa-info-circle'></i>
            '                     </button>"
            's.Append($"<button Class='btn btn-sm btn-primary btn-dots' type='button' onclick='ShowHelp(this);DisableFooterBtns(this)' helptext=""{pHelpText}""> 
            '                       <i class='fas fa-info-circle'></i>
            '                     </button></div>")

            s.Append($"</div>")
            s.Append("<script>fixHelpText();</script>") 'ntg 05.03.24 Function For special field description (To replace the i window)
        End If
        If bWithCrLf Then s.Append(vbCrLf)

        Return s.ToString
    End Function
    Friend Function FullCodePseudoButton(ByRef bp As BuildPage, Optional ByVal bWithCrLf As Boolean = True,
            Optional ByVal sTag As String = "table", Optional ByVal bMakeOk As Boolean = True,
            Optional ByVal bNbsp As Boolean = False) As String 'vk 07.10

        Dim s As New Text.StringBuilder(), v As Attrib
        Dim val As String = ""
        s.Append("<" + sTag)
        If BOOTSTRAP Then
            If bp IsNot Nothing Then
                If bp.m_sCurClass > "" Then s.Append(" Class=""").Append(bp.m_sCurClass).Append("""")
                If bp.m_sCurStyle > "" Then s.Append(" style=""").Append(bp.m_sCurStyle).Append("""")
            End If
        ElseIf sStyle > "" Then
            s.Append(" " + sStyle)
        End If
        Dim enu As IDictionaryEnumerator = hAttribs.GetEnumerator
        Do While enu.MoveNext
            v = enu.Value
            If Not v.sName Like "On*" Then
                Select Case v.sName
                    Case "type", "disabled"
                    Case "value"
                        val = v.sValue
                        If bMakeOk Then
                            qConv.MakeOk(val, bNbsp)
                            val = val.Replace(" ", "&nbsp;") 'vk 12.09
                        End If
                    Case Else
                        If BOOTSTRAP AndAlso (v.sName = "style" OrElse v.sName = "class") Then
                        Else
                            s.Append(" " + v.sName + "=""" + v.sValue + """")
                        End If
                End Select
            End If
        Loop
        s.Append(" ><tr><td id='440'>" + val + "</td></tr></" + sTag + ">" + sTail)
        If bWithCrLf Then s.Append(vbCrLf) 'vk 01.06
        Return s.ToString
    End Function

    Sub Dispose()
        qConv = Nothing
    End Sub
End Class

Friend Class PencilProperties
    Public PencilText As String = ""
    Public Color As Boolean = False
    Friend Sub New(pPencilText As String, pColor As Boolean)
        PencilText = pPencilText
        Color = pColor
    End Sub
End Class

Friend Class Style
    Private hAttribs As Hashtable
    Friend Sub New()
        hAttribs = New Hashtable()
    End Sub
    Friend Sub Add(ByVal sName As String, ByVal sValue As String)
        If sValue = "" Then Exit Sub
        Try
            If hAttribs.ContainsKey(sName) Then hAttribs.Remove(sName)
            hAttribs.Add(sName, New Attrib(sName, sValue))
        Catch e As Exception
            e = e
        End Try
    End Sub
    Friend Sub Add(ByVal sName As String, ByVal nValue As Integer)
        Try
            Select Case sName
                Case "text-align", "font-size" 'font-size vk 09.24
                    If hAttribs.ContainsKey(sName) Then hAttribs.Remove(sName)
                    hAttribs.Add(sName, New Attrib(sName, nValue.ToString + "px"))
            End Select
        Catch e As Exception
            e = e
        End Try
    End Sub
    Friend Sub Add(ByVal sName As String, ByVal nValue As Double) 'vk 05.15
        Try
            If hAttribs.ContainsKey(sName) Then hAttribs.Remove(sName)
            Dim nInt As Integer = Fix(nValue)
            Select Case Math.Abs(nValue) - Math.Abs(nInt)
                Case Is < 0.25
                    hAttribs.Add(sName, New Attrib(sName, nInt.ToString + "px"))
                Case Is < 0.75
                    hAttribs.Add(sName, New Attrib(sName, nInt.ToString + ".5px"))
                Case Else
                    hAttribs.Add(sName, New Attrib(sName, (nInt + Math.Sign(nInt)).ToString + "px"))
            End Select
        Catch e As Exception
            e = e
        End Try
    End Sub

    Friend Function FullCode() As String
        Dim s As New Text.StringBuilder(), v As Attrib
        If hAttribs.Count > 0 Then
            s.Append("style='")
            Dim enu As IDictionaryEnumerator = hAttribs.GetEnumerator
            Do While enu.MoveNext
                v = enu.Value
                s.Append(v.sName + ":" + v.sValue + ";")
            Loop
            s.Append("'")
        End If
        Return s.ToString
    End Function
End Class

Friend Class Node
    Private qConv As New Conv() 'vk 11.09
    Private hAttribs As Hashtable
    Private a As Attrib
    Private sElement As String
    Friend NodeType As String

    'Friend oOrigRd As Xml.XmlTextReader 'vk 09.20
    Friend Sub New()
        hAttribs = New Hashtable()
    End Sub
    ' creates a hashtable of attributes and their possible values, that may be on XMLs.'
    Friend Sub New(ByRef rd As Xml.XmlTextReader, ByVal numL As Integer, ByVal NOTSPACE As String)
        Dim i As Integer
        sElement = rd.Name 'vk 07.04
        hAttribs = New Hashtable()
        For i = 0 To rd.AttributeCount - 1
            rd.MoveToAttribute(i)
            'rem vk 02.24
            'If rd.Name = "val" Then
            '    hAttribs.Add(rd.Name, New Attrib(rd.Name, rd.Value.Trim()))
            'Else
            If rd.Name = "num" Then
                hAttribs.Add(rd.Name, New Attrib(rd.Name, rd.Value.Replace(" ", NOTSPACE)))
            ElseIf rd.Name = "val" AndAlso rd.Value.Trim() = "" Then
                hAttribs.Add(rd.Name, New Attrib(rd.Name, "")) 'vk 02.24++
            Else
                hAttribs.Add(rd.Name, New Attrib(rd.Name, rd.Value))
            End If

        Next i
        rd.MoveToElement()
        'oOrigRd = rd 'vk 09.20
    End Sub

    Friend Sub AddIfNo(ByVal sName As String, Optional ByVal sValue As String = " ")
        Try
            If Not hAttribs.ContainsKey(sName) Then
                hAttribs.Add(sName, New Attrib(sName, sValue)) 'vk 07.04
            End If
        Catch e As Exception
            e = e
        End Try
    End Sub
    Friend Function ParamYes(ByVal sName As String) As Boolean
        Try
            If hAttribs.ContainsKey(sName) Then
                a = hAttribs(sName)
                'Return a.sValue = "Y" Or a.sValue = "E" 
                Return a.sValue = "Y" Or a.sValue = "E" 'ntg 02.07.23 added the option of  Or a.sValue = "E" 
            Else
                Return False
            End If
        Catch e As Exception
            Return False
        End Try
    End Function
    Friend Function ParamIn(ByVal sName As String, ByVal sChars As String) As Boolean 'vk 07.04
        Try
            If hAttribs.ContainsKey(sName) Then
                a = hAttribs(sName)
                If a.sValue = "" Then
                    Return False
                Else
                    Return sChars.IndexOf(a.sValue) >= 0
                End If
            Else
                Return False
            End If
        Catch e As Exception
            Return False
        End Try
    End Function
    Friend Function ParamIn(ByVal sName As String, ByVal sChars As String, ByVal nPos As Integer) As Boolean 'vk 05.05
        Try
            If hAttribs.ContainsKey(sName) Then
                a = hAttribs(sName)
                If Len(a.sValue) < nPos Then
                    Return False
                Else
                    Return sChars.IndexOf(Mid(a.sValue, nPos, 1)) >= 0
                End If
            Else
                Return False
            End If
            a = hAttribs(sName)
        Catch e As Exception
            Return False
        End Try
    End Function
    Friend Function ParamExists(ByVal sName As String) As Boolean 'vk 05.04
        Try
            If hAttribs.ContainsKey(sName) Then
                a = hAttribs(sName)
                Return a.sValue.Trim <> ""
            Else
                Return False
            End If
        Catch e As Exception
            Return False
        End Try
    End Function

    ' receives a tag element, checks if it exists in a hashtable, and if it is numeric, converts it into an int.
    Friend Function ParamVal(ByVal sName As String) As Integer
        Try
            If hAttribs.ContainsKey(sName) Then
                a = hAttribs(sName)
                If IsNumeric(a.sValue) Then
                    Return Int32.Parse(a.sValue)
                Else
                    Return 0
                End If
            Else
                Return 0
            End If
        Catch e As Exception
            Return 0 'vk 09.05
        End Try
    End Function

    Friend Sub ReplaceWin(filName As String, recName As String) 'ntg 31.05.23
        Try
            If filName.TrimEnd() = "HFFLNAVFM" AndAlso recName.TrimEnd() = "ROPTION" Then
                If hAttribs.ContainsKey("win") Then hAttribs.Remove("win")
                hAttribs.Add("win", New Attrib("win", " ")) 'vk 07.04

            End If
        Catch e As Exception
            Return
        End Try
    End Sub



    Friend Property Param(ByVal sName As String) As String
        Get
            Try
                If hAttribs.ContainsKey(sName) Then
                    a = hAttribs(sName)
                    Return a.sValue
                Else
                    Return Nothing
                End If
            Catch e As Exception
                Return Nothing
            End Try
        End Get
        Set(ByVal sValue As String)
            Try
                If hAttribs.ContainsKey(sName) Then hAttribs.Remove(sName)
                hAttribs.Add(sName, New Attrib(sName, sValue)) 'vk 07.04
            Catch e As Exception
                e = e
            End Try
        End Set
    End Property
    Friend Function FullCode(ByVal sIncludeXml As String, ByVal sBkg As String) As String 'vk 07.04, 08.06
        If sIncludeXml <> "true" Then Return "" 'vk 09.06
        Dim s As New Text.StringBuilder(), v As Attrib, r As String
        s.Append(" " + sElement + " xxx=""" + sBkg + """")
        Dim enu As IDictionaryEnumerator = hAttribs.GetEnumerator
        Do While enu.MoveNext
            v = enu.Value
            r = v.sName : If r Is Nothing Then r = ""
            r = r.Replace(">", "&gt;")
            s.Append(" " + r)
            r = v.sValue : If r Is Nothing Then r = ""
            qConv.MakeOk(r)

            s.Append("=""" + r + """")
        Loop
        Return "<!--" + s.ToString + " -->" + vbCrLf
    End Function
    '=== shortcuts for oNode: ===
    Friend Function LongName() As String 'vk 09.05
        If ParamIn("inp", "PQ") AndAlso Not ParamYes("pf4") Then 'vk 02.09
            'Return Param("num") + "0000" + Param("typ") + Param("len") + Param("dec") 'vk 10.08 '--rem ntg 17.07.24
            If ParamExists("dec") Then 'ntg 17.07.24 added the if condition in case there is a inp=P field and no "dec" parameter
                Return Param("num") + "0000" + Param("typ") + Param("len") + Param("dec") 'vk 10.08
            Else
                Return Param("num") + "0000" + Param("typ") + Param("len") + Param("dec") + "00"
            End If
        Else
            If ParamExists("dec") Then
                Return Param("num") + Param("ind") + Param("typ") + Param("len") + Param("dec")
            Else
                Return Param("num") + Param("ind") + Param("typ") + Param("len") + "00" 'rk 11.07.24 override for case of Menu Shortcut, OP, typ N, without dec
            End If
        End If
    End Function

    Friend Function ValModified() As String 'vk 05.05
        'code for combo when it is string
        Dim val As String = Param("val")
        If val Is Nothing OrElse (Param("typ") <> "A" AndAlso Not IsNumeric(val)) Then
            val = "000"
        ElseIf Param("typ") = "A" Then
            val = val.PadRight(ParamVal("len"), " ") 'vk 01.04
        End If
        Return val
    End Function

    Friend Function InMiddle(ByVal oNodeScreen As Node) As Boolean
        If oNodeScreen.ParamVal("fdsp") = 1 Then
            Return ParamVal("col") <= 66 AndAlso ParamVal("col") + ParamVal("wd") >= 66
        Else
            Return ParamVal("col") <= 40 AndAlso ParamVal("col") + ParamVal("wd") >= 40
        End If
    End Function
    Friend Function IsErrMsg(ByVal oNodeScreen As Node) As Boolean
        If oNodeScreen.ParamVal("fdsp") = 1 Then
            Return (ParamVal("lin") = 25 OrElse ParamVal("lin") = 26) AndAlso ParamVal("wd") > 65
        Else
            Return (ParamVal("lin") = 22 OrElse ParamVal("lin") = 23) AndAlso ParamVal("wd") > 65
        End If
    End Function

    Function FontSize(ByRef oNodeScreen As Node, ByRef bp As BuildPage, ByVal bTop132 As Boolean) As Integer 'vk 02.07
        Dim n As Integer = ParamVal("psz")

        If ParamVal("pfn") = 8 Then
            n = bp.FixedFontSize(CInt(n * 0.6 * bp.iScaleWidth / oNodeScreen.CellWidth_1024())) 'vk 07.10
        Else
            n = bp.ResizeFont(n, False, False, bTop132)
            If ParamVal("pht") = 1 Then
                Dim nMax As Integer = bp.iScaleLin + 2
                If n > nMax Then n = nMax
            End If
        End If
        Return n
    End Function

    Function Hafaka2or3(filVal As String) As Boolean 'vk 05.08.24 fixing appearence of fields that are set to lines 2,3 in xml
        Return (ParamVal("lin") = 2 OrElse ParamVal("lin") = 3) ' AndAlso (filVal.Trim = "HFSCREEN" OrElse filVal.Trim = "HFFLNAVFM")
    End Function

    'vk 10.20
    'Function ForBoo(sPurpose As String, Optional nScreenWid As Integer = 80, Optional winVal As String = "", Optional fdspVal As Integer = 0) As Boolean
    Function ForBoo(sPurpose As String, Optional nScreenWid As Integer = 80, Optional winVal As String = "", Optional fdspVal As Integer = 0, Optional filVal As String = "") As Boolean 'vk 05.08.24 fixing appearence of fields that are set to lines 2,3 in xml
        If Param("pch") IsNot Nothing OrElse Param("pchl") IsNot Nothing Then
            Return False
        End If
        Select Case sPurpose
            Case "middle"
                Return ParamVal("lin") = 1 AndAlso
                    ParamVal("col") <= nScreenWid \ 2 AndAlso
                    ParamVal("col") + Width() - 1 >= nScreenWid \ 2 AndAlso winVal <> "S"
            Case "title"
                Return (ParamVal("lin") <> 1 OrElse winVal = "S") AndAlso Param("num") = "TITLE~~~~~"
            Case "teur"
                Return (ParamVal("lin") <> 1 OrElse winVal = "S") AndAlso Param("num") = "TEUR~~~~~~" AndAlso
                    ParamYes("phi") AndAlso ParamVal("pbg") = 43
            Case "label"
                If Param("num") = "HDRNAM~~~~" Then Return True
                'If (ParamVal("lin") = 3 OrElse ParamVal("lin") = 2 OrElse (ParamVal("lin") = 23 AndAlso fdspVal = 0)) AndAlso winVal <> "S" Then 'added  OrElse ParamVal("lin") = 23 for SHUROT SIKUM by Arnon's request 08.05.23
                If (Hafaka2or3(filVal) OrElse (ParamVal("lin") = 23 AndAlso fdspVal = 0)) AndAlso winVal <> "S" Then
                    Return Left(Param("num"), 3) = "S00" OrElse Left(Param("num"), 3) = "$00"
                Else
                    Return Param("inp") Is Nothing
                End If
            Case "graph" 'vk 05.21
                Select Case ParamVal("tch")
                    Case 4, 5, 6 : Return True
                    Case Else : Return False
                End Select
            Case "DatePicker"
                Select Case Param("typ")
                    Case "D", "Y", "H", "Q"
                        Select Case Param("inp")
                            Case "I", "K", "R" : Return True
                            Case Else : Return False
                        End Select
                    Case Else : Return False
                End Select
        End Select
    End Function
    Function Width() As Integer
        If ParamExists("wd") Then
            Return ParamVal("wd")
        Else
            Return ParamVal("len")
        End If
    End Function

    Friend Function Modal() As Boolean
        Return ParamIn("win", "YSGC")
    End Function

    Friend Function CellWidth_1024() As Integer
        Return IIf(ParamVal("fdsp") = 0, 10, 6)
    End Function

    'vk 06.09
    Friend Function SpoolFont(ByRef bp As BuildPage) As String
        Dim sSpoolFont As String = bp.getPropertyPset("SpoolFont", Me)
        'If ParamExists("pset") Then
        '    sSpoolFont = bp.getProperty(Param("pset") + "_SpoolFont")
        'End If
        'If sSpoolFont = "" Then sSpoolFont = bp.getProperty("SpoolFont")
        If sSpoolFont = "" Then sSpoolFont = "Courier New"
        Return sSpoolFont
    End Function
    Friend Function SpoolFontFixed(ByRef bp As BuildPage) As Boolean
        Return SpoolFont(bp).ToUpper = "Courier New".ToUpper AndAlso ParamYes("spo") 'vk 06.10
    End Function
    Friend Function ScrollSide(ByRef bp As BuildPage) As String 'vk 09.06
        Select Case True
            Case bp.getProperty("ScrollSide") > "" : Return bp.getProperty("ScrollSide")
            Case ParamVal("flr") = 0 : Return "right" 'inverted scroll
            Case Else : Return "left"
        End Select
    End Function

    Friend Function Plain(ByVal bp As BuildPage) As Boolean
        Select Case bp.getPropertyPset("Plain", Me)
            Case "true" : Return True
            Case "false" : Return False
            Case Else : Return ParamYes("spo")
        End Select
    End Function

    Friend Function GraphicsKey() As String
        Return Param("cli") & "_" & Param("pset") & "_"
    End Function

    Sub Dispose()
        qConv = Nothing
    End Sub
End Class

Friend Class MenuLine
    Friend nLine, nColumn As Integer, sFld, sN As String, bSubMenu, bLastLine As Boolean
    Friend sLink As String = "" 'vk 11.05
    Friend sText As String = "" 'vk 05.06
    Friend bApp As Boolean = False 'vk 10.20
    Friend bIsMenuShortcut As Boolean = False 'rk 11.07.24 for menu shortcuts
    Friend nLen As Integer = 0 'rk 11.07.24 for menu shortcuts

    Friend Sub New(ByVal pLine As Integer)
        nLine = pLine
    End Sub
End Class

Friend Class MyNumber
    Friend Value As Integer
    Friend Sub New(ByVal pValue As Integer)
        Value = pValue
    End Sub
End Class

Friend Class Button
    Friend ControlItself As Control
    Friend oNode As Node
    Friend PicNumber As Integer
    Friend Sub New(ByVal pControl As Control, pNode As Node)
        ControlItself = pControl
        oNode = pNode
        PicNumber = 0
    End Sub
    Friend Sub New()
        'do nothing
    End Sub
    Friend Sub CopyFrom(ByVal oButton As Button)
        ControlItself = oButton.ControlItself
    End Sub
End Class

Friend Class FlexCombo
    Friend sN, sGroup, sKod As String, bBoss, bDouble As Boolean
End Class
