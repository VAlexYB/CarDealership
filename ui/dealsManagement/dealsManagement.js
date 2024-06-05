let userId;

document.addEventListener("DOMContentLoaded", async function() {
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
    userId = decoded.userId;
    await getFreeCars();
    await getUsers()
    await getEntities();
});

const getFreeCars = async () => {
    const carsResponse = await fetch("http://localhost:7243/api/Cars/getAll", {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        },
        credentials: 'include',
    });
    const cars = (await carsResponse.json())
    if (cars.length === 0) return;
    const dealsResponse = await fetch("http://localhost:7243/api/Deals/getAll", {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        },
        credentials: 'include'
    })
    const deals = (await dealsResponse.json())
    let carsList = [];
    cars.forEach(car => {
        let isFree = true;
        deals.forEach(deal => {
            if (deal.carId === car.id) {
                isFree = false;
                return;
            }
        })
        if (isFree) carsList.push(car);
    })
    const carsSelect = document.getElementById('car');
    let child = carsSelect.firstChild;
    while (child) {
        const nextSibling = child.nextSibling; 
        if (child.tagName !== 'TEMPLATE') carsSelect.removeChild(child); 
        child = nextSibling; 
    }
    carsList.forEach(car => {
        const option = document.createElement('option');
        option.value = car.id;
        option.textContent = car.configuration.brandName + " " + car.configuration.autoModelName + ", " + car.vin;
        carsSelect.appendChild(option);
    });
}

const getUsers = async () => {
    const response = await fetch("http://localhost:7243/api/Users/getUsers", {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        },
        credentials: 'include',
    });
    const entities = (await response.json());
    console.log(entities)
    const usersSelect = document.getElementById('customer');
    entities.forEach(entity => {
        const option = document.createElement('option');
        option.value = entity.id;
        option.textContent =  entity.userName;
        usersSelect.appendChild(option);
    });
}

// const getFreeOrders = async () => {
//     const response = await fetch("http://localhost:7243/api/Orders/getFreeOrders", {
//         method: "GET",
//         headers: {
//             "Content-Type": "application/json",
//             "Accept": "application/json"
//         },
//         credentials: 'include',
//     });
//     const entities = (await response.json())
//     if (entities.length === 0) return;
//     const brandSelect = document.getElementById('brand');
//     entities.forEach(entity => {
//         const option = document.createElement('option');
//         option.value = entity.id;
//         option.textContent = entity.name;
//         brandSelect.appendChild(option);
//     });
// }

const getEntities = async () => {
    const response = await fetch("http://localhost:7243/api/Deals/getByFilter", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        },
        credentials: 'include',
        body: JSON.stringify({
            managerId: userId
        })
    });
    const entities = (await response.json())
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
    console.log(entities);
    let entitiesCount = 0;
    const entitiesCardTemplate = document.getElementById('entitiesCardTemplate');
    entities.forEach(entity => {
        entitiesCount++;
        const entityCard = entitiesCardTemplate.content.cloneNode(true);
        entityCard.querySelector('#id').textContent = entitiesCount;
        entityCard.querySelector('#carTable').textContent = entity.car.configuration.brandName + " " + entity.car.configuration.autoModelName + " " 
            + entity.car.configuration.equipment.name + " " + entity.car.configuration.equipment.releaseYear; ;
        entityCard.querySelector('#dealDateTable').textContent = entity.dealDate.split('T')[0];
        entityCard.querySelector('#dealStatusTable').textContent = entity.status === 1 ? "На согласовании" : 
                                                                    entity.status === 2 ? "Одобрена" :
                                                                    entity.status === 3 ? "Отклонена" : "Завершена"; 
        if (entity.status !== 3 && entity.status !== 4) {
            entityCard.querySelector('#actionsTable').innerHTML = `
            <img src="../assets/img/update-icon.png" width="20px" height="20px" alt="" class="update-icon-img cursor-pointer">
        `
        entityCard.querySelector('.update-icon-img').addEventListener('click', () => {
            openEntityDialog(entity);
        });
        }
        
        entitiesTableBody.appendChild(entityCard);
    });

}

const openEntityDialog = (entity = null) => {
    document.getElementById('addEntityDialog').showModal();
    const entityForm = document.getElementById('entityForm');
    if (entity) {
        const carSelect = document.getElementById('car');
        const carSelectOptions = carSelect.querySelectorAll('option');
        for (let i = 0; i < carSelectOptions.length; i++) {
            if (carSelectOptions[i].value === entity.carId) {
                carSelectOptions[i].selected = true;
                break;
            } 
        };
        const customerSelect = document.getElementById('customer');
        const customerSelectOptions = customerSelect.querySelectorAll('option');
        for (let i = 0; i < customerSelectOptions.length; i++) {
            if (customerSelectOptions[i].value === entity.customerId) {
                customerSelectOptions[i].selected = true;
                break;
            }
        }
        const dealStatusSelect = document.getElementById('dealStatus');
        const dealStatusSelectOptions = dealStatusSelect.querySelectorAll('option');
        for (let i = 0; i < dealStatusSelectOptions.length; i++) {
            if (dealStatusSelectOptions[i].value === entity.status) {
                dealStatusSelectOptions[i].selected = true;
                break;
            }
        }
        document.getElementById('dialogTitle').textContent = 'Редактировать сделку';
        document.getElementById('dialogSubmitBtn').textContent = 'Сохранить';
        entityForm.onsubmit = (event) => {
            event.preventDefault();
            addOrEditEntity(entity);
        };
    }
    else {
        document.getElementById('dialogTitle').textContent = 'Добавить сделку';
        document.getElementById('dialogSubmitBtn').textContent = 'Добавить';
        entityForm.onsubmit = (event) => {
            event.preventDefault();
            addOrEditEntity();
        };
    }
}

const closeEntityDialog = () => {
    const entityForm = document.getElementById('entityForm');
    const carSelect = document.getElementById('car');
    const carSelectOptions = carSelect.querySelectorAll('option');
    for (let i = 0; i < carSelectOptions.length; i++) {
        carSelectOptions[i].selected = false;
    };
    const customerSelect = document.getElementById('customer');
    const customerSelectOptions = customerSelect.querySelectorAll('option');
    for (let i = 0; i < customerSelectOptions.length; i++) {
        customerSelectOptions[i].selected = false;
    }
    const dealStatusSelect = document.getElementById('dealStatus');
    const dealStatusSelectOptions = dealStatusSelect.querySelectorAll('option');
    for (let i = 0; i < dealStatusSelectOptions.length; i++) {
        dealStatusSelectOptions[i].selected = false;
    }
    entityForm.onsubmit = null;
    document.getElementById('addEntityDialog').close();
}

const addOrEditEntity = async (entity = null) => {
    const car = document.getElementById('car');
    const carOptions = car.querySelectorAll('option');
    const carId = carOptions[car.selectedIndex].value;
    const customer = document.getElementById('customer');
    const customerOptions = customer.querySelectorAll('option');
    const customerId = customerOptions[customer.selectedIndex].value;
    const dealStatus = document.getElementById('dealStatus');
    const dealStatusOptions = dealStatus.querySelectorAll('option');
    const status = dealStatusOptions[dealStatus.selectedIndex].value;
    if (!carId || !customerId || !status) {
        alert('Поля не должны быть пустыми');
        return;
    }
    let newEntity = {};
    if (entity) {
        newEntity = {
            "id": entity.id,
            "dealDate": entity.dealDate,
            "carId": carId,
            "customerId": customerId,
            "status": +status,
            "managerId": userId
        }
    } else {
        newEntity = {
            "dealDate": new Date(),
            "carId": carId,
            "customerId": customerId,
            "status": +status,
            "managerId": userId
        }
    }
    const response = await fetch("http://localhost:7243/api/Deals/add", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        },
        body: JSON.stringify(newEntity),
        credentials: 'include',
    });
    if (!response.ok) {
        const error = await response.json()
        alert(error);
        return;
    }
    closeEntityDialog();
    await getFreeCars()
    await getEntities();
}