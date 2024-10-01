//this file contains functions for Tdi application

function changeWindowSize() {
    var mybutton = document.getElementById("GoToTopBtn");
    mybutton.style.display = "block";

    var FooterHeight = $("footer").outerHeight(true)
    $('main').css('padding-bottom', FooterHeight); // this is to prevent the footer from hiding the content

    $('#MDExtraButtons').css('margin-bottom', FooterHeight)

    mybutton.style.display = "none";
    var DocWidth = document.body.clientWidth

    if (DocWidth < 992) { //Mobile
        //$('#offcanvas').addClass('offcanvas offcanvas-end w-75').removeClass('SideMenu')
        $('#offcanvas').addClass('offcanvas w-75').removeClass('SideMenu') //ntg 11.07.23 removed offcanvas-end because the modal window wasnt shown all the time 
        $('#offcanvas').css('top', '0') //ntg 11.07.23 the side bar was in a different height in each screen and this fixed it in place
        $('.sideBarArea').css('width', '100%'); //ntg 30.04.24 change in appearence after remodelling side bar
        $('.sideBarArea').css('margin-right', '0rem'); //ntg 30.04.24 change in appearence after remodelling side bar

    }
    else { // Large screen
        $('.SideMenu').addClass("show")
        $('.HideOnMobile').removeClass("hide")
        $('.HideOnMobile').addClass("show")
        $('#offcanvas').removeClass('offcanvas').addClass('SideMenu').removeClass('offcanvas-end').removeClass('w-75')
        $('.offcanvas-body').css('overflow-y', 'inherit')
        //$('.sideBarArea').css('width', '15%'); //ntg 30.04.24 change in appearence after remodelling side bar
        $('.sideBarArea').css('width', '13%'); //ntg 30.04.24 change in appearence after remodelling side bar
        $('.sideBarArea').css('margin-right', '1.2rem'); //ntg 30.04.24 change in appearence after remodelling side bar

    }
}

//function InitModal() {
//    $("#Modal").on('show.bs.modal', function () {
//        var InFrame = document.getElementById('ifr100');
//        InFrame.height = '60px';
//        $("#OkButton").off();
//        $("#CancelButton").off();
//    })
//}

function InitModal() {
    $("#Modal").on('show.bs.modal', function () {
        var InFrame = document.getElementById('ifr100');

        if (InFrame && InFrame.src.endsWith('/Empty.htm')) { //ntg  01.11.23 added a difference- when its cityStreet- its 255, else (window i) it 100
            InFrame.style.height = '255px';
        }
        else {
            InFrame.style.height = '100px';
        }

        $("#OkButton").off();
        $("#CancelButton").off();
    });
}


function changeIframeSize() {
    $("#Modal").on('shown.bs.modal', function () {
        var InFrame = document.getElementById('ifr100');
        try {
            if (InFrame.contentWindow.document.body) {
                var InnerScrollHeight = InFrame.contentWindow.document.body.scrollHeight;
                if (InnerScrollHeight > 0)
                    InFrame.height = InnerScrollHeight + 5 + 'px';
            }
        }
        catch (ex) {
            console.error("error in changeIframeSize. Error message is :" + ex.message);
        }
    })
}

//function CloseOnce() {
//    top.opener = top;
//    opener = window;
//    top.open("Empty.htm", "_self");
//    top.close();
//}

// When the user clicks on the button, scroll to the top of the document
function topFunction() {

    var isIE = !!document.documentMode;
    if (isIE) {
        document.documentElement.scrollTop = 0
        document.documentElement.scrollLeft = 0
    }
    else {
        document.documentElement.scrollTo({
            top: 0,
            behavior: "smooth"
        })
    }
}

function goToTheTop() {
    document.documentElement.scrollTo({
        top: 0,
        behavior: "smooth"
    })
}


function topScroll(ScrollTopNumber) {

    $('.card:not(.HideOnMobile)').removeClass("hide")
    $('.card:not(.HideOnMobile)').addClass("show")

    var isIE = !!document.documentMode;
    if (isIE) {
        document.documentElement.scrollTop = ScrollTopNumber
        document.documentElement.scrollLeft = 0
    }
    else {
        $(window).scrollTop(ScrollTopNumber);
    }
}

function ControllGoTopButton() {
    var mybutton = document.getElementById("GoToTopBtn");

    window.onscroll = function () { scrollFunction() };

    function scrollFunction() {
        var rootElement = document.documentElement
        var scrollTop = rootElement.scrollTop
        var scrollTotal = rootElement.scrollHeight - rootElement.clientHeight
        if ((scrollTop / scrollTotal) > 0.8) {
            // Show button
            $(mybutton).show();
            // mybutton.style.display = "block";
        } else {
            // Hide button
            $(mybutton).hide();
            // mybutton.style.display = "none";
        }

        if (rootElement.scrollTop > 10) {
            $("#Hqpxl").val(Math.round(rootElement.scrollTop));
        }
    }
}

function InitInvalidFeedback() {
    $('div.InvalidMan').text(msg_Empty(1))
    $('div.InvalidManNonZero').text(msg_Empty(2))
    $('div.InvalidH').text(msg_Empty(3))
    $('div.InvalidManSelect').text(msg_Empty(4))
}

function InitButtons() {
    $("#C_PrintPdf").html(GetCaption("print"))
}

function InitMaskedInputs() {
    $("[data-inputmask-alias]").removeAttr("maxLength");
    $("[data-inputmask-alias]").inputmask();
    $("[data-inputmask]").inputmask();
}

function GraphBack(e, item) {
    //debugger;
    if (item == undefined) {
        return;
    }
    if (e.srcElement == undefined) {
        return;
    }
    if (item.length == 0) {
        return;
    }
    var canvasId;
    try {
        canvasId = e.srcElement.id;
        var obj = charts.get(canvasId);
        var activePoints = obj.getElementsAtEventForMode(e, 'point', obj.options);
        var firstPoint = activePoints[0];
    } catch (ee) {
        //debugger;
    }
    //var label = obj.data.labels[firstPoint._index];
    //var value = obj.data.datasets[firstPoint._datasetIndex].data[firstPoint._index];

    //find hidden with .graph=canvasId
    //set its value to:
    var canvasValue = (firstPoint._datasetIndex + 1) + "ABCDEFGHIJKLMNOPQRSTUVWXYZ".substr(firstPoint._index, 1);
    $("input[graph='" + canvasId + "']").val(canvasValue);
    doSubmit();

    //tmp:
    //window.open("https"+"://www.go-ins.co.il/","Welcome")
}

function HideCardWithEmptyTable() {
    var AccordionItems = $(".accordion-item").length
    if (AccordionItems > 1) {
        $('table').each(function (a, tbl) {
            //var currentTableDatas = $(tbl).find('tbody td')
            var currentTableDatas = $(tbl).find('td')
            var EmptyTableCells = true
            currentTableDatas.each(function () { if ($(this).text().trim() != '') EmptyTableCells = false; });

            if (EmptyTableCells) {
                var currentAccordionItem = $(this).parents('.accordion-item');
                currentAccordionItem.addClass('d-none')
            }
        });
    }
}
/**
 * The function first iterates through each table element and counts the number of rows in each table.
 * For each table, it then iterates through each th element (i.e., table header) and checks whether its text content is empty.
 * If so, it counts the number of empty cells in the corresponding column, except for cells that contain a switch input element.
 * If the number of empty cells equals the total number of rows in the table, the function hides both the th and all td elements in that column.
 * 
 */
function HideEmptyTableCols(IsGridModal) {
    //  למחוק עמודות בטבלה שכל התאים בעמודה ריקים  
    // A checkBox should not be deleted

    $('table').each(function (a, tbl) {
        var currentTableRows = $(tbl).find('tbody tr').length;
        var NumberColsVisible = 0;
        $(tbl).find('th').each(function (i) {

            if ($(this).text().trim() === '') {
                var remove = 0;
                var currentTable = $(this).parents('table');

                var tds = currentTable.find('tr td:nth-child(' + (i + 1) + ') ');
                tds.each(function (j) { if ($(this).text().trim() === '' && $(this).find('.form-switch').length == 0) remove++; }); // && $(this).find('.form-check-input').length == 0) remove++; });

                if (remove == currentTableRows) {
                    $(this).hide();
                    tds.hide();
                }

                else
                    NumberColsVisible = NumberColsVisible + 1;
            }
            else
                NumberColsVisible = NumberColsVisible + 1;
        });
        if (IsGridModal == 1 && NumberColsVisible < 6)
            $(tbl).addClass('tableFitContent');
    });

    //$("td").has("input.form-check-input").css("width", "3.5%");
    //$("td:not(:has(button))").has("input.form-control:only-child").css("width", "15%");
    //$("td:not(:has(button))").has("input.form-control:not(:only-child)").css("width", "35%");
    //$("td:has(button)").has("input.form-control").css("display", "flex");
    //$("td:has(button)").has("input.form-control").css("display", "table-cell"); //ntg 25.03.24 a certain screen with search field+buttons was presented wrong and this fixed it

}

function ClickRowWithRadioButton() {
    if ($("tr").find('.form-switch').length == 0) {

        $("tr").has(".form-check-input").click(function () {
            $('tr').removeClass('trSelected');
            $(this).addClass("trSelected");

            var RadioButtonObj = $(this).find(".form-check-input")[0]

            $(RadioButtonObj).prop("checked", true);
            fnFocusSL(RadioButtonObj);

        });

        $("tr").has(".form-check-input").dblclick(function () {
            $('tr').removeClass('trDbclick');
            $(this).addClass("trDbclick");

            var RadioButtonObj = $(this).find(".form-check-input")[0]

            $(RadioButtonObj).prop("checked", true);
            fnFocusSL(RadioButtonObj);

            // Call Enter to AS400
            $(this).attr("pch", "00");
            fnBtnClick(this);
        });
    }
}
//ntg 23.06.24 fgr=B is the same as fgr=L, but with option to select multiple rows in a table
function ClickMultipleRowsWithRadioButton() {
    //if ($("tr").find('.form-switch').length == 0) { //--rem ntg 25.07.24 when in modal window, this "if" conflicted with some background screens which made the table in the modal screen to not work properly

    $("tr").has(".form-check-input").click(function () {
        if (!event.target.closest('.btn') && !event.target.closest('a')) { //ntg 25.07.24 when clicking on a row and then clicking on the (...) button after, it unselected the row. now its bypassing the problem
            $(this).toggleClass("trSelected");

            var RadioButtonObj = $(this).find(".form-check-input")[0];

            if ($(this).hasClass("trSelected")) {
                $(RadioButtonObj).prop("checked", true);
                fnFocusSL(RadioButtonObj);

            } else {
                $(RadioButtonObj).prop("checked", false);
            }
            updateActionsButtonState(); //ntg 07.07.24 regarding fgr=B
        }
    });

    $("tr").has(".form-check-input").dblclick(function () {
        $(this).toggleClass("trDbclick");

        var RadioButtonObj = $(this).find(".form-check-input")[0];

        if ($(this).hasClass("trDbclick")) {
            $(RadioButtonObj).prop("checked", true);
            fnFocusSL(RadioButtonObj);

            // Call Enter to AS400
            $(this).attr("pch", "00");
            fnBtnClick(this);
        } else {
            $(RadioButtonObj).prop("checked", false);
        }
    });
    //}
}

function FilterButtons() {
    var value = $('#FilterButtonList').val().toLowerCase();
    $("#ButtonList button").filter(function () {
        $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
    });
}

function UpdateActionButton(elem) {
    //  $("#ActionChooseBtn").text($(elem).text());
    $("#ActionChooseBtn").attr("pch", $(elem).attr("pch"));
}

function SetActionButtonsPopOverExternal() {
    $('.LinksPopOverButtonExternal').popover({
        trigger: 'click',
        html: true,
        placement: 'bottom',
        container: 'body',
        content: function () {
            return $('#popover-content').html();
        }
    });

    // Attach event after initializing popovers
    $('.LinksPopOverButtonExternal').on('shown.bs.popover', function () {
        var as = $(".LinksPopOverItem");
        for (var i = 0; i < as.length; i++) {
            as[i].onclick = function () { CmdPchClick(this); };
            $(as[i]).attr('pch', $(as[i]).attr("id").replace('C_PCH', ''));
        }

        // Hide other popovers
        $('.LinksPopOverButtonExternal').not(this).popover('hide');
    });

    // Document click handler to close popovers when clicking outside
    $(document).on('click', function (e) {
        if ($(e.target).closest('.popover').length === 0 && $(e.target).closest('.LinksPopOverButtonExternal').length === 0) {
            $('.LinksPopOverButtonExternal').popover('hide');
        }
    });
}

//$(document).ready(function () {
//    SetActionButtonsPopOverExternal();
//});


function SetActionButtonsPopOver() {
    $('.LinksPopOver').on('shown.bs.popover', function () {
        var as = $(".LinksPopOverItem");
        for (var i = 0; i < as.length; i++) {
            as[i].onclick = function () { CmdPchClick(this); };
            $(as[i]).attr('pch', $(as[i]).attr("id").replace('C_PCH', ''))
        }
    });

    $('.LinksPopOver').click(function () {
        $('.LinksPopOver').not(this).popover('hide'); //all but this
    });

    $('.LinksPopOver').popover({
        //trigger: 'focus', 
        trigger: 'click',
        html: true,
        placement: 'auto',
        container: 'body',
        content: function () {
            return $('#popover-content').html();
        }
    });

    //with this code, clicking anywhere outside, closes the popOver window - ntg 30.04.23
    $(document).on('click', function (e) {
        if ($(e.target).closest('.popover').length === 0 && $(e.target).closest('.LinksPopOver').length === 0) {
            $('.LinksPopOver').popover('hide');
        }
    });
}

function SetFooterButtonsPopOver() {
    $('.FooterButtonsPopOver').popover({
        content: function () {
            return $('#FooterButtonsPopover-content').html();
        },
        html: true,
        trigger: 'click', // Clicking outside closes the popOver window
        placement: 'bottom'
    });

    $('.FooterButtonsPopOver').on('shown.bs.popover', function () {
        var as = document.getElementsByClassName("FooterButtonsPopOverItem");
        for (var i = 0; i < as.length; i++) {
            as[i].onclick = function () { fnBtnClick(this); };
            $(as[i]).attr('pch', $(as[i]).attr("id").replace('C', ''))
        }
    });

    $(document).on('click', function (e) {
        var
            $popover,
            $target = $(e.target);

        //do nothing if there was a click on popover content
        if ($target.hasClass('FooterButtonsPopOver') || $target.closest('.FooterButtonsPopOver').length) {
            return;
        }

        $('.FooterButtonsPopOver').each(function () {
            $popover = $(this);

            if (!$popover.is(e.target) &&
                $popover.has(e.target).length === 0 &&
                $('.popover').has(e.target).length === 0) {
                $popover.popover('hide');
            }
        });
    })
}

function SetFormSelectDefaultColor() {
    $('.form-select').each(function () {
        SetColorForCombo(this);
    });
}

function setSelect2() {
    $(document).ready(function () {
        $(function () {
            $(".ComboSelect2").select2();
            /*$(".select2").css("width", "300px");*/
        })
    })
}


function setBlinkingButton() {
    $("#C90").addClass("C90")
}


//function transformTableToSelect() {
//    // Get the table element
//    const table = document.querySelector('.table');

//    // Create a select element
//    const select = document.createElement('select');
//    select.setAttribute('class', 'form-control');

//    // Add a default blank option
//    const defaultOption = document.createElement('option');
//    defaultOption.text = "בחר";
//    defaultOption.value = "";
//    select.appendChild(defaultOption);
//    select.selectedIndex = 0;

//    // Loop through each row in the table
//    for (let i = 0; i < table.rows.length; i++) {
//        // Create an option element
//        const option = document.createElement('option');

//        // Get the value of the input in the row
//        const input = table.rows[i].querySelector('input');
//        const value = input.value;
//        const dataId = input.dataset.id;
//        const dataN = input.dataset.sn;

//        // Set the value and text of the option
//        option.value = value;
//        option.text = value;
//        option.setAttribute('data-id', dataId);
//        option.setAttribute('data-sn', dataN);
//        option.classList.add('option')

//        // Add the onclick attribute to the option
//        option.setAttribute('onclick', input.getAttribute('onclick'));

//        // Add the option to the select element
//        select.appendChild(option);
//    }
//    select.addEventListener('change', function () {

//        const options = this.getElementsByTagName('option');
//        for (let i = 0; i < options.length; i++) {
//            options[i].classList.remove('selectedOption');
//        }

//        // Set the selected class on the new selected option
//        const selectedOption = this.options[this.selectedIndex];
//        selectedOption.classList.add('selectedOption');

//        if (this.value !== "") {
//            const valId = this.options[this.selectedIndex].getAttribute('data-id');
//            const valN = this.options[this.selectedIndex].getAttribute('data-sn');
//            ListClick(valN, valId);
//        }
//    });

//    // Replace the table with the select element
//    table.parentNode.replaceChild(select, table);
//}


function UpdateHiddenMark(elem) {
    var HiddenId = $(elem).attr("HiddentFieldRef")

    var HiddenElem = document.getElementById(HiddenId);
    $(HiddenElem).val($(elem).attr("Mark"));
}

function ReplaceLastCard(specialImg) { //ntg 07.02.24 change of image and adding text underneath  //ntg 17.06.24 changing the image and removing the text underneath
    $(".accordion-body").last().addClass('text-center').html(`<img class="custom-img my-2" src="./assets/images/EmptyState${specialImg}.png">`);
    $(".card-body").last().addClass('text-center').html(`<img class="custom-img my-2" src="./assets/images/EmptyState${specialImg}.png">`);
}


function ReplaceLastCardEnglish() {  //ntg 07.02.24 change of image and adding text underneath  //ntg 17.06.24 changing the image and removing the text underneath
    $(".accordion-body").last().addClass('text-center').html('<img class="custom-img my-2" src="./assets/images/EmptyStateImg.png">');
    $(".card-body").last().addClass('text-center').html('<img class="custom-img my-2" src="./assets/images/EmptyStateImg.png">');
}

function dragElement(elmnt) {
    var pos1 = 0, pos2 = 0, pos3 = 0, pos4 = 0;

    if (document.getElementById(elmnt.id + "header")) {
        /* if present, the header is where you move the DIV from:*/
        document.getElementById(elmnt.id + "header").onmousedown = dragMouseDown;
    } else {
        /* otherwise, move the DIV from anywhere inside the DIV:*/
        elmnt.onmousedown = dragMouseDown;
    }

    function dragMouseDown(e) {
        e = e || window.event;
        e.preventDefault();
        // get the mouse cursor position at startup:
        pos3 = e.clientX;
        pos4 = e.clientY;
        document.onmouseup = closeDragElement;
        // call a function whenever the cursor moves:
        document.onmousemove = elementDrag;
    }

    function elementDrag(e) {
        e = e || window.event;
        e.preventDefault();
        // calculate the new cursor position:
        pos1 = pos3 - e.clientX;
        pos2 = pos4 - e.clientY;
        pos3 = e.clientX;
        pos4 = e.clientY;
        // set the element's new position:
        elmnt.style.top = (elmnt.offsetTop - pos2) + "px";
        elmnt.style.left = (elmnt.offsetLeft - pos1) + "px";
    }

    function closeDragElement() {
        /* stop moving when mouse button is released:*/
        document.onmouseup = null;
        document.onmousemove = null;
    }
}

function checkSmartphoneMode() {
    if (window.innerWidth < 768) {
        document.body.classList.add('smartphone-mode');
    } else {
        document.body.classList.remove('smartphone-mode');
    }

}

function DisableFooterBtns() { //ntg 26.06.23 - when an 'i' window pops, disable the footer buttons
    event.stopPropagation();
    console.log('DisableFooterBtns function executed');

    const modalContent = document.querySelector('.modal-content');
    const footerButtonsDiv = document.getElementById('FooterButtonsDiv');

    if (modalContent) {
        footerButtonsDiv.classList.add('disableFooter');
    } else {
        footerButtonsDiv.classList.remove('disableFooter');
    }
}

function UnDisableFooterBtns() { //ntg 26.06.23 - when an 'i' window closes, enable the footer buttons
    //console.log('UnDisableFooterBtns function executed');
    const footerButtonsDiv = document.getElementById('FooterButtonsDiv');

    footerButtonsDiv.classList.remove('disableFooter');
    document.getElementById("Hdialog").value = ""; //ntg 20.12.23 vladi change- bug fix in vanila
}

function changeBorderColor(event) { //ntg 02.07.23 change the forder color of a radio button when its checked
    const clickedElement = event.target;

    if (clickedElement.checked) {
        const formCheckDiv = clickedElement.closest('.form-check');
        if (formCheckDiv) {
            formCheckDiv.style.borderColor = 'var(--ComtecLgog-Light-Blue)';
        }
    }
}

function removeInvalidMsg(event) { //ntg 24.12.23 called when a radio button is clicked- if so, remove the warning msg if there is
    const clickedElement = event.target;

    // Find the parent .form-group
    const formGroup = $(clickedElement).closest(".form-group");

    // Find and remove the is-invalid class from form-check-input elements within the form-group
    formGroup.find(".form-check-input.is-invalid").removeClass("is-invalid");

}

function checkAndToggleSelectDisplay() {
    var select1 = document.getElementById('oStreetList');
    var select2 = document.getElementById('oCityList');

    if (select1) {
        if (select1.options.length === 0) {
            select1.style.display = 'none';
        } else {
            select1.style.display = '';
        }
    }
    if (select2) {
        if (select2.options.length === 0) {
            select2.style.display = 'none';
        } else {
            select2.style.display = '';
        }
    }
}

$(document).ready(function () {
    $(".list-group-checkable").on("change", function () {
        var dynamicValue = $(this)
            .closest(".form-group")
            .find("input[type='hidden']")
            .val();

        if (dynamicValue !== "0") {
            $(this)
                .siblings(".form-check")
                .find(".form-check-input.is-invalid")
                .removeClass("is-invalid");
        }
    });
    hideEmptyRows(); /* ntg 07.01.24 sometimes there would be tables with empty rows, and this function hides any empty rows that there might be */
    $("tr:has(input[type='radio'][title='דאבל-קליק']):not(:has(button))").attr("title", "דאבל-קליק"); /*ntg 17.01.24 if its a table with radio buttons, the double click will appear in hover on all the row*/

});

/* ntg 07.01.24 sometimes there would be tables with empty rows, and this function hides any empty rows that there might be */
function hideEmptyRows() {
    var tables = document.querySelectorAll('.table'); // Adjust the selector if needed

    tables.forEach(function (table) {
        var rows = table.querySelectorAll('tr');
        var allRowsHidden = true;
        var numOfRowsInTable = rows.length; //ntg 28.01.24 another case of just input line appears- needs to be shown
        var searchLineExists = false; //ntg 28.01.24 another case of just input line appears- needs to be shown

        rows.forEach(function (row) {
            // Check if the row is a header row
            var isHeaderRow = Array.from(row.children).every(function (cell) {
                return cell.nodeName.toLowerCase() === 'th';
            });

            // Check if the row contains an input element with a specific class (filtering row)
            //var isFilteringRow = row.querySelector('input.form-control');
            var isFilteringRow = row.querySelector('input.form-control:not([readonly="true"])'); //ntg 14.01.24 a little fix
            var isListInsured = row.innerHTML.includes("ListClick");  //ntg 17.01.24 when its a table representing list of insured

            if (isFilteringRow) {
                searchLineExists = true;
            }
            if ((!isHeaderRow && !isFilteringRow)) {
                var cells = row.querySelectorAll('td');
                var allEmpty = true;

                cells.forEach(function (cell) {
                    if (cell.textContent.trim() !== '') {
                        allEmpty = false;
                    }
                });

                if (isListInsured)
                    allEmpty = false;
                if (allEmpty) {
                    row.style.display = 'none';
                } else {
                    row.style.display = ''; // Reset to default display value
                    allRowsHidden = false; // At least one row is visible
                }
            }
        });

        // Check if all rows in the table are hidden, hide the entire table
        if (allRowsHidden && numOfRowsInTable > 3 && !searchLineExists) { //ntg 28.01.24 - && numOfRowsInTable > 3 && !searchLineExists - another case of just input line appears- needs to be shown
            table.style.display = 'none';
        } else {
            table.style.display = ''; // Reset to default display value
        }
    });
}


/* ntg 28.02.24 in a specific screen, the area code fields will be more narrow */
function areaCodeFieldWidth() {
    // Get all input elements within the specified parent container
    const inputs = document.querySelectorAll('.justConSpceBtwn input');

    // Iterate over each input element
    inputs.forEach(input => {
        // Get the corresponding label element using the for attribute
        const label = document.querySelector(`label[for="${input.id}"]`);

        // Check if the label exists and contains the substring "FCELKID"
        if (label && (input.id.includes('FCELKID') || input.id.includes('FFAXKID'))) {
            // Add a class to the parent div to trigger CSS changes
            input.parentNode.classList.add('with-minus-label');
        }
    });
}

//ntg 05.03.24 in the special date field- vladi's func to update the value sent to 400
function PutDate(sN, d) {
    //$("#X" + sN).val(d);
    document.getElementById("X" + sN).value = d; //vk 08.24+++
    CopyOut(document.getElementById("X" + sN));
}
//function ShowDate(sN) {
//    document.getElementById("X" + sN).show();
//}

//ntg 05.03.24 in the special date field- update selected btns color + date range value accordingly
function changeDateColor(event) { //vk 09.24 changes
    // Get all elements with class 'dateSpecialButtons'
    const clickedElement = event.target;
    if (clickedElement) {
        const allDateButtons = document.querySelectorAll('.dateSpecialButtons');
        /*
        const dateRange = document.getElementById('displayDateRange');
        const [year, month, day] = d.split('-');
    
        // Rearrange the components in the desired format (dd-mm-yyyy)
        const flippedDate = `${day}/${month}/${year}`;
        dateRange.innerHTML = "";
        dateRange.innerHTML = `${flippedDate}-${calculateNextYearDate(d)}`;
        */
        allDateButtons.forEach(button => {
            clickedElement.classList.add('dateSpecialButtons')
        });
        // Change the border color of the clicked radio button
        /*
            changeBorderDateBtn(event);
        }

        //ntg 05.03.24 in the special date field- Change the border color of the clicked radio button
        function changeBorderDateBtn(event) {
        */

        clickedElement.classList.add('dateSpecialButtonsSelected')
    }
}
//ntg 05.03.24 in the special date field- if a date was selected inside the date-picker, update all elements accordingly
function updDateRange(event) {

    const clickedElement = event.value;
    if (clickedElement) {
        changeDateColor(event, clickedElement);
    }
}

//ntg 05.03.24 in the special date field - calculating the next year for the insurance date range
function calculateNextYearDate(inputDate) {
    const [year, month, day] = inputDate.split('-').map(Number);
    const currentDate = new Date(year, month - 1, day);
    const nextYearDate = new Date(currentDate.getFullYear() + 1, month - 1, day);

    nextYearDate.setDate(nextYearDate.getDate() - 1);
    const formattedNextYearDate = `${nextYearDate.getDate().toString().padStart(2, '0')}/${(nextYearDate.getMonth() + 1).toString().padStart(2, '0')}/${nextYearDate.getFullYear()}`;
    return formattedNextYearDate;
}

//ntg 05.03.24 function for special field description (to replace the i window)
function fixHelpText() {
    var helpTextElements = document.getElementsByClassName("specialHelpText");

    for (var i = 0; i < helpTextElements.length; i++) {
        var helpText = helpTextElements[i].innerHTML;
        helpText = helpText.replace(/([^\.])\\n/g, "$1 ");
        helpText = helpText.replace(/\.\\n/g, '.<br>');
        helpTextElements[i].innerHTML = helpText;
    }
}

//ntg 07.04.24-tryouts range of field
function applyStyleToFormControl() {
    var rangeWrapper = document.getElementById('rangeWrapper');
    if (rangeWrapper) {
        var formControlParent = findParent(rangeWrapper, 'form-group');
        if (formControlParent) {
            formControlParent.style.flexDirection = 'column';
        }
    }
}

//ntg 07.04.24-tryouts range of field
function findParent(element, className) {
    var parent = element.parentElement;
    while (parent) {
        if (parent.classList.contains(className)) {
            return parent;
        }
        parent = parent.parentElement;
    }
    return null;
}

function toggleFooterButtons() { //ntg 30.04.24 function to hide the footer button in case of an opened side bar in smartphone mode
    // Check if the offcanvas backdrop is shown (the sidebar menu is open)
    if ($('.offcanvas-backdrop.fade.show').length) {
        // If shown, hide the FooterButtonsDiv
        $('#FooterButtonsDiv').attr('style', 'display: none !important');
    } else {
        // If not shown, show the FooterButtonsDiv
        $('#FooterButtonsDiv').removeAttr('style');
    }
}

//vk 09.24
function Accords_Apply(list) {
    if (list.length > 0) {
        var sp = list.split(",");
        for (var param in sp) {
            if (document.getElementById(sp[param]))
                toggleCardAccordion(document.getElementById(sp[param]), "open");
        }
    }
}
function Accords_Gather() {
    var s = "";
    $(".subCardA").each(function () {
        if (toggleCardAccordion(this, "check"))
            s += "," + this.id;
    });
    $("#Haccords").val(s.substr(1));
}

function toggleCardAccordion(button, what) { //vk 09.24 changes
    // Ensure the button is correctly referenced
    if (button.tagName.toLowerCase() !== 'button') {
        button = button.closest('button');
    }

    // Find the closest element with the class "subCardA_Data"
    var container = button.closest('.row').parentNode;  // .row's parent is the container
    var subCardA_Data = container.querySelector('.subCardA_Data');

    if (subCardA_Data) {
        // Toggle between accClosed and accOpen
        if (subCardA_Data.classList.contains('accClosed')) {
            if (what == 'check') return false;
            if (what == 'close') return;
            subCardA_Data.classList.remove('accClosed');
            subCardA_Data.classList.add('accOpen');
        } else if (subCardA_Data.classList.contains('accOpen')) {
            if (what == 'check') return true;
            if (what == 'open') return;
            subCardA_Data.classList.remove('accOpen');
            subCardA_Data.classList.add('accClosed');
        }
        if (what == 'check') return false;

        // Find the <i> element with class "subTitleIcon" inside the button
        var subTitleIcon = button.querySelector('.subTitleIcon');
        if (subTitleIcon) {
            // Toggle between "fa-square-plus" and "fa-square-minus"
            if (subTitleIcon.classList.contains('fa-square-plus')) {
                subTitleIcon.classList.remove('fa-square-plus');
                subTitleIcon.classList.add('fa-square-minus');
            } else if (subTitleIcon.classList.contains('fa-square-minus')) {
                subTitleIcon.classList.remove('fa-square-minus');
                subTitleIcon.classList.add('fa-square-plus');
            }
        }
    }
}

//ntg 24.06.24  trials for fixing card
function retrieveElementHeight() {
    var fixedElement = document.querySelector('.cardNumber_1');
    if (fixedElement) {
        // Capture the element's width before making it fixed
        var elementWidth = fixedElement.offsetWidth;
        var elementHeight = fixedElement.offsetHeight;

        console.log('Element height: ' + elementHeight + 'px');
        console.log('Element width: ' + elementWidth + 'px');

        // Create a placeholder element
        var placeholder = document.createElement('div');
        placeholder.style.height = elementHeight + 'px';
        placeholder.style.width = elementWidth + 'px';
        placeholder.style.backgroundColor = 'red'; // Optional, for visual consistency

        // Insert the placeholder after the element
        fixedElement.parentNode.insertBefore(placeholder, fixedElement.nextSibling);

        // Set element to fixed position with top, width, and height
        fixedElement.style.position = 'fixed';
        fixedElement.style.top = '5rem'; // Adjust as needed
        fixedElement.style.width = elementWidth + 'px'; // Maintain the width
        fixedElement.style.height = elementHeight + 'px'; // Maintain the height
        fixedElement.style.zIndex = '999';
    }
}

//ntg 07.07.24 regarding fgr=B - "select all" mode
function toggleAllTableRows(checkbox) {
    var isChecked = checkbox.checked;
    var container = checkbox.closest('.table-responsive');
    var closestTable = container ? container.querySelector('table') : null;

    if (closestTable) {
        var rows = closestTable.getElementsByTagName('tr');

        // Loop through rows starting from index 1 (skip the header row)
        for (var i = 1; i < rows.length; i++) {
            var row = rows[i];

            // ntg 09.07.24 - Check if the row has a td with a button that has the class 'LinksPopOver'
            var hasLinksPopOverButton = Array.from(row.getElementsByTagName('td')).some(td => {
                return Array.from(td.getElementsByTagName('button')).some(button => button.classList.contains('LinksPopOver'));
            });

            //ntg 09.07.24 - Add or remove the class 'trSelected' only if the row does NOT have 'LinksPopOver'
            if (hasLinksPopOverButton) {
                if (isChecked) {
                    row.classList.add('trSelected');
                } else {
                    row.classList.remove('trSelected');
                }
            }
        }
    }

    //ntg 07.07.24 regarding fgr=B - Toggle label text
    var label = checkbox.nextElementSibling;
    if (label) {
        if (isChecked) {
            label.textContent = 'בטל בחירה';
        } else {
            label.textContent = 'בחר הכל';
        }
    }
    updateActionsButtonState(); //ntg 07.07.24 regarding fgr=B
}

function updateActionsButtonState() { //ntg 07.07.24 regarding fgr=B
    var button = document.getElementById('LinksPopOverButtonExternal');
    var selectedCount = document.querySelectorAll('.trSelected').length;
    var countTableLines = document.querySelectorAll('tbody tr').length; //ntg 24.07.24 added the condition to check how many rows are in the table, so that when its 1, the button wont be disabled
    button.disabled = selectedCount <= 1 && countTableLines != 1;
}

function adjustSidebar() { //ntg 21.07.24 changes regarding sidebar fields- regarding design
    const fieldsSideBarSection = document.querySelector('.fieldsSideBarSection');

    if (fieldsSideBarSection) {
        // If found, adjust the height of the element with class 'sideBarArea'
        const sideBarArea = document.querySelector('.sideBarArea');
        if (sideBarArea) {
            sideBarArea.style.height = 'calc(100% - 23.5rem)';
        }

        // Adjust the bottom attribute of the element with class 'sidebarBtns'
        const sidebarBtns = document.querySelector('.sidebarBtns');
        if (sidebarBtns) {
            sidebarBtns.style.bottom = '20rem';
        }

        const copyRightDiv = document.getElementById('CopyRightDiv');
        if (copyRightDiv) {
            copyRightDiv.style.position = 'fixed';
            copyRightDiv.style.bottom = '1.6rem';
            copyRightDiv.style.right = '7rem';
        }

        const topButtonDiv = document.getElementById('topButtonDiv');
        if (topButtonDiv) {
            topButtonDiv.style.display = 'none';
        }
    }
}