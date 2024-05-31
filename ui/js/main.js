const menuBurger = document.querySelector(".header__burger");
const nav = document.querySelector(".header__nav");



// menuBurger.addEventListener("click",()=>{
//     nav.classList.toggle("mobileMenu");
// });

nav.addEventListener("click",()=>{
    nav.classList.toggle("mobileMenu");
})


//block code


const  catalogLink = document.querySelector(".catalog__btn");

const catalogLinks = document.querySelector(".catalog__links");

function revealContent(){
    if(catalogLinks.classList.contains("catalog__btn")){
        catalogLinks.classList.remove("catalog__btn");
    }else{
        catalogLinks.classList.add("catalog__btn");
    }
}


catalogLink.addEventListener('click', revealContent)