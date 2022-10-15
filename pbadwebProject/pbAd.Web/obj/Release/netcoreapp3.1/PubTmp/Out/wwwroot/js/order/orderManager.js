
var orderManager = {
    init: function () {
        //get orders by filter
        if ($('#loadOrders').length > 0) this.getOrdersByFilter();
    },
   
    getOrdersByFilter: function () {
        var $btnSearch = $('.btn-search');
        var btnSearchPrevHtml = $btnSearch.html();
        $btnSearch.attr('disabled', 'disabled');
        $btnSearch.html(`<i class="fa fa-circle-o-notch fa-spin fa-fw"></i> Searching`);

        var searchTerm = $('#SearchTermOrder').val();

        var formData = {
            searchTerm: searchTerm
        };

        $('#loadOrders').html('<img src="/img/loading.gif" />');

        $.ajax({
            url: '/MyProfile/GetOrderByFilter',
            type: "POST",
            data: formData,
            //dataType: "json",
            success: function (response) {
                $('#loadOrders').html(response);

                $btnSearch.removeAttr('disabled');
                $btnSearch.html(btnSearchPrevHtml);
            },
            error: function (err) {
                var msg = `Warning, There was an error while trying to get orders or check your internet connection.`;
                app.Notify(AlertTypeConstants.Warning, msg, AlertIconConstants.Warning);
            }
        })
    },

}

$(function () {
    orderManager.init();   

    $('.btn-search').on('click', function () {        
        //get orders by filter
        orderManager.getOrdersByFilter();
    })   

})
