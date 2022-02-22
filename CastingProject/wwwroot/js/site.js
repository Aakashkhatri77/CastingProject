// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code

$(function () {
    var gallery = $('#Gallery');
    $('a[uk-toggle="target: #modal-container"]').click(function (event) {
        var url = $(this).data('url');
        var decodedUrl = decodeURIComponent(url);
        $.get(decodedUrl).done(function (data) {
            gallery.html(data);
            var modal = UIkit.modal("#modal-container");
            modal.show();
        })
    })

    gallery.on('click', '[data-save="modal"]', function (event) {
        var form = $(this).parents("#modal-container").find('form');
        var actionUrl = form.attr('asp-action');
        var sendData = form.serialize();
        $.post(actionUrl, sendData).done(function (data) {
            var modal = UIkit.modal("#modal-container");
            modal.hide();
        })
    })
})
/*
function ClearFilter() {
    document.getElementById("formm").input = "";
}*/

function ClearFilter() {
        
/*    $('#filter').find("input:text").val("");
    $("#filterform select").prop("selectedIndex", 0)
*/
    $('.clear').find("input:text").val("");
    $(".clear select").prop("selectedIndex", 0)

}


            /*gallery.find('.uk-modal-container').modal('show');*/
            //UIkit.modal(gallery).show();
            //$('#modal-container').uk-modal('show');