document.addEventListener("DOMContentLoaded", async function() {
    await import('../scripts/navbarManager.js');
    await getTransmissions();
    await getEnginesTypes();
    await getEntities();
});

const getTransmissions = async () => {
    const response = await fetch("https://localhost:7243/api/TransmissionTypes/getAll", {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        }
    });
    const entities = await response.json();
    if (entities.length === 0) return;
    const transmissionTypeSelect = document.getElementById('transmissionType');
    entities.forEach(entity => {
        const option = document.createElement('option');
        option.value = entity.id;
        option.textContent = entity.value;
        transmissionTypeSelect.appendChild(option);
    });
}


const getEnginesTypes = async () => {
    const response = await fetch("https://localhost:7243/api/EngineTypes/getAll", {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        }
    });
    const entities = await response.json();
    if (entities.length === 0) return;
    const engineTypeSelect = document.getElementById('engineType');
    entities.forEach(entity => {
        const option = document.createElement('option');
        option.value = entity.id;
        option.textContent = entity.value;
        engineTypeSelect.appendChild(option);
    });
}

const getEntities = async () => {
    const response = await fetch("https://localhost:7243/api/Engines/getAll", {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        }
    });
    const entities = (await response.json()).sort((a, b) => a.price - b.price);;
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
        entityCard.querySelector('#powerTable').textContent = entity.power + " л.с.";
        entityCard.querySelector('#consumptionTable').textContent = entity.engineType === "Бензиновый" ? `${entity.consumption}л / 100 км`  : `${entity.consumption} кВт / 100 км`;
        entityCard.querySelector('#priceTable').textContent = entity.price + "₽";
        entityCard.querySelector('#engineTypeTable').textContent = entity.engineType;
        entityCard.querySelector('#transmissionTypeTable').textContent = entity.transmissionType;
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
        document.getElementById('power').value = entity.power;
        document.getElementById('consumption').value = entity.consumption;
        document.getElementById('engineType').value = entity.engineType;
        const engineType = document.getElementById('engineType');
        const engineTypeOptions = engineType.querySelectorAll('option');
        for (let i = 0; i < engineTypeOptions.length; i++) {
            if (engineTypeOptions[i].value === entity.engineTypeId) {
                engineTypeOptions[i].selected = true;
                break;
            } 
        };
        const transmissionType = document.getElementById('transmissionType');
        const transmissionTypeOptions = transmissionType.querySelectorAll('option');
        for (let i = 0; i < transmissionTypeOptions.length; i++) {
            if (transmissionTypeOptions[i].value === entity.transmissionTypeId) {
                transmissionTypeOptions[i].selected = true;
                break;
            } 
        };
        document.getElementById('price').value = entity.price;
        document.getElementById('dialogTitle').textContent = 'Редактировать двигатель';
        document.getElementById('dialogSubmitBtn').textContent = 'Сохранить';
        entityForm.onsubmit = (event) => {
            event.preventDefault();
            addOrEditEntity(entity);
        };;
    }
    else {
        document.getElementById('dialogTitle').textContent = 'Добавить двигатель';
        document.getElementById('dialogSubmitBtn').textContent = 'Добавить';
        entityForm.onsubmit = (event) => {
            event.preventDefault();
            addOrEditEntity();
        };
    }
    
}

const closeEntityDialog = () => {
    const entityForm = document.getElementById('entityForm');
    document.getElementById('power').value = "";
    document.getElementById('consumption').value = "";
    const engineType = document.getElementById('engineType');
    const engineTypeOptions = engineType.querySelectorAll('option');
    for (let i = 0; i < engineTypeOptions.length; i++) {
        engineTypeOptions[i].selected = false;
    };
    const transmissionType = document.getElementById('transmissionType');
    const transmissionTypeOptions = transmissionType.querySelectorAll('option');
    for (let i = 0; i < transmissionTypeOptions.length; i++) {
        transmissionTypeOptions[i].selected = false;
    };
    document.getElementById('price').value = "";
    entityForm.onsubmit = null;
    document.getElementById('addEntityDialog').close();
}

const deleteEntity = async (entity) => {
    await fetch(`https://localhost:7243/api/Engines/deleteById/${entity.id}`);
    await getEntities();
}

const addOrEditEntity = async (entity = null) => {
    const power = document.getElementById('power').value;
    const consumption = document.getElementById('consumption').value;
    const engineType = document.getElementById('engineType');
    const engineTypeOptions = engineType.querySelectorAll('option');
    const engineTypeId = engineTypeOptions[engineType.selectedIndex].value;
    const transmissionType = document.getElementById('transmissionType');
    const transmissionTypeOptions = transmissionType.querySelectorAll('option');
    const transmissionTypeId = transmissionTypeOptions[transmissionType.selectedIndex].value;
    const price = document.getElementById('price').value;
    if (power === '' || consumption === '' || price === '' || engineTypeId === '' || transmissionTypeId === '') {
        alert('Поля не должны быть пустыми');
        return;
    }
    let newEntity = {};
    if (entity) {
        newEntity = {
            "id": entity.id,
            "power": power,
            "consumption": consumption,
            "engineTypeId": engineTypeId,
            "transmissionTypeId": transmissionTypeId,
            "price": price
        }
    } else {
        newEntity = {
            "power": power,
            "consumption": consumption,
            "engineTypeId": engineTypeId,
            "transmissionTypeId": transmissionTypeId,
            "price": price
        }
    }
    await fetch("https://localhost:7243/api/Engines/add", {
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