
var removeUploadedFiles = [];

var uploadLaterManager = {
    init: function () {

        //get upload later orders by filter
        if ($('#loadUploadLaterOrders').length > 0) this.getUploadLaterOrdersByFilter();

        if ($('.btn-upload').length > 0)
            $('.btn-upload').addClass('d-none');
    },

    getUploadLaterOrdersByFilter: function () {
        var $btnSearch = $('.btn-search-upload-later');
        var btnSearchPrevHtml = $btnSearch.html();
        $btnSearch.attr('disabled', 'disabled');
        $btnSearch.html(`<i class="fa fa-circle-o-notch fa-spin fa-fw"></i> Searching`);

        var searchTerm = $('#SearchTermUploadLaterOrder').val();

        var formData = {
            searchTerm: searchTerm
        };

        $('#loadUploadLaterOrders').html('<img src="/img/loading.gif" />');

        $.ajax({
            url: '/MyProfile/GetUploadLatersByFilter',
            type: "POST",
            data: formData,
            //dataType: "json",
            success: function (response) {
                $('#loadUploadLaterOrders').html(response);

                $btnSearch.removeAttr('disabled');
                $btnSearch.html(btnSearchPrevHtml);
            },
            error: function (err) {
                var msg = `Warning, There was an error while trying to get orders or check your internet connection.`;
                app.Notify(AlertTypeConstants.Warning, msg, AlertIconConstants.Warning);
            }
        })
    },

    previewPhoto: function () {
        var files = $("#ImageContents").get(0).files;
        if (files) {

            $('.btn-upload').removeClass('d-none');
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
        var fileItem = { name: name }

        _.remove(removeUploadedFiles, function (item) {
            return item.name === name
        });

        removeUploadedFiles.push(fileItem);
    },
}

$(function () {
    uploadLaterManager.init();   

    $('#ImageContents').on('change', function () {
        uploadLaterManager.previewPhoto();
    })

    $('.tbl-uploded-files').on('click', '.cancel-file', function () {
        var fileName = $(this).attr('file-name');

        $(this).closest('tr').remove();

        $('.btn-upload').addClass('d-none');

        if ($('.tbl-uploded-files .uploaded-files>tr').length > 0) { $('.btn-upload').removeClass('d-none'); }

        //remove uploaded file
        uploadLaterManager.removeUploadedFile(fileName);
    });

    $("#form-upload-later").on("submit", function (event) {
        event.preventDefault();

        var _currentForm = $(this).closest('#form-upload-later');
        if (!_currentForm.valid()) { console.log('form is not valid'); return; }

        var $btnUpload = $('.btn-upload');
        var btnPrevHtml = $btnUpload.html();
        $btnUpload.attr('disabled', 'disabled');
        $btnUpload.html(`<i class="fa fa-circle-o-notch fa-spin fa-fw"></i> Processing`);

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

        $.ajax({
            url: url,
            type: "POST",
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                $btnUpload.removeAttr('disabled');
                $btnUpload.html(btnPrevHtml);

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
