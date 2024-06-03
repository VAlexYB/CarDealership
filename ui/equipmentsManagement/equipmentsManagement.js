document.addEventListener("DOMContentLoaded", async function() {
    await import('../scripts/navbarManager.js');
    await getModels();
    await getFeatures();
    await getEntities();
});

const getFeatures = async () => {
    const response = await fetch("https://localhost:7243/api/Features/getAll", {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        }
    });
    const features = await response.json();
    const featuresSelect = document.getElementById('features');
    features.forEach(feature => {
        const option = document.createElement('option');
        option.value = feature.id;
        option.textContent = feature.description;
        featuresSelect.appendChild(option);
    });
}

const getModels = async () => {
    const response = await fetch("https://localhost:7243/api/AutoModels/getAll", {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        }
    });
    const model = await response.json();
    const modelSelect = document.getElementById('model');
    model.forEach(model => {
        const option = document.createElement('option');
        option.value = model.id;
        option.textContent = model.name;
        modelSelect.appendChild(option);
    });
}

const getEntities = async () => {
    const response = await fetch("https://localhost:7243/api/Equipments/getAll", {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        }
    });
    const entities = (await response.json());
    const entitiesTableBody = document.getElementById('entitiesTableBody');
    let child = entitiesTableBody.firstChild;
    while (child) {
        const nextSibling = child.nextSibling; 
        if (child.tagName !== 'TEMPLATE') entitiesTableBody.removeChild(child); 
        child = nextSibling; 
    }
    if (entities.length === 0) {
        const noEntitiesCardTemplate = document.getElementById('noEntitiesCardTemplate');
        const noEntitiesCard = noEntitiesCardTemplate.content.cloneNode(true);
        noEntitiesCard.querySelector('#entitiesMessage').querySelector('td').textContent = "Ничего не нашлось";
        entitiesTableBody.appendChild(noEntitiesCard);
        return;
    }
    
    let entitiesCount = 0;
    const entitiesCardTemplate = document.getElementById('entitiesCardTemplate');
    entities.forEach(entity => {
        entitiesCount++;
        const entityCard = entitiesCardTemplate.content.cloneNode(true);
        entityCard.querySelector('#id').textContent = entitiesCount;
        entityCard.querySelector('#nameTable').textContent = entity.name;
        entityCard.querySelector('#priceTable').textContent = entity.price + "₽";
        entityCard.querySelector('#releaseYearTable').textContent = entity.releaseYear;
        entityCard.querySelector('#featuresTable').textContent = entity.features.map(e => e.description).join(', ');
        entityCard.querySelector('#actionsTable').innerHTML = `
            <img src="../assets/img/update-icon.png" alt="" class="update-icon-img cursor-pointer">
            <img src="../assets/img/delete-icon.png" alt="" class="delete-icon-img cursor-pointer">
        `
        entityCard.querySelector('.update-icon-img').addEventListener('click', () => {
            openEntityDialog(entity);
        });
        entityCard.querySelector('.delete-icon-img').addEventListener('click', () => {
            deleteEntity(entity);
        });
        entitiesTableBody.appendChild(entityCard);
    });

}

const openEntityDialog = (entity = null) => {
    document.getElementById('addEntityDialog').showModal();
    const entityForm = document.getElementById('entityForm');
    if (entity) {
        document.getElementById('name').value = entity.name;
        document.getElementById('price').value = entity.price;
        document.getElementById('releaseYear').value = entity.releaseYear;
        document.getElementById('features');
        const featuresOptions = document.getElementById('features').querySelectorAll('option');
        for (let i = 0; i < featuresOptions.length; i++) {
            if (entity.features.map(e => e.id).includes(featuresOptions[i].value)) {
                featuresOptions[i].selected = true;
            }
        }
        document.getElementById('dialogTitle').textContent = 'Редактировать комплектацию';
        document.getElementById('dialogSubmitBtn').textContent = 'Сохранить';
        entityForm.onsubmit = (event) => {
            event.preventDefault();
            addOrEditEntity(entity);
        };;
    }
    else {
        document.getElementById('dialogTitle').textContent = 'Добавить комплектацию';
        document.getElementById('dialogSubmitBtn').textContent = 'Добавить';
        entityForm.onsubmit = (event) => {
            event.preventDefault();
            addOrEditEntity();
        };
    }
}

const closeEntityDialog = () => {
    const entityForm = document.getElementById('entityForm');
    document.getElementById('name').value = "";
    document.getElementById('price').value = "";
    document.getElementById('releaseYear').value = "";
    const features = document.getElementById('features');
    const featuresOptions = features.querySelectorAll('option');
    for (let i = 0; i < featuresOptions.length; i++) {
        featuresOptions[i].selected = false;
    }
    entityForm.onsubmit = null;
    document.getElementById('addEntityDialog').close();
}

const deleteEntity = async (entity) => {
    await fetch(`https://localhost:7243/api/Equipments/deleteById/${entity.id}`);
    await getEntities();
}

const addOrEditEntity = async (entity = null) => {
    const name = document.getElementById('name').value;
    const model = document.getElementById('model');
    const modelOptions = model.querySelectorAll('option');
    const modelId = modelOptions[model.selectedIndex].value;
    const price = document.getElementById('price').value;
    const releaseYear = document.getElementById('releaseYear').value;
    const features = document.getElementById('features');
    const featuresOptions = features.querySelectorAll('option');
    const featuresIds = [];
    for (let i = 0; i < featuresOptions.length; i++) {
        if (featuresOptions[i].selected) {
            featuresIds.push(featuresOptions[i].value);
        }
    }
    if (name === '' || price === '' || releaseYear === '' || featuresIds.length === 0) {
        alert('Поля не должны быть пустыми');
        return;
    }
    let newEntity = {};
    if (entity) {
        newEntity = {
            "id": entity.id,
            "name": name,
            "autoModelId": modelId,
            "price": price,
            "releaseYear": releaseYear,
            "featuresIds": featuresIds
        }
    } else {
        newEntity = {
            "name": name,
            "price": price,
            "releaseYear": releaseYear,
            "featuresIds": featuresIds  
        }
    }
    await fetch("https://localhost:7243/api/Equipments/add", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        },
        body: JSON.stringify(newEntity)
    });
    closeEntityDialog();
    await getEntities();
}