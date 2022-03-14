var SiteController = function () {
    this.initialize = function () {
        registerEvents();
        loadCart();
    }

    function loadCart() {
        const culture = $('#hiddenCulture').val();
        $.ajax({
            type: "GET",
            url: "/" + culture + "/Cart/GetListItems",
            success: function (res) {
                $("#lb_cart_count").text(res.length);
            }
        });
    }

    function registerEvents() {
        $('body').on('click', '.btn-add-cart', function (e) {
            e.preventDefault();
            const id = $(this).data("id");
            const culture = $('#hiddenCulture').val();
            $.ajax({
                type: "POST",
                url: "/" + culture + "/Cart/AddToCart",
                data: {
                    id: id,
                    languageId: culture
                },
                success: function (res) {
                    $("#lb_cart_count").text(res.length);
                },
                error: function (err) {
                    console.log(err);
                }
            });
        });
    }
}

function numberWithCommas(x) {
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}