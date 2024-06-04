document.addEventListener("DOMContentLoaded", async function() {
    await import('../scripts/navbarManager.js');
    await getCountries();
    await getEntities();
});

const getCountries = async () => {
    const response = await fetch("https://localhost:7243/api/AutoConfig/getAll", {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        }
    });
    const entities = (await response.json());
    if (entities.length === 0) return;
    const configSelect = document.getElementById('config');
    entities.forEach(entity => {
        const option = document.createElement('option');
        option.value = entity.id;
        option.textContent = entity.brandName + " " + entity.modelName + " " + entity.equipmentName; //?????
        configSelect.appendChild(option);
    });
}

const getEntities = async () => {
    const response = await fetch("https://localhost:7243/api/Cars/getAll", {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        }
    });
    const entities = await response.json();
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
        entityCard.querySelector('#vinTable').textContent = entity.vin;
        entityCard.querySelector('#configTable').textContent = entity.autoConfigurationId;
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
        document.getElementById('vin').value = entity.vin;
        const config = document.getElementById('config');
        const configOptions = config.querySelectorAll('option');
        for (let i = 0; i < configOptions.length; i++) {
            if (configOptions[i].value === entity.configId) {
                configOptions[i].selected = true;
                break;
            }
        }
        document.getElementById('dialogTitle').textContent = 'Редактировать машину';
        document.getElementById('dialogSubmitBtn').textContent = 'Сохранить';
        entityForm.onsubmit = (event) => {
            event.preventDefault();
            addOrEditEntity(entity);
        };;
    }
    else {
        document.getElementById('dialogTitle').textContent = 'Добавить машину';
        document.getElementById('dialogSubmitBtn').textContent = 'Добавить';
        entityForm.onsubmit = (event) => {
            event.preventDefault();
            addOrEditEntity();
        };
    }
    
}

const closeEntityDialog = () => {
    const entityForm = document.getElementById('entityForm');
    document.getElementById('vin').value = "";
    const config = document.getElementById('config');
    const configOptions = config.querySelectorAll('option');
    for (let i = 0; i < configOptions.length; i++) {
        configOptions[i].selected = false;
    }
    entityForm.onsubmit = null;
    document.getElementById('addEntityDialog').close();
}

const deleteEntity = async (entity) => {
    await fetch(`https://localhost:7243/api/Cars/deleteById/${entity.id}`);
    await getEntities();
}

const addOrEditEntity = async (entity = null) => {
    const name = document.getElementById('name').value;
    const config = document.getElementById('config');
    const configOptions = config.querySelectorAll('option');
    const configId = configOptions[config.selectedIndex].value;
    if (name === '' || config === '') {
        alert('Поля не должны быть пустыми');
        return;
    }
    let newEntity = {};
    if (entity) {
        newEntity = {
            "id": entity.id,
            "vin": vin,
            "autoConfigurationId": configId
        }
    } else {
        newEntity = {
            "vin": vin,
            "autoConfigurationId": configId
        }
    }
    await fetch("https://localhost:7243/api/Cars/add", {
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