document.addEventListener("DOMContentLoaded", async function() {
    await import('../scripts/navbarCustomer.js');
    //await getEntities();
});

const handleEvent = (event) => {
    event.stopPropagation();
    console.log(123);
}