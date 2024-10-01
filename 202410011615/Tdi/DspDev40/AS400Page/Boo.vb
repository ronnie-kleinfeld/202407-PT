Imports System.Data.SqlClient 'vk 05.21
Imports System.IO
Imports System.Text
Imports System.Linq
Imports System.Xml.Linq
Imports System.Net.Http 'vk 01.22
Imports System.Net
Imports System.Security
Imports System.Net.Security
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Security.Policy
Imports System.Security.Authentication.ExtendedProtection

Public Class ReturnValue 'vk 05.22
    Public version As String
    Public created As Long
    Public message As String
    Public status As String
    Public code As Long
    Public data As ReturnValue_Data
End Class
Public Class ReturnValue_Data
    Public token As String
End Class

Public Class DdsLine
    Friend cNodes As New Collection
    'Friend ColList As List(Of Integer) = New List(Of Integer)
    Friend sPattern As String = ""
    Friend bHasEwrH As Boolean = False 'vk 02.24
    Friend sEwrD As String = "" 'vk 03.24+ changes for special header split with '|'
    Friend sNameForPf4 As String = "" 'vk 04.24+ ntg 10.04.24 changes for special header split with '|'
    Friend nCard As Integer = 0
    Friend LabelsList As List(Of Node) = New List(Of Node)
End Class

Public Class FromTo
    Friend nFrom As Integer = 0
    Friend nTo As Integer = 0
    Friend sHtml As String = ""
    Sub Init(sFrom As String, sTo As String)
        nFrom = Val(sFrom)
        nTo = Val(sTo)
    End Sub
    Function Contains(n As Integer) As Boolean
        If nFrom = 0 OrElse nTo = 0 Then Return False
        Return nFrom <= n AndAlso n <= nTo
    End Function
    Function IsAfter(n As Integer) As Boolean
        If nFrom = 0 OrElse nTo = 0 Then Return False
        Return n > nTo
    End Function
    Function Wid() As Integer
        Return nTo - nFrom + 1
    End Function
End Class

Public Class Boo

    Friend Const numL As Integer = 10
    Friend Const indL As Integer = 4
    Friend Const typL As Integer = 1
    Friend Const lenL As Integer = 5
    Friend Const decL As Integer = 2
    Friend Const usL As Integer = 1
    Const NOTSPACE As String = "~"
    Dim qMulti As New Multi
    Dim qOnce As New Once
    Dim qAround As New Around
    Dim oNodeScreen As Node
    Dim sOut_Local As New StringBuilder()
    Dim sOut As New StringBuilder()
    Dim sOutO As New StringBuilder()
    Dim sOutH As New StringBuilder()

    Dim oDdsLine As DdsLine
    Dim nScreenWid As Integer = 0
    Dim yT, yS, yL, x0 As New FromTo
    Dim x As New Collection
    Dim xTmp As New Collection 'vk 03.24+ changes for special header split with '|'

    Dim sPageTitle As String = ""
    Dim nCard As Integer = 0
    Dim nMaxCard As Integer = 0
    Dim nMaxRect As Integer = 0
    Dim cFolders As New Collection
    Dim SpecialFields As New List(Of Node)
    Dim cFirstLineNodes As New List(Of Node)
    Dim cDdsLines As New Collection
    Dim cCardTitles As New Collection
    Dim CardTitlesNodes As New Collection
    Dim cRectangles As New Collection
    Dim oNodeMiddle As Node
    Dim nMaxLine As Integer = 0
    Dim nMaxLine_Hard As Integer = 0
    Dim nGridStart As Integer = 0
    Dim nLine, nColumn As Integer

    Dim HiddentFieldId As String = ""
    Dim CheckBoxMark As String = ""

    Dim sMenuHiddens As String = qMulti.Hidden("R")
    Dim oNode As Node
    Dim bEnglish As Boolean
    Dim bIsShowMore As Boolean = False
    Dim bIsPgDnBtn As Boolean = False
    Dim nColFrom, nColTo, nColStep As Integer
    Dim cGraphData As Collection
    Dim cGraphData0 As Collection
    Dim oMaxInd1 As Collection
    Dim oMaxInd2 As Collection
    Dim UseAccordion As Boolean = False
    Dim CardCounter As Integer
    Dim CardNumberOnScreen As Integer = 0 'ntg 9.2.23 - identifying the bottom card to spread through the screen width
    Dim CombinedHeadersDict As Dictionary(Of Integer, String) = New Dictionary(Of Integer, String)

    Dim PageHeaderHtml, HeaderCompanyName, HeaderUserName, HeaderMasof, HeaderPageDate, ScreenName As String
    Dim ScreenHasButtonList As Boolean = False
    Dim btnListInXML As New List(Of String)

    Dim screenFilName As String = ""
    Dim screenActionMode As String = ""

    Friend m_ModalScreen As String = "" 'vk 02.23
    Dim onlyVal As String 'ntg 29.06.23 holds the value of the 'only' parameter in xml ('Y'/'E')
    Private IsQtyp3 As Boolean = False 'ntg 04.08.24 todo combine to one param
    Private IsQtyp2 As Boolean = False 'ntg 04.08.24 todo combine to one param
    Dim bAgentStatement As Boolean = False 'vk 02.24++
    Private qCombo As New Combo()

    Dim countBreadCrumbs As Integer = 0
    Dim actualTableLinesCounter As Integer = 0 'ntg 18.06.24 counts amount of lines in a table
    Dim firstHardTableRow As Integer = 0 'ntg 18.06.24 holds number of first hard Row number in a table
    Dim sideBarfields As Dictionary(Of String, String) = New Dictionary(Of String, String) 'ntg 16.07.24 changes regarding sidebar fields


    Function Xml2html(ByRef bp As BuildPage, sXml As String, sScreenNum As String, sLastEntry As String, sWarning As String,
                      sTitle As String, sCommand As String, bFirstScreen As Boolean, ByRef bStopSession As Boolean, As400WarningStr As String,
                      ByRef PageHeaderHtml As String, DifferentPage As Boolean, IsModalWindow As Boolean, sQpxlNew As String) As String
        Dim sr As StringReader, rd As Xml.XmlTextReader
        Dim qstr As Integer = 0
        Dim nLastCardUsed As Integer = -1
        Dim nRect As Integer, sLine As String

        Dim bExist_Title As Boolean = False
        Dim bExist_Teur As Boolean = False
        Dim IsToArchive As Boolean = False
        Dim qConv As New Conv()

        bp.cMenuLines = New Dictionary(Of String, MenuLine)

        cGraphData = New Collection
        cGraphData0 = New Collection
        oMaxInd1 = New Collection
        oMaxInd2 = New Collection

        sr = New StringReader(sXml)
        rd = New Xml.XmlTextReader(sr)

        'vk 06.23 ; ntg 28.06.23 page HeaderCompanyName 
        HeaderCompanyName = bp.getProperty("CompanyName")
        HeaderUserName = bp.m_sUser.ToUpper()
        HeaderMasof = Mid(bp.m_sJob, 2, 6)
        HeaderPageDate = DateTime.Now.ToString("dd/MM/yyyy")

        Do While rd.Read
            If rd.NodeType = Xml.XmlNodeType.Element Then
                oNode = New Node(rd, numL, NOTSPACE)
                Select Case rd.Name
                    Case "s"
                        Select Case oNode.ParamVal("fdsp") 'vk 09.05
                            Case 0 : nScreenWid = 80
                            Case 1 : nScreenWid = 132
                            Case 2 : nScreenWid = 50
                            Case 3 : nScreenWid = 100
                            Case 4 : nScreenWid = 180
                            Case 5 : nScreenWid = 190
                            Case 6 : nScreenWid = 198
                        End Select
                        oNodeScreen = oNode
                        'If oNodeScreen.ParamVal("fil") AndAlso oNodeScreen.ParamVal("rec") Then
                        oNodeScreen.ReplaceWin(oNodeScreen.Param("fil"), oNodeScreen.Param("rec")) 'ntg 31.05.23 treating the special screen fil="HFFLNAVFM " rec="ROPTION   "  as not win=R
                        'End If

                        bEnglish = oNodeScreen.ParamVal("flr") > 0
                        If bEnglish Then
                            nColFrom = 1
                            nColTo = nScreenWid
                            nColStep = 1
                        Else
                            nColFrom = nScreenWid
                            nColTo = 1
                            nColStep = -1
                        End If
                    Case "x" 'vk 09.24 fgr<>D
                        If oNodeScreen.Param("fgr") <> "D" AndAlso BelongsToThisScreen(oNode, IsModalWindow) Then 'vk 02.24 'AndAlso Not IgnoreGrid(IsModalWindow)-ntg 17.03.24 changes by vladi
                            'If BelongsToThisScreen(oNode, IsModalWindow) AndAlso oNodeScreen.Param("fil").TrimEnd() <> "HFFLNAVFM" AndAlso oNodeScreen.Param("rec").TrimEnd() <> "ROPTION" Then  'ntg 31.05.23 ignore special screen-removes lines in yzones that we dont want to show
                            x0 = New FromTo
                            x0.Init(oNode.Param("xs"), oNode.Param("xn"))
                            x.Add(x0, oNode.ParamVal("xn").ToString)
                        End If
                    Case "y" 'vk 09.24 fgr<>D
                        If oNodeScreen.Param("fgr") <> "D" AndAlso BelongsToThisScreen(oNode, IsModalWindow) Then 'AndAlso Not IgnoreGrid(IsModalWindow) Then 'vk 02.24
                            Select Case oNode.Param("yt")
                                Case "T" : yT.Init(oNode.Param("ys"), oNode.Param("yn"))
                                Case "S" : yS.Init(oNode.Param("ys"), oNode.Param("yn"))
                                Case "L" : yL.Init(oNode.Param("ys"), oNode.Param("yn"))
                            End Select
                            If oNode.ParamVal("ys") > 0 AndAlso (oNode.ParamVal("ys") < nGridStart OrElse nGridStart = 0) Then
                                nGridStart = oNode.ParamVal("ys")
                            End If
                        End If
                        'if a line is after the grid (and a grid exists), we are not in a table
                        'if a cell is empty, leave it empty (including title rows)
                        'if the title is one long line (font=8), split it into columns
                    Case Else 'vk 02.24
                        If oNode.ParamExists("val") Then
                            If oNode.ParamExists("ewr") Then
                                If oNode.Param("ewr") <> "H" AndAlso oNode.Param("ewr") <> "D" Then 'vk 03.24+ ntg 10.03.24 changes for special header split with '|'
                                    oNode.Param("val") = oNode.Param("val").Trim
                                End If
                                'vk 02.24+
                                If oNode.ParamExists("lvl") AndAlso oNode.Param("inp") <> "P" Then
                                    oNode.Param("val") = oNode.Param("val").Trim
                                End If
                            Else
                                oNode.Param("val") = oNode.Param("val").Trim
                            End If
                        End If
                End Select
            End If
        Loop

        rd.Close()
        sr = New StringReader(sXml)
        rd = New Xml.XmlTextReader(sr)
        Do While rd.Read
            If rd.NodeType = Xml.XmlNodeType.Element Then
                oNode = New Node(rd, numL, NOTSPACE)
                Select Case rd.Name
                    Case "d"
                        If oNode.Param("qtyp") = "3" Then 'ntg 28.02.24 check if the parameter exists for flexible special screen
                            IsQtyp3 = True
                        End If
                        If oNode.Param("qtyp") = "2" Then 'ntg 04.08.24 check if the parameter exists for flexible special screen
                            IsQtyp2 = True
                        End If
                    Case "f"
                        If BelongsToThisScreen(oNode, IsModalWindow) Then
                            Select Case True
                            'Case oNode.ForBoo("middle", nScreenWid) : bExist_Middle = True
                                Case oNode.ForBoo("title") : bExist_Title = True
                                Case oNode.ForBoo("teur") : bExist_Teur = True
                            End Select

                            If oNode.Param("num") IsNot Nothing AndAlso oNode.Param("num").Replace("~", " ").Trim = "TVIA" Then
                                bp.m_sSystem = "T"
                            End If

                            If oNode.Param("ewr") IsNot Nothing AndAlso oNode.Param("ewr") = "M" Then 'ntg 26.02.23- for indication of '+' in 400 and blinking PgDN
                                bIsShowMore = True
                                'Else bIsShowMore = False
                            End If

                            Select Case oNode.Param("tch")
                                Case "B" : bp.m_sArchiveDescr = oNode.Param("val")
                                    qConv.MakeOk(bp, oNodeScreen, bp.m_sArchiveDescr)
                                Case "C" : bp.m_sArchiveKey1 = oNode.Param("val")
                                Case "D" : bp.m_sArchiveKey2 = oNode.Param("val")
                                Case "E" : bp.m_sArchiveKey3 = oNode.Param("val")
                                Case "G" : bp.m_sArchiveWhat = oNode.Param("val")
                                Case "A" : IsToArchive = True
                                Case "3" : bp.m_sCG = oNode.Param("num")
                            End Select

                            'If oNode.Param("pf4") = "G" AndAlso oNode.ParamExists("apr") Then
                            If oNodeScreen.Param("fgr") = "P" AndAlso oNode.ParamExists("apr") AndAlso oNode.ParamExists("pxn") Then
                                'vk 05.21
                                Dim apr As String = oNode.Param("apr").PadRight(2)
                                Dim ind1 As Integer = InStr("ABCDEFGHIJKLMNOPQRSTUVWXYZ", Mid(apr, 2))
                                Dim ind2 = Val(Left(apr, 1))
                                Dim pxn As String = oNode.Param("pxn")
                                Dim asGraphData(26, 10) As String
                                Dim asGraphData0(26) As String
                                Dim nMaxInd1 As Integer
                                Dim nMaxInd2 As Integer
                                If cGraphData.Contains(pxn) Then
                                    asGraphData = cGraphData(pxn)
                                    asGraphData0 = cGraphData0(pxn)
                                    nMaxInd1 = oMaxInd1(pxn)
                                    nMaxInd2 = oMaxInd2(pxn)
                                End If
                                If ind2 = 0 Then
                                    asGraphData0(ind1) = Heb(Trim(oNode.Param("val")))
                                Else
                                    asGraphData(ind2, ind1) = oNode.ParamVal("val")
                                End If
                                If ind1 > nMaxInd1 Then nMaxInd1 = ind1
                                If ind2 > nMaxInd2 Then nMaxInd2 = ind2
                                If Not cGraphData0.Contains(pxn) Then
                                    cGraphData0.Add(asGraphData0, pxn)
                                End If
                                If cGraphData.Contains(pxn) Then
                                    oMaxInd1.Remove(pxn)
                                    oMaxInd2.Remove(pxn)
                                Else
                                    cGraphData.Add(asGraphData, pxn)
                                End If
                                oMaxInd1.Add(nMaxInd1, pxn)
                                oMaxInd2.Add(nMaxInd2, pxn)
                            End If
                            If oNodeScreen.Param("fil").Trim = bp.MenuFile AndAlso oNode.ParamVal("lin") > 1 Then
                                Dim oMenuLine As MenuLine

                                If bp.cMenuLines.ContainsKey(oNode.Param("lin")) Then
                                    oMenuLine = bp.cMenuLines(oNode.Param("lin"))
                                Else
                                    oMenuLine = New MenuLine(oNode.ParamVal("lin"))
                                    bp.cMenuLines.Add(oNode.Param("lin"), oMenuLine)
                                End If

                                Dim s As String

                                'rk 11.07.24 if f num="OP" set flag to True
                                If oNode.Param("num").Trim("~").Trim() = "OP" Then
                                    With oNode
                                        s = "F" + .LongName +
                                                oNodeScreen.Param("fdate") + "_" +
                                                oNodeScreen.Param("fch")
                                        sMenuHiddens &= qMulti.Hidden("Z" + s, oNode.Param("val"))
                                    End With
                                    oMenuLine.nColumn = oNode.ParamVal("col")
                                    oMenuLine.sFld = oNode.Param("num")
                                    oMenuLine.sN = s
                                    oMenuLine.bIsMenuShortcut = True
                                    oMenuLine.nLen = oNode.Param("len")
                                End If


                                Select Case True
                                    Case oNode.Param("psl") = "G"

                                        With oNode
                                            s = "F" + .LongName +
                                                oNodeScreen.Param("fdate") + "_" +
                                                oNodeScreen.Param("fch")
                                            sMenuHiddens &= qMulti.Hidden("Z" + s, oNode.Param("val"))
                                        End With
                                        oMenuLine.nColumn = oNode.ParamVal("col")
                                        oMenuLine.sFld = oNode.Param("num")
                                        oMenuLine.sN = s
                                    Case oNode.Param("val") = "+"
                                        oMenuLine.bSubMenu = True
                                    Case Else
                                        s = oNode.Param("val")

                                        Dim i1 As Integer = InStr(s, "~")
                                        If i1 > 0 Then
                                            Dim i2 As Integer = InStr(i1 + 1, s, "~")
                                            If i2 > 0 Then
                                                Dim sLink As String = Mid(s, i1 + 1, i2 - i1 - 1).Trim
                                                Select Case sLink
                                                    Case "1" 'vk 05.11
                                                        sLink = DateTime.Now.ToString("dd\/MM\/yyyy HH\:mm\:ss") & "$" & bp.m_sUser & "$" & bp.m_sIP
                                                        sLink = cryptComtec.RijndaelSimple.EncryptSimple(sLink)
                                                        sLink = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(sLink))
                                                        oMenuLine.sLink = bp.getPropertyHttp("InfoBayPath") & System.Web.HttpUtility.UrlEncode(sLink)
                                                    Case Else
                                                        oMenuLine.sLink = sLink.Replace("%%", bp.m_sUser) 'vk 08.06
                                                End Select
                                                s = Left(s, i1 - 1) & Mid(s, i2 + 1)
                                            End If
                                        End If
                                        oMenuLine.sText = Heb(s).Replace("&", "&amp;").Replace("'", "&#39;").Replace("""", "&quot;").Replace("<", "&lt;").Replace(">", "&gt;")
                                End Select
                            End If
                        End If
                    Case "r"
                        If BelongsToThisScreen(oNode, IsModalWindow) Then

                            'If Not oNodeScreen.Param("win") = "R" Then   'ntg 06.03.23
                            nMaxRect += 1
                            oNode.AddIfNo("ftit", "")
                            oNode.Param("ftit") = Heb(oNode.Param("ftit"))
                            cRectangles.Add(oNode, nMaxRect.ToString)
                            'End If    'ntg 06.03.23
                        End If
                End Select
            End If
        Loop

        'If oNodeScreen.Param("win") = "R" Then       'ntg 06.03.23
        '    nMaxRect = 1
        '    Dim ModalNode As Node = New Node()
        '    ModalNode.Param("rx1") = oNodeScreen.Param("wcst")
        '    ModalNode.Param("rx2") = oNodeScreen.Param("wcol")
        '    ModalNode.Param("ry1") = oNodeScreen.Param("wlst")
        '    ModalNode.Param("ry2") = oNodeScreen.Param("wlin")
        '    cRectangles.Add(ModalNode, "1")
        'End If

        sr = New StringReader(sXml)
        rd = New Xml.XmlTextReader(sr)
        bp.cFlexCombos = New Collection()
        Do While rd.Read
            If rd.NodeType = Xml.XmlNodeType.Element Then
                oNode = New Node(rd, numL, NOTSPACE)
                Select Case rd.Name
                    Case "s"
                        qstr = oNode.ParamVal("qstr")
                        bp.InitNodeScreen(oNodeScreen, False, 1)
                    Case "l"
                        cFolders.Add(oNode)
                    Case "c" ': 
                        If m_ModalScreen = "" Then 'only add footer buttons that are relevant to modal screen if this is the case
                            btnListInXML.Add(oNode.Param("fk"))

                            OneButton(bp, oNode, "c")
                        End If
                    Case "f"
                        If oNode.ParamVal("col") >= 100 AndAlso oNodeScreen.Param("fdsp") = "0" Then 'vk 09.24
                            Dim nColumn As Integer = oNode.ParamVal("col")
                            oNode.Param("col") = (nColumn Mod 100).ToString("000")
                            oNode.Param("dummies") = (nColumn \ 100).ToString("0")
                        End If
                        If BelongsToThisScreen(oNode, IsModalWindow) Then
                            'ntg 25.01.24 mode in screen developement +ntg 05.03.24 more changes-code location+kanat
                            screenFilName = oNodeScreen.Param("fil").Trim

                            If oNode.Param("num").Contains("TEUR") AndAlso screenActionMode = "" AndAlso (oNode.Param("lin") = "02" Or oNode.Param("lin") = "01") AndAlso (screenFilName = "HFSCREEN" Or screenFilName = "HFFLNAVFM") AndAlso Not bp.getProperty("ExtraLogo").Contains("kanatNewLogo") Then
                                screenActionMode = Heb(Trim(oNode.Param("val")))
                                Exit Select
                            ElseIf screenFilName = "DCLKHF" AndAlso oNode.Param("lin") = "03" AndAlso oNode.Param("col") = "002" AndAlso oNode.ParamExists("val") AndAlso screenActionMode = "" Then
                                screenActionMode = Heb(Trim(oNode.Param("val")))
                                Exit Select
                            ElseIf screenFilName = "DCORAF" AndAlso oNode.Param("lin") = "03" AndAlso oNode.Param("col") = "002" AndAlso oNode.ParamExists("val") Then
                                screenActionMode = Heb(Trim(oNode.Param("val")))
                                Exit Select
                            ElseIf screenFilName = "DCBTLF" AndAlso oNode.Param("lin") = "12" AndAlso oNode.Param("col") = "005" AndAlso oNode.ParamExists("val") Then 'ntg 15.04.24 added another case for TVIOT
                                screenActionMode = Heb(Trim(oNode.Param("val")))
                                Exit Select
                            End If

                            If oNodeScreen.Param("fil").Trim = "HFFLNAVFM" AndAlso oNodeScreen.Param("rec").Trim = "ROPTION" Then
                                If oNode.ParamVal("lin") >= yT.nTo AndAlso oNode.ParamVal("lin") <= yL.nTo Then 'remove the header+content of non relevant card (list of actions)
                                    Continue Do ' Skip to the next iteration
                                End If
                            End If
                            If bp.IsFoldersScreen(oNodeScreen) AndAlso cFolders.Count = 0 Then
                                nColumn = oNode.ParamVal("col")

                                If oNode.ParamVal("lin") = 0 Then 'flexible line
                                    nLine = oNode.ParamVal("ind") + qstr - 1
                                Else 'hard line
                                    nLine = oNode.ParamVal("lin")
                                End If

                                If (nColumn = 13 OrElse (oNode.ParamExists("chb") AndAlso nColumn > 13)) Then
                                    Dim RecNumber = GetRectanleNumber(nColumn, nLine)
                                    If RecNumber = 0 OrElse RecNumber = 2 Then
                                        If oNode.ParamExists("chb") Then
                                            oNode.AddIfNo("dec", bp.decD)
                                            HiddentFieldId = "F" + oNode.LongName + oNodeScreen.Param("fdate")
                                            oNode.AddIfNo("HiddentFieldId", HiddentFieldId)
                                            CheckBoxMark = oNode.Param("chb")
                                        Else
                                            oNode.AddIfNo("HiddentFieldRef", HiddentFieldId)
                                            oNode.AddIfNo("Mark", CheckBoxMark)
                                        End If
                                        cFolders.Add(oNode)
                                    End If
                                End If
                            End If

                            If oNode.Param("ewr") = "E" Then 'ntg 16.1.23: created option for more than one error to be shown

                                bp.EwrErrorText += $"<div id=""OrangeAlert"" class='px-3 my-3 alert OrangeAlert alert-dismissible fade show' role='alert''>
                                 <span class='px-2' style=""margin-left:3rem;""> <i class='fa-xl fa-solid fa-triangle-exclamation'></i>
                                  {Heb(oNode.Param("val"))}</span>
                                  <button type='button' class='alertCloseButton btn-close' data-bs-dismiss='alert' aria-label='Close'></button>
                               </div>"
                            End If 'ntg 05.02.23: if english- sorted the close btn so it's not hidden

                            If oNode.Param("ewr") = "S" AndAlso oNode.Param("pf4") Is Nothing AndAlso {"A", "S", "N"}.Contains(oNode.Param("typ")) Then 'ntg 17.07.24 saving values for the side bar special section
                                ' Assuming sideBarfields is defined as a Dictionary
                                Dim found As Boolean = False
                                Dim keyToUpdate As String = ""

                                ' Loop through the dictionary to find if any value is "E"
                                Dim curVal As String = ""
                                For Each kvp As KeyValuePair(Of String, String) In sideBarfields
                                    If kvp.Value = "E" Then
                                        found = True
                                        curVal = kvp.Value
                                        If oNode.Param("typ") = "S" OrElse oNode.Param("typ") = "N" Then
                                            Dim longname As String = oNode.LongName()
                                            curVal = qConv.ConvNum(bp, oNode.Param("val"), "F" & oNode.LongName() & oNodeScreen.Param("fdate"), "AS2PC", oNode.Param("edt"), oNode.Param("ewr"))
                                        End If
                                        keyToUpdate = kvp.Key
                                        Exit For
                                    End If
                                Next

                                If found Then ' If a key with value "E" is found, update its value
                                    sideBarfields(keyToUpdate) = $"{curVal}{If(oNode.Param("sgn") = "-", "-", "")}"
                                    'sideBarfields(keyToUpdate) = Heb(Trim(oNode.Param("val")))
                                Else ' If no such key exists, add a new key-value pair
                                    sideBarfields.Add(Heb(Trim(oNode.Param("val"))), "E")
                                End If
                            End If

                            If oNodeScreen.ParamIn("win", "TC") AndAlso oNode.ParamVal("lin") = 0 Then

                                If oNode.ParamVal("ind") = 1 Then
                                    bp.m_nTextLen = oNode.ParamVal("len")
                                    bp.m_sTextPrl = oNode.Param("prl")
                                End If

                                bp.cText.Add(qConv.ConvAlf_Simple(bp, oNode.Param("val"), oNodeScreen, bp.m_sTextPrl))
                            End If

                            If oNode.ParamIn("apr", "BO", 2) AndAlso oNodeScreen.Param("fgr") <> "P" Then
                                Dim oFlexComboObj As New FlexCombo()
                                oNode.AddIfNo("dec", bp.decD)
                                Dim sN As String = "F" + oNode.LongName + oNodeScreen.Param("fdate")
                                oFlexComboObj.bBoss = oNode.ParamIn("apr", "B", 2)
                                oFlexComboObj.sGroup = Left(oNode.Param("apr"), 1)
                                oFlexComboObj.sN = sN
                                oFlexComboObj.sKod = oNode.Param("pxk").ToString
                                bp.cFlexCombos.Add(oFlexComboObj)
                            End If

                            'ntg 10.07.24 vladi's change regarding city-street screen
                            If bp.getProperty("CityStreet") = "inline" AndAlso oNode.ParamIn("apr", "CNST", 2) AndAlso oNodeScreen.Param("fgr") <> "P" Then
                                If Not bp.cApr.Contains(oNode.Param("apr")) Then
                                    bp.cApr.Add(oNode.Param("val"), oNode.Param("apr"))
                                End If
                            End If

                            If oNode.Param("pch") IsNot Nothing Then
                                cFolders.Add(oNode)

                                'ElseIf oNode.ParamYes("only") Then
                            ElseIf oNode.Param("only") IsNot Nothing AndAlso (oNode.Param("only") = "Y" Or oNode.Param("only") = "E") Then 'ntg 03.07.23 check if only = e, if so , change text to SAYEM///// Or oNode.Param("only") = " "
                                onlyVal = oNode.Param("only") 'ntg 03.07.23 check if only = e, if so , change text to SAYEM
                                SpecialFields.Add(oNode)

                            Else
                                If oNode.ParamIn("pf4", "CD") Then
                                    oNode.AddIfNo("OuterXml", rd.ReadOuterXml)
                                End If

                                Select Case True
                                    Case oNode.ParamVal("lin") = 1 AndAlso Not bp.IsMsgFile(oNodeScreen.Param("fil")) AndAlso oNode.Param("val").Trim() <> ""  'first line
                                        cFirstLineNodes.Add(oNode)
                                        SetSessionFirstLineVars(oNode, bp)
                                    Case oNode.ForBoo("title")
                                        oNodeMiddle = oNode
                                    Case oNode.ForBoo("teur") AndAlso Not bExist_Title
                                        oNodeMiddle = oNode
                                    Case Else
                                        If oNode.ParamVal("lin") = 0 Then 'flexible line
                                            nLine = oNode.ParamVal("ind") + qstr - 1
                                        Else 'hard line
                                            nLine = oNode.ParamVal("lin")
                                            If nLine > nMaxLine_Hard Then
                                                nMaxLine_Hard = nLine
                                            End If
                                        End If
                                        If nLine > nMaxLine Then
                                            nMaxLine = nLine
                                        End If
                                        nColumn = oNode.ParamVal("col")
                                        Dim oRect As Node
                                        Dim sRectFound As String = ""
                                        For nRect = 1 To nMaxRect
                                            oRect = cRectangles(nRect.ToString)
                                            If oRect.ParamVal("rx1") <= nColumn _
                                                AndAlso nColumn <= oRect.ParamVal("rx2") _
                                                AndAlso oRect.ParamVal("ry1") <= nLine _
                                                AndAlso nLine <= oRect.ParamVal("ry2") Then
                                                sRectFound = nRect.ToString
                                            End If
                                        Next
                                        If sRectFound = "" Then
                                            sLine = nLine.ToString
                                        Else
                                            sLine = sRectFound & "_" & nLine.ToString
                                        End If
                                        'vk 02.24

                                        'ntg 10.07.24 vladi's change regarding city-street screen
                                        If bp.getProperty("CityStreet") = "inline" AndAlso oNode.ParamIn("apr", "NT", 2) AndAlso oNodeScreen.Param("fgr") <> "P" Then
                                            Dim sN As String = "F" & oNode.LongName & oNodeScreen.Param("fdate")
                                            bp.m_sPrebuilt &= qMulti.Hidden(sN, oNode.Param("val"))
                                            bp.m_sPrebuilt &= qMulti.Hidden(oNode.Param("apr"), sN)

                                        ElseIf cDdsLines.Contains(sLine) Then
                                            oDdsLine = cDdsLines(sLine)
                                            AddField(oDdsLine, oNode, IsModalWindow)
                                        ElseIf oNode.Param("ewr") <> "E" Then
                                            oDdsLine = New DdsLine
                                            AddField(oDdsLine, oNode, IsModalWindow)
                                            cDdsLines.Add(oDdsLine, sLine)
                                        End If
                                        'rem vk 02.24
                                        'If cDdsLines.Contains(sLine) Then
                                        '    oDdsLine = cDdsLines(sLine)
                                        '    If Not oDdsLine.cNodes.Contains(nColumn.ToString) Then
                                        '        oDdsLine.cNodes.Add(oNode, nColumn.ToString)
                                        '        'oDdsLine.ColList.Add(nColumn)
                                        '    Else
                                        '        Dim NewColumn As Integer = nColumn
                                        '        If bEnglish Then
                                        '            While oDdsLine.cNodes.Contains(NewColumn.ToString)
                                        '                NewColumn = NewColumn + 1
                                        '            End While
                                        '            oDdsLine.cNodes.Add(oNode, NewColumn.ToString)
                                        '            ' oDdsLine.ColList.Add(NewColumn)
                                        '        Else
                                        '            While oDdsLine.cNodes.Contains(NewColumn.ToString)
                                        '                NewColumn = NewColumn - 1
                                        '            End While
                                        '            oDdsLine.cNodes.Add(oNode, NewColumn.ToString)
                                        '            '  oDdsLine.ColList.Add(NewColumn)
                                        '        End If
                                        '    End If
                                        'Else
                                        '    If oNode.Param("ewr") <> "E" Then
                                        '        oDdsLine = New DdsLine
                                        '        oDdsLine.cNodes.Add(oNode, nColumn.ToString)
                                        '        cDdsLines.Add(oDdsLine, sLine)
                                        '    End If
                                        'End If
                                End Select
                            End If
                        End If
                End Select
            End If
        Loop

        'vk 09.24
        bp.m_bStatementWindow = True
        If cDdsLines.Count <= 1 OrElse oNodeScreen.ParamIn("win", "N ") Then
            bp.m_bStatementWindow = False
        Else
            Dim sCol As String = ""
            For Each oDdsLine As DdsLine In cDdsLines
                If oDdsLine.cNodes.Count > 1 Then
                    bp.m_bStatementWindow = False
                    Exit For
                End If
                Dim oNode As Node = oDdsLine.cNodes(1)
                If Not (oNode.ParamIn("inp", "PQ") OrElse oNode.ParamIn("orig_inp", "PQ")) Then
                    bp.m_bStatementWindow = False
                    Exit For
                End If
                If sCol = "" Then
                    sCol = oNode.Param("col")
                ElseIf sCol <> oNode.Param("col") Then
                    bp.m_bStatementWindow = False
                    Exit For
                End If
            Next
        End If

        If bp.m_nCall = 1 AndAlso oNodeScreen.ParamIn("win", "TC") AndAlso bp.m_sTextPrl <> "Y" AndAlso (bp.m_sTextPrl = "N" OrElse oNodeScreen.ParamVal("flr") = 1) Then
            Dim iIndex As Integer

            For iIndex = 1 To oNodeScreen.ParamVal("tmx")
                bp.sListOfLtr += "W!00000" + iIndex.ToString("0000") + iIndex.ToString("0000") + "A" + bp.m_nTextLen.ToString("00000") + "00;"
            Next
        End If

        For nRect = 0 To nMaxRect
            For nLine = 1 To nMaxLine
                If nRect = 0 Then
                    sLine = nLine.ToString
                Else
                    sLine = nRect.ToString & "_" & nLine.ToString
                End If
                If cDdsLines.Contains(sLine) Then
                    Dim sChain As String = ""
                    Dim bAllLabels As Boolean = True
                    oDdsLine = cDdsLines(sLine)

                    ' Dim MinimumCol As Integer = oDdsLine.ColList.Min()

                    ' Dim MaximumCol As Integer = oDdsLine.ColList.Max()

                    oNode = Nothing
                    ' For nColumn = MinimumCol To MaximumCol 'oDdsLine.nMaxColumn
                    For nColumn = 1 To nScreenWid 'oDdsLine.nMaxColumn ---- this loop determines the pattern in the current line
                        If oDdsLine.cNodes.Contains(nColumn.ToString) Then
                            oNode = oDdsLine.cNodes(nColumn.ToString)
                            'If oNode.Param("ewr") = "H" Then
                            '    'vk 02.24
                            '    oDdsLine.bHasEwrH = True
                            '    oDdsLine.sPattern &= "L"
                            'End If
                            'If oNode.ForBoo("label", winVal:=oNodeScreen.Param("win"), fdspVal:=oNodeScreen.ParamVal("fdsp")) Then
                            If oNode.ForBoo("label", winVal:=oNodeScreen.Param("win"), fdspVal:=oNodeScreen.ParamVal("fdsp"), filVal:=oNodeScreen.Param("fil")) Then 'vk 05.08.24 for cities and streets screen
                                oDdsLine.sPattern &= "L"
                                oDdsLine.LabelsList.Add(oNode)
                            Else
                                oDdsLine.sPattern &= "F"
                                If oNode.ParamExists("inp") Then
                                    If oNode.Param("inp") <> "O" Then
                                        bAllLabels = False
                                    End If
                                End If
                            End If
                            If oNode.ParamExists("val") Then
                                If bEnglish Then
                                    If sChain > "" Then
                                        sChain = sChain & " "
                                    End If
                                    sChain &= oNode.Param("val")
                                Else
                                    If sChain > "" Then
                                        sChain = " " & sChain
                                    End If
                                    sChain = Heb(oNode.Param("val")) & sChain
                                End If
                            End If
                        End If
                    Next
                    If oNodeScreen.Param("fil").Trim = "HFFLNAVFM" AndAlso oNodeScreen.Param("rec").Trim = "RLISTDDS" AndAlso oNode.Param("num").Contains("HDRNAM") Then
                        sPageTitle = Heb(oNode.Param("val")) 'ntg 19.06.23 identifing a case the last hard line is HDRNAM and should be a title
                        cDdsLines.Remove(sLine)
                    Else
                        Select Case oDdsLine.sPattern
                            Case "F"
                                If bp.IsMsgFile(oNodeScreen.Param("fil")) Then
                                    sPageTitle = Heb(oNode.Param("val"))
                                    cDdsLines.Remove(sLine)
                                End If
                            Case "L"
                                'If oNode.ParamVal("pfn") <> 8 Then '?
                                Dim bRemove As Boolean = False
                                If nRect > 0 Then
                                    Dim oRect As Node = cRectangles(nRect.ToString)
                                    If oRect.Param("ftit") = "" AndAlso oNode.Param("ewr") <> "H" AndAlso oNode.Param("ewr") <> "D" Then ' If the line is a table header, it shouldn't be card header 'vk 03.24+ ntg 10.03.24 changes for special header split with '|'
                                        oRect.Param("ftit") = Heb(oNode.Param("val"))
                                        If oNode.ParamExists("pic") Then oRect.Param("pic") = oNode.Param("pic")
                                        If oNode.ParamExists("pcl") Then oRect.Param("pcl") = oNode.Param("pcl")
                                        bRemove = True
                                    End If
                                End If
                                'If oNodeScreen.Param("fil").Trim = "HFFLNAVFM" AndAlso oNodeScreen.Param("rec").Trim = "RLISTDDS" AndAlso oNode.Param("num").Contains("HDRNAM") Then
                                '    sPageTitle = Heb(oNode.Param("val")) 'ntg 13.06.23 identifing a case the last hard line is HDRNAM and should be a title
                                '    cDdsLines.Remove(sLine)
                                If bRemove Then
                                    cDdsLines.Remove(sLine)
                                ElseIf bp.IsMsgFile(oNodeScreen.Param("fil")) Then
                                    'ignore
                                ElseIf nLine = nMaxLine_Hard Then
                                    If Left(oNode.Param("num"), 3) = "$00" Then 'ntg 12.06.23 identifing a case the last label becomes a header
                                        If oNode.Param("ewr") <> "E" Then ' If this is an error message,dont show it and open an error pop up
                                            sPageTitle = Heb(oNode.Param("val"))
                                        End If
                                        cDdsLines.Remove(sLine)
                                    End If 'ntg 12.06.23 identifing a case the last label becomes a header

                                ElseIf nRect = 0 Then
                                    If Not (yT.Contains(nLine) OrElse yS.Contains(nLine) OrElse yL.Contains(nLine)) Then ' להוסיף בדיקה שהשורה לא נמצאת כחלק משורות הגריד
                                        Select Case oNode.Param("pul") 'ntg 17.06.24 vladi changes regarding sub-title and accordion in card
                                            Case "A", "S" 'ntg 17.06.24 vladi changes regarding sub-title and accordion in card
                                            Case Else
                                                If nLastCardUsed = nCard Then
                                                    nCard += 1
                                                End If
                                                cCardTitles.Add(Heb(oNode.Param("val")), nCard.ToString)
                                                CardTitlesNodes.Add(oNode, nCard.ToString)
                                                cDdsLines.Remove(sLine)
                                        End Select
                                    End If
                                End If
                            'End If
                            Case "FFL", "LFF", "LLL" ', "FLF"
                                If bAllLabels Then
                                    If nRect = 0 Then
                                        If Not (yT.Contains(nLine) OrElse yS.Contains(nLine) OrElse yL.Contains(nLine)) Then ' להוסיף בדיקה שהשורה לא נמצאת כחלק משורות הגריד
                                            If nLastCardUsed = nCard Then
                                                nCard += 1
                                            End If
                                            cCardTitles.Add(sChain, nCard.ToString)
                                            cDdsLines.Remove(sLine)
                                        End If
                                    Else
                                        Dim oRect As Node = cRectangles(nRect.ToString)
                                        If oRect.Param("ftit") = "" Then
                                            oRect.Param("ftit") = sChain
                                            cDdsLines.Remove(sLine)
                                        End If
                                    End If
                                End If
                        End Select
                    End If
                    oDdsLine.nCard = nCard
                    nLastCardUsed = nCard
                    'vk 02.24
                    'ntg 17.03.24 changes by vladi
                    'If oDdsLine.bHasEwrH AndAlso IgnoreGrid(IsModalWindow) AndAlso x.Count = 0 Then
                    '    For n As Integer = 1 To nScreenWid
                    '        If oDdsLine.cNodes.Contains(n.ToString) Then
                    '            Dim oNodeTmp As Node = oDdsLine.cNodes(n.ToString)
                    '            x0 = New FromTo
                    '            x0.Init(n.ToString("000"), (n + oNodeTmp.ParamVal("len") - 1).ToString("000"))
                    '            x.Add(x0, n.ToString)
                    '        End If
                    '    Next
                    'End If
                End If
            Next
            nMaxCard = nCard
        Next

        '-------------------------------------------------
        Dim NarrowSideBar As Boolean = False
        '-------------------------------------------------
        Dim oHtml As New StringBuilder
        For Each oNode In cFolders
            'vk 09.24 fgr=D is a screen with two tables
            If oNode.ParamExists("pch") AndAlso (oNodeScreen.ParamIn("fgr", "LBD")) Then 'ntg 23.06.24 fgr=B is the same as fgr=L, but with option to select multiple rows in a table

                If m_ModalScreen = "" Then 'only add footer buttons that are relevant to modal screen if this is the case
                    bp.ActionNodesIcons.Add(oNode.Param("pch"), oNode.Param("pic")) 'ntg 22.05.24 counting the number of pch that have icons (because we need to use this info before the original count)
                End If
            End If
        Next
        If IsModalWindow = False Then

            oHtml.Append("<!DOCTYPE html>").Append(vbCrLf)
            Dim sDir As String
            If bEnglish Then
                sDir = "ltr"
                oHtml.Append("<html lang=""en"" style=""direction:").Append(sDir).Append(";"">").Append(vbCrLf)
            Else
                sDir = "rtl"
                oHtml.Append("<html lang=""he"" style=""direction:").Append(sDir).Append(";"">").Append(vbCrLf)
            End If
            oHtml.Append("<head>").Append(vbCrLf)
            If bp.getProperty("AdditionalHtml_Header") > "" Then
                oHtml.Append(bp.getProperty("AdditionalHtml_Header")).Append(vbCrLf)
            End If

            oHtml.Append("<!-- Job: ").Append(bp.m_sJob).AppendLine(" -->")
            oHtml.Append("<!-- User: ").Append(bp.m_sUser).AppendLine(" -->")
            oHtml.Append("<!-- Session ID: ").Append(bp.m_sSession).AppendLine(" -->")
            oHtml.Append("<!-- Local counter for XML: ").Append(bp.XmlLocalCouner).AppendLine(" -->")
            oHtml.Append("<!-- Site: ").Append(bp.getProperty("Site")).AppendLine(" -->")
            oHtml.Append("<!-- Now: ").Append(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")).AppendLine(" -->")
            If Not oNodeScreen Is Nothing Then
                oHtml.Append("<!-- Database: ").Append(oNodeScreen.Param("flib").Trim()).AppendLine(" -->")
            End If

            oHtml.Append(qMulti.FromFile(bp.m_sMapPath + "\color\Boo0.htm"))
            oHtml.Append("<meta charset=""utf-8"" />").Append(vbCrLf)
            oHtml.Append("<meta name=""viewport"" content=""width=device-width, initial-scale=1, shrink-to-fit=no"" />").Append(vbCrLf)
            oHtml.Append("<link rel=""stylesheet"" href=""./assets/styles/font.css"" />").Append(vbCrLf)
            oHtml.Append("<link rel=""stylesheet"" href=""https://fonts.cdnfonts.com/css/ploni"" />").Append(vbCrLf)
            oHtml.Append("<link rel=""stylesheet"" href=""./assets/styles/fa-pro-6.1.2/fa-pro-6.1.2/css/all.css"" />").Append(vbCrLf)
            oHtml.Append("<link rel=""stylesheet"" href=""./assets/bootstrap5/css/bootstrap.css"" />").Append(vbCrLf)
            oHtml.Append("<link rel=""stylesheet"" href=""./assets/styles/custom.css"" />").Append(vbCrLf)
            oHtml.Append("<link rel=""stylesheet"" href=""./assets/styles/menurow.css"" />").Append(vbCrLf)
            oHtml.Append("<link rel=""stylesheet"" href=""./assets/styles/datepicker.css"" />").Append(vbCrLf)
            oHtml.AppendLine("<link rel=""stylesheet"" href=""./assets/styles/bootstrap4-toggle.min.css"" />")
            oHtml.AppendLine("<link rel=""stylesheet"" href=""./assets/styles/list-groups.css"" />")
            oHtml.AppendLine("<link rel=""stylesheet"" href=""./assets/styles/floating-labels.css"" />")

            oHtml.AppendLine("<link rel=""stylesheet"" href=""../css/inputSliderRange.css"" />")

            oHtml.Append("<link rel=""stylesheet"" href=""./css/TDI.css"" />").Append(vbCrLf)

            If bp.getProperty("ColorField") <> "" Then
                oHtml.AppendLine($"<link rel=""stylesheet"" href={bp.getProperty("ColorField")} />").Append(vbCrLf)
            End If
            If bp.getProperty("CssFile") <> "" Then
                oHtml.AppendLine($"<link rel=""stylesheet"" href=""./assets/styles/{bp.getProperty("CssFile")}"" />").Append(vbCrLf)
            End If
            oHtml.Append("<link rel=""stylesheet"" href=""./css/sortable.css"" />").Append(vbCrLf)

            oHtml.Append("    <link rel=""SHORTCUT ICON"" href=""./pics/").Append(bp.getProperty("Icon")).Append(""" />").Append(vbCrLf)
            oHtml.Append("    <script nomodule src=""./assets/scripts/duet/duet.js""></script>").Append(vbCrLf)
            oHtml.Append("    <script type=""module"" src=""./assets/scripts/duet/duet.esm.js""></script>").Append(vbCrLf)

            oHtml.Append("  <script src=""./chartjs/Chart.min.js""></script>").Append(vbCrLf)
            oHtml.Append("  <script src=""./chartjs/chartjs-plugin-datalabels.min.js""></script>").Append(vbCrLf)
            oHtml.Append("  <link rel=""stylesheet"" href=""./chartjs/Chart.min.css"" />").Append(vbCrLf)


            'oHtml.Append("    <script src=""https").Append("://cdn.jsdelivr.net/npm/chart.js@2.8.0""></script>").Append(vbCrLf) 'vk 06.21
            'oHtml.Append("    <script src=""https").Append("://cdn.jsdelivr.net/npm/chartjs-plugin-datalabels@0.7.0/dist/chartjs-plugin-datalabels.min.js""></script>").Append(vbCrLf) 'vk 06.21


            'oHtml.Append("    <script src=""https").Append("://cdn.jsdelivr.net/npm/chart.js""></script>").Append(vbCrLf) 'vk 05.21
            '/oHtml.Append("    <script src=""https").Append("://cdn.jsdelivr.net/npm/chart.js@2.8.0""></script>").Append(vbCrLf) 'vk 06.21
            '/oHtml.Append("    <script type=""module"" src=""./chartjs/Chart.js""></script>").Append(vbCrLf)
            'oHtml.Append("    <script type=""module"" src=""./chartjs/Chart.min.js""></script>").Append(vbCrLf)
            'oHtml.Append("    <script type=""module"" src=""./chartjs/chartjs-plugin-datalabels.min.js""></script>").Append(vbCrLf)
            '/oHtml.Append("    <script src=""https").Append("://cdn.jsdelivr.net/npm/chartjs-plugin-datalabels@0.7.0/dist/chartjs-plugin-datalabels.min.js""></script>").Append(vbCrLf) 'vk 06.21
            oHtml.Append("    <title>").Append(sTitle).Append("</title>").Append(vbCrLf)







            'trying to use old DDS things

            If bEnglish Then
                oHtml.Append(qMulti.FromFile(bp.m_sMapPath + "\color\BooEng.htm"))
                oHtml.Append("<style>.form-label, .form-control{left: 0.5rem;}</style>").Append(vbCrLf) 'ntg 05.02.23
            Else
                oHtml.Append(qMulti.FromFile(bp.m_sMapPath + "\color\Boo1.htm"))
            End If
            oHtml.Append("</head>").Append(vbCrLf)



            oHtml.Append("<body class='dark' style=""direction:").Append(sDir).Append(";"" onkeydown=""if(NotReady_Key('keydown'))return;fnWindowKeyDown('',false,'',event.keyCode,true);"" onbeforeunload=""if(NotReady('beforeunload'))return;CloseMsg();"" onload=""InitConst(10,4,1,5,2,1);"">").Append(vbCrLf)
            oHtml.Append("<main class='py-0'><form method='post' action='screen.aspx'  autocomplete='off' >").Append(vbCrLf) 'ntg 01.11.23 added autocomplete='off' because it saved old data 
            If bp.getProperty("AdditionalHtml_Form") > "" Then 'AndAlso Not bPdf Then
                oHtml.Append(bp.getProperty("AdditionalHtml_Form")).Append(vbCrLf) 'vk 02.20
            End If

            If m_ModalScreen = "" Then
                qOnce.FormHiddens(bp, oHtml, oNodeScreen, sScreenNum, sLastEntry, sWarning, sCommand, bFirstScreen, m_ModalScreen, IsModalWindow) 'vk 02.23
                If oNodeScreen.Param("fgr") = "T" Then
                    oHtml.Append("<label Class=""btn btn-primary btn-sm"" name=""lblSqlMsg"" id=""lblSqlMsg""></label>").Append(vbCrLf) 'vk 08.22
                End If
            End If

            oHtml.Append(qMulti.FromFile(bp.m_sMapPath + "\color\Boo2.htm"))
            oHtml.Append(sOutH.ToString()).Append(vbCrLf)

            '-------------------------------------------------


            '-------------------------------------------------

            'If m_ModalScreen <> "" Then ' יצירת חלון מודלי 'vk 02.23 ----------> עדיין קיים, הועבר למטה יותר בקוד
            '    'oHtml.AppendLine(GetModalWindowString(bp.m_ModalScreen))
            '    oHtml.AppendLine(GetModalWindowString(m_ModalScreen, bp, sXml)) 'vk 02.23

            'End If

            oHtml.Append("<div class=""page"">").Append(vbCrLf)
            oHtml.Append("<div class=""flex-fill"">").Append(vbCrLf)
            oHtml.Append("<!-- header info -->").Append(vbCrLf)

            Dim numFoldersNoPch As Integer = 0 'ntg 09.04.24 changing the condition to fgr screens to include more screens
            For Each oNode In cFolders
                If Not oNode.ParamExists("pch") Then
                    numFoldersNoPch += 1
                End If
            Next

            Select Case True
                Case cFolders.Count = 0
                    NarrowSideBar = True
                Case oNodeScreen.Param("fgr") Is Nothing 'vk 09.24 this case added to exclude this check from other cases
                    NarrowSideBar = False
                'Case oNodeScreen.Param("fgr") = "M"
                '    NarrowSideBar = True
                Case oNodeScreen.ParamIn("fgr", "LD") AndAlso numFoldersNoPch = 0 'ntg 09.04.24 changing the condition to fgr screens to include more screens
                    'vk 09.24 fgr=D
                    NarrowSideBar = True
                Case oNodeScreen.Param("fgr") = "B" AndAlso numFoldersNoPch = 0 'ntg 23.06.24 fgr=B is the same as fgr=L, but with option to select multiple rows in a table
                    NarrowSideBar = True
                'Case oNodeScreen.Param("fgr") = "P" AndAlso numFoldersNoPch = 0 'ntg 20.05.24 added fgr=P
                '    NarrowSideBar = True
                Case numFoldersNoPch <> 0 'ntg 09.04.24 changing the condition to fgr screens to include more screens 'ntg 26.05.24 all types of fgr that have folders wont be with narrow sidebar
                    NarrowSideBar = False
                Case Else
                    NarrowSideBar = False
            End Select

            'NarrowSideBar = cFolders.Count = 0 OrElse
            '                            (oNodeScreen.Param("fgr") = "M" OrElse
            '                            (oNodeScreen.ParamIn("fgr","LD") AndAlso Not (oNodeScreen.Param("fil").Trim() = "HFFLNAVFM" AndAlso oNodeScreen.Param("rec").Trim() = "WINLSTGM") _
            '                                                            AndAlso Not (oNodeScreen.Param("fil").Trim() = "HFFLNAVFM" AndAlso oNodeScreen.Param("rec").Trim() = "TKWINGM") _
            '                                                            AndAlso Not (oNodeScreen.Param("fil").Trim() = "HFSCREEN" AndAlso oNodeScreen.Param("rec").Trim() = "TVIA1FN") _
            '                                                            AndAlso Not (oNodeScreen.Param("fil").Trim() = "HFSCREEN" AndAlso oNodeScreen.Param("rec").Trim() = "PESCMFN") _
            '                                                            AndAlso Not (oNodeScreen.Param("win").Trim() = "R") _
            '                                                            AndAlso Not (oNodeScreen.Param("fil").Trim() = "HFSCREEN" AndAlso oNodeScreen.Param("rec").Trim() = "PEMISHFS")))



            oHtml.AppendLine($"<div class='header {If(NarrowSideBar, "WideHeader", "ShortHeader")}'>")
            oHtml.Append("<div class='container-fluid HeaderLine'>").Append(vbCrLf)
            oHtml.AppendLine("<div class='d-flex align-items-center'>")

            Dim nScreenWid1 As Integer = nScreenWid
            If oNodeScreen.ParamYes("dspa") Then
                nScreenWid1 = 80
            End If

            oHtml.AppendLine("<nav id='NavBurger' class='d-lg-none navbar navbar-expand-lg navbar-light bg-light'>
                             <button class='navbar-toggler' type='button' data-bs-toggle='offcanvas' data-bs-target='#offcanvas' aria-controls='navbarSupportedContent' aria-expanded='false' aria-label='Toggle navigation' onclick='toggleFooterButtons();'>
                                    <i class='fa-regular fa-bars'></i> 
                             </button>
                          </nav>")

            Dim policyMode As String
            Dim iconForMode As String = IconForModeString(screenActionMode, bp)

            If (screenFilName <> "") AndAlso screenActionMode <> "" Then
                policyMode = $"<Div id='StatusOfPolicy' class ='text-center d-flex flex-column'>
                                        <span id='StatusOfPolicyName'>{screenActionMode}</span>
                                    </Div>
                                    {iconForMode}"
            Else
                policyMode = ""
            End If

            Dim sHeaderUserIconStyleFA As String = bp.getProperty("HeaderUserIconStyleFA", "fa-2x mx-2 fas fa-user-circle")

            'If PageHeaderHtml = "" Then 'ntg 13.02.24 we need the pageheader to always update in case there's a policy mode
            PageHeaderHtml = $"<Div id='HeaderUserFullInfo' class='d-none d-lg-flex align-items-center d-flex'>
                                    {policyMode}
                                    <Div id='HeaderUserInfo' class ='text-center d-flex flex-column'>
                                        <span id='HeaderUserName'>{HeaderUserName}</span>
                                        <span id='HeaderPageDate'>{HeaderPageDate}</span>
                                    </Div>
                                    <i id='HeaderUserIcon' class='{sHeaderUserIconStyleFA}' data-bs-toggle='tooltip' data-bs-placement='bottom' title='{HeaderMasof}'></i>
                               </Div>
                               <span class='mx-auto badge badgeScreenName bg-info'>$$ScreenName$$</span>"
            'End If

            oHtml.AppendLine(PageHeaderHtml.Replace("$$ScreenName$$", If(ScreenName IsNot Nothing, ScreenName.Replace(" ", "&nbsp;"), "")))

            oHtml.AppendLine($"<div id='CompanyNameWithLogo' class='align-items-center d-flex'>
                             <img class='header-brand-img' src='pics/{If(bp.getProperty("ExtraLogo") <> "", bp.getProperty("ExtraLogo"), bp.getProperty("Logo"))}' alt='logo' loading=""lazy"" />
                             <span id='HeaderCompanyName' class='d-none d-lg-block px-2 mx-2'>{HeaderCompanyName}</span>
                        </div>")

            'For Each oNode In cFirstLineNodes
            '    If oNode.ParamIn("typ", "DYHQ") Then
            '        If Not oNode.ParamIn("inp", "IKR") Then
            '            oNode.Param("val") = qConv.ConvDate(bp, oNode.Param("val"), "", "AS2PC", oNode.Param("typ"))
            '        End If
            '    End If
            '    Dim sText As String = oNode.Param("val")
            '    Dim BlockDisplayClass = "d-lg-inline d-none"
            '    If oNode.ForBoo("middle", nScreenWid1) AndAlso (bExist_Title OrElse bExist_Teur) Then
            '        sText = oNodeMiddle.Param("val")
            '        BlockDisplayClass = "d-inline" ' show this "pill" only on mobile
            '    End If
            '    oHtml.AppendLine($"<span class='badge bg-info {BlockDisplayClass}' style='margin:auto;'  > {Heb(sText)} </span>")
            'Next
            '<span class='navbar-toggler-icon'></span>
            'oHtml.Append("<div class=""order-lg-2 mr-auto col-12 col-lg-7 pt-3 pt-lg-0 pb-3 pb-lg-0 d-lg-flex justify-content-end align-items-center"">").Append(vbCrLf)
            'Dim nScreenWid1 As Integer = nScreenWid
            'If oNodeScreen.ParamYes("dspa") Then
            '    nScreenWid1 = 80
            'End If
            'oHtml.Append("</div>").Append(vbCrLf)

            oHtml.AppendLine("</div>")
            oHtml.AppendLine("</div>")
            oHtml.AppendLine("</div>")

            ' Header Menu

            oHtml.Append("<!-- header menu  -->").Append(vbCrLf)

            oHtml.AppendLine("<div class='container-fluid'>")
            oHtml.AppendLine("<div id='MainRow' class='row'>")

            oHtml.AppendLine($"<div class='{If(NarrowSideBar, "NarrowSideBar", "")} SideMenu px-0 '  tabindex='-1' id='offcanvas'>")
            oHtml.AppendLine("<div class='d-flex flex-column sideBarArea'>") 'ntg 17.04.24 top header and sidebar fixation

            oHtml.AppendLine($"<div class='offcanvas-header d-lg-none'>
                                    <Div id='HeaderUserFullInfo' class='d-flex align-items-center'>
                                        <Div id='HeaderUserInfo' class ='text-center d-flex flex-column'>
                                            <span id='HeaderUserName'>{HeaderUserName}</span>
                                            <span id='HeaderPageDate'>{HeaderPageDate}</span>
                                        </Div>
                                        <i id='HeaderUserIcon' class='{sHeaderUserIconStyleFA}'></i>
                                        <span id='HeaderSessionNumber'>{HeaderMasof}</span>
                                    </Div>
			                        <button id='CanvasCloseButton' type='button' class='mx-auto btn-lg btn-close' data-bs-dismiss='offcanvas' aria-label='Close'></button>
	                      </div>")
            oHtml.AppendLine($"<div class='offcanvas-body  flex-column px-0'>") 'd-flex

            'Dim reverseSideBar As Boolean = bp.getProperty("reverseSideBar") <> "" 'ntg 12.09.23 adding the option to change the order of sidebar sections --ntg 30.04.24 all reverseSideBar changes are cancelled
            oHtml.AppendLine("<div>") 'ntg 12.09.23 a div to hold all sidebar sections except for topButtonDiv (so it wont be influenced by the column-reverse) 
            'oHtml.AppendLine($"<div {If(reverseSideBar, "style=""flex-grow:0; display:flex;flex-direction:column-reverse !important""", "")}>") 'ntg 12.09.23 a div to create a section to do a "column-reverse" on --ntg 30.04.24 all reverseSideBar changes are cancelled
            oHtml.AppendLine($"<div>") 'ntg 12.09.23 a div to create a section to do a "column-reverse" on --ntg 30.04.24 all reverseSideBar changes are cancelled
            oHtml.AppendLine("<ul Class='w-100 nav nav-pills flex-column sidebar'>")

            'For Each oNode In cFolders
            '    If oNode.ParamExists("pch") AndAlso oNodeScreen.ParamIn("fgr","LPD") Then 'ntg 20.05.24 changes in appearence regarding screens with fgr=P-showing button instead of list of actions

            '        If m_ModalScreen = "" Then 'only add footer buttons that are relevant to modal screen if this is the case
            '            bp.ActionNodesIcons.Add(oNode.Param("pch"), oNode.Param("pic"))
            '        End If
            '    End If
            'Next

            For Each oNode In cFolders
                'If (oNode.ParamExists("pch") AndAlso oNodeScreen.Param("fgr") = "L") Then ' גריד עם כפתורי פעולה
                If oNode.ParamExists("pch") AndAlso oNodeScreen.ParamIn("fgr", "LBD") Then 'ntg 23.06.24 fgr=B is the same as fgr=L, but with option to select multiple rows in a table
                    'vk 09.24 fgr=D
                    If m_ModalScreen = "" Then 'only add footer buttons that are relevant to modal screen if this is the case
                        bp.ActionNodes.Add(OneButton(bp, oNode, "c"))
                    End If

                Else
                    Dim IsCheckBox As Boolean = oNode.ParamExists("chb")

                    oHtml.AppendLine($"<li {If(bEnglish, "style=""text-align: left""", "")} class='my-2 nav-item {If(IsCheckBox, "d-none", "")} {If(oNode.ParamYes("ltr"), "active", "")}'>")

                    If IsCheckBox Then
                        If bp.IsFoldersScreen(oNodeScreen) Then 'טיפול במסך בחירת טיפול בפוליסה (להעביר ל-SIDEBAR)
                            oHtml.AppendLine(qMulti.Hidden(oNode.Param("HiddentFieldId")))
                        Else
                            oHtml.AppendLine(OneField(bp, oNode))
                        End If
                    Else
                        oHtml.Append(OneButton(bp, oNode, "l"))
                    End If
                    oHtml.AppendLine("</li>")
                End If
            Next

            For Each oNode In SpecialFields
                oHtml.AppendLine("<!-- special -->")
                oHtml.Append(OneField(bp, oNode))
            Next

            oHtml.AppendLine("</ul>")
            'oHtml.AppendLine($"<div class='sidebarBtns' style='{If(NarrowSideBar, "", "margin-right: 2.3rem;")}'>")
            oHtml.AppendLine($"<div class='sidebarBtns {If(NarrowSideBar, "", "wideSideBarBtns")}'>")
            oHtml.AppendLine(FixedButtons(bp, NarrowSideBar))
            oHtml.AppendLine(AddDevToolsDiv(bp, NarrowSideBar))
            If Not IsModalWindow Then

                Dim langOfComtecLtd = $"{If(bEnglish, "Comtec Ltd", "קומטק בע&quot;מ ")}"
                Dim langOfAllRightsRes = $"{If(bEnglish, "All Rights Reserved", "כל הזכויות שמורות")}"
                Dim FontfCopyRight = "<FONT COLOR=""#FF4500"">&copy;</FONT>"
                'oHtml.AppendLine($"<div id='CopyRightDiv' style='left: {If(bEnglish, "108rem", "1rem")}'>")  'ntg 10.1.23
                oHtml.AppendLine($"<div id='CopyRightDiv'>")  'ntg 10.1.23

                'oHtml.AppendLine($"<a href=""http://www.comtecglobal.com"" target=""_blank"" tabindex=""-1"">{langOfComtecLtd} {FontfCopyRight} {langOfAllRightsRes}</a>")  'ntg 01.02.23
                oHtml.AppendLine($"<a class= 'CopyRightLink' href=""http://www.comtecglobal.com"" target=""_blank"" tabindex=""-1"">{FontfCopyRight}Comtec Ltd</a>")  'ntg 01.02.23

                'oHtml.AppendLine(bp.getProperty("Copyright")).Append(vbCrLf)  'ntg 10.1.23
                oHtml.AppendLine("</div>")  'ntg 10.1.23
            End If
            oHtml.AppendLine("<div id='topButtonDiv'>
                            <button id='GoToTopBtn' type='button' class ='btn blink' onclick='topFunction()' title='Go to top'>
                                <i id='UpIcon' class='fa-regular fa-circle-arrow-up'></i>
                            </button>
                          </div>")
            oHtml.AppendLine("</div>")

            oHtml.AppendLine("</div>") 'ntg 12.09.23 end of the div that created a section to do a "column-reverse" on
            oHtml.AppendLine("</div> ") 'ntg 12.09.23 end of the div to hold all sidebar sections except for topButtonDiv (so it wont be influenced by the column-reverse)


            '-------------------------------------------------
            'If m_ModalScreen <> "" Then ' יצירת חלון מודלי 'vk 02.23
            '    'oHtml.AppendLine(GetModalWindowString(bp.m_ModalScreen))
            '    oHtml.AppendLine(GetModalWindowString(m_ModalScreen, bp)) 'vk 02.23

            'End If
            '-------------------------------------------------

            ' <img class='UpArrow p-1' src='./assets/images/up-arrow.svg'>
            'oHtml.AppendLine("<div id='topButtonDiv'>
            '                <button id='GoToTopBtn' type='button' class ='btn' onclick='topFunction()' title='Go to top'>
            '                    <i id='UpIcon' class='fa-regular fa-circle-arrow-up'></i>
            '                </button>
            '              </div>")

            'ntg 23.01.24 moved all the section of the copyrights to the sidebar
            'If Not IsModalWindow Then

            '    Dim langOfComtecLtd = $"{If(bEnglish, "Comtec Ltd", "קומטק בע&quot;מ ")}"
            '    Dim langOfAllRightsRes = $"{If(bEnglish, "All Rights Reserved", "כל הזכויות שמורות")}"
            '    Dim FontfCopyRight = "<FONT COLOR=""#FF4500"">&copy;</FONT>"
            '    'oHtml.AppendLine($"<div id='CopyRightDiv' style='left: {If(bEnglish, "108rem", "1rem")}'>")  'ntg 10.1.23
            '    oHtml.AppendLine($"<div id='CopyRightDiv'>")  'ntg 10.1.23

            '    'oHtml.AppendLine($"<a href=""http://www.comtecglobal.com"" target=""_blank"" tabindex=""-1"">{langOfComtecLtd} {FontfCopyRight} {langOfAllRightsRes}</a>")  'ntg 01.02.23
            '    oHtml.AppendLine($"<a class= 'CopyRightLink' href=""http://www.comtecglobal.com"" target=""_blank"" tabindex=""-1"">{FontfCopyRight}Comtec Ltd</a>")  'ntg 01.02.23

            '    'oHtml.AppendLine(bp.getProperty("Copyright")).Append(vbCrLf)  'ntg 10.1.23
            '    oHtml.AppendLine("</div>")  'ntg 10.1.23
            'End If
            oHtml.AppendLine("</div>") ' End of offcanvas-body

            oHtml.AppendLine("</div>") ' End of d-flex flex-column

            If sideBarfields.Count <> 0 Then 'ntg 16.07.24 changes regarding sidebar fields
                oHtml.AppendLine("<div class='fieldsSideBarSection'>")
                For Each kvp As KeyValuePair(Of String, String) In sideBarfields
                    oHtml.AppendLine($"<div class='oneSideBarField'><label>{kvp.Key}</label><label>{kvp.Value}</label></label> </div>")
                Next
                oHtml.AppendLine("</div>")
                oHtml.AppendLine("<label class='fieldsSideBarSectionHeader'>סיכום</label>")
            End If

            oHtml.AppendLine("</div>") 'End of col-auto

            'oHtml.AppendLine("</nav>")
            '------------------------------------------------- ntg 21.05.24 moved here because it was inside the sideBar code
            If m_ModalScreen <> "" Then ' יצירת חלון מודלי 'vk 02.23
                'oHtml.AppendLine(GetModalWindowString(bp.m_ModalScreen))
                oHtml.AppendLine(GetModalWindowString(m_ModalScreen, bp)) 'vk 02.23
                bp.m_bStatementWindow = False 'vk 09.24
            End If
            '-------------------------------------------------

        Else
            If oNodeScreen.Param("fgr") = "T" Then 'ntg 05.06.23 was missing in some of the screens
                oHtml.Append("<label Class=""btn btn-primary btn-sm"" name=""lblSqlMsg"" id=""lblSqlMsg"" style=""display: none;margin-right: 2rem;""></label>").Append(vbCrLf) 'ntg 19.06.23 added margin to space a little

            End If
            For Each oNode In cFolders 'vk 05.23
                'If oNode.ParamExists("pch") AndAlso oNodeScreen.Param("fgr") = "L" Then ' גריד עם כפתורי פעולה
                If oNode.ParamExists("pch") AndAlso oNodeScreen.ParamIn("fgr", "LBD") Then 'ntg 23.06.24 fgr=B is the same as fgr=L, but with option to select multiple rows in a table
                    'vk 09.24 fgr=D
                    If m_ModalScreen = "" Then 'only add footer buttons that are relevant to modal screen if this is the case
                        bp.ActionNodes.Add(OneButton(bp, oNode, "c"))
                    End If

                    'Else
                    '    If m_ModalScreen = "" Then 'only add footer buttons that are relevant to modal screen if this is the case
                    '        bp.ActionNodes.Add(OneButton(bp, oNode, "c"))
                    '    End If
                End If
            Next

            For Each oNode In SpecialFields
                oHtml.Append(OneField(bp, oNode))
            Next

        End If '// end of If IsModalWindow = False Then

        '-------------------------------------------------

        If oNodeScreen.Param("fil").Trim = bp.MenuFile AndAlso bp.cMenuLines.Count > 0 Then
            BooMenu(oHtml, bp)
        Else
            BooUsual(oHtml, bp, NarrowSideBar, IsModalWindow)
        End If


        '-------------------------------------------------
        If IsModalWindow = False Then

            oHtml.AppendLine("</div>") ' End of row 


            If bp.EwrErrorText <> "" Then

                oHtml.AppendLine($"{bp.EwrErrorText}") 'ntg 16.1.23
            End If


            oHtml.AppendLine("</div>") ' End of container-fluid

            oHtml.AppendLine("</div>")

            If m_ModalScreen = "" Then
                oHtml.AppendLine("</form>")
            End If


            oHtml.AppendLine("</main>")

            If IsToArchive Then
                qMulti.BuildCmd(bp, sOut, sOutH, "archive_browse", New Node(), oNodeScreen, oNodeScreen, bp.m_sArchiveDescr = "", False, False)
                qMulti.BuildCmd(bp, sOut, sOutH, "archive_submit", New Node(), oNodeScreen, oNodeScreen, True, False, False)
            End If

            If bp.m_sPdf <> "" Then
                qMulti.BuildCmd(bp, sOut, sOutH, "printpdf", New Node(), oNodeScreen, oNodeScreen, False, False, False)
            End If

            If oNodeScreen.ParamVal("jacket") > 0 Then
                qMulti.BuildCmd(bp, sOut, sOutH, "jacket", New Node(), oNodeScreen, oNodeScreen, False, False, False)
            End If



            oHtml.Append("<!-- footer -->").Append(vbCrLf)
            oHtml.AppendLine($"<footer class='footer {If(bp.getProperty("FooterLocation") = "top", "fixed-top", "fixed-bottom")} py-2'>")
            oHtml.AppendLine($"<div class='mx-auto FooterDiv {If(NarrowSideBar, "WideFooter", "col-auto col-md-9 col-xl-10")} '>")
            '


            '
            oHtml.AppendLine("<div class='mx-1 row footerRow align-items-center justify-content-center'>")
            '-----------------err in footer
            'If bp.EwrErrorText <> "" Then

            '    oHtml.AppendLine($"{bp.EwrErrorText}") 'ntg 16.1.23
            'End If
            '-----------------err in footer
            oHtml.AppendLine($"<div id='FooterButtonsDiv' class='d-flex align-items-center' {If(bp.getProperty("FooterLocation") = "top", "style='top: 5rem;'", "")}>")

            bp.m_sCurClass = "footerButton px-0 btn btn-sm"

            Dim ConstGroup = "X"

            Dim sGroupHtml = ""
            Dim GroupText As String = ""
            Dim GroupInModal As List(Of String) = New List(Of String)

            Dim FilteredFooterButtons As List(Of Button) = New List(Of Button)
            Dim FilteredFooterButtonsWithImg As List(Of Button) = New List(Of Button)
            Dim FilteredFooterButtonsNoImg As List(Of Button) = New List(Of Button)

            Dim FilteredFooterButtonsOrdered As List(Of Button) = New List(Of Button)
            Dim CountInButtons As Integer
            Dim UseMoreButton As Boolean
            '


            For Each oButton As Button In bp.cButtons
                Dim FunctionKey As Integer = oButton.oNode.ParamVal("fk")
                If FunctionKey = "90" Then
                    bIsPgDnBtn = True
                End If
                'Dim PicNumber = If(oButton.oNode.ParamExists("icn"), oButton.oNode.ParamVal("icn"), If(oButton.oNode.ParamExists("pic"), oButton.oNode.ParamVal("pic"), 0))
                Dim PicNumber = If(bp.getProperty("RemoveFooterIcons") = "1", 0, If(oButton.oNode.ParamExists("icn"), oButton.oNode.ParamVal("icn"), If(oButton.oNode.ParamExists("pic"), oButton.oNode.ParamVal("pic"), 0))) 'ntg 19.05.24 checking if RemoveFooterIcons property in defprop is 1 also in permanent screens (it already worked in flex screens)

                If qOnce.IsFlexiblePage(bp.m_sXmlFil.Trim(), bp.m_sXmlRec.Trim()) Then
                    FunctionKey = oButton.oNode.ParamVal("fk")
                    If FunctionKey = "90" Then
                        bIsPgDnBtn = True
                    End If
                    If bp.m_colColor.ContainsKey($"Icon__{FunctionKey}") AndAlso bp.getProperty("RemoveFooterIcons") = "" Then 'ntg 03.12.23 added  AndAlso bp.getProperty("RemoveFooterIcons") = "" to have an option for no icons at all
                        PicNumber = bp.m_colColor.Item($"Icon__{FunctionKey}")
                        If bEnglish And PicNumber = "16" Then 'ntg 05.02.23 - make the arrows opposite direction when in english
                            PicNumber = "41"
                        ElseIf bEnglish And PicNumber = "41" Then 'ntg 05.02.23 - make the arrows opposite direction when in english
                            PicNumber = "16"
                        End If
                    End If
                End If

                oButton.PicNumber = PicNumber

                If Not IsConstButton(oButton, IsModalWindow) Then ' הכפתורים הקבועים - יציאה , אפשרויות , חזור ו אווראב - הם בתפריט הצידי, כפתור המשך בפס התחתון הצף 'ntg 21.02.24 added IsModalWindow
                    FilteredFooterButtons.Add(oButton)
                End If
            Next

            If ScreenHasButtonList Then
                FilteredFooterButtons = FilteredFooterButtons.Where(Function(oButton) oButton.oNode.Param("fk") <> "00").ToList()
            End If

            For Each oButton As Button In FilteredFooterButtons
                If oButton.PicNumber = 0 Then
                    FilteredFooterButtonsNoImg.Add(oButton)
                Else
                    FilteredFooterButtonsWithImg.Add(oButton)
                End If
            Next

            FilteredFooterButtonsOrdered = FilteredFooterButtonsWithImg.Concat(FilteredFooterButtonsNoImg).ToList()

            For Each oButton As Button In FilteredFooterButtons
                If Not bEnglish Then
                    If oButton.oNode.ParamVal("fk") = "90" Then ' PageDown
                        oButton.oNode.Param("val") = "דפדוף מטה"
                        oButton.ControlItself.ReplaceVal("value", "דפדוף מטה")
                        'oButton.PicNumber = "37"
                    End If

                    If oButton.oNode.ParamVal("fk") = "91" Then ' PageUp
                        oButton.oNode.Param("val") = "דפדוף מעלה"
                        oButton.ControlItself.ReplaceVal("value", "דפדוף מעלה")
                        'oButton.PicNumber = "36"
                    End If
                End If
            Next

            'ntg 27.05.24 count how many valid "card" cgr's there are 
            Dim cgrCount As Integer = 0
            For i As Integer = 0 To FilteredFooterButtonsOrdered.Count - 1
                Dim oButton As Button = FilteredFooterButtonsOrdered(i)
                Dim cgrValue As String = oButton.oNode.Param("cgr") 'ntg 19.05.24 checking the cgr attr of cmd's  
                If oButton.oNode.ParamExists("cgr") AndAlso cgrValue.Length = 2 AndAlso cgrValue <> "00" AndAlso cgrValue.All(Function(c) Char.IsDigit(c)) Then 'ntg 19.05.24 if there are cmd's that have cgr, they will appear inside cards and not footer 
                    cgrCount += 1
                End If
            Next


            UseMoreButton = FilteredFooterButtons.Count - cgrCount > 5 OrElse (FilteredFooterButtonsNoImg.Count > 0 AndAlso FilteredFooterButtonsWithImg.Count > 0) 'ntg 27.05.24 subtract cards cgr from total buttons count
            CountInButtons = Math.Min(4, If(FilteredFooterButtonsWithImg.Count > 0, FilteredFooterButtonsWithImg.Count, FilteredFooterButtonsNoImg.Count))

            If UseMoreButton Then
                For i As Integer = 0 To FilteredFooterButtonsOrdered.Count - 1
                    Dim oButton As Button = FilteredFooterButtonsOrdered(i)

                    Dim cgrValue As String = oButton.oNode.Param("cgr") 'ntg 19.05.24 checking the cgr attr of cmd's  
                    If cgrValue.Length = 2 AndAlso (cgrValue = "00" OrElse Not cgrValue.All(Function(c) Char.IsDigit(c))) Then 'ntg 19.05.24 if there are cmd's that have cgr, they will appear inside cards and not footer 

                        If i < CountInButtons Then
                            sGroupHtml &= $"<div Class='FooterElement {If(FilteredFooterButtonsWithImg.Count = 0, "d-none d-lg-block", "d-flex")} px-0 mx-2'> 
                                      {oButton.ControlItself.FullCode(bp, "button", IconFullValue:=If(oButton.PicNumber <> 0, oButton.PicNumber, ""), FooterButtonWithImage:=(FilteredFooterButtonsWithImg.Count > 0))}
                                    </div>"

                            If FilteredFooterButtonsWithImg.Count = 0 Then
                                GroupInModal.Add(oButton.ControlItself.FullCode(bp, "button", AdditionalClass:="d-lg-none"))
                            End If
                        Else
                            GroupInModal.Add(oButton.ControlItself.FullCode(bp, "button"))
                        End If

                    End If
                Next
            Else
                For Each oButton As Button In FilteredFooterButtonsOrdered
                    Dim cgrValue As String = oButton.oNode.Param("cgr") 'ntg 19.05.24 checking the cgr attr of cmd's 
                    If oButton.oNode.ParamExists("cgr") AndAlso cgrValue.Length = 2 AndAlso (cgrValue = "00" OrElse Not cgrValue.All(Function(c) Char.IsDigit(c))) Then 'ntg 19.05.24 if there are cmd's that have cgr, they will appear inside cards and not footer 
                        sGroupHtml &= $"<div Class='FooterElement {If(FilteredFooterButtonsWithImg.Count = 0, "d-none d-lg-flex", "d-flex")} px-0 mx-2'> 
                                  {oButton.ControlItself.FullCode(bp, "button", IconFullValue:=If(oButton.PicNumber <> 0, oButton.PicNumber, ""), FooterButtonWithImage:=(FilteredFooterButtonsWithImg.Count > 0))}
                                </div>"

                        If FilteredFooterButtonsWithImg.Count = 0 Then
                            GroupInModal.Add(oButton.ControlItself.FullCode(bp, "button", AdditionalClass:="d-lg-none"))
                        End If
                    End If
                Next
            End If

            '' כפתור המשך
            'If IsFooterButtonExistsByFk(bp, 2) Then
            '    Dim ContinueButton As Button = bp.cButtons.First(Function(oButton) oButton.oNode.ParamVal("fk") = 2)
            '    If ContinueButton.oNode.Param("val").Trim() = "המשך" Then
            '        Dim ImageTag =  $"<img class='w-100 px-2' src='./assets/images/{bp.m_colColor.Item($"ImageFile_901")}'>"
            '        sGroupHtml &= $"<div Class='FooterElement d-flex px-0 mx-2'> 
            '                       <button class='{If(IsFooterButtonExistsByFk(bp, 2), "", "disabled")} footerButton btn btn-sm' title='המשך' onclick='fnBtnClick(this);' id='C02' pch='02' name='C02' type='button'>
            '                                {ImageTag}
            '                                <span>המשך</span>
            '                             </button>
            '                     </div>"
            '    Else
            '        sGroupHtml &= $"<div Class='FooterElement d-flex px-0 mx-2'> 
            '                          {ContinueButton.ControlItself.FullCode(bp, "button", IconFullValue:=If(ContinueButton.PicNumber <> 0 , ContinueButton.PicNumber, ""), FooterButtonWithImage:=(FilteredFooterButtonsWithImg.Count > 0))}
            '                        </div>"
            '    End If
            'End If



            If sGroupHtml > "" Then
                GroupText = $"<div id='{ConstGroup}buttonGroup' class='ButtonGroup d-flex flex-wrap mx-0 px-0 py-1'> {sGroupHtml} </div>"
                oHtml.AppendLine(GroupText)
            End If

            If GroupInModal.Count > 0 Then
                oHtml.AppendLine($"<div data-bs-toggle='tooltip' data-bs-placement='left' title= {If(bEnglish, "More...", "...עוד")} id='OpenExtraButtons' class='FooterElement d-flex {If(UseMoreButton, "", "d-lg-none")} px-0 mx-2'>
                                <button  id='btnOpenExtraButtons' class='FooterButtonsPopOver footerButton btn btn-sm px-0'>
                                    <i class='far fa-circle-ellipsis'></i>
                                </button> 
                             </div>")
            End If ' <span>עוד...</span>

            '  oHtml.AppendLine(bp.getProperty("Copyright"))

            oHtml.AppendLine("</div>") ' End of footerRow
            oHtml.AppendLine("</div>") ' End of FooterButtonsDiv

            ' Copyright Row


            oHtml.AppendLine("</div>")
            oHtml.AppendLine("</footer>")

            If GroupInModal.Count > 0 Then
                Dim ActionButtonsString As String = ""

                For Each ActionNodeStr As String In GroupInModal
                    ActionNodeStr = ActionNodeStr.Replace("button", "a") ' PopOver works with a (hyperlink) and not with buttons
                    ActionNodeStr = ActionNodeStr.Replace("footerButton px-0 btn btn-sm", $"px-1 py-1 btn list-group-item {If(bEnglish, "popOverItemEnglish", "popOverItem")} FooterButtonsPopOverItem") 'ntg 14.06.23 aligned text of items inside (...) in floating menu to the left if english tdi

                    ActionButtonsString = ActionButtonsString + ActionNodeStr
                Next

                oHtml.AppendLine($"<ul id='FooterButtonsPopover-content' class='d-none list-group'>
                                 {ActionButtonsString}
                               </ul>")
            End If

            'ntg 08.06.23 warning and stopping alerts (yellow and red)
            Dim warnings As String() = As400WarningStr.Replace("ExceedsMax", "").Split(New String() {"<br>"}, StringSplitOptions.RemoveEmptyEntries)
            Dim foundWarning As String = ""
            Dim remainingWarnings As String = ""

            For Each warning As String In warnings
                If warning.Contains("נעצר") Then
                    foundWarning = warning
                Else
                    remainingWarnings += "<i class='fa-solid fa-triangle-exclamation'></i> " & warning & "<br>"  'ntg 25.06.23 adding triangle-exclamation before each warning
                End If
            Next
            If As400WarningStr <> "" Then

                If foundWarning <> "" Then
                    oHtml.AppendLine($"<div class='modal fade' id='warningsModal'>
        <div id='warningsModalButtons' class='modal-dialog modal-dialog-centered' style='display: flex;align-items: center;align-content: center;flex-direction: column;justify-content: center'>
<div class='modal-content'>
        <div class='py-1 warningHeaderError modal-header'>
            {If(bEnglish, "<h4 class='modal-title'><i class='fa-solid fa-triangle-exclamation'></i> Stop Warning</h4>", "<h4 class='modal-title'>הודעת עצירה <i class='fa-solid fa-triangle-exclamation'></i></h4>")}
        </div>
        <div class='py-1 modal-body'>
            <div class='row align-items-center'>
                <p class='my-1'>{foundWarning}</p>
                <p class='my-0 fw-bold' id='MaxMessage'></p>
            </div>
        </div>
        <div class='py-2 modal-footer'>
            <button type='button' class='CloseFoundWarningModal btn btn-outline-danger' data-bs-dismiss='modal' data-bs-target='#foundWarningModal'>{If(bEnglish, "Close", "סגירה")}</button>
        </div>
    </div>")
                ElseIf remainingWarnings <> "" Then
                    oHtml.AppendLine($"<div class='modal fade' id='warningsModal'>
        <div id='warningsModalButtons' class='modal-dialog modal-dialog-centered' style='display: flex;align-items: center;align-content: center;flex-direction: column;justify-content: center'>
            <div class='modal-content'>
                <div class='py-1 warningHeaderAlert modal-header'>
                {If(bEnglish, "<h4 class='modal-title'><i class='fa-solid fa-triangle-exclamation'></i> Warnings</h4>", "<h4 class='modal-title'>התראות <i class='fa-solid fa-triangle-exclamation'></i></h4>")}    
                </div>
                <div class='py-1 modal-body'>
                    <div class='row align-items-center'>
                        <p class='my-1'>{remainingWarnings}</p>
                        <p class='my-0 fw-bold' id='MaxMessage'></p>
                    </div>
                </div>
                <div class='py-2 modal-footer'>
                    <button type='button' class='CloseWarningsModal btn btn-outline-warning' data-bs-dismiss='modal' data-bs-target='#warningsModal'>{If(bEnglish, "Close", "סגירה")}</button>
                </div>
            </div>
        </div>
    </div>")
                End If
            End If

            'qAround.FormForms(bp, oHtml, oNodeScreen) ntg 05.06.23

            oHtml.AppendLine("<script src=""./assets/scripts/popper.min.js""></script>")
            oHtml.AppendLine("<script src=""./assets/bootstrap5/js/bootstrap.bundle.min.js""></script>")
            oHtml.AppendLine("<script src='./assets/scripts/bootstrap4-toggle.min.js'></script>")
            oHtml.AppendLine("<script src=""./assets/scripts/datePickerLogic.js""></script>")
            oHtml.AppendLine("<script src='./assets/scripts/inputMask/jquery.inputmask.js'></script>")
            oHtml.AppendLine("<script src='./assets/scripts/inputMask/inputmask.binding.js'></script>")

            oHtml.AppendLine("<script>")

            If bp.getProperty("BlockSelect") = "true" AndAlso oNodeScreen.Param("wait") = "S" Then
                oHtml.Append("document.onselectstart=new Function('return false');").Append(vbCrLf)
            End If
            oHtml.Append("window.onresize = changeWindowSize;").Append(vbCrLf)
            oHtml.Append("console.log('Width: ' + window.innerWidth + ', Height: ' + window.innerHeight);").Append(vbCrLf)


            oHtml.Append("$(document).ready(function() {").Append(vbCrLf)

            ' Open external URL(submit form or in a new tab)
            If sCommand > "" Then
                Select Case bp.m_sImageMethod
                    Case "II" : oHtml.Append($"window.open('{bp.getPropertyHttp("ImagePath")}?{sCommand}');" + vbCrLf)
                    Case "IF", "IP" : oHtml.Append("document.getElementById('frmImage').submit();").Append(vbCrLf)
                    Case "IU" : oHtml.Append($"window.open('{sCommand}');" + vbCrLf)
                    Case "IJ", "IV"
                        'vk 05.22
                        Dim sUrl As String = ""
                        Dim sToken As String = ""
                        Dim sMessage As String = ""
                        Dim a, b(), sParams, sJson As String
                        sParams = sCommand
                        If bp.m_sImageMethod = "IJ" Then
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
                        Dim o As PocketKnife.Info = New PocketKnife.Info 'vk 12.23
                        sJson = "{""app"":""" & o.DecryptPassword(bp.getProperty("UrlToToken_Key")) & """"
                        o = Nothing
                        For Each a In sParams.Split("&")
                            b = a.Split("=")
                            Select Case b.Length
                                Case Is >= 2
                                    sJson &= ",""" & b(0).Trim & """:""" & Replace(Replace(b(1).Trim, "\", "\\"), """", "\""") & """"
                            End Select
                        Next
                        sJson &= "}"
                        'Dim l As Log = New Log
                        'l.LogXML(bp, "FIN", sJson, "", pSes, bp.m_sUser)
                        'l = Nothing
                        Try
                            'vk 07.22
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls Or SecurityProtocolType.Ssl3 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls12
                            ServicePointManager.ServerCertificateValidationCallback = Function(se As Object,
                                cert As Cryptography.X509Certificates.X509Certificate,
                                chain As Cryptography.X509Certificates.X509Chain,
                                sslerror As SslPolicyErrors) True

                            Dim ReturnValue As ReturnValue '= New ReturnValue()
                            Using client As HttpClient = New HttpClient()
                                Using inputContent As HttpContent = New StringContent(sJson, System.Text.Encoding.UTF8, "Application/JSON")
                                    Using response As HttpResponseMessage = client.PostAsync(bp.getProperty("UrlToToken_API"), inputContent).Result
                                        If response.IsSuccessStatusCode Then
                                            Dim rt As Object = response.Content.ReadAsStringAsync().Result
                                            ReturnValue = Newtonsoft.Json.JsonConvert.DeserializeObject(Of ReturnValue)(rt)
                                            If ReturnValue.data.token Is Nothing Then
                                                sMessage = ReturnValue.message
                                            Else
                                                sToken = ReturnValue.data.token
                                            End If
                                        Else
                                            Dim rt As Object = response.Content.ReadAsStringAsync().Result
                                            sMessage = rt
                                        End If
                                    End Using
                                End Using
                            End Using
                        Catch e1 As Exception
                            sMessage = e1.Message
                        End Try
                        If sToken > "" Then
                            oHtml.Append($"window.open('{sUrl}?t={sToken}');" + vbCrLf)
                        Else
                            'oHtml.Append($"MyMsgBox('{sMessage}',false,null,'Getting token for URL');" + vbCrLf)
                            oHtml.Append($"MyMsgBox('{sMessage}');" + vbCrLf)
                        End If
                End Select
            End If

            oHtml.AppendLine("InitConst(10,4,1,5,2,1);")

            If As400WarningStr <> "" Then
                If As400WarningStr.Contains("ExceedsMax") Then
                    oHtml.AppendLine("$('#MaxMessage').text(msg(25))")
                End If
                oHtml.AppendLine("$('#warningsModal').modal('show');")
            End If

            oHtml.Append("changeWindowSize();").Append(vbCrLf)
            oHtml.Append("changeIframeSize();").Append(vbCrLf)
            oHtml.Append("ControllGoTopButton();").Append(vbCrLf)
            oHtml.Append("InitModal();").Append(vbCrLf)
            oHtml.Append("InitInvalidFeedback();").Append(vbCrLf)
            oHtml.Append("InitButtons();").Append(vbCrLf)
            oHtml.AppendLine("InitMaskedInputs();")

            oHtml.AppendLine($"HideEmptyTableCols({If(IsGridModal(), 1, 0)});")

            If oNodeScreen.ParamIn("fgr", "LBD") Then 'ntg 23.06.24 fgr=B is the same as fgr=L, but with option to select multiple rows in a table Then
                'vk 09.24 fgr=D
                oHtml.AppendLine("HideCardWithEmptyTable();")
            End If


            If oNodeScreen.Param("fgr") = "B" OrElse m_ModalScreen.Contains("fgr=""B""") Then 'ntg 23.06.24 fgr=B is the same as fgr=L, but with option to select multiple rows in a table 'ntg 09.07.24 also check if it a modal window type win=s that has fgr=B
                oHtml.AppendLine("ClickMultipleRowsWithRadioButton();")
            Else
                oHtml.AppendLine("ClickRowWithRadioButton();")
            End If

            'oHtml.AppendLine("ClickRowWithRadioButton();")

            If m_ModalScreen <> "" Then ' יצירת חלון מודלי 'vk 02.23
                oHtml.AppendLine("$('#ModalWindow').modal('show');")
            End If

            oHtml.AppendLine("dragElement(document.getElementById(""FooterButtonsDiv""));")


            If bp.IsFoldersScreen(oNodeScreen) Then
                Dim specialImageLastCard As String = "Img"
                If (bp.getProperty("ColorField") <> "" AndAlso bp.getProperty("ColorField").Contains("Ramot")) Then
                    specialImageLastCard = "Ramot"
                End If
                oHtml.AppendLine($"{If(bEnglish, "ReplaceLastCardEnglish();", $"ReplaceLastCard('{specialImageLastCard}');")}")
            End If


            If bp.getProperty("ShowXml") = "True" AndAlso bp.DebugWarningList.Count > 0 AndAlso DifferentPage Then
                bp.DebugWarningList = bp.DebugWarningList.Distinct().ToList()

                Dim index As Integer = 0
                Dim AlertString As String = "שגיאות שקיימות בדף:" + "\n"
                For Each item As String In bp.DebugWarningList
                    AlertString = AlertString + $" {index + 1}. {item} \n"
                    index = index + 1
                Next

                oHtml.AppendLine($"alert('{AlertString}')")
            End If

            'If bp.EwrErrorText <> "" Then
            '    oHtml.AppendLine("$('#warningModal').modal('show');")
            'End If

            'Dim Qpxl = oNodeScreen.ParamVal("qpxl")
            'If oNodeScreen.ParamVal("qpxl") > 0 Then
            '    oHtml.AppendLine($"topScroll({Qpxl})")
            'End If
            'vk 09.24
            Dim aQpxlNew() As String = sQpxlNew.Split(";")
            If aQpxlNew.Length >= 3 AndAlso aQpxlNew(0) = oNodeScreen.Param("fil").Trim AndAlso aQpxlNew(1) = oNodeScreen.Param("rec").Trim Then
                If aQpxlNew.Length >= 4 Then
                    oHtml.AppendLine($"Accords_Apply(""{aQpxlNew(3)}"");")
                End If
                oHtml.AppendLine($"topScroll({aQpxlNew(2)});")
            End If

            If bp.m_sNameFocus > "" Then
                oHtml.Append("fnCur('").Append(bp.m_sNameFocus).Append("');").Append(vbCrLf)
            End If

            If bp.getProperty("PingMaxCount") <> "" Then
                oHtml.Append("StartPing();").Append(vbCrLf)
            End If

            If bp.m_sCG > "" Then
                If bp.getProperty("WsApi") = "ESB" Then
                    oHtml.Append("CG_ESB();" + vbCrLf) 'vk 04.21
                ElseIf bp.getProperty("WsApi") > "" Then 'vk 07.22
                    oHtml.Append("CG1();" + vbCrLf)
                End If
            End If

            Dim oFlexCombo, oFlexComboBoss As FlexCombo
            If bp.cFlexCombos IsNot Nothing Then
                For Each oFlexCombo In bp.cFlexCombos
                    If Not (oFlexCombo.bBoss OrElse oFlexCombo.bDouble) Then
                        For Each oFlexComboBoss In bp.cFlexCombos
                            If oFlexComboBoss.sGroup = oFlexCombo.sGroup AndAlso oFlexComboBoss.bBoss Then
                                oHtml.Append("flexlist1('").Append(oFlexComboBoss.sN).Append("','").Append(oFlexCombo.sN).Append("',false);").Append(vbCrLf)
                            End If
                        Next
                    End If
                Next
            End If

            If bp.sListOfLtr <> ";" Then
                oHtml.AppendLine($"$(""#Hltr"").val(""{bp.sListOfLtr}"")")
            End If

            oHtml.Append("$('[data-bs-toggle=""tooltip""]').tooltip()").Append(vbCrLf) ' to enable Bootstrap tooltip
            oHtml.AppendLine("SetActionButtonsPopOverExternal()")
            oHtml.AppendLine("SetActionButtonsPopOver()")
            oHtml.AppendLine("SetFooterButtonsPopOver()")
            oHtml.AppendLine("SetFormSelectDefaultColor()")
            oHtml.AppendLine("setSelect2();")
            oHtml.AppendLine("adjustSidebar();") 'ntg 21.07.24 changes regarding sidebar fields

            oHtml.Append("$(window).bind('keydown', function (e) {").Append(vbCrLf)
            oHtml.Append("var keyCode = e.keyCode || e.which;").Append(vbCrLf)
            oHtml.Append("if (keyCode >= 112 && keyCode <= 123) {").Append(vbCrLf)
            oHtml.Append("window.onhelp = function() { return false; }").Append(vbCrLf)
            oHtml.Append("e.cancelable = true;").Append(vbCrLf)
            oHtml.Append("e.stopPropagation();").Append(vbCrLf)
            oHtml.Append("e.preventDefault();").Append(vbCrLf)
            oHtml.Append("e.returnValue = false;").Append(vbCrLf)
            oHtml.Append("}});").Append(vbCrLf)

            ' Disable back button
            oHtml.AppendLine("window.onbeforeunload = function () {return CloseMsg();};")
            oHtml.AppendLine("document.addEventListener('backbutton', function(){}, false);")

            oHtml.Append("});").Append(vbCrLf)
            'oHtml.Append("</SCRIPT>").Append(vbCrLf)
            If oNodeScreen.Param("fil").Trim = bp.StopFile Then
                If bp.getProperty("Relogon").ToLower = "true" Then 'vk 04.24 - ntg 08.04.24 changes to not have blank screen after exiting the program
                    oHtml.Append("Relogon(").Append(bp.getProperty("CloseMsgSeconds")).Append(");").Append(vbCrLf)
                Else
                    oHtml.Append("CloseWindow(").Append(bp.getProperty("CloseMsgSeconds")).Append(");").Append(vbCrLf)
                End If
                bStopSession = True
            End If
            If bIsShowMore And bIsPgDnBtn Then
                oHtml.Append("setBlinkingButton()").Append(vbCrLf)
            End If

            'oHtml.Append("transformTableToSelect()").Append(vbCrLf)
            oHtml.Append("checkSmartphoneMode();").Append(vbCrLf)

            'oHtml.Append("$(window).on('unload', function () {OnClose();});").Append(vbCrLf) 'vk 03.23
            If bp.getProperty("CloseJob").ToLower <> "session" Then 'ntg 14.07.24 vladi's change regarding "back" button on browser
                oHtml.Append("$(window).on('unload', function () {OnClose();});").Append(vbCrLf)
            End If


            oHtml.Append("</script>").Append(vbCrLf)
            oHtml.Append("</body>").Append(vbCrLf)
            oHtml.Append("</html>").Append(vbCrLf)
        End If ' end of - If IsModalWindow = False Then

        'If IsModalWindow = True Or (oNodeScreen.Param("win") <> "S" AndAlso oNodeScreen.Param("win") <> "R") Then 'ntg 05.06.23 fixing teufa

        'If IsModalWindow = True Or (IsModalWindow = False And m_ModalScreen = "") Then
        If IsModalWindow OrElse m_ModalScreen = "" OrElse bAgentStatement Then 'vk 02.24++ ntg 28.02.24 changes for הצהרת סוכן

            oHtml.AppendLine("</form>") 'ntg 20.06.23 in case of a modal window, the script didnt reach this line so now its added

            qAround.FormForms(bp, oHtml, oNodeScreen)
            qOnce.FormHiddens(bp, oHtml, oNodeScreen, sScreenNum, sLastEntry, sWarning, sCommand, bFirstScreen, m_ModalScreen, IsModalWindow) 'ntg 07.06.23
            If oNodeScreen.Param("fgr") = "T" Then
                oHtml.Append("<label Class=""btn btn-primary btn-sm"" name=""lblSqlMsg"" id=""lblSqlMsg""></label>").Append(vbCrLf) 'ntg 07.06.23
            End If

            If oNodeScreen.Param("fil").Trim = "HFPSENDSFM" AndAlso oNodeScreen.Param("rec").Trim = "PEPRNTHL" Then
                oHtml.AppendLine("<script>areaCodeFieldWidth()</script>")
            End If
        End If

        Return oHtml.ToString

    End Function

    'Private Function GetModalWindowString(ModalScreen As String) As String
    Private Function GetModalWindowString(ModalScreen As String, ByRef bp As BuildPage) As String
        ' {ModalScreen}
        ' <iframe src='saved1_grid.htm' frameborder='0' id='iframeResult' name='iframeResult' allowfullscreen='true'></iframe>
        Dim ModalWindow As String = ""

        'ModalWindow = $"<div class='modal fade' id='ModalWindow' data-bs-backdrop='static' data-bs-keyboard='false'>
        '                   <div id='ModalDialogDiv' class='modal-dialog modal-xl'>
        '                     <div id='modalContent' class='modal-content'>
        '                        {ModalScreen}
        '                     </div>
        '                   </div>
        '                </div>"
        'Return ModalWindow

        Dim XDocModal As XDocument = XDocument.Parse(ModalScreen)
        Dim sElement As XElement = XDocModal.Descendants("s").First()

        If sElement.Attribute("fil").Value.Trim() = "HFMELELFM" And sElement.Attribute("rec").Value.Trim() = "RISURMVTH" Then
            '------------------------------------אם מדובר במסך הצהרת סוכן ------------------------------------------------
            bAgentStatement = True 'vk 02.24++ ntg 28.02.24 changes for הצהרת סוכן

            Dim ModalTitle As String = Heb(XDocModal.Descendants("r").FirstOrDefault().Attribute("ftit").Value)
            Dim BodyTextLines As List(Of String) = (From e In XDocModal.Descendants("f")
                                                    Where e.Attribute("num").Value.StartsWith("LIN")
                                                    Select Heb(e.Attribute("val").Value.Trim())).ToList()

            Dim BodyText As String = String.Join(" ", BodyTextLines)
            BodyText = BodyText.Replace(":", ":<br>").Replace(".", ".<br>")

            Dim Labeltext As String = (From e In XDocModal.Descendants("f")
                                       Where e.Attribute("num").Value.StartsWith("$00000")
                                       Select Heb(e.Attribute("val").Value.Trim())).FirstOrDefault()

            Dim AnswerElement As XElement = (From e In XDocModal.Descendants("f")
                                             Where e.Attribute("num").Value.StartsWith("ANS")
                                             Select e).FirstOrDefault()

            Dim Answers As List(Of XElement) = AnswerElement.Descendants("b").ToList()

            Dim YesAnswer As String = Heb((From e In Answers
                                           Where e.Attribute("op").Value = "כ"
                                           Select e.Attribute("nm").Value.Trim()).FirstOrDefault().ToString())

            Dim NoAnswer As String = Heb((From e In Answers
                                          Where e.Attribute("op").Value = "ל"
                                          Select e.Attribute("nm").Value.Trim()).FirstOrDefault().ToString())
            Dim FullName As String = ""
            If AnswerElement IsNot Nothing Then
                FullName = "M" + AnswerElement.Attribute("num").Value + AnswerElement.Attribute("ind").Value + AnswerElement.Attribute("typ").Value + AnswerElement.Attribute("len").Value + "000"
            End If

            ModalWindow = $"<div class='modal fade' id='ModalWindow' data-bs-backdrop='static' data-bs-keyboard='false'>
                       <div id='ModalWindowButtons' class='modal-dialog modal-lg modal-dialog-centered'>
                        <div class='modal-content'>
            	            <div class='py-3 modal-header'>
            		            <h4 class='mx-auto modal-title'><strong>{ModalTitle}</strong></h4>
                                <button type='button' class='visually-hidden btn-close' data-bs-dismiss='modal' aria-label='Close'></button>
            	            </div>
            	            <div class='py-1 modal-body'>
                                 <div class='row align-items-center'>
                                     <p>{BodyText}</p>
                                 </div>
                                 <div class='py-3'>
                                     <p>{Labeltext}</p>
                                 </div>
                            </div>
            	            <div id='ModalWindowFooter' class='modal-footer d-flex justify-content-center py-3'>
                                 <input value="""" id='{FullName}' name='{FullName}' type='hidden'>
            		             <button name='S{FullName}' value='כ' type='button' class='YesButton modalFooterButtons btn btn-success' pch=""00"" onclick='CopyOut(this);fnBtnClick(this);'>{YesAnswer}</button>
                                 <button name='S{FullName}' value='ל' type='button' class='NoButton modalFooterButtons btn btn-danger' pch=""00"" onclick='CopyOut(this);fnBtnClick(this);'>{NoAnswer}</button>
            	            </div>
                        </div>
                       </div>
                       </div>"
            'Return ModalWindow  'ntg 28.02.24 changes for הצהרת סוכן

            Dim sOut As StringBuilder = New StringBuilder 'ntg 28.02.24 changes for הצהרת סוכן
            qOnce.FormHiddens(bp, sOut, oNodeScreen, "0", "", "", "", False, "+", False)
            Return ModalWindow & sOut.ToString

        Else

            '------------------------------------כל מסך אחר------------------------------------------------
            Dim qqBoo As New Boo()
            Dim ModScreen = qqBoo.Xml2html(bp, ModalScreen, "", "", "", "", "", False, False, "", "", False, True, "")

            ModalWindow = $"<div class='modal fade' id='ModalWindow' data-bs-backdrop='static' data-bs-keyboard='false'>
                   <div id='ModalWindowButtons' class='modal-dialog modal-lg modal-dialog-centered'>
                    <div class='modal-content-reg' style='margin:auto;'>
                {If(bp.getProperty("ShowXml") = "true", $"<a id='ShowXmlElem' class='nav-link'  title='Show XML' href='tmp-{bp.m_sSession}-InputFile.xml' target='_blank'>
                                <i id='showXmlBtnModal' class='fas fa-eye';></i>
                                </a>", "")}
                    {ModScreen}" 'vk 09.24 margin:auto

            Return ModalWindow
        End If
    End Function

    Protected Friend Property IsEnglish As Boolean
        Get
            Return bEnglish
        End Get
        Set(value As Boolean)
            bEnglish = value
        End Set
    End Property
    'Public Function IsEnglish()
    '    Return bEnglish
    'End Function

    Private Function FixedButtons(bp As BuildPage, hideSideBar As Boolean) As String
        Dim Options As String = $"{If(bEnglish, "Options", "אפשרויות") }"
        Dim Back As String = $"{If(bEnglish, "Back", "חזרה") }"
        Dim Out As String = $"{If(bEnglish, "Finish/Exit", "סיום/יציאה") }"
        Dim toHideSideBar As String = $"{If(hideSideBar, "class='d-lg-none'", "")}"

        Dim oMenuLine As MenuLine 'ntg 17.06.24 counting number of breadCrumbs (under class='title-header')


        For Each oMenuLine In bp.cMenuLines.Values.ToList() 'ntg 17.06.24 counting number of breadCrumbs (under class='title-header')
            If oMenuLine.bSubMenu = False AndAlso oMenuLine.sText <> "" Then
                countBreadCrumbs += 1
            End If
        Next

        Dim sOptionsButtonStyleFA As String = bp.getProperty("OptionsButtonStyleFA", "fa-solid fa-circle-plus sideIcon")
        Dim sBackButtonStyleFA As String = bp.getProperty("BackButtonStyleFA", "fa-solid fa-circle-arrow-right sideIcon")
        Dim sFinishButtonStyleFA As String = bp.getProperty("FinishButtonStyleFA", "fa-solid fa-flag-checkered sideIcon")

        Dim FixedButtonsList As String = 'ntg 17.04.24 top header and sidebar fixation
          $"{If(hideSideBar, "", "<hr class='mx-auto sideBarDivider'> ")}
            <ul class='NavFixedButtonsList w-100 nav nav-pills {If(hideSideBar, "flex-column", "flex-row")}'>
	            <li class='my-2 nav-item'>
                    <button class='{If(hideSideBar, "text-center", "")} {If(IsFooterButtonExistsByFk(bp, 10), "", "disabled")} nav-link CommandButtonInMenu btn btnIcon'  {If(bEnglish, "style=""text-align: left"" ", "") } {If(hideSideBar, "data-bs-toggle='tooltip' data-bs-placement=""left""", "")} title={Options} onclick='fnBtnClick(this);' id='C10' pch='10' name='C10' type='button'>
                        <i class='{sOptionsButtonStyleFA}' ></i>
                        <span style='display:none;' {toHideSideBar}>{Options}</span>
                    </button>
               </li>

                <li class='my-2 nav-item'>
		            <button class='{If(hideSideBar, "text-center", "")} {If(countBreadCrumbs <> 1 AndAlso (IsFooterButtonExistsByFk(bp, 12) OrElse oNodeScreen.Param("fgr") = "M"), "", "disabled")} nav-link CommandButtonInMenu btn btnIcon'  {If(bEnglish, "style=""text-align: left"" ", "") } {If(hideSideBar, "data-bs-toggle='tooltip' data-bs-placement=""left""", "")} title={Back} onclick='fnBtnClick(this);' id='C12' pch='12' name='C12' type='button'>
                        <i class='{sBackButtonStyleFA}'></i>
                        <span style='display:none;'{toHideSideBar}>{Back}</span>
                    </button>
	            </li>

                <li  class='my-2 nav-item'>
		            <button class='{If(hideSideBar, "text-center", "")} {If((countBreadCrumbs = 1 OrElse countBreadCrumbs = 0) AndAlso (IsFooterButtonExistsByFk(bp, 3) OrElse oNodeScreen.Param("fgr") = "M"), "", "disabled")} nav-link CommandButtonInMenu btn btnIcon'  {If(bEnglish, "style=""text-align: left"" ", "") } {If(hideSideBar, "data-bs-toggle='tooltip' data-bs-placement=""left""", "")} title={Out} onclick='fnBtnClick(this);' id={If(countBreadCrumbs = 1, "C12", "C03")} pch={If(countBreadCrumbs = 1, "12", "03")} name={If(countBreadCrumbs = 1, "C12", "C03")} type='button'>
                        <i class='{sFinishButtonStyleFA}'></i>
                        <span style='display:none;' {toHideSideBar}>{Out}</span>
                    </button>
	            </li>
            </ul>
            {If(hideSideBar, "", "<hr class='mx-auto sideBarDivider'> ")}"
        Return FixedButtonsList
    End Function

    Private Function IsFooterButtonExistsByFk(bp As BuildPage, fk As Integer) As Boolean
        Return bp.cButtons.Any(Function(oButton) oButton.oNode.ParamVal("fk") = fk)
    End Function

    Private Function IsFooterButtonExistsByFk(bp As BuildPage, fk As String) As Boolean
        Return bp.cButtons.Any(Function(oButton) oButton.oNode.Param("fk") = fk)
    End Function

    Private Function GetDefaultPicNumber(oButton As Button) As Integer
        Dim FunctionKey = oButton.oNode.ParamVal("fk")
        Select Case FunctionKey
            Case 2
                Return 901
            Case 3
                Return 902
            Case 10
                Return 903
            Case 12
                Return 904
            Case Else
                Return 0
        End Select
    End Function

    Private Function IsConstButton(oButton As Button, IsModalWindow As Boolean) As Boolean 'ntg 21.02.24 added IsModalWindow As Boolean

        If oButton.oNode.Param("fk") = "OR" Then
            Return True
        End If

        Dim FunctionKey = oButton.oNode.ParamVal("fk")

        If oButton.oNode.Param("fk") = "10" AndAlso (IsModalWindow = True Or (IsModalWindow = False And m_ModalScreen <> "")) Then 'ntg 21.02.24 if its a modal screen, show the f10 anyways
            Return False
        End If

        Select Case FunctionKey
            'Case 12 And m_ModalScreen <> ""
            '    Return True
            'Case 3 AndAlso m_ModalScreen = ""
                'Return False
            Case 3, 10, 12 '2
                If oButton.oNode.Param("cgr") = "0!" Then 'ntg 16.06.24 buttons with this parameter are "forced" to appear in the footer
                    Return False
                Else
                    Return True
                End If
            Case Else
                Return False
        End Select
    End Function

    Private Function GetRectanleNumber(nColumn As Integer, nLine As Integer) As Integer
        Dim i As Integer
        For i = 1 To nMaxRect
            Dim oRect As Node = cRectangles(i.ToString)
            If oRect.ParamVal("rx1") <= nColumn AndAlso nColumn <= oRect.ParamVal("rx2") AndAlso oRect.ParamVal("ry1") <= nLine AndAlso nLine <= oRect.ParamVal("ry2") Then
                Return i
            End If
        Next
        Return 0
    End Function

    Private Sub SetSessionFirstLineVars(oNode As Node, bp As BuildPage)
        If PageHeaderHtml = "" Then
            Dim qConv As New Conv()

            Select Case oNode.Param("num").Trim("~").Trim()
                'Case "MAINKT2" 'rem ntg 17.06.24 from now on, we'll use the text coming only from the parameter EnvironmentIndication in the defprop
                    'HeaderCompanyName = Heb(oNode.Param("val").Trim())
                Case "USER"
                    HeaderUserName = Heb(oNode.Param("val").Trim())
                Case "MSOF"
                    HeaderMasof = Heb(oNode.Param("val").Trim())
                Case "DATE"
                    HeaderPageDate = qConv.ConvDate(bp, oNode.Param("val").Trim(), "", "AS2PC", oNode.Param("typ"))
            End Select
        End If

        'ntg 17.06.24 from now on, we'll use the text coming only from the parameter EnvironmentIndication in the defprop
        If bp.getProperty("EnvironmentIndication") <> "" Then
            HeaderCompanyName = bp.getProperty("EnvironmentIndication")
        End If

        If oNode.ParamVal("col") < 40 AndAlso oNode.ParamVal("col") + oNode.ParamVal("len") > 40 Then
            ScreenName = Heb(oNode.Param("val").Trim())
        End If

    End Sub

    Private Sub BooMenu(ByRef oHtml As StringBuilder, ByRef bp As BuildPage)

        oHtml.Append(sMenuHiddens).Append(vbCrLf)
        Dim oMenuLine As MenuLine
        Dim cLevels As New Collection(), oLevel_i, oLevel_j As MyNumber
        Dim i, j As Integer
        'Dim oControl As Control

        For Each oMenuLine In bp.cMenuLines.Values.ToList()
            If oMenuLine.nColumn = 0 AndAlso bEnglish Then
                oMenuLine.nColumn = 1000
            End If
            Try
                oLevel_i = New MyNumber(oMenuLine.nColumn)
                cLevels.Add(oLevel_i, oMenuLine.nColumn.ToString)
            Catch
            End Try
        Next

        If cLevels.Count > 1 Then
            For i = 1 To cLevels.Count
                oLevel_i = cLevels(i)
                For j = i + 1 To cLevels.Count
                    oLevel_j = cLevels(j)
                    If bEnglish Then
                        If oLevel_j.Value < oLevel_i.Value Then Flip(cLevels, i, j)
                    Else
                        If oLevel_j.Value > oLevel_i.Value Then Flip(cLevels, i, j)
                    End If
                Next
            Next
            Flip(cLevels, cLevels.Count, cLevels.Count - 1)
        End If
        oHtml.AppendLine("<div class='col mx-auto py-2 menu-container'>")
        oHtml.AppendLine("<div class='title'>")
        oHtml.Append("<button type=""button"" class=""title-back"" pch=""12"" id=""C12_plus"" name=""C12_plus"" ")
        oHtml.Append("onclick=""event.preventDefault();Fire('123');return false;"">")
        If bEnglish Then
            oHtml.Append("<i class=""fas fa-arrow-left""></i></button>").Append(vbCrLf)
        Else
            oHtml.Append("<i class=""fas fa-arrow-right""></i></button>").Append(vbCrLf)
        End If
        Dim bSecondHalf As Boolean = False
        bp.m_sCurClass = "" '"title-header"

        Dim MenuItemsOrdered As List(Of MenuLine) = (From x In bp.cMenuLines.Values.ToList()
                                                     Select x
                                                     Order By x.nColumn, x.bSubMenu Descending).ToList()

        For i = 1 To cLevels.Count
            oLevel_i = cLevels(i)
            Dim MenuItemsRelevant As List(Of MenuLine) = MenuItemsOrdered.Where(Function(x) x.nColumn = oLevel_i.Value).ToList()

            If bSecondHalf Then ' בניית החלק של כפתורי התפריט

                Dim RelevantButtons As List(Of MenuLine) = MenuItemsRelevant.Where(Function(x) x.bSubMenu).ToList()

                If RelevantButtons.Count > 0 Then

                    'rk 15.07.24 - add html for menu shortcuts ,rk 29.07.24 a little fix in case we dont receive the "op" param in xml
                    Dim oMenuLineShortcut As MenuLine = MenuItemsOrdered.Where(Function(x) x.bIsMenuShortcut).FirstOrDefault
                    If Not oMenuLineShortcut Is Nothing Then
                        oHtml.AppendLine("<div class='menuShortcut form-group d-flex floating-label'>")
                        oHtml.AppendLine($"<input class=' form-control l  ' style='text-align:right; width:10%' id='" + oMenuLineShortcut.sN + "' type='text'
                       name='" + oMenuLineShortcut.sN + "'onbeforedeactivate=""if(NotReady('beforedeactivate'))return;fnBlurNum(this, 3, 0,' ');""
                       oninput=""if(NotReady('input'))return;this.value=this.value.slice(0,this.maxLength);"" dir='ltr' placeholder=' '
                       maxlength='" + oMenuLineShortcut.nLen.ToString() + "' onfocus=""if(NotReady('focus'))return;bDontSelect=true;fnFocusNum(this);bDontSelect=false;"" step='any'>")

                        oHtml.AppendLine($"<button class='menuShortcutBtn' type='button'
                       onclick=""fnFocusBoo4('" + oMenuLineShortcut.sN + "', 1);fnpEnter_dblclick();DisableFooterBtns();checkAndToggleSelectDisplay();addHeaderText();"">
                       <i class='fa-regular fa-arrow-left-long'></i>
                       </button>")

                        oHtml.AppendLine("<label class='form-label' dir='rtl'>מסלול מקוצר</label>")
                        oHtml.AppendLine("</div>")
                    End If

                    oHtml.AppendLine("<div class='accordion col-lg-5' id='accordionPanels1'>")
                    oHtml.AppendLine("<div class='my-2 accordion-item'>")

                    'oHtml.AppendLine("<div class='my-3 gap-3 d-flex MenuColumnHeader'>
                    '                 <img class='grid-icon' src='./assets/images/024-grid-3.svg'>
                    '                 <h3 class='my-auto'>תפריטים</h3>
                    '                 </div>")

                    'oHtml.AppendLine($"<h3 class='accordion-header' id='card-heading1'>
                    '           <button class='accordion-button Menu-accordion-button' type='button' data-bs-toggle='collapse' data-bs-target='#Card1' aria-expanded='True' aria-controls='card1'>
                    '                      <img class='mx-2 grid-icon' src='./assets/images/024-grid-3.svg'>
                    '                      <span>תפריטים</span>
                    '                    </button>
                    '             </h3>")

                    Dim MenuIcon As String = $"./assets/images/{If(bp.getProperty("MenuIcon") <> "", bp.getProperty("MenuIcon"), "menuNew.svg")}"

                    oHtml.AppendLine($"<h3 class='accordion-header' id='card-heading1'>
                                        <button class='accordion-button Menu-accordion-button' type='button' data-bs-toggle='collapse' data-bs-target='#Card1' aria-expanded='True' aria-controls='card1'>
                                          <img class='mx-2 grid-icon' src={MenuIcon}>") 'ntg 14.11.23 - added a check if it is a company with special design, and change the menu logo accordingly
                    If bEnglish Then
                        oHtml.AppendLine($"<span>Menus</span>") 'ntg 31.01.23
                    Else oHtml.AppendLine($"<span>תפריטים</span>")
                    End If

                    oHtml.AppendLine($"</button> </h3>")

                    oHtml.AppendLine("<div class='accordion-collapse collapse show' id='Card1'>
							          <div class='accordion-body'>")

                    CreateMenuItems(oHtml, bp, bSecondHalf, RelevantButtons)

                    oHtml.AppendLine("</div>") 'accordion-body
                    oHtml.AppendLine("</div>") 'accordion-collapse

                    oHtml.AppendLine("</div>") 'accordion-item
                    oHtml.AppendLine("</div>") 'accordion
                End If

                RelevantButtons = MenuItemsRelevant.Where(Function(x) Not x.bSubMenu).ToList()

                If RelevantButtons.Count > 0 Then
                    oHtml.AppendLine("<div class='accordion col-lg-5' id='accordionPanels2'>")
                    oHtml.AppendLine("<div class='my-2 accordion-item'>")

                    'oHtml.AppendLine("<div class='my-3 gap-3 d-flex MenuColumnHeader'>
                    '                 <img class='grid-icon' src='./assets/images/025-left-arrow-4.svg'>
                    '                 <h3 class='my-auto'>יישומים</h3>
                    '                 </div>")

                    'oHtml.AppendLine($"<h3 class='accordion-header' id='card-heading2'>
                    '           <button class='accordion-button Menu-accordion-button' type='button' data-bs-toggle='collapse' data-bs-target='#Card2' aria-expanded='True' aria-controls='card2'>
                    '                      <img class='mx-2 grid-icon' src='./assets/images/025-left-arrow-4.svg'>
                    '                      <span>יישומים</span>
                    '                    </button>
                    '             </h3>")
                    Dim AppsIcon As String = $"./assets/images/{If(bp.getProperty("AppsIcon") <> "", bp.getProperty("AppsIcon"), "application.svg")}"

                    oHtml.AppendLine($"<h3 class='accordion-header' id='card-heading2'>
			                            <button class='accordion-button Menu-accordion-button' type='button' data-bs-toggle='collapse' data-bs-target='#Card2' aria-expanded='True' aria-controls='card2'>
                                          <img class='mx-2 grid-icon' src={AppsIcon}>") 'ntg 14.11.23 - added a check if it is a company with special design, and change the menu logo accordingly
                    If bEnglish Then
                        oHtml.AppendLine($"<span>Applications</span>") 'ntg 31.01.23
                    Else oHtml.AppendLine($"<span>יישומים</span>")
                    End If
                    oHtml.AppendLine($"</button> </h3>")

                    oHtml.AppendLine("<div class='accordion-collapse collapse show' id='Card2'>
							          <div class='accordion-body'>")

                    CreateMenuItems(oHtml, bp, bSecondHalf, RelevantButtons)

                    oHtml.AppendLine("</div>") 'accordion-body
                    oHtml.AppendLine("</div>") 'accordion-collapse

                    oHtml.AppendLine("</div>") 'accordion-item
                    oHtml.AppendLine("</div>") 'accordion
                End If

            Else
                CreateMenuItems(oHtml, bp, bSecondHalf, MenuItemsRelevant) ' בניית הכותרת של מסך התפריטים
            End If
        Next

        oHtml.AppendLine("</div>") ' div class="row"
        oHtml.AppendLine("</div>") ' menu-container
    End Sub

    Private Sub CreateMenuItems(ByRef oHtml As StringBuilder, ByRef bp As BuildPage, ByRef bSecondHalf As Boolean, MenuItemsRelevant As List(Of MenuLine))
        For Each oMenuLine As MenuLine In MenuItemsRelevant

            Dim bCurTitle As Boolean = False

            Dim oControl As Control = New Control()

            If oMenuLine.sLink > "" Then
                oControl.Add("onclick", "window.open('" + oMenuLine.sLink + "');")
            ElseIf oMenuLine.sN > "" Then
                oControl.Add("onclick", "document.getElementById('Hx').value='';")
                oControl.Add("onclick", "MenuClick('" + oMenuLine.sN + "');")
            Else
                bCurTitle = True
                bSecondHalf = True
            End If

            oControl.Add("type", "button")
            oControl.Add("onclick", "return false;")

            If bCurTitle And Not oMenuLine.bIsMenuShortcut Then 'rk 11.08.24 - add to ignore OP for all other menu items
                oHtml.Append("<div class=""title-header"">").Append(oMenuLine.sText).Append("</div>").Append(vbCrLf)
                oHtml.AppendLine("</div>") ' div class="title"
                oHtml.AppendLine("<div class='row'>") 'justify-content-around
            ElseIf Not bSecondHalf And Not oMenuLine.bIsMenuShortcut Then 'rk 11.08.24 - add to ignore OP for all other menu items
                oControl.Add("value", oMenuLine.sText)
                oHtml.AppendLine("<div class=""title-header"">")
                oHtml.Append(oControl.FullCodeSpan(bp,, "button", False)).Append("</div>").Append(vbCrLf)
            ElseIf Not oMenuLine.bIsMenuShortcut Then 'rk 11.08.24 - add to ignore OP for all other menu items
                oHtml.Append(MenuButton(bp, oControl, oMenuLine, "", "")) ' "grid-link grid-link-menu",$"<img class='grid-icon' src='./assets/images/023-left-arrow-black.svg'>"))
            End If

            oControl.Dispose()
            oControl = Nothing
        Next
    End Sub

    Private Sub BooUsual(ByRef oHtml As StringBuilder, ByRef bp As BuildPage, NarrowSideBar As Boolean, IsModalWindow As Boolean)

        Dim cardNumber As Integer = 1

        UseAccordion = (bp.getProperty("UseCollapseCards") = "true")
        oHtml.Append("<!-- BodyDiv -->").Append(vbCrLf)
        oHtml.AppendLine($"<div id='BodyDiv' class='col mx-1 py-2 px-0'>")
        oHtml.AppendLine($"<div id='BodyContainer' class='container-fluid' style='{If(bp.m_bStatementWindow, "width:fit-content;", "")}'>") 'vk 09.24

        If sPageTitle > "" And (IsModalWindow = False And m_ModalScreen = "") Then 'ntg 22.06.23 show the page header only if its not a modal window
            oHtml.AppendLine("<div class=""page-header"">")
            oHtml.Append("<h2 class='my-2 page-title text-right'>").Append(sPageTitle).Append("</h2>").Append(vbCrLf)
            oHtml.AppendLine("</div>")
        End If

        'oHtml.AppendLine("<div class=""row"">") todo
        'oHtml.AppendLine("<div class=""col-4"">") 'ntg 9.1.23, was col-12 todo

        'If bp.EwrErrorText <> "" Then

        '    oHtml.AppendLine($"{bp.EwrErrorText}") 'ntg 16.1.23
        'End If


        Dim IsFgr1 = False 'ntg 19.02.23
        If oNodeScreen.ParamIn("fgr", "1") Then
            IsFgr1 = True
        End If

        Dim strc As Integer
        If oNodeScreen.ParamIn("strc", "2") Then
            strc = 2
        ElseIf oNodeScreen.ParamIn("strc", "3") Then
            strc = 3
        Else
            strc = 1
        End If

        If bp.m_sPdf <> "" Then
            CreatePdfCard(oHtml, bp)
        ElseIf bp.m_nCall = 1 AndAlso oNodeScreen.ParamIn("win", "TC") Then  ' Build text area
            qOnce.FormTextarea(bp, oHtml, oNodeScreen)
        Else
            Dim buttonType = CheckBtnHierarchy()
            Dim rowString = $"<div Class='row {If(Not IsModalWindow, "accordion", "")}' id='accordionPanels' {If(IsModalWindow, "style=""padding: 1rem;justify-content: left;""", "")} > {If(IsModalWindow, $"<button class='exitModal footerButton px-0 btn btn-sm' id='C{buttonType}' name='C{buttonType}' onblur=""if(NotReady('blur'))return;fnBtnBlur(this);"" title='יציאה' pch='{buttonType}' type='button' onclick=""if(NotReady('click'))return;fnBtnClick(this);"" onfocus=""if(NotReady('focus'))return;fnBtnFocus(this);bDontSelect=false;"" onmouseenter=""if(NotReady('mouseenter'))return;fnBtnEnter(this,false);"" {If(buttonType = "00", "style=""display:none;""", "")}><i id='exitBtnModal' class='far fa-times-circle'></i> </button>", "")}"
            If UseAccordion Then oHtml.AppendLine(rowString)
            CardCounter = 1

            If nMaxRect > 0 Then
                Dim nLine, nColumn As Integer
                For nLine = 1 To nMaxLine
                    For nColumn = nColFrom To nColTo Step nColStep
                        For nCard = 1 To nMaxRect
                            Dim oRect As Node = cRectangles(nCard.ToString)
                            If oRect.ParamVal("rx1") = nColumn AndAlso oRect.ParamVal("ry1") = nLine Then

                                OneCard(oHtml, bp, nCard, True, cardNumber, IsModalWindow, IsFgr1, IsQtyp3, nMaxRect, strc)
                                'oHtml.Append("
                                '            <script>
                                '                window.onload = function() {
                                '                    retrieveElementHeight();
                                '                };
                                '            </script>
                                '            ")

                            End If
                        Next
                    Next
                Next
            End If

            'If Not oNodeScreen.Param("win") = "R" Then   'ntg 06.03.23
            For nCard = 0 To nMaxCard
                OneCard(oHtml, bp, nCard, False, cardNumber, IsModalWindow, IsFgr1, IsQtyp3, nMaxCard, strc)
            Next
            'End If    'ntg 06.03.23

            If UseAccordion Then oHtml.AppendLine("</div>")
        End If

        oHtml.AppendLine("</div>")
        oHtml.AppendLine("</div>")
        'If Not IsModalWindow Then

        '    Dim langOfComtecLtd = $"{If(bEnglish, "Comtec Ltd", "קומטק בע&quot;מ ")}"
        '    Dim langOfAllRightsRes = $"{If(bEnglish, "All Rights Reserved", "כל הזכויות שמורות")}"
        '    Dim FontfCopyRight = "<FONT COLOR=""#FF4500"">&copy;</FONT>"
        '    oHtml.AppendLine($"<div id='CopyRightDiv' style='left: {If(bEnglish, "108rem", "1rem")}'>")  'ntg 10.1.23

        '    oHtml.AppendLine($"<a href=""http://www.comtecglobal.com"" target=""_blank"" tabindex=""-1"">{langOfComtecLtd} {FontfCopyRight} {langOfAllRightsRes}</a>")  'ntg 01.02.23

        '    'oHtml.AppendLine(bp.getProperty("Copyright")).Append(vbCrLf)  'ntg 10.1.23
        '    oHtml.AppendLine("</div>")  'ntg 10.1.23
        'End If
        oHtml.Append("<!-- end of BodyContainer -->").Append(vbCrLf)

        oHtml.AppendLine("</div>") ' End of BodyContainer

        'oHtml.AppendLine("<div id='CopyRightDiv'>") - moved into footer - ntg 10.1.23
        'oHtml.AppendLine(bp.getProperty("Copyright")).Append(vbCrLf) - moved into footer - ntg 10.1.23
        'oHtml.AppendLine("</div>") - moved into footer - ntg 10.1.23

        oHtml.Append("<!-- end of body -->").Append(vbCrLf)
        oHtml.AppendLine("</div>") ' End of BodyDiv



        oHtml.Append("<!-- end of Row -->").Append(vbCrLf)
        oHtml.AppendLine("</div>") ' End of Row

    End Sub
    Sub CreatePdfCard(ByRef oHtml_Param As StringBuilder, ByRef bp As BuildPage)
        Dim oHtml As New StringBuilder

        oHtml.Append("<!-- rect-").Append(nCard.ToString).Append(" -->").Append(vbCrLf)
        oHtml.Append("<div class=""card"">").Append(vbCrLf)

        Dim IconElement As String = $"<i class='far fa-file-pdf' style='color:red;' ></i>"
        Dim PdfCardTitle As String = $" {bp.getProperty("PdfTitle")} "

        oHtml.Append("<div class='card-header' >").Append(vbCrLf)

        oHtml.Append("<h4 class=""card-title"">").Append(IconElement).Append(PdfCardTitle).Append("</h4>").Append(vbCrLf)
        oHtml.AppendLine("</div>")

        oHtml.AppendLine("<div class=""card-body"">")
        oHtml.AppendLine("<div class=""row"">")
        oHtml.AppendLine("<div class=""col-12 col-md-12"">")
        oHtml.AppendLine($"<div style='text-align:center;'> {bp.getProperty("PdfDisclaimer")} </div>")
        oHtml.AppendLine($"<iframe id='acrobat' src='{bp.m_sPdf}' type='application/pdf' width='100%' height='800px'>")
        oHtml.AppendLine("<p>")
        oHtml.AppendLine($"{bp.getProperty("PdfMissingExtension")}")
        oHtml.AppendLine($"<a href='{bp.m_sPdf}'>{bp.getProperty("PdfDownload")}</a>")
        oHtml.AppendLine("</p>")
        oHtml.AppendLine("</iframe>")

        oHtml.AppendLine("</div>").AppendLine("</div>").AppendLine("</div>").AppendLine("</div>")

        oHtml_Param.Append(oHtml)
    End Sub

    Private Function IsGridModal() As Boolean
        Return oNodeScreen.Param("win") = "S" AndAlso oNodeScreen.Param("fgr") = "G"
    End Function
    Sub OneCard(ByRef oHtml_Param As StringBuilder, ByRef bp As BuildPage, nCard As Integer, bRect As Boolean, ByRef cardNumber As Integer, IsModalWindow As Boolean, IsFgr1 As Boolean, IsQtyp3 As Boolean, LastCard As Integer, strc As Integer)
        Dim oHtml As New StringBuilder
        Dim bEmptyCard As Boolean = True
        Dim ExtendedCardType As String = " card "
        Dim CardTitle = ""
        Dim IconElement As String = ""
        Dim endOfRow As String = ""
        'vk 09.24 from here
        Dim bBorder As Boolean = False
        If UseAccordion Then
            If CardTitlesNodes.Contains(nCard.ToString) AndAlso Not bRect Then
                Dim CardTitleNode As Node = CardTitlesNodes(nCard.ToString)
                If CardTitleNode.Param("pul") = "F" Then
                    bBorder = True
                End If
            End If
            'vk 09.24 till here
            If IsFgr1 Then
                If nCard = LastCard Then
                    oHtml.AppendLine($"<div class='col-12 marTopBotBetweenCards accordion-item{If(bBorder, " border2", "")} cardNumber_{cardNumber}' style='flex-grow:1'>") 'ntg 19.02.23 - the last card will be a grid probably / ntg 07.02.24 adding spacing between lines with marTopBotCardFields
                Else
                    If nCard Mod 3 = 1 Then
                        oHtml.AppendLine("<div class='rowStart'>")
                    End If

                    oHtml.AppendLine($"<div class='marTopBotBetweenCards accordion-item{If(bBorder, " border2", "")} cardNumber_{cardNumber}'  style='flex-grow:1; width:50%'>") 'ntg 19.02.23 '' / ntg 07.02.24 adding spacing between lines with marTopBotCardFields

                    If nCard Mod 3 = 0 Then
                        endOfRow = "</div>"
                    Else endOfRow = ""
                    End If
                End If
            ElseIf strc = 2 OrElse strc = 3 Then
                If nCard = 1 Then
                    ' skip, this is the header card
                ElseIf nCard = 2 Then
                    oHtml.AppendLine($"<div class='rowStart'>")
                    oHtml.AppendLine($"<div class='marTopBotBetweenCards accordion-item{If(bBorder, " border2", "")} cardNumber_{cardNumber}' style='flex:1'>")
                ElseIf nCard > 2 AndAlso strc = 3 Then
                    If (nCard - 2) Mod 3 = 1 Then
                        oHtml.AppendLine($"<div class='rowStart'>")
                        oHtml.AppendLine($"<div class='marTopBotBetweenCards accordion-item{If(bBorder, " border2", "")} cardNumber_{cardNumber}' style='flex:1'>")
                    ElseIf (nCard - 2) Mod 3 = 2 Then
                        oHtml.AppendLine($"<div class='marTopBotBetweenCards accordion-item{If(bBorder, " border2", "")} cardNumber_{cardNumber}' style='flex:1'>")
                    Else
                        oHtml.AppendLine($"<div class='marTopBotBetweenCards accordion-item{If(bBorder, " border2", "")} cardNumber_{cardNumber}' style='flex:1'>")
                    End If
                ElseIf nCard > 2 AndAlso strc = 2 Then
                    If (nCard - 2) Mod 2 = 1 Then
                        oHtml.AppendLine($"<div class='rowStart'>")
                        oHtml.AppendLine($"<div class='marTopBotBetweenCards accordion-item{If(bBorder, " border2", "")} cardNumber_{cardNumber}' style='flex:1'>")
                    Else
                        oHtml.AppendLine($"<div class='marTopBotBetweenCards accordion-item{If(bBorder, " border2", "")} cardNumber_{cardNumber}' style='flex:1'>")
                    End If
                ElseIf nCard = 0 Then
                    ' skip, this is a redundant card at the end of the cards list
                Else
                End If
            ElseIf IsQtyp3 AndAlso Not bRect Then 'ntg 28.02.24 creating the layout of the special screen according to given param qtyp in xml
                If nCard Mod 3 = 0 Then
                    oHtml.AppendLine("<div class='rowStart'>")
                End If
                oHtml.AppendLine($"<div class='marTopBotBetweenCards accordion-item{If(bBorder, " border2", "")} cardNumber_{cardNumber}' style='flex-grow:1'>")

                If nMaxCard = nCard OrElse nCard Mod 3 = 2 Then
                    endOfRow = "</div>"
                Else endOfRow = ""
                End If
            ElseIf IsQtyp2 AndAlso Not bRect Then 'ntg 04.08.24 creating the layout of the special screen according to given param qtyp in xml
                If nCard Mod 2 = 0 Then
                    oHtml.AppendLine("<div class='rowStart'>")
                End If
                oHtml.AppendLine($"<div class='marTopBotBetweenCards accordion-item{If(bBorder, " border2", "")} cardNumber_{cardNumber}' style='flex-grow:1'>")

                If nMaxCard = nCard OrElse nCard Mod 2 = 1 Then
                    endOfRow = "</div>"
                Else
                    endOfRow = ""
                End If
            ElseIf (IsQtyp3 OrElse IsQtyp2) AndAlso bRect Then 'ntg 04.08.24 added IsQtyp2 - todo - combine to one param
                oHtml.AppendLine("<div class='rowStart'>")
                oHtml.AppendLine($"<div class='marTopBotBetweenCards accordion-item{If(bBorder, " border2", "")} cardNumber_{cardNumber}' style='flex-grow:1'>")
            Else
                oHtml.AppendLine($"<div class='col-12 marTopBotBetweenCards accordion-item{If(bBorder, " border2", "")} cardNumber_{cardNumber}'>")
            End If
            cardNumber += 1
        End If
        If bRect Then

            For nLine = 1 To nMaxLine
                Dim sLine As String = nCard.ToString & "_" & nLine.ToString
                If cDdsLines.Contains(sLine) Then
                    bEmptyCard = False
                    Exit For
                End If
            Next

            oHtml.Append("<!-- rect-").Append(nCard.ToString).Append(" -->").Append(vbCrLf)
            Dim oRect As Node = cRectangles(nCard.ToString)

            If oRect.ParamExists("ficn") Then
                Dim IconNumber As String = oRect.Param("ficn")
                Dim IconColorNumber As String = oRect.Param("rcl")

                Dim IconName As Object = bp.m_colColor.Item($"Icon_{CInt(IconNumber)}")

                Dim iconColor As Object = bp.m_colColor.Item($"clr_{IconColorNumber}")

                Dim ColorStyle = ""
                If iconColor IsNot Nothing Then
                    ColorStyle = $"style='color:{iconColor};'"
                End If

                If IconName IsNot Nothing Then
                    IconElement = $"<i class=""fas {IconName} {If(oNodeScreen.Param("fgr") <> "1", "sideIcon px-3", "")}"" {ColorStyle} ></i>" 'remove 'fas' if we dont want thick always
                End If
            End If

            Dim toCollapseCard = oRect.ParamExists("opn")
            If UseAccordion Then
                Dim ButtonText As String = If(oRect.Param("ftit") > "", oRect.Param("ftit"), $"  ")

                If oNodeScreen.Param("fgr") = "1" Or oNodeScreen.Param("fsid") = "1" Then 'ntg 22.1.23
                    oHtml.AppendLine($"<h3 id='card-heading{If(IsModalWindow, cardNumber, "")}{CardCounter}' style=""margin-top: 0.5rem;margin-right: 1rem;font-weight:bold"">
                                         {IconElement}&nbsp; {ButtonText}
		                               </h3>")
                Else
                    'oHtml.AppendLine($"<h3 class='accordion-header{If(bBorder, " border1", "")}' id='card-heading{If(IsModalWindow, cardNumber, "")}{CardCounter}'>
                    '          <button class='{If(IsModalWindow, "accordion-button-modal", "accordion-button")} {If(toCollapseCard, "collapsed", "")}' type='button' data-bs-toggle={If(IsModalWindow, "", "collapse")} data-bs-target='#Card{If(IsModalWindow, cardNumber, "")}{CardCounter}' aria-expanded={If(toCollapseCard, "false", "true")} aria-controls='card{If(IsModalWindow, cardNumber, "")}{CardCounter}'>
                    '                     {IconElement}&nbsp; {ButtonText}
                    '                   </button>
                    '            </h3>")
                    oHtml.AppendLine($"<h3 class='accordion-header' id='card-heading{If(IsModalWindow, cardNumber, "")}{CardCounter}'>
                               <button class='{If(IsModalWindow, "accordion-button-modal", "accordion-button")} {If(toCollapseCard, "collapsed", "")}' type='button' data-bs-toggle={If(IsModalWindow, "", "collapse")} data-bs-target='#Card{If(IsModalWindow, cardNumber, "")}{CardCounter}' aria-expanded={If(toCollapseCard, "false", "true")} aria-controls='card{If(IsModalWindow, cardNumber, "")}{CardCounter}'>
                                          {IconElement}&nbsp; {ButtonText}
                                        </button>
                                 </h3>")
                End If
                ExtendedCardType = " accordion-collapse collapse "

                If bp.getProperty("AllCardsOpen") = "true" And toCollapseCard Then
                    ExtendedCardType += "  "
                Else
                    ExtendedCardType += " show " 'hide todo
                End If
                CardTitle = ButtonText
            End If


            oHtml.Append($"<div class=' {ExtendedCardType} ' id='Card{If(IsModalWindow, cardNumber, "")}{CardCounter}'>").Append(vbCrLf)
            If oRect.Param("ftit") > "" Then
                If Not UseAccordion Then
                    oHtml.Append("<div class=""card-header"">").Append(vbCrLf)
                    oHtml.Append("<h3 class=""card-title"">").Append(IconElement).Append(oRect.Param("ftit")).Append("</h3>").Append(vbCrLf)
                    oHtml.Append("</div>").Append(vbCrLf)
                    CardTitle = oRect.Param("ftit")
                End If
            End If
        Else
            oHtml.Append("<!-- card-").Append(nCard.ToString).Append(" -->").Append(vbCrLf)

            Dim StrongTypedCard As Boolean = False

            ' בדיקה - במקרה של מסך גמיש - להעלים את החלק הקשיח בתצוגת מובייל

            Dim Onode As Node = Nothing

            For nLine = 1 To nMaxLine
                Dim sLine As String = nLine.ToString
                If cDdsLines.Contains(sLine) Then
                    oDdsLine = cDdsLines(sLine)
                    'If oDdsLine.nCard = nCard Then
                    If oDdsLine.nCard = nCard AndAlso oDdsLine.cNodes.Count > 0 Then 'ntg 02.04.24 vladi's fix for sheiltat z/x
                        Onode = oDdsLine.cNodes(1)
                        bEmptyCard = False
                        If bp.getProperty("HideStrongTypedCard") = "true" AndAlso qOnce.IsFlexiblePage(bp.m_sXmlFil.Trim(), bp.m_sXmlRec.Trim()) AndAlso Onode.ParamVal("lin") <> 0 Then
                            StrongTypedCard = True
                        End If
                        Exit For
                    End If
                End If
            Next

            If Not bEmptyCard Then
                If StrongTypedCard Then
                    ExtendedCardType = " HideOnMobile "
                End If

                IconElement = ""

                If Not bEmptyCard AndAlso CardTitlesNodes.Contains(nCard.ToString) Then
                    Dim CardTitleNode As Node = CardTitlesNodes(nCard.ToString)
                    If CardTitleNode IsNot Nothing AndAlso (CardTitleNode.ParamExists("icn") OrElse CardTitleNode.ParamExists("pic")) Then
                        'vk 09.24 minor correction
                        Dim IconNumber As String = If(CardTitleNode.ParamExists("icn"), CardTitleNode.Param("icn"), CardTitleNode.Param("pic"))
                        Dim IconColorNumber As String = If(CardTitleNode.ParamExists("fcl"), CardTitleNode.Param("fcl"), CardTitleNode.Param("pcl"))

                        Dim IconName As Object = bp.m_colColor.Item($"Icon_{CInt(IconNumber)}")

                        Dim iconColor As Object = bp.m_colColor.Item($"clr_{IconColorNumber}")

                        Dim ColorStyle = ""
                        If iconColor IsNot Nothing Then
                            ColorStyle = $"style='color:{iconColor};'"
                        End If

                        If IconName IsNot Nothing Then
                            IconElement = $"<i class=""fas {IconName} px-3 sideIcon"" {ColorStyle}></i>"
                        End If
                    End If
                End If

                If UseAccordion Then
                    Dim ButtonText As String = If(cCardTitles.Contains(nCard.ToString), cCardTitles(nCard.ToString), $"  ")

                    If oNodeScreen.Param("fgr") = "1" Or oNodeScreen.Param("fsid") = "1" Then 'ntg 22.1.23
                        'If qtyp2 = True Or oNodeScreen.Param("fsid") = "1" Then 'ntg 22.1.23
                        oHtml.AppendLine($"<h2 id='card-heading{If(IsModalWindow, cardNumber, "")}{CardCounter}'>
                                          {IconElement} {ButtonText}
		                               </h2>")
                    Else
                        'oHtml.AppendLine($"<h2 class='accordion-header{If(bBorder, " border1", "")}' id='card-heading{If(IsModalWindow, cardNumber, "")}{CardCounter}'>
                        '       <button class={If(IsModalWindow, "accordion-button-modal", "accordion-button")} type='button' data-bs-toggle={If(IsModalWindow, "", "collapse")} data-bs-target='#Card{If(IsModalWindow, cardNumber, "")}{CardCounter}' aria-expanded='True' aria-controls='card{If(IsModalWindow, cardNumber, "")}{CardCounter}'>
                        '                    {IconElement}&nbsp; {ButtonText}
                        '                </button>
                        '         </h2>")
                        oHtml.AppendLine($"<h2 class='accordion-header' id='card-heading{If(IsModalWindow, cardNumber, "")}{CardCounter}'>
                               <button class={If(IsModalWindow, "accordion-button-modal", "accordion-button")} type='button' data-bs-toggle={If(IsModalWindow, "", "collapse")} data-bs-target='#Card{If(IsModalWindow, cardNumber, "")}{CardCounter}' aria-expanded='True' aria-controls='card{If(IsModalWindow, cardNumber, "")}{CardCounter}'>
                                            {IconElement}&nbsp; {ButtonText}
                                        </button>
                                 </h2>")
                    End If


                    ExtendedCardType += " accordion-collapse collapse "

                    If bp.getProperty("AllCardsOpen") = "true" Then
                        ExtendedCardType += " show "
                    Else
                        ExtendedCardType += " show " 'hide
                    End If

                    CardTitle = ButtonText
                End If

                oHtml.AppendLine($"<div class=' {ExtendedCardType} ' id='Card{If(IsModalWindow, cardNumber, "")}{CardCounter}'>")
                If cCardTitles.Contains(nCard.ToString) Then
                    If Not UseAccordion Then
                        oHtml.Append("<div class=""card-header"">").Append(vbCrLf)
                        oHtml.Append("<h3 class=""card-title"">").Append(IconElement).Append(cCardTitles(nCard.ToString)).Append("</h3>").Append(vbCrLf)
                        oHtml.Append("</div>").Append(vbCrLf)
                        CardTitle = cCardTitles(nCard.ToString)
                    End If
                End If
            End If
        End If

        Dim SingleImageInCard As Boolean = False

        oHtml.AppendLine($"<div class={If(SingleImageInCard, " 'text-center' ", "")} {If(UseAccordion, " 'accordion-body' ", " 'card-body' ")}>")

        Dim bTableMode As Boolean = False

        Dim ButtonList As List(Of Node) = New List(Of Node)

        'ntg 18.06.24 this loop will count the number of table rows, which later be used to highlight the final line if it has parameters pbg and phi in the xml
        If actualTableLinesCounter = 0 Then
            For nLine = 1 To nMaxLine
                If cDdsLines.Contains(nLine.ToString) AndAlso yL.Contains(nLine) Then
                    If firstHardTableRow = 0 AndAlso Not nLine.ToString.Contains("_") Then firstHardTableRow = nLine
                    actualTableLinesCounter += 1
                End If
            Next
        End If

        For nLine = 1 To nMaxLine
            Dim sLine As String = nLine.ToString
            If bRect Then
                sLine = nCard.ToString & "_" & sLine
            End If
            If cDdsLines.Contains(sLine) Then
                oDdsLine = cDdsLines(sLine)
                If oDdsLine.nCard = nCard OrElse bRect Then

                    'ntg 17.06.24 vladi changes regarding sub-title and accordion in card
                    If oDdsLine.cNodes.Count = 1 Then
                        Dim oNodeTmp As Node = oDdsLine.cNodes(1)
                        If oNodeTmp.Param("pul") = "A" Then
                            If bp.m_bInSubCard Then
                                oHtml.Append("<!-- subcard end --></div></div>" & vbCrLf)
                            Else
                                bp.m_bInSubCard = True
                            End If
                            oHtml.Append("<!-- subcard begin --><div>" & vbCrLf)
                        End If
                    End If


                    bEmptyCard = False
                    Dim sPattern As String = oDdsLine.sPattern
                    Dim CountNonEmptyLabels = oDdsLine.LabelsList.AsEnumerable.Count(Function(obj) obj.Param("val").Trim() <> "") ' מספר התויות/כותרות שהערך שלהם שונה מריק- בודקים שמספר הכותרות גדול מ1

                    Dim LabelsRow = (sPattern = "".PadRight(Len(sPattern), "L")) AndAlso sPattern <> "L" AndAlso CountNonEmptyLabels > 1 AndAlso Not bTableMode

                    If LabelsRow OrElse yT.Contains(nLine) Then

                        If LabelsRow OrElse nLine = nGridStart Then
                            bTableMode = True
                            CombinedHeadersDict = New Dictionary(Of Integer, String)
                            oHtml.Append("<!-- table-responsive -->").Append(vbCrLf)
                            '                            oHtml.Append("<script>
                            '        document.addEventListener('DOMContentLoaded', function() {
                            '            const table = document.querySelector('.table');
                            '            table.addEventListener('click', function(event) {
                            '                if (event.target.tagName === 'TD') {
                            '                    const row = event.target.parentNode;

                            '                    // Check if Ctrl key is pressed
                            '                    if (event.ctrlKey) {
                            '                        row.classList.toggle('mulSelection');
                            'const RadioButtonObj = row.querySelector("".form-check-input"");

                            '                if (RadioButtonObj) {
                            '                    RadioButtonObj.checked = !RadioButtonObj.checked;
                            '                    fnFocusSL(RadioButtonObj);
                            '                }
                            '                    } else {
                            '                        // Clear previous selections
                            '                        Array.from(table.getElementsByClassName('trSelected')).forEach(function(selectedRow) {
                            '                            selectedRow.classList.remove('trSelected');
                            '                        });
                            '                        Array.from(table.getElementsByClassName('mulSelection')).forEach(function(selectedRow) {
                            '                            selectedRow.classList.remove('mulSelection');
                            '                        });

                            '                        // Add trSelected to the clicked row
                            '                        row.classList.add('trSelected');
                            '                    }
                            '                }
                            '            });
                            '        });
                            '    </script>")
                            oHtml.AppendLine("<div class='table-responsive'>")


                            If oNodeScreen.Param("fgr") = "B" Then
                                Dim actionPopover As String = createActionsPopover(bp) 'ntg 23.06.24 creating the top section of "select multiple" in a table on fgr=B screens
                                oHtml.AppendLine(actionPopover)
                            End If

                            oHtml.AppendLine($"<table class=' {If(bp.getProperty("SortTables") = "true", "sortable", "")} table table-hover table-bordered table-vcenter text-nowrap card-table {If(oNodeScreen.Param("fgr") = "P", "table-first-fit", "")} '>")
                            oHtml.Append("<thead>").Append(vbCrLf)
                        End If

                        'vk rem 02.24
                        'If CType(oDdsLine.cNodes(1), Node).Param("ewr") = "H" Then ' Special long table header
                        '    oHtml.Append(SpecialLongHeader(bp, Heb(CType(oDdsLine.cNodes(1), Node).Param("val"))))
                        'End If


                        oHtml.Append(TR(bp, "th", nLine))

                        If (yT.nTo = 0 AndAlso LabelsRow) OrElse nLine = yT.nTo Then ' Close the headers section 
                            oHtml.Append("</thead>").Append(vbCrLf)
                            'oHtml.Append("<select>").Append(vbCrLf)
                            oHtml.Append("<tbody>").Append(vbCrLf)
                        End If
                    Else
                        If bTableMode AndAlso yL.IsAfter(nLine) Then
                            bTableMode = False
                            oHtml.Append("</tbody>").Append(vbCrLf)
                            'oHtml.Append("</select>").Append(vbCrLf)
                            oHtml.Append("</table>").Append(vbCrLf)
                            oHtml.Append("<!-- table-responsive end-->").Append(vbCrLf)

                            oHtml.Append("</div>").Append(vbCrLf)
                        End If
                        If bTableMode Then
                            oHtml.Append(TR(bp, "td", nLine)) '"col d-flex justify-content-start"))
                        Else
                            oHtml.Append("<div class=""row"">").Append(vbCrLf)

                            Dim cGroups As New Collection
                            Dim cGroup As List(Of Node) = Nothing
                            Dim n As Integer
                            If bEnglish Then
                                n = 0
                            Else
                                n = Len(sPattern) + 1
                            End If
                            'Dim ColFrom, ColTo As Integer
                            'Dim MinimumCol As Integer = oDdsLine.ColList.Min()
                            'Dim MaximumCol As Integer = oDdsLine.ColList.Max()

                            'If bEnglish Then
                            '    ColFrom = MinimumCol
                            '    ColTo = MaximumCol
                            'Else
                            '    ColFrom = MaximumCol
                            '    ColTo = MinimumCol
                            'End If
                            ' For nColumn = ColFrom To ColTo Step nColStep
                            For nColumn = nColFrom To nColTo Step nColStep
                                If oDdsLine.cNodes.Contains(nColumn.ToString) Then
                                    If bEnglish Then
                                        n += 1
                                    Else
                                        n -= 1
                                    End If
                                    Dim NodeType As String = Mid(sPattern, n, 1)
                                    oNode = oDdsLine.cNodes(nColumn.ToString)
                                    oNode.NodeType = NodeType

                                    Dim isSideBarField As Boolean = oNode.Param("ewr") = "S" AndAlso oNode.Param("pf4") Is Nothing AndAlso {"A", "S", "N"}.Contains(oNode.Param("typ")) 'ntg 21.07.24 changes regarding sidebar fields

                                    If (NodeType = "L" OrElse cGroups.Count = 0) AndAlso Not isSideBarField Then 'ntg 21.07.24 changes regarding sidebar fields
                                        cGroup = New List(Of Node)
                                        cGroups.Add(cGroup)
                                    End If

                                    If oNode.ParamExists("pchl") Then
                                        ButtonList.Add(oNode)
                                    Else
                                        If NodeType = "F" AndAlso oNode.ParamExists("alias") AndAlso Not isSideBarField Then 'ntg 21.07.24 changes regarding sidebar fields
                                            If cGroup.Count = 0 OrElse cGroup.Last().NodeType = "F" Then
                                                cGroup = New List(Of Node)
                                                cGroups.Add(cGroup)

                                                Dim NewLabelField As New Node() ' Creating a label before the field with the value of the 'alias' 
                                                NewLabelField.Param("val") = Heb(oNode.Param("alias"))
                                                NewLabelField.Param("col") = If(bEnglish, oNode.ParamVal("col") - 1, oNode.ParamVal("col") + 1)
                                                NewLabelField.NodeType = "L"
                                                cGroup.Add(NewLabelField)
                                            Else
                                                ' Previoud Field is a label
                                                cGroup.Last().Param("val") = oNode.Param("alias")
                                            End If

                                        End If
                                        If Not isSideBarField Then cGroup.Add(oNode)
                                    End If
                                End If
                            Next

                            For Each cGroup In cGroups
                                If cGroup.Count = 0 Then
                                    Continue For
                                End If

                                If cGroup.Count = 1 AndAlso cGroup(0).NodeType = "L" AndAlso cGroup(0).Param("val") = "" Then 'לא להציג לבל ריק
                                    Continue For
                                End If

                                Dim CheckIfRadioInGroup As Boolean = False
                                Dim bDatePicker As Boolean = False
                                Dim bWideGraph As Boolean = False
                                Dim bImageButtons As Boolean = False
                                Dim HalfScreenWidth As Boolean = False
                                Dim ThirdScreenWidth As Boolean = False
                                Dim SixthScreenWidth As Boolean = False 'ntg 11.02.24

                                Dim GroupPattern As String = String.Join("", cGroup.Select(Function(x) x.NodeType))

                                If cGroups.Count = 1 Then
                                    bp.singleFieldInLine = True
                                    Dim oNode As Node
                                    For Each oNode In cGroup
                                        If oNode.ForBoo("graph") Then bWideGraph = True
                                        If oNode.ParamVal("psz") > 0 Then bImageButtons = True ' if there are big buttons with images then Wide
                                    Next
                                    If cGroup.Count <= 2 Then
                                        For Each oNode In cGroup
                                            If oNode.ParamVal("len") >= 31 AndAlso oNode.ParamVal("len") <= 40 Then
                                                ThirdScreenWidth = True
                                            End If

                                            If oNode.ParamIn("cry", "GI") Then 'BuildOption as grid
                                                ThirdScreenWidth = True
                                            End If

                                            If oNode.ParamVal("len") >= 41 Then
                                                HalfScreenWidth = True
                                            End If
                                        Next
                                    End If
                                Else
                                    bp.singleFieldInLine = False
                                End If

                                For Each oNode In cGroup
                                    CheckIfRadioInGroup = CheckIfRadioInGroup OrElse CheckIfNodeIsRadio(oNode)
                                    If oNode.ForBoo("DatePicker") Then bDatePicker = True
                                Next

                                Dim _toolTipType As Integer = 0
                                If ThirdScreenWidth Then
                                    'oHtml.AppendLine("<div class=""col-12 col-md-6 col-lg-4 my-2 align-self-end"">")
                                    oHtml.AppendLine($"<div class=""col-12 col-md-6 col-lg-4 {If(bp.m_bStatementWindow, "", "marTopBotCardFields")} align-self-end"">") ' ntg 07.02.24 adding spacing between lines with marTopBotCardFields
                                Else
                                    If bWideGraph OrElse bImageButtons OrElse bp.IsMsgFile(oNodeScreen.Param("fil")) Then ' Full screen field
                                        'oHtml.AppendLine($"<div class='col-12 col-md-12 my-2 {If(bWideGraph, "mx-auto WideGrapDiv", "")}'>").Append(vbCrLf)
                                        oHtml.AppendLine($"<div class='col-12 col-md-12 {If(bp.m_bStatementWindow, "", "marTopBotCardFields")} {If(bWideGraph, "mx-auto WideGrapDiv", "")}'>").Append(vbCrLf) '/ ntg 07.02.24 adding spacing between lines with marTopBotCardFields
                                    Else
                                        If HalfScreenWidth Then
                                            'oHtml.AppendLine("<div class='col-12 col-md-6 my-2'>")
                                            oHtml.AppendLine($"<div class='col-12 col-md-6 {If(bp.m_bStatementWindow, "", "marTopBotCardFields")}'>") '/ ntg 07.02.24 adding spacing between lines with marTopBotCardFields
                                        Else
                                            If cGroup.Count <= 2 Then ' אם מדובר על מקסימום תוית ושדה אז יתפוס שישית מהמסך אחרת שליש מהמסך
                                                'oHtml.AppendLine($"<div class=""col-12 col-md-6 col-lg-{If(IsFgr1, "6", "2")} my-2 align-self-end "">") 'widthTrial
                                                If Not IsFgr1 Then 'ntg 11.02.24 
                                                    SixthScreenWidth = True
                                                End If
                                                Dim isSpecialDate As Boolean = oNode.Param("typ") = "B" 'ntg 05.03.24 for case of special date-picker

                                                Dim isThreeRadioBtns As Boolean 'ntg 05.03.24 class threeRadios for case of 3 radio buttons
                                                If oNode.Param("psl") = "G" Then isThreeRadioBtns = True Else isThreeRadioBtns = False
                                                If oNode.Param("cry") = "Y" Then isThreeRadioBtns = qCombo.CountLinesInCombo_ByTbl(bp, oNode, oNodeScreen) = 3

                                                Dim threeRadiosClass As String = If(isThreeRadioBtns, "threeRadios", "") 'ntg 04.08.24
                                                Dim isOpenTooltip As Boolean = cGroup(0).Param("hky") <> Nothing 'ntg 04.08.24 if this param is not nothing, then we have a tooltip 

                                                If cGroup(0).ParamExists("tip") Then
                                                    _toolTipType = cGroup(0).ParamVal("tip")
                                                ElseIf cGroup.Count > 1 AndAlso cGroup(1).ParamExists("tip") Then
                                                    _toolTipType = cGroup(1).ParamVal("tip")
                                                ElseIf cGroup(0).ParamExists("hky") AndAlso cGroup(0).Param("hky") <> Nothing AndAlso Not cGroup(0).ParamExists("tip") Then
                                                    _toolTipType = 1
                                                End If

                                                Dim columnSize As String = "2"
                                                If nCard = 2 Then
                                                    columnSize = "2"
                                                    If _toolTipType = 2 OrElse isSpecialDate OrElse isThreeRadioBtns Then
                                                        columnSize = "4"
                                                    End If
                                                Else
                                                    If (IsQtyp2 OrElse strc = 2) Then
                                                        columnSize = "4"
                                                        If (_toolTipType = 2 OrElse isSpecialDate OrElse isThreeRadioBtns) Then
                                                            columnSize = "4"
                                                        End If
                                                    ElseIf (IsFgr1 OrElse IsQtyp3 OrElse strc = 3) Then
                                                        columnSize = "6"
                                                        If (_toolTipType = 2 OrElse isSpecialDate OrElse isThreeRadioBtns) Then
                                                            columnSize = "6"
                                                        End If
                                                    Else
                                                        columnSize = "2"
                                                        If (_toolTipType = 2 OrElse isSpecialDate OrElse isThreeRadioBtns) Then
                                                            columnSize = "4"
                                                        End If
                                                    End If
                                                End If

                                                'oHtml.AppendLine($"<div class=""col-12 col-md-6 col-lg-{If((isSpecialDate OrElse isThreeRadioBtns) AndAlso IsQtyp2, "12", If(IsFgr1 OrElse IsQtyp2, "6", "2"))} marTopBotCardFields align-self-end {If(isThreeRadioBtns AndAlso (IsQtyp2 OrElse IsFgr1), "threeRadios", "")}"">")
                                                'oHtml.AppendLine($"<div class=""col-12 col-md-6 col-lg-{If((isSpecialDate OrElse isThreeRadioBtns) AndAlso (IsQtyp3 OrElse IsQtyp2), "12", If(IsFgr1 OrElse IsQtyp3, "6", If(isSpecialDate OrElse isThreeRadioBtns, "4", "2")))} marTopBotCardFields align-self-End {If(isThreeRadioBtns, "threeRadios", "")}"">") ' regular inp field
                                                oHtml.AppendLine($"<div class=""col-12 col-md-6 col-lg-{columnSize} {If(bp.m_bStatementWindow, "", "marTopBotCardFields")} align-self-End {threeRadiosClass}"">") 'ntg 04.08.24 simplifing all the different layouts of all types of fields inside cards

                                            Else
                                                'oHtml.AppendLine("<div class=""col-12 col-md-6 col-lg-4 my-2 align-self-end"">")
                                                oHtml.AppendLine($"<div class=""col-12 col-md-6 col-lg-4 {If(bp.m_bStatementWindow, "", "marTopBotCardFields")} align-self-end"">") ' / ntg 07.02.24 adding spacing between lines with marTopBotCardFields
                                            End If
                                        End If
                                    End If
                                End If


                                bp.m_InputId = ""
                                Dim FloatingLabels As Boolean = False
                                Dim FloatingClass As String = ""
                                Dim ToolTipProperties As String = ""
                                Dim LabelOverflow = False
                                Const MaximumLabelChars As Integer = 25 'ntg 11.02.24 changed from 22 to 25

                                FloatingLabels = (GroupPattern = "LFF" OrElse GroupPattern = "LF" OrElse GroupPattern = "FL")
                                FloatingClass = If(FloatingLabels, "floating-label ", "")

                                If GroupPattern = "LFF" AndAlso FloatingLabels Then
                                    Dim Group1 = cGroup.Take(2).Reverse().ToList()  ' In Floating label the input (Field) should be before the label
                                    Dim LabelValue As String = CType(Group1(1), Node).Param("val").Trim()

                                    If LabelValue.Length > MaximumLabelChars Then
                                        ToolTipProperties = $"data-bs-toggle='tooltip' title='{Heb(LabelValue)}'"
                                        ' LabelOverflow = True
                                    End If

                                    CType(Group1(0), Node).Param("placeholder") = "  "

                                    Dim Group2 = cGroup.Skip(2).ToList()
                                    oHtml.AppendLine("<div class='row'>")
                                    oHtml.AppendLine($"<div {ToolTipProperties} class='col-6 form-group d-flex {FloatingClass}' { If(bEnglish, "style='display: flex; flex-direction: row-reverse;'", "")}>")

                                    For Each oNode In Group1
                                        oHtml.Append(OneField(bp, oNode, bValueDisplay:=bWideGraph, labelOverflow:=LabelOverflow))
                                    Next

                                    oHtml.AppendLine("</div>")

                                    oHtml.AppendLine($"<div {ToolTipProperties} class='col-6 form-group d-flex {FloatingClass}' { If(bEnglish, "style='display: flex; flex-direction: row-reverse;'", "")}>")

                                    For Each oNode In Group2
                                        oHtml.Append(OneField(bp, oNode, bValueDisplay:=bWideGraph, labelOverflow:=LabelOverflow))
                                    Next

                                    oHtml.AppendLine("</div>")

                                    oHtml.AppendLine("</div>") 'row
                                Else
                                    If cGroup.Count <= 2 Then
                                        'oHtml.Append($"<div class=' form-group d-flex {FloatingClass} justConSpceBtwn' { If(bEnglish, "style='display: flex; flex-direction: row-reverse;'", "")}{ If(oNode.Param("pf4") = "R", "style='flex-direction: column-reverse;'", "")}") 'ntg 25.06.23 added justConSpceBtwn so that when there's switch btn, its spaced

                                        Dim flexLayoutStyle As String = "flex-reverse"
                                        If _toolTipType = 2 Then
                                            flexLayoutStyle = "flex-column-reverse"
                                        End If
                                        For Each oNode In cGroup
                                            If oNode.Param("cry") = "S" Then
                                                flexLayoutStyle = "flex-column-reverse"
                                            End If
                                        Next

                                        oHtml.Append($"<div class=' form-group d-flex {flexLayoutStyle} {FloatingClass} justConSpceBtwn' { If(bEnglish, "style='display: flex; flex-direction: row-reverse;'", "")}{ If(oNode.Param("cry") = "C", "", "")}") 'ntg 04.08.24 fixing the switch btn to appear in one line
                                        If FloatingLabels AndAlso cGroup.Count = 2 AndAlso CType(cGroup(0), Node).NodeType = "L" Then ' In Floating label the input (Field) should be before the label
                                            Dim LabelNode As Node = cGroup(0)
                                            Dim LabelValue As String = LabelNode.Param("val").Trim()
                                            Dim LenPropNode As String = LabelNode.Param("len") 'ntg 05.02.24 now fields with len=80 wont have tooltip

                                            If LabelValue.Length > MaximumLabelChars AndAlso LenPropNode <> "00080" AndAlso SixthScreenWidth Then 'ntg 05.02.24 now fields with len=80 wont have tooltip / ntg 11.02.24 added SixthScreenWidth to check widenss
                                                ToolTipProperties = $"data-bs-toggle='tooltip' title='{Heb(LabelValue.Replace("'", ""))}'"
                                                '  LabelOverflow = True
                                            End If
                                            'oNode.ParamVal("typ")
                                            Dim FieldNode As Node = cGroup(1)
                                            FieldNode.Param("placeholder") = " "

                                            cGroup.Clear()
                                            cGroup.Add(FieldNode)
                                            cGroup.Add(LabelNode)
                                        End If

                                        oHtml.AppendLine($"{ToolTipProperties}>")

                                        For Each oNode In cGroup
                                            Dim sliderHtml As String = ""
                                            If oNode.Param("cry") = "S" Then
                                                Dim inputID = "YF" + oNode.LongName + oNodeScreen.Param("fdate")

                                                Dim sliderMin As Integer = 0
                                                If oNode.ParamExists("min") Then
                                                    sliderMin = oNode.ParamVal("min")
                                                End If
                                                Dim sliderMax As Integer = sliderMin
                                                If oNode.ParamExists("max") Then
                                                    sliderMax = oNode.ParamVal("max")
                                                End If
                                                Dim sliderValue As Integer = sliderMin
                                                If oNode.ParamExists("val") Then
                                                    sliderValue = oNode.ParamVal("val")
                                                End If
                                                oHtml.Append($"<div style='display: flex; margin-top: 8px;'><div style='width: 50%; text-align: start;'>{sliderMax}</div><div style='width: 50%; text-align: end;'>{sliderMin}</div></div>")

                                                sliderHtml = "<script src=""https://code.jquery.com/jquery-3.3.1.min.js"" integrity=""sha256-FgpCb/KJQlLNfOu91ta32o/NMZxltwRo8QtmkMRdAu8="" crossorigin=""anonymous""></script>"
                                                sliderHtml += "<script src=""../js/jQuery.inputSliderRange.js""></script>"
                                                sliderHtml += "<script>$('#" + inputID + "').inputSliderRange({""min"": " + sliderMin.ToString + ", ""max"": " + sliderMax.ToString + ", ""start"": " + sliderValue.ToString + "})</script>"
                                            End If

                                            oHtml.Append(OneField(bp, oNode, bValueDisplay:=bWideGraph, labelOverflow:=LabelOverflow))

                                            If oNode.Param("cry") = "S" Then
                                                oHtml.Append(sliderHtml)
                                            End If
                                        Next
                                        'If oNode.Param("pul") = "A" Then
                                        '    oHtml.Append("<i class='fa-solid fa-flag-checkered sideIcon'></i>")
                                        'End If

                                        oHtml.AppendLine("</div>")
                                        Dim toolTipType As Integer = 0
                                        Dim toolTipTextHky As String = ""
                                        Dim toolTipText As String = ""
                                        Dim qColor As New Color()
                                        If cGroup(0).ParamExists("tip") AndAlso cGroup(0).ParamExists("hky") Then
                                            toolTipType = cGroup(0).ParamVal("tip")
                                            toolTipTextHky = cGroup(0).Param("hky")
                                            toolTipText = qColor.GetHelp(bp, cGroup(0), oNodeScreen, "2")
                                        ElseIf cGroup.Count > 1 AndAlso cGroup(1).ParamExists("tip") AndAlso cGroup(1).ParamExists("hky") Then
                                            toolTipType = cGroup(1).ParamVal("tip")
                                            toolTipTextHky = cGroup(1).Param("hky")
                                            toolTipText = qColor.GetHelp(bp, cGroup(1), oNodeScreen, "2")
                                        End If
                                        If toolTipType = 3 Then
                                            oHtml.AppendLine(toolTipText)
                                        End If
                                    Else
                                        oHtml.Append(OneField(bp, cGroup(0)))
                                        oHtml.AppendLine("<div class='row' >")
                                        Dim sPhone As String = ""
                                        If cGroup.Count = 3 Then
                                            Dim oNodeTmp As Node
                                            Dim nWid2, nWid3 As Integer
                                            oNodeTmp = cGroup(1)
                                            nWid2 = oNodeTmp.Width
                                            oNodeTmp = cGroup(2)
                                            nWid3 = oNodeTmp.Width
                                            Select Case True
                                                Case nWid2 < nWid3 : sPhone = "8+4"
                                                Case nWid2 > nWid3 : sPhone = "4+8"
                                                    'Case Else : sPhone = "6+6"
                                            End Select
                                        End If
                                        For n = 2 To cGroup.Count
                                            Select Case True
                                                Case cGroup.Count = 2
                                                    oHtml.Append("<div class=""col-12"">").Append(vbCrLf)
                                                Case cGroup.Count = 3 AndAlso n = 2 AndAlso sPhone = "4+8"
                                                    oHtml.Append("<div class=""col-8"">").Append(vbCrLf)
                                                Case cGroup.Count = 3 AndAlso n = 3 AndAlso sPhone = "8+4"
                                                    oHtml.Append("<div class=""col-8"">").Append(vbCrLf)
                                                Case cGroup.Count = 3 AndAlso n = 3 AndAlso sPhone = "4+8"
                                                    oHtml.Append("<div class='col-4 d-flex'>").Append(vbCrLf)
                                                Case cGroup.Count = 3 AndAlso n = 2 AndAlso sPhone = "8+4"
                                                    oHtml.Append("<div class='col-4 d-flex'>").Append(vbCrLf)
                                                Case cGroup.Count = 3
                                                    oHtml.Append("<div class=""col-6"">").Append(vbCrLf)
                                                Case cGroup.Count = 4
                                                    'oHtml.AppendLine("<div class=""col-4 d-flex"">")
                                                    oHtml.AppendLine("<div class=""col-12 col-md-6 col-lg-2 d-flex"">") 'ntg 05.02.23
                                                Case cGroup.Count

                                                    oHtml.Append("<div class=""col-3"">").Append(vbCrLf)
                                                Case Else
                                                    oHtml.Append("<div class=""col-4"">").Append(vbCrLf)
                                            End Select
                                            oNode = cGroup(n - 1)
                                            oHtml.Append(OneField(bp, oNode))
                                            oHtml.Append("</div>").Append(vbCrLf)
                                        Next
                                        oHtml.Append("</div>").Append(vbCrLf)
                                    End If
                                End If
                                If oHtml.ToString.Contains("< class=' form-label   ' style='position:absolute;'>") Then
                                    oHtml.Replace("< class=' form-label   ' style='position:absolute;'>", "")
                                End If
                                oHtml.Append("</div>").Append(vbCrLf)
                            Next
                            oHtml.Append("</div>").Append(vbCrLf)

                            'ntg 19.06.24 changes regarding sub-title and accordion inside cards- this class wraps the data without the title*/
                            If oNode.Param("pul") = "A" Then
                                oHtml.Append("<div class= 'subCardA_Data accClosed'>").Append(vbCrLf)
                            End If
                        End If
                    End If
                End If
            End If
        Next
        If ButtonList.Count > 0 Then
            ShowButtonList(bp, oHtml, ButtonList)
            ScreenHasButtonList = True
        End If

        'ntg 17.06.24 vladi changes regarding sub-title and accordion in card
        If bp.m_bInSubCard Then
            'oHtml.Append("<!-- subcard end --></div>" & vbCrLf)
            oHtml.Append("<!-- subcard end --></div></div>" & vbCrLf)
            bp.m_bInSubCard = False
        End If


        If bTableMode Then
            oHtml.Append("</tbody>").Append(vbCrLf)
            oHtml.Append("</table>").Append(vbCrLf)
            oHtml.Append("</div>").Append(vbCrLf)
        End If
        oHtml.Append("</div>").Append(vbCrLf)

        'ntg 19.05.24 new code section regarding cmd buttons that go inside cards
        Dim CmdButtonsForCard As List(Of Button) = New List(Of Button)

        'ntg 23.05.24 more code regarding splitting cmd buttons that go inside cards into 2 groups-navigation and the rest, and creating more spacing between the groups

        Dim CmdButtonsForCardNavigation As List(Of Button) = New List(Of Button)
        Dim CmdButtonsForCardRegular As List(Of Button) = New List(Of Button)
        Dim navCmdButtons As New List(Of String)({"90", "91", "19", "20"})
        For Each oButton As Button In bp.cButtons
            If bp.m_bStatementWindow Then
                'vk 09.24
                CmdButtonsForCard.Add(oButton)
                CmdButtonsForCardRegular.Add(oButton)
            Else
                Dim cgrValue As String = oButton.oNode.Param("cgr")
                If oButton.oNode.ParamExists("cgr") AndAlso cgrValue.Length = 2 AndAlso cgrValue <> "00" AndAlso IsNumeric(cgrValue) Then
                    If cgrValue.Substring(1, 1) = CardCounter.ToString Then
                        CmdButtonsForCard.Add(oButton)
                        If navCmdButtons.Contains(oButton.oNode.Param("fk")) Then
                            CmdButtonsForCardNavigation.Add(oButton)
                        Else
                            CmdButtonsForCardRegular.Add(oButton)
                        End If
                    End If
                End If
            End If
        Next

        ' Arrange the buttons based on the first digit of cgrValue
        CmdButtonsForCard = CmdButtonsForCard.OrderBy(Function(button) button.oNode.Param("cgr")(0)).ToList()
        CmdButtonsForCardNavigation = CmdButtonsForCardNavigation.OrderBy(Function(button) button.oNode.Param("cgr")(0)).ToList()
        CmdButtonsForCardRegular = CmdButtonsForCardRegular.OrderBy(Function(button) button.oNode.Param("cgr")(0)).ToList()
        If CmdButtonsForCard.Count <> 0 Then
            oHtml.Append("<div class='cardCmdButtonsWrapper'>").Append(vbCrLf) 'cmd buttons inside cards div

            'For Each button As Button In CmdButtonsForCard
            '    Dim PicNumber = If(button.oNode.ParamExists("icn"), button.oNode.ParamVal("icn"), If(button.oNode.ParamExists("pic"), button.oNode.ParamVal("pic"), 0))
            '    button.PicNumber = PicNumber
            '    oHtml.Append($"{button.ControlItself.FullCode(bp, "button", IconFullValue:=If(button.PicNumber <> 0, button.PicNumber, ""), FooterButtonWithImage:=(PicNumber > 0), AdditionalClass:="cardCmdBtn")}")
            'Next

            If CmdButtonsForCardNavigation.Count <> 0 Then
                oHtml.Append("<div class='CmdBtnsForCardNav'>").Append(vbCrLf)
                For Each button As Button In CmdButtonsForCardNavigation
                    Dim PicNumber = If(button.oNode.ParamExists("icn"), button.oNode.ParamVal("icn"), If(button.oNode.ParamExists("pic"), button.oNode.ParamVal("pic"), 0))
                    button.PicNumber = PicNumber
                    oHtml.Append($"{button.ControlItself.FullCode(bp, "button", IconFullValue:=If(button.PicNumber <> 0, button.PicNumber, ""), FooterButtonWithImage:=(PicNumber > 0), AdditionalClass:="cardCmdBtn")}")
                Next
                oHtml.Append("</div>").Append(vbCrLf)
            End If

            If CmdButtonsForCardRegular.Count <> 0 Then
                oHtml.Append("<div class='CmdBtnsForCardRegular'>").Append(vbCrLf)
                For Each button As Button In CmdButtonsForCardRegular
                    Dim PicNumber = If(button.oNode.ParamExists("icn"), button.oNode.ParamVal("icn"), If(button.oNode.ParamExists("pic"), button.oNode.ParamVal("pic"), 0))
                    button.PicNumber = PicNumber
                    oHtml.Append($"{button.ControlItself.FullCode(bp, "button", IconFullValue:=If(button.PicNumber <> 0, button.PicNumber, ""), FooterButtonWithImage:=(PicNumber > 0), AdditionalClass:="cardCmdBtn")}")
                Next
                oHtml.Append("</div>").Append(vbCrLf)

            End If

            oHtml.Append("</div>").Append(vbCrLf)
        End If

        If strc = 2 OrElse strc = 3 Then
            'oHtml.Append("<!-- before last div of OneCard-->").Append(vbCrLf)

            'oHtml.Append($"</div>closeDiv{nCard}").Append(vbCrLf)

            'If UseAccordion Then oHtml.AppendLine($"</div>closeAccrodion{nCard}")

            If nCard = 1 Then
                ' skip, this is the header card
            ElseIf nCard = 2 Then
                oHtml.AppendLine($"</div>")
                oHtml.AppendLine($"</div>")
                oHtml.AppendLine($"</div>")
            ElseIf nCard > 2 AndAlso strc = 3 Then
                oHtml.AppendLine($"</div>")
                If (nCard - 2) Mod 3 = 1 Then
                    oHtml.AppendLine($"</div>")
                ElseIf (nCard - 2) Mod 3 = 2 Then
                    oHtml.AppendLine($"</div>")
                Else
                    oHtml.AppendLine($"</div>")
                    oHtml.AppendLine($"</div>")
                End If
            ElseIf nCard > 2 AndAlso strc = 2 Then
                If (nCard - 2) Mod 2 = 1 Then
                    oHtml.AppendLine($"</div>")
                Else
                    oHtml.AppendLine($"</div>")
                    oHtml.AppendLine($"</div>")
                End If
            ElseIf nCard = 0 Then
                ' skip, this is a redundant card at the end of the cards list
            Else
            End If
        Else
            'ntg 19.05.24 END OF new code section regarding cmd buttons that go inside cards
            oHtml.Append("<!-- before last div of OneCard-->").Append(vbCrLf)

            'If UseAccordion Then
            oHtml.AppendLine("</div>") 'vk 09.24 this "if" looked to be wrong

            If UseAccordion Then oHtml.AppendLine($"</div>")

            If IsFgr1 OrElse ((IsQtyp3 OrElse IsQtyp2) AndAlso Not bRect) Then 'ntg 04.08.24 added IsQtyp2
                oHtml.AppendLine($"{endOfRow}") 'this is the div that closes a "rowStart class" for type qtype and not the first card
            ElseIf (IsQtyp3 OrElse IsQtyp2) AndAlso bRect Then 'ntg 04.08.24 added IsQtyp2
                oHtml.AppendLine("</div>") 'this is the div that closes a "rowStart class" for type qtype="2" in the first card
            End If
        End If

        oHtml.Append("<!-- end of OneCard-->").Append(vbCrLf)

        If Not bEmptyCard Then
            oHtml_Param.Append(oHtml)
            CardCounter += 1
        End If
    End Sub

    'vk 02.24
    Private Function FreeCol(cNodes As Collection, nColumn As Integer) As Integer
        Dim NewColumn As Integer = nColumn
        While cNodes.Contains(NewColumn.ToString)
            NewColumn += IIf(bEnglish, 1, -1)
        End While
        Return NewColumn
    End Function
    Private Sub AddField(oDdsLine As DdsLine, oNode As Node, IsModalWindow As Boolean)

        'rem vk 03.24
        'Dim nColumn As Integer = oNode.ParamVal("col")
        'If oNode.Param("ewr") <> "H" OrElse oNode.Param("inp") <> "P" Then 'vk 02.24+
        '    oDdsLine.cNodes.Add(oNode, FreeCol(oDdsLine.cNodes, nColumn).ToString)
        '    Exit Sub
        'End If
        'Dim sText As String = oNode.Param("val")

        'vk 03.24+ ntg 10.03.24 changes for special header split with '|'
        Dim nColumn As Integer = oNode.ParamVal("col")
        Dim sText As String = oNode.Param("val")
        If oNode.Param("ewr") <> "H" OrElse oNode.Param("inp") <> "P" Then 'vk 02.24+
            If oNode.Param("ewr") = "D" Then
                'vk 03.24+
                If oNode.ParamVal("col") > 1 Then
                    sText = Space(oNode.ParamVal("col") - 1) & sText
                End If
                sText = sText.PadRight(nScreenWid)
                oDdsLine.sEwrD = sText
                If oNode.ParamYes("pf4") AndAlso oNode.ParamYes("f4p") Then 'ntg 10.04.24 changes for special header split with '|'
                    oDdsLine.sNameForPf4 = "F" & oNode.LongName & oNodeScreen.Param("fdate")
                End If
            Else
                If oNode.ParamVal("dummies") > 0 Then 'vk 09.24
                    Dim nDummies As Integer = oNode.ParamVal("dummies")
                    Dim i As Integer, bLabel As Integer
                    For i = 1 To nDummies
                        For bLabel = -1 To 0
                            Dim oNodeTmp As Node = New Node
                            If bLabel Then
                            Else
                                oNodeTmp.AddIfNo("inp", "P")
                            End If
                            oNodeTmp.AddIfNo("typ", "A")
                            oNodeTmp.AddIfNo("col", nColumn)
                            oNodeTmp.AddIfNo("len", 1)
                            oNodeTmp.AddIfNo("val", "x")
                            oNodeTmp.AddIfNo("num", "placeholder")
                            oNodeTmp.AddIfNo("lin", oNode.Param("lin"))
                            If oNode.ParamExists("ind") Then
                                oNodeTmp.AddIfNo("ind", oNode.Param("ind"))
                            Else
                                oNodeTmp.AddIfNo("ind", "0000")
                            End If
                            oDdsLine.cNodes.Add(oNodeTmp, FreeCol(oDdsLine.cNodes, nColumn).ToString)
                        Next
                    Next
                End If
                oDdsLine.cNodes.Add(oNode, FreeCol(oDdsLine.cNodes, nColumn).ToString)
            End If
            Exit Sub
        End If

        If sText > "" Then
            oDdsLine.bHasEwrH = True
            Dim n1, n2 As Integer
            Dim bEnd As Boolean = False

            'vk 03.24 - ntg 17.03.24 changes by vladi - new code section
            If oNode.ParamVal("lin") = yT.nFrom Then
                Dim nMiddle As Integer = oNode.ParamVal("col") + oNode.ParamVal("len") \ 2
                Dim n As Integer
                For n = 1 To x.Count
                    x0 = x(n)
                    If x0.nFrom < nMiddle AndAlso x0.nTo > nMiddle Then
                        x.Remove(n)
                        Exit For
                    End If
                Next
            End If


            n1 = 0
            Do
                n2 = InStr(n1 + 1, sText, "|")
                If n2 = 0 Then
                    n2 = Len(sText) + 1
                    bEnd = True
                End If
                Dim oNodeTmp As Node = New Node
                Dim nColNew As Integer = FreeCol(oDdsLine.cNodes, n1 + nColumn)
                Dim qConv As New Conv()
                oNodeTmp.AddIfNo("val", Mid(sText, n1 + 1, n2 - n1 - 1))
                oNodeTmp.AddIfNo("typ", "A")
                oNodeTmp.AddIfNo("col", nColNew.ToString("000"))
                oNodeTmp.AddIfNo("len", (n2 - n1 - 1).ToString("00000"))
                oNodeTmp.AddIfNo("num", oNode.Param("num"))
                oNodeTmp.AddIfNo("lin", oNode.Param("lin"))
                oNodeTmp.AddIfNo("pnt", oNode.Param("pnt")) 'ntg 28.03.24 was missing as an option of a parameter
                If oNode.ParamExists("ind") Then
                    oNodeTmp.AddIfNo("ind", oNode.Param("ind"))
                Else
                    oNodeTmp.AddIfNo("ind", "0000")
                End If
                oNodeTmp.AddIfNo("dec", "00")
                oNodeTmp.AddIfNo("orig_col", n1.ToString)
                oNodeTmp.AddIfNo("orig_inp", oNode.Param("inp")) 'vk 09.24
                oDdsLine.cNodes.Add(oNodeTmp, nColNew.ToString)

                'vk 03.24 - ntg 17.03.24 changes by vladi - new code section
                If oNode.ParamVal("lin") = yT.nFrom Then
                    x0 = New FromTo
                    x0.Init(n1 + 1, n2 - 1)
                    x.Add(x0, (n2 - 1).ToString)
                End If

                If bEnd Then
                    Exit Do
                End If
                n1 = n2
            Loop
        End If

    End Sub

    'rem vk 02.24
    'Private Function SpecialLongHeader(bp As BuildPage, HeaderValue As String) As String
    '    Dim HeaderHtml As New StringBuilder
    '    HeaderHtml.AppendLine("<tr>")

    '    Dim HeaderWords As List(Of String) = HeaderValue.Split("|").ToList()

    '    Dim HeaderWord As String
    '    Dim HeaderWordLen As Integer
    '    Dim HeaderWordLenDynamic As Integer = 0
    '    Dim HeaderWordsIndex As Integer = 0
    '    Dim ColNum As Integer = 0
    '    Dim RangeSize As Integer
    '    For nColumn = nColFrom To nColTo Step nColStep
    '        While HeaderWords(HeaderWordsIndex).Trim() = ""
    '            HeaderWordsIndex = HeaderWordsIndex + 1
    '        End While
    '        HeaderWord = HeaderWords(HeaderWordsIndex)
    '        HeaderWordLen = HeaderWord.Length
    '        If x.Contains(nColumn.ToString) Then
    '            x0 = x(nColumn.ToString)
    '            ColNum = ColNum + 1
    '            RangeSize = x0.nTo - x0.nFrom + 1
    '            HeaderWordLenDynamic = HeaderWordLenDynamic + RangeSize

    '            If HeaderWordLenDynamic > HeaderWordLen Then
    '                HeaderHtml.AppendLine($"<th  colspan='{ColNum - 1}'>{HeaderWord}</th>")
    '                ColNum = 1
    '                HeaderWordLenDynamic = RangeSize
    '                HeaderWordsIndex = HeaderWordsIndex + 1
    '                If HeaderWordsIndex = HeaderWords.Count Then
    '                    Exit For
    '                End If
    '            End If
    '        End If
    '    Next

    '    HeaderHtml.AppendLine("</tr>")
    '    Return HeaderHtml.ToString()

    'End Function

    Private Sub ShowButtonList(ByRef bp As BuildPage, ByRef oHtml As StringBuilder, buttonList As List(Of Node))

        Dim ContinueBtn As String
        If onlyVal = "E" Then  'ntg 04.07.23 holds the value of the 'only' parameter in xml ('Y'/'E'/' ')
            ContinueBtn = $"{If(bEnglish, "Finish", "סיים") }"
        Else
            ContinueBtn = $"{If(bEnglish, "Continue", "המשך") }"
        End If
        'Dim ContinueBtn As String = $"{If(bEnglish, "Continue", "המשך") }"

        oHtml.AppendLine("<div class='row p-3'>
                            <div id='ButtonList' class='my-3 mx-2 col-12 col-lg-4 align-self-end list-group'>")
        '<input onkeyup='FilterButtons()' class='text-center my-1 form-control' id='FilterButtonList' type='text' placeholder='חיפוש...'>")

        For Each buttonElem As Node In buttonList
            oHtml.Append(OneField(bp, buttonElem))
        Next

        oHtml.AppendLine("</div>")

        Dim hideDivButtonChoose As Boolean = bp.getProperty("hideDivButtonChoose") <> "" 'ntg 12.09.23 adding the option to hide the "continue" buttom in the list
        oHtml.AppendLine($"<div id='DivButtonChoose'  style=""align-items: end;display:{If(hideDivButtonChoose, "none", "flex")} ;"" class='my-3 col-12 col-lg-2'>") 'ntg 01.02.23

        oHtml.AppendLine($"<button pch='' onclick='CmdPchlClick(this)' type='button' class ='w-100 btn btn-info' id='ActionChooseBtn' title={ContinueBtn} >{ContinueBtn} </button>")
        oHtml.AppendLine("</div>")
        oHtml.AppendLine("</div>")
    End Sub

    Private Function CheckIfNodeIsSideButton(oNode As Node) As Boolean
        Return (oNode.ParamIn("typ", "NSA") AndAlso oNode.ParamYes("pf4"))
    End Function

    Private Function CheckIfNodeIsRadio(oNode As Node) As Boolean
        Return (oNode.ParamYes("cry") OrElse oNode.ParamVal("tch") = 1 OrElse oNode.ParamVal("pkv") = 1)
    End Function

    Private Sub CreateColGroups(oHtml As StringBuilder)
        oHtml.Append("<colgroup>").Append(vbCrLf)
        Dim sWid As String
        If nLine = nGridStart Then
            For nColumn = nColFrom To nColTo Step nColStep
                If x.Contains(nColumn.ToString) Then
                    sWid = (x(nColumn.ToString).Wid * 5).ToString
                    oHtml.Append("<col style=""width:").Append(sWid).Append("px;"" />").Append(vbCrLf)
                End If
            Next
        Else
            For nColumn = nColFrom To nColTo Step nColStep
                If oDdsLine.cNodes.Contains(nColumn.ToString) Then
                    oNode = oDdsLine.cNodes(nColumn.ToString)
                    sWid = (oNode.ParamVal("len") * 5).ToString
                    oHtml.Append("<col style=""width:").Append(sWid).Append("px;"" />").Append(vbCrLf)
                End If
            Next
        End If
        oHtml.Append("</colgroup>").Append(vbCrLf)
    End Sub

    Private Function MenuButton(ByRef bp As BuildPage, oControl As Control, oMenuLine As MenuLine, sClass As String, sText As String)
        'If bEnglish Then
        '    oControl.Add("value", "<div class=""" & sClass & """>" & sText & "</div><div class='grid-title'>" & oMenuLine.sText & "</div>")
        '    Return "<div class=""grid-item"">" & oControl.FullCodeSpan(bp,, "button", False) & "</div>" & vbCrLf
        'Else
        If bEnglish Then
            oControl.Add("value", $"<div style=""text-align: left"" class='grid-title'>{oMenuLine.sText}</div>") 'ntg 31.01.23
            Return "<div style=""text-align: left"" class='grid-item'>" & oControl.FullCodeSpan(bp,, "button", False) & "</div>" & vbCrLf
        Else
            oControl.Add("value", $"<div style=""text-align: right"" class='grid-title'>{oMenuLine.sText}</div>") 'ntg 31.01.23
            Return "<div style=""text-align: right"" class='grid-item'>" & oControl.FullCodeSpan(bp,, "button", False) & "</div>" & vbCrLf
        End If
        'End If
    End Function

    Private Function Heb(ByVal val As String) As String
        If Not bEnglish Then
            Dim sLang As String = ""
            Dim oConv As Comtec.TIS.ConvCom = New Comtec.TIS.ConvCom()
            val = oConv.RevHeb(val.Trim, sLang)
            oConv = Nothing
        End If
        Return val
    End Function

    Private Function BelongsToThisScreen(oNode As Node, IsModalWindow As Boolean) As Boolean
        If oNodeScreen.Param("fil").Trim = "HFFLNAVFM" AndAlso oNodeScreen.Param("rec").Trim = "ROPTION" Then 'ntg 12.06.23 this screen is special and should be treated like non win=R screen
            Return True
        End If
        If oNodeScreen.Param("win") = "R" Then
            If IsModalWindow Then
                Return oNode.Param("bkg") <> "B"
            Else
                Return oNode.Param("bkg") = "B"
            End If
        Else
            Return True
        End If
    End Function
    'Private Function IgnoreGrid(IsModalWindow As Boolean) As Boolean 'vk 02.24 ---'ntg 17.03.24 cancelled by vladi
    '    If Not (oNodeScreen.Param("fil").Trim = "HFFLNAVFM" AndAlso oNodeScreen.Param("rec").Trim = "TKWINGM") Then
    '        Return False
    '    End If
    '    If oNodeScreen.Param("win") <> "R" Then
    '        Return True
    '    End If
    '    Return IsModalWindow
    'End Function

    Private Function TR(ByRef bp As BuildPage, sCell As String, nLine As Integer) As String
        Dim nColumn As Integer
        Dim oHtml As New StringBuilder
        'oHtml.Append("<option>").Append(vbCrLf)
        oHtml.Append($"<tr {If(oNodeScreen.Param("fgr") = "B", "title='קליק לבחירה/ביטול בחירה'", "")}>").Append(vbCrLf) 'ntg 08.07.24 adding a tooltip for the table rows in case its fgr=B

        If sCell = "th" AndAlso yT.nTo > yT.nFrom Then ' במקרה שמדובר על מספר שורות כותרת, אז מאחדים את הכותרות
            If nLine <= yT.nTo Then
                For Each oNode As Node In oDdsLine.cNodes
                    'If Not oNode.Param("ewr") = "H" Then ' special header - rem vk 02.24
                    Dim NodeCol As Integer = oNode.Param("col") + oNode.Width \ 2
                    Dim sText As String = oNode.Param("val")
                    If CombinedHeadersDict.ContainsKey(NodeCol) Then
                        CombinedHeadersDict(NodeCol) = CombinedHeadersDict(NodeCol) + " " + Heb(sText)
                    Else
                        CombinedHeadersDict.Add(NodeCol, Heb(sText))
                    End If
                    'End If
                Next
            End If

            If nLine < yT.nTo Then
                Return ""
            End If

            If nLine = yT.nTo Then
                For Each x0 In x
                    x0.sHtml = "<th></th>"
                Next

                For nColumn = nColFrom To nColTo Step nColStep
                    If CombinedHeadersDict.ContainsKey(nColumn) Then
                        For Each x0 In x
                            If x0.Contains(nColumn) Then
                                x0.sHtml = $"<th>{CombinedHeadersDict(nColumn)}</th> "
                                Exit For
                            End If
                        Next
                    End If
                Next

                If bp.ActionNodes.Count > 0 AndAlso bp.getProperty("ShowTableHeaderForActions") = "true" Then 'בכל פעם שיש מסך גריד עם 3 נקודות לבחירת אפשרויות נוספות, יש להוסיף את המילה פעולות בכותרת אותה עמודה של ה-3 נקודות
                    'Dim LastX As FromTo = x(x.Count) 'rem vk 03.24
                    'If LastX.sHtml = "<th></th>" Then 'ntg 21.02.24 if there are no results, the פעולות column wont be shown
                    '    LastX.sHtml = "<th>פעולות</th>"
                    'End If

                    'ntg 17.03.24 changes by vladi- added new code section
                    Dim n As Integer
                    Dim nMaxCol As Integer = 0
                    Dim nMaxColIndex As Integer
                    For n = 1 To x.Count
                        x0 = x(n)
                        If x0.nFrom > nMaxCol Then
                            nMaxCol = x0.nFrom
                            nMaxColIndex = n
                        End If
                    Next
                    Dim LastX As FromTo = x(nMaxColIndex)
                    If LastX.sHtml = "<th></th>" Then 'ntg 21.02.24 if there are no results, the פעולות column wont be shown
                        LastX.sHtml = "<th>פעולות</th>"
                    End If

                End If

                For nColumn = nColFrom To nColTo Step nColStep
                    If x.Contains(nColumn.ToString) Then
                        x0 = x(nColumn.ToString)
                        oHtml.AppendLine(x0.sHtml)
                    End If
                Next

                oHtml.AppendLine("</tr>")
                'oHtml.Append("</option>").Append(vbCrLf)

                Return oHtml.ToString
            End If
        ElseIf yT.Contains(nLine) AndAlso oDdsLine.sPattern = "L" Then
            Dim oNode As Node = oDdsLine.cNodes(1)
            Dim sText As String = oNode.Param("val")
            For nColumn = nColFrom To nColTo Step nColStep
                If x.Contains(nColumn.ToString) Then
                    x0 = x(nColumn.ToString)
                    oHtml.Append("<" & sCell & ">" & Heb(Mid(sText, x0.nFrom, x0.nTo - x0.nFrom + 1)) & "</" & sCell & ">" & vbCrLf)
                End If
            Next
        ElseIf yT.Contains(nLine) OrElse yS.Contains(nLine) OrElse yL.Contains(nLine) Then
            For Each x0 In x
                x0.sHtml = "<" & sCell & "></" & sCell & ">"
            Next

            'ntg 18.06.24 this loop will build a cell in a table as highlighted if its the final line + has parameters pbg and phi in the xml
            For nColumn = nColFrom To nColTo Step nColStep
                If oDdsLine.cNodes.Contains(nColumn.ToString) Then
                    Dim oNode As Node = oDdsLine.cNodes(nColumn.ToString)

                    For Each x0 In x
                        If x0.Contains(nColumn + oNode.Width \ 2) Then
                            If (oNode.ParamExists("phi") AndAlso oNode.ParamExists("pbg") AndAlso oNode.Param("phi") = "Y" AndAlso oNode.Param("lin") = (actualTableLinesCounter + firstHardTableRow - 1).ToString) Then
                                x0.sHtml = OneField(bp, oNode, sCell, isHighlitedCell:=True)
                            Else
                                x0.sHtml = OneField(bp, oNode, sCell)
                            End If
                        End If
                    Next
                End If
            Next

            If sCell = "th" AndAlso bp.ActionNodes.Count > 0 AndAlso bp.getProperty("ShowTableHeaderForActions") = "true" Then 'בכל פעם שיש מסך גריד עם 3 נקודות לבחירת אפשרויות נוספות, יש להוסיף את המילה פעולות בכותרת אותה עמודה של ה-3 נקודות
                Dim LastX As FromTo = x(x.Count)
                If LastX.sHtml = "<th></th>" Then 'ntg 21.02.24 if there are no results, the פעולות column wont be shown
                    LastX.sHtml = "<th>פעולות</th>"
                End If
            End If

            For nColumn = nColFrom To nColTo Step nColStep
                If x.Contains(nColumn.ToString) Then
                    x0 = x(nColumn.ToString)
                    oHtml.Append(x0.sHtml)
                End If
            Next
        Else
            'rem vk 03.24 ntg 10.03.24 changes for special header split with '|'
            'For nColumn = nColFrom To nColTo Step nColStep
            '    If oDdsLine.cNodes.Contains(nColumn.ToString) Then
            '        oHtml.Append(OneField(bp, oDdsLine.cNodes(nColumn.ToString), sCell)) ', sClass))
            '    End If
            'Next
            If sCell = "th" AndAlso oNodeScreen.Param("fgr") = "D" Then
                'vk 09.24
                oHtml.Append("<th>פעולות</th>")
            End If

            'vk 03.24+ ntg 10.03.24 changes for special header split with '|'
            If oDdsLine.sEwrD > "" Then
                'vk 03.24+
                For nColumn = nColFrom To nColTo Step nColStep
                    If xTmp.Contains(nColumn.ToString) Then
                        x0 = xTmp(nColumn.ToString)
                        oHtml.Append("<td>" & Heb(Mid(oDdsLine.sEwrD, x0.nFrom, x0.nTo - x0.nFrom + 1)) & "</td>")
                    End If
                Next
                If oDdsLine.sNameForPf4 > "" Then  'ntg 10.04.24 changes for special header split with '|'
                    oHtml.Remove(oHtml.Length - 5, 5)
                    oHtml.Append(qMulti.SideButton(oDdsLine.sNameForPf4, "fnpf4_dblclick();", "float:left;") & "</td>")
                End If

            Else
                xTmp = New Collection 'ntg 10.04.24 changes by vladi regarding a certain bug that was found
                For nColumn = nColFrom To nColTo Step nColStep
                    'If oDdsLine.cNodes.Contains(nColumn.ToString) Then
                    '    oHtml.Append(OneField(bp, oDdsLine.cNodes(nColumn.ToString), sCell)) ', sClass))
                    'If sCell = "th" Then
                    '    'vk 03.24+
                    '    x0 = New FromTo
                    '    Dim oNodeTmp As Node = oDdsLine.cNodes(nColumn.ToString)
                    '    x0.Init(nColumn.ToString, (nColumn + oNodeTmp.Param("len") - 1).ToString)
                    If oDdsLine.cNodes.Contains(nColumn.ToString) Then 'ntg 31.03.24 changes by vladi regarding a certain bug that was found
                        Dim oNodeTmp As Node = oDdsLine.cNodes(nColumn.ToString)
                        oHtml.Append(OneField(bp, oNodeTmp, sCell)) ', sClass))
                        If sCell = "th" AndAlso oNodeTmp.Param("bkg") <> "B" Then
                            x0 = New FromTo
                            x0.Init(nColumn.ToString, (nColumn + oNodeTmp.Param("len") - 1).ToString)

                            xTmp.Add(x0, nColumn.ToString)
                        End If
                    End If
                Next
            End If

        End If
        oHtml.Append("</tr>").Append(vbCrLf)
        'oHtml.Append("</option>").Append(vbCrLf)

        Return oHtml.ToString
    End Function

    Private Function OneGraph(ByRef bp As BuildPage, bValueDisplay As Boolean) 'vk 05.21

        Dim data As Double(,) '= New Double(,) {{210, 1220, 400}, {150, 820, 550}, {50, 82, 55}}
        Dim dataLabels As String() '= New String() {"May", "June", "July"}
        Dim dataLabel As String() '= New String() {"Ilia", "Vova", "Fima"}
        Dim pxn As String = oNode.Param("pxn")
        Dim asGraphData(,) As String
        Dim asGraphData0() As String
        Dim nMaxInd1 As Integer
        Dim nMaxInd2 As Integer
        Try
            asGraphData = cGraphData(pxn)
            nMaxInd1 = oMaxInd1(pxn)
            nMaxInd2 = oMaxInd2(pxn)
        Catch
            Return ""
        End Try

        If nMaxInd2 = 1 Then
            ReDim data(nMaxInd1 - 1, 0)
            ReDim dataLabel(nMaxInd1 - 1)
            ReDim dataLabels(0)
            Dim j As Integer
            For j = 0 To nMaxInd1 - 1
                data(j, 0) = asGraphData(1, j + 1)
            Next
        Else
            ReDim data(nMaxInd2 - 1, nMaxInd1 - 1)
            ReDim dataLabels(nMaxInd1 - 1)
            ReDim dataLabel(nMaxInd2 - 1)
            Dim i, j As Integer
            For i = 0 To nMaxInd2 - 1
                For j = 0 To nMaxInd1 - 1
                    data(i, j) = asGraphData(i + 1, j + 1)
                Next
            Next
        End If

        'If cGraphData0.Contains(pxn) Then
        Dim bHasLabels As Boolean = False
        If cGraphData0.Contains(pxn) Then
            asGraphData0 = cGraphData0(pxn)
            Dim ind As Integer
            For ind = 1 To nMaxInd1
                If asGraphData0(ind) IsNot Nothing Then
                    bHasLabels = True
                    Exit For
                End If
            Next
        End If
        If bHasLabels Then
            asGraphData0 = cGraphData0(pxn)
            Dim ind As Integer
            If nMaxInd2 = 1 Then
                For ind = 1 To nMaxInd1
                    dataLabel(ind - 1) = asGraphData0(ind)
                Next
                dataLabels(0) = "" 'asGraphData0(31)
            Else
                For ind = 1 To nMaxInd1
                    dataLabels(ind - 1) = asGraphData0(ind)
                Next
                For ind = 31 To 30 + nMaxInd2
                    dataLabel(ind - 31) = "" 'asGraphData0(ind)
                Next
            End If
        Else
            Dim conn As SqlConnection = New SqlConnection()
            Dim dsRecords As New DataSet()
            Dim dr As DataRow ', dc As DataColumn
            Try
                conn = New SqlConnection(bp.m_sConnectionString)
                conn.Open()
                Dim dtAdapter As New SqlDataAdapter("Select * from tblFlex where flkod=" & oNode.Param("pxk") & " And fltbno=" + pxn, conn)
                dtAdapter.SelectCommand.CommandType = CommandType.Text
                dtAdapter.Fill(dsRecords)
                For Each dr In dsRecords.Tables(0).Rows
                    Dim ind As Integer = dr("flnoin")
                    Dim txt As String = Trim(dr("flltxt")).Replace(ChrW(8238), "")
                    If nMaxInd2 = 1 Then
                        Select Case ind
                            Case 1 To nMaxInd1 : dataLabel(ind - 1) = txt
                            Case 31 : dataLabels(0) = txt
                        End Select
                    Else
                        Select Case ind
                            Case 1 To nMaxInd1 : dataLabels(ind - 1) = txt
                            Case 31 To 30 + nMaxInd2 : dataLabel(ind - 31) = txt
                        End Select
                    End If
                Next
            Catch e As Exception
                Throw New Exception("1020  DB Error. " + vbCrLf + e.Message, e)
                Return ""
            Finally
                Try
                    If conn.State = ConnectionState.Open Then
                        conn.Close()
                    End If
                Catch
                End Try
                Try
                    If Not conn Is Nothing Then
                        conn.Dispose()
                    End If
                Catch
                End Try
                conn = Nothing
            End Try
        End If

        Dim color As String() = New String() {
            "rgba(255,99,132,0.2)",
            "rgba(255,159,64,0.2)",
            "rgba(75,192,192,0.2)",
            "rgba(54,162,235,0.2)",
            "rgba(153,102,255,0.2)",
            "rgba(201,203,207,0.2)",
            "rgba(255,205,86,0.2)",
            "rgba(182,127,0,0.2)",
            "rgba(71,161,65,0.2)",
            "rgba(255,141,106,0.2)",
            "rgba(160,160,160,0.2)",
            "rgba(151,187,205,0.2)",
            "rgba(255,99,132,0.2)",
            "rgba(255,159,64,0.2)",
            "rgba(75,192,192,0.2)",
            "rgba(54,162,235,0.2)",
            "rgba(153,102,255,0.2)",
            "rgba(201,203,207,0.2)",
            "rgba(255,205,86,0.2)",
            "rgba(182,127,0,0.2)",
            "rgba(71,161,65,0.2)",
            "rgba(255,141,106,0.2)",
            "rgba(160,160,160,0.2)",
            "rgba(151,187,205,0.2)",
            "rgba(255,99,132,0.2)",
            "rgba(255,159,64,0.2)"}
        Dim crt As ChartJs.ChartJs = New ChartJs.ChartJs()
        Dim json As String = ""
        Select Case oNode.ParamVal("tch")
            Case 4 : json = crt.getChartJsScript("myChart" & pxn, "bar", data, dataLabels, dataLabel, color, bValueDisplay, "", False)
            Case 5 : json = crt.getChartJsPieScript("myChart" & pxn, "pie", data, dataLabels, dataLabel, color, bValueDisplay, "", False)
            Case 6 : json = crt.getChartJsPieScript("myChart" & pxn, "doughnut", data, dataLabels, dataLabel, color, bValueDisplay, "", False)
        End Select
        Dim oControl As Control = New Control()
        oControl.AddId("F" & oNode.LongName)
        oControl.Add("type", "hidden")
        oControl.Add("graph", "myChart" & pxn)
        Dim s As String = oControl.FullCode(bp)
        oControl.Dispose()
        oControl = Nothing
        Return s & "<canvas id=""myChart" & pxn & """ ></canvas>" & vbCrLf & json & vbCrLf

    End Function

    Private Function OneField(ByRef bp As BuildPage, oNode As Node,
                              Optional sCell As String = "", Optional bValueDisplay As Boolean = True,
                              Optional labelOverflow As Boolean = False, Optional isHighlitedCell As Boolean = False) As String
        'ntg 23.05.24 isHighlitedCell-highlited table cells according to xml parameters phi, pbg
        If oNode.ForBoo("graph") Then
            Return OneGraph(bp, bValueDisplay)
        End If
        If oNode.Param("pchl") IsNot Nothing Then
            If sCell = "td" Then
                Dim TdButtonValue As String

                If oNode.Param("val").ToString().Trim() = "" Then ' If it an empty button, do not show it
                    TdButtonValue = ""
                Else
                    TdButtonValue = OneButton(bp, oNode, "f")
                End If

                Return $"<td>  
                            {TdButtonValue}
                        </td>
                        "
            Else
                Return OneButton(bp, oNode, "f")
            End If
        End If

        Dim isSearchBtn As Boolean = False

        If oNode.ParamExists("lnk") Then
            bp.m_sCurClass = ""
            'ElseIf oNode.Param("inp") = "" AndAlso oNode.ForBoo("label", winVal:=oNodeScreen.Param("win")) Then
            'ElseIf oNode.Param("inp") = "" AndAlso oNode.ForBoo("label", winVal:=oNodeScreen.Param("win"), fdspVal:=oNodeScreen.ParamVal("fdsp")) Then 'ntg 25.06.23 taking care of line 23 that was hidden for some reason
        ElseIf oNode.Param("inp") = "" AndAlso oNode.ForBoo("label", winVal:=oNodeScreen.Param("win"), fdspVal:=oNodeScreen.ParamVal("fdsp"), filVal:=oNodeScreen.Param("fil")) Then 'vk 05.08.24 for cities and streets screen
            bp.m_sCurClass = $"form-label {If(labelOverflow, "labelOverflow", "")}"
            oNode.Param("val") = oNode.Param("val").Trim
        ElseIf oNode.Param("pf4") = "" OrElse oNode.Param("pf4") = "Y" OrElse oNode.Param("pf4") = "A" Then
            If bp.getProperty("CityStreet") = "inline" AndAlso oNode.ParamIn("apr", "CS", 2) AndAlso oNodeScreen.Param("fgr") <> "P" Then  'ntg 10.07.24 vladi's change regarding city-street screen
                bp.m_sCurClass = "form-control form-select ComboSelect2" 'ntg 10.07.24 vladi's change regarding city-street screen
            Else
                bp.m_sCurClass = "form-control l"
            End If
        ElseIf oNode.Param("pf4") = "D" OrElse oNode.Param("pf4") = "S" OrElse oNode.Param("pf4") = "B" Then
            bp.m_sCurClass = "form-control form-select ComboSelect2"
        Else
            bp.m_sCurClass = "form-control form-select"
        End If

        Dim sFind As String = ""
        Dim sFil As String = ""
        sOut = New StringBuilder
        sOut_Local = New StringBuilder
        sOutO = New StringBuilder
        Dim s As String = oNode.Param("OuterXml")
        If s = "" Then s = "<f/>"
        Dim sr As StringReader = New StringReader(s)
        Dim rd As Xml.XmlTextReader = New Xml.XmlTextReader(sr)
        bp.m_sSpecialCtl = ""
        bp.m_sSideButton = ""

        If oNode.Param("tch") = "A" Then
            oNode.AddIfNo("dec", bp.decD)
            oNode.AddIfNo("ind", bp.indD)

            Select Case oNode.Param("inp")
                Case "O", "P", "Q"
                    sOut.Append(qMulti.Hidden("Harchive", "SF" + oNode.LongName + oNodeScreen.Param("fdate")))
                Case Else
                    sOut.Append(qMulti.Hidden("Harchive", "F" + oNode.LongName + oNodeScreen.Param("fdate")))
            End Select
        End If

        If oNode.Param("tch") = "3" Then
            oNode.AddIfNo("dec", "".PadRight(decL, "0"))
            oNode.AddIfNo("ind", "".PadRight(indL, "0"))
            sOut.Append(qMulti.Hidden("Hcg", "F" + oNode.LongName + oNodeScreen.Param("fdate")))
            sOut.Append(qMulti.Hidden("HcgHeight", bp.getProperty("CG_Height")))
            sOut.Append(qMulti.Hidden("HcgWidth", bp.getProperty("CG_Width")))
            sOut.Append(qMulti.Hidden("HcgTop", bp.getProperty("CG_Top")))
            Dim i As Integer
            For i = 1 To 9
                'vk 08.13
                sOut.Append(qMulti.Hidden("Hu" + CStr(i) + "_value"))
                sOut.Append(qMulti.Hidden("Hv" + CStr(i) + "_value"))
                sOut.Append(qMulti.Hidden("Hw" + CStr(i) + "_value"))
            Next
        End If

        qMulti.BuildField(bp, rd, sOut, sOut_Local, sOutO, oNode, oNodeScreen, sFind, sFil, False, m_ModalScreen) 'vk 02.23

        If sCell > "" AndAlso oNode.Param("inp") = "" Then
            Return "<" & sCell & ">" & Heb(oNode.Param("val")) & "</" & sCell & ">" & vbCrLf
        End If

        If bp.m_sSideButton > "" Then
            bp.m_sOneSomething = bp.m_sOneSomething & vbCrLf & bp.m_sSideButton & vbCrLf
        End If

        'If sCell > "" Then
        '    Return "<" & sCell & ">" & vbCrLf & bp.m_sOneSomething & vbCrLf & "</" & sCell & ">" & vbCrLf
        If sCell > "" Then 'ntg 28.03.24 change for Kanat->adding colors to tables lines when param "pnt" exists inside a table
            Dim colorTd As String = ""

            If sCell = "td" AndAlso oNode.Param("pnt") <> "" AndAlso bp.getProperty("ColorTableRows") = "1" Then
                Select Case oNode.Param("pnt")
                    Case "1" : colorTd = "style=color:rgb(73,141,179);"
                    Case "2" : colorTd = "style=color:rgb(73,179,111);"
                    Case "3" : colorTd = "style=color:rgb(179,73,140);"
                    Case "4" : colorTd = "style=color:rgb(188,3,6);"
                    Case "5" : colorTd = "style=color:rgb(73,179,164);"
                End Select
            End If
            Dim searchBtn As String = "<button class='searchBtn' onclick='Fire(13);'></button>"
            If yS.nFrom = oNode.Param("lin") AndAlso yT.nFrom <> 0 Then 'ntg 07.05.24 added AndAlso yT.nFrom <> 0 so that this doesnt apply in flexible screens where tables act different
                isSearchBtn = True
            End If
            Dim retStr As String = $"<{sCell} {If(colorTd <> "", colorTd, "")}  {If(isHighlitedCell, "class='highlitedCell'", "")} >{If(isSearchBtn, "<div class='input-group'>", "")}" & vbCrLf & bp.m_sOneSomething & vbCrLf & $"{If(isSearchBtn, searchBtn, "")}{If(isSearchBtn, "</div>", "")}</{sCell}>" & vbCrLf 'ntg 23.05.24 isHighlitedCell-highlited table cells according to xml parameters phi, pbg
            isSearchBtn = False

            Dim a = bp
            Return retStr
        Else
            Dim a = bp
            Return bp.m_sOneSomething & vbCrLf
        End If

    End Function

    Private Function OneButton(ByRef bp As BuildPage, oNode As Node, ByVal sKind As String) As String

        Dim bDisabled As Boolean = False
        Select Case sKind
            Case "l"
                bp.m_sCurClass = $"btn {If(oNode.ParamExists("pch"), "btn-info btn-sm", "")} nav-link btnIcon" 'mx-2
                If oNode.ParamYes("ltr") Then
                    bDisabled = True
                End If
            Case "c", "f"
                Dim ButtonSize As String = If(oNode.ParamVal("psz") = 0, " btn-primary btn-sm", " mx-4 btn-info imageButton ")
                bp.m_sCurClass = $"btn {ButtonSize}"
        End Select

        If oNode.Param("pch") IsNot Nothing Then
            sKind = "pch"
        ElseIf oNode.Param("pchl") IsNot Nothing Then
            sKind = "pchl"
            bp.m_sCurClass = $"list-group-item list-group-item-action list-group-item-info {If(bEnglish, "leftAlign", "")}" ' pchlButtons "
            If bEnglish Then
                If bp.m_sCurStyle < "" Then bp.m_sCurStyle = "style= ""text-align: left;"""
            End If
        End If


        qMulti.BuildCmd(bp, sOut, sOutH, sKind, oNode, oNodeScreen, oNodeScreen, bDisabled, False, False)
        Return bp.m_sOneSomething & vbCrLf

    End Function

    Private Sub Flip(ByRef cLevels As Collection, ByVal i As Integer, ByVal j As Integer)
        Dim nLevel As Integer, oLevel_i, oLevel_j As MyNumber
        oLevel_i = cLevels(i)
        oLevel_j = cLevels(j)
        nLevel = oLevel_i.Value
        oLevel_i.Value = oLevel_j.Value
        oLevel_j.Value = nLevel
    End Sub

    Private Function CheckBtnHierarchy() As String

        If btnListInXML.Contains("12") Then
            Return "12"
        ElseIf btnListInXML.Contains("03") Then
            Return "03"
        Else
            Return "00"
        End If

    End Function

    Private Function AddDevToolsDiv(bp As BuildPage, NarrowSideBar As Boolean) As String
        Dim toHideSideBar As String = $"{If(NarrowSideBar, "class='d-lg-none'", "")}"

        Return $"                               
<ul class='devToolsDivDesign {If(NarrowSideBar, "flex-column", "flex-row")}' ><span class='mx-auto' id='DevToolsSpan'> Dev Tools </span>
                               <hr class='mx-auto sideBarDivider'> 
                    {If(bp.getProperty("ShowXml") = "true",
                            $"<li class= ' my-2 nav-item'>
                                <a {If(bEnglish, "style=""text-align: left"" ", "") } id='ShowXmlElem' class='{If(NarrowSideBar, "text-center", "")} nav-link CommandButtonInMenu btn btnIcon' {If(NarrowSideBar, "data-bs-toggle='tooltip' data-bs-placement=""left""", "")} title='Show XML' href='tmp-{bp.m_sSession}-InputFile.xml' target='_blank'>
                                <i class='fas fa-eye sideIcon'></i>
                                <span style='display:none;' {toHideSideBar}>Show xml</span>
                                </a>
                             </li>", "")}
                {If(IsFooterButtonExistsByFk(bp, "OR"),
                            $"<li class='my-2 nav-item'>
		                        <button class='{If(NarrowSideBar, "text-center", "")} nav-link CommandButtonInMenu btn btnIcon' {If(bEnglish, "style=""text-align: left"" ", "") } {If(NarrowSideBar, "data-bs-toggle='tooltip' data-bs-placement=""left""", "")} title='Ovrheb' onclick='fnBtnClick(this);' id='COR' pch='OR' name='COR' type='button'>
                                <i class='far fa-table-layout sideIcon' style='width:18px !important;'></i>
                                <span style='display:none;' {toHideSideBar}>Ovrheb</span>
                                </button>
	                        </li>", "")}</ul>"
    End Function

    'ntg 10.01.24 changes by vladi regarding special table header split with |

    'Private Function FreeCol(cNodes As Collection, nColumn As Integer) As Integer 'vk 01.24
    '    Dim NewColumn As Integer = nColumn
    '    While cNodes.Contains(NewColumn.ToString)
    '        NewColumn += IIf(bEnglish, 1, -1)
    '    End While
    '    Return NewColumn
    'End Function
    ''ntg 10.01.24 changes by vladi regarding special table header split with |

    'Private Sub AddField(cNodes As Collection, oNode As Node, nColumn As Integer) 'vk 01.24

    '    If oNode.Param("ewr") <> "H" Then
    '        cNodes.Add(oNode, FreeCol(cNodes, nColumn).ToString)
    '        Exit Sub
    '    End If
    '    Dim sText As String = oNode.Param("val")
    '    Dim nCol As Integer = oNode.ParamVal("col")
    '    If sText > "" Then
    '        Dim n1, n2 As Integer
    '        Dim bEnd As Boolean = False
    '        n1 = 0
    '        Do
    '            n2 = InStr(n1 + 1, sText, "|")
    '            If n2 = 0 Then
    '                n2 = Len(sText) + 1
    '                bEnd = True
    '            End If
    '            Dim oNodeTmp As Node = New Node
    '            Dim nColNew As Integer = FreeCol(cNodes, n1 + nCol)
    '            oNodeTmp.AddIfNo("val", Mid(sText, n1 + 1, n2 - n1 - 1))
    '            oNodeTmp.AddIfNo("typ", "A")
    '            oNodeTmp.AddIfNo("col", nColNew.ToString("000"))
    '            oNodeTmp.AddIfNo("len", (n2 - n1 - 1).ToString("00000"))
    '            oNodeTmp.AddIfNo("num", oNode.Param("num"))
    '            oNodeTmp.AddIfNo("ind", "0000")
    '            oNodeTmp.AddIfNo("dec", "00")
    '            oNodeTmp.AddIfNo("orig_col", n1.ToString)
    '            cNodes.Add(oNodeTmp, nColNew.ToString)
    '            If bEnd Then
    '                Exit Do
    '            End If
    '            n1 = n2
    '        Loop
    '    End If

    'End Sub

    Private Function IconForModeString(modeStr As String, ByRef bp As BuildPage) As String
        Dim IconName As String = ""

        Select Case True
            Case modeStr.Contains("פתיחת") OrElse modeStr.Contains("הוספה")
                IconName = bp.m_colColor.Item($"Icon_{24}")
            Case modeStr.Contains("עדכון")
                IconName = bp.m_colColor.Item($"Icon_{7}")
            Case modeStr.Contains("שאילתא") OrElse modeStr.Contains("הצגה")
                IconName = bp.m_colColor.Item($"Icon_{54}")
            Case modeStr.Contains("חידוש")
                IconName = bp.m_colColor.Item($"Icon_{55}")
            Case modeStr.Contains("העתקה")
                IconName = bp.m_colColor.Item($"Icon_{53}")
            Case modeStr.Contains("ביטול")
                IconName = bp.m_colColor.Item($"Icon_{35}")
        End Select

        Return $"<i class=""IconForMode fas {IconName}""></i>"
    End Function

    Private Function createActionsPopover(ByRef bp As BuildPage) As String  'ntg 23.06.24 creating the top section of "select multiple" in a table on fgr=B screens

        Dim actionsPopover As String = "<div class=""externalPopoverWrapper"">"

        actionsPopover += "<div class='checkAllRowsWrapper'>
        <input class='form-check-input' type='checkbox' id='checkAllRowsCheckbox' onclick='toggleAllTableRows(this)'>
        <label class='form-check-label' for='checkAllRowsCheckbox'>
            בחר הכל
        </label>
        </div>"

        actionsPopover += " <div id='LinksPopOverButtonWrapper' class='tooltip-wrapper' title='יש לבחור יותר מפריט אחד'>
                                <button id='LinksPopOverButtonExternal' type='button' title='' class='LinksPopOverButtonExternal'  disabled>
                                    פעולות נוספות
                                </button>
                                <div class='info-icon'>
                                    <i class='extraActionsIcon fa-sharp fa-regular fa-info'></i>
                                </div>
                            </div>"

        ' Create a popover window with all the relevant buttons
        Dim ActionButtonsString As String = ""

        For Each ActionNodeStr As String In bp.ActionNodes
            'Dim ascombo As String = ActionNodeStr.Replace("button", "option")

            ActionNodeStr = ActionNodeStr.Replace("button", "a") ' PopOver works with a (hyperlink) and not with buttons

            'ntg 29.05.24 code section that adds the  "CheckTheRow(this);" function to a row in the table in which an action was selected (table with icons)
            Dim newFunctionCall As String = "CheckTheRow(this);"
            Dim onclickPosition As Integer = ActionNodeStr.IndexOf("onclick=""")
            If onclickPosition <> -1 Then
                Dim insertPosition As Integer = onclickPosition + 9
                ActionNodeStr = ActionNodeStr.Insert(insertPosition, newFunctionCall)
            End If

            Dim ActionNodeElement As XElement = XDocument.Parse(ActionNodeStr).Root

            ActionNodeElement.SetAttributeValue("class", $"px-1 btn list-group-item popOverItem LinksPopOverItem {If(bp.RetIsEnglish, "leftAlign", "")}") 'ntg 06.02.23 'ntg 20.05.24 changes in appearence regarding screens with fgr=P-showing button instead of list of actions
            'ActionNodeElement.SetAttributeValue("onclick", $"{If(isFgrP, "selectTr();", "")}") 'ntg 06.02.23 'ntg 20.05.24 changes in appearence regarding screens with fgr=P-showing button instead of list of actions
            ActionButtonsString += ActionNodeElement.ToString()
        Next
        '<ul id='popover-content' class='d-none list-group'>   {ActionButtonsString}       </ul>
        actionsPopover += $"</div>"

        Return actionsPopover
    End Function

End Class