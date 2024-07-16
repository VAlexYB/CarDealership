let userId;

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
    userId = decoded.userId;
    await getEntities();
});

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
    const response = await fetch("http://localhost:7243/api/Orders/getByFilter", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        },
        credentials: 'include',
        body: JSON.stringify({managerId: userId})
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
        entityCard.querySelector('#carTable').textContent = entity.autoConfiguration.brandName + " " + entity.autoConfiguration.autoModelName + " " 
        + entity.autoConfiguration.equipment.name + " " + entity.autoConfiguration.equipment.releaseYear; ;
        entityCard.querySelector('#orderDateTable').textContent = entity.orderDate.split('T')[0];
        entityCard.querySelector('#deliveryDateTable').textContent = (entity.completeDate !== '0001-01-01T00:00:00' ? entity.completeDate.split('T')[0] : "Не доставлено");
        entityCard.querySelector('#orderStatusTable').textContent = entity.status === 1 ? "Ожидание" : 
                                                                    entity.status === 2 ? "В обработке" :
                                                                    entity.status === 3 ? "Доставлен" : "Отменен"; 
        if (entity.status === 2) {
            entityCard.querySelector('#actionsTable').innerHTML = `
            <img src="../assets/img/update-icon.png" alt="" class="update-icon-img cursor-pointer">
            <img src="../assets/img/delete-icon.png" alt="" class="delete-icon-img cursor-pointer">
            `
            entityCard.querySelector('.update-icon-img').addEventListener('click', () => {
                openEntityDialog(entity);
            });
            entityCard.querySelector('.delete-icon-img').addEventListener('click', () => {
                leaveOrder(entity);
            });
        }
        entitiesTableBody.appendChild(entityCard);
    });

}
const changeStatus = async (entity) => {
    const orderStatus = document.getElementById('orderStatus');
    const orderStatusOptions = orderStatus.querySelectorAll('option');
    const status = orderStatusOptions[orderStatus.selectedIndex].value;
    console.log(entity)
    if (status == 3) {
        const newEntity = {
            "id": entity.id ,
            "status": 3
        }
        console.log(newEntity)
        const deliverOrder = await fetch("http://localhost:7243/api/Orders/changeStatus", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            },
            credentials: 'include',
            body: JSON.stringify(newEntity)
        })
        if (!deliverOrder.ok) {
            alert("Заказ " + entity.id + " не доставлен");
        }
    } else {
        const cancelOrder = await fetch("http://localhost:7243/api/Orders/changeStatus", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            },
            credentials: 'include',
            body: JSON.stringify({
                "id": entity.id, 
                "status": 4
            })
        })
        if (!cancelOrder.ok) {
            alert("Заказ " + entity.id + " не отменен");
        }
    }
    await getEntities()
}

const leaveOrder = async (entity) => {
    const response = await fetch(`http://localhost:7243/api/Orders/leaveOrder/${entity.id}`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        },
        credentials: 'include',
        body: JSON.stringify({
            "orderId": entity.id
        })
    })
    if (!response.ok) {
        alert("Заказ " + entity.id + " не убран из обработки");
    }
    await getEntities();
}

const openEntityDialog = (entity) => {
    document.getElementById('addEntityDialog').showModal();
    const entityForm = document.getElementById('entityForm');
    if (entity) {
        const orderStatus = document.getElementById('orderStatus');
        const orderStatusOptions = orderStatus.querySelectorAll('option');
        for (let i = 0; i < orderStatusOptions.length; i++) {
            if (orderStatusOptions[i].value === entity.brandId) {
                orderStatusOptions[i].selected = true;
                break;
            } 
        };
        document.getElementById('dialogTitle').textContent = 'Заказ';
        document.getElementById('dialogSubmitBtn').textContent = 'Сохранить';
        entityForm.onsubmit = (event) => {
            event.preventDefault();
            changeStatus(entity);
        };
    }
}

const closeEntityDialog = () => {
    const entityForm = document.getElementById('entityForm');
    const orderStatus = document.getElementById('orderStatus');
    const orderStatusOptions = orderStatus.querySelectorAll('option');
    for (let i = 0; i < orderStatusOptions.length; i++) {
        orderStatusOptions[i].selected = false;
    };
    entityForm.onsubmit = null;
    document.getElementById('addEntityDialog').close();
}
