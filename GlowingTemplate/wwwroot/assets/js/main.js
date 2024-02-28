const addbasketbtns = document.querySelectorAll(".add-basket-button");
const box = document.querySelector(".basket-products");

addbasketbtns.forEach(btn => {
    btn.addEventListener("click", function (e) {
        e.preventDefault();

        var endpoint = btn.getAttribute("href");

        fetch(endpoint)
            .then(respose => respose.text())
            .then(data => {
                box.innerHTML = data;
            })
    })
})

/*---------------------------------------------------------------------------*/

const showModalBtns = document.querySelectorAll(".prdmodalshow");
const modalbox = document.querySelector(".product-details-modal");

showModalBtns.forEach(btn => {
    btn.addEventListener("click", function (e) {
        e.preventDefault();

        var endpoint = btn.getAttribute("href");

        fetch(endpoint)
            .then(respose => respose.text())
            .then(data => {
                modalbox.innerHTML = data;
            })
    })
})
