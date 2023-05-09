var mybutton = document.getElementById("btn-back-to-top");
window.onscroll = function () {
    scrollFuntion()
};
function scrollFuntion() {
    if (document.body.scrollTop > 30 || document.documentElement.scrollTop > 30) {

        mybutton.style.display = "block";
    }
    else {

        mybutton.style.display = "none";
    }
}

mybutton.addEventListener("click", function(){
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
});



// Product Details Slider

$(".product__details__pic__slider").owlCarousel({
        loop: false,
        margin: 0,
        items: 1,
        dots: false,
        nav: true,
        navText: ["<i class='arrow_carrot-left'></i>", "<i class='arrow_carrot-right'></i>"],
        smartSpeed: 1200,
        autoHeight: false,
        autoplay: false,
        mouseDrag: false,
        startPosition: 'URLHash'
    }).on('changed.owl.carousel', function (event) {
        var indexNum = event.item.index + 1;
        product_thumbs(indexNum);
});

    function product_thumbs(num) {
        var thumbs = document.querySelectorAll('.product__thumb a');
        thumbs.forEach(function (e) {
            e.classList.remove("active");
            if (e.hash.split("-")[1] == num) {
                e.classList.add("active");
            }
        })
    };
//Preloader
$(window).on('load', function () {
    $(".loader").fadeOut();
    $("#preloder").delay(100).fadeOut("slow");

    /*------------------
        Product filter
    --------------------*/
    $('.filter__controls li').on('click', function () {
        $('.filter__controls li').removeClass('active');
        $(this).addClass('active');
    });
    if ($('.property__gallery').length > 0) {
        var containerEl = document.querySelector('.property__gallery');
        var mixer = mixitup(containerEl);
    }
});




//Load--img
function ShowImagePreview(imageUploader, previewImage) {
    if (imageUploader.files && imageUploader.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $(previewImage).attr('src', e.target.result);
        }
        reader.readAsDataURL(imageUploader.files[0]);
    }
}







