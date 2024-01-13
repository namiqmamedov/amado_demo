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
