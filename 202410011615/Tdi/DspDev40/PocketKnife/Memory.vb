Imports Comtec.Tis

Public Class Memory 'vk 09.06

    'Dim m_conv As New ConvCom()
    'Dim m_sLang As String
    Dim c0 As New Hashtable()

    Public Class Record
        Public nNoin, nDel As Integer, sMtxt, sExcl As String, bHebrew As Boolean
    End Class
    Private Class Group
        Friend nKod, nTbNo As Integer, List As New SortedList()
    End Class

    Public Sub AddItem(ByVal sData As String, ByVal flr As String)

        Dim nKod, nTbno, nNoin, nDel As Integer, sMtxt, sExcl As String
        Dim r As Record

        nKod = nz(sData.Substring(1, 2))
        nTbno = nz(sData.Substring(3, 3))
        nNoin = nz(sData.Substring(6, 9))
        sMtxt = sData.Substring(15, 60)
        'If flr = "0" Then
        '    sMtxt = m_conv.RevHeb(sMtxt, m_sLang)
        'End If
        nDel = nz(sData.Substring(75, 1))
        sExcl = sData.Substring(76, 3) 'vk 03.10

        r = New Record()
        r.nNoin = nNoin
        r.sMtxt = sMtxt
        r.nDel = nDel
        r.bHebrew = (flr = "0")
        r.sExcl = sExcl 'vk 03.10

        AddOne(nKod, nTbno, True, r, nNoin.ToString("000000000"))
        AddOne(nKod, nTbno, False, r, sMtxt.ToLower) 'vk 10.06

    End Sub

    Public Sub Clear()
        Dim g As Group, o As Object
        For Each o In c0
            g = o.Value
            g.List.Clear()
        Next
    End Sub
    Public Sub ClearByKey(ByVal nKod As Integer)
        Dim g As Group, o As Object
        For Each o In c0
            g = o.Value
            If g.nKod = nKod Then g.List.Clear()
        Next
    End Sub
    Public Sub ClearByTwoKeys(ByVal nKod As Integer, ByVal nTbNo As Integer) 'vk 11.07
        Dim g As Group, o As Object
        For Each o In c0
            g = o.Value
            If g.nKod = nKod AndAlso g.nTbNo = nTbNo Then g.List.Clear()
        Next
    End Sub
    Public Function CountByTwoKeys(ByVal nKod As Integer, ByVal nTbno As Integer) As Integer
        Try
            Dim List As SortedList
            List = c0(KeyString(nKod, nTbno, True))
            Return List.Count
        Catch
            Return 0
        End Try
    End Function
    Public Function ListByTwoKeys(ByVal nKod As Integer, ByVal nTbno As Integer, ByVal bOrderByCode As Boolean) As SortedList
        Dim Gro As Group, sOuterKey As String
        sOuterKey = KeyString(nKod, nTbno, bOrderByCode)
        If c0.ContainsKey(sOuterKey) Then
            Gro = c0(sOuterKey)
            Return Gro.List
        Else
            Return New SortedList()
        End If
    End Function

    Private Function nz(ByVal s As String) As Integer
        Try
            Return Int64.Parse(s.Trim)
        Catch
            Return 0
        End Try
    End Function
    Private Function KeyString(ByVal nKod As Integer, ByVal nTbno As Integer, ByVal bOrderByCode As Boolean) As String
        Return nKod.ToString("00") + nTbno.ToString("000") + IIf(bOrderByCode, "y", "n")
    End Function
    Private Sub AddOne(ByVal nKod As Integer, ByVal nTbno As Integer, ByVal bOrderByCode As Boolean, _
            ByRef r As PocketKnife.Memory.Record, ByVal sInnerKey As String)
        Dim Gro As Group, sOuterKey As String
        sOuterKey = KeyString(nKod, nTbno, bOrderByCode)
        If c0.ContainsKey(sOuterKey) Then
            Gro = c0(sOuterKey)
        Else
            Gro = New Group()
            Gro.nKod = nKod
            Gro.nTbNo = nTbno 'vk 11.07
            c0.Add(sOuterKey, Gro)
        End If
        Do While Gro.List.ContainsKey(sInnerKey) AndAlso Len(sInnerKey) < 1000
            sInnerKey += "z"
        Loop
        Gro.List.Add(sInnerKey, r)
    End Sub

End Class
