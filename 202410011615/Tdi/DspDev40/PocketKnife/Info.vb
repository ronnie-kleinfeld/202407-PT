Imports System.Security.Cryptography
Imports System.IO
Imports System.Web
Imports System.Xml
Imports System.Security.Principal 'vk 08.07
'Imports Winnovative.WnvHtmlConvert 'vk 05.09
Imports System.Net.Mail
Imports System.DirectoryServices 'vk 11.15
Imports System.Text 'vk 04.21

Public Class Info

    'vk 08.07
    Dim LOGON32_LOGON_INTERACTIVE As Integer = 2
    Dim LOGON32_PROVIDER_DEFAULT As Integer = 0
    Dim LOGON_TYPE_NEW_CREDENTIALS As Integer = 9 'vk 04.17
    Dim LOGON32_PROVIDER_WINNT50 As Integer = 3 'vk 04.17
    Dim impersonationContext As WindowsImpersonationContext
    Declare Function LogonUserA Lib "advapi32.dll" (ByVal lpszUsername As String,
        ByVal lpszDomain As String,
        ByVal lpszPassword As String,
        ByVal dwLogonType As Integer,
        ByVal dwLogonProvider As Integer,
        ByRef phToken As IntPtr) As Integer
    Declare Auto Function DuplicateToken Lib "advapi32.dll" (
        ByVal ExistingTokenHandle As IntPtr,
        ByVal ImpersonationLevel As Integer,
        ByRef DuplicateTokenHandle As IntPtr) As Integer
    Declare Auto Function RevertToSelf Lib "advapi32.dll" () As Long
    Declare Auto Function CloseHandle Lib "kernel32.dll" (ByVal handle As IntPtr) As Long
    'vk 04.09
    Public Event DelFiles_Error(ByVal e As Exception)
    'vk 02.16
    Dim oAS400 As AS400.WinNet
    'vk 04.21
    'Private Declare Ansi Function OemToCharBuffA Lib "user32.dll" (ByVal oemstr As String, ByVal ansistr As StringBuilder, ByVal cnt As Integer) As Boolean
    Declare Function BiDiConvertAnsiToOem Lib "BIDI32.DLL" Alias "#3" (lpConvertInfo As CONVERTINFO, ByRef lpStrInfo As STRINFO) As Long
    Declare Function BiDiConvertOemToAnsi Lib "BIDI32.DLL" Alias "#2" (lpConvertInfo As CONVERTINFO, ByRef lpStrInfo As STRINFO) As Long
    Structure CONVERTINFO
        Dim hWndOwner As Long          'Owner Window
        Dim nOemCP As Long             'OEM code page
        Dim fOemLayout As Long         'OEM text layout
        Dim nWindowsCP As Long         'Windows code page
        Dim fControlChars As Long      'Output string BiDi control characters
        Dim fShowLayout As Long        'Show/hide Layout dialog item
        Dim nUILang As Long            'UI language
    End Structure
    Structure STRINFO
        Dim lpStrIn As String          'Input string
        Dim cchIn As Long              'Length of input string
        Dim lpStrOut As String         'Output string buffer
        Dim cchOut As Long             'Length of output string buffer
    End Structure
    Private Const key As String = "2L/g90khe+Sx0rDN0iySRPhyaCxwOtTFSCBkZOv/1HE="
    Private Const vec As String = "UrxN71doNmio2bIbK+zc0w=="

    Public Function ConvertPasswordDesToAes(ByVal passwordDes As String) As String
        Dim passwordAes As String
        passwordAes = EncryptPassword(DecryptPasswordDES(passwordDes))
        Return passwordAes
    End Function
    Public Function EncryptPassword(ByVal sSource As String) As String
        Dim rt As String = ""
        Try
            If sSource = Nothing Or sSource.Length <= 0 Then
                Throw New ArgumentNullException("sSource")
            End If

            Dim encrypted As Byte()
            Dim encryptor As ICryptoTransform

            '/------------------------------------------
            '/ Create an AesCryptoServiceProvider object
            '/ with the specified key And IV.
            '/-------------------------------------------
            Using aesAlg As AesCryptoServiceProvider = New AesCryptoServiceProvider()
                aesAlg.Key = Convert.FromBase64String(key)
                aesAlg.IV = Convert.FromBase64String(vec)
                '/-----------------------------------------------------
                '/ Create an encryptor to perform the stream transform.
                '/-----------------------------------------------------
                encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV)
                '/----------------------------------------
                '/ Create the streams used for encryption.
                '/----------------------------------------
                Using msEncrypt As MemoryStream = New MemoryStream()
                    Using csEncrypt As CryptoStream = New CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write)
                        Using swEncrypt As StreamWriter = New StreamWriter(csEncrypt)
                            '/-----------------------------
                            '/Write all data to the stream.
                            '/-----------------------------
                            swEncrypt.Write(sSource)
                        End Using
                        '/---------------------------------------------------
                        '/ Return the encrypted bytes from the memory stream.
                        '/---------------------------------------------------
                        encrypted = msEncrypt.ToArray()
                        rt = Convert.ToBase64String(encrypted)
                    End Using
                End Using
            End Using
        Catch e As Exception
            Return rt
        End Try

        Return rt
    End Function
    Public Function DecryptPassword(ByVal cipherText As String) As String
        Dim plaintext As String
        Dim decryptor As ICryptoTransform

        Try
            '/-----------------
            '/ Check arguments.
            '/-----------------
            If cipherText = Nothing Or cipherText.Length <= 0 Then
                Throw New ArgumentNullException("sSource")
            End If
            '/--------------------------------
            '/ Declare the string used to hold
            '/ the decrypted text.
            '/--------------------------------
            plaintext = Nothing

            '/------------------------------------------
            '/ Create an AesCryptoServiceProvider object
            '/ with the specified key And IV.
            '/------------------------------------------
            Using aesAlg As AesCryptoServiceProvider = New AesCryptoServiceProvider()
                aesAlg.Key = Convert.FromBase64String(key)
                aesAlg.IV = Convert.FromBase64String(vec)
                '/----------------------------------------------------
                '/ Create a decryptor to perform the stream transform.
                '/----------------------------------------------------
                decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV)
                '/----------------------------------------
                '/ Create the streams used for decryption.
                '/----------------------------------------
                Using msDecrypt As MemoryStream = New MemoryStream(Convert.FromBase64String(cipherText))
                    Using csDecrypt As CryptoStream = New CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read)
                        Using srDecrypt As StreamReader = New StreamReader(csDecrypt)
                            '/----------------------------------------------------
                            '/ Read the decrypted bytes from the decrypting stream
                            '/ And place them in a string.
                            '/----------------------------------------------------
                            plaintext = srDecrypt.ReadToEnd()
                        End Using
                    End Using
                End Using
            End Using
        Catch e As Exception
            Return ""
        End Try

        Return plaintext
    End Function
    Public Function DecryptPasswordDES(ByVal sSource As String) As String

        If sSource = "" Then Return ""
        Try
            Dim bySource As Byte() = Convert.FromBase64String(sSource)
            Dim des As New DESCryptoServiceProvider()
            'Dim Key As Byte() = {&HB7, &HB, &H4C, &H3B, &HD2, &HEB, &HA, &H6}
            'Dim IV As Byte() = {&H53, &HA1, &H10, &H27, &H80, &H41, &H94, &H4C}
            Dim Key As Byte() = {73, 239, 155, 215, 7, 234, 66, 14}
            Dim IV As Byte() = {172, 191, 28, 140, 177, 121, 236, 50}
            Dim ms As New MemoryStream()
            Dim cs As New CryptoStream(ms, des.CreateDecryptor(Key, IV), CryptoStreamMode.Write)
            Dim byRetArray As Byte()
            Try
                cs.Write(bySource, 0, bySource.Length)
                cs.FlushFinalBlock()
                byRetArray = ms.ToArray()
            Finally
                cs.Close()
                If Not cs Is Nothing Then cs.Dispose()
                cs = Nothing
                If Not ms Is Nothing Then
                    ms.Close()
                    ms.Dispose()
                End If
                ms = Nothing
            End Try
            Return System.Text.Encoding.Unicode.GetString(byRetArray)
        Catch e As Exception
            Return ""
        End Try

    End Function
    Public Function ToMqKey(ByVal sSource As String) As String

        Dim s As String = "", i As Integer
        For i = 1 To Len(sSource)
            s += Hex(Asc(Mid(sSource, i, 1))).PadLeft(2, "0")
        Next
        '"3ebe89e2-7a8a-41f9-be62-817e67c98c70\0"
        s = s.PadRight(32, "0")
        Return Mid(s, 1, 8) + "-" _
            + Mid(s, 9, 4) + "-" _
            + Mid(s, 13, 4) + "-" _
            + Mid(s, 17, 4) + "-" _
            + Mid(s, 21, 12) + "\0"

    End Function

    'vk 03.05
    Function ToUni(ByVal s As String) As String
        Dim i As Integer, s0 As String = ""
        For i = 1 To Len(s)
            s0 += Right("0000" + Hex(AscW(Mid(s, i, 1))), 4)
        Next
        Return s0
    End Function
    Function FromUni(ByVal s As String) As String
        Dim i As Integer, s0 As String = ""
        For i = 1 To Len(s) Step 4
            s0 += ChrW(Val("&H" + Mid(s, i, 4)))
        Next
        Return s0
    End Function

    Function Heb11(ByVal s As String) As String 'vk 02.07
        Dim i As Integer, s0 As String = ""
        For i = 1 To Len(s)
            Dim c As Char, j As Integer
            c = Mid(s, i, 1)
            j = InStr("()[]{}<>", c)
            If j > 0 Then c = Mid(")(][}{><", j, 1)
            s0 = ChrW(8238) + c + s0
        Next
        Return s0
    End Function
    Function UnHeb11(ByVal s As String) As String 'vk 12.07
        Dim i As Integer, s0 As String = ""
        For i = 1 To Len(s)
            Dim c As Char, j As Integer
            c = Mid(s, i, 1)
            If AscW(c) <> 8238 Then
                j = InStr("()[]{}<>", c)
                If j > 0 Then c = Mid(")(][}{><", j, 1)
                s0 = c + s0
            End If
        Next
        Return s0
    End Function

    'vk 08.07
    Function RunAndWait(ByVal sExe As String, ByVal sParam As String, ByVal nMaxSeconds As Integer) As String
        Dim myproc As Process
        Dim i As Integer = 0
        Try
            myproc = New Process()
            myproc.StartInfo.FileName = sExe
            myproc.StartInfo.Arguments = sParam
            myproc.Start()
            Do
                System.Threading.Thread.Sleep(1000)
                If myproc.HasExited Then Return ""
                If i > nMaxSeconds Then Return "Timeout"
                i += 1
            Loop
        Catch e As Exception
            Return e.Message
        End Try
    End Function

    'thanx to pv
    Function impersonateValidUser(ByVal dp As Comtec.TIS.Reader, ByVal sPrefix As String) As Boolean
        Dim userName, domain, password As String
        Dim bRetValue As Boolean = False
        Dim sLogonType As String ', rc As Integer
        userName = dp.getProperty(sPrefix + "NetworkUser")
        If userName = "" Then
            userName = dp.getProperty("NetworkUser")
            domain = dp.getProperty("NetworkDomain")
            password = DecryptPassword(dp.getProperty("NetworkPass"))
        Else
            domain = dp.getProperty(sPrefix + "NetworkDomain")
            password = DecryptPassword(dp.getProperty(sPrefix + "NetworkPass"))
        End If
        'vk 04.10 till here
        'If Application("WinAuthentication").ToString() = "1" Then
        '    Return
        'End If
        sLogonType = dp.getProperty(sPrefix + "LogonType") 'vk 04.17
        If sLogonType.Trim = "" Then
            sLogonType = "2"
        End If
        If userName > "" Then
            Dim tempWindowsIdentity As WindowsIdentity
            Dim token As IntPtr = IntPtr.Zero
            Dim tokenDuplicate As IntPtr = IntPtr.Zero
            If RevertToSelf() Then
                If LogonUserA(userName, domain, password, Val(sLogonType), LOGON32_PROVIDER_DEFAULT, token) <> 0 Then
                    If DuplicateToken(token, 2, tokenDuplicate) <> 0 Then
                        tempWindowsIdentity = New WindowsIdentity(tokenDuplicate)
                        impersonationContext = tempWindowsIdentity.Impersonate()
                        If Not impersonationContext Is Nothing Then
                            bRetValue = True
                        End If
                    End If
                End If
            End If
            If Not tokenDuplicate.Equals(IntPtr.Zero) Then
                CloseHandle(tokenDuplicate)
            End If
            If Not token.Equals(IntPtr.Zero) Then
                CloseHandle(token)
            End If
        End If
        Return bRetValue
    End Function
    Sub undoImpersonation()
        'If Application("WinAuthentication").ToString() = "1" Then
        '    Return
        'End If
        impersonationContext.Undo()
    End Sub
    'vk 02.16
    Function impersonateValidUser_AS400(ByVal dp As Comtec.TIS.Reader, ByVal sPrefix As String, ByVal sFolder As String) As String
        Dim userName As String, domain As String, password As String
        Dim sRetValue As String = ""
        userName = dp.getProperty(sPrefix + "NetworkUser")
        If userName = "" Then
            userName = dp.getProperty("NetworkUser")
            domain = dp.getProperty("NetworkDomain")
            password = DecryptPassword(dp.getProperty("NetworkPass"))
        Else
            domain = dp.getProperty(sPrefix + "NetworkDomain")
            password = DecryptPassword(dp.getProperty(sPrefix + "NetworkPass"))
        End If
        If userName > "" Then
            Try
                oAS400 = New AS400.WinNet
                sRetValue = oAS400.UseRecord(sFolder, userName, password, domain)
            Catch e As Exception
                sRetValue = e.Message
            End Try
        End If
        Return sRetValue
    End Function
    Sub undoImpersonation_AS400(ByVal sFolder As String)
        oAS400.Stop(sFolder)
    End Sub
    'Function GetUncSourcePath(ByVal driveLetter As String) As String
    '    'If String.IsNullOrEmpty(driveLetter) Then Throw New ArgumentNullException("driveLetter")
    '    If (driveLetter < "a" OrElse driveLetter > "z") AndAlso (driveLetter < "A" OrElse driveLetter > "Z") Then Return Nothing 'Throw New ArgumentOutOfRangeException("driveLetter", "driveLetter must be a letter from A to Z")
    '    Dim P As New Process()
    '    With P.StartInfo
    '        .FileName = "net"
    '        .Arguments = String.Format("use {0}:", driveLetter)
    '        .UseShellExecute = False
    '        .RedirectStandardOutput = True
    '        .CreateNoWindow = True
    '    End With
    '    P.Start()
    '    Dim T As String = P.StandardOutput.ReadToEnd()
    '    P.WaitForExit()
    '    Dim Line As String
    '    For Each Line In Split(T, vbNewLine)
    '        If Line.StartsWith("Remote name") Then Return Line.Replace("Remote name", "").Trim()
    '    Next
    '    Return Nothing
    'End Function

    Sub DelFiles(ByVal sPath As String, ByVal sMask As String,
            ByVal dp As Comtec.TIS.Reader, ByVal sPrefix As String) 'vk 04.09
        Dim bImpersonated As Boolean = False
        Try
            Dim sFiles As String() = Directory.GetFiles(sPath, sMask)
            'If sUser > "" Then
            bImpersonated = impersonateValidUser(dp, sPrefix)
            'End If
            Dim s As String
            For Each s In sFiles
                Try
                    File.Delete(s)
                Catch e As Exception
                    RaiseEvent DelFiles_Error(e)
                End Try
            Next
        Catch e As Exception
            RaiseEvent DelFiles_Error(e)
        Finally
            If bImpersonated Then
                undoImpersonation()
            End If
        End Try

    End Sub

    Function html2pdf(ByVal st1 As String, ByVal st2 As String, ByVal sLicenseKey As String,
            ByVal nPageSize As Integer, ByVal nPageOrientation As Integer, ByVal bSelectable As Boolean,
            ByVal nPageWidth As Integer, ByVal nLeftMargin As Integer, ByVal nRightMargin As Integer,
            ByVal nTopMargin As Integer, ByVal nBottomMargin As Integer, Optional ByVal sProductVer As String = "") As String 'vk 05.09

        Select Case sProductVer
            Case "1" 'vk 05.15
                Try
                    Dim pdfConverter As New Winnovative.PdfConverter
                    Dim pdfDocument As Winnovative.Document
                    pdfConverter.PdfDocumentOptions.PdfPageSize = Winnovative.PdfPageSize.A4 '?
                    pdfConverter.PdfDocumentOptions.PdfPageOrientation = IIf(nPageOrientation = 0, Winnovative.PdfPageOrientation.Portrait, Winnovative.PdfPageOrientation.Landscape) 'vk 05.16
                    pdfConverter.PdfDocumentOptions.PdfCompressionLevel = Winnovative.PdfCompressionLevel.Normal
                    'pdfConverter.PdfDocumentOptions.SinglePage = True
                    pdfConverter.PdfDocumentOptions.ShowHeader = False
                    pdfConverter.PdfDocumentOptions.ShowFooter = False
                    pdfConverter.HtmlViewerWidth = nPageWidth '800
                    pdfConverter.PdfDocumentOptions.LeftMargin = nLeftMargin '50
                    pdfConverter.PdfDocumentOptions.RightMargin = nRightMargin '10
                    pdfConverter.PdfDocumentOptions.TopMargin = nTopMargin '20
                    pdfConverter.PdfDocumentOptions.BottomMargin = nBottomMargin '20
                    pdfConverter.LicenseKey = sLicenseKey
                    'pdfConverter.LicenseKey = "qiQ3JTQlNDA2JTwrNSU2NCs0Nys8PDw8"
                    pdfDocument = pdfConverter.GetPdfDocumentObjectFromUrl(st1)
                    pdfDocument.Save(st2)
                    pdfDocument.Close()
                    Return "OK"
                Catch ex As Exception
                    Return ex.ToString
                End Try
            Case Else
                Dim fs As FileStream = Nothing, bw As BinaryWriter = Nothing
                Try
                    Dim pdfConverter As Winnovative.WnvHtmlConvert.PdfConverter = New Winnovative.WnvHtmlConvert.PdfConverter
                    pdfConverter.PdfDocumentOptions.PdfPageSize = nPageSize 'PdfPageSize.A4
                    pdfConverter.PdfDocumentOptions.PdfPageOrientation = nPageOrientation 'PDFPageOrientation.Portrait
                    pdfConverter.PdfDocumentOptions.PdfCompressionLevel = Winnovative.WnvHtmlConvert.PdfCompressionLevel.Normal
                    pdfConverter.PdfDocumentOptions.GenerateSelectablePdf = bSelectable 'True
                    pdfConverter.PdfDocumentOptions.ShowHeader = False
                    pdfConverter.PdfDocumentOptions.ShowFooter = False
                    pdfConverter.PageWidth = nPageWidth '800
                    pdfConverter.PdfDocumentOptions.LeftMargin = nLeftMargin '50
                    pdfConverter.PdfDocumentOptions.RightMargin = nRightMargin '10
                    pdfConverter.PdfDocumentOptions.TopMargin = nTopMargin '20
                    pdfConverter.PdfDocumentOptions.BottomMargin = nBottomMargin '20
                    pdfConverter.LicenseKey = sLicenseKey
                    Dim downloadBytes() As Byte = pdfConverter.GetPdfFromUrlBytes(st1)
                    fs = New FileStream(st2, FileMode.Create, FileAccess.Write, FileShare.ReadWrite)
                    bw = New BinaryWriter(fs)
                    bw.Write(downloadBytes)
                    Return "OK"
                Catch ex As Exception
                    Return ex.ToString
                Finally
                    If Not bw Is Nothing Then bw.Close()
                    If Not fs Is Nothing Then
                        fs.Close()
                        fs.Dispose()
                    End If
                    fs = Nothing
                    bw = Nothing
                End Try
        End Select

    End Function

    'ik 2012-01-30
    Public Function AntiXSSHtml(ByVal sStr As String, ByVal bFilter As Boolean) As String
        Dim strReturn As String = sStr
        If bFilter Then
            If sStr IsNot Nothing AndAlso sStr.Trim.Length > 0 Then
                strReturn = Microsoft.Security.Application.AntiXss.HtmlAttributeEncode(sStr)
            End If
        End If
        Return strReturn
    End Function

    'ik 2012-01-30
    Public Function InputIsOK(ByVal sStr As String) As Boolean
        Dim bOK As Boolean = True
        If sStr.IndexOf("--", StringComparison.OrdinalIgnoreCase) >= 0 Then
            bOK = False
        ElseIf sStr.IndexOf("<", StringComparison.OrdinalIgnoreCase) >= 0 Then
            bOK = False
        ElseIf sStr.IndexOf("execute", StringComparison.OrdinalIgnoreCase) >= 0 Then
            bOK = False
        ElseIf sStr.IndexOf("eval", StringComparison.OrdinalIgnoreCase) >= 0 Then
            bOK = False
        ElseIf sStr.IndexOf("script", StringComparison.OrdinalIgnoreCase) >= 0 Then
            bOK = False
        End If
        Return bOK
    End Function

    Public Sub SendMail(ByVal sFrom As String, ByVal sTo As String,
            ByVal sSubj As String, ByVal sBody As String, ByVal sServer As String)
        Dim v As String
        Dim mail As New MailMessage()
        mail.From = New MailAddress(sFrom)
        sTo = sTo.Replace(";", ",")
        For Each v In sTo.Split(",")
            mail.To.Add(v)
        Next
        mail.Subject = sSubj
        mail.Body = sBody
        Dim smtp As New SmtpClient(sServer)
        smtp.Send(mail)
    End Sub

    Public Sub ReloadWWA(ByVal sPool_One As String, ByRef sReturn As String, ByRef eReturn1 As Exception, ByRef eReturn2 As Exception,
                         Optional ByVal sGuid As String = "09CA1271-6745-4468-8E3A-876E51ED1114")  'vk 11.15

        Dim sWs_One As String = ""
        Dim sWs_Result As String '= ""
        Dim i As Integer = InStr(sPool_One, "+")

        If i > 0 Then
            sWs_One = Left(sPool_One, i - 1)
            sPool_One = Mid(sPool_One, i + 1)
        End If
        If sWs_One = "" Then
            sReturn = "WS URL not defined"
        Else
            Try
                Dim ws As WWA.MainService = New WWA.MainService
                ws.Url = sWs_One
                ws.Timeout = 30000 '30 sec
                ws.Credentials = System.Net.CredentialCache.DefaultCredentials 'vk 01.16
                sWs_Result = ws.Reload(sGuid)
                ws.Dispose()
                ws = Nothing
                If sWs_Result = "OK" Then
                    sReturn = "OK"
                    Exit Sub
                Else
                    sReturn = sWs_Result
                End If
            Catch ee As Exception
                eReturn1 = ee
                sReturn = ""
            End Try
        End If
        Try
            Dim applicationPool As DirectoryEntry = New DirectoryEntry("IIS://" + sPool_One)
            applicationPool.Invoke("Recycle")
            applicationPool.Dispose()
            applicationPool = Nothing
            sReturn = "OK"
        Catch ee As Exception
            eReturn2 = ee
        End Try

    End Sub

    Public Function ToNumeric(ByVal s1 As String, Optional ByVal nDelta As Integer = 0) As String 'vk 11.17
        Dim s2 As String = ""
        Dim i As Integer
        For i = 1 To Len(s1)
            s2 &= Format(Asc(Mid(s1, i, 1)) + nDelta, "00")
        Next
        Return s2
    End Function

    'vk 04.21
    Public Function Heb(s As String) As String
        Return Heb_Private(s, 38598, 28598)
    End Function
    Private Function Heb_Private(unicodeString As String, enc1 As Integer, enc2 As Integer) As String 'vk 04.21
        '28598	iso-8859-8	Hebrew (ISO-Visual)
        '38598	iso-8859-8-i	Hebrew (ISO-Logical)
        '20127	us-ascii	US-ASCII
        '65001	utf-8	Unicode (UTF-8)
        'Dim enc1, enc2 As Integer
        'enc1 = 38598
        'enc2 = 28598
        Dim ascii As Encoding = Encoding.GetEncoding(enc1) '.ASCII
        Dim unicode As Encoding = Encoding.GetEncoding(enc2) '.Unicode
        Dim unicodeBytes As Byte() = unicode.GetBytes(unicodeString)
        Dim asciiBytes As Byte() = Encoding.Convert(unicode, ascii, unicodeBytes)
        Dim asciiChars As Char() = New Char(ascii.GetCharCount(asciiBytes, 0, asciiBytes.Length)) {}
        ascii.GetChars(asciiBytes, 0, asciiBytes.Length, asciiChars, 0)
        Dim asciiString As String = New String(asciiChars)
        Return asciiString
    End Function
    Private Function tmp(read As String, enc1 As Integer, enc2 As Integer) As String
        Dim utf8 As System.Text.Encoding, windows_1256 As System.Text.Encoding
        utf8 = System.Text.Encoding.GetEncoding(enc1)
        windows_1256 = System.Text.Encoding.GetEncoding(enc2)
        Dim binary As Byte()
        binary = utf8.GetBytes(read)
        Return windows_1256.GetString(binary)
    End Function
    'Public Function BiDiConvertOemToAnsi(ByVal oem As String) As String
    '    Dim ansi As New StringBuilder(2 * oem.Length)
    '    OemToCharBuffA(oem, ansi, oem.Length())
    '    Return ansi.ToString()
    'End Function

    'This function receives one argument containing a logical text and returns visual text.
    Public Function LogicalToVisual(LogicalStr As String) As String
        Dim ConvInfo As CONVERTINFO
        Dim StrInfo1 As STRINFO
        Dim Buf As String = "".PadRight(2000)

        ConvInfo.nOemCP = 1
        ConvInfo.fOemLayout = 2
        ConvInfo.nWindowsCP = 1255
        ConvInfo.fControlChars = 1
        ConvInfo.nUILang = &H409
        ConvInfo.fShowLayout = 1

        StrInfo1.lpStrIn = LogicalStr
        StrInfo1.cchIn = Len(LogicalStr)
        StrInfo1.lpStrOut = Buf
        StrInfo1.cchOut = Len(Buf)

        BiDiConvertAnsiToOem(ConvInfo, StrInfo1)
        LogicalToVisual = Trim(ZeroTrim(StrInfo1.lpStrOut))
    End Function

    'This function receives one argument containing a visual text and returns logical text.
    Public Function VisualToLogical(LogicalStr As String) As String
        Dim ConvInfo As CONVERTINFO
        Dim StrInfo1 As STRINFO
        Dim Buf As String = "".PadRight(2000)

        ConvInfo.nOemCP = 1
        ConvInfo.fOemLayout = 2
        ConvInfo.nWindowsCP = 1255
        ConvInfo.fControlChars = 0
        ConvInfo.nUILang = &H409
        ConvInfo.fShowLayout = 1

        StrInfo1.lpStrIn = LogicalStr
        StrInfo1.cchIn = Len(LogicalStr)
        StrInfo1.lpStrOut = Buf
        StrInfo1.cchOut = Len(Buf)

        BiDiConvertOemToAnsi(ConvInfo, StrInfo1)
        VisualToLogical = ZeroTrim(StrInfo1.lpStrOut)
    End Function

    Private Function ZeroTrim(Str1 As String) As String
        Dim Pos As Long

        Pos = InStr(Str1, Chr(0))
        If Pos > 0 Then
            ZeroTrim = Mid(Str1, 1, Pos - 1)
        Else
            ZeroTrim = Str1
        End If
    End Function

End Class
