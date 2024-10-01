Imports Comtec.Tis  ' NET20

Friend Class Conv

    Function ConvAlf(ByRef bp As BuildPage, ByVal sVal As String,
        ByVal sName As String, ByVal sDirection As String, ByVal dir As String,
        ByVal b254 As Boolean, ByVal bTrim As Boolean, ByVal bUni As Boolean, ByVal bNbsp As Boolean,
        ByVal flr As String, Optional ByVal bMakeOk As Boolean = False) As String
        ' --------------------------------------------
        ' sVal - value of control
        ' sName - name of control FaaaaaaaaabbbbAssdd
        ' sDirection - AS2PC or PC2AS
        ' --------------------------------------------

        Dim iSize As Integer
        'Dim iDec As Integer
        'Dim fVal As Double

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
        'Dim sHeb = bp.getProperty("Heb")
        'Dim sHeb As String = "" '"ConvCom_New"
        Dim sLang As String = ""
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
