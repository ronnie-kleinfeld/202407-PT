Imports Comtec.TIS  ' NET20
Imports System.IO   ' NET20
Imports System.Linq
Imports System.Xml.Linq

Public Class BuildPage

    Private qColor As New Color()
    Private qConv As New Conv()
    Private qMulti As New Multi()
    Private qOnce As New Once()
    Private qAround As New Around() 'vk 12.07
    Private qStyle As New StyleModule()
    Private qSpool As New Spool()
    Private qBoo As New Boo()
    Private sScreenNumFromHtml As String = ""

    Friend m_sPath As String
    Friend m_colColor As Hashtable
    Friend m_sNameFocus As String = ""
    Friend m_sClickLine As String = ""
    Friend m_inp_bkg As String
    Friend m_inp_fgd As String
    Friend m_cCss As New Collection()
    Friend m_nGridTitleFrom As Integer = 0
    Friend m_nGridTitleTill As Integer = 0
    Friend m_nGridSearch As Integer = 0
    Friend m_nGridTill As Integer = 0
    Friend iScaleLin As Integer = 18
    Friend iScaleLinComboDelta As Integer = 1 'vk 01.04
    Friend iScaleWidth As Integer = 10
    Friend iScaleWidthK As Integer = 12 'vk 02.05
    Friend iSpoolBaseFontSize As Integer = 10 'vk 08.07
    Friend iSpoolLineHeightCoef As Single 'vk 06.10
    Friend sFontSize As String '= "15px"
    Friend sFontSizeFixed As String '= "15px" 'vk 01.06
    Friend iLenBtn As Integer = 14
    Friend iLenBtnF As Integer = 12
    Friend iButtonHeight As Integer = 20
    Friend iButtonScroll As Integer = 15
    Friend iButtonScrollF As Integer
    Friend iButtonInLineF As Integer = 6
    Friend iButtonInLineFF As Integer = 2 'vk 07.07
    Friend iScaleLin_Btn As Integer = 18
    Friend iScaleCol_Btn As Integer = 8
    Friend cButtons As List(Of Button) = New List(Of Button)
    Friend sBtnLineF As Collection
    Friend sBtnLineFF As Collection 'vk 07.07
    Friend sTopLine1 As Text.StringBuilder
    Friend sTopLine2 As Text.StringBuilder
    Friend iButtons As Integer = 0
    Friend iButtons_GroupX As Integer = 1 'vk 07.09
    Friend iButtons_GroupR As Integer = 5 'vk 07.09
    Friend iButtonsF As Integer = 0
    Friend iButtonsFF As Integer = 0 'vk 07.07
    Friend iButtonsExit As Integer = 0 'vk 09.11
    Friend cLinChecked As New Collection()
    Friend sButton_E As String = ""
    Friend sButton_B As String = ""
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
    Friend sSqlServer, sSqlUser, sSqlPass, sJndiName As String
    Friend cXzones, cYzones As Collection
    Friend cRadios As Collection 'vk 03.05
    Friend oXzone As Xzone
    Friend oYzone As Yzone
    'Friend nButtonTabIndex As Integer = 31601
    'Friend nButtonTabIndexF As Integer = 31821 'vk 05.07
    'Friend nButtonTabIndexFF As Integer = 31921 'vk 07.07
    Friend sListOfLtr As String = ";"
    Friend sListOfUni As String 'vk 01.05
    Friend sListOfChk As String 'vk 05.21

    Friend cMenuLines As Dictionary(Of String, MenuLine)

    Friend Const MenuFile As String = "MENUDDSFM"
    Friend Const HelpFile As String = "HELPDDSFM" 'vk 06.04
    Friend Const StopFile As String = "MENUCLSFM"
    Friend Const TestFile As String = "@@@@@@@@@@" 'vk 06.04
    Friend Const CgWindow As String = "DCCGWINF" 'vk 02.21
    Friend Const MsgFile As String = "MSGSCRFM"
    Friend Const MsgFile2 As String = "UNSNDERR"
    Friend m_sUser As String 'vk 06.04
    Friend m_sGuid As String 'vk 09.06
    Friend m_sJob As String 'vk 05.07
    Friend m_sUrl As String 'vk 02.13
    Friend m_sIP As String 'vk 12.13
    Friend m_sSynchr As String = "" 'vk 03.08
    Friend m_bHelpButton As Boolean = False
    Friend m_bHelp As Boolean
    Friend m_nCall As Integer
    Friend Const m_nResolutionW As Integer = 1024
    Friend Const m_nResolutionH As Integer = 768
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
    Friend m_bStreetCombos As Boolean = False 'ntg 10.07.24 vladi's change regarding city-street screen
    Friend m_sPrebuilt As String = "" 'ntg 10.07.24 vladi's change regarding city-street screen
    Friend m_sModel_Code As New Collection()
    Friend m_sModel_Year As New Collection()
    Friend m_sModel_Gear As New Collection()
    Friend m_sConnectionString As String
    Friend m_sCardField As String = ""
    Friend m_sCardPer As String = ""
    Friend m_sCardPfk As String = ""
    Friend m_sMapPath As String
    Friend m_sCommand As String = ""
    Friend m_sGraph As String = ""
    Friend m_sEnvironment As String = ""
    Friend m_sImageMethod As String = ""
    Friend m_sSession As String = ""
    Friend XmlLocalCouner As String = ""

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

    Dim sr As StringReader
    Dim rd As Xml.XmlTextReader
    Dim nodeC As String
    Dim m_bFoundFolderInWindow As Boolean
    Dim m_dp As Reader

    Friend m_nDeltaForScrollFields As Integer
    Dim m_mem As PocketKnife.Memory
    Dim m_sht As PocketKnife.ShowTable 'vk 06.16

    Friend cText As New Collection()
    Friend m_nTextLen As Integer
    Friend m_sTextPrl As String
    Friend cFlexCombos As Collection
    Friend cApr As Collection = New Collection 'ntg 10.07.24 vladi's change regarding city-street screen
    Friend m_nCode_Bold As Integer
    Friend m_nCode_Size2 As Integer
    Friend m_nCode_Size3 As Integer
    Friend m_nCode_Half As Integer
    Friend m_nCode_Usual As Integer
    Friend m_nCode_Gray As Integer
    Friend m_nCode_Bold_Underline As Integer
    Friend m_nCode_Size2_Underline As Integer
    Friend m_nCode_Underline As Integer
    Const OBJ_FOR_PRINT As String = "<object ID='WebBrowser1' CLASSID='CLSID:8856F961-340A-11D0-A96B-00C04FD705A2' style='display:none;'></object>"
    Dim bDelPdf As Boolean

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

    Friend m_sArchiveDescr As String = ""
    Friend m_sArchiveKey1 As String = ""
    Friend m_sArchiveKey2 As String = ""
    Friend m_sArchiveKey3 As String = ""
    Friend m_sArchiveWhat As String = "" 'vk 03.20
    'vk 07.13
    Friend m_sCG As String = ""
    'vk 12.13
    'Friend m_bTray As Boolean = False
    'vk 12.14
    Friend m_sHova As String
    'vk 11.15
    Const DONT_ANSWER = "don't need to answer to AS/400"
    'vk 09.20

    Friend m_sOneSomething As String
    Friend m_sSpecialCtl As String
    Friend m_sSideButton As String
    Friend m_sCurCap As String
    Friend m_sCurClass As String
    Friend m_sCurStyle As String = ""
    Friend m_bInSubCard As Boolean = False 'ntg 17.06.24 vladi changes regarding sub-title and accordion in card

    Friend m_InputId As String = ""

    Friend ActionNodes As List(Of String) = New List(Of String)
    Friend ActionNodesIcons As Dictionary(Of String, String) = New Dictionary(Of String, String)

    Friend singleFieldInLine As Boolean
    Friend DebugWarningList As List(Of String) = New List(Of String)

    Friend EwrErrorText As String = ""
    'Friend m_ModalScreen As String = "" 'rem vk 02.23
    Friend m_bStatementWindow As Boolean 'vk 09.24

    Friend Function IsFoldersScreen(oNodeScreen As Node) As Boolean
        Return (oNodeScreen.Param("fil").Trim() = "HFSCREEN" AndAlso oNodeScreen.Param("rec").Trim() = "PEMENU") Or (oNodeScreen.Param("fil").Trim() = "HFSCREEN" AndAlso oNodeScreen.Param("rec").Trim() = "PEHRIG1") Or (oNodeScreen.Param("fil").Trim() = "HFFLNAVFM" AndAlso oNodeScreen.Param("rec").Trim() = "ROPTION") 'ntg 05.02.24 added a popup screen with the PEMENU as background
        'Return (oNodeScreen.Param("fil").Trim() = "HFSCREEN" AndAlso oNodeScreen.Param("rec").Trim() = "PEMENU") Or (oNodeScreen.Param("fil").Trim() = "HFSCREEN" AndAlso oNodeScreen.Param("rec").Trim() = "PEHRIG1")  'ntg 05.02.24 added a popup screen with the PEMENU as background
    End Function

    Function IsMsgFile(FileName As String)
        Return FileName.Trim = MsgFile OrElse FileName.Trim = MsgFile2
    End Function
    Function getProperty(s As String, sDefault As String) As String 'vk 11.16
        Dim v As String
        v = m_dp.getProperty(s)
        If v = "" Then v = sDefault
        Return v
    End Function
    Function getProperty(s As String) As String 'vk 07.06
        Return m_dp.getProperty(s)
    End Function
    Friend Function getPropertyPset(s As String, oNodeScreen As Node) 'vk 06.10
        Dim sReturn As String
        sReturn = getProperty(oNodeScreen.Param("pset") + "_" + s)
        If sReturn = "" Then sReturn = getProperty(s)
        Return sReturn
    End Function
    Function getPropertyStamp(s As String, stp As String) As String 'vk 11.09
        Dim r As String
        r = m_dp.getProperty("Stamp" + s + "_" + stp)
        If r = "" Then r = m_dp.getProperty("Stamp" + s)
        Return r
    End Function

    Function getPropertyHttp(s As String) As String 'vk 09.07
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
    Function getPropertyVal(s As String) As Integer 'vk 09.06
        Try
            Return Int64.Parse(m_dp.getProperty(s))
        Catch
            Return 0
        End Try
    End Function
    Function getPropertyStampVal(s As String, stp As String) As Integer 'vk 11.09
        Dim r As String
        r = m_dp.getProperty("Stamp" + s + "_" + stp)
        If r = "" Then r = m_dp.getProperty("Stamp" + s)
        Try
            Return Int64.Parse(r)
        Catch
            Return 0
        End Try
    End Function
    Function getPropertyValSng(s As String, Optional nDefault As Single = 0) As Single 'vk 09.06
        Try
            Return Single.Parse(m_dp.getProperty(s))
        Catch
            Return nDefault 'vk 05.10
        End Try
    End Function
    Function Memory() As PocketKnife.Memory 'vk 09.06
        Return m_mem
    End Function
    Function ShowTable() As PocketKnife.ShowTable 'vk 06.16
        Return m_sht
    End Function

    Sub New(dp As Reader, mem As PocketKnife.Memory, sht As PocketKnife.ShowTable, sColorXml As String, sMapPath As String,
            sUser As String, sGuid As String, sJob As String, sUrl As String, sIP As String, sClient As String, Optional SessionId As String = "")

        Dim v As String
        Dim pp As New PocketKnife.Info()

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
        m_sSession = SessionId

        sSqlServer = getProperty("SqlServer")
        sSqlUser = getProperty("SqlUser")
        sSqlPass = pp.DecryptPassword(getProperty("SqlPass"))

        'vk 08.07
        m_nCode_Bold = getPropertyVal("Code_Bold")
        m_nCode_Size2 = getPropertyVal("Code_Size2")
        m_nCode_Size3 = getPropertyVal("Code_Size3")
        m_nCode_Half = getPropertyVal("Code_Half")
        m_nCode_Usual = getPropertyVal("Code_Usual")
        m_nCode_Gray = getPropertyVal("Code_Gray")

        'vk 04.11
        m_nCode_Bold_Underline = getPropertyVal("Code_Bold_Underline")
        m_nCode_Size2_Underline = getPropertyVal("Code_Size2_Underline")
        m_nCode_Underline = getPropertyVal("Code_Underline")

        Try
            m_colColor = qColor.GetColorTable(Me, sColorXml)

            qColor.AddColors(m_colColor, getProperty("BackGround"), "", "_bckg")
            qColor.AddColors(m_colColor, getProperty("ForeGround"), "", "_forg")
            qColor.AddColors(m_colColor, getProperty("Grid"), "grid", "_bckg")
            qColor.AddColors(m_colColor, getProperty("Grid0"), "grid0", "_bckg") 'vk 10.06

        Catch ex1 As Exception
            Throw ex1
        End Try

        For Each v In AllStyles
            Try
                Dim s As String = getProperty(v) 'Resol(v)
                m_cCss.Add(New Attrib(v, s), v)
            Catch e As Exception
            End Try
        Next
    End Sub

    Sub FilRec(ByRef sXmlFil As String, ByRef sXmlRec As String) 'vk 05.09
        sXmlFil = m_sXmlFil
        sXmlRec = m_sXmlRec
    End Sub
    Function ErrorInBuffer() As Boolean 'vk 05.12
        Return m_bErrorInBuffer
    End Function

    Sub UpdateLocalCounter(pLocalCounter As String)
        XmlLocalCouner = pLocalCounter
    End Sub

    Function SetSTableValue(sXml As String, ByRef copyPdfFilePath As String) As String

        'If GetScreenParamValue(sXml, "wait") = "S" Then ' The xml file before getting the pdf Spool
        If GetScreenParamValue(sXml, "wait") = "S" AndAlso qBoo.m_ModalScreen = "" Then ' ntg 10.07.23 added AndAlso qBoo.m_ModalScreen = "" for when previous page is pdf and next is modal screen
            copyPdfFilePath = GetScreenParamValue(sXml, "path")
            Return "spoolpdfnew"
        End If

        Select Case GetScreenParamValue(sXml, "wait")
            Case "B", "V" : Return GetScreenParamValue(sXml, "brz")
            Case "H" : Return "memory_delete"
            Case "Y" : Return "memory_delete_by_key"
        End Select

        If GetScreenParamValue(sXml, "sic") = "I" Then
            Dim imgValue As String = GetScreenParamValue(sXml, "img")
            m_sImageMethod = "I" + imgValue
            Select Case imgValue
                Case "I" : Return "image"
                Case "J" : Return "imagetoken" 'vk 05.22
                Case "F" : Return "imagepost"
                Case "U" : Return "imageurl"
                Case "V" : Return "imageurltoken" 'vk 05.22
                Case "P" : Return "imageurlpost"
            End Select
        End If

        Return ""
    End Function

    Function GetPage(sXml As String, sVersion As String, sLastEntry As String, sWarning As String, sScreenNum As String, ByRef bStopSession As Boolean, ByRef nMaxLen As Long,
                     ByRef sTable As String, ByRef sCommand As String, ByRef copyPdfFilePath As String, sPdf As String, nCall As Integer, bFirstScreen As Boolean,
                     As400WarningStr As String, ByRef PageHeaderHtml As String, DifferentPage As Boolean, ModalScreen As String, sQpxlNew As String) As String

        If sXml.Trim = "" Then
            Throw New Exception("Connection broken, empty string received.")
            Return ""
        End If
        m_nCall = nCall
        m_sPdf = sPdf

        If ModalScreen <> "" Then
            qBoo.m_ModalScreen = ModalScreen 'vk 02.23
            'm_ModalScreen = qBoo.Xml2html(Me, ModalScreen, sScreenNum, sLastEntry, sWarning, sVersion, sCommand, bFirstScreen, bStopSession, As400WarningStr, PageHeaderHtml, DifferentPage, True)
        End If

        If nCall = 1 Then
            sTable = SetSTableValue(sXml, copyPdfFilePath)

            If sTable = "30" OrElse sTable = "memory_delete" OrElse sTable = "memory_delete_by_key" Then
                Return ""
            End If

            If GetScreenParamValue(sXml, "sic") = "I" Then
                ConcatFields(sXml, sCommand)
                m_sCommand = sCommand
            End If
        End If

        If nCall = 2 Then
            cButtons.Clear()
        End If

        Dim HtmlObj As String = qBoo.Xml2html(Me, sXml, sScreenNum, sLastEntry, sWarning, sVersion, sCommand, bFirstScreen, bStopSession, As400WarningStr, PageHeaderHtml, DifferentPage, False, sQpxlNew)
        If HtmlObj.Length > nMaxLen Then
            Return LongHtml()
        Else
            Return HtmlObj
        End If

        Return ""
    End Function

    Function RetIsEnglish()
        Return qBoo.IsEnglish
    End Function



    Private Sub ConcatFields(sXml As String, ByRef sCommand As String)
        Dim xmlDoc As XDocument = XDocument.Parse(sXml)

        For Each FieldElem As XElement In xmlDoc.Descendants("f")
            sCommand += FieldElem.Attribute("val").Value.ToString().Trim()
        Next
    End Sub

    Private Function LongHtml(Optional sMessage As String = "The HTML is too long") As String 'vk 03.10
        Dim s As String
        s = qMulti.FromFile(m_sMapPath + "\Color\LongHtml.htm")
        s = s.Replace("@@", sMessage)
        s = s.Replace("@R@", IIf(getProperty("SameWindow").ToLower = "true", "flexible", "hard")) 'vk 01.12
        Return s
    End Function

    Private Function GetScreenParamValue(sXML As String, sParam As String) As String
        Dim xmlDoc As XDocument = XDocument.Parse(sXML)
        Dim sElement As XElement = xmlDoc.Descendants("s").First()

        If sElement.Attribute(sParam) Is Nothing Then
            Return ""
        Else
            Return sElement.Attribute(sParam)
        End If
    End Function

    Private Function ParamCheckByXml(sXML As String, sParam As String, sValue As String, bInitNode As Boolean) As Boolean
        Dim bRetVal As Boolean
        sr = New StringReader(sXML)
        rd = New Xml.XmlTextReader(sr)

        Do While rd.Read
            If rd.NodeType = Xml.XmlNodeType.Element Then
                Select Case rd.Name
                    Case "s"
                        oNodeScreenTmp = New Node(rd, numL, NOTSPACE)
                        If bInitNode Then
                            InitNodeScreen(oNodeScreenTmp, True, 1)
                        End If
                        bRetVal = oNodeScreenTmp.Param(sParam) = sValue
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
        Return bRetVal
    End Function

    Friend Function ResolValue(y1 As Single, y2 As Single, bReal As Boolean, sHorVert As String) As Integer 'vk 02.08, 05.09
        If bReal OrElse (Not m_bSpool) OrElse m_nCall = 2 Then
            Dim x1, x2 As Integer
            Select Case sHorVert
                Case "hor", "font" : x1 = 800 : x2 = 1024 ': x3 = m_nResolutionW
                Case "vert" : x1 = 550 : x2 = 700 ': x3 = m_nResolutionH
            End Select
            Dim d As Single = IIf(sHorVert = "font", 0, 0.3) 'vk 06.09
            Return Int((y2 - y1) + y1 + d)
        Else
            Return y1
        End If
    End Function
    Function ResizeFont(n As Integer, bDefpropButton As Boolean, bFirstLine As Boolean, bTop132 As Boolean) As Integer
        Dim r As Integer
        If bFirstLine Then
            'vk 06.10
            r = ResolValue(
                n * getPropertyValSng("FontCoefTop_800", 0.9),
                n * getPropertyValSng("FontCoefTop_1024", 1),
                bDefpropButton, "font")
            If bTop132 Then r = Int(r * 0.6)
        Else
            r = ResolValue(
                n * getPropertyValSng("FontCoef_800", 0.9),
                n * getPropertyValSng("FontCoef_1024", 1),
                bDefpropButton, "font")
        End If
        Return r
    End Function

    Friend Sub SetCellSize(ByRef oNodeScreen As Node, ByRef iScaleLin As Integer, ByRef iScaleWidth As Integer) 'vk 08.10
        Select Case oNodeScreen.ParamVal("fdsp")
            Case 0, 2 : iScaleLin = ResolValue(18, 24, False, "vert")
            Case Else : iScaleLin = ResolValue(16, 21, False, "vert")
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
                Case 0 : iScaleWidth = Int((m_nResolutionW) / 80) 'vk 06.10
                Case 1 : iScaleWidth = Int((m_nResolutionW) / 132) 'vk 06.10
                Case 2 : iScaleWidth = Int((m_nResolutionW) / 50)
                Case 3 : iScaleWidth = Int((m_nResolutionW) / 100)
                Case 4 : iScaleWidth = Int((m_nResolutionW) / 180)
                Case 5 : iScaleWidth = Int((m_nResolutionW) / 190)
                Case 6 : iScaleWidth = Int((m_nResolutionW) / 198)
            End Select
        End If
    End Sub

    Friend Function FixedFontSize(iScaleWidth As Integer) As Integer 'vk 06.09
        Dim n As Integer
        n = Int(iScaleWidth / 0.6 + 0.7) '- 1
        'If (Not bPdf) AndAlso (m_sClient Like "*Trident/7.0*" OrElse Not (m_sClient Like "*Trident*" OrElse m_sClient Like "*MSIE*")) Then
        '    n -= 1 'vk 04.15
        'End If
        Return n
    End Function
    Friend Function HeightByClass(sClass As String, ByRef oNodeScreen As Node) As Integer 'vk 08.07
        Return SizeByClass(iSpoolBaseFontSize.ToString, sClass, oNodeScreen) * iSpoolLineHeightCoef
    End Function
    Friend Function SizeByClass(sFontSize As String, sClass As String, ByRef oNodeScreen As Node) As Integer 'vk 01.06

        sFontSize = CStr(FixedFontSize(iScaleWidth) + Val(getPropertyPset("FontSizeDelta_Pdf", oNodeScreen))) 'vk 06.09

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

    Friend Function ArchiveKey() As String 'vk 07.13
        Return (m_sArchiveKey1 + m_sArchiveKey2 + m_sArchiveKey3).Trim 'vk 10.17 ' + m_sArchiveKey4
    End Function

    Friend Sub InitNodeScreen(ByRef oNodeScreen As Node, bNodeOnly As Boolean, nCall As Integer)

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

            If getProperty("DisableFKeys") = "true" Then
                .Param("fnn") = "".PadRight(26, "0")
            End If

            .Param("plgc") = "P"

            If bNodeOnly Then Exit Sub

            If sSqlUser = "" Then 'ntg 10.07.24 vladi's change regarding sql migration for bituah haklai
                m_sConnectionString = "Data Source=" + sSqlServer + ";Initial Catalog=" + .Param("flib").Trim +
                                ";Integrated Security=true;" + getProperty("ConnStrAddition")
            Else 'ntg 10.07.24 vladi's change regarding sql migration for bituah haklai
                m_sConnectionString = "Data Source=" + sSqlServer + ";Initial Catalog=" + .Param("flib").Trim +
                                ";User ID=" + sSqlUser + ";Password=" + sSqlPass + ";" + getProperty("ConnStrAddition")
            End If
            iButtonScrollF = 3
        End With
    End Sub

    Function GetXML2AS400ModCancel(sXMLModPage As String) As String

        Dim sXML As Text.StringBuilder
        Dim fld, fcmd, find As String
        Dim fil As String = ""
        Dim rec As String = ""
        Const Quo As String = """"

        sXML = New Text.StringBuilder()
        sXML.Append("<?xml version=" + Quo + "1.0" + Quo + " encoding=" + Quo + "Windows-1255" + Quo + "?>")
        sXML.Append("<screen>")

        fld = fldD
        find = findD
        fcmd = "XX" 'vk 07.05

        Dim sr As New StringReader(sXMLModPage)
        Dim rd As New Xml.XmlTextReader(sr)

        Try
            Do While rd.Read
                If rd.NodeType = Xml.XmlNodeType.Element Then
                    Select Case rd.Name
                        Case "s"
                            fil = rd.GetAttribute("fil")
                            rec = rd.GetAttribute("rec")
                            Exit Do
                    End Select
                End If
            Loop
        Catch e1 As Exception
            Throw New Exception("Error trying to reply in cancel mode. " + e1.Message, e1)
            Return ""
        End Try

        sXML.Append("<s fil=" + Quo + fil + Quo +
            " rec=" + Quo + rec + Quo +
            " fld=" + Quo + fld + Quo +
            " find=" + Quo + find + Quo +
            " fcmd=" + Quo + fcmd + Quo + "/>")
        sXML.Append("<fields></fields></screen>")
        Return sXML.ToString

    End Function

    Private Sub AdjustVal(ByRef val As String, ByRef sgn As String, typ As String, sKey As String, flr As String)

        If sKey > "" Then
            Select Case typ
                Case "S", "N"
                    val = qConv.ConvNum(Me, val, sKey, "PC2AS", " ", " ")
                    sgn = val.Substring(0, 1)
                    val = val.Substring(1)
                Case "A"
                    val = qConv.ConvAlf(Me, val, sKey, "PC2AS",
                        IIf(InStr(sListOfLtr, ";" + sKey + ";") > 0, "ltr", "rtl"),
                        False, True,
                        InStr(sListOfUni, ";" + sKey + ";") > 0, False, flr)
                    qConv.MakeOk(val) 'vk 07.04
                Case "D", "Y", "H", "Q", "B" 'Q vk 07.05, B vk 03.24
                    val = qConv.ConvDate(Me, val, sKey, "PC2AS")
                    sgn = "+" 'vk 07.05
                Case "T", "U"
                    val = qConv.ConvTime(Me, val, sKey, "PC2AS")
                    sgn = "+" 'vk 07.05
            End Select
        End If

        If val.Trim = "" Then Return
        Dim i As Integer
        For i = 1 To Len(val)
            If Chr(Asc(Mid(val, i, 1))) = "?" Then
                Mid(val, i, 1) = "?"
            End If
        Next

    End Sub

    Function ScreenNumFromHtml() As String
        Return sScreenNumFromHtml
    End Function
    Function DelPdf() As Boolean 'vk 04.09
        Return bDelPdf
    End Function
    Function GetXML2AS400(ctx As Web.HttpContext, ByRef sXmlOK As String, ByRef sFind As String, ByRef sFil As String, ByRef sGridPos As String,
                           sXmlFil As String, sXmlRec As String, ByRef sClient As String, ByRef ErrorInXML As String, ByRef sQpxlNew As String) As String

        Dim R As String
        Dim R_List As New List(Of String) 'ntg 26.06.24 for fgr=B screens with an option to select multiple rows in a table
        Dim bMultipleR As Boolean = False 'ntg 26.06.24 for fgr=B screens with an option to select multiple rows in a table
        Dim sKey As String
        Dim sXML As Text.StringBuilder
        Dim fld, fcmd, find, flr, fpch, qpxl, textlen, textused, err, fil, rec, rdf As String
        Dim fpchR As String = ""
        Dim num, ind, val, typ, sgn As String
        Dim i As Integer
        Dim bInGrid As Boolean = False
        Dim bModalWindow As Boolean
        Const Quo As String = """"

        If OneValue(ctx, "HdontAnswer") = "dont" Then
            Return DONT_ANSWER
        End If

        sXML = New Text.StringBuilder()
        sXML.Append("<?xml version=").Append(Quo).Append("1.0").Append(Quo).Append(" encoding=").Append(Quo).Append("Windows-1255").Append(Quo).Append("?>")
        sXML.Append("<screen>")

        sClient = OneValue(ctx, "Hclient")
        err = OneValue(ctx, "Herr")
        If err > "" Then
            sXML.Append("<s fil=" + Quo + "          " + Quo + " rec=" + Quo + "          " + Quo _
                + " fld=" + Quo + "          " + Quo + " find=" + Quo + "0000" + Quo + " fcmd=" + Quo _
                + err + Quo + "/><fields></fields></screen>")
            sXmlOK = sXML.ToString
            Return sXmlOK
        End If

        fld = OneValue(ctx, "Hfld").Replace(NOTSPACE, " ")
        flr = OneValue(ctx, "Hflr")
        fpch = OneValue(ctx, "Hfpch") 'vk 12.05
        fil = OneValue(ctx, "Hfil")
        rec = OneValue(ctx, "Hrec")
        If fil = "x" Then
            fil = sXmlFil
            rec = sXmlRec
        End If

        bModalWindow = (OneValue(ctx, "HModalWindow") = "true")

        If ctx.Current.Request.Form.GetValues("R") IsNot Nothing AndAlso ctx.Current.Request.Form.GetValues("R").Length > 1 Then 'ntg 26.06.24 for fgr=B screens with an option to select multiple rows in a table
            SaveMultiple_R_Values(ctx, R_List)
            bMultipleR = True
            R = R_List(1)
        Else
            R = OneValue(ctx, "R") 'vk 12.05 'ntg 26.06.24 the original state where you select only one row in a table
        End If

        If R > "" Then fpchR = R.Substring(R.IndexOf("_") + 1, 1) 'vk 12.05
        sListOfLtr = OneValue(ctx, "Hltr")
        sListOfUni = OneValue(ctx, "Huni") 'vk 01.05
        sListOfChk = OneValue(ctx, "Hchk") 'vk 05.21
        sScreenNumFromHtml = OneValue(ctx, "Hscreen") 'vk 04.05
        If fld Is Nothing OrElse fld = "" Then fld = fldD
        find = OneValue(ctx, "Hfind")
        If find Is Nothing OrElse find = "" Then find = findD

        'ntg 17.06.24 changes regarding focus in field- by vladi
        Dim find_per As String = OneValue(ctx, "Hfind_per")
        If find_per > "" Then
            find = find_per
            fld = OneValue(ctx, "Hfld_per").Replace(NOTSPACE, " ")
        End If
        '

        If Not IsNumeric(find) Then
            ErrorInXML = $"Field find is not a number: {find}"
        End If
        m_bErrorInBuffer = fil.Trim = "" OrElse rec.Trim = "" 'vk 05.12
        If m_bErrorInBuffer Then
            fcmd = "BF" 'vk 05.12
        Else
            fcmd = OneValue(ctx, "Hfcmd")
            If fcmd = "" Then
                fcmd = "00" 'vk 07.21
            End If
        End If
        textlen = OneValue(ctx, "Htextlen")
        textused = OneValue(ctx, "Htextused")
        rdf = OneValue(ctx, "Hrdf") 'vk 01.09
        bDelPdf = (OneValue(ctx, "Hdelpdf") = "true") 'vk 04.09

        sXML.Append("<s fil=").Append(Quo).Append(fil).Append(Quo)
        sXML.Append(" rec=").Append(Quo).Append(rec).Append(Quo)
        sXML.Append(" fld=").Append(Quo).Append(fld).Append(Quo)
        sXML.Append(" find=").Append(Quo).Append(find).Append(Quo)
        sXML.Append(" fcmd=").Append(Quo).Append(fcmd).Append(Quo)

        'qpxl = OneValue(ctx, "Hqpxl")
        '' Response Qpxl only if we got a value in Qpxl
        'If qpxl > "" AndAlso OneValue(ctx, "HInputQpxl") <> "" Then
        '    qpxl = qpxl.PadLeft(5, "0")
        '    If Len(qpxl) > 5 Then qpxl = Right(qpxl, 5)
        '    sXML.Append(" qpxl=").Append(Quo).Append(qpxl).Append(Quo)
        'End If
        'vk 09.24
        If OneValue(ctx, "HInputQpxl") > "" Then
            sXML.Append(" qpxl=").Append(Quo).Append("00000").Append(Quo)
        End If
        sQpxlNew = OneValue(ctx, "Hqpxl") & ";" & OneValue(ctx, "Haccords")

        sXML.Append("/>")
        sXML.Append("<fields>")

        Try
            If OneValue(ctx, "Hdll") <> "M" Then 'vk 04.06
                With ctx.Current.Request.Form
                    For i = 0 To .AllKeys.Length - 1
                        sKey = .AllKeys.GetValue(i)
                        Dim BeginingLetter As String = Left(sKey, 1)

                        If bModalWindow Then
                            If BeginingLetter = "M" Then ' אם זה חלון מודלי אז לקחת רק שדות שמתחילות באות אמ
                                ind = sKey.Substring(indS, indL)
                                If Int32.Parse(ind) <> 0 Then
                                    num = sKey.Substring(numS, numL).Replace(NOTSPACE, " ")
                                    typ = sKey.Substring(typS, typL)
                                    val = OneValue(ctx, sKey)
                                    If InStr(sListOfChk, ";" + sKey + ";") > 0 AndAlso Trim(val) > "" AndAlso Trim(fpch) > "" Then 'fpch vk 06.21
                                        val = fpch
                                    End If
                                    sgn = " "
                                    AdjustVal(val, sgn, typ, sKey, flr)
                                    sXML.Append("<f num=").Append(Quo).Append(num).Append(Quo)
                                    sXML.Append(" ind=").Append(Quo).Append(ind).Append(Quo)
                                    sXML.Append(" sgn=").Append(Quo).Append(sgn).Append(Quo)
                                    sXML.Append(" val=").Append(Quo).Append(val).Append(Quo).Append("/>")
                                End If
                            End If
                        Else
                            Select Case BeginingLetter
                                Case "F"
                                    ind = sKey.Substring(indS, indL)
                                    If Int32.Parse(ind) <> 0 Then
                                        num = sKey.Substring(numS, numL).Replace(NOTSPACE, " ")
                                        typ = sKey.Substring(typS, typL)
                                        val = OneValue(ctx, sKey)
                                        If InStr(sListOfChk, ";" + sKey + ";") > 0 AndAlso Trim(val) > "" AndAlso Trim(fpch) > "" Then 'fpch vk 06.21
                                            val = fpch
                                        End If
                                        sgn = " "
                                        AdjustVal(val, sgn, typ, sKey, flr)
                                        sXML.Append("<f num=").Append(Quo).Append(num).Append(Quo)
                                        sXML.Append(" ind=").Append(Quo).Append(ind).Append(Quo)
                                        sXML.Append(" sgn=").Append(Quo).Append(sgn).Append(Quo)
                                        sXML.Append(" val=").Append(Quo).Append(val).Append(Quo).Append("/>")
                                    End If
                                Case "W" 'vk 11.06
                                    If sKey <> "W" Then
                                        ind = sKey.Substring(2, 4)
                                        If Int64.Parse(ind) <= Int64.Parse(textused) Then
                                            num = "!00000" + ind
                                            typ = "A"
                                            val = OneValue(ctx, sKey)
                                            sgn = " "
                                            AdjustVal(val, sgn, typ, "W" + num + ind + "A" + textlen + "00", flr)
                                            sXML.Append("<f num=").Append(Quo).Append(num).Append(Quo)
                                            sXML.Append(" ind=").Append(Quo).Append(ind).Append(Quo)
                                            sXML.Append(" sgn=").Append(Quo).Append(sgn).Append(Quo)
                                            sXML.Append(" val=").Append(Quo).Append(val).Append(Quo).Append("/>")
                                        End If
                                    End If
                                Case "Z" 'vk 12.05
                                    ind = sKey.Substring(indS + 1, indL)
                                    If ind = find Then bInGrid = True 'vk 01.09
                                    num = sKey.Substring(numS + 1, numL).Replace(NOTSPACE, " ")
                                    typ = sKey.Substring(typS + 1, typL)
                                    val = " "
                                    sgn = " "

                                    If bMultipleR Then 'ntg 26.06.24 for fgr=B screens with an option to select multiple rows in a table
                                        Dim oneR As String

                                        For Each oneR In R_List
                                            If oneR > "" AndAlso fcmd <> "90" AndAlso fcmd <> "91" AndAlso oneR.Substring(numS, numL).Replace(NOTSPACE, " ") = num Then '> 90-91 vk 12.04
                                                If fpch > "" Then
                                                    val = fpch
                                                Else
                                                    val = fpchR
                                                End If
                                                If val = " " AndAlso rdf <> " " Then
                                                    val = rdf 'vk 01.09
                                                End If
                                            End If
                                        Next

                                    Else
                                        If R > "" AndAlso fcmd <> "90" AndAlso fcmd <> "91" AndAlso R.Substring(numS, numL).Replace(NOTSPACE, " ") = num Then '> 90-91 vk 12.04
                                            If fpch > "" Then
                                                val = fpch
                                            Else
                                                val = fpchR
                                            End If
                                            If val = " " AndAlso rdf <> " " Then
                                                val = rdf 'vk 01.09
                                            End If
                                        End If
                                    End If
                                    AdjustVal(val, sgn, typ, sKey.Substring(1), flr) 'vk 09.10
                                    sXML.Append("<f num=").Append(Quo).Append(num).Append(Quo)
                                    sXML.Append(" ind=").Append(Quo).Append(ind).Append(Quo)
                                    sXML.Append(" sgn=").Append(Quo).Append(sgn).Append(Quo)
                                    sXML.Append(" val=").Append(Quo).Append(val).Append(Quo).Append("/>")
                            End Select
                        End If
                    Next
                End With
            End If
            sXML.Append("</fields>")
            sXML.Append("</screen>")

            'vk 01.09
            sFind = find
            sFil = fil
            sGridPos = OneValue(ctx, "Hgridpos")
            If sGridPos = "remember" AndAlso Not bInGrid Then
                sGridPos = "delete"
            End If
            'vk 07.04
            Dim s As String = sXML.ToString
            sXmlOK = s
            qConv.MakeNotOk(s)
            Return s
        Catch e1 As Exception
            Throw New Exception("Error trying to reply. " + e1.Message, e1)
            Return ""
        End Try

    End Function
    Private Function OneValue(ctx As Web.HttpContext, sName As String) As String 'vk 02.05
        Try
            'If ctx.Current.Request.Form.AllKeys Then
            Dim s As String
            For Each s In ctx.Current.Request.Form.AllKeys 'vk 10.11
                If s = sName Then
                    Return ctx.Current.Request.Form.GetValues(sName)(0)
                End If
            Next
            Return ""
        Catch
            Return ""
        End Try
    End Function

    Private Sub SaveMultiple_R_Values(ctx As Web.HttpContext, ByRef R_List As List(Of String))
        Try
            Dim s, r As String
            For Each s In ctx.Current.Request.Form.AllKeys 'vk 10.11
                If s = "R" Then
                    For Each r In ctx.Current.Request.Form.GetValues("R")
                        R_List.Add(r)
                    Next
                End If
            Next
        Catch
        End Try
    End Sub

    Function GetUpdatePageXML(PageXML As String, ChangesXML As String) As String

        Dim xmlD As Xml.XmlDocument
        Dim sr As New StringReader(ChangesXML)
        Dim rd As Xml.XmlTextReader
        Dim root, node, node1 As Xml.XmlNode
        Dim atrL As Xml.XmlAttributeCollection

        Dim num, ind, val As String
        Dim sXPath As String

        Try 'vk 02.07
            rd = New Xml.XmlTextReader(sr)
            xmlD = New Xml.XmlDocument()
            xmlD.LoadXml(PageXML)
            root = xmlD.DocumentElement

            Do While rd.Read
                If rd.NodeType = Xml.XmlNodeType.Element Then
                    Select Case rd.Name
                        Case "f"
                            num = rd.GetAttribute("num")
                            If Left(num, 1) <> "!" Then 'vk 11.06, 07.07
                                ind = rd.GetAttribute("ind")
                                val = rd.GetAttribute("val")
                                sXPath = "//f[@num='" + num + "' and @ind='" + ind + "']"
                                node = root.SelectSingleNode(sXPath)
                                If node IsNot Nothing Then
                                    atrL = node.Attributes
                                    node1 = atrL.GetNamedItem("val")
                                    node1.Value = val
                                End If
                            End If
                    End Select
                End If
            Loop
            Return xmlD.InnerXml.Replace("><", "> <")
        Catch e1 As Exception
            Return PageXML
        End Try

    End Function

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

        If Not oNodeScreenSaved Is Nothing Then oNodeScreenSaved.Dispose()
        oNodeScreenSaved = Nothing
    End Sub
End Class
