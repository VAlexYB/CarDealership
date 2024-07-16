let configuration = {};
let brandsResponse = [];
let autoConfigurationId;

document.addEventListener("DOMContentLoaded", async function() {
    await import('../scripts/navbarCustomer.js');
    await import('https://cdn.jsdelivr.net/npm/jwt-decode/build/jwt-decode.min.js')
    window.location.href = "#";
    window.location.href = '#config_step_1';
    document.getElementById('config_step_2').classList.add('d-none');
    document.getElementById('config_step_3').classList.add('d-none');
    document.getElementById('config_step_4').classList.add('d-none');
    document.getElementById('config_step_5').classList.add('d-none');
    document.getElementById('config_step_6').classList.add('d-none');
    document.getElementById('config_step_7').classList.add('d-none');
    document.getElementById('configuratedCar').classList.add('d-none');
});

window.addEventListener('hashchange', async function() {
    const hash = window.location.hash;
    document.getElementById('config_step_1').classList.add('d-none');
    document.getElementById('config_step_2').classList.add('d-none');
    document.getElementById('config_step_3').classList.add('d-none');
    document.getElementById('config_step_4').classList.add('d-none');
    document.getElementById('config_step_5').classList.add('d-none');
    document.getElementById('config_step_6').classList.add('d-none');
    document.getElementById('config_step_7').classList.add('d-none');
    document.getElementById('configuratedCar').classList.add('d-none');
    if (hash == '#config_step_1') {
        document.getElementById('config_step_1').classList.remove('d-none');
        await getConfigurations(configuration, 1);
    }
    if (hash == '#config_step_2') {
        document.getElementById('config_step_2').classList.remove('d-none');
        await getConfigurations(configuration, 2);
    }
    if (hash == '#config_step_3') {
        document.getElementById('config_step_3').classList.remove('d-none');
        await getConfigurations(configuration, 3);
    }
    if (hash == '#config_step_4') {
        document.getElementById('config_step_4').classList.remove('d-none');
        await getConfigurations(configuration, 4);
    }
    if (hash == '#config_step_5') {
        document.getElementById('config_step_5').classList.remove('d-none');
        await getConfigurations(configuration, 5);
    }
    if (hash == '#config_step_6') {
        document.getElementById('config_step_6').classList.remove('d-none');
        await getConfigurations(configuration, 6);
    }
    if (hash == '#config_step_7') {
        document.getElementById('config_step_7').classList.remove('d-none');
        await getConfigurations(configuration, 7);
    }
    if (hash == '#configuratedCar') {
        document.getElementById('configuratedCar').classList.remove('d-none');
        await getConfigurations(configuration, 8);
    }
})

const getBrands = async () => {
    const response = await fetch('http://localhost:7243/api/Brands/getAll', {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        credentials: 'include',
    });
    brandsResponse = (await response.json()).sort((a, b) => a.name.localeCompare(b.name));
}

const getConfigurations = async (configuration, step = 1) => {
    if (step === 1) {
        const response = await fetch('http://localhost:7243/api/AutoConfig/getAll', {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        credentials: 'include',
        });
        const entities = await response.json();
        let brands = [];
        for (let i = 0; i < entities.length; i++) {
            if (!brands.some(obj => obj.brandName === entities[i].brandName)) {
                brands.push({
                name: entities[i].brandName
                });
            }
        }
        await getBrands();
        brands = brands.map(item => {
            const itemInArr1 = brandsResponse.find(obj => obj.name === item.name);
            if (itemInArr1) {
              return { ...item, id: itemInArr1.id };
            } else {
              return null;
            }
        });
        let uniqueBrands = [];
        for (let i = 0; i < entities.length; i++) {
            if (!uniqueBrands.some(obj => obj.name === brands[i].name)) {
                uniqueBrands.push({
                    id: brands[i].id,
                    name: brands[i].name
                });
            }
        }
        const config_step_1 = document.getElementById('config_step_1');
        let child = config_step_1.firstChild;
        while (child) {
            const nextSibling = child.nextSibling; 
            if (child.tagName !== 'TEMPLATE') config_step_1.removeChild(child); 
            child = nextSibling; 
        }
        const brandTitleTemplate = document.getElementById('brandTitleTemplate');
        const brandTitle = brandTitleTemplate.content.cloneNode(true);
        brandTitle.querySelector('.config-part-title').textContent = 'Выберите бренд';
        config_step_1.appendChild(brandTitle);
        if (entities.length === 0) {
            console.log('Бренды не найдены');
        }
        else {
            const brandCardTemplate = document.getElementById('brandCardTemplate');
            uniqueBrands.forEach(brand => {
                const brandCard = brandCardTemplate.content.cloneNode(true);
                const article = brandCard.querySelector('.config-part-article');
        
                brandCard.querySelector('.config-part-title').textContent = brand.name;
                article.addEventListener('click', function() {
                    configuration.brandId = brand.id;
                    window.location.href = `#config_step_2`;
                });
                config_step_1.appendChild(brandCard);
            });
        }
    }
    if (step === 2) {
        const response = await fetch('http://localhost:7243/api/AutoConfig/getByFilter', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify({
                brandId: configuration.brandId
            }),
            credentials: 'include',
        });
        const entities = await response.json();
        const config_step_2 = document.getElementById('config_step_2');
        let child = config_step_2.firstChild;
        while (child) {
            const nextSibling = child.nextSibling;
            if (child.tagName !== 'TEMPLATE') config_step_2.removeChild(child);
            child = nextSibling;
        }
        const modelTitleTemplate = document.getElementById('modelTitleTemplate');
        const modelTitle = modelTitleTemplate.content.cloneNode(true);
        modelTitle.querySelector('.config-part-title').textContent = 'Выберите модель';
        config_step_2.appendChild(modelTitle);
        
        if (entities.length === 0) {
            console.log('Модели не найдены');
        } else {
            const modelCardTemplate = document.getElementById('modelCardTemplate');
            const addedModelNames = new Set(); 
            
            entities.forEach(entity => {
                if (!addedModelNames.has(entity.autoModelName)) {
                    const modelCard = modelCardTemplate.content.cloneNode(true);
                    const article = modelCard.querySelector('.config-part-article');
    
                    modelCard.querySelector('.config-part-title').textContent = entity.autoModelName;
                    article.addEventListener('click', function () {
                        configuration.autoModelId = entity.autoModelId;
                        window.location.href = `#config_step_3`;
                    });
                    config_step_2.appendChild(modelCard);
                    addedModelNames.add(entity.autoModelName); 
                }
            });
        }
    }
    if (step === 3) {
        const response = await fetch('http://localhost:7243/api/AutoConfig/getByFilter', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify({
                brandId: configuration.brandId,
                autoModelId: configuration.autoModelId
            }),
            credentials: 'include',
        });
        const entities = await response.json();
        const config_step_3 = document.getElementById('config_step_3');
        let child = config_step_3.firstChild;
        while (child) {
            const nextSibling = child.nextSibling;
            if (child.tagName !== 'TEMPLATE') config_step_3.removeChild(child);
            child = nextSibling;
        }
        const bodyTypeTitleTemplate = document.getElementById('bodyTypeTitleTemplate');
        const bodyTypeTitle = bodyTypeTitleTemplate.content.cloneNode(true);
        bodyTypeTitle.querySelector('.config-part-title').textContent = 'Выберите кузов';
        config_step_3.appendChild(bodyTypeTitle);
    
        if (entities.length === 0) {
            console.log('Кузовы не найдены');
        } else {
            const bodyTypeCardTemplate = document.getElementById('bodyTypeCardTemplate');
            const addedBodyTypes = new Set(); 
    
            entities.forEach(entity => {
                if (!addedBodyTypes.has(entity.bodyType)) {
                    const bodyTypeCard = bodyTypeCardTemplate.content.cloneNode(true);
                    const article = bodyTypeCard.querySelector('.config-part-article');
    
                    bodyTypeCard.querySelector('.config-part-title').textContent = entity.bodyType;
                    article.addEventListener('click', function () {
                        configuration.bodyTypeId = entity.bodyTypeId;
                        window.location.href = `#config_step_4`;
                    });
                    config_step_3.appendChild(bodyTypeCard);
                    addedBodyTypes.add(entity.bodyType); 
                }
            });
        }
    }    
    if (step === 4) {
        const response = await fetch('http://localhost:7243/api/AutoConfig/getByFilter', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify({
                brandId: configuration.brandId,
                autoModelId: configuration.autoModelId,
                bodyTypeId: configuration.bodyTypeId
            }),
            credentials: 'include',
        });
        const entities = await response.json();
        const config_step_4 = document.getElementById('config_step_4');
        let child = config_step_4.firstChild;
        while (child) {
            const nextSibling = child.nextSibling;
            if (child.tagName !== 'TEMPLATE') config_step_4.removeChild(child);
            child = nextSibling;
        }
        const driveTypeTitleTemplate = document.getElementById('driveTypeTitleTemplate');
        const driveTypeTitle = driveTypeTitleTemplate.content.cloneNode(true);
        driveTypeTitle.querySelector('.config-part-title').textContent = 'Выберите привод';
        config_step_4.appendChild(driveTypeTitle);
    
        if (entities.length === 0) {
            console.log('Приводы не найдены');
        } else {
            const driveTypeCardTemplate = document.getElementById('driveTypeCardTemplate');
            const addedDriveTypes = new Set(); 
    
            entities.forEach(entity => {
                if (!addedDriveTypes.has(entity.driveType)) {
                    const driveTypeCard = driveTypeCardTemplate.content.cloneNode(true);
                    const article = driveTypeCard.querySelector('.config-part-article');
    
                    driveTypeCard.querySelector('.config-part-title').textContent = entity.driveType + " привод";
                    article.addEventListener('click', function () {
                        configuration.driveTypeId = entity.driveTypeId;
                        window.location.href = `#config_step_5`;
                    });
                    config_step_4.appendChild(driveTypeCard);
                    addedDriveTypes.add(entity.driveType); 
                }
            });
        }
    }
    
    if (step === 5) {
        const response = await fetch('http://localhost:7243/api/AutoConfig/getByFilter', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify({
                brandId: configuration.brandId,
                autoModelId: configuration.autoModelId,
                bodyTypeId: configuration.bodyTypeId,
                driveTypeId: configuration.driveTypeId
            }),
            credentials: 'include',
        });
        const entities = await response.json();
        const config_step_5 = document.getElementById('config_step_5');
        let child = config_step_5.firstChild;
        while (child) {
            const nextSibling = child.nextSibling;
            if (child.tagName !== 'TEMPLATE') config_step_5.removeChild(child);
            child = nextSibling;
        }
        const equipmentTitleTemplate = document.getElementById('equipmentTitleTemplate');
        const equipmentTitle = equipmentTitleTemplate.content.cloneNode(true);
        equipmentTitle.querySelector('.config-part-title').textContent = 'Выберите комплектацию';
        config_step_5.appendChild(equipmentTitle);
    
        if (entities.length === 0) {
            console.log('Комплектации не найдены');
        } else {
            const equipmentCardTemplate = document.getElementById('equipmentCardTemplate');
            const addedEquipments = new Set(); 
    
            entities.forEach(entity => {
                const equipmentIdentifier = entity.equipment.name + " " + entity.equipment.releaseYear;
                if (!addedEquipments.has(equipmentIdentifier)) {
                    const equipmentCard = equipmentCardTemplate.content.cloneNode(true);
                    const article = equipmentCard.querySelector('.config-part-article');
    
                    equipmentCard.querySelector('.config-part-title').textContent = equipmentIdentifier;
                    article.addEventListener('click', function () {
                        configuration.equipmentId = entity.equipmentId;
                        window.location.href = `#config_step_6`;
                    });
                    config_step_5.appendChild(equipmentCard);
                    addedEquipments.add(equipmentIdentifier); 
                }
            });
        }
    }
    
    if (step === 6) {
        const response = await fetch('http://localhost:7243/api/AutoConfig/getByFilter', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify({
                brandId: configuration.brandId,
                autoModelId: configuration.autoModelId,
                bodyTypeId: configuration.bodyTypeId,
                driveTypeId: configuration.driveTypeId,
                equipmentId: configuration.equipmentId
            }),
            credentials: 'include',
        });
        const entities = await response.json();
        const config_step_6 = document.getElementById('config_step_6');
        let child = config_step_6.firstChild;
        while (child) {
            const nextSibling = child.nextSibling;
            if (child.tagName !== 'TEMPLATE') config_step_6.removeChild(child);
            child = nextSibling;
        }
        const engineTitleTemplate = document.getElementById('engineTitleTemplate');
        const engineTitle = engineTitleTemplate.content.cloneNode(true);
        engineTitle.querySelector('.config-part-title').textContent = 'Выберите двигатель';
        config_step_6.appendChild(engineTitle);
    
        if (entities.length === 0) {
            console.log('Двигатели не найдены');
        } else {
            const engineCardTemplate = document.getElementById('engineCardTemplate');
            const addedEngines = new Set(); 
    
            entities.forEach(entity => {
                const engineIdentifier = entity.engine.engineType + ", " + entity.engine.power + " л.с, " + (entity.engine.transmissionType === "Механическая" ? "МТ" : "АТ");
                if (!addedEngines.has(engineIdentifier)) {
                    const engineCard = engineCardTemplate.content.cloneNode(true);
                    const article = engineCard.querySelector('.config-part-article');
    
                    engineCard.querySelector('.config-part-title').textContent = engineIdentifier;
                    article.addEventListener('click', function () {
                        configuration.engineId = entity.engine.id;
                        window.location.href = `#config_step_7`;
                    });
                    config_step_6.appendChild(engineCard);
                    addedEngines.add(engineIdentifier);
                }
            });
        }
    }
    
    if (step === 7) {
        const response = await fetch('http://localhost:7243/api/AutoConfig/getByFilter', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify({
                brandId: configuration.brandId,
                autoModelId: configuration.autoModelId,
                bodyTypeId: configuration.bodyTypeId,
                driveTypeId: configuration.driveTypeId,
                equipmentId: configuration.equipmentId,
                engineId: configuration.engineId
            }),
            credentials: 'include',
        });
        const entities = await response.json();
        const config_step_7 = document.getElementById('config_step_7');
        let child = config_step_7.firstChild;
        while (child) {
            const nextSibling = child.nextSibling;
            if (child.tagName !== 'TEMPLATE') config_step_7.removeChild(child);
            child = nextSibling;
        }
        const colorTitleTemplate = document.getElementById('colorTitleTemplate');
        const colorTitle = colorTitleTemplate.content.cloneNode(true);
        colorTitle.querySelector('.config-part-title').textContent = 'Выберите цвет';
        config_step_7.appendChild(colorTitle);
    
        if (entities.length === 0) {
            console.log('Цвета не найдены');
        } else {
            const colorCardTemplate = document.getElementById('colorCardTemplate');
            const addedColors = new Set(); // To keep track of unique colors
    
            entities.forEach(entity => {
                if (!addedColors.has(entity.color)) {
                    const colorCard = colorCardTemplate.content.cloneNode(true);
                    const article = colorCard.querySelector('.config-part-article');
    
                    colorCard.querySelector('.config-part-title').textContent = entity.color;
                    article.addEventListener('click', function () {
                        configuration.colorId = entity.colorId;
                        window.location.href = `#configuratedCar`;
                    });
                    config_step_7.appendChild(colorCard);
                    addedColors.add(entity.color); // Mark this color as added
                }
            });
        }
    }
    
    if (step === 8 ) {
        const response = await fetch('http://localhost:7243/api/AutoConfig/getByFilter', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify({
                brandId: configuration.brandId,
                autoModelId: configuration.autoModelId,
                bodyTypeId: configuration.bodyTypeId,
                driveTypeId: configuration.driveTypeId,
                equipmentId: configuration.equipmentId,
                engineId: configuration.engineId,
                colorId: configuration.colorId
            }),
            credentials: 'include',
        })
        const entities = await response.json();
        const configuratedCarTitle = document.getElementById('carTitle');
        configuratedCarTitle.textContent = "Вы выбрали";
        const configuratedCar = document.getElementById('carText');
        entities.forEach(entity => {
            configuratedCar.textContent = entity.brandName + " " + entity.autoModelName + ", "
            + entity.bodyType + ", " + entity.color + ", "  + entity.equipment.name + ", " + entity.equipment.releaseYear + ", " + entity.totalPrice + "₽";
            autoConfigurationId = entity.id;
            document.getElementById('carImg').src = `http://localhost:7243/api/File/getImage/${entity.id}`;

        })
    }
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
        autoConfigurationId: autoConfigurationId,
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