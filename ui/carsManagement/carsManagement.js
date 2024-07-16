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
    await getAutoConfigs();
    await getEntities();
});

const getAutoConfigs = async () => {
    const response = await fetch("http://localhost:7243/api/AutoConfig/getAll", {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        },
        credentials: 'include',
    });
    const entities = await response.json();
    console.log(entities);
    const configSelect = document.getElementById('config');
    let child = configSelect.firstChild;
    while (child) {
        const nextSibling = child.nextSibling; 
        if (child.tagName !== 'TEMPLATE') configSelect.removeChild(child); 
        child = nextSibling; 
    }
    entities.forEach(entity => {
        const option = document.createElement('option');
        option.value = entity.id;
        option.textContent = entity.brandName + " " + entity.autoModelName + ", " + entity.bodyType + ", " + entity.color + ", "  + entity.equipment.name
            + ", " + entity.equipment.releaseYear + ", " + entity.totalPrice + "₽";
        configSelect.appendChild(option);
    });
}

const getEntities = async () => {
    const response = await fetch("http://localhost:7243/api/Cars/getAll", {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        },
        credentials: 'include',
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
    console.log(entities);
    entities.forEach(entity => {
        entitiesCount++;
        const entityCard = entitiesCardTemplate.content.cloneNode(true);
        entityCard.querySelector('#id').textContent = entitiesCount;
        entityCard.querySelector('#vinTable').textContent = entity.vin;
        entityCard.querySelector('#configTable').textContent = entity.configuration.brandName + " " + entity.configuration.autoModelName + ", "
             + entity.configuration.bodyType + ", " + entity.configuration.color + ", "  + entity.configuration.equipment.name
            + ", " + entity.configuration.equipment.releaseYear + ", " + entity.configuration.price + "₽";
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
            if (configOptions[i].value === entity.configuration.id) {
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
    await fetch(`http://localhost:7243/api/Cars/deleteById/${entity.id}`, {
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
    const vin = document.getElementById('vin').value;
    const config = document.getElementById('config');
    const configOptions = config.querySelectorAll('option');
    const configId = configOptions[config.selectedIndex].value;
    if (vin === '' || configId === '') {
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
    const response = await fetch("http://localhost:7243/api/Cars/add", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        },
        body: JSON.stringify(newEntity),
        credentials: 'include',
    })
    if (!response.ok) {
        const error = await response.json()
        alert(error);
        return;
    }
    closeEntityDialog();
    await getEntities();
}