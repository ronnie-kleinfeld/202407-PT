var __awaiter = this && this.__awaiter || function (e, t, a, o) {
    function i(e) {
        return e instanceof a ? e : new a((function (t) {
            t(e)
        }))
    }
    return new (a || (a = Promise))((function (a, n) {
        function r(e) {
            try {
                s(o.next(e))
            } catch (e) {
                n(e)
            }
        }

        function d(e) {
            try {
                s(o["throw"](e))
            } catch (e) {
                n(e)
            }
        }

        function s(e) {
            e.done ? a(e.value) : i(e.value).then(r, d)
        }
        s((o = o.apply(e, t || [])).next())
    }))
};
var __generator = this && this.__generator || function (e, t) {
    var a = {
        label: 0,
        sent: function () {
            if (n[0] & 1) throw n[1];
            return n[1]
        },
        trys: [],
        ops: []
    },
        o, i, n, r;
    return r = {
        next: d(0),
        throw: d(1),
        return: d(2)
    }, typeof Symbol === "function" && (r[Symbol.iterator] = function () {
        return this
    }), r;

    function d(e) {
        return function (t) {
            return s([e, t])
        }
    }

    function s(r) {
        if (o) throw new TypeError("Generator is already executing.");
        while (a) try {
            if (o = 1, i && (n = r[0] & 2 ? i["return"] : r[0] ? i["throw"] || ((n = i["return"]) && n.call(i), 0) : i.next) && !(n = n.call(i, r[1])).done) return n;
            if (i = 0, n) r = [r[0] & 2, n.value];
            switch (r[0]) {
                case 0:
                case 1:
                    n = r;
                    break;
                case 4:
                    a.label++;
                    return {
                        value: r[1], done: false
                    };
                case 5:
                    a.label++;
                    i = r[1];
                    r = [0];
                    continue;
                case 7:
                    r = a.ops.pop();
                    a.trys.pop();
                    continue;
                default:
                    if (!(n = a.trys, n = n.length > 0 && n[n.length - 1]) && (r[0] === 6 || r[0] === 2)) {
                        a = 0;
                        continue
                    }
                    if (r[0] === 3 && (!n || r[1] > n[0] && r[1] < n[3])) {
                        a.label = r[1];
                        break
                    }
                    if (r[0] === 6 && a.label < n[1]) {
                        a.label = n[1];
                        n = r;
                        break
                    }
                    if (n && a.label < n[2]) {
                        a.label = n[2];
                        a.ops.push(r);
                        break
                    }
                    if (n[2]) a.ops.pop();
                    a.trys.pop();
                    continue
            }
            r = t.call(e, a)
        } catch (e) {
            r = [6, e];
            i = 0
        } finally {
                o = n = 0
            }
        if (r[0] & 5) throw r[1];
        return {
            value: r[0] ? r[1] : void 0,
            done: true
        }
    }
};
System.register(["./index-3aa2297e.system.js"], (function (e) {
    "use strict";
    var t, a, o, i, n;
    return {
        setters: [function (e) {
            t = e.h;
            a = e.r;
            o = e.c;
            i = e.H;
            n = e.g
        }],
        execute: function () {
            var r = /^(\d{4})-(\d{2})-(\d{2})$/;
            var d;
            (function (e) {
                e[e["Sunday"] = 0] = "Sunday";
                e[e["Monday"] = 1] = "Monday";
                e[e["Tuesday"] = 2] = "Tuesday";
                e[e["Wednesday"] = 3] = "Wednesday";
                e[e["Thursday"] = 4] = "Thursday";
                e[e["Friday"] = 5] = "Friday";
                e[e["Saturday"] = 6] = "Saturday"
            })(d || (d = {}));

            function s(e, t, a) {
                var o = parseInt(a, 10);
                var i = parseInt(t, 10);
                var n = parseInt(e, 10);
                var r = Number.isInteger(n) && Number.isInteger(i) && Number.isInteger(o) && i > 0 && i <= 12 && o > 0 && o <= 31 && n > 0;
                if (r) {
                    return new Date(n, i - 1, o)
                }
            }

            function l(e) {
                if (!e) {
                    return
                }
                var t = e.match(r);
                if (t) {
                    return s(t[1], t[2], t[3])
                }
            }

            function u(e) {
                if (!e) {
                    return ""
                }
                var t = e.getDate().toString(10);
                var a = (e.getMonth() + 1).toString(10);
                var o = e.getFullYear().toString(10);
                if (e.getDate() < 10) {
                    t = "0" + t
                }
                if (e.getMonth() < 9) {
                    a = "0" + a
                }
                return o + "-" + a + "-" + t
            }

            function c(e, t) {
                if (e == null || t == null) {
                    return false
                }
                return e.getFullYear() === t.getFullYear() && e.getMonth() === t.getMonth() && e.getDate() === t.getDate()
            }

            function h(e, t) {
                var a = new Date(e);
                a.setDate(a.getDate() + t);
                return a
            }

            function p(e, t) {
                if (t === void 0) {
                    t = d.Monday
                }
                var a = new Date(e);
                var o = a.getDay();
                var i = (o < t ? 7 : 0) + o - t;
                a.setDate(a.getDate() - i);
                return a
            }

            function f(e, t) {
                if (t === void 0) {
                    t = d.Monday
                }
                var a = new Date(e);
                var o = a.getDay();
                var i = (o < t ? -7 : 0) + 6 - (o - t);
                a.setDate(a.getDate() + i);
                return a
            }

            function v(e) {
                return new Date(e.getFullYear(), e.getMonth(), 1)
            }

            function g(e) {
                return new Date(e.getFullYear(), e.getMonth() + 1, 0)
            }

            function y(e, t) {
                var a = new Date(e);
                a.setMonth(t);
                return a
            }

            function b(e, t) {
                var a = new Date(e);
                a.setFullYear(t);
                return a
            }

            function _(e, t, a) {
                return m(e, t, a) === e
            }

            function m(e, t, a) {
                var o = e.getTime();
                if (t && t instanceof Date && o < t.getTime()) {
                    return t
                }
                if (a && a instanceof Date && o > a.getTime()) {
                    return a
                }
                return e
            }

            function x(e, t) {
                var a = [];
                var o = e;
                while (!c(o, t)) {
                    a.push(o);
                    o = h(o, 1)
                }
                a.push(o);
                return a
            }

            function w(e, t) {
                if (t === void 0) {
                    t = d.Monday
                }
                var a = p(v(e), t);
                var o = f(g(e), t);
                return x(a, o)
            }

            function D() {
                return Math.random().toString(16).slice(-4)
            }

            function k(e) {
                return e + "-" + D() + D() + "-" + D() + "-" + D() + "-" + D() + "-" + D() + D() + D()
            }
            var M = function (e) {
                var a = e.onClick,
                    o = e.localization,
                    i = e.name,
                    n = e.formattedValue,
                    r = e.value,
                    d = e.identifier,
                    s = e.disabled,
                    l = e.required,
                    u = e.role,
                    c = e.buttonRef,
                    h = e.inputRef,
                    p = e.onInput,
                    f = e.onBlur,
                    v = e.onFocus;
                return t("div", {
                    class: "duet-date__input-wrapper"
                }, t("input", {
                    class: "duet-date__input",
                    value: n,
                    placeholder: o.placeholder,
                    id: d,
                    disabled: s,
                    role: u,
                    required: l ? true : undefined,
                    "aria-autocomplete": "none",
                    onInput: p,
                    onFocus: v,
                    onBlur: f,
                    autoComplete: "off",
                    ref: h
                }), t("input", {
                    type: "hidden",
                    name: i,
                    value: r
                }), t("button", {
                    class: "duet-date__toggle",
                    onClick: a,
                    disabled: s,
                    ref: c,
                    type: "button"
                }, t("span", {
                    class: "duet-date__toggle-icon"
                }, t("svg", {
                    "aria-hidden": "true",
                    height: "24",
                    viewBox: "0 0 21 21",
                    width: "24",
                    xmlns: "http://www.w3.org/2000/svg"
                }, t("g", {
                    fill: "none",
                    "fill-rule": "evenodd",
                    transform: "translate(2 2)"
                }, t("path", {
                    d: "m2.5.5h12c1.1045695 0 2 .8954305 2 2v12c0 1.1045695-.8954305 2-2 2h-12c-1.1045695 0-2-.8954305-2-2v-12c0-1.1045695.8954305-2 2-2z",
                    stroke: "currentColor",
                    "stroke-linecap": "round",
                    "stroke-linejoin": "round"
                }), t("path", {
                    d: "m.5 4.5h16",
                    stroke: "currentColor",
                    "stroke-linecap": "round",
                    "stroke-linejoin": "round"
                }), t("g", {
                    fill: "currentColor"
                }, t("circle", {
                    cx: "8.5",
                    cy: "8.5",
                    r: "1"
                }), t("circle", {
                    cx: "4.5",
                    cy: "8.5",
                    r: "1"
                }), t("circle", {
                    cx: "12.5",
                    cy: "8.5",
                    r: "1"
                }), t("circle", {
                    cx: "8.5",
                    cy: "12.5",
                    r: "1"
                }), t("circle", {
                    cx: "4.5",
                    cy: "12.5",
                    r: "1"
                }), t("circle", {
                    cx: "12.5",
                    cy: "12.5",
                    r: "1"
                }))))), t("span", {
                    class: "duet-date__vhidden"
                }, o.buttonLabel, n && t("span", null, ", ", o.selectedDateMessage, " ", n))))
            };
            var F = function (e) {
                var a = e.focusedDay,
                    o = e.today,
                    i = e.day,
                    n = e.onDaySelect,
                    r = e.onKeyboardNavigation,
                    d = e.focusedDayRef,
                    s = e.inRange,
                    l = e.dateFormatter;
                var u = c(i, o);
                var h = c(i, a);
                var p = i.getMonth() !== a.getMonth();
                var f = !s;

                function v(e) {
                    n(e, i)
                }
                return t("button", {
                    class: {
                        "duet-date__day": true,
                        "is-outside": f,
                        "is-disabled": p,
                        "is-today": u
                    },
                    tabIndex: h ? 0 : -1,
                    onClick: v,
                    onKeyDown: r,
                    disabled: f || p,
                    type: "button",
                    ref: function (e) {
                        if (h && e && d) {
                            d(e)
                        }
                    }
                }, t("span", {
                    "aria-hidden": "true"
                }, i.getDate()), t("span", {
                    class: "duet-date__vhidden"
                }, l(i)))
            };

            function z(e, t) {
                var a = [];
                for (var o = 0; o < e.length; o += t) {
                    a.push(e.slice(o, o + t))
                }
                return a
            }

            function S(e, t, a) {
                return e.map((function (o, i) {
                    var n = (i + t) % e.length;
                    return a(e[n])
                }))
            }
            var T = function (e) {
                var a = e.selectedDate,
                    o = e.focusedDate,
                    i = e.labelledById,
                    n = e.localization,
                    r = e.firstDayOfWeek,
                    d = e.min,
                    s = e.max,
                    l = e.dateFormatter,
                    u = e.onDateSelect,
                    h = e.onKeyboardNavigation,
                    p = e.focusedDayRef,
                    f = e.onMouseDown,
                    v = e.onFocusIn;
                var g = new Date;
                var y = w(o, r);
                return t("table", {
                    class: "duet-date__table",
                    role: "grid",
                    "aria-labelledby": i,
                    onFocusin: v,
                    onMouseDown: f
                }, t("thead", null, t("tr", null, S(n.dayNames, r, (function (e) {
                    return t("th", {
                        class: "duet-date__table-header",
                        scope: "col"
                    }, t("span", {
                        "aria-hidden": "true"
                    }, e.substr(0, 2)), t("span", {
                        class: "duet-date__vhidden"
                    }, e))
                })))), t("tbody", null, z(y, 7).map((function (e) {
                    return t("tr", {
                        class: "duet-date__row"
                    }, e.map((function (e) {
                        return t("td", {
                            class: "duet-date__cell",
                            role: "gridcell",
                            "aria-selected": c(e, a) ? "true" : undefined
                        }, t(F, {
                            day: e,
                            today: g,
                            focusedDay: o,
                            inRange: _(e, d, s),
                            onDaySelect: u,
                            dateFormatter: l,
                            onKeyboardNavigation: h,
                            focusedDayRef: p
                        }))
                    })))
                }))))
            };
            var Y = {
                buttonLabel: "Choose date",
                placeholder: "YYYY-MM-DD",
                selectedDateMessage: "Selected date is",
                prevMonthLabel: "Previous month",
                nextMonthLabel: "Next month",
                monthSelectLabel: "Month",
                yearSelectLabel: "Year",
                closeLabel: "Close window",
                keyboardInstruction: "You can use arrow keys to navigate dates",
                calendarHeading: "Choose a date",
                dayNames: ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"],
                monthNames: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"],
                monthNamesShort: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"]
            };
            var C = {
                parse: l,
                format: u
            };
            var I = '.duet-date *,.duet-date *::before,.duet-date *::after{box-sizing:border-box;margin:0;width:auto}.duet-date{box-sizing:border-box;color:var(--duet-color-text);display:block;font-family:var(--duet-font);margin:0;position:relative;text-align:left;width:100%}.duet-date__input{-webkit-appearance:none;appearance:none;background:var(--duet-color-surface);border:1px solid var(--duet-color-text);border-radius:var(--duet-radius);color:var(--duet-color-text);float:none;font-family:var(--duet-font);font-size:100%;line-height:normal;padding:14px 60px 14px 14px;width:100%}.duet-date__input:focus{border-color:var(--duet-color-primary);box-shadow:0 0 0 1px var(--duet-color-primary);outline:0}.duet-date__input::-webkit-input-placeholder{color:var(--duet-color-placeholder);opacity:1}.duet-date__input:-moz-placeholder{color:var(--duet-color-placeholder);opacity:1}.duet-date__input:-ms-input-placeholder{color:var(--duet-color-placeholder)}.duet-date__input-wrapper{position:relative;width:100%}.duet-date__toggle{-moz-appearance:none;-webkit-appearance:none;-webkit-user-select:none;align-items:center;appearance:none;background:var(--duet-color-button);border:0;border-radius:0;border-bottom-right-radius:var(--duet-radius);border-top-right-radius:var(--duet-radius);box-shadow:inset 1px 0 0 rgba(0, 0, 0, 0.1);color:var(--duet-color-text);cursor:pointer;display:flex;height:calc(100% - 2px);justify-content:center;padding:0;position:absolute;right:1px;top:1px;user-select:none;width:48px;z-index:2}.duet-date__toggle:focus{box-shadow:0 0 0 2px var(--duet-color-primary);outline:0}.duet-date__toggle-icon{display:flex;flex-basis:100%;justify-content:center;align-items:center}.duet-date__dialog{display:flex;left:0;min-width:320px;opacity:0;position:absolute;top:100%;transform:scale(0.96) translateZ(0) translateY(-20px);transform-origin:top right;transition:transform 300ms ease, opacity 300ms ease, visibility 300ms ease;visibility:hidden;width:100%;will-change:transform, opacity, visibility;z-index:var(--duet-z-index)}@media (max-width: 35.9375em){.duet-date__dialog{background:var(--duet-color-overlay);bottom:0;position:fixed;right:0;top:0;transform:translateZ(0);transform-origin:bottom center}}.duet-date__dialog.is-left{left:auto;right:0;width:auto}.duet-date__dialog.is-active{opacity:1;transform:scale(1.0001) translateZ(0) translateY(0);visibility:visible}.duet-date__dialog-content{background:var(--duet-color-surface);border:1px solid rgba(0, 0, 0, 0.1);border-radius:var(--duet-radius);box-shadow:0 4px 10px 0 rgba(0, 0, 0, 0.1);margin-left:auto;margin-top:8px;max-width:310px;min-width:290px;padding:16px 16px 20px;position:relative;transform:none;width:100%;z-index:var(--duet-z-index)}@media (max-width: 35.9375em){.duet-date__dialog-content{border:0;border-radius:0;border-top-left-radius:var(--duet-radius);border-top-right-radius:var(--duet-radius);bottom:0;left:0;margin:0;max-width:none;min-height:26em;opacity:0;padding:0 8% 20px;position:absolute;transform:translateZ(0) translateY(100%);transition:transform 400ms ease, opacity 400ms ease, visibility 400ms ease;visibility:hidden;will-change:transform, opacity, visibility}.is-active .duet-date__dialog-content{opacity:1;transform:translateZ(0) translateY(0);visibility:visible}}.duet-date__table{border-collapse:collapse;border-spacing:0;color:var(--duet-color-text);font-size:1rem;font-weight:var(--duet-font-normal);line-height:1.25;text-align:center;width:100%}.duet-date__table-header{font-size:0.75rem;font-weight:var(--duet-font-bold);letter-spacing:1px;line-height:1.25;padding-bottom:8px;text-decoration:none;text-transform:uppercase}.duet-date__cell{text-align:center}.duet-date__day{-moz-appearance:none;-webkit-appearance:none;appearance:none;background:transparent;border:0;border-radius:50%;color:var(--duet-color-text);cursor:pointer;display:inline-block;font-family:var(--duet-font);font-size:0.875rem;font-variant-numeric:tabular-nums;font-weight:var(--duet-font-normal);height:36px;line-height:1.25;padding:0 0 1px;position:relative;text-align:center;vertical-align:middle;width:36px;z-index:1}.duet-date__day.is-today{box-shadow:0 0 0 1px var(--duet-color-primary);position:relative;z-index:200}.duet-date__day:hover::before,.duet-date__day.is-today::before{background:var(--duet-color-primary);border-radius:50%;bottom:0;content:"";left:0;opacity:0.06;position:absolute;right:0;top:0}[aria-selected=true] .duet-date__day,.duet-date__day:focus{background:var(--duet-color-primary);box-shadow:none;color:var(--duet-color-text-active);outline:0}.duet-date__day:active{background:var(--duet-color-primary);box-shadow:0 0 5px var(--duet-color-primary);color:var(--duet-color-text-active);z-index:200}.duet-date__day:focus{box-shadow:0 0 5px var(--duet-color-primary);z-index:200}.duet-date__day.is-disabled{background:transparent;box-shadow:none;color:var(--duet-color-text);cursor:default;opacity:0.5}.duet-date__day.is-disabled::before{display:none}.duet-date__day.is-outside{background:var(--duet-color-button);box-shadow:none;color:var(--duet-color-text);cursor:default;opacity:0.6;pointer-events:none}.duet-date__day.is-outside::before{display:none}.duet-date__header{align-items:center;display:flex;justify-content:space-between;margin-bottom:16px;width:100%}.duet-date__nav{white-space:nowrap}.duet-date__prev,.duet-date__next{-moz-appearance:none;-webkit-appearance:none;align-items:center;appearance:none;background:var(--duet-color-button);border:0;border-radius:50%;color:var(--duet-color-text);cursor:pointer;display:inline-flex;height:32px;justify-content:center;margin-left:8px;padding:0;transition:background-color 300ms ease;width:32px}@media (max-width: 35.9375em){.duet-date__prev,.duet-date__next{height:40px;width:40px}}.duet-date__prev:focus,.duet-date__next:focus{box-shadow:0 0 0 2px var(--duet-color-primary);outline:0}.duet-date__prev:active:focus,.duet-date__next:active:focus{box-shadow:none}.duet-date__prev:disabled,.duet-date__next:disabled{cursor:default;opacity:0.5}.duet-date__prev svg,.duet-date__next svg{margin:0 auto}.duet-date__select{display:inline-flex;margin-top:4px;position:relative}.duet-date__select span{margin-right:4px}.duet-date__select select{cursor:pointer;font-size:1rem;height:100%;left:0;opacity:0;position:absolute;top:0;width:100%;z-index:2}.duet-date__select select:focus+.duet-date__select-label{box-shadow:0 0 0 2px var(--duet-color-primary)}.duet-date__select-label{align-items:center;border-radius:var(--duet-radius);color:var(--duet-color-text);display:flex;font-size:1.25rem;font-weight:var(--duet-font-bold);line-height:1.25;padding:0 4px 0 8px;pointer-events:none;position:relative;width:100%;z-index:1}.duet-date__select-label svg{width:16px;height:16px}.duet-date__mobile{align-items:center;border-bottom:1px solid rgba(0, 0, 0, 0.12);display:flex;justify-content:space-between;margin-bottom:20px;margin-left:-10%;overflow:hidden;padding:12px 20px;position:relative;text-overflow:ellipsis;white-space:nowrap;width:120%}@media (min-width: 36em){.duet-date__mobile{border:0;margin:0;overflow:visible;padding:0;position:absolute;right:-8px;top:-8px;width:auto}}.duet-date__mobile-heading{display:inline-block;font-weight:var(--duet-font-bold);max-width:84%;overflow:hidden;text-overflow:ellipsis;white-space:nowrap}@media (min-width: 36em){.duet-date__mobile-heading{display:none}}.duet-date__close{-webkit-appearance:none;align-items:center;appearance:none;background:var(--duet-color-button);border:0;border-radius:50%;color:var(--duet-color-text);cursor:pointer;display:flex;height:24px;justify-content:center;padding:0;width:24px}@media (min-width: 36em){.duet-date__close{opacity:0}}.duet-date__close:focus{box-shadow:0 0 0 2px var(--duet-color-primary);outline:none}@media (min-width: 36em){.duet-date__close:focus{opacity:1}}.duet-date__close svg{margin:0 auto}.duet-date__vhidden{border:0;clip:rect(1px, 1px, 1px, 1px);height:1px;overflow:hidden;padding:0;position:absolute;top:0;width:1px}';

            function N(e, t) {
                var a = [];
                for (var o = e; o <= t; o++) {
                    a.push(o)
                }
                return a
            }
            var L = {
                TAB: 9,
                ESC: 27,
                SPACE: 32,
                PAGE_UP: 33,
                PAGE_DOWN: 34,
                END: 35,
                HOME: 36,
                LEFT: 37,
                UP: 38,
                RIGHT: 39,
                DOWN: 40
            };
            var A = /[^0-9\.\/\-]+/g;
            var B = 300;
            var E = e("duet_date_picker", function () {
                function e(e) {
                    var t = this;
                    a(this, e);
                    this.duetChange = o(this, "duetChange", 7);
                    this.duetBlur = o(this, "duetBlur", 7);
                    this.duetFocus = o(this, "duetFocus", 7);
                    this.monthSelectId = k("DuetDateMonth");
                    this.yearSelectId = k("DuetDateYear");
                    this.dialogLabelId = k("DuetDateLabel");
                    this.initialTouchX = null;
                    this.initialTouchY = null;
                    this.activeFocus = false;
                    this.focusedDay = new Date;
                    this.open = false;
                    this.name = "date";
                    this.identifier = "";
                    this.disabled = false;
                    this.direction = "right";
                    this.required = false;
                    this.value = "";
                    this.min = "";
                    this.max = "";
                    this.firstDayOfWeek = d.Monday;
                    this.localization = Y;
                    this.dateAdapter = C;
                    this.enableActiveFocus = function () {
                        t.activeFocus = true
                    };
                    this.disableActiveFocus = function () {
                        t.activeFocus = false
                    };
                    this.toggleOpen = function (e) {
                        e.preventDefault();
                        t.open ? t.hide(false) : t.show()
                    };
                    this.handleEscKey = function (e) {
                        if (e.keyCode === L.ESC) {
                            t.hide()
                        }
                    };
                    this.handleBlur = function (e) {
                        e.stopPropagation();
                        t.duetBlur.emit({
                            component: "duet-date-picker"
                        })
                    };
                    this.handleFocus = function (e) {
                        e.stopPropagation();
                        t.duetFocus.emit({
                            component: "duet-date-picker"
                        })
                    };
                    this.handleTouchStart = function (e) {
                        var a = e.changedTouches[0];
                        t.initialTouchX = a.pageX;
                        t.initialTouchY = a.pageY
                    };
                    this.handleTouchMove = function (e) {
                        e.preventDefault()
                    };
                    this.handleTouchEnd = function (e) {
                        var a = e.changedTouches[0];
                        var o = a.pageX - t.initialTouchX;
                        var i = a.pageY - t.initialTouchY;
                        var n = 70;
                        var r = Math.abs(o) >= n && Math.abs(i) <= n;
                        var d = Math.abs(i) >= n && Math.abs(o) <= n && i > 0;
                        if (r) {
                            t.addMonths(o < 0 ? 1 : -1)
                        } else if (d) {
                            t.hide(false);
                            e.preventDefault()
                        }
                        t.initialTouchY = null;
                        t.initialTouchX = null
                    };
                    this.handleNextMonthClick = function (e) {
                        e.preventDefault();
                        t.addMonths(1)
                    };
                    this.handlePreviousMonthClick = function (e) {
                        e.preventDefault();
                        t.addMonths(-1)
                    };
                    this.handleFirstFocusableKeydown = function (e) {
                        if (e.keyCode === L.TAB && e.shiftKey) {
                            t.focusedDayNode.focus();
                            e.preventDefault()
                        }
                    };
                    this.handleKeyboardNavigation = function (e) {
                        if (e.keyCode === L.TAB && !e.shiftKey) {
                            e.preventDefault();
                            t.firstFocusableElement.focus();
                            return
                        }
                        var a = true;
                        switch (e.keyCode) {
                            case L.RIGHT:
                                t.addDays(1);
                                break;
                            case L.LEFT:
                                t.addDays(-1);
                                break;
                            case L.DOWN:
                                t.addDays(7);
                                break;
                            case L.UP:
                                t.addDays(-7);
                                break;
                            case L.PAGE_UP:
                                if (e.shiftKey) {
                                    t.addYears(-1)
                                } else {
                                    t.addMonths(-1)
                                }
                                break;
                            case L.PAGE_DOWN:
                                if (e.shiftKey) {
                                    t.addYears(1)
                                } else {
                                    t.addMonths(1)
                                }
                                break;
                            case L.HOME:
                                t.startOfWeek();
                                break;
                            case L.END:
                                t.endOfWeek();
                                break;
                            default:
                                a = false
                        }
                        if (a) {
                            e.preventDefault();
                            t.enableActiveFocus()
                        }
                    };
                    this.handleDaySelect = function (e, a) {
                        if (!_(a, l(t.min), l(t.max))) {
                            return
                        }
                        if (a.getMonth() === t.focusedDay.getMonth()) {
                            t.setValue(a);
                            t.hide()
                        } else {
                            t.setFocusedDay(a)
                        }
                    };
                    this.handleMonthSelect = function (e) {
                        t.setMonth(parseInt(e.target.value, 10))
                    };
                    this.handleYearSelect = function (e) {
                        t.setYear(parseInt(e.target.value, 10))
                    };
                    this.handleInputChange = function (e) {
                        var a = e.target;
                        a.value = a.value.replace(A, "");
                        var o = t.dateAdapter.parse(a.value, s);
                        if (o || a.value === "") {
                            t.setValue(o)
                        }
                    };
                    this.processFocusedDayNode = function (e) {
                        t.focusedDayNode = e;
                        if (t.activeFocus && t.open) {
                            setTimeout((function () {
                                return e.focus()
                            }), 0)
                        }
                    }
                }
                e.prototype.handleDocumentClick = function (e) {
                    if (!this.open) {
                        return
                    }
                    var t = e.target;
                    if (this.dialogWrapperNode.contains(t) || this.datePickerButton.contains(t)) {
                        return
                    }
                    this.hide(false)
                };
                e.prototype.setFocus = function () {
                    return __awaiter(this, void 0, void 0, (function () {
                        return __generator(this, (function (e) {
                            return [2, this.datePickerInput.focus()]
                        }))
                    }))
                };
                e.prototype.show = function () {
                    return __awaiter(this, void 0, void 0, (function () {
                        var e = this;
                        return __generator(this, (function (t) {
                            this.open = true;
                            this.setFocusedDay(l(this.value) || new Date);
                            clearTimeout(this.focusTimeoutId);
                            this.focusTimeoutId = setTimeout((function () {
                                return e.monthSelectNode.focus()
                            }), B);
                            return [2]
                        }))
                    }))
                };
                e.prototype.hide = function (e) {
                    if (e === void 0) {
                        e = true
                    }
                    return __awaiter(this, void 0, void 0, (function () {
                        var t = this;
                        return __generator(this, (function (a) {
                            this.open = false;
                            clearTimeout(this.focusTimeoutId);
                            if (e) {
                                setTimeout((function () {
                                    return t.datePickerButton.focus()
                                }), B + 200)
                            }
                            return [2]
                        }))
                    }))
                };
                e.prototype.addDays = function (e) {
                    this.setFocusedDay(h(this.focusedDay, e))
                };
                e.prototype.addMonths = function (e) {
                    this.setMonth(this.focusedDay.getMonth() + e)
                };
                e.prototype.addYears = function (e) {
                    this.setYear(this.focusedDay.getFullYear() + e)
                };
                e.prototype.startOfWeek = function () {
                    this.setFocusedDay(p(this.focusedDay, this.firstDayOfWeek))
                };
                e.prototype.endOfWeek = function () {
                    this.setFocusedDay(f(this.focusedDay, this.firstDayOfWeek))
                };
                e.prototype.setMonth = function (e) {
                    var t = y(v(this.focusedDay), e);
                    var a = g(t);
                    var o = y(this.focusedDay, e);
                    this.setFocusedDay(m(o, t, a))
                };
                e.prototype.setYear = function (e) {
                    var t = b(v(this.focusedDay), e);
                    var a = g(t);
                    var o = b(this.focusedDay, e);
                    this.setFocusedDay(m(o, t, a))
                };
                e.prototype.setFocusedDay = function (e) {
                    this.focusedDay = m(e, l(this.min), l(this.max))
                };
                e.prototype.setValue = function (e) {
                    this.value = u(e);
                    this.duetChange.emit({
                        component: "duet-date-picker",
                        value: this.value,
                        valueAsDate: e
                    })
                };
                e.prototype.render = function () {
                    var e = this;
                    var a = l(this.value);
                    var o = a && this.dateAdapter.format(a);
                    var n = (a || this.focusedDay).getFullYear();
                    var r = this.focusedDay.getMonth();
                    var d = this.focusedDay.getFullYear();
                    var s = l(this.min);
                    var u = l(this.max);
                    var c = s != null && s.getMonth() === r && s.getFullYear() === d;
                    var h = u != null && u.getMonth() === r && u.getFullYear() === d;
                    var p = n - 100;
                    var f = n + 100;
                    if (s) {
                        p = Math.max(p, s.getFullYear())
                    }
                    if (u) {
                        f = Math.min(f, u.getFullYear())
                    }
                    return t(i, null, t("div", {
                        class: "duet-date"
                    }, t(M, {
                        value: this.value,
                        formattedValue: o,
                        onInput: this.handleInputChange,
                        onBlur: this.handleBlur,
                        onFocus: this.handleFocus,
                        onClick: this.toggleOpen,
                        name: this.name,
                        disabled: this.disabled,
                        role: this.role,
                        required: this.required,
                        identifier: this.identifier,
                        localization: this.localization,
                        buttonRef: function (t) {
                            return e.datePickerButton = t
                        },
                        inputRef: function (t) {
                            return e.datePickerInput = t
                        }
                    }), t("div", {
                        class: {
                            "duet-date__dialog": true,
                            "is-left": this.direction === "left",
                            "is-active": this.open
                        },
                        role: "dialog",
                        "aria-modal": "true",
                        "aria-hidden": this.open ? "false" : "true",
                        "aria-labelledby": this.dialogLabelId,
                        onTouchMove: this.handleTouchMove,
                        onTouchStart: this.handleTouchStart,
                        onTouchEnd: this.handleTouchEnd
                    }, t("div", {
                        class: "duet-date__dialog-content",
                        onKeyDown: this.handleEscKey,
                        ref: function (t) {
                            return e.dialogWrapperNode = t
                        }
                    }, t("div", {
                        class: "duet-date__vhidden duet-date__instructions",
                        "aria-live": "polite"
                    }, this.localization.keyboardInstruction), t("div", {
                        class: "duet-date__mobile",
                        onFocusin: this.disableActiveFocus
                    }, t("label", {
                        class: "duet-date__mobile-heading"
                    }, this.localization.calendarHeading), t("button", {
                        class: "duet-date__close",
                        ref: function (t) {
                            return e.firstFocusableElement = t
                        },
                        onKeyDown: this.handleFirstFocusableKeydown,
                        onClick: function () {
                            return e.hide()
                        },
                        type: "button"
                    }, t("svg", {
                        "aria-hidden": "true",
                        fill: "currentColor",
                        xmlns: "http://www.w3.org/2000/svg",
                        width: "16",
                        height: "16",
                        viewBox: "0 0 24 24"
                    }, t("path", {
                        d: "M0 0h24v24H0V0z",
                        fill: "none"
                    }), t("path", {
                        d: "M18.3 5.71c-.39-.39-1.02-.39-1.41 0L12 10.59 7.11 5.7c-.39-.39-1.02-.39-1.41 0-.39.39-.39 1.02 0 1.41L10.59 12 5.7 16.89c-.39.39-.39 1.02 0 1.41.39.39 1.02.39 1.41 0L12 13.41l4.89 4.89c.39.39 1.02.39 1.41 0 .39-.39.39-1.02 0-1.41L13.41 12l4.89-4.89c.38-.38.38-1.02 0-1.4z"
                    })), t("span", {
                        class: "duet-date__vhidden"
                    }, this.localization.closeLabel))), t("div", {
                        class: "duet-date__header",
                        onFocusin: this.disableActiveFocus
                    }, t("div", null, t("h2", {
                        id: this.dialogLabelId,
                        class: "duet-date__vhidden",
                        "aria-live": "polite"
                    }, this.localization.monthNames[r], " ", this.focusedDay.getFullYear()), t("label", {
                        htmlFor: this.monthSelectId,
                        class: "duet-date__vhidden"
                    }, this.localization.monthSelectLabel), t("div", {
                        class: "duet-date__select"
                    }, t("select", {
                        id: this.monthSelectId,
                        class: "duet-date__select--month",
                        ref: function (t) {
                            return e.monthSelectNode = t
                        },
                        onChange: this.handleMonthSelect
                    }, this.localization.monthNames.map((function (e, a) {
                        return t("option", {
                            key: e,
                            value: a,
                            selected: a === r
                        }, e)
                    }))), t("div", {
                        class: "duet-date__select-label",
                        "aria-hidden": "true"
                    }, t("span", null, this.localization.monthNamesShort[r]), t("svg", {
                        fill: "currentColor",
                        xmlns: "http://www.w3.org/2000/svg",
                        width: "16",
                        height: "16",
                        viewBox: "0 0 24 24"
                    }, t("path", {
                        d: "M8.12 9.29L12 13.17l3.88-3.88c.39-.39 1.02-.39 1.41 0 .39.39.39 1.02 0 1.41l-4.59 4.59c-.39.39-1.02.39-1.41 0L6.7 10.7c-.39-.39-.39-1.02 0-1.41.39-.38 1.03-.39 1.42 0z"
                    })))), t("label", {
                        htmlFor: this.yearSelectId,
                        class: "duet-date__vhidden"
                    }, this.localization.yearSelectLabel), t("div", {
                        class: "duet-date__select"
                    }, t("select", {
                        id: this.yearSelectId,
                        class: "duet-date__select--year",
                        onChange: this.handleYearSelect
                    }, N(p, f).map((function (e) {
                        return t("option", {
                            key: e,
                            selected: e === d
                        }, e)
                    }))), t("div", {
                        class: "duet-date__select-label",
                        "aria-hidden": "true"
                    }, t("span", null, this.focusedDay.getFullYear()), t("svg", {
                        fill: "currentColor",
                        xmlns: "http://www.w3.org/2000/svg",
                        width: "16",
                        height: "16",
                        viewBox: "0 0 24 24"
                    }, t("path", {
                        d: "M8.12 9.29L12 13.17l3.88-3.88c.39-.39 1.02-.39 1.41 0 .39.39.39 1.02 0 1.41l-4.59 4.59c-.39.39-1.02.39-1.41 0L6.7 10.7c-.39-.39-.39-1.02 0-1.41.39-.38 1.03-.39 1.42 0z"
                    }))))), t("div", {
                        class: "duet-date__nav"
                    }, t("button", {
                        class: "duet-date__prev",
                        onClick: this.handlePreviousMonthClick,
                        disabled: c,
                        type: "button"
                    }, t("svg", {
                        "aria-hidden": "true",
                        fill: "currentColor",
                        xmlns: "http://www.w3.org/2000/svg",
                        width: "21",
                        height: "21",
                        viewBox: "0 0 24 24"
                    }, t("path", {
                        d: "M14.71 15.88L10.83 12l3.88-3.88c.39-.39.39-1.02 0-1.41-.39-.39-1.02-.39-1.41 0L8.71 11.3c-.39.39-.39 1.02 0 1.41l4.59 4.59c.39.39 1.02.39 1.41 0 .38-.39.39-1.03 0-1.42z"
                    })), t("span", {
                        class: "duet-date__vhidden"
                    }, this.localization.prevMonthLabel)), t("button", {
                        class: "duet-date__next",
                        onClick: this.handleNextMonthClick,
                        disabled: h,
                        type: "button"
                    }, t("svg", {
                        "aria-hidden": "true",
                        fill: "currentColor",
                        xmlns: "http://www.w3.org/2000/svg",
                        width: "21",
                        height: "21",
                        viewBox: "0 0 24 24"
                    }, t("path", {
                        d: "M9.29 15.88L13.17 12 9.29 8.12c-.39-.39-.39-1.02 0-1.41.39-.39 1.02-.39 1.41 0l4.59 4.59c.39.39.39 1.02 0 1.41L10.7 17.3c-.39.39-1.02.39-1.41 0-.38-.39-.39-1.03 0-1.42z"
                    })), t("span", {
                        class: "duet-date__vhidden"
                    }, this.localization.nextMonthLabel)))), t(T, {
                        selectedDate: a,
                        focusedDate: this.focusedDay,
                        onDateSelect: this.handleDaySelect,
                        onKeyboardNavigation: this.handleKeyboardNavigation,
                        labelledById: this.dialogLabelId,
                        localization: this.localization,
                        firstDayOfWeek: this.firstDayOfWeek,
                        focusedDayRef: this.processFocusedDayNode,
                        min: s,
                        max: u,
                        dateFormatter: this.dateAdapter.format
                    })))))
                };
                Object.defineProperty(e.prototype, "element", {
                    get: function () {
                        return n(this)
                    },
                    enumerable: false,
                    configurable: true
                });
                return e
            }());
            E.style = I
        }
    }
}));