

var app = {
    init: function () {
        this.globalControls();
    },
    allowOnlyNumber: function (event) {
        // Allow only backspace and delete
        if (event.keyCode === 46 || event.keyCode === 8) {
            // let it happen, don't do anything
        }
        else {
            // Ensure that it is a number and stop the keypress
            if (event.keyCode < 48 || event.keyCode > 57) {
                event.preventDefault();
            }
        }
    },
    globalControls: function () {
        this.initDatePicker();
        this.toggleActiveMenu();
        this.isActiveMenu();
        this.initToolTip();
    },
    initToolTip: function () {
        $('[data-toggle="tooltip"]').tooltip();
    },
    toggleActiveMenu: function () {
        var url = window.location;
        $("a.menu-link").removeClass("menu-active");

        $('a.menu-link').filter(function () {
            return this.href === url.href;
        }).addClass('menu-active');
    },
    isActiveMenu: function () {
        var url = window.location;

        $('a.menu-link').each(function () {
            var paymentUrl = $(this).attr('payment-url');
            var currentPaymentUrl = url.href;
            var paymentUrlIndex = currentPaymentUrl.search(paymentUrl);

            if (paymentUrlIndex > 0) {
                $("a.menu-link").removeClass("menu-active");
                $(this).addClass('menu-active');
            }
        });

    },
    toggleMenu: function () {
        let menu = document.querySelector("#navbar-mobile");
        let body = document.querySelector("body");
        menu.classList.toggle("d-none");
        body.classList.toggle("overflow-hidden");
    },
    initDatePicker: function () {
        var $datePickerGlobal = $('.prs-datepicker');

        if ($datePickerGlobal.length) {

            var date = new Date();
            date.setDate(date.getDate() + 1);

            $datePickerGlobal.datepicker({
                dateFormat: 'dd-M-yy',
                autoclose: true,
                changeMonth: true,
                changeYear: true,
                todayHighlight: true,
                minDate: date
            }).on('changeDate', function (ev) {
                $datePickerGlobal.datepicker('hide');
            });
            $datePickerGlobal.attr("autocomplete", "off");
        }

        var $datePicker = $('.gr-datepicker');

        if ($datePicker.length) {
            $datePicker.datepicker({
                dateFormat: 'dd-M-yy',
                autoclose: true,
                changeMonth: true,
                changeYear: true,
                todayHighlight: true
            }).on('changeDate', function (ev) {
                $datePicker.datepicker('hide');
            });
            $datePicker.attr("autocomplete", "off");
        }

        $dropify = $('.dropify');
        if ($dropify.length)
            $('.dropify').dropify({
                messages: {

                }
            });

    },
    resetDropify: function ($selector) {
        var drEvent = $selector.dropify();
        drEvent = drEvent.data('dropify');
        drEvent.resetPreview();
        drEvent.clearElement();
    },
    isUndefinedOrNull: function (val) {
        return (typeof val === 'undefined' || val === undefined || val === null);
    },
    cookieExpireTime: function (minutes) {
        var expireDate = new Date();
        expireDate.setTime(expireDate.getTime() + (minutes * 60 * 1000)); // add minutes
        return expireDate;
    },
    preventBackPageInBrowser: function () {
        window.history.pushState(null, "", window.location.href);
        window.onpopstate = function () {
            window.history.pushState(null, "", window.location.href);
        };
    },
    fileIcon: function (type, scale) {
        var defaultIconHtml = `<i class="fa fa-file-o ${scale}" aria-hidden="true"></i>`;
        if (!type) return defaultIconHtml;
        var fileType = type.split("/");
        if (!fileType) return defaultIconHtml;

        if (fileType[0] === FileTypeConstants.Image) {
            defaultIconHtml = `<i class="fa fa-file-image-o ${scale}" aria-hidden="true"></i>`;
            return defaultIconHtml;
        }

        else if (type === FileTypeConstants.Pdf) {
            defaultIconHtml = `<i class="fa fa-file-pdf-o ${scale}" aria-hidden="true"></i>`;
            return defaultIconHtml;
        }

        else if (type === FileTypeConstants.Word) {
            defaultIconHtml = `<i class="fa fa-file-word-o ${scale}" aria-hidden="true"></i>`;
            return defaultIconHtml;
        }

        else if (type === FileTypeConstants.Excel) {
            defaultIconHtml = `<i class="fa fa-file-excel-o ${scale}" aria-hidden="true"></i>`;
            return defaultIconHtml;
        }

        else {
            defaultIconHtml = `<i class="fa fa-file-o ${scale}" aria-hidden="true"></i>`;
            return defaultIconHtml;
        }
    },
    Notify: function (type, message, icon = 'info') {
        $.alert.open({
            title: type,
            type: type,
            icon: icon,
            content: message,
            buttons: {
                Ok: 'Ok'
            },
            align: 'left',
        });
    },
    getFormattedDate: function (date) {
        var day = date.getDate();
        var month = date.toLocaleString('en-us', { month: 'short' });
        var year = date.getFullYear('en-us');

        if (('' + day).length === 1) { day = '0' + day; }
        var formattedDate = `${day}-${month}-${year}`;

        return formattedDate;
    }
}

$(function () {
    app.init();
    $("#show_hide_password-1 a").on('click', function (event) {
        event.preventDefault();
        if ($('#show_hide_password-1 input').attr("type") == "text") {
            $('#show_hide_password-1 input').attr('type', 'password');
            $('#show_hide_password-1 i').addClass("fa-eye-slash");
            $('#show_hide_password-1 i').removeClass("fa-eye");
        } else if ($('#show_hide_password-1 input').attr("type") == "password") {
            $('#show_hide_password-1 input').attr('type', 'text');
            $('#show_hide_password-1 i').removeClass("fa-eye-slash");
            $('#show_hide_password-1 i').addClass("fa-eye");
        }
    });

    $("#show_hide_password-2 a").on('click', function (event) {
        event.preventDefault();
        if ($('#show_hide_password-2 input').attr("type") == "text") {
            $('#show_hide_password-2 input').attr('type', 'password');
            $('#show_hide_password-2 i').addClass("fa-eye-slash");
            $('#show_hide_password-2 i').removeClass("fa-eye");
        } else if ($('#show_hide_password-2 input').attr("type") == "password") {
            $('#show_hide_password-2 input').attr('type', 'text');
            $('#show_hide_password-2 i').removeClass("fa-eye-slash");
            $('#show_hide_password-2 i').addClass("fa-eye eye-second");
        }
    });

    $(".generate").click(function () {
        $(".new-pass").addClass("show");
    });
})

jQuery.fn.ForceNumericOnly =
    function () {
        return this.each(function () {
            $(this).keydown(function (e) {

                var evt = e || window.event // IE support
                var keyCode = evt.keyCode
                var ctrlDown = evt.ctrlKey || evt.metaKey // Mac support             
                
                if (ctrlDown && keyCode === 86) {                    
                    return true;
                }

                var key = e.charCode || e.keyCode || 0;
                // allow backspace, tab, delete, enter, arrows, numbers and keypad numbers ONLY
                // home, end, period, and numpad decimal
                return (
                    key === 8 ||
                    key === 9 ||
                    key === 13 ||
                    key === 46 ||
                    //key === 110 ||
                    key === 190 ||
                    (key >= 35 && key <= 40) ||
                    (key >= 48 && key <= 57) ||
                    (key >= 96 && key <= 105));
            });
        });
    };


jQuery.fn.ForceNumericOnlyOnPast =
    function () {
        return this.each(function () {
            $(this).keyup(function (e) {
                var isNumber = $.isNumeric($(this).val());
                if (!isNumber) {
                    $(this).val('');
                    return false;
                }

                return true;
            });
        });
    };

jQuery.fn.extend({
    ForceDecimalOnly: function () {

        return this.each(function () {
            $(this).keydown(function (event) {

                if (event.shiftKey === true) {
                    event.preventDefault();
                }

                if (!((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105) || event.keyCode == 8 || event.keyCode == 9 || event.keyCode === 37 || event.keyCode === 39 || event.keyCode === 46 || event.keyCode === 190 || event.keyCode === 110)) {
                    event.preventDefault();
                }

                if ($(this).val().indexOf('.') !== -1 && (event.keyCode === 190 || event.keyCode === 110))
                    event.preventDefault();

            });
        })
    }
});

var FileTypeConstants = {
    Image: 'image',
    Pdf: 'application/pdf',
    Word: 'application/vnd.openxmlformats-officedocument.wordprocessingml.document',
    Excel: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
}

var ScreenTypeConstants = {
    Desktop: 'Desktop',
    Mobile: 'Mobile'
}

var UserGroupConstants = {
    Correspondent: "1",
    CRM_User: "2",
    Agency: "3"
}

var ActionVerificationTypeConstants = {
    Email: "Email",
    SMS: "SMS"
}

var enumToastStatus = {
    Success: true,
    Fail: false
}

var enumToastType = {
    Error: 'error',
    Info: 'info',
    Success: 'success',
    Warning: 'warning',
}

var enumActionResultType = {
    Message: "message",
    Url: "url"
}

var AddEnhancementTypeConstants = {
    Big_Bullet_Point_Single: 'Big Bullet Point (Single)',
    Big_Bullet_Point_Double: 'Big Bullet Point (Double)',

    BoldInScreen: "Bold in Screen",
    Bold: "Bold",
    BoldInScreenAndSingleBox: "Bold in screen and single box",
    BoldInScreenAndDoubleBoxes: "Bold in screen and double boxes"
}

var CheckoutPaymentTypeConstants = {
    Direct: 1,
    Card: 2,
    Check_Or_Payorder:3
}

var PaymentMethodConstants = {
    bKash: 1,
    Rocket: 2,
    Nogod: 3,
    Check_Or_Payorder : 4
}

var EditionConstants = {
    National: '20',
    Rajshahi : '9',
    Rangpur : '10'
}

var AlertTypeConstants = {
    Success: 'Info',
    Info: 'Info',
    Warning: 'Warning',
    Error: 'Error'
}

var AlertIconConstants = {
    Info: 'info',
    Confirm: 'confirm',
    Warning: 'warning',
    Error: 'error',
    Prompt: 'prompt'
}

var PrivateAdTypesConstants = {
    Private: "Private",
    Spot: "Spot",
    EARPanel: "EAR Panel",
    Inhouse: "Inhouse"
}

var EditionPagesConstants = {
    Page_1: 1,
    Page_2: 2,
    Page_3: 3,
    Page_4: 4,
    Page_5: 5,
    Page_6: 6,
    Page_7: 7,
    Page_8: 8,
    Page_9: 9,
    Page_10: 10,
    Page_11: 11,
    Page_12: 12
}

var BrandsConstants ={
    Others : 9200
}