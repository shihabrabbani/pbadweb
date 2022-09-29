

var paymentCompleteManager = {
    init: function () {
        this.preventBackPageInBrowser();
        this.acceptTermsAndConditions();

        $(`input[type="radio"][name="PaymentType"][value="${CheckoutPaymentTypeConstants.Direct}"]`).prop('checked', true);
        $('.section-check-or-payorder-payment').addClass('d-none');
        $('.section-card-payment').addClass('d-none');
    },
    acceptTermsAndConditions: function () {
        var isAccceptConditions = $('input[type="checkbox"][name="AcceptTermsAndConditions"]').is(':checked');
                
        $('.btn-place-order').attr('disabled', 'disabled');
        if (isAccceptConditions) {            
            $('.btn-place-order').removeAttr('disabled');
        }
    },
    preventBackPageInBrowser: function () {
        window.history.pushState(null, "", window.location.href);
        window.onpopstate = function () {
            window.history.pushState(null, "", window.location.href);
        };
    },
}

$(function () {
    paymentCompleteManager.init();

    $("#PaymentMobileNumber").ForceNumericOnly();
    $("#PaymentMobileNumber").ForceNumericOnlyOnPast();

    $("#PaymentPaidAmount").ForceDecimalOnly(event);

    $('input[type="radio"][name="PaymentType"]').on('click', function () {
        var paymentType = $('input[name="PaymentType"]:checked').val();
        $('.section-direct-payment').addClass('d-none');
        $('.section-card-payment').addClass('d-none');
        $('.section-check-or-payorder-payment').addClass('d-none');

        $('.btn-continue-payment').removeClass('d-none');
        if (paymentType === `${CheckoutPaymentTypeConstants.Direct}`) {
            $('.section-direct-payment').removeClass('d-none');
        }
        else if (paymentType === `${CheckoutPaymentTypeConstants.Check_Or_Payorder}`) {
            $('.section-check-or-payorder-payment').removeClass('d-none');
        }
        else if (paymentType === `${CheckoutPaymentTypeConstants.Card}`) {
            $('.section-card-payment').removeClass('d-none');
            $('.btn-continue-payment').addClass('d-none');
        }
        else {
            $('.btn-continue-payment').addClass('d-none');
        }
    })

    $('.logo-direct-payment').on('click', function () {
        var paymentMethod = $(this).attr('PaymentMethod');
        $('#PaymentMethod').val(paymentMethod);

        $('.logo-direct-payment').removeClass('selected-payment-method');
        $(this).addClass('selected-payment-method');
    });

    $('input[type="checkbox"][name="AcceptTermsAndConditions"]').on('click', function () {
        paymentCompleteManager.acceptTermsAndConditions();
    })

    $('.btn-place-order').on('click', function () {
        var isAccceptConditions = $('input[type="checkbox"][name="AcceptTermsAndConditions"]').is(':checked');
        if (!isAccceptConditions) {
            app.Notify(AlertTypeConstants.Warning, "You must accept terms and conditions", AlertIconConstants.Warning);
            return;
        }

        $.alert.open('confirm', 'Are you sure you want to proceed to place order',
            function (button, value) {
                if (button === 'yes') {
                    var sslPaymentLink = $('#hiddenSSLPaymentLink').val();
                    window.location.href = sslPaymentLink;
                }
            }
        ); 

    })

    $("#form-checkout-complete-payment").on("submit", function (event) {
        event.preventDefault();
        var _currentForm = $(this).closest('#form-checkout-complete-payment');
        if (!_currentForm.valid()) { console.log('form is not valid'); return; }

        var $btnMakePayment = $('.btn-continue-payment');
        var btnPrevHtml = $btnMakePayment.html();
        $btnMakePayment.attr('disabled', 'disabled');
        $btnMakePayment.html(`<i class="fa fa-circle-o-notch fa-spin fa-fw"></i> Processing`);

        var url = $(this).attr("action");
        var formData = $(this).serialize();

        $.ajax({
            url: url,
            type: "POST",
            data: formData,
            dataType: "json",
            success: function (response) {
                $btnMakePayment.removeAttr('disabled');
                $btnMakePayment.html(btnPrevHtml);

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
                var msg = `Warning, There was an error while trying to complete payment or check your internet connection.`;
                app.Notify(AlertTypeConstants.Warning, msg, AlertIconConstants.Warning);
            }
        })
    });
})
