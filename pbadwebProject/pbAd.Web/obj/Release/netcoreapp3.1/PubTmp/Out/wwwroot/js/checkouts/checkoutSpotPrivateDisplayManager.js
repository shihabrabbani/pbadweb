
var editionList = [];

var checkoutSpotPrivateDisplayManager = {
   
}

$(function () {
    $('.chk-edition').on('click', function () {
        var isChecked = this.checked;

        var editionName = $(this).attr('edition-text');
        _.remove(editionList, function (item) { return item === editionName; });

        if (isChecked) {            
            editionList.push(editionName);
        }

        var editionsInString = editionList.join();
        $('.selected-editions').html(editionsInString);
    });

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
