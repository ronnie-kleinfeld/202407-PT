Imports System.Data
Imports Comtec.TIS  ' NET20
Imports System.IO   ' NET20

Public Class BuildPage

    'vk 03.04
    Private qColor As New Color()
    Private qConv As New Conv()
    Private qMulti As New Multi()
    Private qOnce As New Once()
    Private qAround As New Around() 'vk 12.07
    Private qStyle As New StyleModule()
    Private qSpool As New Spool()
    'Private qMenu As New Menu()

    Private sScreenNumFromHtml As String = ""

    Friend m_sPath As String
    Friend m_colColor As System.Collections.Hashtable
    Friend m_sNameFocus As String = ""
    Friend m_sNameTab As String = "" 'vk 07.13
    Friend m_sClickLine As String = "" 'vk 08.04
    Friend m_inp_bkg As String
    Friend m_inp_fgd As String
    'vk 05.05
    Friend m_cCss As New Collection()
    Friend m_nGridTitleFrom As Integer = 0
    Friend m_nGridTitleTill As Integer = 0
    Friend m_nGridSearch As Integer = 0
    'vk 07.07
    Friend m_nGridTill As Integer = 0
    Friend m_sMoveSmth As String

    ' Const for scaling
    Friend iScaleLin As Integer = 18
    'Friend iScaleLinK As Integer = 20 'vk 02.05
    Friend iScaleLinComboDelta As Integer = 1 'vk 01.04
    Friend iScaleWidth As Integer = 10
    Friend iScaleWidthK As Integer = 12 'vk 02.05
    Friend m_nModalWindowShiftRight As Integer 'vk 05.10
    Friend m_nModalWindowShiftUp As Integer 'vk 05.10
    Friend m_nModalWindowShiftRight_Cells As Integer 'vk 05.10
    Friend m_nModalWindowShiftUp_Cells As Integer 'vk 05.10
    Friend iSpoolBaseFontSize As Integer = 10 'vk 08.07
    Friend iSpoolLineHeightCoef As Single 'vk 06.10
    Friend iMarginLeft As Integer
    Friend iMarginLeftCom As Integer = 20
    Friend sFontSize As String '= "15px"
    Friend sFontSizeFixed As String '= "15px" 'vk 01.06
    Friend iLenBtn As Integer = 14
    Friend iLenBtnF As Integer = 12
    Friend iMarginTop As Integer
    Friend iMarginRightMod As Integer = 5
    Friend iButtonHeight As Integer = 20
    Friend iButtonScroll As Integer = 15
    Friend iButtonScrollF As Integer
    Friend iButtonInLineF As Integer = 6
    Friend iButtonInLineFF As Integer = 2 'vk 07.07
    Friend bNarrowFolders As Boolean 'vk 06.07
    Friend iScaleLin_Btn As Integer = 18
    Friend iScaleCol_Btn As Integer = 8

    Friend cButtons As Collection 'vk 06.04
    Friend sBtnLineF As Collection
    Friend sBtnLineFF As Collection 'vk 07.07
    Friend sTopLine1 As System.Text.StringBuilder
    Friend sTopLine2 As System.Text.StringBuilder
    Friend iButtons As Integer = 0
    Friend iButtons_GroupX As Integer = 1 'vk 07.09
    Friend iButtons_GroupR As Integer = 5 'vk 07.09
    Friend iButtonsF As Integer = 0
    Friend iButtonsFF As Integer = 0 'vk 07.07
    Friend iButtonsExit As Integer = 0 'vk 09.11
    Friend cLinChecked As New Collection()
    'vk 05.07
    'Friend sButton_L As String = ""
    'Friend sButton_R As String = ""
    Friend sButton_E As String = ""
    Friend sButton_B As String = ""
    'Friend sButton_O As String = ""
    'Friend sButton_D As String = ""
    'Friend sButton_U As String = ""
    'Friend nSpecialButtons As Integer = 0
    'vk 07.07
    Friend oScrollButton_L As Control
    Friend oScrollButton_R As Control
    Friend oScrollButton_D As Control
    Friend oScrollButton_U As Control

    ' Const for Length of attribute
    Friend Const numL As Integer = 10
    Friend Const indL As Integer = 4
    Friend Const typL As Integer = 1
    Friend Const lenL As Integer = 5
    Friend Const decL As Integer = 2
    Friend Const usL As Integer = 1
    Friend Const NOTSPACE As String = "~"

    ' Const for Start of attribute
    Friend Const numS As Integer = 1
    Friend Const indS As Integer = numS + numL
    Friend Const typS As Integer = indS + indL
    Friend Const lenS As Integer = typS + typL
    Friend Const decS As Integer = lenS + lenL
    Friend Const usS As Integer = decS + decL

    'vk 08.04
    Friend CALC_COLUMNS As String() = New String() {
        "IFA1", "IFA2", "IFA3", "IF", "IFB1", "IFB2", "IFB3", "OR",
        "ACTA1", "ACTA2", "ACTA3", "ACT", "ACTB1", "ACTB2", "ACTB3",
        "RND", "INTO", "RSL1", "RSL2", "RSL3", "GO", "T40", "REM"}
    Friend nGridLinesFrom As Integer = 0
    Friend nGridLinesTill As Integer = 0
    Friend aGridlines As Integer()
    Friend m_sAnf As String
    Friend m_sSystem As String 'vk 12.18

    ' Const for Default
    Friend inpD As String = "O"
    Friend decD As String = "".PadRight(decL, "0")
    Friend indD As String = "".PadRight(indL, "0")
    Friend findD As String = "".PadRight(indL, "0")
    Friend fldD As String = "".PadRight(numL, "0")
    Friend fpcD As String = "".PadRight(indL, "0")

    'Friend m_oConv As ConvCom
    'Friend m_sLang As String

    Friend sSqlServer, sSqlUser, sSqlPass, sJndiName As String
    Friend cXzones, cYzones As Collection
    Friend cRadios As Collection 'vk 03.05
    'Friend oXzone As Xzone
    'Friend oYzone As Yzone
    Friend nButtonTabIndex As Integer = 31601
    Friend nButtonTabIndexF As Integer = 31821 'vk 05.07
    Friend nButtonTabIndexFF As Integer = 31921 'vk 07.07
    Friend sListOfLtr As String
    Friend sListOfUni As String 'vk 01.05
    Friend sListOfChk As String 'vk 05.21
    'Friend sListOfButtons As String 'vk 12.07
    'vk 02.05
    Friend cKbdFields As New Collection()
    Friend oKbdField As KbdField
    Friend cBgLines As Collection
    Friend cMenuLines As Collection
    Friend Const MenuFile As String = "MENUDDSFM"
    Friend Const HelpFile As String = "HELPDDSFM" 'vk 06.04
    Friend Const StopFile As String = "MENUCLSFM"
    Friend Const TestFile As String = "@@@@@@@@@@" 'vk 06.04
    Friend Const CgWindow As String = "DCCGWINF" 'vk 02.21 universal window by Mark
    'Friend m_sSite As String
    Friend m_sUser As String 'vk 06.04
    Friend m_sGuid As String 'vk 09.06
    Friend m_sJob As String 'vk 05.07
    'Friend m_sHost As String 'vk 04.11
    Friend m_sUrl As String 'vk 02.13
    Friend m_sIP As String 'vk 12.13
    Friend m_sSynchr As String = "" 'vk 03.08
    'Friend m_bF5 As Boolean 'vk 03.08
    'vk 07.04
    Friend m_bHelpButton As Boolean = False
    Friend m_bHelp As Boolean
    Friend m_nCall As Integer
    'Friend m_bBigRes As Boolean
    'Friend m_bBigResReal As Boolean 'vk 09.05
    Friend m_nResolutionW As Integer 'vk 02.08
    Friend m_nResolutionH As Integer 'vk 02.08
    Friend m_sResolutionSpy As String 'vk 01.12
    Friend m_bSpool As Boolean 'vk 02.08
    Friend m_sVersion As String
    Friend m_bStopSession As Boolean = False 'vk 12.04
    Friend m_nPagesToSend As Integer 'vk 09.05
    Friend m_nPages As Integer 'vk 01.06
    Friend m_nSpoolWidth As Integer = 0 'vk 01.06
    Friend m_nSpoolHeight As Integer = 0 'vk 01.06
    Friend m_bPrintServer As Boolean 'vk 01.06
    'Friend m_bPreprint As Boolean 'vk 01.06
    Friend m_sModelCombosFill As String = "" 'vk 05.05
    Friend m_sModel_Code As New Collection()
    Friend m_sModel_Year As New Collection()
    Friend m_sModel_Gear As New Collection()
    Friend m_sConnectionString As String
    'vk 02.05
    Friend m_bKiosk As Boolean
    Friend m_sCardField As String = ""
    Friend m_sCardPer As String = ""
    Friend m_sCardPfk As String = ""
    Friend m_sMapPath As String
    Friend m_sKbdCode As String = ""
    Friend m_sKbdRus As String = "000"
    Friend m_sKbdVal As String = ""
    'vk 03.06
    Friend m_sCommand As String = ""
    'vk 09.07
    Friend m_sGraph As String = ""
    'Friend m_bLogoBuilt = False
    'vk 08.08
    Friend m_sEnvironment As String = ""
    'vk 03.09
    Friend m_sImageMethod As String = ""
    Friend m_sSession As String = ""
    'vk 04.15
    Friend m_sClient As String = ""

    Friend Const SCRIPT_START_1 As String = "<!-- ( --><SCRIPT> $(document).ready(function() {"
    Friend Const SCRIPT_FINISH_1 As String = "});</SCRIPT>"
    Friend Const SCRIPT_START_2 As String = "<SCRIPT> $(window).on('unload', function () {"
    Friend Const SCRIPT_FINISH_2 As String = "});</SCRIPT><!-- ) -->"

    'vk 09.05
    Dim oNodeScreenSaved As Node
    Dim oNodeScreenTmp As Node 'vk 01.08
    Friend cPrintServer As Collection
    Friend cPrintServerCount As Collection 'vk 07.08
    Friend cPrintServerPreprint As Collection 'vk 12.06
    Friend cTabs As Collection 'vk 01.06

    'vk 06.06
    Dim sr As StringReader '= New StringReader(m_sXML)
    Dim rd As Xml.XmlTextReader '= New Xml.XmlTextReader(sr)
    Dim nodeC As String
    Friend m_bDrawModal As Boolean
    'vk 07.06
    Dim m_bFoundFolderInWindow As Boolean
    Dim m_dp As Comtec.TIS.Reader
    'vk 08.06
    Friend m_nLogoW, m_nLogoH As Integer
    Friend m_nInnerLeftWidth As Integer
    'vk 09.06
    Friend m_nDeltaForScrollFields As Integer
    Dim m_mem As PocketKnife.Memory
    Dim m_sht As PocketKnife.ShowTable 'vk 06.16
    'vk 11.06
    Friend cText As New Collection()
    Friend m_nTextLen As Integer
    Friend m_sTextPrl As String 'vk 10.07
    'vk 07.07
    'Friend cFlexCombos As New Collection
    'vk 08.07
    Friend m_nCode_Bold As Integer
    Friend m_nCode_Size2 As Integer
    Friend m_nCode_Size3 As Integer
    Friend m_nCode_Half As Integer
    Friend m_nCode_Usual As Integer
    Friend m_nCode_Gray As Integer
    'vk 03.11
    Friend m_nCode_Bold_Underline As Integer
    Friend m_nCode_Size2_Underline As Integer
    Friend m_nCode_Underline As Integer
    'Friend m_nSpoolLineHeightCoef As Single
    'vk 11.07
    Const OBJ_FOR_PRINT As String = "<object ID='WebBrowser1' CLASSID='CLSID:8856F961-340A-11D0-A96B-00C04FD705A2' style='display:none;'></object>"
    'vk 04.09
    Dim bDelPdf As Boolean
    'vk 05.09
    Friend m_sXmlFil As String = ""
    Friend m_sXmlRec As String = ""
    'vk 05.12
    Friend m_bErrorInBuffer As Boolean = False
    'vk 09.09
    'Dim m_sPset As String = ""
    'vk 06.10
    Friend m_nPartLength As Integer
    'vk 11.09
    Friend cAdrPer As New Collection
    'vk 12.12
    Friend m_sErrorForClient As String = ""
    'vk 02.13
    Friend m_sPdf As String
    Friend m_sPrinter As String
    Friend m_sTray As String 'vk 12.13
    Friend m_sTif As String
    'vk 06.13
    'Friend m_sArchiveField As String = ""
    Friend m_sArchiveDescr As String = ""
    Friend m_sArchiveKey1 As String = ""
    Friend m_sArchiveKey2 As String = ""
    Friend m_sArchiveKey3 As String = ""
    'Friend m_sArchiveKey4 As String = "" 'rem vk 10.17
    Friend m_sArchiveWhat As String = "" 'vk 03.20
    'vk 07.13
    Friend m_sCG As String = ""
    'vk 12.13
    Friend m_bTray As Boolean = False
    'vk 12.14
    Friend m_sHova As String
    'vk 11.15
    Const DONT_ANSWER = "don't need to answer to AS/400"

    Sub Init(sXml As String) 'vk 06.21
        InitNodeScreenByXml(oNodeScreenSaved, sXml, 1)
    End Sub

    Function getProperty(ByVal s As String, ByVal sDefault As String) As String 'vk 11.16
        Dim v As String
        v = m_dp.getProperty(s)
        If v = "" Then v = sDefault
        Return v
    End Function
    Function getProperty(ByVal s As String) As String 'vk 07.06
        Return m_dp.getProperty(s)
    End Function
    Friend Function getPropertyPset(ByVal s As String, ByVal oNodeScreen As Node) 'vk 06.10
        Dim sReturn As String
        sReturn = getProperty(oNodeScreen.Param("pset") + "_" + s)
        If sReturn = "" Then sReturn = getProperty(s)
        Return sReturn
    End Function
    Function getPropertyStamp(ByVal s As String, ByVal stp As String) As String 'vk 11.09
        Dim r As String
        r = m_dp.getProperty("Stamp" + s + "_" + stp)
        If r = "" Then r = m_dp.getProperty("Stamp" + s)
        Return r
    End Function
    Function getPropertyResol(ByVal s As String) As String 'vk 06.09
        Dim i1, i2 As Integer, a As Array, v As Integer
        s = m_dp.getProperty(s)
        Do
            i1 = InStr(s, "{")
            i2 = InStr(s, "}")
            If i1 = 0 OrElse i2 = 0 OrElse i2 < i1 Then Return s
            a = Mid(s, i1 + 1, i2 - i1 - 1).Split(",")
            v = ResolValue(Val(a(0)), Val(a(1)), Val(a(2)), True, a(3))
            s = Left(s, i1 - 1) + v.ToString() + Mid(s, i2 + 1)
        Loop
    End Function
    Function getPropertyHttp(ByVal s As String) As String 'vk 09.07
        Dim s0 As String = m_dp.getProperty(s)
        If s0 = "" Then
            Return ""
        End If
        Select Case True
            Case s0 Like "http" + "://*"
            Case s0 Like "https" + "://*"
            Case Else
                s0 = "http" + "://" + s0
        End Select
        Return s0
    End Function
    Function getPropertyVal(ByVal s As String) As Integer 'vk 09.06
        Try
            Return Int64.Parse(m_dp.getProperty(s))
        Catch
            Return 0
        End Try
    End Function
    Function getPropertyStampVal(ByVal s As String, ByVal stp As String) As Integer 'vk 11.09
        Dim r As String
        r = m_dp.getProperty("Stamp" + s + "_" + stp)
        If r = "" Then r = m_dp.getProperty("Stamp" + s)
        Try
            Return Int64.Parse(r)
        Catch
            Return 0
        End Try
    End Function
    Function getPropertyValSng(ByVal s As String, Optional ByVal nDefault As Single = 0) As Single 'vk 09.06
        Try
            Return Single.Parse(m_dp.getProperty(s))
        Catch
            Return nDefault 'vk 05.10
        End Try
    End Function

    Sub New(ByVal dp As Comtec.TIS.Reader, ByVal mem As PocketKnife.Memory, ByVal sht As PocketKnife.ShowTable,
            ByVal sColorXml As String, ByVal sMapPath As String,
            ByVal sUser As String, ByVal sGuid As String, ByVal sJob As String,
            ByVal sUrl As String, ByVal sIP As String, ByVal sClient As String)

        Dim v As String
        Dim pp As New PocketKnife.Info()
        'm_oConv = New ConvCom()
        m_sMapPath = sMapPath
        If m_sMapPath Like "*\" Then m_sMapPath = Left(m_sMapPath, Len(m_sMapPath) - 1) 'vk 05.07
        m_dp = dp
        m_mem = mem
        m_sht = sht 'vk 06.16
        m_sUser = sUser
        m_sGuid = sGuid
        m_sJob = sJob 'vk 05.07
        'm_sHost = sHost 'vk 04.11
        m_sUrl = sUrl 'vk 02.13
        m_sIP = sIP
        m_sClient = sClient 'vk 04.15
        'm_sSynchr = sSynchr 'vk 03.08
        'm_bF5 = bF5 'vk 03.08

        sSqlServer = getProperty("SqlServer")
        sSqlUser = getProperty("SqlUser")
        sSqlPass = pp.DecryptPassword(getProperty("SqlPass"))
#If JAVA_MAINSOFT Then
        sJndiName = getProperty("JndiName")
        sSqlServer = sJndiName
#End If

        'vk 08.07
        m_nCode_Bold = getPropertyVal("Code_Bold")
        m_nCode_Size2 = getPropertyVal("Code_Size2")
        m_nCode_Size3 = getPropertyVal("Code_Size3")
        m_nCode_Half = getPropertyVal("Code_Half")
        m_nCode_Usual = getPropertyVal("Code_Usual")
        m_nCode_Gray = getPropertyVal("Code_Gray")
        'm_nSpoolLineHeightCoef = getPropertyValSng("SpoolLineHeightCoef")
        'vk 04.11
        m_nCode_Bold_Underline = getPropertyVal("Code_Bold_Underline")
        m_nCode_Size2_Underline = getPropertyVal("Code_Size2_Underline")
        m_nCode_Underline = getPropertyVal("Code_Underline")

        'vk 02.05
        Try
            m_colColor = qColor.GetColorTable(Me, sColorXml)
            'vk 05.05
            qColor.AddColors(m_colColor, getProperty("BackGround"), "", "_bckg")
            qColor.AddColors(m_colColor, getProperty("ForeGround"), "", "_forg")
            qColor.AddColors(m_colColor, getProperty("Grid"), "grid", "_bckg")
            qColor.AddColors(m_colColor, getProperty("Grid0"), "grid0", "_bckg") 'vk 10.06
            'vk 04.08, rem vk 05.09
            'For Each v In New String() {"Height", "Width", "Margin"}
            '    Dim i As Integer, s As String
            '    For i = 1 To 3
            '        s = v + i.ToString
            '        qColor.AddColors(m_colColor, getProperty(s), s + "_", "")
            '    Next
            'Next
        Catch ex1 As Exception
            Throw ex1
        End Try

        'vk 08.06
        'm_bBigResReal = Int32.Parse(sResolutionW) >= 1024 'AndAlso Int32.Parse(sResolutionH) >= 768 'vk 09.05
        Dim d As System.Drawing.Bitmap = Nothing
        Try
            d = New System.Drawing.Bitmap(m_sMapPath + "\pics\" + getProperty("Logo"))
            m_nLogoW = d.Width
            m_nLogoH = d.Height
        Catch ex1 As Exception
            m_nLogoW = 0
            m_nLogoH = 0
        Finally
            If d IsNot Nothing Then
                d.Dispose()
            End If
            d = Nothing
        End Try

        'moved here vk 11.06
        For Each v In AllStyles
            Try
                'vk 05.08, 06.09
                Dim s As String = getPropertyResol(v)
                m_cCss.Add(New Attrib(v, s), v)
            Catch e As Exception
                e = e
            End Try
        Next

    End Sub

    Private Sub InitNodeScreenByXml(ByRef oNodeScreen As Node, ByVal sXML As String, ByVal nCall As Integer)

        sr = New StringReader(sXML)
        rd = New Xml.XmlTextReader(sr)
        nodeC = ""

        Do While rd.Read
            If rd.NodeType = Xml.XmlNodeType.Element Then
                nodeC = rd.Name
                Select Case rd.Name
                    Case "s"
                        oNodeScreen = New Node(rd, numL, NOTSPACE)
                        InitNodeScreen(oNodeScreen, True, False, nCall)
                End Select
            End If
        Loop
        If Not rd Is Nothing Then rd.Close()
        If Not sr Is Nothing Then
            sr.Close()
            sr.Dispose()
        End If
        rd = Nothing
        sr = Nothing
        'vk 01.07
        If oNodeScreen Is Nothing Then
            oNodeScreen = New Node()
            InitNodeScreen(oNodeScreen, True, False, nCall)
        End If

    End Sub

    Function GetPage_PrintOnly(ByVal sScreenNum As String,
            ByVal winI As String, ByVal wlinI As String, ByVal wcolI As String,
            ByVal wlstI As String, ByVal wcstI As String,
            ByVal nPage As Integer, ByVal nPages As Integer,
            ByVal bBrowser As Boolean, ByVal bPdf As Boolean, ByVal bFirstScreen As Boolean) As String 'vk 09.05

        Dim sOut As System.Text.StringBuilder
        Dim bHideOnScreen As Boolean = True 'getProperty("Design") <> "lex" 'vk 08.08 - tmp!

        m_bHelp = False
        m_sVersion = ""
        m_bSpool = True 'vk 02.08
        sOut = New System.Text.StringBuilder()
        Try
            InitPage(oNodeScreenSaved, True, False, oNodeScreenSaved.ParamVal("fdsp"), bPdf)
            If qSpool.IsNetto(Me, oNodeScreenSaved) Then
                'vk 12.07
                qSpool.FormSpool(Me, sOut, oNodeScreenSaved, nPage, True, False, bBrowser, bPdf)
            Else
                With qAround
                    .FormStyle(Me, sOut, oNodeScreenSaved, bHideOnScreen, False, True, bPdf)
                    .FormHeader(Me, sOut, oNodeScreenSaved, oNodeScreenSaved, True, nPage, nPages, True, False, bPdf)
                End With
                qOnce.FormHiddens(Me, sOut, oNodeScreenSaved, sScreenNum, "", "", "", False, bFirstScreen) 'vk 12.07
                If Not bPdf Then sOut.Append(OBJ_FOR_PRINT + vbCrLf)
                qSpool.FormSpool(Me, sOut, oNodeScreenSaved, nPage, False, False, bBrowser, bPdf) 'vk 01.06
                If Not bPdf Then qOnce.FormStamp(Me, sOut, oNodeScreenSaved)
                With qAround
                    .FormForms(Me, sOut, oNodeScreenSaved, "", "")
                    If Not bPdf Then .FormFunction(Me, sOut, winI, wlinI, wlstI, wcolI, wcstI,
                        oNodeScreenSaved, oNodeScreenSaved, True, True, "", False, False)
                End With
            End If
        Catch e1 As Exception
            Throw New Exception("XML from AS400 is wrong. PrintServer" + vbCrLf + e1.Message, e1)
            Return ""
        End Try

        Return sOut.ToString

    End Function

    Sub FillPrintServer(ByVal sBuffer As String, ByVal nPartLength As Integer, ByRef nPages As Integer) 'vk 01.06

        Const ENDDOC As String = "~XXXENDDOCUMENTXXX~" 'vk 01.07
        Const START As String = "~XXXSTARTFLDXXX~" 'vk 12.07
        Const FINISH As String = "~XXXFINISHFLDXXX~" 'vk 12.07
        Const START_TXT As String = "~XXXSTARTTXTXXX~" 'vk 01.08
        Const FINISH_TXT As String = "~XXXFINISHTXTXXX~" 'vk 01.08
        'Const START_TRAY As String = "~XXXSTARTTRAYXXX~" 'vk 07.08
        'Const FINISH_TRAY As String = "~XXXFINISHTRAYXXX~" 'vk 07.08
        Dim i, j1, j2, j As Integer, s, sOrig As String
        Dim bIgnore As Boolean = False
        Dim nPageToIgnore As Integer = 0
        Dim sCurPage As String
        Dim nCurPage As Integer = 0
        Dim bInLongList As Boolean = False 'vk 12.07
        Dim bInLongField As Boolean = False 'vk 01.08
        Dim sStartPage As String = "" 'vk 12.07
        Dim sLongFieldName As String = "" 'vk 01.08
        Dim sLongFieldValue As String = "" 'vk 01.08
        'vk 04.11
        Dim sCode As String
        Dim sWait As String = ""

        m_nPartLength = nPartLength 'vk 06.10
        cPrintServer = New Collection()
        cPrintServerCount = New Collection()
        cPrintServerPreprint = New Collection()
        nPages = 0
        For i = 1 To Len(sBuffer) Step nPartLength
            s = Mid(sBuffer, i, nPartLength)
            'vk 01.10
            sOrig = s
            For j = 1 To s.Length
                Select Case AscW(Mid(s, j, 1))
                    Case m_nCode_Bold, m_nCode_Size2, m_nCode_Size3, m_nCode_Half, m_nCode_Usual,
                            m_nCode_Bold_Underline, m_nCode_Size2_Underline, m_nCode_Underline
                        Mid(s, j, 1) = " "
                End Select
            Next
            'vk 07.08
            'n = InStr(s, START_TRAY)
            'If n > 0 Then Mid(s, n, Len(START_TRAY) + 3) = Space(Len(START_TRAY) + 3)
            'n = InStr(s, FINISH_TRAY)
            'If n > 0 Then Mid(s, n, Len(FINISH_TRAY)) = Space(Len(FINISH_TRAY))
            sCurPage = Left(s, 4)
            If IsNumeric(sCurPage) Then
                nCurPage = Val(sCurPage)
                'vk 01.07
                If nCurPage <> nPageToIgnore Then
                    bIgnore = False
                End If
                If s Like "*" + ENDDOC + "*" Then
                    bIgnore = True
                    nPageToIgnore = nCurPage
                End If
            End If
            If Not bIgnore Then
                'vk 01.08
                Select Case True
                    Case s Like "*" + START + "*"
                        bInLongList = True
                        sStartPage = sCurPage
                End Select
                If bInLongList Then
                    sCurPage = sStartPage
                    nCurPage = Val(sCurPage)
                End If
                If nCurPage > nPages Then
                    nPages = nCurPage
                End If
                'vk 04.11, 11.16
                For Each sCode In getProperty("IgnoreLines", "MAILFROM,MAILCC,MAILBCC,STARTTRAY,FINISHTRAY").Split(",")
                    If sOrig Like "*~XXX" + sCode + "XXX~*" Then
                        sOrig = Left(sOrig, 6).PadRight(Len(sOrig))
                        sWait = ""
                    End If
                Next
                For Each sCode In getProperty("IgnoreSections", "MAILBODY,MAILFOOTER,MAILSUBJECT").Split(",")
                    If sOrig Like "*~XXX" + sCode + "STARTXXX~*" Then
                        sOrig = Left(sOrig, 6).PadRight(Len(sOrig))
                        sWait = sCode + "END"
                    End If
                    If sOrig Like "*~XXX" + sCode + "ENDXXX~*" Then
                        sOrig = Left(sOrig, 6).PadRight(Len(sOrig))
                        sWait = ""
                    End If
                Next

                If sWait > "" Then
                    sOrig = Left(sOrig, 6).PadRight(Len(sOrig))
                End If
                Try
                    cPrintServer.Add(sCurPage + Mid(sOrig, 5), Left(sOrig, 6)) 'vk 02.06, 12.07, 01.10
                Catch e As Exception
                    '
                End Try
                Select Case True
                    Case s Like "*" + FINISH + "*"
                        bInLongList = False
                    Case s Like "*" + START_TXT + "*" 'vk 01.08
                        bInLongField = True
                        Try
                            'Dim j As Integer
                            j = InStr(s, START_TXT)
                            sLongFieldName = Mid(s, j + Len(START_TXT)).Trim
                        Catch
                            sLongFieldName = ""
                        End Try
                        sLongFieldValue = ""
                    Case s Like "*" + FINISH_TXT + "*" 'vk 01.08
                        bInLongField = False
                        If Val(cPrintServerPreprint(sCurPage)) > 0 AndAlso sLongFieldName > "" Then
                            AddFieldToPrintServer("~" + sLongFieldName + "~" + sLongFieldValue, sCurPage + "_" + sLongFieldName)
                        End If
                    Case Else
                        'vk 12.06
                        Try
                            cPrintServerPreprint.Add("", sCurPage) 'vk 01.09
                        Catch
                        End Try
                        Dim sPreprintNo As String = qSpool.PreprintNo(s)
                        If sPreprintNo > "" AndAlso Val(cPrintServerPreprint(sCurPage)) = 0 Then
                            cPrintServerPreprint.Remove(sCurPage)
                            cPrintServerPreprint.Add(sPreprintNo, sCurPage)
                        End If
                        If bInLongField Then
                            If sLongFieldValue > "" Then
                                sLongFieldValue += "<br>"
                            End If
                            sLongFieldValue += Mid(s, 7).Trim
                        Else
                            'two loops united by vk 12.07
                            'If Val(cPrintServerPreprint(sCurPage)) > 0 Then
                            If Trim(cPrintServerPreprint(sCurPage)) > "" Then 'vk 11.09
                                s = Mid(s, 7, nPartLength - 7)
                                j1 = InStr(s, "~")
                                If j1 > 0 Then
                                    j2 = InStr(j1 + 1, s, "~")
                                    If j2 > 0 Then
                                        AddFieldToPrintServer(s, sCurPage + "_" + Mid(s, j1 + 1, j2 - j1 - 1)) 'vk 08.07
                                    End If
                                End If
                            End If
                        End If
                End Select
            End If
        Next
        m_nPages = nPages 'vk 01.06

    End Sub

    Private Sub AddFieldToPrintServer(ByVal sValue As String, ByVal sName As String) 'vk 07.07
        Dim n As Integer = 1
        Do
            Try
                Select Case n
                    Case Is > 99
                    Case 1
                        cPrintServer.Add(sValue, sName)
                        cPrintServerCount.Add(n, sName)
                    Case Else
                        cPrintServer.Add(sValue, sName + n.ToString("00"))
                        cPrintServerCount.Remove(sName)
                        cPrintServerCount.Add(n, sName)
                End Select
                Return
            Catch
                n += 1
            End Try
        Loop
    End Sub

    Friend Function WideScreen(Optional ByVal bReal As Boolean = False) As Boolean 'vk 05.09
        Return (bReal OrElse (Not m_bSpool) OrElse m_nCall = 2) AndAlso m_nResolutionW = 1440 AndAlso m_nResolutionH = 900 AndAlso getProperty("WideScreenSupport").ToLower = "true"
    End Function
    Friend Function ResolValue(ByVal y1 As Single, ByVal y2 As Single, ByVal y3 As Integer,
            ByVal bReal As Boolean, ByVal sHorVert As String) As Integer 'vk 02.08, 05.09
        If bReal OrElse (Not m_bSpool) OrElse m_nCall = 2 Then
            If WideScreen(bReal) Then
                Return y3
                'ElseIf m_nResolutionW >= 1024 Then
                '    Return nValue2
            Else
                'Return nValue1
                'vk 05.09
                Dim x1, x2, x3 As Integer
                Select Case sHorVert
                    Case "hor", "font" : x1 = 800 : x2 = 1024 : x3 = m_nResolutionW
                    Case "vert" : x1 = 550 : x2 = 700 : x3 = m_nResolutionH
                        'Case "vert" : x1 = 600 : x2 = 768 : x3 = m_nResolutionH
                End Select
                If sHorVert = "font" AndAlso x3 > x2 Then
                    Return Int(0.6 * (y2 - y1) * (x3 - x1) / (x2 - x1) + y1) 'vk 04.15
                Else
                    Dim d As Single = IIf(sHorVert = "font", 0, 0.3) 'vk 06.09
                    Return Int((y2 - y1) * (x3 - x1) / (x2 - x1) + y1 + d)
                End If
            End If
        Else
            Return y1
        End If
    End Function
    Function ResizeFont(ByVal n As Integer, ByVal bDefpropButton As Boolean,
            ByVal bFirstLine As Boolean, ByVal bTop132 As Boolean) As Integer 'vk 05.10
        Dim r As Integer
        If bFirstLine Then
            'vk 06.10
            r = ResolValue(
                n * getPropertyValSng("FontCoefTop_800", 0.9),
                n * getPropertyValSng("FontCoefTop_1024", 1),
                n, bDefpropButton, "font")
            If bTop132 Then r = Int(r * 0.6)
        Else
            r = ResolValue(
                n * getPropertyValSng("FontCoef_800", 0.9),
                n * getPropertyValSng("FontCoef_1024", 1),
                n, bDefpropButton, "font")
        End If
        Return r
    End Function
    Friend Sub SetCellSize(ByRef oNodeScreen As Node, ByRef iScaleLin As Integer, ByRef iScaleWidth As Integer) 'vk 08.10
        Select Case oNodeScreen.ParamVal("fdsp")
            Case 0, 2 : iScaleLin = ResolValue(18, 24, 28, False, "vert")
            Case Else : iScaleLin = ResolValue(16, 21, 24, False, "vert")
        End Select
        If m_bSpool Then
            Select Case oNodeScreen.ParamVal("fdsp")
                Case 0 : iScaleWidth = 8
                Case 1 : iScaleWidth = 5
                Case 2 : iScaleWidth = 13
                Case 3 : iScaleWidth = 6
                Case Else : iScaleWidth = 3
            End Select
            Dim nTmp As Integer
            nTmp = Val(getPropertyPset("CellWidth", oNodeScreen))
            If nTmp > 0 Then iScaleWidth = nTmp
            nTmp = Val(getPropertyPset("CellHeight", oNodeScreen))
            If nTmp > 0 Then iScaleLin = nTmp
        Else
            '+0.1 removed vk 01.12
            Select Case oNodeScreen.ParamVal("fdsp")
                Case 0 : iScaleWidth = Int((m_nResolutionW - iMarginLeftCom) / 80) 'vk 06.10
                Case 1 : iScaleWidth = Int((m_nResolutionW - iMarginLeftCom) / 132) 'vk 06.10
                Case 2 : iScaleWidth = Int((m_nResolutionW - iMarginLeftCom) / 50)
                Case 3 : iScaleWidth = Int((m_nResolutionW - iMarginLeftCom) / 100)
                Case 4 : iScaleWidth = Int((m_nResolutionW - iMarginLeftCom) / 180)
                Case 5 : iScaleWidth = Int((m_nResolutionW - iMarginLeftCom) / 190)
                Case 6 : iScaleWidth = Int((m_nResolutionW - iMarginLeftCom) / 198)
            End Select
        End If
        '! for wide screen it's possible to increase: 28 -> 32, 24 -> 28
        'vk 04.08
        'iScaleLin = ResolValueFdspParam(oNodeScreen, "Height", False)
        'iScaleWidth = ResolValueFdspParam(oNodeScreen, "Width", False)
    End Sub
    Friend Function GridExists() As Boolean
        Return cXzones.Count > 0 AndAlso cYzones.Count > 0
    End Function
    Friend Function FixedFontSize(ByVal iScaleWidth As Integer, bPdf As Boolean) As Integer 'vk 06.09
        Dim n As Integer
        n = Int(iScaleWidth / 0.6 + 0.7) '- 1
        If (Not bPdf) AndAlso (m_sClient Like "*Trident/7.0*" OrElse Not (m_sClient Like "*Trident*" OrElse m_sClient Like "*MSIE*")) Then
            n -= 1 'vk 04.15
        End If
        Return n
    End Function
    Friend Function HeightByClass(ByVal sClass As String, ByVal bPdf As Boolean, ByRef oNodeScreen As Node) As Integer 'vk 08.07
        Return SizeByClass(iSpoolBaseFontSize.ToString, sClass, bPdf, oNodeScreen) * iSpoolLineHeightCoef
    End Function
    Friend Function SizeByClass(ByVal sFontSize As String, ByVal sClass As String, ByVal bPdf As Boolean, ByRef oNodeScreen As Node) As Integer 'vk 01.06
        If bPdf Then
            'Dim s As String = getProperty(m_sPset + "_FontSizeDelta_Pdf") 'vk 09.09
            'If s = "" Then s = getProperty("FontSizeDelta_Pdf")
            sFontSize = CStr(FixedFontSize(iScaleWidth, bPdf) + Val(getPropertyPset("FontSizeDelta_Pdf", oNodeScreen))) 'vk 06.09
        End If
        If sFontSize Like "*px" Then
            sFontSize = Left(sFontSize, Len(sFontSize) - 2) 'vk 01.06
        End If
        If Not IsNumeric(sFontSize) Then
            Return 0
        End If
        Dim nPlus As Integer = 0
        Select Case sClass
            Case "Size2", "Size3", "Half" : nPlus = getPropertyVal("FontSizeDelta_" + sClass)
            Case "Size2_Underline" : nPlus = getPropertyVal("FontSizeDelta_Size2")
        End Select
        Return Int32.Parse(sFontSize) + nPlus 'vk 06.09
    End Function
    Friend Function OrganizedButtons(ByVal bScroll As Boolean) As Boolean 'vk 03.10
        If bScroll Then
            Return getProperty("Design") = "lex" OrElse getProperty("Design") = "pro"
        Else
            Return getProperty("Design") = "lex" OrElse getProperty("Design") = "pro" _
                OrElse getProperty("Design") = "halfpro" 'vk 05.11
        End If
    End Function
    Friend Function LogoCoef(ByVal bSpool As Boolean) As Single 'vk 06.10
        Dim nLogoScaleBase As String = getPropertyVal("LogoScaleBase")
        Dim nResol As Integer = ResolValue(800, 1024, 1440, Not bSpool, "hor")
        If nLogoScaleBase > 0 AndAlso nLogoScaleBase <> nResol Then
            Return nResol / nLogoScaleBase
        Else
            Return 1.0
        End If
    End Function
    Friend Function ArchiveKey() As String 'vk 07.13
        Return (m_sArchiveKey1 + m_sArchiveKey2 + m_sArchiveKey3).Trim 'vk 10.17 ' + m_sArchiveKey4
    End Function

    Private Sub InitNodeScreen(ByRef oNodeScreen As Node,
            ByVal bNodeOnly As Boolean, ByVal bMergeNodes As Boolean, ByVal nCall As Integer)

        Dim v As String
        With oNodeScreen
            For Each v In AllParams_Screen
                .AddIfNo(v)
            Next
            For Each v In AllParams_Screen0
                .AddIfNo(v, "0")
            Next
            For Each v In AllParams_Screen000
                .AddIfNo(v, "000")
            Next
            .AddIfNo("fpc", fpcD)
            .AddIfNo("flib", "Z")
            .AddIfNo("fsid", "L")
            .AddIfNo("srg", "U")
            .AddIfNo("win", "N")
            .AddIfNo("wlst", "1")
            .AddIfNo("wcst", "1")
            .AddIfNo("path", "") 'vk 11.15
            'vk 06.10
            Select Case oNodeScreen.ParamVal("fdsp")
                Case 0 : .AddIfNo("wcol", "80") : .AddIfNo("wlin", "24")
                Case 1 : .AddIfNo("wcol", "132") : .AddIfNo("wlin", "27")
                Case 2 : .AddIfNo("wcol", "50") : .AddIfNo("wlin", "24")
                Case 3 : .AddIfNo("wcol", "100") : .AddIfNo("wlin", "24")
                Case 4 : .AddIfNo("wcol", "180") : .AddIfNo("wlin", "24")
                Case 5 : .AddIfNo("wcol", "190") : .AddIfNo("wlin", "24")
                Case 6 : .AddIfNo("wcol", "198") : .AddIfNo("wlin", "24")
            End Select
            .AddIfNo("wcol", "80")
            'vk 02.05
            .AddIfNo("esc", "00")
            .AddIfNo("sec", "0000")
            'vk 11.06
            .AddIfNo("tmx", "001")
            'vk 01.07
            .AddIfNo("fil", "")
            'vk 08.07
            .AddIfNo("cmf", "")
            'vk 12.12
            .AddIfNo("sic", "I")
            'vk 12.07
            If nCall = 1 Then
                If .Param("wait") = "S" Then
                    .AddIfNo("fnn", "".PadRight(26, "0")) 'vk 01.08
                Else
                    .AddIfNo("fnn", "".PadRight(26, "1"))
                End If
                If oNodeScreen.Param("qstr") Is Nothing Then
                Else
                    Mid(.Param("fnn"), 25, 2) = "00"
                End If
            End If
            'vk 01.08
            If bMergeNodes AndAlso oNodeScreenTmp.Param("dll") = "W" Then
                Dim s As String
                For Each s In AllParams_ScreenDllW
                    .Param(s) = oNodeScreenTmp.Param(s)
                Next
            End If
            'vk 04.09
            If getProperty("PdfAlways") = "true" AndAlso .ParamIn("plgc", " A") Then
                .Param("plgc") = "P"
            End If
            If bNodeOnly Then Exit Sub
            'm_bBigRes = m_bBigResReal AndAlso (.Param("wait") <> "S" OrElse m_nCall = 2) 'vk 11.06

#If Not JAVA_MAINSOFT Then
            'vk 05.05
            If sSqlUser = "" Then 'ntg 10.07.24 vladi's change regarding sql migration for bituah haklai
                m_sConnectionString =
                "Data Source=" + sSqlServer + ";Initial Catalog=" + .Param("flib").Trim +
                ";Integrated Security=true;" + getProperty("ConnStrAddition")
            Else
                m_sConnectionString =
                "Data Source=" + sSqlServer + ";Initial Catalog=" + .Param("flib").Trim +
                ";User ID=" + sSqlUser + ";Password=" + sSqlPass + ";" + getProperty("ConnStrAddition") 'vk 03.08
            End If
#Else
            m_sConnectionString = sJndiName
#End If
            If .ParamIn("srg", "BC") AndAlso Not .Ortogonal(Me) Then
                iButtonScrollF = IIf(bNarrowFolders AndAlso oNodeScreen.Param("wsml") = "S", 1, 2) 'vk 02.10
            Else
                iButtonScrollF = 3
            End If
        End With

    End Sub

    Private Sub InitPage(ByRef oNodeScreen As Node, ByVal bClearButtons As Boolean, ByVal bBkg As Boolean, ByVal nFdspForMargin As Integer, bPdf As Boolean)
        'LANG = "1"   ' 0 - English, 1 - Hebrew
        'US = "0"     ' 0 - usual, 1 - USA

        If oNodeScreen.Param("wait") = "S" AndAlso oNodeScreen.Param("plgc") = "P" Then
            nFdspForMargin = 0 'vk 11.19
        End If

        'm_sPset = oNodeScreen.Param("pset") 'vk 09.09
        If bClearButtons Then
            cButtons = New Collection() 'vk 06.04
            sBtnLineF = New Collection()
            sBtnLineF.Add(New System.Text.StringBuilder())
            sBtnLineFF = New Collection() 'vk 07.07
            sBtnLineFF.Add(New System.Text.StringBuilder()) 'vk 07.07
            iButtons = 0
            iButtonsF = 0
            iButtonsFF = 0 'vk 07.07
            iButtonsExit = 0 'vk 09.11
            'vk 05.07
            sButton_E = ""
            sButton_B = ""
            'vk 07.07
            If Not oScrollButton_L Is Nothing Then oScrollButton_L.Dispose()
            oScrollButton_L = Nothing
            If Not oScrollButton_R Is Nothing Then oScrollButton_R.Dispose()
            oScrollButton_R = Nothing
            If Not oScrollButton_D Is Nothing Then oScrollButton_D.Dispose()
            oScrollButton_D = Nothing
            If Not oScrollButton_U Is Nothing Then oScrollButton_U.Dispose()
            oScrollButton_U = Nothing
        End If
        iLenBtn = 14
        iButtonHeight = 20
        sTopLine1 = New System.Text.StringBuilder()
        sTopLine2 = New System.Text.StringBuilder()

        'iScaleLinK = ResolValue(20, 27, 34) 'vk 02.05
        Select Case nFdspForMargin 'vk 07.10
            Case 1 : iMarginLeftCom = ResolValue(130, 175, 250, False, "hor")
            Case Else : iMarginLeftCom = ResolValue(145, 175, 250, False, "hor")
        End Select
        SetCellSize(oNodeScreen, iScaleLin, iScaleWidth)
        m_nModalWindowShiftRight = getPropertyVal("ModalWindowShiftRight") * iScaleWidth / oNodeScreen.CellWidth_1024 'vk 05.10
        m_nModalWindowShiftUp = getPropertyVal("ModalWindowShiftUp") * iScaleLin / oNodeScreen.CellHeight_1024 'vk 05.10
        m_nModalWindowShiftRight_Cells = CInt(getPropertyVal("ModalWindowShiftRight") / oNodeScreen.CellWidth_1024) 'vk 05.10
        m_nModalWindowShiftUp_Cells = CInt(getPropertyVal("ModalWindowShiftUp") / oNodeScreen.CellHeight_1024) 'vk 05.10
        sFontSizeFixed = FixedFontSize(iScaleWidth, bPdf).ToString + "px" 'vk 05.09
        Select Case oNodeScreen.ParamVal("fdsp")
            Case 0 : iScaleWidthK = ResolValue(9, 12, 16, False, "hor")
            Case 1 : iScaleWidthK = ResolValue(6, 7, 10, False, "hor")
        End Select
        Dim iTmp As Integer
        If oNodeScreen.ParamVal("flang") = 1 Then
            Select Case oNodeScreen.ParamVal("fdsp")
                Case 0 : iTmp = ResolValue(11, 13, 15, False, "font")
                Case 1 : iTmp = ResolValue(9, 11, 13, False, "font")
                Case 2 : iTmp = ResolValue(14, 16, 18, False, "font")
                Case 3 : iTmp = ResolValue(10, 12, 14, False, "font")
                Case 4 : iTmp = ResolValue(8, 9, 10, False, "font")
                Case Else : iTmp = ResolValue(7, 8, 9, False, "font")
            End Select
        Else
            Select Case oNodeScreen.ParamVal("fdsp")
                Case 0 : iTmp = ResolValue(13, 15, 17, False, "font")
                Case 1 : iTmp = ResolValue(9, 11, 13, False, "font")
                Case 2 : iTmp = ResolValue(16, 18, 20, False, "font")
                Case 3 : iTmp = ResolValue(11, 13, 15, False, "font")
                Case 4 : iTmp = ResolValue(9, 10, 11, False, "font")
                Case Else : iTmp = ResolValue(8, 9, 10, False, "font")
            End Select
        End If
        sFontSize = iTmp.ToString + "px" 'vk 01.06
        iLenBtnF = ResolValue(12, 16, 20, False, "hor")
        iMarginRightMod = 5
        iScaleLinComboDelta = ResolValue(1, 2, 3, False, "vert") 'vk 01.04
        iSpoolBaseFontSize = Val(getPropertyPset("SpoolFontSize", oNodeScreen))
        iSpoolLineHeightCoef = Val(getPropertyPset("SpoolLineHeightCoef", oNodeScreen))

        If oNodeScreen.Modal Then
            If m_bDrawModal Then
                iMarginTop = iScaleLin * 0.5 'vk 09.06
            Else
                iMarginTop = 0
            End If
        Else
            If oNodeScreen.Param("wait") = "S" AndAlso m_nCall = 1 AndAlso Not bBkg Then 'vk 11.06
                iMarginTop = 0 'vk 02.04
            ElseIf oNodeScreen.ParamIn("srg", "UBAC") Then
                iMarginTop = ResolValue(96, 105, 120, False, "vert") 'vk 08.05
            Else
                iMarginTop = 82
            End If
            If getProperty("Design") = "lex" Then
                iMarginTop += 28 'vk 05.07
            End If
        End If
        iMarginLeft = iMarginLeftCom
        If (oNodeScreen.Modal AndAlso oNodeScreen.Ortogonal(Me)) _
                OrElse Not oNodeScreen.ParamIn("fsid", "LMOP") Then 'ms+vk 06.05, vk 08.06, 05.07
            iMarginLeft = 5 'IIf(flr = "1", 15, 0)
        End If
        If m_bHelp AndAlso m_nCall = 1 Then
            'vk 07.04
            iMarginLeft = 0
            iMarginTop = -iScaleLin
        End If

        If getProperty("ModalWindows") = "dialog" Then
            If oNodeScreen.Modal Then
                bNarrowFolders = True
            End If
        ElseIf oNodeScreen.Ortogonal(Me) Then
            If oNodeScreen.Modal AndAlso Not bBkg Then
                bNarrowFolders = True
            ElseIf oNodeScreen.Drawn Then
                bNarrowFolders = True
            End If
        End If
        If OrganizedButtons(False) Then
            'vk 07.07
            iButtonInLineF = IIf(bNarrowFolders AndAlso oNodeScreen.Param("wsml") = "S", 999, 4) 'vk 02.10
            iButtonInLineFF = 2
            'vk 10.08, 01.09
            If oNodeScreen.Modal AndAlso oNodeScreen.Param("win") <> "C" Then
                'iMarginLeft += IIf(oNodeScreen.ParamVal("flr") = 1, 2, -3.5) * iScaleWidth
                iMarginLeft += IIf(oNodeScreen.ParamVal("flr") = 1, 2, -1) * iScaleWidth 'vk 07.10
            End If
        Else
            iButtonInLineF = 6 'vk 05.07
        End If
        'Dim bNarrowFolders As Boolean = False 'vk 05.07
        If bNarrowFolders Then
            'Dim i As Integer = (oNodeScreen.ParamVal("wcol") * iScaleWidth) \ (iLenBtnF * iScaleCol_Btn)
            'If i < iButtonInLineF Then iButtonInLineF = i
            iButtonInLineF = (oNodeScreen.ParamVal("wcol") * iScaleWidth) \ (iLenBtnF * iScaleCol_Btn) 'vk 09.11
        End If

        'vk 08.06
        If Not bBkg Then
            m_nInnerLeftWidth = iMarginLeftCom
        End If
        'vk 07.07
        m_sMoveSmth = ""

    End Sub

    Sub Dispose()
        qColor.Dispose()
        qColor = Nothing
        qConv = Nothing
        qMulti.Dispose()
        qMulti = Nothing
        qOnce.Dispose()
        qOnce = Nothing
        qAround.Dispose()
        qAround = Nothing
        qStyle = Nothing
        qSpool.Dispose()
        qSpool = Nothing
        'qMenu.Dispose()
        'qMenu = Nothing
        If Not oNodeScreenSaved Is Nothing Then oNodeScreenSaved.Dispose()
        oNodeScreenSaved = Nothing
    End Sub

End Class
