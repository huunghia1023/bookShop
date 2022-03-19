$('body').on('click', '#btn_search_order', async function (e) {
    e.preventDefault();
    const id = $("#inp_search_order").val();
    if (!id) {
        await Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: "Order is invalid"
        });
        return;
    }
    const culture = $('#hiddenCulture').val();
    window.location.href = "/" + culture + "/orders/" + id;
});

$('body').on('click', '.btn_cancel_order', async function (e) {
    e.preventDefault();
    const id = $("#order_id").val();
    if (!id) {
        await Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: "Order is invalid"
        });
        return;
    }
    const culture = $('#hiddenCulture').val();
    $.ajax({
        type: "POST",
        url: "/" + culture + "/Order/Cancel",
        data: {
            id: id
        },
        success: function (res) {
            if (res) {
                Swal.fire({
                    position: 'center',
                    icon: 'success',
                    title: "Cancel Order #" + id + " Successed",
                    showConfirmButton: true
                }).then(function () {
                    // function when confirm button clicked
                    window.location.reload();
                });
            }
            else {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: "Cancel Order #" + id + " Failed"
                });
            }
        },
        error: function (err) {
            console.log(err);
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: "Cancel Order #" + id + " Failed"
            });
        }
    });
});