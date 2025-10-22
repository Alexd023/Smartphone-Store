// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    const filterForm = document.getElementById("filterForm");

    if (filterForm) {
        const minPriceInput = filterForm.querySelector("#minPrice");
        const maxPriceInput = filterForm.querySelector("#maxPrice");
        const isUserSpecifiedMinPriceInput = filterForm.querySelector("#isUserSpecifiedMinPrice");
        const isUserSpecifiedMaxPriceInput = filterForm.querySelector("#isUserSpecifiedMaxPrice");

        if (minPriceInput && maxPriceInput && isUserSpecifiedMinPriceInput && isUserSpecifiedMaxPriceInput) {
            minPriceInput.addEventListener("change", function () {
                isUserSpecifiedMinPriceInput.value = "true";
                document.getElementById("filterForm").submit();
            });

            maxPriceInput.addEventListener("change", function () {
                isUserSpecifiedMaxPriceInput.value = "true";
                document.getElementById("filterForm").submit();
            });

            var sortOrder = document.querySelector("#sortOrder").getAttribute("data-sort-order");
            if (sortOrder) {
                document.getElementById("sortOrder").value = sortOrder;
            }
        }
    }

    const checkoutButton = document.getElementById("checkoutButton");
    if (checkoutButton) {
        checkoutButton.addEventListener("click", function () {
            document.getElementById("orderForm").style.display = "block";
        });
    }

    $('.thumbnail-img').on('click', function () {
        var newSrc = $(this).attr('data-src');
        var mainImage = $('#mainImage');
        var mainImageContainer = $('.main-image-container');

        if (mainImage.attr('src') === newSrc) {
            return;
        }

        var tempImage = $('<img>').attr('src', newSrc).addClass('img-fluid main-image').css({
            position: 'absolute',
            left: '100%',
            top: 0,
            display: 'none'
        });

        mainImageContainer.append(tempImage);

        tempImage.css('display', 'block').animate({ left: '0' }, 500, function () {
            mainImage.animate({ left: '-100%' }, 500, function () {
                $(this).remove();
            });
            $(this).css('position', 'relative');
            tempImage.removeClass('main-image');
            mainImage.remove();
            tempImage.attr('id', 'mainImage');
        });
    });
});

function confirmDelete() {
    return confirm('Are you sure you want to delete this order?');
}