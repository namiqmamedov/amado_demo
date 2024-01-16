$(document).ready(function () {

    $('.searchInput').keyup(function () {

        $(".search-body").addClass("d-block");

        let search = $(this).val().trim();

        let url = $(this).data("url")

        url = url + '?search=' + search

        if (search) {
            fetch(url).then(res => res.text()).then(data => {
                $('.search-body .list-group').html(data);
            })
        }
        else {
            $('.search-body .list-group').html('');
            $(".search-body").removeClass("d-block");
        }
    })

})

$(document).on('click', '.minusCount', function (e) {
    e.preventDefault();

    debugger

    let inputCount = $(this).next().val();

    if (inputCount >= 2) {
        inputCount--;
        $(this).next().val(inputCount);
        let url = $(this).attr('href') + '/?count=' + inputCount;

        fetch(url)
            .then(res => res.text())
            .then(data => {
                $('#cartIndex').html(data);
            });

    }
})

$(document).on('click', '.plusCount', function (e) {
    e.preventDefault();

    let inputCount = $(this).prev().val();

    if (inputCount > 0) {
        inputCount++;
    }
    else {
        inputCount = 1;
    }

    $(this).prev().val(inputCount); inputCount

    let url = $(this).attr('href') + '/?count=' + inputCount;


    fetch(url)
        .then(res => res.text())
        .then(data => {
            $('#cartIndex').html(data);
        });

})


$(document).on('click', '.remove-item', function (e) {
    e.preventDefault();

    let url = $(this).attr('href');

    fetch(url).then(response => {

        fetch("/cart/getcart").then(response => {
            return response.text();
        }).then(data => {
            $("#cart-info").html(data);
        })

        return response.text();
    }).then(data => {
        $('#cartIndex').html(data);
    })
})


$(document).ready(function () {
    $('.addToCart1').click(function (e) {
        e.preventDefault();

        let quantity = $('#qty').val();

        let url = $(this).attr('href') + '?value=' + quantity;

        fetch(url).then(res => res.text())
            .then(data => {
                $('#cart-info').html(data);
            });
    });
});



