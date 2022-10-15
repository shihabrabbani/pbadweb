
var removeUploadedFiles = [];

var uploadManager = {
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
                            <p class="size">${file.size} kb</p>
                            <div class="progress progress-striped active" role="progressbar" aria-valuemin="0" aria-valuemax="100" aria-valuenow="0"><div class="progress-bar progress-bar-success" style="width:0%;"></div></div>
                        </td>
                        <td> 
                            <button type='button' file-name='${file.name}' file-size='${file.size}' class="btn btn-sm btn-danger cancel-file">
                                <i class="fa fa-times"></i> <span>Cancel</span>
                            </button>                  
                        </td>
                    </tr>
                    `;

                    $('.uploaded-files').append(filePreview);
                }

                reader.readAsDataURL(file);

            });
        }
    },
    removeUploadedFile: function (name, size) {
        var fileItem = {
            name: name,
            size: size
        }

        _.remove(removeUploadedFiles, function (item) {
            return item.name === name && item.size === size;
        });

        removeUploadedFiles.push(fileItem);
    }
}

$(function () {

    $('.tbl-uploded-files').on('click', '.cancel-file', function () {
        var fileName = $(this).attr('file-name');
        var fileSize = $(this).attr('file-size');
        $(this).closest('tr').remove();

        uploadManager.removeUploadedFile(fileName, fileSize);
    });

    $("#fileupload").on("submit", function (event) {
        event.preventDefault();

        var url = $(this).attr("action");
        var form = $(this);
        var formData = new FormData();
        
        if (removeUploadedFiles && removeUploadedFiles.length > 0) { 
            var removeFiles = '';
            var separator="@@AdPro@@"
            _.forEachRight(removeUploadedFiles, function (item) {
                removeFiles = removeFiles + `${item.name}${separator}`;
            });

            formData.append("RemoveUploadedFiles", removeFiles);
        } 
        
        formData.append("Username", 'Junainah Yusra Roha');

        $.ajax({
            url: url,
            type: "POST",
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                console.log('Uploaded!')
            },
            error: function (err) {
                var msg = `Warning, There was an error while trying to make payment or check your internet connection.`;
                app.Notify(AlertTypeConstants.Warning, msg, AlertIconConstants.Warning);
            }
        })
    });
});
