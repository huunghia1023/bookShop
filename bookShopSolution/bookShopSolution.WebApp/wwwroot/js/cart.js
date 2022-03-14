﻿var CartController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
    }

    function registerEvents() {
        $('body').on('click', '.btn-plus', function (e) {
            e.preventDefault();
            const id = $(this).data("id");
            const quantity = parseInt($('#txt_quantity_' + id).val()) + 1;
            updateCart(id, quantity);
        });

        $('body').on('click', '.btn-minus', function (e) {
            e.preventDefault();
            const id = $(this).data("id");
            const quantity = parseInt($('#txt_quantity_' + id).val()) - 1;
            updateCart(id, quantity);
        });

        $('body').on('click', '.btn-remove', function (e) {
            e.preventDefault();
            const id = $(this).data("id");
            const quantity = 0;
            updateCart(id, quantity);
        });
    }

    function updateCart(id, quantity) {
        const culture = $('#hiddenCulture').val();
        $.ajax({
            type: "POST",
            url: "/" + culture + "/Cart/UpdateCart",
            data: {
                id: id,
                quantity: quantity
            },
            success: function (res) {
                $("#lb_cart_count").text(res.length);
                loadData();
            },
            error: function (err) {
                console.log(err);
            }
        });
    }

    function loadData() {
        const culture = $('#hiddenCulture').val();
        const baseAdress = $('#hiddenBaseAdress').val();
        $.ajax({
            type: "GET",
            url: "/" + culture + "/Cart/GetListItems",
            success: function (res) {
                if (res.length === 0) {
                    $("#table_product_cart").hide();
                    $(".shopping-cart").html("<p class=\"text-center\">Add more product to continue payment</p>");
                }
                var html = "";
                var total = 0;

                $.each(res, function (i, item) {
                    var priceUnit = (item.price * item.quantity).toFixed(2);
                    html += "<tr>" +
                        "<td class=\"image\" data-title=\"No\"><img src=\"" + baseAdress + "/user-content/" + item.image + "\" alt=\"#\"></td>" +
                        "<td class=\"product-des\" data-title=\"Description\">" +
                        "<p class=\"product-name\"><a href=\"#\">" + item.productName + "</a></p>" +
                        "</td>" +
                        "<td class=\"price\" data-title=\"Price\"><span>" + item.price + "</span></td>" +
                        "<td class=\"qty\" data-title=\"Qty\">" +
                        "<div class=\"input-group\">" +
                        "<div class=\"button minus\">" +
                        "<button type=\"button\" class=\"btn btn-primary btn-number btn-minus\" data-id=\"" + item.productId + "\" data-type=\"minus\" data-field=\"quant[1]\">" +
                        "<i class=\"ti-minus\"></i>" +
                        "</button>" +
                        "</div>" +
                        "<input type=\"text\" name=\"quant[1]\" class=\"input-number\" data-min=\"1\" data-max=\"100\" id=\"txt_quantity_" + item.productId + "\" value=\"" + item.quantity + "\">" +
                        "<div class=\"button plus\">" +
                        "<button type=\"button\" class=\"btn btn-primary btn-number btn-plus\" data-id=\"" + item.productId + "\" data-type=\"plus\" data-field=\"quant[1]\">" +
                        "<i class=\"ti-plus\"></i>" +
                        "</button>" +
                        "</div>" +
                        "</div>" +
                        "</td>" +
                        "<td class=\"total-amount\" data-title=\"Total\"><span>" + priceUnit + "</span></td>" +
                        "<td class=\"action\" data-title=\"Remove\"><button type=\"button\" class=\"btn btn-danger btn-remove\" data-id=\"" + item.productId + "\"><i class=\"ti-trash remove-icon\"></i></button></td>" +
                        "</tr>";
                    total = total + item.price * item.quantity;
                });
                $("#card-body").html(html);

                $("#lb_cart_total").text(total.toFixed(2).toString());
            }
        });
    }
}