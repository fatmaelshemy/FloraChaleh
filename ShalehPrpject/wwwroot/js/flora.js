const sectionHero = document.querySelector(".hero");
const mainImg = document.querySelector(".main-img");
// Float image
const classFloat = document.querySelector(".floating-img");
const mainImgFloat = document.querySelector(".main-img-float");
// Thumbnail images
const thumbnailImg = document.querySelectorAll(".thumb-img img");
// Thumbnail images floating
const thumbnailImgFloat = document.querySelectorAll(".thumb-img-float img");

const overlayC = document.querySelector(".overlay");
const closeIcon = document.querySelector(".close-icon");

// Buttons
const leftArrow = document.querySelector(".btn-swipe-left");
const rightArrow = document.querySelector(".btn-swipe-right");

const minusBtn = document.querySelector(".minus");
const cartNumber = document.querySelector(".cart-number");
const plusBtn = document.querySelector(".plus");

let index = 0;

mainImg.addEventListener("click", function () {
    console.log("Main image clicked");
    classFloat.classList.add("activate");
    overlayC.classList.add("activate");

    mainImgFloat.src = mainImg.src;
    for (let j = 0; j < thumbnailImgFloat.length; j++) {
        thumbnailImgFloat[j].parentNode.classList.remove("active-thumb");
        if (thumbnailImg[j].parentNode.classList.contains("active-thumb")) {
            thumbnailImgFloat[j].parentNode.classList.add("active-thumb");
            index = j;
        }
    }
});

const removeFloatImg = function () {
    classFloat.classList.remove("activate");
    overlayC.classList.remove("activate");
};

closeIcon.addEventListener("click", function () {
    removeFloatImg();
});

document.addEventListener("keydown", function (e) {
    if (e.key === "Escape") {
        removeFloatImg();
    }
});

// Function to set the image source based on available extensions
function setImageSource(basePath, index) {
    const jpgPath = `${basePath}${index}.jpg`;
    const jpegPath = `${basePath}${index}.jpeg`;
    const mp4Path = `${basePath}${index}.mp4`;

    fetch(jpgPath, { method: 'HEAD' })
        .then((response) => {
            if (response.ok) {
                mainImg.src = jpgPath;
                mainImgFloat.src = jpgPath;
            } else {
                fetch(jpegPath, { method: 'HEAD' })
                    .then((response) => {
                        if (response.ok) {
                            mainImg.src = jpegPath;
                            mainImgFloat.src = jpegPath;
                        } else {
                            fetch(mp4Path, { method: 'HEAD' })
                                .then((response) => {
                                    if (response.ok) {
                                        mainImg.style.display = "none";
                                        mainImgFloat.style.display = "none";
                                        let videoElem = document.createElement('video');
                                        videoElem.setAttribute('controls', '');
                                        videoElem.src = mp4Path;
                                        mainImg.parentNode.appendChild(videoElem);
                                    } else {
                                        console.error('No valid media found.');
                                    }
                                })
                                .catch(() => {
                                    console.error('Error fetching mp4.');
                                });
                        }
                    })
                    .catch(() => {
                        console.error('Error fetching jpeg.');
                    });
            }
        })
        .catch(() => {
            console.error('Error fetching jpg.');
        });
}


// Changing main image
thumbnailImg.forEach((thumbnail, i) => {
    thumbnail.parentNode.addEventListener("click", function () {
        console.log(`Thumbnail ${i + 1} clicked`);
        thumbnailImg.forEach(img => img.parentNode.classList.remove("active-thumb"));
        thumbnail.parentNode.classList.add("active-thumb");

        const thumbnailIndex = i + 1;
        setImageSource("/assets/images/shop", thumbnailIndex);
        console.log(`Main image changed to /assets/images/shop${thumbnailIndex}.jpg or .jpeg`);
    });
});

// Changing floating images
thumbnailImgFloat.forEach((thumbnail, i) => {
    thumbnail.parentNode.addEventListener("click", function () {
        console.log(`Floating thumbnail ${i + 1} clicked`);
        thumbnailImgFloat.forEach(img => img.parentNode.classList.remove("active-thumb"));
        thumbnail.parentNode.classList.add("active-thumb");
        index = i;

        const thumbnailFloatIndex = i + 1;
        setImageSource("/assets/images/shop", thumbnailFloatIndex);
        console.log(`Floating main image changed to /assets/images/shop${thumbnailFloatIndex}.jpg or .jpeg`);
    });
});

// Swiping through floating images
leftArrow.addEventListener("click", function () {
    if (index > 0) {
        thumbnailImgFloat[index].parentNode.classList.remove("active-thumb");
        index -= 1;
        setImageSource("/assets/images/shop", index + 1);
        thumbnailImgFloat[index].parentNode.classList.add("active-thumb");
        console.log(`Swiped left to image /assets/images/shop${index + 1}.jpg or .jpeg`);
    }
});

rightArrow.addEventListener("click", function () {
    if (index < thumbnailImgFloat.length - 1) {
        thumbnailImgFloat[index].parentNode.classList.remove("active-thumb");
        index += 1;
        setImageSource("/assets/images/shop", index + 1);
        thumbnailImgFloat[index].parentNode.classList.add("active-thumb");
        console.log(`Swiped right to image /assets/images/shop${index + 1}.jpg or .jpeg`);
    }
});

// Cart functionality
let cartCount = 0;

minusBtn.addEventListener("click", function () {
    if (cartCount > 0) {
        cartCount -= 1;
        cartNumber.textContent = cartCount;
    }
});

plusBtn.addEventListener("click", function () {
    if (cartCount < 10) {
        cartCount += 1;
        cartNumber.textContent = cartCount;
    }
});

const navCart = document.querySelector(".nav-cart");
const cartBox = document.querySelector(".cart");
const emptyCart = document.querySelector(".empty-cart");
const filledCart = document.querySelector(".cart-bottom");
const numbercart = document.querySelector(".count-items");
const currPrice = document.querySelector(".current-price");
const deleteIcon = document.querySelector(".delete");
const navAfter = document.querySelector(".nav-cart-after");

let itemsCount = 0;

navCart.addEventListener("click", function () {
    cartBox.classList.toggle("show");
});

document.addEventListener("keydown", function (e) {
    if (e.key === "Escape") cartBox.classList.remove("show");
});

const btnRight = document.querySelector(".btn-right");

btnRight.addEventListener("click", function () {
    if (cartCount > 0 && itemsCount + cartCount <= 20) {
        itemsCount += cartCount;
        emptyCart.classList.remove("show");
        filledCart.classList.add("show");
        currPrice.textContent = `$${itemsCount * 125}.00`;
        numbercart.textContent = itemsCount;
        navAfter.classList.add("show");
        navAfter.textContent = itemsCount;
    }

    if (itemsCount === 20) {
        navAfter.textContent = "full";
    }
});

deleteIcon.addEventListener("click", function () {
    emptyCart.classList.add("show");
    filledCart.classList.remove("show");
    itemsCount = 0;
    navAfter.classList.remove("show");
});

document.body.addEventListener("click", function (e) {
    if (!cartBox.classList.contains("show")) {
        return;
    }

    const arr = e.path || e.composedPath();
    for (let i = 0; i < arr.length; i++) {
        if (
            btnRight === arr[i] ||
            leftArrow === arr[i] ||
            navCart === arr[i] ||
            cartBox === arr[i]
        ) {
            return;
        }
    }

    cartBox.classList.remove("show");
});
