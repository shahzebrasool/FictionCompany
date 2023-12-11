$("#dataTable").DataTable()
let placeholderElement = $('#modal-placeholder');
$(".btn-edit-user").click(function (e) {
    e.preventDefault();
    let element = $(this);
    let id = element.attr("data-id");

});


$(".btn-new-user").on("click", function (e) {
    e.preventDefault();

    let element = $(this);

    $.ajax({
        url: "/Home/Create",
        type: "GET",
        data: { id: element.attr('data-id') },
        success: function (response) {
            placeholderElement.html(response);
            placeholderElement.find('.modal').modal('show');
        },
        error: function () {
        }

    });
});





$(".btn-edit-user").on("click", function (e) {
    e.preventDefault();

    let element = $(this);

    $.ajax({
        url: "/Home/Edit",
        type: "GET",
        data: { id: element.attr('data-id') },
        success: function (response) {
            placeholderElement.html(response);
            placeholderElement.find('.modal').modal('show');
        },
        error: function () {
        }

    });
});


$(".btn-delete-user").on("click", function (e) {
    e.preventDefault();
    let element = $(this);

    $.ajax({
        url: "/Home/Delete",
        type: 'POST',
        data: { id: element.attr("data-id") },
        success: function (response) {
            
            toastr.success('Record Deleted Successfully');
            
        },
        error: function () {
            toastr.error('somethng went wrong');
        }
    });

});


