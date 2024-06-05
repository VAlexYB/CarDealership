document.addEventListener("DOMContentLoaded", async function() {
    await import('../scripts/navbarCustomer.js');
    await getBrands();
    await getEntities();
});

const getBrands = async () => {
    const response = await fetch("http://localhost:7243/api/Brands/getAll", {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        },
        credentials: 'include',
    });
    const entities = (await response.json()).sort((a, b) => a.name.localeCompare(b.name));
    const brandSelect = document.getElementById('brand');
    let child = brandSelect.firstChild;
    while (child) {
        const nextSibling = child.nextSibling; 
        if (child.tagName !== 'TEMPLATE') brandSelect.removeChild(child); 
        child = nextSibling; 
    }
    if (entities.length === 0) {
        document.getElementById('brandMessage').textContent = "Ничего не нашлось";
        return;
    } 
    document.getElementById('brandMessage').textContent = "";
    for (let i = 0; i < entities.length; i++) {
        const entity = entities[i];
        if (i == 0)
            await getModels(entity.id);
        const option = document.createElement('option');
        option.value = entity.id;
        option.textContent = entity.name;
        brandSelect.appendChild(option);
    }
    brandSelect.addEventListener('change', async function(event) {
        await getModels(event.target.value);

    })
}

const getModels = async (brandId) => {
    const response = await fetch("http://localhost:7243/api/AutoModels/getByFilter", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        },
        body: JSON.stringify({brandId: brandId}),
        credentials: 'include',
    });
    const entities = (await response.json()).sort((a, b) => a.name.localeCompare(b.name));
    
    const modelSelect = document.getElementById('model');
    let child = modelSelect.firstChild;
    while (child) {
        const nextSibling = child.nextSibling; 
        if (child.tagName !== 'TEMPLATE') modelSelect.removeChild(child); 
        child = nextSibling; 
    }
    if (entities.length === 0) {
        document.getElementById('modelMessage').textContent = "Ничего не нашлось";
        return;
    } 
    document.getElementById('modelMessage').textContent = "";
    entities.forEach(entity => {
        const option = document.createElement('option');
        option.value = entity.id;
        option.textContent = entity.name;
        modelSelect.appendChild(option);
    });
}




const getEntities = async (body) => {
    const response = await fetch(`http://localhost:7243/api/Cars/getAll`, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        },
        credentials: 'include',
    });
    const entities = (await response.json());
    let cars = [];
    for (let i = 0; i < entities.length; i++) {
        if (!cars.some(obj => obj.configuration.id === entities[i].configuration.id)) {
            cars.push(entities[i]);
        }
    }

    // if (!response.ok) {
    //     const error = await response.json()
    //     console.log(error)
    //     alert(error.errors);
    //     return;
    // }
    // console.log(document.cookie)
    // console.log(123);
    const catalog = document.getElementById('catalog');
    let child = catalog.firstChild;
    while (child) {
        const nextSibling = child.nextSibling; 
        if (child.tagName !== 'TEMPLATE') catalog.removeChild(child); 
        child = nextSibling; 
    }
    if (entities.length === 0) {
        const noEntitiesParagraph = document.getElementById('noEntitiesMessage');
        noEntitiesParagraph.textContent = "Ничего не нашлось";
        return;
    }
    else {
        const carCardTemplate = document.getElementById('carCardTemplate');
        cars.forEach(entity => {
            const carCard = carCardTemplate.content.cloneNode(true);
            const article = carCard.querySelector('.car-card');
            carCard.querySelector('#carBrand').textContent = entity.configuration.brandName;
            carCard.querySelector('#carModel').textContent = entity.configuration.autoModelName;
            carCard.querySelector('#carBodyType').textContent = entity.configuration.bodyType;
            carCard.querySelector('#carEquipmentName').textContent = entity.configuration.equipment.name;
            carCard.querySelector('#carReleaseYear').textContent = entity.configuration.equipment.releaseYear;
            carCard.querySelector('#carTotalPrice').textContent = "Цена: " + entity.configuration.totalPrice + "₽";
            carCard.querySelector('#carEngine').textContent = entity.configuration.engine.power + " л.с.";
            carCard.querySelector('#carImg').src = `http://localhost:7243/api/File/getImage/${entity.configuration.id}`;
            carCard.querySelector('#carTransmission').textContent = entity.configuration.engine.transmissionType;
            article.addEventListener('click', function() {
                window.location.href = `/carPage/carPage.html?carId=${entity.id}`;
            });
            catalog.appendChild(carCard);
        });
    }
}