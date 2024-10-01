Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.IO   ' NET20
Imports System.Text

Friend Class Multi

    'vk 03.04
    Private qColor As New Color()
    'Private qCombo As New Combo()
    Private qConv As New Conv()
    Private qStyle As New StyleModule()
    Private qSecurity As New Security() 'vk 06.04

    Private sN, sStyle As String
    Private dir As String
    Private sfnClearOptions As String

    Private sTab As String 'vk 01.06
    Private sStyleT As String
    Private ilin, icol, iwidth As Integer

    Private len1 As String ', col1 As String
    Private iLen, iComma, iSign, iPoint, iAdd As Integer

    Private sXML_Combo As String
    Private oControl, oControlT As Control
    Private bNoControl As Boolean
    Private bLongField As Boolean

    Private nPosOfLogo As Integer
    Private Const sBigPoint As String = "*" '"&#149;" 'ChrW(9679)
    'vk 09.06
    Private iRow As Integer, sOptionT As String

    Function Hidden(ByVal sName As String, Optional ByVal sValue As String = "",
            Optional ByVal sParamName As String = "", Optional ByVal sParamVal As String = "") As String 'vk 09.05
        Dim oControlT As New Control()
        oControlT.AddId(sName)
        oControlT.Add("type", "hidden")
        oControlT.Add("value", sValue, True) 'vk 11.07
        If sParamName > "" Then oControlT.Add(sParamName, sParamVal, True)
        Return oControlT.FullCode
    End Function

    Function Logo(ByRef bp As BuildPage, ByRef oStyle As Style,
            ByVal bBkg As Boolean, ByVal bSpool As Boolean) As String 'vk 03.06
        'If bp.m_bLogoBuilt Then Return ""
        'bp.m_bLogoBuilt = True
        Select Case bp.getProperty("LogoClick")
            Case "click", "dblclick" : oStyle.Add("cursor", "pointer")
        End Select
        Dim oControlT As New Control(oStyle.FullCode)
        oControlT.Add("src", "pics/" + bp.getProperty("Logo"))
        oControlT.Add("disabled", "true")
        oControlT.AddId("logo")
        If Not bBkg Then
            Select Case bp.getProperty("LogoClick")
                Case "click", "dblclick" : oControlT.Add("on" + bp.getProperty("LogoClick"), bp.getProperty("LogoAction"))
            End Select
        End If
        'vk 02.08
        'Dim nLogoScaleBase As String = bp.getPropertyVal("LogoScaleBase")
        'Dim nResol As Integer = bp.ResolValue(800, 1024, 1440, Not bSpool, "hor") 'vk 05.09
        'If nLogoScaleBase > 0 AndAlso nLogoScaleBase <> nResol Then
        Dim nCoef As Single = bp.LogoCoef(bSpool) 'nResol / nLogoScaleBase
        If nCoef <> 1 Then
            oControlT.Add("width", Int(bp.m_nLogoW * nCoef).ToString)
            oControlT.Add("height", Int(bp.m_nLogoH * nCoef).ToString)
        End If
        'End If
        'vk 08.06
        Return oControlT.FullCode("img")
    End Function

    Function FromFile(ByVal sFile As String) As String 'vk 02.05, 03.10

        Dim fs As FileStream = Nothing, sw As StreamReader = Nothing, s As String
        Try
            fs = New FileStream(sFile, FileMode.Open, FileAccess.Read, FileShare.Read)
            sw = New StreamReader(fs)
            s = sw.ReadToEnd
            'If sFrom > "" Then s = s.Replace(sFrom, sTo) 'vk 07.07
            Return s
        Catch
            Return ""
        Finally
            Try
                sw.Close()
            Catch
            End Try
            Try
                fs.Close()
            Catch
            End Try
            Try
                sw.Dispose()
            Catch
            End Try
            Try
                fs.Dispose()
            Catch
            End Try
            sw = Nothing
            fs = Nothing
        End Try

    End Function

    Sub Dispose()
        qColor.Dispose()
        qColor = Nothing
        'qCombo.Dispose()
        'qCombo = Nothing
        qConv = Nothing
        qStyle = Nothing
        qSecurity = Nothing
    End Sub

End Class
