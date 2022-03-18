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
            var quantity = parseInt($("#quantity").val());
            if (!quantity)
                quantity = 1;
            const culture = $('#hiddenCulture').val();
            $.ajax({
                type: "POST",
                url: "/" + culture + "/Cart/AddToCart",
                data: {
                    id: id,
                    languageId: culture,
                    quantity: quantity
                },
                success: function (res) {
                    $("#lb_cart_count").text(res.length);
                    Swal.fire({
                        position: 'top-end',
                        icon: 'success',
                        title: 'Add product to cart success',
                        showConfirmButton: false,
                        timer: 1500
                    });
                },
                error: function (err) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: err.toString()
                    })
                }
            });
        });
    }
}

function numberWithCommas(x) {
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}