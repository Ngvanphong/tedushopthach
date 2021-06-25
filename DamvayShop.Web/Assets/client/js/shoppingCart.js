shoppingCart = {
    intit: function () {

        shoppingCart.loadData();
        shoppingCart.registerEvent();
   
    },
    registerEvent: function () {
        $(".btnshoppingCart").off('click').on('click', function (e) {
            e.preventDefault();
            var productId = parseInt($(this).data('id'));
            var sizeId = parseInt($("#SizeSelectList_"+productId).val());
            shoppingCart.addItem(productId,sizeId);
        });
        $("#gotoCheckout").off('click').on('click', function (e) {
            e.preventDefault();
            shoppingCart.updateAll();
        });

        $(".btnDeleteItemShoppingCart").off('click').on('click', function (e) {
            e.preventDefault();
            var productId = parseInt($(this).data('id'));
            var sizeName = $(this).data('size')
            shoppingCart.deleteItem(productId,sizeName);
        });
        $(".SizeSelectList").off('change').on('change', function (e) {
            e.preventDefault();
            if ($(".SizeSelectList").val() != "nochoice") {
                $(".btnshoppingCart").removeAttr('disabled');
            }
            else {
                $(".btnshoppingCart").attr('disabled',true)
            }
            
        });
       

        $("#gotoComfirm").off('click').on('click', function (e) {
            e.preventDefault();
            var valid = $("#validationOrder").valid();
            if (valid) {
                shoppingCart.gotoComfirm();
            };
       
        });

        $("#validationOrder").validate({
            rules: {
                name: "required",
                address: "required",
                email: {
                    email: true,
                },
                mobile: "required",
                CitySelectList: "required",
               
            },
            messages: {
                name: "Bạn phải nhập tên",
                address: "Bạn phải nhập địa chỉ",
                email: {
                    email: "Email không đúng"
                },
                mobile: "Bạn phải nhập số điện thoại",
                CitySelectList: "",             
            }
        });

        $("#CitySelectList").off('change').on('change', function (e) {
            e.preventDefault();
            var province = $("#CitySelectList").val();
            if (province == 701) {
                $("#proviceDisplay").show();
                shoppingCart.loadDistrict(province);
            }
            else {
                $("#proviceDisplay").hide();
                $("#taxtransfer").text("20.000");
            }

        });

        $("#DistrictSelectList").off('change').on('change', function (e) {
            e.preventDefault();
            
            var districtId = $("#DistrictSelectList").val();
            if (districtId != "") {
                shoppingCart.getTaxForHCM(districtId);
            }
            else {
                $("#taxtransfer").text("14.000");
            }
        });

        $("#createOrder").off('click').on('click', function (e) {
            e.preventDefault();
            $.ajax({
                url: "/Checkout/CreateOrder",
                type: "POST",
                dataType: "Json",
                success: function (res) {
                    if (res.status) {
                        alert("Cảm ơn quý khách đã mua hàng");                     
                        window.location.href = "/index.html";
                    }
                }
            })

        });
       

        $(".txtKeyupQuantity").off('keyup').on('keyup', function (e) {
            e.preventDefault();
            var salePrice = parseInt($(this).data('price'));
            var quantity = parseInt($(this).val());
            var size = $(this).data('size')
            var productId = parseInt($(this).data('id'));

            if (isNaN(quantity) == false) {

                $("#amount_" + productId+"_"+size).text(numeral(salePrice * quantity).format('0,0'));

            }
            else {
                $("#amount_" + productId+"-"+size).text(0)
            }
            shoppingCart.getTotalPrice();
        });


    },

    getTotalPrice: function () {
        var total = 0;
        $.each($(".txtKeyupQuantity"), function (i, item) {
            var price = parseInt($(item).data('price'));
            var quantity = parseInt($(item).val());
            total += price * quantity;
        });
        $("#totalPriceShopping").text(numeral(total).format('0,0'));

    },



    addItem: function (productId,sizeId) {
        $.ajax({
            url: "/ShoppingCart/Add",
            type: "POST",
            dataType: "Json",
            data: {
                productId: productId,
                sizeId:sizeId,
            },
            success: function (res) {
                if (res.status) {
                   
                    alert("Bạn đã thêm một sản phẩm vào giỏ hàng");
                }
            }
        })

    },

    updateAll: function () {

        var cartList = [];
        $.each($(".txtKeyupQuantity"), function (i, item) {
            cartList.push({
                productId: $(this).data("id"),
                Quantity: $(this).val(),
                SizesVm:{
                   Name: $(this).data("size"),
                }
            });
        });
        $.ajax({
            url: "/ShoppingCart/Update",
            type: "POST",
            dataType: "Json",
            data: {
                listCart: JSON.stringify(cartList),
            },
            success: function (res) {
                if (res.status) {
                    shoppingCart.loadData();
                    window.location.href = "/checkout.html";
                }
            }

        });

    },
   

    loadData: function () {
        $.ajax({
            cache: false,
            url: "/ShoppingCart/GetAll",
            type: "GET",
            dataType: "Json",
            success: function (res) {
                if (res.status) {

                    var template = $('#tmpShoppingContent').html();
                    var html = "";
                    $.each(res.data, function (i, item) {
                        var salePrice = 0;
                        if (item.productViewModel.PromotionPrice != null && item.productViewModel.PromotionPrice != 0) {
                            salePrice = parseInt(item.productViewModel.PromotionPrice) * 1000;
                        }
                        else {
                            salePrice = parseInt(item.productViewModel.Price) * 1000;
                        };
                        html += Mustache.render(template, {
                            Image: item.productViewModel.ThumbnailImage,
                            Name: item.productViewModel.Name,
                            Size: item.SizesVm.Name,
                            ProductId: item.productId,
                            Quantity: item.Quantity,
                            Price: item.productViewModel.Price * 1000,
                            PriceF: numeral(item.productViewModel.Price * 1000).format('0,0'),
                            PromotionPrice: numeral(item.productViewModel.PromotionPrice * 1000).format('0,0'),
                            salePrice: salePrice,
                            Amount: numeral(salePrice * item.Quantity).format('0,0'),
                        })
                    });

                    $('#shopingContent').html(html);          
                    shoppingCart.registerEvent();
                    shoppingCart.getTotalPrice();
                  
                }
            }
        })
    },

  

    deleteItem: function (productId,sizeName) {
        $.ajax({
            url: "/ShoppingCart/DeleteItem",
            type: "POST",
            dataType: "Json",
            data: {
                productId: productId,
                size:sizeName
            },
            success: function (res) {
                if (res.status) {
                    shoppingCart.loadData();
                }
            }

        })

    },

    getTaxForHCM: function (districtId) {
        $.ajax({
            url: "/Checkout/GetTaxHCM",
            type: "POST",
            dataType: "Json",
            data: {
                districtId:districtId
            },
            success: function (res) {
                if (res.status) {
                    var taxTransfer = res.data;
                    $("#taxtransfer").text(taxTransfer);
                }
            }

        });
    },

    gotoComfirm: function () {
        var taxPrice = 0;
        var orderVm = {};
        taxPrice = $("#taxtransfer").text();
        var taxTransfer = parseInt(taxPrice.slice(0, taxPrice.indexOf('.')));
        orderVm = {
            CustomerName:$("#name").val(),
            CustomerAddress: $("#address").val(),
            CustomerMobile: $("#mobile").val(),
            CustomerEmail: $("#email").val(),          
        };

        $.ajax({
            url: "/Checkout/GotoComfirm",
            type: "POST",
            dataType: "Json",
            data: {
                totalPrice:taxTransfer,
                orderVm: JSON.stringify(orderVm),
            },
            success: function (res) {
                if (res.status) {
                    window.location.href = "/overview.html";
                }
            }

        });

    },

    loadDistrict: function (provinceId) {
        $.ajax({
            url: "/Checkout/LoadDistrict",
            type: "POST",
            dataType: "Json",
            data: {
                provinceId:provinceId,
            },
            success: function (res) {
                if (res.status) {                
                    var html = '<option value="">Chọn quận/huyện</option>';
                    var data = res.data;
                    $.each(data, function (i, item) {

                        html += '<option value="'+item.ID+'">'+item.Name+'</option>'
                    });
                    $("#DistrictSelectList").html(html);
                }
            }


        });

    }

   


}

shoppingCart.intit();