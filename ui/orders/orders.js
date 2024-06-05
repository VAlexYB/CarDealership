let userId;

document.addEventListener("DOMContentLoaded", async function() {
    await import('../scripts/navbarCustomer.js');
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
    userId = decoded.userId;
    await getOrders(userId);
});

const getOrders = async (userId) => {
    const response = await fetch("http://localhost:7243/api/Orders/getByFilter", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        },
        credentials: 'include',
        body: JSON.stringify({
            customerId: userId
        })
    });
    const entities = await response.json();
    console.log(entities);
    const orders = document.getElementById('orders');
    let child = orders.firstChild;
    while (child) {
        const nextSibling = child.nextSibling; 
        if (child.tagName !== 'TEMPLATE') orders.removeChild(child); 
        child = nextSibling; 
    }
    console.log(entities);
    if (entities.length === 0) {
        alert('Заказы не найдены');
        return;
    }
    else {
        const entityCardTemplate = document.getElementById('entityCardTemplate');
        entities.forEach(entity => {
            const entityCard = entityCardTemplate.content.cloneNode(true);
            const img = entityCard.querySelector('div img');
            entityCard.querySelector('#carTitle').textContent = entity.autoConfiguration.brandName + " " + entity.autoConfiguration.autoModelName
                + " " + entity.autoConfiguration.equipment.name + " " + entity.autoConfiguration.equipment.releaseYear; 
            entityCard.querySelector('#carBodyType').textContent = entity.autoConfiguration.bodyType;
            entityCard.querySelector('#carDriveType').textContent = entity.autoConfiguration.driveType;
            entityCard.querySelector('#carTransmissionType').textContent = entity.autoConfiguration.engine.transmissionType;
            entityCard.querySelector('#carEnginePower').textContent = entity.autoConfiguration.engine.power;
            entityCard.querySelector('#carConsumption').textContent = entity.autoConfiguration.engine.consumption;
            entityCard.querySelector('#carColor').textContent = entity.autoConfiguration.color;
            entityCard.querySelector('#carTotalPrice').innerHTML = entity.autoConfiguration.totalPrice + "<b>₽</b>";
            entityCard.querySelector('#orderDate').textContent += entity.orderDate.split('T')[0];
            entityCard.querySelector('#orderPrice').textContent += entity.price + "₽";
            entityCard.querySelector('#carImg').src = `http://localhost:7243/api/File/getImage/${entity.autoConfiguration.id}`;
            entityCard.querySelector('#deliveryDate').textContent += (entity.completeDate !== '0001-01-01T00:00:00' ? entity.completeDate.split('T')[0] : "Не доставлено");
            entityCard.querySelector('#orderStatus').textContent += entity.status === 1 ? "Ожидание" : 
                                                                entity.status === 2 ? "В обработке" :
                                                                entity.status === 3 ? "Доставлен" : "Отменен";
            if (entity.status !== 3 && entity.status !== 4) {
                entityCard.querySelector('#cancelOrder').classList.remove('d-none');
                entityCard.querySelector('#cancelOrder').addEventListener('click', () => {
                    cancelOrder(entity.id);
                });
            }
            orders.appendChild(entityCard);
        });
    }
}

const cancelOrder = async (orderId) => {
    const response = await fetch(`http://localhost:7243/api/Orders/cancelOrder/${orderId}`, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        },
        credentials: 'include'
    });
    if (!response.ok) {
        alert('Не удалось отменить заказ');
        return;
    }
    await getOrders(userId);
    window.location.reload();
}