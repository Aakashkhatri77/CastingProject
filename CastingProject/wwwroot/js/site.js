

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

/*Search Text*/
function ClearFilter() {
    $('.clear').find("input:text").val("");
    $(".clear select").prop("selectedIndex", 0)
}


//Tags Input
//var skills = document.getElementById("skill")
//var skill = new Tagify(skills, { originalInputValueFormat: valuesArr => valuesArr.map(item => item.value).join(',')})
//var hobbies = document.getElementById("hobby")
//var hobby = new Tagify(hobbies, { originalInputValueFormat: valuesArr => valuesArr.map(item => item.value).join(',') })


/*for tag input select option*/
$(document).ready(function () {
    $('.s2').select2();
});


/*For Load More button*/
$(".artist-image").slice(0, 3).show()
$(".load-btn").on("click", function () {
    $(".artist-image:hidden").slice(0, 3).slideDown()
    if ($(".artist-image:hidden").length == 0) {
        $(".load-btn").fadeOut('slow')
    }
})


//-------------------js for testimonialsssss------------------

var galleryThumbs = new Swiper('.gallery-thumbs', {
    effect: 'coverflow',
    grabCursor: true,
    centeredSlides: true,
    slidesPerView: '2',
    // coverflowEffect: {
    //   rotate: 50,
    //   stretch: 0,
    //   depth: 100,
    //   modifier: 1,
    //   slideShadows : true,
    // },

    coverflowEffect: {
        rotate: 0,
        stretch: 0,
        depth: 50,
        modifier: 6,
        slideShadows: false,
    },

});


var galleryTop = new Swiper('.swiper-container.testimonial', {
    speed: 400,
    spaceBetween: 50,
    autoplay: {
        delay: 3000,
        disableOnInteraction: false,
    },
    direction: 'vertical',
    pagination: {
        clickable: true,
        el: '.swiper-pagination',
        type: 'bullets',
    },
    thumbs: {
        swiper: galleryThumbs
    }
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