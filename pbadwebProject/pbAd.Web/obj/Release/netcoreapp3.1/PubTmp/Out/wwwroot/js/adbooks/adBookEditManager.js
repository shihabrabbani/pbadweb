
var datesBasedOffer = [];

var adBookEditManager = {
    init: function () {        
        var adContent = $('textarea[name="AdContent"]').val();
        $('.ad-preview').html(adContent);

        //configure ad enhancement
        this.configureAdEnhancement();

        //toggle person
        this.togglePerson();

        //get total words
        this.getTotalWords();

        var baseOfferDateList = $('#DatesBasedOffer').val();
        datesBasedOffer = baseOfferDateList.split(',');


        //generate base offer date container html
        this.generateBaseOfferDateContainerHtml();

        //get discount by date offer          
        this.getDiscountByDateOffer();

        this.personalActions();
    },
    togglePerson: function () {
        $('.section-advitiser').removeClass('d-none');
        var isPersonal = $("#Personal").is(':checked');
        if (!isPersonal)
            $('.section-advitiser').addClass('d-none');

        var $agencyAutoCom = $('#AgencyAutoComplete');
        var $agencyId = $('#AgencyId');
        $agencyAutoCom.removeAttr('readonly');

        if (isPersonal && ($agencyAutoCom && $agencyId)) {
            $agencyAutoCom.val('');
            $agencyId.val('');
            $agencyAutoCom.attr('readonly', 'readonly');
        }
    },
    loadSubCategoryByCategory: function (categoryId) {
        var ddlSubCategory = $("#SubCategoryId");
        $.ajax({
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            url: '/AdBook/GetSubCategoryList',
            data: { categoryId: categoryId },
            dataType: 'json',
            success: function (data) {
                ddlSubCategory.html('');
                $.each(data, function (id, option) {
                    ddlSubCategory.append($('<option></option>').val(option.value).html(option.text));
                });
            },
            error: function (request, status, error) {
                alert(request.statusText + "/" + request.statusText + "/" + error);
            }
        });
    },
    getDiscountByDateOffer: function () {
        if (!datesBasedOffer) {
            $('#DiscountPercentage').val(0);
            $('#OfferDateId').val(0);
            $('.discount-offer').text(0);
            return
        };

        var formData = {
            offerDates: datesBasedOffer
        };

        $.ajax({
            url: '/AdBook/GetDateOffer',
            type: "POST",
            data: formData,
            dataType: "json",
            success: function (data) {
                if (!data) {
                    $('#DiscountPercentage').val(0);
                    $('#OfferDateId').val(0);
                    $('.discount-offer').text(0);
                    return;
                }

                $('#DiscountPercentage').val(data.discountPercentage);
                $('#OfferDateId').val(data.offerDateId);
                $('.discount-offer').text(data.discountPercentage);
            },
            error: function (err) {
                var msg = `Warning, There was an error while trying to get discount or check your internet connection.`;
                app.Notify(AlertTypeConstants.Warning, msg, AlertIconConstants.Warning);
            }
        })
    },
    getTotalWords: function () {
        $('#TotalWordCount').val(0);
        $('.no-of-words').text(0);

        if (!$('textarea[name="AdContent"]') || $('textarea[name="AdContent"]').length <= 0) {
            //get estimated costing
            adBookEditManager.getEstimatedCosting();
            return;
        }
        var totalWords = 0;
        var words = $('textarea[name="AdContent"]').val();
        if (!words || words.trim() === "") {
            $('#TotalWordCount').val(totalWords);
            $('.no-of-words').text(totalWords);

            //get estimated costing
            adBookEditManager.getEstimatedCosting();
            return;
        }

        words = words.trim().split(' ');
        words = _.filter(words, function (item) { return item !== ''; });

        totalWords = words.length;
        $('#TotalWordCount').val(totalWords);
        $('.no-of-words').text(totalWords);

        //get estimated costing
        adBookEditManager.getEstimatedCosting();
    },
    totalWords: function () {
        if (!$('textarea[name="AdContent"]') || $('textarea[name="AdContent"]').length <= 0) return 0;

        var totalWords = 0;
        var words = $('textarea[name="AdContent"]').val();
        if (!words || words.trim() === "") return 0;

        words = words.trim().split(' ');
        words = _.filter(words, function (item) { return item !== ''; });
        totalWords = words.length;
        return totalWords;
    },
    populateInPreview: function () {
        $('.ad-preview').removeAttr('style');
        var adContent = $('textarea[name="AdContent"]').val();
        $('.ad-preview').html('');
        $('.ad-preview').html(adContent);
    },
    getEstimatedCosting: function () {
        var numberOfWords = $('#TotalWordCount').val();
        var perWordRate = $('#hiddenPerWordRate').val();
        $('#EstimatedTotal').val(0);
        $('.esitmated-cost-amount').text(0);

        if ((!numberOfWords || numberOfWords <= 0) || (!perWordRate || perWordRate <= 0)) {
            $('#EstimatedTotal').val(0);
            $('.esitmated-cost-amount').text(0);
            return;
        }

        var baseAmount = parseFloat(numberOfWords) * parseFloat(perWordRate);

        var adEnhancementTypeBulletPercentage = $("input[name='AdEnhancementTypeBullet'][type='checkbox']:checked").attr('ad-enhancement-type');
        if (!adEnhancementTypeBulletPercentage) {
            adEnhancementTypeBulletPercentage = 0;
        }

        var additionalAdEnhancementBulletAmount = ((baseAmount * parseFloat(adEnhancementTypeBulletPercentage)) / 100);

        var adEnhancementTypePercentage = $("input[name='AdEnhancementType'][type='checkbox']:checked").attr('ad-enhancement-type');
        if (!adEnhancementTypePercentage) {
            adEnhancementTypePercentage = 0;
        }

        var additionalAdEnhancementAmount = ((baseAmount * parseFloat(adEnhancementTypePercentage)) / 100);

        var totalBaseAmount = (baseAmount + additionalAdEnhancementBulletAmount + additionalAdEnhancementAmount).toFixed(2);
        $('#EstimatedTotal').val(totalBaseAmount);
        $('.esitmated-cost-amount').text(totalBaseAmount);
    },
    configureAdEnhancement: function () {
        var adContent = $('textarea[name="AdContent"]').val();
        $('.ad-preview').html(adContent);
        var adEnhancementType = $('input[type="checkbox"][name="AdEnhancementType"]:checked').val();
        var adEnhancementTypeBullet = $('input[type="checkbox"][name="AdEnhancementTypeBullet"]:checked').val();

        if (adEnhancementTypeBullet === AddEnhancementTypeConstants.Big_Bullet_Point_Single) {
            $('.ad-preview').prepend("&#128903; ");
            $(`input[type="checkbox"][name="AdEnhancementTypeBullet"][value="${AddEnhancementTypeConstants.Big_Bullet_Point_Double}"]`).prop('checked', false);
        }

        if (adEnhancementTypeBullet === AddEnhancementTypeConstants.Big_Bullet_Point_Double) {
            $('.ad-preview').prepend("&#128903;&#128903; ");
            $(`input[type="checkbox"][name="AdEnhancementTypeBullet"][value="${AddEnhancementTypeConstants.Big_Bullet_Point_Single}"]`).prop('checked', false);
        }

        if (adEnhancementType === AddEnhancementTypeConstants.Bold) {
            $('.ad-preview').css('font-weight', 'bold');

            $(`input[type="checkbox"][name="AdEnhancementType"][value="${AddEnhancementTypeConstants.BoldInScreen}"]`).prop('checked', false);
            $(`input[type="checkbox"][name="AdEnhancementType"][value="${AddEnhancementTypeConstants.BoldInScreenAndSingleBox}"]`).prop('checked', false);
            $(`input[type="checkbox"][name="AdEnhancementType"][value="${AddEnhancementTypeConstants.BoldInScreenAndDoubleBoxes}"]`).prop('checked', false);
        }

        if (adEnhancementType === AddEnhancementTypeConstants.BoldInScreen) {
            $('.ad-preview').css({ 'font-weight': 'bold', 'background-color': 'gray' });
            $(`input[type="checkbox"][name="AdEnhancementType"][value="${AddEnhancementTypeConstants.Bold}"]`).prop('checked', false);
            $(`input[type="checkbox"][name="AdEnhancementType"][value="${AddEnhancementTypeConstants.BoldInScreenAndSingleBox}"]`).prop('checked', false);
            $(`input[type="checkbox"][name="AdEnhancementType"][value="${AddEnhancementTypeConstants.BoldInScreenAndDoubleBoxes}"]`).prop('checked', false);
        }

        if (adEnhancementType === AddEnhancementTypeConstants.BoldInScreenAndSingleBox) {
            $('.ad-preview').css({ 'font-weight': 'bold', 'background-color': 'gray', 'border': '2px solid black' });

            $(`input[type="checkbox"][name="AdEnhancementType"][value="${AddEnhancementTypeConstants.Bold}"]`).prop('checked', false);
            $(`input[type="checkbox"][name="AdEnhancementType"][value="${AddEnhancementTypeConstants.BoldInScreen}"]`).prop('checked', false);
            $(`input[type="checkbox"][name="AdEnhancementType"][value="${AddEnhancementTypeConstants.BoldInScreenAndDoubleBoxes}"]`).prop('checked', false);
        }
        if (adEnhancementType === AddEnhancementTypeConstants.BoldInScreenAndDoubleBoxes) {
            $('.ad-preview').css({ 'font-weight': 'bold', 'background-color': 'gray', 'border': '2px solid black' });
            var appendedHtml = $('.ad-preview').html();
            $('.ad-preview').html(`<div style='border: 2px solid black; margin:1px;'> ${appendedHtml} </div>`);

            $(`input[type="checkbox"][name="AdEnhancementType"][value="${AddEnhancementTypeConstants.Bold}"]`).prop('checked', false);
            $(`input[type="checkbox"][name="AdEnhancementType"][value="${AddEnhancementTypeConstants.BoldInScreen}"]`).prop('checked', false);
            $(`input[type="checkbox"][name="AdEnhancementType"][value="${AddEnhancementTypeConstants.BoldInScreenAndSingleBox}"]`).prop('checked', false);
        }
    },
    generateBaseOfferDateContainerHtml: function () {

        if (datesBasedOffer.length <= 0) {
            var warningInfo = `<span><i class="fa fa-exclamation-triangle" aria-hidden="true"></i> No dates selected</span>`;
            $('.date-based-offer-container').html(warningInfo);
            $('#DatesBasedOffer').val('');
            return;
        }

        var dateBasedOfferContainer = '';
        $.each(datesBasedOffer, function (index, date) {
            dateBasedOfferContainer += `
                <div class="alert">
                    <strong>${date}</strong>
                    <a href="javascript:void(0);" base-offer-date='${date}' class="custom-close btn-remove-base-offer-date">×</a>
                </div>
            `;
        });
        $('.date-based-offer-container').html('');
        $('.date-based-offer-container').html(dateBasedOfferContainer);

        var newDatesBasedOffer = datesBasedOffer.join();
        $('#DatesBasedOffer').val('');
        $('#DatesBasedOffer').val(newDatesBasedOffer);
    },
    togglePersonal: function () {
        $('#Personal').prop("checked", false);
        $('#AdvertiserName').val('')
        $('#AdvertiserContactNumber').val('')
    },
    addDateOffer: function (dateBasedOffer) {
        var isExistInDatesBasedOffer = _.some(datesBasedOffer, function (item) { return item === dateBasedOffer; });
        if (isExistInDatesBasedOffer) { app.Notify(AlertTypeConstants.Warning, `Warning! Date already selected`, AlertIconConstants.Warning); return; }

        datesBasedOffer.push(dateBasedOffer);

        //generate base offer date container's html
        adBookEditManager.generateBaseOfferDateContainerHtml();

        //get discount by date offer          
        adBookEditManager.getDiscountByDateOffer();

        $('#DateBasedOffer').val('');
    },
    personalActions: function () {
        $('.section-advitiser').removeClass('d-none');

        var isPersonal = $("#Personal").is(':checked');
        if (!isPersonal)
            $('.section-advitiser').addClass('d-none');

        var userGroup = $('#UserGroup').val();
        if (userGroup === UserGroupConstants.Correspondent) {
            $('.section-advitiser').removeClass('d-none');
        }

        var $agencyAutoCom = $('#AgencyAutoComplete');
        var $agencyId = $('#AgencyId');
        $agencyAutoCom.removeAttr('readonly');

        if (isPersonal && ($agencyAutoCom && $agencyId)) {
            $agencyAutoCom.val('');
            $agencyId.val('');
            $agencyAutoCom.attr('readonly', 'readonly');
        }
    },
}

$(function () {
    adBookEditManager.init();

    $('#CategoryId').on('change', function () {
        var categoryId = $(this).val();

        if (!categoryId || categoryId <= 0) {
            var ddlSubCategory = $("#SubCategoryId");
            ddlSubCategory.append($('<option></option>').val('').html('Select Sub Category'));
            return;
        }

        adBookEditManager.loadSubCategoryByCategory(categoryId);
    })

    $('textarea[name="AdContent"]').on('keydown', function () {
        var prevAdCompose = $('textarea[name="AdContent"]').val();
        $('#previousAdCompose').val(prevAdCompose);
    })

    $('textarea[name="AdContent"]').on('keyup mouseup', function () {
        var totalWords = adBookEditManager.totalWords();
        var maxWords = parseInt($('#hiddenMaxWords').val());
        if (totalWords > maxWords) {
            var prevAdCompose = $('#previousAdCompose').val();
            $('textarea[name="AdContent"]').val(prevAdCompose);
            app.Notify(AlertTypeConstants.Warning, `Warning! You have entered maximum words of ${maxWords}`, AlertIconConstants.Warning);            
            return;
        }

        //populate in preview
        adBookEditManager.configureAdEnhancement();
            
        //get total words
        adBookEditManager.getTotalWords();
    })

    $('input[type="checkbox"][name="AdEnhancementTypeBullet"]').on('click', function () {
        var adEnhancementType = $(this).val();
        var adContent = $('textarea[name="AdContent"]').val();
        $('.ad-preview').removeAttr('style');
        //get total words
        adBookEditManager.getTotalWords();

        if (!adContent || adContent.length <= 0) {
            app.Notify(AlertTypeConstants.Warning, "Warning! Please start typing your ADD CONTENT", AlertIconConstants.Warning);            
            $('input[type="checkbox"][name="AdEnhancementTypeBullet"]').prop('checked', false);
            return;
        }
        var isChecked = $(`input[type="checkbox"][name="AdEnhancementTypeBullet"][value="${adEnhancementType}"]`).is(":checked")

        $(`input[type="checkbox"][name="AdEnhancementTypeBullet"]`).prop('checked', false);
        if (isChecked)
            $(`input[type="checkbox"][name="AdEnhancementTypeBullet"][value="${adEnhancementType}"]`).prop('checked', true);

        //configure ad enhancement
        adBookEditManager.configureAdEnhancement();

        //get total words
        adBookEditManager.getTotalWords();
    })

    $('input[type="checkbox"][name="AdEnhancementType"]').on('click', function () {
        var adEnhancementType = $(this).val();
        var adContent = $('textarea[name="AdContent"]').val();
        $('.ad-preview').removeAttr('style');
        //get total words
        adBookEditManager.getTotalWords();

        if (!adContent || adContent.length <= 0) {
            app.Notify(AlertTypeConstants.Warning, "Warning! Please start typing your ADD CONTENT", AlertIconConstants.Warning);            
            return;
        }

        var isChecked = $(`input[type="checkbox"][name="AdEnhancementType"][value="${adEnhancementType}"]`).is(":checked")

        $(`input[type="checkbox"][name="AdEnhancementType"]`).prop('checked', false);
        if (isChecked) $(`input[type="checkbox"][name="AdEnhancementType"][value="${adEnhancementType}"]`).prop('checked', true);

        //configure ad enhancement
        adBookEditManager.configureAdEnhancement();

        //get total words
        adBookEditManager.getTotalWords();
    })

    $('.btn-date-based-offer').on('click', function () {
        var dateBasedOffer = $('#DateBasedOffer').val();
        if (!dateBasedOffer) { return; }

        var todyDate = new Date();
        //get today formatted date
        var todayFormattedDate = app.getFormattedDate(todyDate);

        var tomorrow = new Date();
        tomorrow.setDate(new Date().getDate() + 1);
        //get tomorrow formatted date
        var tomorrowFormattedDate = app.getFormattedDate(tomorrow);

        if (!(tomorrowFormattedDate === dateBasedOffer)) { //check tomorrow booking
            adBookEditManager.addDateOffer(dateBasedOffer);
            return;
        }

         var todayFormattedDateWithTime = moment(`${todayFormattedDate} 16:00:00`, "DD-MMM-YYYY HH:mm:ss").toDate(); 
        if (!(todayFormattedDateWithTime < todyDate)) {
            adBookEditManager.addDateOffer(dateBasedOffer);
            return;
        }

        $.alert.open('confirm', 'Are you sure to add this date offer after 04:00 PM',
            function (button, value) {
                if (button === 'yes') {
                    adBookEditManager.addDateOffer(dateBasedOffer);
                }
            }
        );       
    });

    $('.date-based-offer-container').on('click', '.btn-remove-base-offer-date', function () {
        var baseOfferDate = $(this).attr('base-offer-date');

        _.remove(datesBasedOffer, function (date) { return date === baseOfferDate; });

        //generate base offer date container's html
        adBookEditManager.generateBaseOfferDateContainerHtml();

        //get discount by date offer
        adBookEditManager.getDiscountByDateOffer();

        $(this).remove();
    });

    $('#Personal').on('click', function () {
        //toggle person
        adBookEditManager.togglePerson();
    })

    $("#AgencyAutoComplete").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/adbook/agencyautocomplete',
                type: 'POST',
                data: { 'searchKey': request.term },
                dataType: 'json',
                success: function (data) {
                    response($.map(data.data, function (item) {
                        return {
                            label: item.agencyName,
                            value: item.agencyId
                        };
                    }));
                }
            });
        },
        minLength: 2,
        search: function (event, ui) {
            //$(this).addClass('autocom-loading');
        },
        response: function (event, ui) {
            //$(this).removeClass('autocom-loading');
        },
        focus: function (event, ui) {
            $("#AgencyAutoComplete").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            event.stopPropagation();
            $("#AgencyAutoComplete").val(ui.item.label);
            $("#AgencyId").val(ui.item.value);

            if (ui.item.value > 0) adBookEditManager.togglePersonal();

            return false;
        },
        change: function (event, ui) {
            if (!ui.item) {
                $("#AgencyAutoComplete").val('');
                $("#AgencyId").val('');
                return;
            }
            $("#AgencyAutoComplete").val(ui.item.label);
            $("#AgencyId").val(ui.item.value);

            if (ui.item.value > 0) adBookEditManager.togglePersonal();

            return false;
        }
    });

    $("#AgencyAutoComplete").on('keyup', function () {
        var agency = $(this).val();
        if (!agency) {
            $("#AgencyId").val('');
        }
    })

    $("#AdvertiserContactNumber").ForceNumericOnly();

     $('#AdvertiserContactNumber').on('focusout', function (event) {
        var mobileNoLenght = $(this).val().length;
        if (mobileNoLenght > 11) {
            var message = "Warning, Contact No. must be 11 digits";
            app.Notify(AlertTypeConstants.Warning, message, AlertIconConstants.Warning);
        }
    })

    $("#form-adbook").on("submit", function (event) {
        event.preventDefault();
        var _currentForm = $(this).closest('#form-adbook');
        if (!_currentForm.valid()) { console.log('form is not valid'); return; }

        var $btnMakePayment = $('.btn-make-payment');
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
                var msg = `Warning, There was an error while trying to make payment or check your internet connection.`;
                app.Notify(AlertTypeConstants.Warning, msg, AlertIconConstants.Warning);               
            }
        })
    });
})
