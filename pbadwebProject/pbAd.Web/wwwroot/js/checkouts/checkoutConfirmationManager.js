
var checkoutConfirmationManager = {
    init: function () {
        //prevent to go back in browser
        app.preventBackPageInBrowser();
    },
    printBookingConfirmation: function () {
        $(".print-section").printThis({
            header: '',
            footer: '',
            importCSS: true,
            //loadCSS: "/Assets/css/digitalIDCard.css",
        });
    }
}


$(document).ready(function () {
    checkoutConfirmationManager.init();

    $('#btnPrint').on('click', function () {
        checkoutConfirmationManager.printBookingConfirmation();
    });    
});

