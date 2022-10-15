
var editionList = [];

var checkoutGovtPrivateDisplayManager = {
    init: function () {

        //default disable checkout button
        var $btnMakePayment = $('.btn-continue-payment');
        $btnMakePayment.attr('disabled', 'disabled');

        var nationalEdition = this.isDefaultNatinalEdition();
        if (nationalEdition) {
            $("input[type=checkbox][name=NationalEdition]").attr('checked', 'checked');
        }

        //default National Edition Checking
        checkoutGovtPrivateDisplayManager.defaultNationalEditionChecked();

        // get estimated costing
        checkoutGovtPrivateDisplayManager.getEstimatedCosting();

        this.fixedDiscountActions();
    },
    isDefaultNatinalEdition: function () {
        var defaultEdition = $('#DefaultEditionId').val();
        var nationalEdition = (defaultEdition === EditionConstants.National);
        return nationalEdition;
    },
    defaultNationalEditionChecked: function () {
        editionList = [];
        var isChecked = $("input[type=checkbox][name=NationalEdition]").is(':checked');

        //check all editions
        $(':checkbox[name=EditionIds]').prop('checked', isChecked);

        $('input[name=EditionIds]:checked').each(function () {
            var editionId = $(this).attr('value');
            var editionName = $(this).attr('edition-text');
            _.remove(editionList, function (item) { return item.EditionId === editionId; });

            if (isChecked) {
                var edition = {
                    EditionId: editionId,
                    EditionName: editionName,
                };
                editionList.push(edition);
            }
        });

        var editionName = $('#DefaultEditionName').val();
        var editionId = $('#DefaultEditionId').val();
        _.remove(editionList, function (item) { return item.EditionId === editionId; });

        var edition = {
            EditionId: editionId,
            EditionName: editionName,
        };
        editionList.push(edition);

        var nationalEdition = '';

        if (this.isNatinalEdition()) {
            nationalEdition = $("input[type=checkbox][name=NationalEdition]").attr('NationalEdition');
            _.remove(editionList, function (item) { return item.EditionId === nationalEdition; });

            edition = {
                EditionId: nationalEdition,
                EditionName: 'National',
            };
            editionList.push(edition);

            $('.selected-editions').html('National');
        }
        else {
            nationalEdition = $("input[type=checkbox][name=NationalEdition]").attr('NationalEdition');
            _.remove(editionList, function (item) { return item.EditionId === nationalEdition; });

            $(`input[type=checkbox][id=EditionIds_${editionId}]`).prop('checked', true);

            var editionNames = _.map(editionList, 'EditionName');
            var editionsInString = editionNames.join();
            $('.selected-editions').html(editionsInString);
        }
    },
    getEstimatedCosting: function () {
        var masterTableId = $('#ABPrintPrivateDisplayId').val();
        var bookingNo = $('#BookingNo').val();
        var manualDiscountPercentage = $('#ManualDiscountPercentage').val();
        var editionPageId = $('#EditionPageId').val();
        var editionIds = _.map(editionList, 'EditionId');
        var fixedAmount = $('#FixedAmount').val();
        var isFixed = $('#IsFixed').is(':checked');

        $('.section-vat').addClass('hide-section');
        $('.section-netpayable').addClass('hide-section');
        if ($("input[type=checkbox][name=NationalEdition]").is(':checked')) {
            $('.section-vat').removeClass('hide-section');
            $('.section-netpayable').removeClass('hide-section');
        }

        if (!masterTableId || !bookingNo) {
            $('.total-estimated-costing').text(0);
            $('.edition-offer-discount').text('0');
            $('.total-net-amount').text(0);
            $('.vat-amount').text(0);
            $('.net-payable').text(0);
            $('.ad-rate').text(0);
            if ($('.order-comission') && $('.order-comission').length > 0)
                $('.order-comission').text(0);
            app.Notify(AlertTypeConstants.Warning, 'Booking No. not found. Please contact support team!', AlertIconConstants.Warning);
            return
        };

        if (!editionIds) {
            $('.total-estimated-costing').text(0);
            $('.edition-offer-discount').text('0');
            $('.total-net-amount').text(0);
            $('.vat-amount').text(0);
            $('.net-payable').text(0);
            $('.ad-rate').text(0);
            if ($('.order-comission') && $('.order-comission').length > 0)
                $('.order-comission').text(0);
            app.Notify(AlertTypeConstants.Warning, 'Editions not found. Please check atleast one edition!', AlertIconConstants.Warning); return
        };

        var formData = {
            EditionIds: editionIds,
            MasterTableId: masterTableId,
            BookingNo: bookingNo,
            ManualDiscountPercentage: manualDiscountPercentage,
            NationalEdition: this.isNatinalEdition(),
            EditionPageId: editionPageId,
            IsFixed: isFixed,
            FixedAmount: fixedAmount,  
        };

        $.ajax({
            url: '/CheckoutGovtPrivateDisplay/GetOrderTotalAmount',
            type: "POST",
            data: formData,
            dataType: "json",
            success: function (data) {
                if (!data.status) {                    
                    app.Notify(AlertTypeConstants.Warning, data.message, AlertIconConstants.Warning);
                    $('.total-estimated-costing').text(0);
                    $('.edition-offer-discount').text('0 %');
                    $('.total-net-amount').text(0);
                    $('.ad-rate').text(0);
                    $('.vat-amount').text(0);
                    $('.net-payable').text(0);

                    if ($('.order-comission') && $('.order-comission').length > 0)
                        $('.order-comission').text(0)

                    return;
                }              

                $('.total-estimated-costing').text(parseInt(Math.round(data.estimatedTotalAmount)));
                $('.edition-offer-discount').text(parseInt(Math.round(data.editionDiscountPercentage)));
                $('.total-net-amount').text(parseInt(Math.round(data.netAmount)));
                $('.ad-rate').text(parseInt(Math.round(data.adRate)));

                if ($('.order-comission') && $('.order-comission').length > 0)
                    $('.order-comission').text(parseInt(Math.round(data.commission)));

                $('.vat-amount').text(data.vatAmount);
                var netPayable = parseInt(Math.round(Number(data.netAmount)) + Math.round(Number(data.vatAmount)) - Math.round(Number(data.commission)));
                $('.net-payable').text(netPayable); 

            },
            error: function (err) {
                var msg = `Warning, There was an error while trying to get estimated costing or check your internet connection.`;
                app.Notify(AlertTypeConstants.Warning, msg, AlertIconConstants.Warning);
            }
        })
    },
    isNatinalEdition: function () {
        var nationalEdition = $("input[type=checkbox][name=NationalEdition]").is(':checked');
        return nationalEdition;
    },
    getDefaultDiscount: function () {
        var paymentModeId = $('input[type="radio"][name="PaymentModeId"]:checked').val();

        var formData = {
            paymentModeId: paymentModeId
        };

        $.ajax({
            url: '/CheckoutGovtPrivateDisplay/GetManualDiscount',
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
    fixedDiscountActions: function () {
        $('.section-discount-amount').addClass('hide-section');
        var isFixed = $("#IsFixed").is(':checked');
        $('#FixedAmount').val(0.00);

        if (isFixed) {
            var netPayable = $('.net-payable').text();
            $('#FixedAmount').val(netPayable);
            $('.section-discount-amount').removeClass('hide-section');     
        }

        //get estimated costing
        checkoutGovtPrivateDisplayManager.getEstimatedCosting();
    },
    getManualDiscountAmount: function () {
        var estimatedCost = $('.total-estimated-costing').text();
        var discountPercentage = $('#ManualDiscountPercentage').val();

        var fixedAmount = Number(estimatedCost) - ((Number(estimatedCost) * Number(discountPercentage)) / 100);
        $('#FixedAmount').val(fixedAmount.toFixed(2));
    },
    getManualDiscountPercentage: function () {
        var estimatedCost = $('.total-estimated-costing').text();
        var fixedAmount = $('#FixedAmount').val();
        var discountPercentage = ((Number(estimatedCost) - Number(fixedAmount)) * 100) / Number(estimatedCost);
        $('#ManualDiscountPercentage').val(discountPercentage.toFixed(2));
    }
}

$(function () {
    checkoutGovtPrivateDisplayManager.init();  

    $(':checkbox[name=NationalEdition]').on('click', function () {
        //default National Edition Checking
        checkoutGovtPrivateDisplayManager.defaultNationalEditionChecked();

        // get estimated costing
        checkoutGovtPrivateDisplayManager.getEstimatedCosting();
    });

    $(':checkbox[name=EditionIds]').on('click', function () {
        var totalCheckedEditions = $('input[name="EditionIds"]:checked').length;
        var totalEditions = $('input[name="EditionIds"]').length;

        $(':checkbox[name=NationalEdition]').prop('checked', false);
        if (totalEditions === totalCheckedEditions) {
            $(':checkbox[name=NationalEdition]').prop('checked', this.checked);

            //default National Edition Checking
            checkoutGovtPrivateDisplayManager.defaultNationalEditionChecked();

            // get estimated costing
            checkoutGovtPrivateDisplayManager.getEstimatedCosting();

            return;
        }

        var isChecked = this.checked;

        var editionId = $(this).attr('value');
        var editionName = $(this).attr('edition-text');
        _.remove(editionList, function (item) { return item.EditionId === editionId; });

        if (isChecked) {
            var edition = {
                EditionId: editionId,
                EditionName: editionName,
            };
            editionList.push(edition);
        }

        if (!checkoutGovtPrivateDisplayManager.isNatinalEdition()) {
            var nationalEdition = $("input[type=checkbox][name=NationalEdition]").attr('NationalEdition');
            _.remove(editionList, function (item) { return item.EditionId === nationalEdition; });
        }

        var editionNames = _.map(editionList, 'EditionName');

        var editionsInString = editionNames.join();
        $('.selected-editions').html(editionsInString);

        // get estimated costing
        checkoutGovtPrivateDisplayManager.getEstimatedCosting();
    });

    $('#ManualDiscountPercentage').on('blur', function () {
        //get manual discount
        checkoutGovtPrivateDisplayManager.getManualDiscountAmount();

        //get estimated costing
        checkoutGovtPrivateDisplayManager.getEstimatedCosting();
    });

    $('#FixedAmount').on('keyup blur', function () {
        //get manual discount
        //checkoutGovtPrivateDisplayManager.getManualDiscountPercentage();

        //get estimated costing
        checkoutGovtPrivateDisplayManager.getEstimatedCosting();
    });

    $('input[type="radio"][name="PaymentModeId"]').on('click', function () {
        //get default discount
        checkoutGovtPrivateDisplayManager.getDefaultDiscount();
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
        checkoutGovtPrivateDisplayManager.fixedDiscountActions();
    })

    $("#form-private-display-checkout-payment").on("submit", function (event) {
        event.preventDefault();
        var _currentForm = $(this).closest('#form-private-display-checkout-payment');
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
