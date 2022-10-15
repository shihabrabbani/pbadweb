

var gdDetailList = [];
var removeUploadedFiles = [];
var removeExistingFiles = [];

var bookDigitalDisplayAdEditManager = {
    init: function () {
        $('.section-advitiser').addClass('d-none');

        var categoryId = $('#SelectedCategoryId').val();
        //load category
        this.loadCategory(categoryId);

        //toggle upload later
        this.toggleUploadLater();

        //get digital display listings
        this.getDigitalDisplayListings();
    },
    loadSubCategoryByCategory: function (categoryId) {
        var ddlSubCategory = $("#SubCategoryId");
        $.ajax({
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            url: '/BookDigitalDisplayAd/GetSubCategoryList',
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
    loadPagePositionByPage: function (pageId) {
        var ddlDigitalPagePosition = $("#DigitalPagePositionId");
        $.ajax({
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            url: '/BookDigitalDisplayAd/GetPagePositionByPage',
            data: { pageId: pageId },
            dataType: 'json',
            success: function (data) {
                ddlDigitalPagePosition.html('');
                $.each(data, function (id, option) {
                    ddlDigitalPagePosition.append($('<option></option>').val(option.value).html(option.text));
                });
            },
            error: function (request, status, error) {
                alert(request.statusText + "/" + request.statusText + "/" + error);
            }
        });
    },
    getEstimatedCosting: function () {

        //validate add new digital detail
        var isValidToAddNew = bookDigitalDisplayAdEditManager.validateNewDetails();
        if (!isValidToAddNew) return;

        var digitalAdUnitTypeId = $("#DigitalAdUnitTypeId").val();
        var digitalPageId = $("#DigitalPageId").val();
        var digitalPagePositionId = $("#DigitalPagePositionId").val();
        var adQty = $("#AdQty").val();
        var estimatedCosting = 0;

        if (!(digitalAdUnitTypeId > 0 && digitalPageId > 0 && digitalPagePositionId > 0)) {           
            bookDigitalDisplayAdEditManager.populateDigitalDisplayDetail(0);
            return;
        }

        $.ajax({
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            url: '/BookDigitalDisplayAd/GetRateForDigitalDisplay',
            data: {
                digitalAdUnitTypeId: digitalAdUnitTypeId,
                digitalPageId: digitalPageId,
                digitalPagePositionId: digitalPagePositionId
            },
            dataType: 'json',
            success: function (data) {
                if (!data) {                   
                    bookDigitalDisplayAdEditManager.populateDigitalDisplayDetail(0);
                    return;
                }
                var perUnitRate = data.perUnitRate;
                adQty = adQty ? adQty : 0;
                estimatedCosting = parseFloat(perUnitRate) * parseFloat(adQty)

                bookDigitalDisplayAdEditManager.populateDigitalDisplayDetail(parseInt(estimatedCosting));                
            },
            error: function (request, status, error) {
                //return 0;
                alert(request.statusText + "/" + request.statusText + "/" + error);
            }
        });
    },
    populateDigitalDisplayDetail: function (perUnitRate) {
        if (perUnitRate <= 0) {
            app.Notify(AlertTypeConstants.Warning, "Rate not found. Please try another", AlertIconConstants.Warning); return;
        }    

        var pageId = $('#DigitalPageId').val();
        var pageName = $('#DigitalPageId option:selected').text();

        var positionId = $('#DigitalPagePositionId').val();
        var positionName = $('#DigitalPagePositionId option:selected').text();

        var unitId = $('#DigitalAdUnitTypeId').val();
        var unitName = $('#DigitalAdUnitTypeId option:selected').text();
        var qty = $('#AdQty').val();

        var publishDateStart = $('#PublishDateStart').val();
        var publishTimeStart = $('#PublishTimeStart').val();

        var publishDateEnd = $('#PublishDateEnd').val();
        var publishTimeEnd = $('#PublishTimeEnd').val();


        var isExistGdDetailInList = _.some(gdDetailList, function (item) {
            return item.DigitalPagePositionId === positionId && item.DigitalAdUnitTypeId == unitId && item.DigitalPageId == pageId;
        });

        if (isExistGdDetailInList)
            return;

        var gdDetailMax = _.maxBy(gdDetailList, 'SerialNo');
        newSerialNo = (jQuery.type(gdDetailMax) === "undefined" || jQuery.type(gdDetailMax.SerialNo) === "undefined") ? 0 : gdDetailMax.SerialNo;

        var gdDetail = {
            SerialNo: newSerialNo + 1,

            DigitalPageId: pageId,
            DigitalPageName: pageName,

            DigitalPagePositionId: positionId,
            PagePositionName: positionName,

            DigitalAdUnitTypeId: unitId,
            UnitName: unitName,
            AdQty: qty,
            PerUnitRate: perUnitRate,

            PublishDateStart: publishDateStart,
            PublishTimeStart: publishTimeStart,

            PublishDateEnd: publishDateEnd,
            PublishTimeEnd: publishTimeEnd
        };

        gdDetailList.push(gdDetail);

        //get estimated cost
        var estimatedCosting = _.sumBy(gdDetailList, function (item) { return item.PerUnitRate; });
        $('.esitmated-cost-amount').text(estimatedCosting);

        var gdDetailHtml = '';
        $('#tbleDigitalDisplayDetail>tbody').html('');
        _.forEach(gdDetailList, function (item) {
            gdDetailHtml += `
                <tr>
                    <td>                        
                        <input type='hidden' name='DigitalDisplayDetailList.Index' id='DigitalDisplayDetailList_Index' value='${item.SerialNo}' />
                        <input type='hidden' name='DigitalDisplayDetailList[${item.SerialNo}].DigitalPageId' id='DigitalDisplayDetailList[${item.SerialNo}]_DigitalPageId' value='${item.DigitalPageId}' />
                        ${item.DigitalPageName}
                    </td>
                    <td>                       
                        <input type='hidden' name='DigitalDisplayDetailList[${item.SerialNo}].DigitalPagePositionId' id='DigitalDisplayDetailList[${item.SerialNo}]_DigitalPagePositionId' value='${item.DigitalPagePositionId}' />
                        ${item.PagePositionName}
                    </td>
                    <td>                        
                        <input type='hidden' name='DigitalDisplayDetailList[${item.SerialNo}].DigitalAdUnitTypeId' id='DigitalDisplayDetailList[${item.SerialNo}]_DigitalAdUnitTypeId' value='${item.DigitalAdUnitTypeId}' />
                        ${item.UnitName}
                    </td>
                    <td>
                        <input type='hidden' name='DigitalDisplayDetailList[${item.SerialNo}].AdQty' id='DigitalDisplayDetailList[${item.SerialNo}]_AdQty' value='${item.AdQty}' />
                        ${item.AdQty}
                    </td>
                    <td>
                        <input type='hidden' name='DigitalDisplayDetailList[${item.SerialNo}].PerUnitRate' id='DigitalDisplayDetailList[${item.SerialNo}]_PerUnitRate' value='${item.PerUnitRate}' />
                        ${item.PerUnitRate}
                    </td>
                    <td>
                        <input type='hidden' name='DigitalDisplayDetailList[${item.SerialNo}].PublishDateStart' id='DigitalDisplayDetailList[${item.SerialNo}]_PublishDateStart' value='${item.PublishDateStart}' />
                        <input type='hidden' name='DigitalDisplayDetailList[${item.SerialNo}].PublishTimeStart' id='DigitalDisplayDetailList[${item.SerialNo}]_PublishTimeStart' value='${item.PublishTimeStart}' />
                        <input type='hidden' name='DigitalDisplayDetailList[${item.SerialNo}].PublishDateEnd' id='DigitalDisplayDetailList[${item.SerialNo}]_PublishDateEnd' value='${item.PublishDateEnd}' />
                        <input type='hidden' name='DigitalDisplayDetailList[${item.SerialNo}].PublishTimeEnd' id='DigitalDisplayDetailList[${item.SerialNo}]_PublishTimeEnd' value='${item.PublishTimeEnd}' />

                        ${item.PublishDateStart} ${item.PublishTimeStart} - ${item.PublishDateEnd} ${item.PublishTimeEnd}
                    </td>
                    <td>
                        <a href='JavaScript:void(0);' class='btn btn-danger btn-remove-gd-detail' serialNo="${item.SerialNo}"><i class="fa fa-trash"></i></a>
                    </td>
                </tr>
                `;
        });

        $('#tbleDigitalDisplayDetail>tbody').append(gdDetailHtml);
    },
    removeGdDetail: function (serialNo) {
        _.remove(gdDetailList, function (gdDetail) {
            return gdDetail.SerialNo === parseInt(serialNo);
        });        
    },
    loadCategory: function (categorId = 0) {
        var brandId = $("#BrandId").val()
        var ddlCategory = $("#CategoryId"); ddlCategory.html('');
        if (!brandId || brandId <= 0) {
            ddlCategory.html('').append($('<option></option>').val('').html('Select Category'));
            return;
        }

        $.ajax({
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            url: '/BookDigitalDisplayAd/GetCategoryList',
            data: { brandId: brandId },
            dataType: 'json',
            success: function (data) {
                var selectedCategoryId = '';
                if (data && data.length > 0) selectedCategoryId = data[1].value;
                $.each(data, function (id, option) {
                    ddlCategory.append($('<option></option>').val(option.value).html(option.text));
                });

                if (categorId > 0) {
                    ddlCategory.val(categorId);
                }
                else {
                    ddlCategory.val(selectedCategoryId);
                }
            },
            error: function (request, status, error) {
                alert(request.statusText + "/" + request.statusText + "/" + error);
            }
        });
    },
    GetBrandWiseAdvirtiser: function () {
        var brandId = $("#BrandId").val()

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
            url: '/BookDigitalDisplayAd/GetBrandWiseAdvirtiser',
            data: { brandId: brandId },
            dataType: 'json',
            success: function (data) {
                if (!data) {

                    $("#AdvertiserName").autocomplete({ source: [] });
                    $("#AdvertiserName").autocomplete("destroy");
                    bookDigitalDisplayAdEditManager.initAutoComForAdvertiser();

                    $advertiserName.val($('#hiddenAdvertiserName').val());
                    $advertiserId.val($('#hiddenAdvertiserContactNumber').val());

                    return;
                }
                $advertiserName.val(data.advertiserName);
                $advertiserId.val(data.advertiserId);

                $('#HiddenAdvertiserName').val(data.advertiserName);
            },
            error: function (request, status, error) {
                alert(request.statusText + "/" + request.statusText + "/" + error);
            }
        });
    },
    previewPhoto: function () {
        var files = $("#ImageContents").get(0).files;
        if (files) {
            removeUploadedFiles = [];
            $('.uploaded-files').html('');

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
                            <p class="size">${file.size} kb</p>
                            <div class="progress progress-striped active" role="progressbar" aria-valuemin="0" aria-valuemax="100" aria-valuenow="0"><div class="progress-bar progress-bar-success" style="width:0%;"></div></div>
                        </td>
                        <td> 
                            <button type='button' file-name='${file.name}' file-size='${file.size}' class="btn btn-sm btn-danger cancel-file">
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
            return item.name === name
        });

        removeUploadedFiles.push(fileItem);
    },
    getDigitalDisplayListings: function () {
        var digitalDisplayId = $("#ABDigitalDisplayId").val();
        if (digitalDisplayId <= 0) {
            $('#tbleDigitalDisplayDetail>tbody').html(''); return;
        }
        $.ajax({
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            url: '/bookdigitaldisplayad/getdigitaldisplaylisting',
            data: { digitalDisplayId: digitalDisplayId },
            dataType: 'json',
            success: function (response) {
                $('#tbleDigitalDisplayDetail>tbody').html('');

                $.each(response.data, function (id, item) {                    
                    var gdDetail = {
                        SerialNo: id + 1,

                        DigitalPageId: item.digitalPageId,
                        DigitalPageName: item.digitalPage.digitalPageName,

                        DigitalPagePositionId: item.digitalPagePositionId,
                        PagePositionName: item.digitalPagePosition.digitalPagePositionName,

                        DigitalAdUnitTypeId: item.digitalAdUnitTypeId,
                        UnitName: item.digitalAdUnitType.digitalAdUnitTypeName,

                        AdQty: item.adQty,
                        PerUnitRate: item.perUnitRate,

                        PublishDateStart: item.publishDateStartInText,
                        PublishTimeStart: item.publishTimeStartInText,

                        PublishDateEnd: item.publishDateEndInText,
                        PublishTimeEnd: item.publishTimeEndInText
                    };

                    gdDetailList.push(gdDetail);
                });

                var gdDetailHtml = '';
                _.forEach(gdDetailList, function (item) {
                    gdDetailHtml += `
                <tr>
                    <td>                        
                        <input type='hidden' name='DigitalDisplayDetailList.Index' id='DigitalDisplayDetailList_Index' value='${item.SerialNo}' />
                        <input type='hidden' name='DigitalDisplayDetailList[${item.SerialNo}].DigitalPageId' id='DigitalDisplayDetailList[${item.SerialNo}]_DigitalPageId' value='${item.DigitalPageId}' />
                        ${item.DigitalPageName}
                    </td>
                    <td>                       
                        <input type='hidden' name='DigitalDisplayDetailList[${item.SerialNo}].DigitalPagePositionId' id='DigitalDisplayDetailList[${item.SerialNo}]_DigitalPagePositionId' value='${item.DigitalPagePositionId}' />
                        ${item.PagePositionName}
                    </td>
                    <td>                        
                        <input type='hidden' name='DigitalDisplayDetailList[${item.SerialNo}].DigitalAdUnitTypeId' id='DigitalDisplayDetailList[${item.SerialNo}]_DigitalAdUnitTypeId' value='${item.DigitalAdUnitTypeId}' />
                        ${item.UnitName}
                    </td>
                    <td>
                        <input type='hidden' name='DigitalDisplayDetailList[${item.SerialNo}].AdQty' id='DigitalDisplayDetailList[${item.SerialNo}]_AdQty' value='${item.AdQty}' />
                        ${item.AdQty}
                    </td>
                    <td>
                        <input type='hidden' name='DigitalDisplayDetailList[${item.SerialNo}].PerUnitRate' id='DigitalDisplayDetailList[${item.SerialNo}]_PerUnitRate' value='${item.PerUnitRate}' />
                        ${item.PerUnitRate}
                    </td>
                    <td>
                        <input type='hidden' name='DigitalDisplayDetailList[${item.SerialNo}].PublishDateStart' id='DigitalDisplayDetailList[${item.SerialNo}]_PublishDateStart' value='${item.PublishDateStart}' />
                        <input type='hidden' name='DigitalDisplayDetailList[${item.SerialNo}].PublishTimeStart' id='DigitalDisplayDetailList[${item.SerialNo}]_PublishTimeStart' value='${item.PublishTimeStart}' />
                        <input type='hidden' name='DigitalDisplayDetailList[${item.SerialNo}].PublishDateEnd' id='DigitalDisplayDetailList[${item.SerialNo}]_PublishDateEnd' value='${item.PublishDateEnd}' />
                        <input type='hidden' name='DigitalDisplayDetailList[${item.SerialNo}].PublishTimeEnd' id='DigitalDisplayDetailList[${item.SerialNo}]_PublishTimeEnd' value='${item.PublishTimeEnd}' />

                        ${item.PublishDateStart} ${item.PublishTimeStart} - ${item.PublishDateEnd} ${item.PublishTimeEnd}
                    </td>
                    <td>
                        <a href='JavaScript:void(0);' class='btn btn-danger btn-remove-gd-detail' serialNo="${item.SerialNo}"><i class="fa fa-trash"></i></a>
                    </td>
                </tr>
                `;
                });

                $('#tbleDigitalDisplayDetail>tbody').append(gdDetailHtml);

                var estimatedCosting = _.sumBy(gdDetailList, function (item) { return item.PerUnitRate; });

                $('.esitmated-cost-amount').text(estimatedCosting);
            },
            error: function (request, status, error) {
                alert(request.statusText + "/" + request.statusText + "/" + error);
            }
        });
    },
    toggleUploadLater: function () {
        var uploadLater = $("#UploadLater").is(':checked');
        $('.section-add-files').removeClass('disable-upload-files');
        $('#ImageContents').removeAttr('disabled');
        $('#ImageContents').val();
        $('.uploaded-files').html('');
        removeUploadedFiles = [];
        if (uploadLater) {
            $('.section-add-files').addClass('disable-upload-files');
            $('#ImageContents').attr('disabled', 'disabled');
        }
    },
    validateNewDetails: function () {
        var pageId = $('#DigitalPageId').val();
        var positionId = $('#DigitalPagePositionId').val();
        var unitId = $('#DigitalAdUnitTypeId').val();
        var qty = $('#AdQty').val();

        var publishDateStart = $('#PublishDateStart').val();
        var publishTimeStart = $('#PublishTimeStart').val();

        var publishDateEnd = $('#PublishDateEnd').val();
        var publishTimeEnd = $('#PublishTimeEnd').val();
        if (!pageId || pageId <= 0) {
            app.Notify(AlertTypeConstants.Warning, "Select Page", AlertIconConstants.Warning);
            return false;
        }
        else if (!positionId || positionId <= 0) {
            app.Notify(AlertTypeConstants.Warning, "Select Page Position", AlertIconConstants.Warning);
            return false;
        }
        else if (!unitId || unitId <= 0) {
            app.Notify(AlertTypeConstants.Warning, "Select Unit Type", AlertIconConstants.Warning);
            return false;
        }
        else if (!qty || qty <= 0) {
            app.Notify(AlertTypeConstants.Warning, "Qty is Required", AlertIconConstants.Warning);
            return false;
        }
        else if (!publishDateStart || publishDateStart === '') {
            app.Notify(AlertTypeConstants.Warning, "Publish Start Date is Required", AlertIconConstants.Warning);
            return false;
        }
        else if (!publishTimeStart || publishTimeStart === '') {
            app.Notify(AlertTypeConstants.Warning, "Publish Start Time is Required", AlertIconConstants.Warning);
            return false;
        }
        else if (!publishDateEnd || publishDateEnd === '') {
            app.Notify(AlertTypeConstants.Warning, "Publish End Date is Required", AlertIconConstants.Warning);
            return false;
        }
        else if (!publishTimeEnd || publishTimeEnd === '') {
            app.Notify(AlertTypeConstants.Warning, "Publish End Time is Required", AlertIconConstants.Warning);
            return false;
        }

        return true;

    },
    removExistingFile: function (filepath) {
        var fileItem = {
            filepath: filepath
        }

        _.remove(removeExistingFiles, function (item) {
            return item.filepath === filepath;
        });

        removeExistingFiles.push(fileItem);
    },
}

$(function () {
    bookDigitalDisplayAdEditManager.init();

    $('.btn-gd-detail').on('click', function () {
        bookDigitalDisplayAdEditManager.getEstimatedCosting();
    })

    $('#tbleDigitalDisplayDetail').on('click', '.btn-remove-gd-detail', function () {
        var serialNo = $(this).attr('serialNo');
        //remove gd detail
        bookDigitalDisplayAdEditManager.removeGdDetail(serialNo);
        $(this).parents('tr').remove();

        //get estimated cost       
        var estimatedCosting = _.sumBy(gdDetailList, function (item) { return item.PerUnitRate; });
        $('.esitmated-cost-amount').text(estimatedCosting);            
    })

    $('#CategoryId').on('change', function () {
        var categoryId = $(this).val();

        if (!categoryId || categoryId <= 0) {
            var ddlSubCategory = $("#SubCategoryId");
            ddlSubCategory.append($('<option></option>').val('').html('Select Sub Category'));
            return;
        }

        bookDigitalDisplayAdEditManager.loadSubCategoryByCategory(categoryId);
    })

    $('#DigitalPageId').on('change', function () {
        var digitalPageId = $(this).val();

        if (!digitalPageId || digitalPageId <= 0) {
            var ddlDigitalPagePosition = $("#DigitalPagePositionId");
            ddlDigitalPagePosition.append($('<option></option>').val('').html('Select Page Position'));
            return;
        }

        bookDigitalDisplayAdEditManager.loadPagePositionByPage(digitalPageId);
    })

    $("#BrandAutoComplete").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/BookDigitalDisplayAd/brandautocomplete',
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
            bookDigitalDisplayAdEditManager.loadCategory();

            //get brand wise advirtiser
            bookDigitalDisplayAdEditManager.GetBrandWiseAdvirtiser();

            return false;
        },
        change: function (event, ui) {
            if (!ui.item) {
                $("#BrandAutoComplete").val('');
                $("#BrandId").val('');

                //get categories for dropdown
                bookDigitalDisplayAdEditManager.loadCategory();
                return;
            }
            $("#BrandAutoComplete").val(ui.item.label);
            $("#BrandId").val(ui.item.value);

            //get categories for dropdown
            bookDigitalDisplayAdEditManager.loadCategory();
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
                url: '/BookDigitalDisplayAd/agencyautocomplete',
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

    $('.tbl-uploded-files').on('click', '.cancel-file', function () {
        var fileName = $(this).attr('file-name');
        var fileSize = $(this).attr('file-size');
        $(this).closest('tr').remove();

        //remove uploaded file
        bookDigitalDisplayAdEditManager.removeUploadedFile(fileName);
    });

    $('.tbl-existing-files').on('click', '.cancel-file', function () {
        var filePath = $(this).attr('file-path');
        $(this).closest('tr').remove();

        //remove uploaded file
        bookDigitalDisplayAdEditManager.removExistingFile(filePath);
    });

    $("#AgencyAutoComplete").on('keyup', function () {
        var agency = $(this).val();
        if (!agency) {
            $("#AgencyId").val('');
        }
    })

    $('#AdvertiserContactNumber').on('focusout', function (event) {
        var mobileNoLenght = $(this).val().length;
        if (mobileNoLenght > 11) app.Notify(AlertTypeConstants.Warning, "Warning, Contact No. must be 11 digits", AlertIconConstants.Warning);
    })

    $("#AdvertiserContactNumber").ForceNumericOnly();

    $('.tbl-uploded-files').on('click', '.cancel-file', function () {
        var fileName = $(this).attr('file-name');
        $(this).closest('tr').remove();

        //remove uploaded file
        bookDigitalDisplayAdEditManager.removeUploadedFile(fileName);
    });

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
            $('#ImageContents').attr('disabled', 'disabled');
        }
    })

    $("#form-book-digital-display-ad").on("submit", function (event) {
        event.preventDefault();

        var _currentForm = $(this).closest('#form-book-digital-display-ad');
        if (!_currentForm.valid()) { console.log('form is not valid'); return; }

        var $btnMakePayment = $('.btn-make-payment');
        var btnPrevHtml = $btnMakePayment.html();
        $btnMakePayment.attr('disabled', 'disabled');
        $btnMakePayment.html(`<i class="fa fa-circle-o-notch fa-spin fa-fw"></i> Processing`);

        var url = $(this).attr("action");
        var form = $(this);
        var formData = new FormData(form[0]);

        if (removeUploadedFiles && removeUploadedFiles.length > 0) {
            var removeFiles = '';
            var separator = "@@AdPro@@"
            _.forEachRight(removeUploadedFiles, function (item) {
                removeFiles = removeFiles + `${item.name}${separator}`;
            });

            formData.append("RemoveUploadedFiles", removeFiles);
        }

        if (removeExistingFiles && removeExistingFiles.length > 0) {
            var removeExistingFilePaths = '';
            var separatorExisting = "@@AdPro@@"
            _.forEachRight(removeExistingFiles, function (item) {
                removeExistingFilePaths = removeExistingFilePaths + `${item.filepath}${separatorExisting}`;
            });

            formData.append("RemoveExistingFiles", removeExistingFilePaths);
        }

        var existingFiles = [];
        $('.tbl-existing-files tbody tr').each(function () {
            var fileName = $(this).find(".existing-file-name").attr('file-path');
            existingFiles.push(fileName);
        });

        formData.append("IsFoundExistingFiles", existingFiles.length > 0 ? true : false);

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