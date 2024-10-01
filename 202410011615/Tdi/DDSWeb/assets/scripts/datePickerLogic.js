const pickers = document.querySelectorAll("duet-date-picker");
const DATE_FORMATD = /^(\d{1,2})\/(\d{1,2})\/(\d{4})$/;
const DATE_FORMATY = /^(\d{1,2})\/(\d{1,2})\/(\d{4})$/;
const DATE_FORMATHQ = /^(\d{1,2})\/([12]\d{3})$/;

let LangValue = document.querySelector("html").getAttribute("lang")
let bEnglish = (LangValue == "en")

if (window.NodeList && !NodeList.prototype.forEach) {
    NodeList.prototype.forEach = Array.prototype.forEach;
}

pickers.forEach(function (picker) {
    let DateFormatValue = picker.getAttribute("DFormat");
    let DATE_FORMAT = "";
    let LocalPlaceHolder = "";
    //let DatePickerObj = document.querySelector("duet-date-picker")

    switch (DateFormatValue) {
        case "DDMMYY":
            DATE_FORMAT = DATE_FORMATD
            LocalPlaceHolder = (bEnglish == true) ? "day/month/year" : "שנה/חודש/יום"
            break;
        case "DDMMYYYY":
            DATE_FORMAT = DATE_FORMATY
            LocalPlaceHolder = (bEnglish == true) ? "day/month/year" : "שנה/חודש/יום"
            break;
        case "MMYYYY":
            DATE_FORMAT = DATE_FORMATHQ
            LocalPlaceHolder = (bEnglish == true) ? "month/year" : "שנה/חודש"
            break;
    }

    picker.dateAdapter = {
        parse: function parse(value, createDate) {
            const matches = value.match(DATE_FORMAT);
            if (matches) {
                switch (DateFormatValue) {
                    case "DDMMYY":
                    case "DDMMYYYY":
                        return createDate(matches[3], matches[2], matches[1]);
                    case "MMYYYY":
                        return createDate(matches[2], matches[1], 1);
                }
            }
        },
        change: picker.onchange(),
        focus: picker.onfocus(),
        blur: (picker.onblur != null ? picker.onblur() : null),
        format: function format(date) {
            switch (DateFormatValue) {
                case "DDMMYY":
                case "DDMMYYYY":
                    return date.getDate() + "/" + (date.getMonth() + 1) + "/" + date.getFullYear()
                case "MMYYYY":
                    return (date.getMonth() + 1) + "/" + date.getFullYear()
            }
        },
    };

    if (bEnglish == true) {
        picker.localization = {
            buttonLabel: "choose a day",
            placeholder: LocalPlaceHolder,
            selectedDateMessage: "chosen date",
            prevMonthLabel: "previous",
            nextMonthLabel: "next",
            monthSelectLabel: "month",
            yearSelectLabel: "year",
            closeLabel: "closed",
            keyboardInstruction: "you can use the arrows to navigate the months",
            calendarHeading: "Choose a date",
            dayNames: ["S", "M", "T", "W", "T", "F", "S"],
            monthNames: [
                "January",
                "February",
                "March",
                "April",
                "May",
                "June",
                "July",
                "August",
                "September",
                "October",
                "November",
                "December",
            ],
            monthNamesShort: [
                "Jan",
                "Feb",
                "Mar",
                "Apr",
                "May",
                "Jun",
                "Jul",
                "Aug",
                "Sep",
                "Oct",
                "Nov",
                "Dec",
            ],
        };
    }
    else {
        picker.localization = {
            buttonLabel: "בחר יום",
            placeholder: LocalPlaceHolder,
            selectedDateMessage: "התאריך הנבחר",
            prevMonthLabel: "הקודם",
            nextMonthLabel: "הבא",
            monthSelectLabel: "חודש",
            yearSelectLabel: "שנה",
            closeLabel: "סגור",
            keyboardInstruction: "ניתן להשתמש בחצים לניווט בין החודשים",
            calendarHeading: "בחר תאריך",
            dayNames: ["א", "ב", "ג", "ד", "ה", "ו", "ש"],
            monthNames: [
                "ינואר",
                "פברואר",
                "מרץ",
                "אפריל",
                "מאי",
                "יוני",
                "יולי",
                "אוגוסט",
                "ספטמבר",
                "אוקטובר",
                "נובמבר",
                "דצמבר",
            ],
            monthNamesShort: [
                "ינו",
                "פבר",
                "מרץ",
                "אפר",
                "מאי",
                "יונ",
                "יול",
                "אוג",
                "ספט",
                "אוק",
                "נוב",
                "דצמ",
            ],
        };
    }
});

let dateInput = "";
window.addEventListener("load", function () {
    setTimeout(function () {
        dateInputsDivs = document.querySelectorAll(".duet-date-picker");
        dateInputsDivs.forEach(function (dateInputItem) {

            let dateTable = dateInputItem.querySelector(".duet-date__dialog");
            dateInput = dateInputItem.querySelector(".duet-date__input");

            if ($(dateInput).prop('required')) {
                $(dateInput).after(function () {
                    return "<div class='invalid-feedback InvalidMan' style='top: 90%; position: absolute;'>" + msg_Empty(1) + "</div>"; //added styling for the required field
                });

                let ToggleElem = dateInputItem.querySelector(".duet-date__toggle");
                if (bEnglish) {
                    ToggleElem.style.left = "auto";
                }

                $(ToggleElem).height($(dateInput).innerHeight())
            }

            dateInputItem.addEventListener("duetChange", function (e) {
                dateInputItem.onchange();

                if (dateInputItem.querySelector(".duet-date__input") !== document.activeElement) {
                    if (dateInputItem.onblur !== null)
                        dateInputItem.onblur();
                }
            });

            dateInputItem.addEventListener("duetFocus", function () {
                dateInputItem.onfocus();
            });

            dateInputItem.addEventListener("duetBlur", function () {
                dateInputItem.onblur();
            });

            let DateFormatValue = dateInputItem.getAttribute("DFormat");

            switch (DateFormatValue) {
                case "DDMMYY":
                case "DDMMYYYY":
                    dateInput.maxLength = 10;
                    dateInput.minLength = 10; //ntg 28.03.24 it didnt sent a response to 400 if the value wasnt in the right length
                    break;
                case "MMYYYY":
                    dateInput.maxLength = 7;
                    dateInput.minLength = 7; //ntg 28.03.24 it didnt sent a response to 400 if the value wasnt in the right length
                    break;
            }

            var node = document.createElement("BUTTON");
            node.setAttribute("type", "button");
            if (bEnglish)
                node.textContent = "Today";
            else
                node.textContent = "היום";

            node.classList.add('duet-date-today');
            node.onclick = function () {
                dateTable.classList.toggle("is-active");
                dateTable.setAttribute("aria-hidden", true);

                const d = new Date();
                let month = String(d.getMonth() + 1);
                let day = String(d.getDate());
                const year = String(d.getFullYear());

                if (month.length < 2) month = "0" + month;
                if (day.length < 2) day = "0" + day;

                switch (DateFormatValue) {
                    case "DDMMYY":
                    case "DDMMYYYY":
                        dateInputItem.querySelector(".duet-date__input").value = day + "/" + month + "/" + year;
                    case "MMYYYY":
                        dateInputItem.querySelector(".duet-date__input").value = month + "/" + year;
                }

                dateInputItem.value = year + "-" + month + "-" + day

                var event = document.createEvent("Event");
                event.initEvent("input", true, true);

                dateInput.dispatchEvent(event);

                if (dateInputItem.onblur !== null)
                    dateInputItem.onblur();
            };

            let buttonToggle = dateInputItem.querySelector(".duet-date__toggle");
            buttonToggle.addEventListener("click", function () {
                dateInputItem.onfocus();
                dateInputItem.show();
            });

            let closedDialog = dateInputItem.querySelector(".duet-date__dialog");
            closedDialog.addEventListener("click", function () {
                changeDateColor(event);
            });

            dateInputItem
                .querySelector(".duet-date__dialog-content")
                .appendChild(node);

            var dateInputMask = function dateInputMask(elm) {
                elm.addEventListener("input", function (e) {
                    if (e.keyCode < 47 || e.keyCode > 57 || e.keyCode == 220) {
                        e.preventDefault();
                    }

                    var len = elm.value.length;

                    // If we're at a particular place, let the user type the slash
                    // i.e., 12/12/1212
                    if (len !== 1 || len !== 3) {
                        if (e.keyCode == 47) {
                            e.preventDefault();
                        }
                    }

                    //// let the user type one number
                    //if (elm.value[1] == "/" && elm.value[3] == "/") {
                    //    dateInput.maxLength = 8;
                    //} else if (
                    //    (elm.value[1] == "/" || elm.value[2] == "/") &&
                    //    (elm.value[3] == "/" || elm.value[4] == "/")
                    //) {
                    //    dateInput.maxLength = 9;
                    //} else if (elm.value[2] == "/" && elm.value[5] == "/") {
                    //    dateInput.maxLength = 10;
                    //}

                    // If they don't add the slash, do it for them...
                    if (len === 2 && elm.value[1] != "/") {
                        elm.value += "/";
                    }

                    // If they don't add the slash, do it for them...
                    //if (len === 5 && elm.value[1] != "/" && elm.value[4] != "/") { 
                    if (len === 5 && elm.value[1] != "/" && elm.value[4] != "/" && DateFormatValue != "MMYYYY") {//ntg 13.12.23 when the format was mmyyyy, it added '/' accidently
                        elm.value += "/";
                    }
                });
            };
            dateInputMask(dateInput);
        });
    }, 800);
});
