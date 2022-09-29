
var datesBasedOffer = [];
var removeUploadedFiles = [];

var bookGovtAdManager = {
    init: function () {
        //$("input[type=checkbox][name=IsColor]").attr('checked', 'checked');       
        $('.section-advitiser').addClass('d-none');
    },
    loadCategory: function () {
        var brandId = $("#BrandId").val()
        var ddlCategory = $("#CategoryId"); ddlCategory.html('');
        if (!brandId || brandId <= 0) {
            ddlCategory.append($('<option></option>').val('').html('Select Category'));
            return;
        }

        $.ajax({
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            url: '/BookGovtPrivateDisplayAd/GetCategoryList',
            data: { brandId: brandId },
            dataType: 'json',
            success: function (data) {
                $.each(data, function (id, option) {
                    ddlCategory.append($('<option></option>').val(option.value).html(option.text));
                });
                var selectedCategoryId = '';
                if (data && data.length > 0) selectedCategoryId = data[1].value;
                ddlCategory.val(selectedCategoryId);

            },
            error: function (request, status, error) {
                alert(request.statusText + "/" + request.statusText + "/" + error);
            }
        });
    },
    GetBrandWiseAdvirtiser: function () {
        var brandId = $("#BrandId").val();
        var $advertiserName = $("#AdvertiserName");
        var $advertiserId = $("#AdvertiserId");

        if (!brandId || brandId <= 0) {
            $advertiserName.val('');
            $advertiserId.val(0);
            return;
        }

        $.ajax({
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            url: '/BookPrivateDisplayAd/GetBrandWiseAdvirtiser',
            data: { brandId: brandId },
            dataType: 'json',
            success: function (data) {
                if (!data) {
                    $advertiserName.val('');
                    $advertiserId.val(0);

                    $("#AdvertiserName").autocomplete({ source: [] });
                    $("#AdvertiserName").autocomplete("destroy");
                    bookGovtAdManager.initAutoComForAdvertiser();

                    return;
                }
                $advertiserName.val(data.advertiserName);
                $advertiserId.val(data.advertiserId);

                $("input[type=checkbox][name=Personal]").prop('checked', false);
                $('#HiddenAdvertiserName').val(data.advertiserName);

                //personal actions
                //bookGovtAdManager.personalActions();
            },
            error: function (request, status, error) {
                alert(request.statusText + "/" + request.statusText + "/" + error);
            }
        });
    },
    getEstimatedCosting: function () {
        var columnSize = $('#ColumnSize').val();
        var inchSize = $('#InchSize').val();
        var isColor = $("#IsColor").is(':checked');
        var corporation = $("#Corporation").is(':checked');
        var editionPageId = $('#EditionPageId').val();

        columnSize = (columnSize === '' || columnSize === 0) ? 0 : columnSize;
        inchSize = (inchSize === '' || inchSize === 0) ? 0 : inchSize;

        $('.column-size').text(columnSize);
        $('.inch-size').text(inchSize);

        if (/*!columnSize || !inchSize ||*/ !editionPageId) {
            $('.esitmated-cost-amount').text(0);
            $('#EstimatedTotal').val(0);
            $('.rate-per-column-inch').text(0);

            return;
        }

        var formData = {
            ColumnSize: columnSize,
            InchSize: inchSize,
            IsColor: isColor,
            Corporation: corporation,
            EditionPageId: editionPageId
        };

        $.ajax({
            url: '/BookGovtPrivateDisplayAd/GetEstimatedCosting',
            type: "POST",
            data: formData,
            dataType: "json",
            success: function (data) {

                var costingInfo = data.estimatedCosting;

                if (costingInfo.isRateNotFound) {
                    $('.esitmated-cost-amount').text(0);
                    $('#EstimatedTotal').val(0);
                    $('.rate-per-column-inch').text(0.00);

                    app.Notify(AlertTypeConstants.Warning, "Warning! Rate not found. Please try another edition page.", AlertIconConstants.Warning);
                    return;
                }
                
                var userGroup = $('#UserGroup').val();

                var existigRate = $('.rate-per-column-inch').text();
                existigRate = Number(existigRate);
                if (userGroup === UserGroupConstants.Correspondent && costingInfo.isNationalEditionRate
                    && (!existigRate || existigRate === '' || existigRate <= 0 || existigRate !== costingInfo.govtAdRate)
                ) {
                    $.alert.open('confirm', 'Rate not found.\n Are You Sure to Apply National Edition Rate?',
                        function (button, value) {
                            if (button === 'yes') {
                                $('.esitmated-cost-amount').text(costingInfo.estimatedCosting);
                                $('#EstimatedTotal').val(costingInfo.estimatedCosting);                                          
                                $('.rate-per-column-inch').text(parseInt(costingInfo.govtAdRate));
                                
                            }
                            else {
                                $('.esitmated-cost-amount').text(0);
                                $('#EstimatedTotal').val(0);
                                $('.rate-per-column-inch').text(0);
                            }
                        }
                    );

                    return;
                }

                $('.esitmated-cost-amount').text(costingInfo.estimatedCosting);
                $('#EstimatedTotal').val(costingInfo.estimatedCosting);
                $('.rate-per-column-inch').text(parseInt(costingInfo.govtAdRate));
            },
            error: function (err) {
                var msg = `Warning, There was an error while trying to get estimated costing or check your internet connection.`;
                app.Notify(AlertTypeConstants.Warning, msg, AlertIconConstants.Warning);
            }
        })
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
            url: '/BookGovtPrivateDisplayAd/getdateoffer',
            type: "POST",
            data: formData,
            dataType: "json",
            success: function (data) {
                if (!data) {
                    $('.discount-offer').text(0);
                    return;
                }
                $('.discount-offer').text(data.discountPercentage);
            },
            error: function (err) {
                var msg = `Warning, There was an error while trying to get discount or check your internet connection.`;
                app.Notify(AlertTypeConstants.Warning, msg, AlertIconConstants.Warning);
            }
        })
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
    previewPhoto: function () {
        var files = $("#ImageContents").get(0).files;
        if (files) {
            _.forEachRight(files, function (file) {                       
                var reader = new FileReader();
                reader.onload = function () {
                    var type = file.type.split("/");
                    var scale = 'fa-4x';
                    var previewContent = '';
                    if (type[0] === FileTypeConstants.Image)
                        previewContent = `<img style="width: 65px;height: 70px;" src='${reader.result}' />`;
                    else
                        previewContent = app.fileIcon(file.type, scale);

                    var filePreview = `
                    <tr>
                        <td>
                            <span class="preview">
                                    ${previewContent}
                            </span>
                        </td>
                        <td>
                            <p class="name">${file.name}</p>
                            <strong class="error text-danger"></strong>
                        </td>
                        <td> 
                            <button type='button' file-name='${file.name}' class="btn btn-sm btn-danger cancel-file">
                                <i class="fa fa-times"></i> Cancel
                            </button>                  
                        </td>
                    </tr>
                    `;

                    $('.uploaded-files').append(filePreview);
                }
                reader.readAsDataURL(file);

            });            
        }

        $('.section-add-files').find('.add-icon').removeClass('fa fa-circle-o-notch fa-spin fa-fw').addClass('fa fa-plus');
    },
    removeUploadedFile: function (name) {
        var fileItem = {
            name: name
        }

        _.remove(removeUploadedFiles, function (item) {
            return item.name === name;
        });

        removeUploadedFiles.push(fileItem);
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

        /*
        $('.section-advitiser').removeClass('d-none');
        var isPersonal = $("#Personal").is(':checked');
        var advertiserName = $('#HiddenAdvertiserName').val();
        $('#AdvertiserContactNumber').val('');

        if (!isPersonal) {
            $('.section-advitiser').addClass('d-none');
            $('#AdvertiserName').val(advertiserName);
            return;
        }

        $('#AdvertiserName').val(''); 
        */
    },
    addDateOffer: function (dateBasedOffer) {
        var isExistInDatesBasedOffer = _.some(datesBasedOffer, function (item) { return item === dateBasedOffer; });
        if (isExistInDatesBasedOffer) { app.Notify(AlertTypeConstants.Warning, `Warning! Date already selected`, AlertIconConstants.Warning); return; }

        datesBasedOffer.push(dateBasedOffer);

        //generate base offer date container's html
        bookGovtAdManager.generateBaseOfferDateContainerHtml();

        //get discount by date offer          
        bookGovtAdManager.getDiscountByDateOffer();

        $('#DateBasedOffer').val('');
    },
    initAutoComForAdvertiser: function () {
        $("#AdvertiserName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '/BookPrivateDisplayAd/AdvertiserAutoComplete',
                    type: 'POST',
                    data: { 'searchKey': request.term },
                    dataType: 'json',
                    success: function (data) {
                        response($.map(data.data, function (item) {
                            return {
                                label: item.advertiserName,
                                value: item.advertiserId
                            };
                        }));
                    }
                });
            },
            minLength: 2,
            search: function (event, ui) {
                $(this).addClass('autocom-loading');
            },
            response: function (event, ui) {
                $(this).removeClass('autocom-loading');
            },
            focus: function (event, ui) {
                $("#AdvertiserName").val(ui.item.label);
                return false;
            },
            select: function (event, ui) {
                event.stopPropagation();
                $("#AdvertiserName").val(ui.item.label);
                $("#AdvertiserId").val(ui.item.value);

                return false;
            },
            change: function (event, ui) {
                if (!ui.item) {
                    //$("#AdvertiserName").val('');
                    $("#AdvertiserId").val('');
                    return;
                }
                $("#AdvertiserName").val(ui.item.label);
                $("#AdvertiserId").val(ui.item.value);
                return false;
            }
        });

        $("#AdvertiserName").on('keyup', function () {
            var advertiser = $(this).val();
            if (!advertiser) {
                $("#AdvertiserId").val('');
            }
        })
    },
}

$(function () {
    bookGovtAdManager.init();

    $('#ColumnSize').on('change', function () {
        //get estimated costing
        bookGovtAdManager.getEstimatedCosting();
    })

    $('#InchSize').on('blur', function () {
        //get estimated costing
        bookGovtAdManager.getEstimatedCosting();
    })

    $('#IsColor').on('click', function () {
        //get estimated costing
        bookGovtAdManager.getEstimatedCosting();
    })

    $('#Corporation').on('click', function () {
        //get estimated costing
        bookGovtAdManager.getEstimatedCosting();
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
            bookGovtAdManager.addDateOffer(dateBasedOffer);
            return;
        }

         var todayFormattedDateWithTime = moment(`${todayFormattedDate} 16:00:00`, "DD-MMM-YYYY HH:mm:ss").toDate(); 
        if (!(todayFormattedDateWithTime < todyDate)) {
            bookGovtAdManager.addDateOffer(dateBasedOffer);
            return;
        }

        $.alert.open('confirm', 'Are you sure to add this date offer after 04:00 PM',
            function (button, value) {
                if (button === 'yes') {
                    bookGovtAdManager.addDateOffer(dateBasedOffer);
                }
            }
        );
    });

    $('.date-based-offer-container').on('click', '.btn-remove-base-offer-date', function () {
        var baseOfferDate = $(this).attr('base-offer-date');

        _.remove(datesBasedOffer, function (date) { return date === baseOfferDate; });

        //generate base offer date container's html
        bookGovtAdManager.generateBaseOfferDateContainerHtml();

        //get discount by date offer          
        bookGovtAdManager.getDiscountByDateOffer();

        $(this).remove();
    });

    $('#Personal').on('click', function () {
        //personal actions
        bookGovtAdManager.personalActions(); 
    })

    $("#BrandAutoComplete").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/BookGovtPrivateDisplayAd/brandautocomplete',
                type: 'POST',
                data: { 'searchKey': request.term },
                dataType: 'json',
                success: function (data) {
                    response($.map(data.data, function (item) {
                        return {
                            label: item.brandName,
                            value: item.brandId
                        };
                    }));
                }
            });
        },
        minLength: 2,
        search: function (event, ui) {
            $(this).addClass('autocom-loading');
        },
        response: function (event, ui) {
            $(this).removeClass('autocom-loading');
        },
        focus: function (event, ui) {
            $("#BrandAutoComplete").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            event.stopPropagation();
            $("#BrandAutoComplete").val(ui.item.label);
            $("#BrandId").val(ui.item.value);

            //get categories for dropdown
            bookGovtAdManager.loadCategory();

            //get brand wise advirtiser
            bookGovtAdManager.GetBrandWiseAdvirtiser();

            return false;
        },
        change: function (event, ui) {
            if (!ui.item) {
                $("#BrandAutoComplete").val('');
                $("#BrandId").val('');

                //get categories for dropdown
                bookGovtAdManager.loadCategory();
                return;
            }
            $("#BrandAutoComplete").val(ui.item.label);
            $("#BrandId").val(ui.item.value);

            //get categories for dropdown
            bookGovtAdManager.loadCategory();
            return false;
        }
    });

    $("#BrandAutoComplete").on('keyup', function () {
        var brand = $(this).val();
        if (!brand) {
            $("#BrandId").val('');
        }
    })

    $("#AgencyAutoComplete").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/BookGovtPrivateDisplayAd/agencyautocomplete',
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
            $(this).addClass('autocom-loading');
        },
        response: function (event, ui) {
            $(this).removeClass('autocom-loading');
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

    $("#AdvertiserName").on('keypress', function () {
        var brand = $('#BrandId').val();
        if (brand === `${BrandsConstants.Others}`) {
            $("#AdvertiserName").autocomplete({ source: [] });
            $("#AdvertiserName").autocomplete("destroy");
            bookGovtAdManager.initAutoComForAdvertiser();
        }
    })

     $('#AdvertiserContactNumber').on('focusout', function (event) {
        var mobileNoLenght = $(this).val().length;
        if (mobileNoLenght > 11) app.Notify(AlertTypeConstants.Warning, "Warning, Contact No. must be 11 digits", AlertIconConstants.Warning);
    })
    
    $("#AdvertiserContactNumber").ForceNumericOnly();

    $('.tbl-uploded-files').on('click', '.cancel-file', function () {
        var fileName = $(this).attr('file-name');
        //var fileSize = $(this).attr('file-size');
        $(this).closest('tr').remove();

        //remove uploaded file
        bookGovtAdManager.removeUploadedFile(fileName);
    });

    $('#EditionPageId').on('change', function () {
        //get estimated costing
        bookGovtAdManager.getEstimatedCosting();
    })

    $('#ImageContents').on('click', function () {
        setTimeout(function () {
            $('.section-add-files').find('.add-icon').removeClass('fa fa-plus').addClass('fa fa-circle-o-notch fa-spin fa-fw');
        }, 100);
        
    })

    $('#UploadLater').on('click', function () {
        var uploadLater = $("#UploadLater").is(':checked');
        $('.section-add-files').removeClass('disable-upload-files');
        $('#ImageContents').removeAttr('disabled');
        $('#ImageContents').val();
        $('.uploaded-files').html('');
        removeUploadedFiles = [];       
        if (uploadLater) {
            $('.section-add-files').addClass('disable-upload-files');
            $('#ImageContents').attr('disabled','disabled');
        }        
    })

    $("#form-book-govt-ad").on("submit", function (event) {
        event.preventDefault();

        var _currentForm = $(this).closest('#form-book-govt-ad');
        if (!_currentForm.valid()) { console.log('form is not valid'); return; }

        var $btnMakePayment = $('.btn-make-payment');
        var btnPrevHtml = $btnMakePayment.html();
        $btnMakePayment.attr('disabled', 'disabled');
        $btnMakePayment.html(`<i class="fa fa-circle-o-notch fa-spin fa-fw"></i> Processing`);

        var url = $(this).attr("action");
        var form = $(this);
        var formData = new FormData(form[0]);

        var isColor = $('#IsColor:checked').val();
        formData.append('IsColor', isColor);

        if (removeUploadedFiles && removeUploadedFiles.length > 0) {
            var removeFiles = '';
            var separator = "@@AdPro@@"
            _.forEachRight(removeUploadedFiles, function (item) {
                removeFiles = removeFiles + `${item.name}${separator}`;
            });

            formData.append("RemoveUploadedFiles", removeFiles);
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