
var accountManager = {
    init: function () {           
        //accountManager.defaultVerificationType();
        //accountManager.defaultViewofForgotPassword();
    },   
    defaultVerificationType: function () {
        var verificationType = $(`input[type="radio"][name="AccountVerificationType"]`);
        if (!verificationType || verificationType.length <= 0) return;

        $(`input[type="radio"][name="AccountVerificationType"][value="Email"]`).attr('checked', true);
    },
    defaultViewofForgotPassword: function () {
        if (!$('.verification-instruction') || !$('.section-verify-instruction') || !$('.section-password-confirm-password')) return;

        $('.verification-instruction').text('');
        $('.section-verify-instruction').hide();
        $('.section-password-confirm-password').hide();
        $('.section-btn-verifycode').hide();
    }
}

$(function () {
    accountManager.init();

    $("#formLogin").on("submit", function (event) {
        event.preventDefault();

        var $loginButton = $('.btn-login');
        $loginButton.attr('disabled', 'disabled');

        var _currentForm = $(this).closest('#formLogin');
        if (!_currentForm.valid()) { console.log('form is not valid'); return; }

        var url = $(this).attr("action");
        var formData = $(this).serialize();
        $.ajax({
            url: url,
            type: "POST",
            data: formData,
            dataType: "json",
            success: function (response) {
                $loginButton.removeAttr('disabled');

                if (response.status === enumToastStatus.Fail) {
                    app.Notify(AlertTypeConstants.Warning, response.message, AlertIconConstants.Warning);
                    return;
                }
                if (response.type === enumActionResultType.Url) {
                    var message = response.message ? response.message : '/';
                    window.location = `${message}`;
                    return;
                }
            },
            error: function (err) {
                var msg = `Warning, There was an error while trying to login or check your internet connection.`;
                app.Notify(AlertTypeConstants.Warning, msg, AlertIconConstants.Warning);
            }
        })
    });
    $("#formSignup").on("submit", function (event) {
        event.preventDefault();
        var _currentForm = $(this).closest('#formSignup');
        if (!_currentForm.valid()) { console.log('form is not valid'); return; }

        var url = $(this).attr("action");
        var method = $(this).attr("method");
        var formData = $(this).serialize();
        $.ajax({
            url: url,
            type: method,
            data: formData,
            dataType: "json",
            success: function (response) {
                if (response.status === enumToastStatus.Fail) {
                    app.Notify(AlertTypeConstants.Warning, response.message, AlertIconConstants.Warning);
                    return;
                }
                if (response.type === enumActionResultType.Url) {
                    var message = response.message ? response.message : '/';
                    window.location = `${message}`;
                    return;
                }
                if (response.type === enumActionResultType.Message) {
                    var canVisible = false;
                    accountManager.toggleBookNowNLoginVisibility(canVisible);

                    toastr[enumToastType.Success](response.message);
                    return;
                }
            },
            error: function (err) {
                var msg = `Warning, There was an error while trying to signup or check your internet connection.`;
                app.Notify(AlertTypeConstants.Warning, msg, AlertIconConstants.Warning);
            }
        })
    });
    $("#formOtpGenerate").on("submit", function (event) {
        event.preventDefault();

        var $submitButton = $('.btn-generate-otp');
        $submitButton.attr('disabled', 'disabled');

        var _currentForm = $(this).closest('#formOtpGenerate');
        if (!_currentForm.valid()) { console.log('form is not valid'); return; }

        var url = $(this).attr("action");
        var formData = $(this).serialize();
        $.ajax({
            url: url,
            type: "POST",
            data: formData,
            dataType: "json",
            success: function (response) {
                $submitButton.removeAttr('disabled');

                if (response.status === enumToastStatus.Fail) {
                    app.Notify(AlertTypeConstants.Warning, response.message, AlertIconConstants.Warning);
                    return;
                }
                if (response.type === enumActionResultType.Url) {
                    var message = response.message ? response.message : '/';
                    window.location = `${message}`;
                    return;
                }
            },
            error: function (err) {
                var msg = `Warning, There was an error while trying to generate otp or check your internet connection.`;
                app.Notify(AlertTypeConstants.Warning, msg, AlertIconConstants.Warning);
            }
        })
    });
    $("#formChangePassword").on("submit", function (event) {
        event.preventDefault();
        var _currentForm = $(this).closest('#formChangePassword');
        if (!_currentForm.valid()) { console.log('form is not valid'); return; }

        var $submitButton = $('.btn-change-password');
        $submitButton.attr('disabled', 'disabled');

        var url = $(this).attr("action");
        var method = $(this).attr("method");
        var formData = $(this).serialize();
        $.ajax({
            url: url,
            type: method,
            data: formData,
            dataType: "json",
            success: function (response) {
                $submitButton.removeAttr('disabled');

                if (response.status === enumToastStatus.Fail) {
                    app.Notify(AlertTypeConstants.Warning, response.message, AlertIconConstants.Warning);
                    return;
                }
                if (response.type === enumActionResultType.Url) {
                    var url = response.message;
                    window.location = `${url}`;
                    return;
                }
            },
            error: function (err) {
                var msg = `Warning, There was an error while trying to change password or check your internet connection.`;
                app.Notify(AlertTypeConstants.Warning, msg, AlertIconConstants.Warning);
            }
        })
    });
    $("#formForgotPassword").on("submit", function (event) {
        event.preventDefault();
        var _currentForm = $(this).closest('#formForgotPassword');
        if (!_currentForm.valid()) { console.log('form is not valid'); return; }

        var url = $(this).attr("action");
        var method = $(this).attr("method");
        var formData = $(this).serialize();
        $.ajax({
            url: url,
            type: method,
            data: formData,
            dataType: "json",
            success: function (response) {
                if (response.status === enumToastStatus.Fail) {
                    app.Notify(AlertTypeConstants.Warning, response.message, AlertIconConstants.Warning);
                    return;
                }
                if (response.type === enumActionResultType.Url) {
                    var url = response.message;
                    window.location = `${url}`;
                    return;
                }
            },
            error: function (err) {
                var msg = `Warning, There was an error while trying to reset password or check your internet connection.`;
                app.Notify(AlertTypeConstants.Warning, msg, AlertIconConstants.Warning);
            }
        })
    });
    $("#formResetPassword").on("submit", function (event) {
        event.preventDefault();
        var _currentForm = $(this).closest('#formResetPassword');
        if (!_currentForm.valid()) { console.log('form is not valid'); return; }

        var url = $(this).attr("action");
        var method = $(this).attr("method");
        var formData = $(this).serialize();
        $.ajax({
            url: url,
            type: method,
            data: formData,
            dataType: "json",
            success: function (response) {
                if (response.status === enumToastStatus.Fail) {
                    app.Notify(AlertTypeConstants.Warning, response.message, AlertIconConstants.Warning);
                    return;
                }
                if (response.type === enumActionResultType.Url) {
                    var url = response.message;
                    window.location = `${url}`;
                    return;
                }
            },
            error: function (err) {
                var msg = `Warning, There was an error while trying to reset password or check your internet connection.`;
                app.Notify(AlertTypeConstants.Warning, msg, AlertIconConstants.Warning);
            }
        })
    });
    $('.btn-social-login').on('click', function () {
        var socialLoginUrl = $(this).attr('socialloginurl');

        if (!socialLoginUrl || socialLoginUrl === '') { toastr[enumToastType.Error]('There was an error while login'); return; }

        window.location = `${socialLoginUrl}`;
        return;
    });
    $('#btnSendVerificationCode').on('click', function () {
        $('.verification-instruction').text('');
        $('.section-verify-instruction').hide();
        $('.section-password-confirm-password').hide();
        $('.section-btn-verifycode').hide();
        $('#VerificationCode').val('');

        var verificationType = $(`input[type="radio"][name="AccountVerificationType"]:checked`).val();
        var emailOrPhone = $('#EmailOrPhone').val();
        if (!emailOrPhone || emailOrPhone === '') {
            toastr[enumToastType.Error]('Enter Valid Email Or Phone');return;
        }

        if (verificationType === ActionVerificationTypeConstants.SMS && emailOrPhone.length!==11) {
            toastr[enumToastType.Error]('Phone Number must be within 11 digits'); return;
        }

        var url = `/account/generateaccountverificationcode`;

        var formData = {
            EmailOrPhone: emailOrPhone,
            AccountVerificationType:verificationType,
            VerificationCode: "N/A"
        }

        $.ajax({
            url: url,
            type: 'POST',
            data: formData,
            dataType: "json",
            success: function (response) {
                if (response.status === enumToastStatus.Fail) {
                    app.Notify(AlertTypeConstants.Warning, response.message, AlertIconConstants.Warning);
                    return;
                }

                toastr[enumToastType.Success](response.message);

                var verificationInfoType = verificationType === ActionVerificationTypeConstants.SMS ?
                    'Mobile' : 'Email';

                var instruction = `We sent a verificaion code to your ${verificationInfoType}`;
                $('.verification-instruction').text(instruction);
                $('.section-verify-instruction').show();
                $('.section-btn-verifycode').show();
            },
            error: function (err) {
                var msg = `Warning, There was an error while trying to reset password or check your internet connection.`;
                app.Notify(AlertTypeConstants.Warning, msg, AlertIconConstants.Warning);
            }
        })
    });

    $('#verifyCode').on('click', function () {
        $('.section-password-confirm-password').hide();
        var verificationType = $(`input[type="radio"][name="AccountVerificationType"]:checked`).val();
        var emailOrPhone = $('#EmailOrPhone').val();
        var verificationCode = $('#VerificationCode').val();

        var url = `/account/verifyresettoken`;

        var formData = {
            EmailOrPhone: emailOrPhone,
            AccountVerificationType:verificationType,
            VerificationCode:verificationCode
        }

        $.ajax({
            url: url,
            type: 'POST',
            data: formData,
            dataType: "json",
            success: function (response) {
                if (response.status === enumToastStatus.Fail) {
                    app.Notify(AlertTypeConstants.Warning, response.message, AlertIconConstants.Warning);
                    return;
                }
                $('.section-password-confirm-password').show();
            },
            error: function (err) {
                var msg = `Warning, There was an error while trying to reset password or check your internet connection.`;
                app.Notify(AlertTypeConstants.Warning, msg, AlertIconConstants.Warning);
            }
        })
    })
});