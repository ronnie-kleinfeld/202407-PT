Module Common
    Public BOOTSTRAP As Boolean = True
    Public AllStyles As String() = {"BtnCom", "BtnComCur", "BtnComFoc", "BtnField", "BtnFieldFoc",
        "BtnFolder", "BtnFolderCur", "BtnFolderFoc",
        "BtnFolderE", "BtnFolderCurE", "BtnFolderFocE",
        "BtnFolderB", "BtnFolderCurB", "BtnFolderFocB",
        "BtnFolderO", "BtnFolderCurO", "BtnFolderFocO",
        "FirstLine", "SecondLine", "SecondLineEnv", "TopOut1", "TopOut2", "ErrMsg",
        "menu", "menu a", "menu a:hover", "JobNumberStyle", "LastEntryStyle"}
    Public AllStyles_Menu As String() = {"menu", "menu a:hover"}
    Public AllStyleSuffixes_Grid1 As String() = {"", "0"}
    Public AllStyleSuffixes_Grid2 As String() = {"T", "S", "L1", "L2", "LF"}
    Public AllParams_Screen As String() = {"fch", "fgr", "fps", "adr", "plgc", "wait", "nopb",
        "dll", "pml", "arw", "nfr", "agr", "rdf", "stp", "dspa", "hov"}
    Public AllParams_Screen0 As String() = {"fdate", "flang", "flr", "fdsp", "fwt", "fru", "dtr", "jacket"}
    Public AllParams_Screen000 As String() = {"cli", "ffk", "ver"}
    Public AllParams_ScreenDllW As String() = {"dll", "esc", "sec", "fru", "fnn"}
    Public AllParams_Field As String() = {"pxm", "psz", "pf4", "ptp", "psl", "sgn", "edt", "only", "heb", "chb", "pwd", "cry", "lvl",
        "rtp", "dfs", "eml", "bua", "hlp", "uni", "f4p", "man", "plc", "ewr"}
    Public AllParams_Field0 As String() = {"pxk", "pxn", "pnt", "tch", "pkv", "clv"}
    Public AllParams_Field00 As String() = {"pfk"}
    Public AllParams_Field000 As String() = {"rus", "pic"}
    Public AllParams_FieldN As String() = {"phi", "pri", "pul"}
    Public AllParams_Cmd As String() = {"lon", "ltr", "pch", "pchl"}
    Public AllParams_Cmd0 As String() = {"lin", "col", "cnf"}
    Public AllFonts As String() = {"", "Usual", "Bold", "Size2", "Size3", "Half", "Bold_Underline", "Size2_Underline", "Underline"}
    Public AllFiles_Js As String() = {"", "Kiosk", "Grid", "Date", "Num", "Time", "Select", "Submit", "Dialog", "Focus", "Print", "Lang", "Resol"}
    Public AllFields_CityStreet As String() = {"sHalves", "sCityCode", "sCityName", "sStreetCode", "sStreetName"}
    Public AllFields_Model1 As String() = {"hMnf", "hIzr", "hTrn", "hDgm", "hSN", "hPer", "hPfk", "hWidth1", "hWidth2", "hHeight1", "sMaxRows"}
    Public AllFields_Model2 As String() = {"hWhat", "hHeight", "hWidth", "hCurrentText"}
    Public AllFields_FlexCombo1 As String() = {"hTbno", "hCurValue", "hTarget", "hPer", "hPfk", "hOrderBy", "sMaxRows"}
    Public AllFields_FlexCombo2 As String() = {"hHeight", "hWidth", "hCurrentText"}
    Public AllGridDirs As String() = {"V", "H"}
    Public AllGridScrolls As String() = {"U", "D", "L", "R"}
    Public AllHiddens As String() = {"fld", "fpch", "find", "fcmd", "cur", "wait", "result100", "text", "client", "fld_per", "find_per"} 'ntg 17.06.24 added "fld_per", "find_per" regarding focus in field-vladi's change
    Public AllHiddens_Value As String() = {"flr", "dll", "agr"}
    Public AllHiddens_FilRec As String() = {"fil", "rec"}
End Module
