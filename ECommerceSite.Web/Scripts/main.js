//displays the basket items
function listBasketItems(products) {
    var $basket = $('#basket');
    $basket.empty();
    var $ul = $('<ul/>');
    if (products && products.length > 0) {
        var tootalPrice = 0, currentPrice = 0;
        for (var i = 0, len = products.length; i < len; i++) {
            currentPrice = products[i].Amount * products[i].Price;
            tootalPrice += currentPrice;
            $('<li id="' + products[i].Id + '"/>')
             .append($('<h5 class="name"/>')
             .html(products[i].Name))
             .append($('<div class="info"/>')
                .append($('<span class="price"/>')
                .html("price: " + products[i].Price))
                .append($('<span class="amount"/>')
                .html("amount: " + products[i].Amount))
                .append($('<span class="total"/>')
                .html("total: " + currentPrice)))
             .append($('<div class="remove-btn"/>')
                .append($('<a href="#" class="btn btn-danger" onclick="removeItem(' + products[i].Id + ')"/>')
                .html("Remove &raquo;")))
             .appendTo($ul);
        }
        $basket.append($ul);
        $('<div class="total-price"/>').html("Total price: <strong>" + tootalPrice + "</strong>").appendTo($basket);
        $('#confirmOrderBtn').show();
        $('#basket-wrap').show('slow');
    }
    else {
        $('#basket').append($('<h3 class="empty-bag-msg"/>').html("The basket is empty!"));
        $('#confirmOrderBtn').hide();
    }
}

//shows/hides the local storage basket
function toggleBasketLS() {
    var $basketWrap = $('#basket-wrap');
    if ($basketWrap.css('display') == 'none') {
        var products = localStorage.getObject("products");
        $('#basketBtn').html("Hide");
        listBasketItems(products);
        $basketWrap.show('slow');
    } else {
        $('#basketBtn').html("Basket");
        $basketWrap.hide('slow');
    }
}

//shows/hides the database basket
function toggleBasketDB() {
    var $basketWrap = $('#basket-wrap');
    if ($basketWrap.css('display') == 'none') {
        $.ajax({
            url: 'Home/GetBasketItems',
            type: 'Get',
            contentType: 'application/json',
            success: function (products) {
                $('#basketBtn').html("Hide");
                listBasketItems(products);
                $basketWrap.show('slow');
            },
            error: function () {
                $('#error').html("Error happened: " + err)
            }
        });
    } else {
        $('#basketBtn').html("Basket");
        $basketWrap.hide('slow');
    }
}

//removes product to the local storage basket
function removeFromLS(id) {
    var products = localStorage.getObject("products");
    for (var i = 0, len = products.length; i < len; i++) {
        if (products[i].Id == id) {
            if (products[i]['Amount'] > 1) {
                products[i]['Amount']--;
                break;
            }
            if (products[i]['Amount'] == 1) {
                products.splice(i, 1);
                break;
            }
        }
    }

    localStorage.setObject("products", products);
    listBasketItems(products);
}

//removes product from the database basket
function removeFromDB(id) {
    $.ajax({
        url: 'Home/RemoveFromBasket?productId=' + id + '',
        type: 'POST',
        contentType: 'application/json',
        success: function (products) {
            listBasketItems(products)
        },
        error: function () {
            $('#error').html("Error happened: " + err)
        }
    });
}

//adds product to the local storage basket
function addToBasketLS(product) {
    var products = localStorage.getObject("products");
    if (!products) {
        products = [];
        newProduct = {};
        newProduct['Id'] = product.Id;
        newProduct['Name'] = product.Name;
        newProduct['Price'] = product.Price;
        newProduct['Amount'] = 1;
        products.push(newProduct);
    } else {
        var isInBasket = false;
        for (var i = 0, len = products.length; i < len; i++) {
            if (products[i].Id == product.Id) {
                products[i]['Amount']++;
                isInBasket = true;
                break;
            }
        }
        if (!isInBasket) {
            newProduct = {};
            newProduct['Id'] = product.Id;
            newProduct['Name'] = product.Name;
            newProduct['Price'] = product.Price;
            newProduct['Amount'] = 1;
            products.push(newProduct);
        }
    }
    localStorage.setObject("products", products);
    listBasketItems(products);
}

//adds product to the database basket
function addToBasketDB(productId) {
    $.ajax({
        url: 'Home/AddToBasket?productId=' + productId + '',
        type: 'POST',
        contentType: 'application/json',
        success: function (products) {
            listBasketItems(products)
        },
        error: function () {
            $('#error').html("Error happened: " + err)
        }
    });
}

//on login/register sets value the hidden products input
function setInputValueToProjects(id) {
    var products = localStorage.getObject("products");
    if (products) {
        var productsTiny = {};
        for (var i = 0, len = products.length; i < len; i++) {
            productsTiny[products[i].Id] = products[i].Amount;
        }

        $(id).val(JSON.stringify(productsTiny));
    }
}

//confirm order - local storage
function confirmOrderLS() {
    var products = localStorage.getObject("products");
    confirmProducts(products);
    toggleBasketLS();
}

//confirm order - database (if the user is logged)
function confirmOrderDB() {
    $.ajax({
        url: 'Home/GetBasketItems',
        type: 'Get',
        contentType: 'application/json',
        success: function (data) {
            confirmProducts(data);
            toggleBasketDB();
        },
        error: function () {
            $('#error').html("Error happened: " + err)
        }
    })
}

//validates the products and if there are no errors posts them
function confirmProducts(products) {
    if (products && products.length > 0) {
        var order = {};
        order.Products = [];
        order.TotalPrice = 0;
        for (var i = 0, len = products.length; i < len; i++) {
            if (products[i].Amount > 5) {
                $('#error').html('Тhe selected quantity for product ' + products[i].Name + ' is out of stock!').fadeIn(2000).delay(2000).fadeOut(2000);
                return;
            }
            var product = {};
            product['Name'] = products[i].Name;
            product['Amount'] = products[i].Amount;
            order.Products.push(product);
            order.TotalPrice += products[i].Amount * products[i].Price;
        }
        postOrder(order);
    } else {
        $('#error').html('Your shopping card is empty!').fadeIn(2000).delay(2000).fadeOut(2000);
    }
}

//posts the order to the server
function postOrder(order) {
    $.ajax({
        url: 'Home/ConfirmOrder',
        type: 'POST',
        data: JSON.stringify(order),
        contentType: 'application/json',
        success: function (order) {
            confirmOrderSuccess(order);
        },
        error: function (err) {
            console.log("Order sent error!");
        }
    });
}

//displays the ordered items on post success
function confirmOrderSuccess(order) {
    localStorage.clear();
    $success = $('#success');
    $success.empty();
    $success.append($('<h3/>').html('Your order is confirmed!'));
    $ul = $('<ul class="ordered-products"/>')
    for (var i = 0, len = order.Products.length; i < len; i++) {
        $ul.append($('<li/>')
            .append($('<span/>')
            .html(order.Products[i].Name))
        .append($('<strong/>')
        .html("amount: "+order.Products[i].Amount)));
    }
    $ul.appendTo($success);
    $success.append($('<div class="total-price-success"/>')
            .html("Total price: " + order.TotalPrice))
            .fadeIn(2000).delay(2000).fadeOut(2000);
}