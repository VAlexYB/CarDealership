let entity = {};

document.addEventListener("DOMContentLoaded", async function() {
    await import('../scripts/navbarCustomer.js');
    await import('https://cdn.jsdelivr.net/npm/jwt-decode/build/jwt-decode.min.js')
    const queryParams = new URLSearchParams(window.location.search);
    const carId = queryParams.get('carId');
    if (!carId) {
        console.error('ID машины не указан.');
        return;
    }
    await getCar(carId);
});

const getCar = async (carId) => {
    const response = await fetch(`http://localhost:7243/api/Cars/getById/${carId}`, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        },
        credentials: 'include',
    });
    entity = await response.json();
    document.getElementById('carTitle').textContent = entity.configuration.brandName + " " + entity.configuration.autoModelName
        + " " + entity.configuration.equipment.releaseYear; 
    document.getElementById('carTotalPrice').innerHTML = entity.configuration.totalPrice + "<b>₽</b>";
    document.getElementById('carCountry').textContent = entity.configuration.countryName;
    document.getElementById('carEquipmentName').textContent = entity.configuration.equipment.name;
    document.getElementById('carReleaseYear').textContent = entity.configuration.equipment.releaseYear;
    document.getElementById('carDriveType').textContent = entity.configuration.driveType;
    document.getElementById('carBodyType').textContent = entity.configuration.bodyType;
    document.getElementById('carTransmissionType').textContent = entity.configuration.engine.transmissionType;
    document.getElementById('carEnginePower').textContent = entity.configuration.engine.power;
    document.getElementById('carEngineType').textContent = entity.configuration.engine.engineType;
    document.getElementById('carConsumption').textContent = entity.configuration.engine.consumption;
    document.getElementById('carColor').textContent = entity.configuration.color;
    document.getElementById('carImg').src = `http://localhost:7243/api/File/getImage/${entity.configuration.id}`;
    document.getElementById('carFeatures').textContent = entity.configuration.equipment.features.map(feature => feature.description).join(', ');
}

const orderCar = async () => {
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
        return;
    }
    const decoded = jwt_decode(alterTroubleSuckyKey);
    const order = {
        customerId: decoded.userId,
        autoConfigurationId: entity.configuration.id,
        status: 1,
        orderDate: new Date().toISOString()
    }
    const response = await fetch('http://localhost:7243/api/Orders/add', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        },
        body: JSON.stringify(order),
        credentials: 'include',
    })
    if (response.ok) {
        alert('Заказ оформлен');
        window.location.href = `../catalog/catalog.html`;
    }
    else {
        console.log(response);
        alert('Заказ не оформлен');
    }
}