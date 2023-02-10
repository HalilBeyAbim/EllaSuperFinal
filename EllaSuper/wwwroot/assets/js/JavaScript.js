





$(document).on('change', '#product-quality-from-icon', function () {
    var productId = $(this).parent().children().val();
    var count = $(this).val();

    $.ajax({
        type: "POST",
        url: '/Basket/ChangeProductQuality',
        data: { productId: productId, count: count },
        success: function () {
        },
    });


});






