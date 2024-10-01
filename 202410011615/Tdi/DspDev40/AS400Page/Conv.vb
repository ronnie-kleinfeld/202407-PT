Imports Comtec.TIS  ' NET20

Friend Class Conv
    Dim isEnglish As Boolean = False
    Function RetIsEnglish()
        Return isEnglish
    End Function

    Function ConvAlf_Simple(ByRef bp As BuildPage, ByVal sVal As String, ByRef oNodeScreen As Node, ByVal sTextPrl As String) As String 'vk 03.08
        If oNodeScreen.ParamVal("flang") = 1 Then
            isEnglish = True
        End If


        Select Case sTextPrl
            Case "Y" : HebOnce(bp, sVal)
            Case "N"
            Case Else
                If oNodeScreen.ParamVal("flr") = 0 Then
                    HebOnce(bp, sVal)
                End If
        End Select
        Return sVal
    End Function

    Function ConvAlf(ByRef bp As BuildPage, ByVal sVal As String,
        ByVal sName As String, ByVal sDirection As String, ByVal dir As String,
        ByVal b254 As Boolean, ByVal bTrim As Boolean, ByVal bUni As Boolean, ByVal bNbsp As Boolean,
        ByVal flr As String, Optional ByVal bMakeOk As Boolean = False) As String

        Dim iSize As Integer

        If sName = "" Then
            iSize = Len(sVal) 'vk 09.05
        ElseIf sName.Substring(bp.typS, bp.typL) = "A" Then
            iSize = Int32.Parse(sName.Substring(bp.lenS, bp.lenL))
        Else
            Return ""
        End If

        If sDirection = "AS2PC" Then
            ' AS2PC
            If bUni Then
                sVal = Left(sVal, iSize * 4) 'vk 03.05
            Else
                sVal = Left(sVal, iSize) 'vk 01.05
            End If
            If bTrim Then
                If dir = "rtl" Then 'vk 01.05
                    sVal = sVal.TrimStart
                Else
                    sVal = sVal.TrimEnd
                End If
            End If
            Select Case True 'ms+vk 06.05
                Case bUni
                    Dim o As New PocketKnife.Info()
                    sVal = o.FromUni(sVal) 'vk 03.05
                    o = Nothing
                Case b254
                    Dim o As New PocketKnife.Info()
                    sVal = o.Heb11(sVal) 'vk 02.07
                    o = Nothing
                Case dir = "rtl" 'flr = "0"
                    HebOnce(bp, sVal)
            End Select
            If bMakeOk Then
                MakeOk(sVal)
            End If
            If bNbsp AndAlso Not b254 Then
                sVal = sVal.Replace(" ", "&nbsp;") 'vk 11.05
            End If
        Else
            ' PC2AS
            sVal = sVal.Replace(ChrW(160), ChrW(32)) 'vk 12.05
            MakeNotOk(sVal) 'vk 05.11
            If dir = "ltr" Then 'vk 05.03
                If sVal.Length > iSize Then
                    sVal = Left(sVal, iSize) 'vk 11.16
                Else
                    sVal = sVal.PadRight(iSize)
                End If
            Else
                If Not bUni Then
                    HebOnce(bp, sVal) 'vk 07.05
                End If
                If sVal.Length > iSize Then
                    sVal = Right(sVal, iSize) 'vk 11.16
                Else
                    sVal = sVal.PadLeft(iSize)
                End If
            End If
            If bUni Then
                Dim o As New PocketKnife.Info()
                sVal = o.ToUni(sVal) 'vk 03.05
                o = Nothing
            End If
        End If
        Return sVal

    End Function

    Private Sub HebOnce(ByRef bp As BuildPage, ByRef sVal As String) 'vk 08.08
        Dim sLang As String
        Dim i As Integer
        Dim b As Boolean
        If bp.getProperty("HebrewByLetters") = "true" Then
            b = False
            For i = 1 To Len(sVal)
                Select Case AscW(Mid(sVal, i, 1))
                    Case 1488 To 1514
                        b = True
                        Exit For
                End Select
            Next
        Else
            b = Trim(sVal) > ""
        End If
        If b Then
            Dim oConv As ConvCom = New ConvCom()
            sVal = oConv.RevHeb(sVal, sLang)
            oConv = Nothing
        End If
    End Sub

    Function ConvDate(ByRef bp As BuildPage, ByVal sVal As String,
        ByVal sName As String, ByVal sDirection As String, Optional ByVal Type As String = "") As String

        Dim iSize As Integer
        Dim sType As String
        Dim Zero6 As String = "000000"

        sType = If(sName = "", Type, sName.Substring(bp.typS, bp.typL))

        Select Case sType
            Case "D", "H", "Q" 'Q vk 07.05
                iSize = 6
            Case "Y", "B" 'ntg 05.03.24 added "B"
                iSize = 8
            Case Else
                Return ""
        End Select

        If sDirection = "AS2PC" Then
            ' AS2PC

            sVal = Right(sVal.PadLeft(iSize, "0"), iSize) 'Right vk 09.03

            Select Case sType
                Case "D", "H", "Q" 'Q vk 07.05
                    If sVal = Zero6 Then Return ""
                Case "Y", "B" 'ntg 05.03.24 added "B"
                    If sVal.Trim("0") = "" Then Return ""
            End Select

            Select Case sType
                Case "D"
                    sVal = sVal.Substring(0, 2) + "/" + sVal.Substring(2, 2) + "/" + sVal.Substring(4, 2)
                Case "Y", "B" 'ntg 05.03.24 added "B"
                    sVal = sVal.Substring(0, 2) + "/" + sVal.Substring(2, 2) + "/" + sVal.Substring(4, 4)
                Case "H", "Q" 'Q vk 07.05
                    sVal = sVal.Substring(0, 2) + "/" + sVal.Substring(2, 4)
                Case Else
                    Return ""
            End Select

            Return sVal

        Else ' PC2AS
            sVal = sVal.Replace("/", "")
            sVal = sVal.PadLeft(iSize, "0")

            Select Case sType
                Case "H", "Q"
                    '2019-11-01 -> 112019
                    Dim a() As String = sVal.Split("-")
                    If UBound(a) >= 2 Then
                        sVal = Right(a(1).PadLeft(2, "0"), 2) & Right(a(0).PadLeft(4, "0"), 4)
                    End If
                Case "D"
                    '2019-11-23 -> 231119
                    Dim a() As String = sVal.Split("-")
                    If UBound(a) >= 2 Then
                        sVal = Right(a(2).PadLeft(2, "0"), 2) & Right(a(1).PadLeft(2, "0"), 2) & Right(a(0), 2)
                    End If
                Case "Y", "B" 'ntg 05.03.24 added "B"
                    '2020-11-23 -> 23112020
                    Dim a() As String = sVal.Split("-")
                    If UBound(a) >= 2 Then
                        sVal = Right(a(2).PadLeft(2, "0"), 2) & Right(a(1).PadLeft(2, "0"), 2) & Right(a(0).PadLeft(4, "0"), 4)
                    End If
            End Select

            Return sVal
        End If
    End Function

    Function ConvTime(ByRef bp As BuildPage, ByVal sVal As String,
        ByVal sName As String, ByVal sDirection As String) As String

        ' --------------------------------------------
        ' sVal - value of control
        ' sName - name of control FaaaaaaaaabbbbFsssssdd
        ' sDirection - AS2PC or PC2AS
        ' --------------------------------------------
        Dim sType As String
        Dim sHour As String = ""
        Dim sMin As String = ""
        Dim sSec As String = ""

        If sVal.Trim = "" Then
            sVal = "0"
        End If

        sType = sName.Substring(bp.typS, bp.typL)

        If sDirection = "AS2PC" Then
            ' AS2PC
            If Not IsNumeric(sVal) Then Return "" 'moved here vk 07.03
            Select Case sType
                Case "T"
                    sVal = sVal.PadLeft(6, "0")
                    sVal = sVal.Substring(0, 2) + ":" + sVal.Substring(2, 2) + ":" + sVal.Substring(4, 2)
                Case "U"
                    sVal = sVal.PadLeft(4, "0")
                    sVal = sVal.Substring(0, 2) + ":" + sVal.Substring(2, 2)
            End Select
            Return sVal

        Else
            ' PC2AS
            sVal = sVal.Replace(":", "")
            If Not IsNumeric(sVal) Then Return "0" 'moved here + 0 vk 07.03
            Select Case sType
                Case "T"
                    sVal = sVal.PadLeft(6, "0")
                    sHour = sVal.Substring(0, 2)
                    sMin = sVal.Substring(2, 2)
                    sSec = sVal.Substring(4, 2)
                Case "U"
                    sVal = sVal.PadLeft(4, "0")
                    sHour = sVal.Substring(0, 2)
                    sMin = sVal.Substring(2, 2)
                    sSec = "00"
            End Select

            'vk 11.08
            If Int32.Parse(sHour) > 23 OrElse Int32.Parse(sMin) > 59 OrElse Int32.Parse(sSec) > 59 Then
                Select Case sType
                    Case "T" : If sVal <> "999999" Then sVal = "000000"
                    Case "U" : If sVal <> "9999" Then sVal = "0000"
                End Select
            End If
            Return sVal

        End If
    End Function

    Function ConvNum(ByRef bp As BuildPage, ByVal sVal As String, ByVal sName As String,
            ByVal sDirection As String, ByVal edt As String, ByVal ewr As String) As String

        ' --------------------------------------------
        ' sVal - value of control
        ' sName - name of control FaaaaaaaaabbbbFsssssdd
        ' sDirection - AS2PC or PC2AS
        ' --------------------------------------------

        Dim sDefault As String
        If sDirection = "AS2PC" Then
            sDefault = ""
        Else
            sDefault = "+0"
        End If

        Dim iSize, iDec, iInt As Integer
        Dim sType As String
        Dim sForm As String

        If sVal Is Nothing OrElse sVal.Trim = "" Then sVal = "0" 'Nothing vk 07.13
        sType = sName.Substring(bp.typS, bp.typL)

        If sType = "S" OrElse sType = "N" Then
            iSize = Int32.Parse(sName.Substring(bp.lenS, bp.lenL))
            iDec = Int32.Parse(sName.Substring(bp.decS, bp.decL))
            Select Case iDec
                Case Is > iSize
                    Return sDefault
                Case Is > 0 'We have decimal point
                    iInt = iSize - iDec
                Case Else
                    iInt = iSize
            End Select
        Else
            Return sDefault
        End If

        If sDirection = "AS2PC" Then
            ' AS2PC
            If iInt < 1 Then
                sForm = "#"
            ElseIf edt = "J" Then
                sForm = "#,##0"
            Else
                sForm = "###0"
            End If

            If iDec > 0 Then
                sForm += "." + "".PadRight(iDec, "0")
            End If

            'vk 02.04
            Try
                If Double.Parse(sVal) = 0 AndAlso ewr <> "0" Then 'vk 05.07
                    sVal = ""
                ElseIf iDec = 0 Then
                    sVal = Convert.ToInt64(Long.Parse(sVal)).ToString(sForm) 'vk 11.03
                Else
                    sVal = (Double.Parse(sVal) / 10 ^ iDec).ToString(sForm)
                End If
                Return sVal
            Catch
                Return 0.ToString(sForm)
            End Try

        Else
            ' PC2AS
            sVal = sVal.Replace(",", "")
            sVal = sVal.Replace("_", "") 'vk 02.05

            Dim dVal As Double, lVal As Long 'vk 10.04
            Try
                If iDec = 0 Then
                    lVal = Long.Parse(sVal) 'vk 02.04
                    If lVal < 0 Then
                        sVal = Convert.ToInt64(-lVal).ToString("#0") 'vk 11.03
                        sVal = "-" + sVal
                    Else
                        sVal = Convert.ToInt64(lVal).ToString("#0") 'vk 11.03
                        sVal = "+" + sVal
                    End If
                Else
                    dVal = Double.Parse(sVal) * 10 ^ iDec
                    If dVal < 0 Then
                        sVal = Convert.ToInt64(-dVal).ToString("#0") 'vk 11.03
                        sVal = "-" + sVal
                    Else
                        sVal = Convert.ToInt64(dVal).ToString("#0") 'vk 11.03
                        sVal = "+" + sVal
                    End If
                End If
            Catch e As Exception
                'sVal = "0"
                sVal = sDefault 'ntg 11.04.24 vladi change- when typed non-numeric value in a numeric field, it had an exception
            End Try

            Return sVal
        End If
    End Function

    Sub MakeOk(ByRef bp As BuildPage, ByRef oNodeScreen As Node, ByRef nm As String) 'vk 01.04
        If nm = "" Then Exit Sub 'vk 02.04
        nm = ConvAlf_Simple(bp, nm, oNodeScreen, "")
        nm = nm.Replace("&", "&amp;")
        nm = nm.Replace("""", "&quot;")
        nm = nm.Replace("'", "&#39;")
        nm = nm.Replace("<", "&lt;")
        nm = nm.Replace(">", "&gt;")
    End Sub
    Sub MakeOk(ByRef nm As String, Optional ByVal bNbsp As Boolean = False) 'vk 07.04
        If nm = "" Then Exit Sub
        nm = nm.Replace("&", "&amp;")
        nm = nm.Replace("""", "&quot;")
        nm = nm.Replace("'", "&#39;")
        nm = nm.Replace("<", "&lt;")
        nm = nm.Replace(">", "&gt;")
        If bNbsp Then nm = nm.Replace(" ", "&nbsp;") 'vk 11.09
    End Sub
    Sub MakeNotOk(ByRef nm As String) 'vk 07.04
        If nm = "" Then Exit Sub
        nm = nm.Replace("&quot;", """")
        nm = nm.Replace("&#39;", "'")
        nm = nm.Replace("&lt;", "<")
        nm = nm.Replace("&gt;", ">")
        nm = nm.Replace("&nbsp;", " ") 'vk 01.08
        nm = nm.Replace("&amp;", "&") 'moved here vk 05.11
    End Sub
End Class
