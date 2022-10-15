
var checkoutDigitalDisplayManager = {
    inint: function () {
        //default disable checkout button
        var $btnMakePayment = $('.btn-continue-payment');
        $btnMakePayment.attr('disabled', 'disabled');

        // get estimated costing
        this.getEstimatedCosting();

        this.fixedDiscountActions();
    },   
    getEstimatedCosting: function () {
        var masterTableId = $('#ABDigitalDisplayId').val();
        var bookingNo = $('#BookingNo').val();
        var manualDiscountPercentage = $('#ManualDiscountPercentage').val();        
        var fixedAmount = $('#FixedAmount').val();
        var isFixed = $('#IsFixed').is(':checked');       

        $('.section-vat').addClass('hide-section');
        $('.section-netpayable').addClass('hide-section');
        /*
        if (this.isNatinalEdition()) {
            $('.section-vat').removeClass('hide-section');
            $('.section-netpayable').removeClass('hide-section');
        }
        */

        if (!masterTableId || !bookingNo) {
            $('.total-estimated-costing').text(0);            
            $('.total-net-amount').text(0);
            $('.vat-amount').text(0);
            $('.net-payable').text(0);            
            if ($('.order-comission') && $('.order-comission').length > 0)
                $('.order-comission').text(0);
            app.Notify(AlertTypeConstants.Warning, 'Booking No. not found. Please contact support team!', AlertIconConstants.Warning); return
        };      

        var formData = {            
            MasterTableId: masterTableId,
            BookingNo: bookingNo,
            ManualDiscountPercentage: manualDiscountPercentage,            
            IsFixed: isFixed,
            FixedAmount: fixedAmount,
        };

        $.ajax({
            url: '/CheckoutDigitalDisplay/GetOrderTotalAmount',
            type: "POST",
            data: formData,
            dataType: "json",
            success: function (data) {
                if (!data.status) {
                    app.Notify(AlertTypeConstants.Warning, data.message, AlertIconConstants.Warning);
                    $('.total-estimated-costing').text(0);                    
                    $('.total-net-amount').text(0);                    
                    $('.vat-amount').text(0);
                    $('.net-payable').text(0);

                    if ($('.order-comission') && $('.order-comission').length > 0)
                        $('.order-comission').text(0)

                    return;
                }

                $('.total-estimated-costing').text(parseInt(Math.round(data.estimatedTotalAmount)));
                
                $('.total-net-amount').text(parseInt(Math.round(data.netAmount)));
                
                if ($('.order-comission') && $('.order-comission').length > 0)
                    $('.order-comission').text(parseInt(Math.round(data.commission)));

                $('.vat-amount').text(parseInt(Math.round(data.vatAmount)));
                var netPayable = parseInt(Math.round(Number(data.netAmount)) + Math.round(Number(data.vatAmount)) - Math.round(Number(data.commission)));
                $('.net-payable').text(netPayable);
            },
            error: function (err) {
                var msg = `Warning, There was an error while trying to get estimated costing or check your internet connection.`;
                app.Notify(AlertTypeConstants.Warning, msg, AlertIconConstants.Warning);
            }
        })
    },
    fixedDiscountActions: function () {
        $('.section-discount-amount').addClass('hide-section');
        var isFixed = $("#IsFixed").is(':checked');
        $('#FixedAmount').val(0.00);

        if (isFixed) {
            var netPayable = $('.net-payable').text();
            $('#FixedAmount').val(netPayable);
            $('#ManualDiscountPercentage').val(0);
            $('.section-discount-amount').removeClass('hide-section');
        }

        //get estimated costing
        checkoutDigitalDisplayManager.getEstimatedCosting();
    },
    getDefaultDiscount: function () {
        var paymentModeId = $('input[type="radio"][name="PaymentModeId"]:checked').val();
        var privateAdType = $('#PrivateAdType').val();
        if (privateAdType === PrivateAdTypesConstants.Inhouse) {
            $('#FixedAmount').val(0); return;
        }

        var formData = {
            paymentModeId: paymentModeId
        };

        $.ajax({
            url: '/CheckoutDigitalDisplay/GetManualDiscount',
            type: "POST",
            data: formData,
            dataType: "json",
            success: function (data) {
                if (!data.status) {
                    app.Notify(AlertTypeConstants.Warning, data.message, AlertIconConstants.Warning);
                    $('#ManualDiscountPercentage').val(0);
                    return;
                }

                $('#ManualDiscountPercentage').val(data.manualDiscountPercentage);
            },
            error: function (err) {
                var msg = `Warning, There was an error while trying to get discount or check your internet connection.`;
                app.Notify(AlertTypeConstants.Warning, msg, AlertIconConstants.Warning);
            }
        })
    },
}

$(function () {    

    checkoutDigitalDisplayManager.inint();

    $('#ManualDiscountPercentage').on('blur', function () {
        //get estimated costing
        checkoutDigitalDisplayManager.getEstimatedCosting();
    });

    $('#FixedAmount').on('blur', function () {       
        //get estimated costing
        checkoutDigitalDisplayManager.getEstimatedCosting();
    });

    $('input[type="radio"][name="PaymentModeId"]').on('click', function () {
        //get default discount
        checkoutDigitalDisplayManager.getDefaultDiscount();
    })

    $(':checkbox[name=AcceptTermsAndConditions]').on('click', function () {
        var acceptTermsCondition = $('input[name="AcceptTermsAndConditions"]:checked').length;
        var $btnMakePayment = $('.btn-continue-payment');
        $btnMakePayment.attr('disabled', 'disabled');

        if (acceptTermsCondition) {
            $btnMakePayment.removeAttr('disabled');
        }
    });

    $('#IsFixed').on('click', function () {
        checkoutDigitalDisplayManager.fixedDiscountActions();
    })

    $("#form-digital-display-checkout-payment").on("submit", function (event) {
        event.preventDefault();
        var _currentForm = $(this).closest('#form-digital-display-checkout-payment');
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
                var msg = `Warning, There was an error while trying to continue payment or check your internet connection.`;
                app.Notify(AlertTypeConstants.Warning, msg, AlertIconConstants.Warning);
            }
        })
    }); 
})
