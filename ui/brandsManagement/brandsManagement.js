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
    await getCountries();
    await getEntities();
});

const getCountries = async () => {
    const response = await fetch("http://localhost:7243/api/Countries/getAll", {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        },
        credentials: 'include',
    });
    const entities = (await response.json()).sort((a, b) => a.name.localeCompare(b.name));
    if (entities.length === 0) return;
    const countrySelect = document.getElementById('country');
    entities.forEach(entity => {
        const option = document.createElement('option');
        option.value = entity.id;
        option.textContent = entity.name;
        countrySelect.appendChild(option);
    });
}

const getEntities = async () => {
    const response = await fetch("http://localhost:7243/api/Brands/getAll", {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        },
        credentials: 'include',
    });
    const entities = (await response.json()).sort((a, b) => a.name.localeCompare(b.name));
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
        entityCard.querySelector('#countryTable').textContent = entity.country;
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
        const country = document.getElementById('country');
        const countryOptions = country.querySelectorAll('option');
        for (let i = 0; i < countryOptions.length; i++) {
            if (countryOptions[i].value === entity.countryId) {
                countryOptions[i].selected = true;
                break;
            }
        }
        document.getElementById('dialogTitle').textContent = 'Редактировать бренд';
        document.getElementById('dialogSubmitBtn').textContent = 'Сохранить';
        entityForm.onsubmit = (event) => {
            event.preventDefault();
            addOrEditEntity(entity);
        };;
    }
    else {
        document.getElementById('dialogTitle').textContent = 'Добавить бренд';
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
    const country = document.getElementById('country');
    const countryOptions = country.querySelectorAll('option');
    for (let i = 0; i < countryOptions.length; i++) {
        countryOptions[i].selected = false;
    }
    entityForm.onsubmit = null;
    document.getElementById('addEntityDialog').close();
}

const deleteEntity = async (entity) => {
    await fetch(`http://localhost:7243/api/Brands/deleteById/${entity.id}`, {
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
    const country = document.getElementById('country');
    const countryOptions = country.querySelectorAll('option');
    const countryId = countryOptions[country.selectedIndex].value;
    if (name === '' || country === '') {
        alert('Поля не должны быть пустыми');
        return;
    }
    let newEntity = {};
    if (entity) {
        newEntity = {
            "id": entity.id,
            "name": name,
            "countryId": countryId
        }
    } else {
        newEntity = {
            "name": name,
            "countryId": countryId
        }
    }
    const response = await fetch("http://localhost:7243/api/Brands/add", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        },
        body: JSON.stringify(newEntity)
    });
    if (!response.ok) {
        const error = await response.json()
        alert(error);
        return;
    }
    closeEntityDialog();
    await getEntities();
}