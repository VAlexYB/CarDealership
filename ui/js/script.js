const backToTop = document.querySelector(".back-to-top");

backToTop.addEventListener("click", backTop);
window.addEventListener("scroll", trackScroll);

function trackScroll(){
    const offset = window.pageYOffset;
    const coords = document.documentElement.clientHeight;
    if(offset > coords){
        backToTop.classList.add("back-to-top--show");
    }else{
        backToTop.classList.remove("back-to-top--show");
    }
}

function backTop(){
    if(window.pageYOffset > 0){
        window.scrollBy(0, -75);
        setTimeout(backTop, 0);
    }
}