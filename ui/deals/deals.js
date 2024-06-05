
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
    await getDeals(decoded.userId);
});

const getDeals = async (userId) => {
    const response = await fetch("http://localhost:7243/api/Deals/getByFilter", {
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
    console.log(entities)
    const deals = document.getElementById('deals');
    let child = deals.firstChild;
    while (child) {
        const nextSibling = child.nextSibling; 
        if (child.tagName !== 'TEMPLATE') deals.removeChild(child); 
        child = nextSibling; 
    }
    if (entities.length === 0) {
        alert('Сделки не найдены');
        return;
    }
    else {
        const entityCardTemplate = document.getElementById('entityCardTemplate');
        entities.forEach(entity => {
            const entityCard = entityCardTemplate.content.cloneNode(true);
            entityCard.querySelector('#carTitle').textContent = entity.car.configuration.brandName + " " + entity.car.configuration.autoModelName
                + " " + entity.car.configuration.equipment.name + " " + entity.car.configuration.equipment.releaseYear; 
            entityCard.querySelector('#carBodyType').textContent = entity.car.configuration.bodyType;
            entityCard.querySelector('#carDriveType').textContent = entity.car.configuration.driveType;
            entityCard.querySelector('#carTransmissionType').textContent = entity.car.configuration.engine.transmissionType;
            entityCard.querySelector('#carEnginePower').textContent = entity.car.configuration.engine.power;
            entityCard.querySelector('#carConsumption').textContent = entity.car.configuration.engine.consumption;
            entityCard.querySelector('#carColor').textContent = entity.car.configuration.color;
            entityCard.querySelector('#dealPrice').textContent = "Стоимость сделки: " + entity.price + "₽";
            entityCard.querySelector('#carImg').src = `http://localhost:7243/api/File/getImage/${entity.car.configuration.id}`;
            entityCard.querySelector('#carTotalPrice').innerHTML =  entity.car.configuration.totalPrice + "₽";
            entityCard.querySelector('#dealDate').textContent += entity.dealDate.split('T')[0];
            entityCard.querySelector('#dealStatus').textContent += entity.status === 1 ? "Согласование" : 
                                                                entity.status === 2 ? "Одобрена" :
                                                                entity.status === 3 ? "Отклонена" : "Завершена";
            deals.appendChild(entityCard);
        });
    }
}