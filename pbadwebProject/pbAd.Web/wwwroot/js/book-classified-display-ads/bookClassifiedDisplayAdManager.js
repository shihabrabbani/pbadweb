
var datesBasedOffer = [];
var storedAdPreview=null;

var classifiedDisplayManager = {
    init: function () {
       
        classifiedDisplayManager.personalActions();

        var title = $('#Title').val();
        $('.classified-display-title').html(title);


        var adContent = $('textarea[name="AdContent"]').val();
        $('.classified-display-content').html(adContent);

        //get total words
        this.getTotalWords();

        //get total words
        this.getTotalTitleWords();

        var adPrevContainer = $('.classified-display-ad-preview-container').html();
        storedAdPreview = adPrevContainer;

        var adColumnInch = $('input[type="radio"][name="AdColumnInch"]').val();
        classifiedDisplayManager.getAdColumnInchRate(adColumnInch);
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
    getTotalTitleWords: function () {
        $('#TotalTitleWordCount').val(0);
        $('.no-of-title-words').text(0);

        if (!$('#Title') || $('#Title').length <= 0) return;

        var totalWords = 0;
        var words = $('#Title').val();
        if (!words || words.trim() === "") {
            $('#TotalTitleWordCount').val(totalWords);
            $('.no-of-title-words').text(totalWords);
            return;
        }

        words = words.trim().split(' ');
        totalWords = words.length;
        $('#TotalTitleWordCount').val(totalWords);
        $('.no-of-title-words').text(totalWords);
    },
    getTotalWords: function () {
        $('#TotalAdContentWordCount').val(0);
        $('.no-of-words').text(0);

        if (!$('textarea[name="AdContent"]') || $('textarea[name="AdContent"]').length <= 0) {
            return;
        }
        var totalWords = 0;
        var words = $('textarea[name="AdContent"]').val();
        if (!words || words.trim() === "") {
            $('#TotalAdContentWordCount').val(totalWords);
            $('.no-of-words').text(totalWords);
            return;
        }

        words = words.trim().split(' ');
        totalWords = words.length;
        $('#TotalWordCount').val(totalWords);
        $('.no-of-words').text(totalWords);
    },
    totalAdContentWords: function () {
        if (!$('textarea[name="AdContent"]') || $('textarea[name="AdContent"]').length <= 0) return 0;

        var totalAdContentWords = 0;
        var words = $('textarea[name="AdContent"]').val();
        if (!words || words.trim() === "") return 0;

        words = words.trim().split(' ');
        totalAdContentWords = words.length;
        return totalAdContentWords;
    },
    totalTitleWords: function () {
        if (!$('#Title') || $('#Title').length <= 0) return 0;

        var totalTitleWords = 0;
        var words = $('#Title').val();
        if (!words || words.trim() === "") return 0;

        words = words.trim().split(' ');
        totalTitleWords = words.length;
        return totalTitleWords;
    },
    populateInPreview: function () {
        var adContent = $('textarea[name="AdContent"]').val();
        $('.classified-display-content').html('');
        $('.classified-display-content').html(adContent);
    },
    loadPhotoInPreview: function () {
        var file = $("#ImageContent").get(0).files[0];
        if (file) {
            var reader = new FileReader();
            reader.onload = function () {       
                //var completeMatter = $("#CompleteMatter").is(':checked');                
                var defaultImage = `<img src='${reader.result}' />`
                $(".image-content-in-preview").removeClass('d-none');
                $(".image-content-in-preview").html(defaultImage);

                //get estimated costing
                //classifiedDisplayManager.getEstimatedCosting();
            }

            reader.readAsDataURL(file);
        }
    },
    generateBaseOfferDateContainerHtml: function () {

        if (datesBasedOffer.length <= 0) {
            var warningInfo = `<span><i class="fa fa-exclamation-triangle" aria-hidden="true"></i> No dates selected</span>`;
            $('.date-based-offer-container').html(warningInfo);
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
    getDiscountByDateOffer: function () {
        if (!datesBasedOffer) {
            $('.discount-offer').text(0);
            return
        };

        var formData = {
            offerDates: datesBasedOffer
        };

        $.ajax({
            url: '/bookclasssifieddisplayad/getdateoffer',
            type: "POST",
            data: formData,
            dataType: "json",
            success: function (data) {
                if (!data) {
                    //$('#DiscountPercentage').val(0);
                    //$('#OfferDateId').val(0);
                    $('.discount-offer').text(0);
                    return;
                }

                //$('#DiscountPercentage').val(data.discountPercentage);
                //$('#OfferDateId').val(data.offerDateId);
                $('.discount-offer').text(data.discountPercentage);
            },
            error: function (err) {
                var msg = `Warning, There was an error while trying to get discount or check your internet connection.`;
                app.Notify(AlertTypeConstants.Warning, msg, AlertIconConstants.Warning);
            }
        })
    },
    getEstimatedCosting: function () {
        var formData = new FormData();

        var fileUpload = $("#ImageContent").get(0);
        var files = fileUpload.files;

        if (files.length > 0) {
            var file = files[0];
            formData.append("ImageContent", file);
        }

        var title = $('#Title').val();
        formData.append("Title", title);

        var adContent = $('textarea[name="AdContent"]').val();
        formData.append("AdContent", adContent);
        
        $.ajax({
            url: '/bookclasssifieddisplayad/getestimatedcosting',
            type: "POST",
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.status === enumToastStatus.Fail) {
                    app.Notify(AlertTypeConstants.Warning, response.message, AlertIconConstants.Warning);
                    $('.esitmated-cost-amount').text(0);
                    $('#EstimatedTotal').val(0);
                    return;
                }

                $('.esitmated-cost-amount').text(parseFloat(response.estimatedCosting).toFixed(2));
                $('#EstimatedTotal').val(response.estimatedCosting);
            },
            error: function (err) {
                var msg = `Warning, There was an error while trying to make payment or check your internet connection.`;
                app.Notify(AlertTypeConstants.Warning, msg, AlertIconConstants.Warning);
            }
        })
    },
    getAdColumnInchRate: function (adColumnInch) {
        var formData = new FormData();
        
        formData.append("adColumnInch", adColumnInch);
        $.ajax({
            url: '/bookclasssifieddisplayad/GetAdColumnInchRate',
            type: "POST",
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.status === enumToastStatus.Fail) {
                    app.Notify(AlertTypeConstants.Warning, response.message, AlertIconConstants.Warning);
                    $('.ad-rate-inch').text(0);
                    $('.esitmated-cost-amount').text(0);
                    return;
                }
                var esitimatedCost = response.adColumnInchRate;
                $('.ad-rate-inch').text(response.adColumnInchRate);
                $('.esitmated-cost-amount').text(esitimatedCost);
               
            },
            error: function (err) {
                var msg = `Warning, There was an error while trying to make payment or check your internet connection.`;
                app.Notify(AlertTypeConstants.Warning, msg, AlertIconConstants.Warning);
            }
        })
    },
    addDateOffer: function (dateBasedOffer) {
        var isExistInDatesBasedOffer = _.some(datesBasedOffer, function (item) { return item === dateBasedOffer; });
        if (isExistInDatesBasedOffer) { app.Notify(AlertTypeConstants.Warning, `Warning! Date already selected`, AlertIconConstants.Warning); return; }

        datesBasedOffer.push(dateBasedOffer);

        //generate base offer date container's html
        classifiedDisplayManager.generateBaseOfferDateContainerHtml();

        //get discount by date offer          
        classifiedDisplayManager.getDiscountByDateOffer();

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
    classifiedDisplayManager.init();

    $('#CategoryId').on('change', function () {
        var categoryId = $(this).val();

        if (!categoryId || categoryId <= 0) {
            var ddlSubCategory = $("#SubCategoryId");
            ddlSubCategory.append($('<option></option>').val('').html('Select Sub Category'));
            return;
        }

        classifiedDisplayManager.loadSubCategoryByCategory(categoryId);
    })
    
    $('#Title').on('keyup blur', function () {
        /*
        var prevTitle = $('#previousTitle').val();
        var totalTitleWords = classifiedDisplayManager.totalTitleWords();
        var maxWords = parseInt($('#MaxTitleWords').val());
        if (totalTitleWords > maxWords) {
            $('#Title').val(prevTitle);
            toastr[enumToastType.Error](`Warning! You have entered title than maximum words of ${maxWords}`);
            return;
        }
        */
        var title = $(this).val();
        //$('#previousTitle').val(title);
        $('.classified-display-title').text(title);
        //get total words
        //classifiedDisplayManager.getTotalTitleWords();

        //calculate estimated costing
        //classifiedDisplayManager.getEstimatedCosting();
    })

    $('textarea[name="AdContent"]').on('keyup blur', function () {
        /*
        var prevAdCompose = $('#previousAdCompose').val();
        var totalAdContentWords = classifiedDisplayManager.totalAdContentWords();
        var maxWords = parseInt($('#MaxContentWords').val());
        if (totalAdContentWords > maxWords) {
            $('textarea[name="AdContent"]').val(prevAdCompose);
            toastr[enumToastType.Error](`Warning! You have entered maximum words of ${maxWords}`);
            return;
        }
        */

        var adContent = $('textarea[name="AdContent"]').val();
        //$('#previousAdCompose').val(adContent);

        //populate in preview
        classifiedDisplayManager.populateInPreview();

        //get total words
        //classifiedDisplayManager.getTotalWords();

        //get canvas and generate blob and calculate estimated costing
        //classifiedDisplayManager.getEstimatedCosting();
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
            classifiedDisplayManager.addDateOffer(dateBasedOffer);
            return;
        }

         var todayFormattedDateWithTime = moment(`${todayFormattedDate} 16:00:00`, "DD-MMM-YYYY HH:mm:ss").toDate(); 
        if (!(todayFormattedDateWithTime < todyDate)) {
            classifiedDisplayManager.addDateOffer(dateBasedOffer);
            return;
        }

        $.alert.open('confirm', 'Are you sure to add this date offer after 04:00 PM',
            function (button, value) {
                if (button === 'yes') {
                    classifiedDisplayManager.addDateOffer(dateBasedOffer);
                }
            }
        );
    });

    $('.date-based-offer-container').on('click', '.btn-remove-base-offer-date', function () {
        var baseOfferDate = $(this).attr('base-offer-date');

        _.remove(datesBasedOffer, function (date) { return date === baseOfferDate; });

        //generate base offer date container's html
        classifiedDisplayManager.generateBaseOfferDateContainerHtml();

        //get discount by date offer
        classifiedDisplayManager.getDiscountByDateOffer();

        $(this).remove();
    });

    $('#ImageContent').bind('fileuploadstop', function (e) {
        classifiedDisplayManager.loadPhotoInPreview();
    })

    $('#Personal').on('click', function () {
        classifiedDisplayManager.personalActions();
    })

    $('#CompleteMatter').on('click', function () {
        $selector = $('.dropify');
        //reset dropify
        app.resetDropify($selector);

        $('textarea[name="AdContent"]').removeAttr('disabled');
        $('input[name="Title"]').removeAttr('disabled');

        $('textarea[name="AdContent"]').val('');
        $('input[name="Title"]').val('');
        $('.classified-display-ad-preview-container').html(storedAdPreview);

        var completeMatter = $("#CompleteMatter").is(':checked');
        if (completeMatter) {
            $('textarea[name="AdContent"]').attr('disabled', 'disabled');
            $('input[name="Title"]').attr('disabled', 'disabled');
            $('.classified-display-ad-preview-container').html('');
        }
    })

    $('input[type="radio"][name="AdColumnInch"]').on('click', function () {   
        var adColumnInch = $(this).val();
        classifiedDisplayManager.getAdColumnInchRate(adColumnInch);
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
            return false;
        }
    });

    $("#AgencyAutoComplete").on('keyup', function () {
        var agency = $(this).val();
        if (!agency) {
            $("#AgencyId").val('');
        }
    })

     $('#AdvertiserContactNumber').on('focusout', function (event) {
        var mobileNoLenght = $(this).val().length;
        if (mobileNoLenght > 11) {
            app.Notify(AlertTypeConstants.Warning, "Warning, Contact No. must be 11 digits", AlertIconConstants.Warning);
        }
    })

    $("#AdvertiserContactNumber").ForceNumericOnly();

    $("#form-book-classified-display-ad").on("submit", function (event) {
        event.preventDefault();

        var _currentForm = $(this).closest('#form-book-classified-display-ad');
        if (!_currentForm.valid()) { console.log('form is not valid'); return; }

        var $btnMakePayment = $('.btn-make-payment');
        var btnPrevHtml = $btnMakePayment.html();
        $btnMakePayment.attr('disabled', 'disabled');
        $btnMakePayment.html(`<i class="fa fa-circle-o-notch fa-spin fa-fw"></i> Processing`);

        var url = $(this).attr("action");
        var form = $(this);
        var formData = new FormData(form[0]);

        var fileUpload = $("#ImageContent").get(0);
        var files = fileUpload.files;

        if (files.length > 0) {
            var file = files[0];
            formData.append("ImageContent", file);
        }

        $.ajax({
            url: url,
            type: "POST",
            data: formData,
            processData: false,
            contentType: false,
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