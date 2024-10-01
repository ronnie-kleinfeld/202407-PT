'all Collections turned to HashTables by vk 03.08

'for controls:
Friend Class Attrib
    Friend sName, sValue As String
    Friend Sub New(ByVal pName As String, ByVal pValue As String)
        sName = pName
        sValue = pValue
    End Sub
End Class

'vk 02.05
'for virtual keyboard:
Friend Class KbdField
    Friend sName As String = ""
    Friend nFrom As Integer = 0
    Friend nLength As Integer = 0
End Class

Friend Class Control
    Private qConv As New Conv() 'vk 11.09
    Private sStyle, sTail As String, a As Attrib
    Private hAttribs As Hashtable

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
    Friend Sub Add(ByVal sName As String, ByVal sValue As String, Optional ByVal bAlways As Boolean = False)
        If sValue = "" AndAlso Not bAlways Then Exit Sub
        Try
            If hAttribs.ContainsKey(sName) Then
                a = hAttribs(sName)
                a.sValue += sValue
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
    Friend Function FullCode(Optional ByVal sType As String = "input",
            Optional ByVal bWithCrLf As Boolean = True, Optional ByVal bMakeOk As Boolean = True,
            Optional ByVal bNbsp As Boolean = False) As String
        Dim s As New System.Text.StringBuilder(), v As Attrib
        Dim val As String = ""

        s.Append("<").Append(sType)
        If sStyle <> "" Then s.Append(" ").Append(sStyle)
        Dim enu As IDictionaryEnumerator = hAttribs.GetEnumerator
        Do While enu.MoveNext
            v = enu.Value
            If v.sName = "value" Then
                val = v.sValue
                If bMakeOk Then qConv.MakeOk(val, bNbsp)
                'val = val.Replace(" ", "&nbsp;") 'vk 12.09
                s.Append(" ").Append(v.sName).Append("=""").Append(val).Append("""")
            Else
                s.Append(" ").Append(v.sName).Append("=""").Append(v.sValue).Append("""")
            End If
        Loop
        s.Append(" >")
        If sTail.Trim > "" Then s.Append(vbCrLf).Append(sTail)
        If bWithCrLf Then s.Append(vbCrLf) 'vk 11.06
        Return s.ToString
    End Function
    Friend Function FullCodeSpan(Optional ByVal bWithCrLf As Boolean = True, _
            Optional ByVal sTag As String = "span", Optional ByVal bMakeOk As Boolean = True, _
            Optional ByVal bNbsp As Boolean = False) As String
        Dim s As New System.Text.StringBuilder(), v As Attrib
        Dim val As String = ""
        s.Append("<" + sTag)
        If sStyle <> "" Then s.Append(" " + sStyle)
        Dim enu As IDictionaryEnumerator = hAttribs.GetEnumerator
        Do While enu.MoveNext
            v = enu.Value
            Select Case v.sName
                Case "type"
                Case "value"
                    val = v.sValue
                    If bMakeOk Then
                        qConv.MakeOk(val, bNbsp)
                        val = val.Replace(" ", "&nbsp;") 'vk 12.09
                    End If
                Case Else
                    s.Append(" " + v.sName + "=""" + v.sValue + """")
            End Select
        Loop
        s.Append(" >" + val + "</" + sTag + ">" + sTail)
        If bWithCrLf Then s.Append(vbCrLf) 'vk 01.06
        Return s.ToString
    End Function

    Sub Dispose()
        qConv = Nothing
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
            If hAttribs.ContainsKey(sName) Then hAttribs.Remove(sName)
            hAttribs.Add(sName, New Attrib(sName, nValue.ToString + "px"))
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
        Dim s As New System.Text.StringBuilder(), v As Attrib
        s.Append("style='")
        Dim enu As IDictionaryEnumerator = hAttribs.GetEnumerator
        Do While enu.MoveNext
            v = enu.Value
            s.Append(v.sName + ":" + v.sValue + ";")
        Loop
        s.Append("'")
        Return s.ToString
    End Function

End Class

Friend Class Node
    Private qConv As New Conv() 'vk 11.09
    Private hAttribs As Hashtable, a As Attrib, sElement As String
    Friend Sub New()
        hAttribs = New Hashtable()
    End Sub
    Friend Sub New(ByRef rd As Xml.XmlTextReader, ByVal numL As Integer, ByVal NOTSPACE As String)
        Dim i As Integer
        sElement = rd.Name 'vk 07.04
        hAttribs = New Hashtable()
        For i = 0 To rd.AttributeCount - 1
            rd.MoveToAttribute(i)
            If rd.Name = "num" Then
                hAttribs.Add(rd.Name, New Attrib(rd.Name, rd.Value.Replace(" ", NOTSPACE)))
            Else
                hAttribs.Add(rd.Name, New Attrib(rd.Name, rd.Value))
            End If
        Next i
        rd.MoveToElement()
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
                Return a.sValue = "Y"
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
        Dim s As New System.Text.StringBuilder(), v As Attrib, r As String
        s.Append(" " + sElement + " xxx=""" + sBkg + """")
        Dim enu As IDictionaryEnumerator = hAttribs.GetEnumerator
        Do While enu.MoveNext
            v = enu.Value
            r = v.sName : If r Is Nothing Then r = ""
            r = r.Replace(">", "&gt;")
            s.Append(" " + r)
            r = v.sValue : If r Is Nothing Then r = ""
            qConv.MakeOk(r)
            'r = r.Replace("&", "&amp;")
            'r = r.Replace("""", "&quot;")
            'r = r.Replace("'", "&#39;")
            'r = r.Replace("<", "&lt;")
            'r = r.Replace(">", "&gt;")
            s.Append("=""" + r + """")
        Loop
        Return "<!--" + s.ToString + " -->" + vbCrLf
    End Function
    'vk 11.09
    Friend Function ShiftedFirstLine(ByVal oNodeScreen As Node, ByRef bp As BuildPage) As Boolean
        Return oNodeScreen.ParamIn("srg", "ABC") AndAlso ParamVal("lin") = 1 _
            AndAlso Not oNodeScreen.Modal AndAlso Not bp.OrganizedButtons(False)
        'bp.getProperty("Design") <> "lex" 'vk 06.10, 01.12
    End Function
    'vk 05.09
    '=== shortcuts for oNodeScreen: ===
    'vk 09.07, 01.09, 06.10
    Friend Function Modal() As Boolean
        Return ParamIn("win", "YSGC")
    End Function
    Friend Function Drawn() As Boolean
        Return ParamIn("win", "RT")
    End Function
    Friend Function CellWidth_1024() As Integer
        Return IIf(ParamVal("fdsp") = 0, 10, 6)
    End Function
    Friend Function CellHeight_1024() As Integer
        Return IIf(ParamVal("fdsp") = 0, 24, 21)
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
    Friend Function FontColor(ByVal bckg As String, ByVal forg As String,
        ByVal bp As BuildPage, ByVal sClass As String, ByVal bPdf As Boolean) As String
        Dim s As String = ""
        s += "BACKGROUND-COLOR:" + bckg + ";" + vbCrLf
        s += "COLOR:" + forg + ";" + vbCrLf
        If sClass = "" Then
            s += "FONT-FAMILY:" + bp.getProperty("BodyFont") + ";" + vbCrLf
            s += "FONT-SIZE:" + bp.SizeByClass(bp.sFontSize, sClass, bPdf, Me).ToString + "px;" + vbCrLf
        Else
            s += "FONT-FAMILY:Courier New;" + vbCrLf
            s += "FONT-SIZE:" + bp.SizeByClass(bp.sFontSizeFixed, sClass, bPdf, Me).ToString + "px;" + vbCrLf
        End If
        Return s
    End Function
    Friend Function FontColor(ByVal bckg As String, ByVal forg As String,
            ByVal bp As BuildPage, ByVal Compon As String, ByVal sClass As String, ByVal bPdf As Boolean) As String
        Dim s As String = ""
        If Compon.Chars(0) = "1" Then s += "BACKGROUND-COLOR:" + bckg + ";" + vbCrLf
        If Compon.Chars(1) = "1" Then s += "COLOR:" + forg + ";" + vbCrLf
        If sClass = "" Then
            If Compon.Chars(2) = "1" Then s += "FONT-FAMILY:" + bp.getProperty("BodyFont") + ";" + vbCrLf
            If Compon.Chars(3) = "1" Then s += "FONT-SIZE:" + bp.SizeByClass(bp.sFontSize, sClass, bPdf, Me).ToString + "px;" + vbCrLf
        Else
            If Compon.Chars(2) = "1" Then s += "FONT-FAMILY:" + SpoolFont(bp) + ";" + vbCrLf
            If SpoolFontFixed(bp) Then
                If Compon.Chars(3) = "1" Then s += "FONT-SIZE:" + bp.SizeByClass(bp.sFontSizeFixed, sClass, bPdf, Me).ToString + "px;" + vbCrLf
            Else
                If Compon.Chars(3) = "1" Then s += "FONT-SIZE:" + bp.SizeByClass(bp.sFontSize, sClass, bPdf, Me).ToString + "px;" + vbCrLf
            End If
        End If
        Return s
    End Function
    Friend Function HeightPos(
            ByVal bp As BuildPage, ByVal sClass As String, ByVal bPdf As Boolean) As String
        Dim s As String = ""
        Dim sHei As String = Hei(bp, sClass, bPdf)
        s += "height:" + sHei + vbCrLf
        's += "padding:0;" + vbCrLf
        's += "position:absolute;" + vbCrLf
        Return s
    End Function
    Friend Function HeightPos(
            ByVal bp As BuildPage, ByVal Compon As String, ByVal sClass As String, ByVal bPdf As Boolean) As String
        Dim s As String = ""
        Dim sHei As String = Hei(bp, sClass, bPdf)
        If Compon.Chars(0) = "1" Then s += "height:" + sHei + vbCrLf
        If Compon.Chars(1) = "1" Then s += "line-height:" + sHei + vbCrLf
        If Compon.Chars(2) = "1" Then s += "padding:0;" + vbCrLf
        If Compon.Chars(3) = "1" Then s += "position:absolute;" + vbCrLf
        Return s
    End Function
    Private Function Hei(ByVal bp As BuildPage, ByVal sClass As String, ByVal bPdf As Boolean) As String 'vk 02.06
        Dim nDelta As Integer
        If sClass = "" Then
            nDelta = bp.ResolValue(1, 2, 3, False, "vert")
        Else
            nDelta = 2
        End If
        Return bp.SizeByClass((bp.iScaleLin - nDelta).ToString, sClass, bPdf, Me).ToString + "px;"
    End Function
    'vk 03.10
    Friend Function Ortogonal(ByVal bp As BuildPage) As Boolean
        Return ParamExists("fort") OrElse bp.OrganizedButtons(False)
    End Function
    'vk 06.10
    Friend Function TopLength() As Integer
        If ParamVal("fdsp") = 0 OrElse ParamYes("dspa") Then
            Return 80
        Else
            Return 132
        End If
    End Function
    'vk 08.16
    Friend Function Plain(ByVal bp As BuildPage) As Boolean
        Select Case bp.getPropertyPset("Plain", Me)
            Case "true" : Return True
            Case "false" : Return False
            Case Else : Return ParamYes("spo")
        End Select
    End Function
    'vk 05.20
    Friend Function GraphicsKey() As String
        Return Param("cli") & "_" & Param("pset") & "_"
    End Function

    Sub Dispose()
        qConv = Nothing
    End Sub
End Class

'vk 01.06
Friend Class TabIndex
    Friend Chain As String, Tab As Integer, ControlName As String
    Friend Sub New(ByVal sChain As String, ByVal nTab As Integer, ByVal sN As String)
        Chain = sChain
        Tab = nTab
        ControlName = sN 'vk 09.06
    End Sub
End Class

