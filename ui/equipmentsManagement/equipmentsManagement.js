let brands = [];

document.addEventListener("DOMContentLoaded", async function() {
    await import('../scripts/navbarManager.js');
    await import('https://cdn.jsdelivr.net/npm/jwt-decode/build/jwt-decode.min.js');
    function getCookie(name) {
        let cookieValue = null;
        if (document.cookie && document.cookie !== '') {
          const cookies = document.cookie.split(';');
          for (let i = 0; i < cookies.length; i++) {
            const cookie = cookies[i].trim();
            // Если начало строки совпадает с искомым именем, берём значение
            if (cookie.substring(0, name.length + 1) === (name + '=')) {
              cookieValue = decodeURIComponent(cookie.substring(name.length + 1));
              break;
            }
          }
        }
        return cookieValue;
      }
      
    const alterTroubleSuckyKey = getCookie('altertroublesuckykey');
    if (!alterTroubleSuckyKey) {
        alert('Вы не авторизованы');
        window.location.href = "../login/login.html";
    }
    const decoded = jwt_decode(alterTroubleSuckyKey);
    if (decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] !== 'Admin' &&  
        !(decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] === 'Manager' || 
         decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"].some(role => role === "Manager"))) {
        alert('Вы не админ или менеджер');
        window.location.href = "../login/login.html";
    }
    if (decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] === 'Manager') {
        await import('../scripts/navbarManager.js');
    }
    else if (decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] === 'Admin') {
        await import('../scripts/navbarAdmin.js');
    }
    else {
        await import('../scripts/navbarSeniorManager.js');
    }
    await getBrands();
    await getFeatures();
    await getEntities();
});

const getFeatures = async () => {
    const response = await fetch("http://localhost:7243/api/Features/getAll", {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        },
        credentials: 'include',
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
    entities.forEach(entity => {
        const option = document.createElement('option');
        option.value = entity.id;
        option.textContent = entity.name;
        modelSelect.appendChild(option);
    });
}

const getEntities = async () => {
    const response = await fetch("http://localhost:7243/api/Equipments/getAll", {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        },
        credentials: 'include',
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
        entityCard.querySelector('#brandTable').textContent = entity.brandName;
        entityCard.querySelector('#modelTable').textContent = entity.autoModelName;
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
        const featuresOptions = document.getElementById('features').querySelectorAll('option');
        for (let i = 0; i < featuresOptions.length; i++) {
            if (entity.features.map(e => e.id).includes(featuresOptions[i].value)) {
                featuresOptions[i].selected = true;
            }
        }
        const modelSelect = document.getElementById('model');
        const modelOptions = modelSelect.querySelectorAll('option');
        for (let i = 0; i < modelOptions.length; i++) {
            if (entity.autoModelId === modelOptions[i].value) {
                modelOptions[i].selected = true;
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
    await fetch(`http://localhost:7243/api/Equipments/deleteById/${entity.id}`, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        },
        credentials: 'include',
    });
    await getEntities();
}

const addOrEditEntity = async (entity = null) => {
    const name = document.getElementById('name').value;
    const model = document.getElementById('model');
    const modelOptions = model.querySelectorAll('option');
    const price = document.getElementById('price').value;
    const releaseYear = document.getElementById('releaseYear').value;
    const features = document.getElementById('features');
    const featuresOptions = features.querySelectorAll('option');
    let featuresIds = [];
    for (let i = 0; i < featuresOptions.length; i++) {
        if (featuresOptions[i].selected) {
            featuresIds.push(featuresOptions[i].value);
        }
    }
    let modelId;
    try {
        modelId = modelOptions[model.selectedIndex].value;
    }
    catch (error) {
        alert("Выберите модель");
        return;
    }
    if (name === '' || price === '' || releaseYear === '' || featuresIds.length === 0) {
        alert('Поля не должны быть пустыми');
        return;
    }
    let newEntity = {};
    if (entity) {
        const oldFeatureIds = entity.features.map(e => e.id);
        oldFeatureIds.forEach(async id => {
            if (!featuresIds.includes(id)) {
                await fetch(`http://localhost:7243/api/Equipments/removeFeature`, {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                        "Accept": "application/json"
                    },
                    body: JSON.stringify({ "featureId": id, "equipmentId": entity.id }),
                    credentials: 'include',
                });
            }
        });
        featuresIds.forEach(async id => {
            if (!oldFeatureIds.includes(id)) {
                await fetch(`http://localhost:7243/api/Equipments/addFeature`, {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                        "Accept": "application/json"
                    },
                    body: JSON.stringify({ "featureId": id, "equipmentId": entity.id }),
                    credentials: 'include',
                });
            }
        });
        newEntity = {
            "id": entity.id,
            "name": name,
            "price": price,
            "autoModelId": modelId,
            "releaseYear": releaseYear
        }

        await fetch("http://localhost:7243/api/Equipments/add", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            },
            body: JSON.stringify(newEntity),
            credentials: 'include',
        });
    } else {
        newEntity = {
            "name": name,
            "price": price,
            "autoModelId": modelId,
            "releaseYear": releaseYear,
            "featureIds": featuresIds  
        }
        console.log(newEntity);
        await fetch("http://localhost:7243/api/Equipments/add", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        },
        body: JSON.stringify(newEntity),
        credentials: 'include',
        });
    }
    closeEntityDialog();
    await getEntities();
}