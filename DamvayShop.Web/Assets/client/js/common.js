var search = {
    init: function () {
        search.registerEvents();
    },
    registerEvents: function () {
        $(function () {
            $("#keyworks").autocomplete({
                minLength: 0,
                source: function (request, response) {
                    $.ajax({
                        url: "/Product/GetListProductByName",
                        dataType: "json",
                        data: {
                            prodcuctName: request.term
                        },
                        success: function (res) {
                            response(res.data);
                        }
                    });
                },
                focus: function (event, ui) {
                    $("#keyworks").val(ui.item.label);
                    return false;
                },
                select: function (event, ui) {
                    $("#keyworks").val(ui.item.label);
                    return false;
                }
            }).autocomplete("instance")._renderItem = function (ul, item) {
                return $("<li>")
                    .append("<a>" + item.label + "</a>")
                    .appendTo(ul);
            };    
        })

        $("#btnlogout").off('click').on('click', function (e) {
            e.preventDefault();
            $("#frmLogout").submit();
        })
       
    }
    

}

search.init();