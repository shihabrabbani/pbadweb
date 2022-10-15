//const { round } = require("lodash");

var editionList = [];

var checkoutManager = {
    init: function () {

        //prevent to go back in browser
        app.preventBackPageInBrowser();

        //default disable checkout button
        var $btnMakePayment = $('.btn-continue-payment');
        $btnMakePayment.attr('disabled', 'disabled');

        var nationalEdition = this.isDefaultNatinalEdition();
        if (nationalEdition) {
            $("input[type=checkbox][name=NationalEdition]").attr('checked', 'checked');
        }

        //default National Edition Checking
        checkoutManager.defaultNationalEditionChecked();        

        // get estimated costing
        checkoutManager.getEstimatedCosting();
    },
    isDefaultNatinalEdition: function () {
        var defaultEdition = $('#DefaultEditionId').val();
        var nationalEdition = (defaultEdition === EditionConstants.National);
        return nationalEdition;
    },
    isNatinalEdition: function () {   
        var nationalEdition = $("input[type=checkbox][name=NationalEdition]").is(':checked');
        return nationalEdition;
    },
    getEstimatedCosting: function () {
        var masterTableId = $('#ABPrintClassifiedTextId').val();
        var bookingNo = $('#BookingNo').val();
        var manualDiscountPercentage = $('#ManualDiscountPercentage').val();
        var editionIds = _.map(editionList, 'EditionId');

        $('.section-vat').addClass('hide-section');
        //$('.section-netpayable').addClass('hide-section');
        if (this.isNatinalEdition()) {
            $('.section-vat').removeClass('hide-section');
            //$('.section-netpayable').removeClass('hide-section');
        }

        if (!masterTableId || !bookingNo) {
            app.Notify(AlertTypeConstants.Warning, 'Booking No. not found. Please contact support team!', AlertIconConstants.Warning);            
            return;
        };

        if (!editionIds || editionIds.length<=0) {
            app.Notify(AlertTypeConstants.Warning, 'Editions not found. Please check atleast one edition!', AlertIconConstants.Warning);
            return;
        };

        var formData = {
            EditionIds: editionIds,
            MasterTableId: masterTableId,
            BookingNo: bookingNo,
            NationalEdition: this.isNatinalEdition(),
            ManualDiscountPercentage: manualDiscountPercentage
        };

        $.ajax({
            url: '/Checkout/GetOrderTotalAmount',
            type: "POST",
            data: formData,
            dataType: "json",
            success: function (data) {
                if (!data.status) {
                    app.Notify(AlertTypeConstants.Warning, data.message, AlertIconConstants.Warning);
                    $('.total-estimated-costing').text(0);
                    $('.edition-offer-discount').text(0);
                    $('.total-net-amount').text(0);

                    $('.vat-amount').text(0);
                    $('.net-payable').text(0);

                    if ($('.order-comission') && $('.order-comission').length > 0)
                        $('.order-comission').text(0);

                    //get total discounts
                    checkoutManager.getTotalDiscounts();
                    return;
                }

                $('.total-estimated-costing').text(parseInt(Math.round(data.estimatedTotalAmount)));
                $('.edition-offer-discount').text(parseInt(Math.round(data.editionDiscountPercentage)));
                $('.total-net-amount').text(parseInt(Math.round(data.netAmount)));
                if ($('.order-comission') && $('.order-comission').length > 0)
                    $('.order-comission').text(parseInt(Math.round(data.commission)));

                $('.vat-amount').text(parseInt(Math.round(data.vatAmount)));
                var netPayable = parseInt(Math.round(Number(data.netAmount)) + Math.round(Number(data.vatAmount)) - Math.round(Number(data.commission)) );
                $('.net-payable').text(netPayable);               

                //get total discounts
                checkoutManager.getTotalDiscounts();
            },
            error: function (err) {
                var msg = `Warning, There was an error while trying to get discount or check your internet connection.`;
                app.Notify(AlertTypeConstants.Warning, msg, AlertIconConstants.Warning);
            }
        })
    },
    getDefaultDiscount: function () {
        var paymentModeId = $('input[type="radio"][name="PaymentModeId"]:checked').val();

        var formData = {
            paymentModeId: paymentModeId
        };

        $.ajax({
            url: '/Checkout/GetManualDiscount',
            type: "POST",
            data: formData,
            dataType: "json",
            success: function (data) {
                if (!data.status) {
                    app.Notify(AlertTypeConstants.Warning, data.message, AlertIconConstants.Warning);
                    $('#ManualDiscountPercentage').val(0);
                    return;
                }

                $('#ManualDiscountPercentage').val(parseInt(Math.round(data.manualDiscountPercentage)));
            },
            error: function (err) {
                var msg = `Warning, There was an error while trying to get discount or check your internet connection.`;
                app.Notify(AlertTypeConstants.Warning, msg, AlertIconConstants.Warning);
            }
        })
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
    getTotalDiscounts: function () {
        var dateOfferDiscount = $('.date-offer-discount').text();
        var editionOfferDiscount = $('.edition-offer-discount').text();
        var mannualDiscount = $('#ManualDiscountPercentage').val();

        var totalDiscount = parseInt(Math.round(parseFloat(dateOfferDiscount) + Number(editionOfferDiscount) + Number(mannualDiscount)));
        $('.total-discounts').text(parseInt(Math.round(totalDiscount)));
    }
}

$(function () {
    checkoutManager.init();

    $(':checkbox[name=NationalEdition]').on('click', function () {
        //default National Edition Checking
        checkoutManager.defaultNationalEditionChecked();

        // get estimated costing
        checkoutManager.getEstimatedCosting();
    });

    $(':checkbox[name=EditionIds]').on('click', function () {
        var totalCheckedEditions = $('input[name="EditionIds"]:checked').length;
        var totalEditions = $('input[name="EditionIds"]').length;

        $(':checkbox[name=NationalEdition]').prop('checked', false);
        if (totalEditions === totalCheckedEditions) {
            $(':checkbox[name=NationalEdition]').prop('checked', this.checked);

            //default National Edition Checking
            checkoutManager.defaultNationalEditionChecked();

            // get estimated costing
            checkoutManager.getEstimatedCosting();

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

        if (!checkoutManager.isNatinalEdition()) {
            var nationalEdition = $("input[type=checkbox][name=NationalEdition]").attr('NationalEdition');
            _.remove(editionList, function (item) { return item.EditionId === nationalEdition; });           
        }
        
        var editionNames = _.map(editionList, 'EditionName');

        var editionsInString = editionNames.join();
        $('.selected-editions').html(editionsInString);

        // get estimated costing
        checkoutManager.getEstimatedCosting();
    });

    $('#ManualDiscountPercentage').on('keyup', function () {
        //get estimated costing
        checkoutManager.getEstimatedCosting();
    });

    $('input[type="radio"][name="PaymentModeId"]').on('click', function () {
        //get default discount
        checkoutManager.getDefaultDiscount();
    })

    $(':checkbox[name=AcceptTermsAndConditions]').on('click', function () {
        var acceptTermsCondition = $('input[name="AcceptTermsAndConditions"]:checked').length;
        var $btnMakePayment = $('.btn-continue-payment');
        $btnMakePayment.attr('disabled', 'disabled');

        if (acceptTermsCondition) {
            $btnMakePayment.removeAttr('disabled');
        }
    });

    $("#form-checkout-payment").on("submit", function (event) {
        event.preventDefault();
        var _currentForm = $(this).closest('#form-checkout-payment');
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
