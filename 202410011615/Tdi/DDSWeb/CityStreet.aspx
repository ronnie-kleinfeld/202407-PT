<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CityStreet.aspx.cs" Inherits="DDSWeb.CityStreet" %>

<!DOCTYPE html>
<html lang="he" dir="rtl">

<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <link rel="stylesheet" href="assets/styles/font.css" />
    <link rel="stylesheet" href="assets/styles/fa-pro-6.1.2/fa-pro-6.1.2/css/all.css" />
    <link rel="stylesheet" href="assets/bootstrap5/css/bootstrap.css" />
    <link rel="stylesheet" href="assets/styles/custom.css" />
    <link rel="stylesheet" href="assets/styles/fnxHome_1.css" />

    <style>
        /* CSS code to hide scrollbars */
        /* Hide scrollbar for Chrome, Safari, and Opera */
        ::-webkit-scrollbar {
            width: 0.5em;
        }

        ::-webkit-scrollbar-track {
            background-color: transparent;
        }

        ::-webkit-scrollbar-thumb {
            background-color: transparent;
        }

        /* Hide scrollbar for Firefox */
        html {
            scrollbar-width: none;
        }
    </style>
    <title>comtec</title>
    <%=Application["META"]%>
    <script src="js/ddsMsgHeb.js"></script>
    <script>
        //document.addEventListener('DOMContentLoaded', function () {
        //    var inputElement = document.getElementById('oCity');
        //    var excludedElement = document.getElementById('oCityList');

        //    var inputElement = document.getElementById('oStreet');
        //    var excludedElement = document.getElementById('oStreetList');

        //    inputElement.addEventListener('blur', function (event) {
        //        var target = event.relatedTarget;
        //        if (target !== excludedElement) {
        //            // Your onblur logic here
        //            hideCityList();
        //        }
        //    });
        //    inputElement.addEventListener('blur', function (event) {
        //        var target = event.relatedTarget;
        //        if (target !== excludedElement) {
        //            // Your onblur logic here
        //            hideStreetList();
        //        }
        //    });
        //});
        //document.addEventListener('DOMContentLoaded', function () {
        //    var cityInput = document.getElementById('oCity');
        //    var cityList = document.getElementById('oCityList');

        //    var streetInput = document.getElementById('oStreet');
        //    var streetList = document.getElementById('oStreetList');

        //    cityInput.addEventListener('blur', function (event) {
        //        var target = event.relatedTarget;
        //        if (target !== cityList) {
        //            // Your onblur logic here
        //            hideCityList();
        //        }
        //    });

        //    streetInput.addEventListener('blur', function (event) {
        //        var target = event.relatedTarget;
        //        if (target !== streetList) {
        //            // Your onblur logic here
        //            hideStreetList();
        //        }
        //    });
        //});
        document.addEventListener('DOMContentLoaded', function () {

            //var modalHeader = document.querySelector('.modal-header');
            //var modalHeaderText = document.getElementById('ModalHeaderText');

            //if (modalHeaderText && modalHeader) {
            //    modalHeader.style.display = 'none';
            //}

            var cityInput = document.getElementById('oCity');
            var cityList = document.getElementById('oCityList');

            var streetInput = document.getElementById('oStreet');
            var streetList = document.getElementById('oStreetList');

            cityInput.addEventListener('blur', function (event) {
                var target = event.relatedTarget;
                if (target !== cityList) {
                    // Your onblur logic here
                    hideCityList();
                }
            });

            streetInput.addEventListener('blur', function (event) {
                var target = event.relatedTarget;
                if (target !== streetList) {
                    // Your onblur logic here
                    hideStreetList();
                }
            });

            cityList.addEventListener('change', function () {
                hideCityList();
            });

            streetList.addEventListener('change', function () {
                hideStreetList();
            });

            //var modalFooter = document.querySelector('.modal-footer.popUpFooter');
            //modalFooter.addEventListener('click', DisableFooterBtns);

            var modal = document.getElementById('Modal');

            modal.addEventListener('click', function (event) {
                event.preventDefault();
                event.stopPropagation();
            });
        });


        /*
        function xxx()
        {
        alert(1);
        $('#Modal').modal('show');
        alert(2);
        }
        */
        var bInValueUp = false;
        var sPrevCity = "";
        var sPrevStreet = "";
        var bClosing = false;

        function Bring(sTable, sWhere, sWhat) {
            document.getElementById("txtTable").value = sTable;
            document.getElementById("txtWhere").value = sWhere;
            document.getElementById("txtCurrent").value = document.getElementById("o" + sWhat).value;
            document.forms[0].submit();
        }
        function CopyList() {
            var s = document.getElementById("txtWhat").value;
            //document.getElementById("o" + s + "List").outerHTML =
            //"<select ID=\"o"+s+"List\" name=\"o"+s+"List\" class=\"SelInput\" size=9 tabindex=-1 "
            //+"onclick=\"Next(\'"+s+"\',\'click\');\" onkeyup=\"Next(\'"+s+"\',\'keyup\');\">"
            //+ document.getElementById("txtResult").value + "</select>";
            document.getElementById("o" + s + "List").innerHTML = document.getElementById("txtResult").value;
            if (s == "City") {
                EnableStreet();
            }
        }

        //function CityChanged() {
        //    if (!bInValueUp) {
        //        if (document.getElementById("oCity").value != sPrevCity) {
        //            sPrevCity = document.getElementById("oCity").value;
        //            document.getElementById("oStreet").value = "";
        //            Bring("tblCities", "sText like '" + Replace39(document.getElementById("oCity").value) + "%'", "City");
        //        }
        //    }
        //}

        function CityChanged() {
            if (!bInValueUp) {
                var cityInput = document.getElementById("oCity");
                if (cityInput.value.length >= 2 && cityInput.value !== sPrevCity) {
                    sPrevCity = cityInput.value;
                    document.getElementById("oStreet").value = "";
                    Bring("tblCities", "sText like '" + Replace39(cityInput.value) + "%'", "City");
                }
            }
        }


        //function StreetChanged() {
        //    if (!bInValueUp) {
        //        if (document.getElementById("oStreet").value != sPrevStreet) {
        //            sPrevStreet = document.getElementById("oStreet").value;
        //            Bring("tblStreets", "sCity='" + document.getElementById("oCityList").value +
        //                "' and sText like '" + Replace39(document.getElementById("oStreet").value) + "%'", "Street");
        //        }
        //    }
        //}
        function StreetChanged() {
            if (!bInValueUp) {
                var streetInput = document.getElementById("oStreet");
                var cityList = document.getElementById("oCityList");

                if (streetInput.value.length >= 2 && streetInput.value !== sPrevStreet) {
                    sPrevStreet = streetInput.value;

                    if (streetInput.value !== "") {
                        Bring("tblStreets", "sCity='" + cityList.value +
                            "' and sText like '" + Replace39(streetInput.value) + "%'", "Street");

                        // Remove the "hidden" class from oCityList element
                        cityList.classList.remove("hidden");
                    } else {
                        // Add the "hidden" class to oCityList element
                        cityList.classList.add("hidden");
                    }
                }
            }
        }




        function Replace39(t) //vk 02.09
        {
            return t.replace(/\'/g, "'+CHAR(39)+'");
        }

        function Next(s, ev) //???
        {
            document.getElementById("msg").innerText = "";
            if (s == 'City') {
                EnableStreet();
            }
            if (ev == 'keyup_text') {
                if (event.keyCode == 40) {
                    document.getElementById("o" + s + "List").focus();
                }

                if (s == 'City') {
                    CityChanged();

                } else {
                    StreetChanged();
                }
            }
            else {
                ValueUp(s);
                var b = true;
                if (ev == 'keyup') {
                    b = (event.keyCode == 13);
                }
                if (b) {
                    if (s == 'Street' || document.getElementById("oStreet").disabled) {
                        document.getElementById("bOk").focus();
                    } else {
                        document.getElementById("oStreet").focus();
                    }
                }
            }
        }
        function BodyLoad() {
            //with (parent.document.location.href)
            //popup = substring(lastIndexOf("/"))=="/WinModalSubmit.htm";
            //if (!popup)
            //SetMiddle(330,310);
            //vk 05.09 till here
            EnableStreet();
            ValueUp("City");
            ValueUp("Street");
            if (document.getElementById("oCityList").selectedIndex < 0) {
                document.getElementById("oCity").focus();
            } else {
                document.getElementById("oStreet").focus();
            }
        }

        function ValueUp(s) {
            bInValueUp = true;
            if (document.getElementById("o" + s + "List").selectedIndex >= 0) {
                document.getElementById("o" + s).value = document.getElementById("o" + s + "List").options[document.getElementById("o" + s + "List").selectedIndex].text;
            }
            bInValueUp = false;
        }

        function OkClick() {
            var sCity = "";
            var sStreet = "";
            if (document.getElementById("oCityList").selectedIndex < 0) {
                document.getElementById("msg").innerText = msg_Address(1); //vk 12.06
                document.getElementById("oCity").focus();
                return;
            }


            if (document.getElementById("oCityList").selectedIndex >= 0) {
                sCity = document.getElementById("oCityList").options[document.getElementById("oCityList").selectedIndex].text;
            }
            if (document.getElementById("oStreetList").selectedIndex >= 0) {
                sStreet = document.getElementById("oStreetList").options[document.getElementById("oStreetList").selectedIndex].text;
            }
            //if (popup) {
            //    window.returnValue = new Array(document.getElementById("oCityList").value, sCity, document.getElementById("oStreetList").value, sStreet);
            //} else {
            //vk 05.09
            var s = location.search.substring(1);
            var objC, objN, objS, objT;
            with (parent.document) {
                if (getElementById(s + "C")) {
                    objC = getElementById(getElementById(s + "C").value);
                }
                if (objC) {
                    if (getElementById(s + "N")) objN = getElementById(getElementById(s + "N").value);
                    if (getElementById(s + "S")) objS = getElementById(getElementById(s + "S").value);
                    if (getElementById(s + "T")) objT = getElementById(getElementById(s + "T").value);
                    objC.value = document.getElementById("oCityList").value;
                }

                if (objN) objN.value = sCity;
                if (objS) objS.value = document.getElementById("oStreetList").value;
                if (objT) objT.value = sStreet;
                CopyToProtect(objC);
                CopyToProtect(objN);
                CopyToProtect(objS);
                CopyToProtect(objT);
            }
            //}
            CancelClick('y');

        }

        function CopyToProtect(obj) //vk 05.09
        {
            if (obj) {
                if (parent.document.getElementById("P" + obj.name)) {
                    parent.document.getElementById("P" + obj.name).value = obj.value;
                }
                if (parent.document.getElementById("Y" + obj.name)) {
                    parent.document.getElementById("Y" + obj.name).value = obj.value; //vk 06.09
                }
            }
        }
        function CancelClick(v) //vk 05.09
        {
            //if (popup) {
            //    window.close();
            //} else {
            parent.document.getElementById("Hresult100").value = v; //vk 11.09
            bClosing = true;
            //HideAndUnlock();
            //}
        }
        function CancelBlur() //vk 05.09
        {
            if (!bClosing) {
                document.getElementById("oCity").focus();
            }
        }

        function EnableStreet() {
            var b = (document.getElementById("oCityList").selectedIndex < 0);
            if (document.getElementById("txtHalves").value == "1") {
                b = true;
            }
            document.getElementById("oStreet").disabled = b;
            document.getElementById("oStreetList").disabled = b;

            //vk 02.09
            var s = "Street";
            if (document.getElementById("oCityList").selectedIndex < 0) {
                //document.getElementById("o" + s + "List").outerHTML =
                //    "<select id=\"o" + s + "List\" name=\"o" + s + "List\" class=\"SelInput\" size=9 tabindex=-1 "
                //    + "onclick=\"Next(\'" + s + "\',\'click\');\" onkeyup=\"Next(\'" + s + "\',\'keyup\');\">"
                //    + "</select>";
                document.getElementById("o" + s + "List").innerHTML = "";
            }
        }
        function showCityList() {
            var b = document.getElementById("oCityList");
            b.style.display = '';
        }

        function hideCityList() {
            var b = document.getElementById("oCityList");
            b.style.display = 'none';
        }

        function showStreetList() {
            var b = document.getElementById("oStreetList");
            b.style.display = '';
        }

        function hideStreetList() {
            var b = document.getElementById("oStreetList");
            b.style.display = 'none';
        }

        var inputElement = document.getElementById('oCity');
        var excludedElement = document.getElementById('oCityList');


    </script>
</head>

<body onload="BodyLoad();">
    <div class="modal-content" id="ifrCityStreet" style="border-color: transparent; height: 250px !important;">
        <div class="modal-body">
            <div class="row">
                <div class="col-6">
                    <div class="form-group d-flex flex-wrap" style="font-weight: bold;">
                        <label class="form-label col-12">עיר</label>
                        <input type="text" class="form-control" placeholder="" id="oCity" value="<%=sCity%>" oninput="CityChanged();" onfocus="showCityList();" onkeyup="Next('City','keyup_text');" />
                        <select class="form-control custom-select" size="5" id="oCityList" onclick="Next('City','click');" onkeyup="Next('City','keyup');" style="display: none;">
                            <%=sCities%>
                        </select>
                    </div>
                </div>
                <div class="col-6">
                    <div class="form-group d-flex flex-wrap" style="font-weight: bold;">
                        <label class="form-label col-12">רחוב</label>
                        <input type="text" class="form-control" placeholder="" id="oStreet" value="<%=sStreet%>" oninput="StreetChanged();" onfocus="showStreetList();" onkeyup="Next('Street','keyup_text')" />
                        <select class="form-control custom-select" size='5' id="oStreetList" onclick="Next('Street','click');" onkeyup="Next('Street','keyup');" style="display: none;">
                            <%=sStreets%>
                        </select>
                    </div>
                </div>
            </div>
        </div>

        <p class="col-12" id="msg"></p>
        <!--
        <div class="modal-footer">
          <button
            type="button"
            class="btn btn-secondary"
            data-dismiss="modal"
          >
            ביטול
          </button>
          <button type="button" class="btn btn-primary mr-2">אישור</button>
        </div>

		<div dir="rtl" style="width:92%; text-align:right;">
			<span class="Caption" id="msg" style="color:red;height:20px;"></span><br/>
			<input type="button" id="bOk" name="bOk" value="אישור" class="ActionButton" onclick="OkClick();"/>
			&nbsp;
			<input type="button" id="bCancel" name="bCancel" value="ביטול" class="ActionButton" onclick="CancelClick('n');"
			onblur="CancelBlur();"/>
		</div>
-->

        <div style='display: none;'>
            <iframe id='oIFrame' name='oIFrame' src='Empty.htm'></iframe>
            <form id="Form1" target="oIFrame" method="post" action="Bring.aspx">
                <input type="hidden" id="txtDb" name="txtDb" value="<%=sDb%>" />
                <input type="hidden" id="txtHalves" name="txtHalves" value="<%=sHalves%>" />
                <input type="hidden" id="txtTable" name="txtTable" value="" />
                <input type="hidden" id="txtWhere" name="txtWhere" value="" />
                <input type="hidden" id="txtCurrent" name="txtCurrent" value="" />
            </form>
            <input type="hidden" id="txtResult" name="txtResult" value="" />
            <input type="hidden" id="txtWhat" name="txtWhat" value="" />
        </div>
        <script src="assets/scripts/jquery-3.3.1.slim.min.js"></script>
        <script src="assets/scripts/popper.min.js"></script>
        <script src="assets/bootstrap5/js/bootstrap.bundle.min.js"></script>
    </div>
    <script>
        $(document).ready(function () {
            window.parent.$("#OkButton").click(OkClick)
            window.parent.$("#CancelButton").click(function () { CancelClick('n') })

            //if (document.body.scrollHeight > 0) {
            //    var frame = window.parent.$("#ifr100");
            //    frame.height = document.body.scrollHeight + 'px';
            //}
            /*ntg 18.05.23 */
            var IFrameObj = document.getElementById('ifr100');
            IFrameObj.onload = function () {
                IFrameObj.setAttribute('height', '297px'); // Set the iframe height attribute
            };
            // Get a reference to the iframe element

            // Check if the iframe's id is "ifr100" and its src is "Empty.htm"
            if (IFrameObj && IFrameObj.id === 'ifr100' && IFrameObj.src === "Empty.htm") {
                // Set the height to 255px
                IFrameObj.style.height = '255px';
            }

            //modal - header display none

        });

    </script>
</body>
</html>
