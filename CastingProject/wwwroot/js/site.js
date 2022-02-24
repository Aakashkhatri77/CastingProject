// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

/*const tagify = require("./tagify");
const tagify = require("./tagify");*/


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

function ClearFilter() {
    $('.clear').find("input:text").val("");
    $(".clear select").prop("selectedIndex", 0)
}


/*Tags Input*/
var skills = document.getElementById("skill")
var skill = new Tagify(skills, { originalInputValueFormat: valuesArr => valuesArr.map(item => item.value).join(',')})
var hobbies = document.getElementById("hobby")
var hobby = new Tagify(hobbies, { originalInputValueFormat: valuesArr => valuesArr.map(item => item.value).join(',') })


//for tag input select option
$(document).ready(function () {
    $('.s2').select2();
});
























































/*<div class="tag">
    <span>Javascript</span>
    <i href="" uk-icon="close"></i>
</div>*/
/*
const tagContainer = document.querySelector('.tag-container');
const input = document.querySelector('.tag-container .input-tag');

var tags = [];

function createTag(label) {
    const div = document.createElement('div');
    div.setAttribute('class', 'tag');
    const span = document.createElement('span');
    span.innerHTML = label;
    const closeBtn = document.createElement('i');
    closeBtn.setAttribute('class', 'uk-icon');
    closeBtn.setAttribute('data-item', label);
    closeBtn.innerHTML = 'X';

    div.appendChild(span);
    div.appendChild(closeBtn);
    return div;


}

function reset() {
    document.querySelectorAll('.tag').forEach(function (tag) {
        tag.parentElement.removeChild(tag);
    })
}

function addTags() {
    reset();
    tags.slice().reverse().forEach(function (tag) {
        const input = createTag(tag);
        tagContainer.prepend(input);
    })
}

input.addEventListener('keyup', function (e) {
    if (e.key === "Enter") {
        tags.push(input.value);
        addTags();
        input.value = "";
    }
})

document.addEventListener('click', function (e) {
    if (e.target.tagName === 'I') {
        const value = e.target.getAttribute('data-item');
        const index = tags.indexOf(value);
        tags = [...tags.slice(0, index), ...tags.slice(index + 1)];
        console.log(tags);
        addTags();
    }
})*/