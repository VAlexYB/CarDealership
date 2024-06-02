document.addEventListener("DOMContentLoaded", async function() {
    await import('../scripts/navbarManager.js');
    await getEntities();
});

const getEntities = async () => {
    const response = await fetch("https://localhost:7243/api/AutoConfig/getAll", {
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
        entityCard.querySelector('#nameTable').textContent = entity.name;
        entityCard.querySelector('#brandTable').textContent = entity.brand;
        entityCard.querySelector('#priceTable').textContent = entity.price + "₽";
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
        const brand = document.getElementById('brand');
        const brandOptions = brand.querySelectorAll('option');
        for (let i = 0; i < brandOptions.length; i++) {
            if (brandOptions[i].value === entity.brandId) {
                brandOptions[i].selected = true;
                break;
            } 
        };
        document.getElementById('price').value = entity.price;
        document.getElementById('dialogTitle').textContent = 'Редактировать модель';
        document.getElementById('dialogSubmitBtn').textContent = 'Сохранить';
        entityForm.onsubmit = (event) => {
            event.preventDefault();
            addOrEditEntity(entity);
        };;
    }
    else {
        document.getElementById('dialogTitle').textContent = 'Добавить модель';
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
    const brand = document.getElementById('brand');
    const brandOptions = brand.querySelectorAll('option');
    for (let i = 0; i < brandOptions.length; i++) {
        brandOptions[i].selected = false;
    };
    document.getElementById('price').value = "";
    entityForm.onsubmit = null;
    document.getElementById('addEntityDialog').close();
}

const deleteEntity = async (entity) => {
    await fetch(`https://localhost:7243/api/AutoModels/deleteById/${entity.id}`);
    await getEntities();
}

const addOrEditEntity = async (entity = null) => {
    const name = document.getElementById('name').value;
    const brand = document.getElementById('brand');
    const brandOptions = brand.querySelectorAll('option');
    const brandId = brandOptions[brand.selectedIndex].value;
    const price = document.getElementById('price').value;
    if (name === '' || price === '' || brandId === '') {
        alert('Поля не должны быть пустыми');
        return;
    }
    let newEntity = {};
    if (entity) {
        newEntity = {
            "id": entity.id,
            "name": name,
            "brandId": brandId,
            "price": price
        }
    } else {
        newEntity = {
            "name": name,
            "brandId": brandId,
            "price": price
        }
    }
    await fetch("https://localhost:7243/api/AutoModels/add", {
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