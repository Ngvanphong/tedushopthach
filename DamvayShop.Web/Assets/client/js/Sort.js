var sort = {
    init: function () {
        sort.registerEvnet();
    },
    registerEvnet: function () {
        $("#choiceshort").on("change", function () {            
            var url = $("#choiceshort").val();      
            $.ajax({
                type: 'GET',
                url: url, 
                dataType: "html",
                success: function (data) {
                    window.location.href = url;
                    //$("#content").html(data);
                }
            });
        })
    }
}
sort.init();



