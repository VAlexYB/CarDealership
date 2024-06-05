let brands = [];

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
    await getBrand();
    await getBodyTypes();
    await getColors();
    await getDriveTypes();
    await getEngines();
    await getEntities();
});

const getBrand = async () => {
    const response = await fetch("http://localhost:7243/api/Brands/getAll", {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        },
        credentials: 'include',
    });
    brands = (await response.json()).sort((a, b) => a.name.localeCompare(b.name));
    const brandSelect = document.getElementById('brand');
    for (let i = 0; i < brands.length; i++) {
        const entity = brands[i];
        if (i == 0)
            await getModels(entity.id);
        const option = document.createElement('option');
        option.value = entity.id;
        option.textContent = entity.name;
        brandSelect.appendChild(option);
    }
    brandSelect.addEventListener('change', async function(event) {
        await getModels(event.target.value);
    })
}

const getModels = async (brandId) => {
    const response = await fetch("http://localhost:7243/api/AutoModels/getByFilter", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        },
        body: JSON.stringify({brandId: brandId}),
        credentials: 'include',
    });
    const entities = (await response.json()).sort((a, b) => a.name.localeCompare(b.name));
    const modelsSelect = document.getElementById('model');
    let child = modelsSelect.firstChild;
    while (child) {
        const nextSibling = child.nextSibling; 
        if (child.tagName !== 'TEMPLATE') modelsSelect.removeChild(child); 
        child = nextSibling; 
    }
    for (let i = 0; i < entities.length; i++) {
        const entity = entities[i];
        if (i == 0)
            await getEquipments(entity.id);
        const option = document.createElement('option');
        option.value = entity.id;
        option.textContent = entity.name;
        modelsSelect.appendChild(option);
    }
    modelsSelect.addEventListener('change', async function(event) {
        await getEquipments(event.target.value);
    })
}

const getBodyTypes = async () => {
    const response = await fetch("http://localhost:7243/api/BodyTypes/getAll", {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        },
        credentials: 'include',
    });
    const bodyTypes = (await response.json()).sort((a, b) => a.value.localeCompare(b.value));
    const bodyTypeSelect = document.getElementById('bodyType');
    bodyTypes.forEach(bodyType => {
        const option = document.createElement('option');
        option.value = bodyType.id;
        option.textContent = bodyType.value + ", " + bodyType.price + "₽";
        bodyTypeSelect.appendChild(option);
    })
}

const getDriveTypes = async () => {
    const response = await fetch("http://localhost:7243/api/DriveTypes/getAll", {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        },
        credentials: 'include',
    });
    const driveTypes = (await response.json()).sort((a, b) => a.value.localeCompare(b.value));
    const driveTypeSelect = document.getElementById('driveType');
    driveTypes.forEach(driveType => {
        const option = document.createElement('option');
        option.value = driveType.id;
        option.textContent = driveType.value;
        driveTypeSelect.appendChild(option);
    });
}

const getColors = async () => {
    const response = await fetch("http://localhost:7243/api/Colors/getAll", {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        },
        credentials: 'include',
    });
    const colors = (await response.json()).sort((a, b) => a.value.localeCompare(b.value));
    const colorSelect = document.getElementById('color');
    colors.forEach(color => {
        const option = document.createElement('option');
        option.value = color.id;
        option.textContent = color.value + ", " + color.price + "₽";
        colorSelect.appendChild(option);
    });
}

const getEngines = async () => {
    const response = await fetch("http://localhost:7243/api/Engines/getAll", {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        },
        credentials: 'include',
    });
    const engines = (await response.json());
    const engineSelect = document.getElementById('engine');
    engines.forEach(engine => {
        const option = document.createElement('option');
        option.value = engine.id;
        const transType = engine.transmissionType === "Механическая" ? "МТ" : "АТ";
        option.textContent = engine.engineType + ", " + engine.power + "л.с., " + transType + ", " + engine.price + "₽"; 
        engineSelect.appendChild(option);
    });
}

const getEquipments = async (modelId) => {
    const response = await fetch("http://localhost:7243/api/Equipments/getByFilter", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        },
        body: JSON.stringify({ 
            autoModelId: modelId
        }),
        credentials: 'include',
    });
    const equipments = (await response.json());
    const equipmentSelect = document.getElementById('equipment');
    let child = equipmentSelect.firstChild;
    while (child) {
        const nextSibling = child.nextSibling; 
        if (child.tagName !== 'TEMPLATE') equipmentSelect.removeChild(child); 
        child = nextSibling; 
    }
    equipments.forEach(equipment => {
        const option = document.createElement('option');
        option.value = equipment.id;
        option.textContent = equipment.name + ", " + equipment.releaseYear + ", " + equipment.price + "₽";
        equipmentSelect.appendChild(option);
    });
}

const getEntities = async () => {
    const response = await fetch("http://localhost:7243/api/AutoConfig/getAll", {
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
        entityCard.querySelector('#brandTable').textContent = entity.brandName;
        entityCard.querySelector('#modelTable').textContent = entity.autoModelName;
        entityCard.querySelector('#bodyTypeTable').textContent = entity.bodyType;
        entityCard.querySelector('#driveTypeTable').textContent = entity.driveType;
        entityCard.querySelector('#engineTable').textContent = entity.engine.engineType + ", " + entity.engine.power + "л.с., " + (entity.engine.transmissionType === "Механическая" ? "МТ" : "АТ");
        entityCard.querySelector('#equipmentTable').textContent = entity.equipment.name + ", " + entity.equipment.releaseYear; 
        entityCard.querySelector('#colorTable').textContent = entity.color;
        entityCard.querySelector('#priceTable').textContent = entity.price + "₽";
        entityCard.querySelector('#totalPriceTable').textContent = entity.totalPrice + "₽";
        entityCard.querySelector('#carImage').src = `http://localhost:7243/api/File/getImage/${entity.id}`;
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

const openEntityDialog = async (entity = null) => {
    document.getElementById('addEntityDialog').showModal();
    const entityForm = document.getElementById('entityForm');
    if (entity) {
        const brand = document.getElementById('brand');
        const brandOptions = brand.querySelectorAll('option');
        for (let i = 0; i < brandOptions.length; i++) {
            if (brandOptions[i].textContent === entity.brandName) {
                brandOptions[i].selected = true;
                const brand = brands.find(brand => brand.name === entity.brandName);
                await getModels(brand.id);
                break;
            } 
        };
        const model = document.getElementById('model');
        const modelOptions = model.querySelectorAll('option');
        for (let i = 0; i < modelOptions.length; i++) {
            if (modelOptions[i].value === entity.autoModelId) {
                modelOptions[i].selected = true;
                await getEquipments(entity.autoModelId);
                break;
            } 
        };
        const bodyType = document.getElementById('bodyType');
        const bodyTypeOptions = bodyType.querySelectorAll('option');
        for (let i = 0; i < bodyTypeOptions.length; i++) {
            if (bodyTypeOptions[i].value === entity.bodyTypeId) {
                bodyTypeOptions[i].selected = true;
                break;
            } 
        };
        const driveType = document.getElementById('driveType');
        const driveTypeOptions = driveType.querySelectorAll('option');
        for (let i = 0; i < driveTypeOptions.length; i++) {
            if (driveTypeOptions[i].value === entity.driveTypeId) {
                driveTypeOptions[i].selected = true;
                break;  
            }
        };
        const engine = document.getElementById('engine');
        const engineOptions = engine.querySelectorAll('option');
        for (let i = 0; i < engineOptions.length; i++) {
            if (engineOptions[i].value === entity.engine.id) {
                engineOptions[i].selected = true;
                break;
            } 
        };
        const equipment = document.getElementById('equipment');
        const equipmentOptions = equipment.querySelectorAll('option');
        for (let i = 0; i < equipmentOptions.length; i++) {
            if (equipmentOptions[i].value === entity.equipmentId) {
                equipmentOptions[i].selected = true;
                break;
            } 
        };
        const color = document.getElementById('color');
        const colorOptions = color.querySelectorAll('option');
        for (let i = 0; i < colorOptions.length; i++) {
            if (colorOptions[i].value === entity.colorId) {
                colorOptions[i].selected = true;
                break;
            } 
        };
        document.getElementById('price').value = entity.price;
        document.getElementById('dialogTitle').textContent = 'Редактировать конфигурацию';
        document.getElementById('dialogSubmitBtn').textContent = 'Сохранить';
        entityForm.onsubmit = (event) => {
            event.preventDefault();
            addOrEditEntity(entity);
        };;
    }
    else {
        document.getElementById('dialogTitle').textContent = 'Добавить конфигурацию';
        document.getElementById('dialogSubmitBtn').textContent = 'Добавить';
        entityForm.onsubmit = (event) => {
            event.preventDefault();
            addOrEditEntity();
        };
    }
}

const closeEntityDialog = () => {
    const entityForm = document.getElementById('entityForm');
    const brand = document.getElementById('brand');
    const brandOptions = brand.querySelectorAll('option');
    for (let i = 0; i < brandOptions.length; i++) {
        brandOptions[i].selected = false;
    };
    const model = document.getElementById('model');
    const modelOptions = model.querySelectorAll('option');
    for (let i = 0; i < modelOptions.length; i++) {
        modelOptions[i].selected = false;
    }
    const bodyType = document.getElementById('bodyType');
    const bodyTypeOptions = bodyType.querySelectorAll('option');
    for (let i = 0; i < bodyTypeOptions.length; i++) {
        bodyTypeOptions[i].selected = false;
    }
    const driveType = document.getElementById('driveType');
    const driveTypeOptions = driveType.querySelectorAll('option');
    for (let i = 0; i < driveTypeOptions.length; i++) {
        driveTypeOptions[i].selected = false;
    }
    const engine = document.getElementById('engine');
    const engineOptions = engine.querySelectorAll('option');
    for (let i = 0; i < engineOptions.length; i++) {
        engineOptions[i].selected = false;
    }
    const equipment = document.getElementById('equipment');
    const equipmentOptions = equipment.querySelectorAll('option');
    for (let i = 0; i < equipmentOptions.length; i++) {
        equipmentOptions[i].selected = false;
    }
    const color = document.getElementById('color');
    const colorOptions = color.querySelectorAll('option');
    for (let i = 0; i < colorOptions.length; i++) {
        colorOptions[i].selected = false;
    }
    document.getElementById('price').value = "";
    entityForm.onsubmit = null;
    document.getElementById('addEntityDialog').close();
}

const deleteEntity = async (entity) => {
    await fetch(`http://localhost:7243/api/AutoConfig/deleteById/${entity.id}`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        credentials: 'include',
    });
    await getEntities();
}

const addOrEditEntity = async (entity = null) => {
    const brand = document.getElementById('brand');
    const brandOptions = brand.querySelectorAll('option');
    let brandId;
    try {
        brandId = brandOptions[brand.selectedIndex].value;
    } catch (error) {
        alert("Выберите марку");
        return;
    }
    const model = document.getElementById('model');
    const modelOptions = model.querySelectorAll('option');
    let autoModelId;
    try {
        autoModelId = modelOptions[model.selectedIndex].value;
    } catch (error) {
        alert("Выберите модель");
        return;
    }
    const bodyType = document.getElementById('bodyType');
    const bodyTypeOptions = bodyType.querySelectorAll('option');
    let bodyTypeId;
    try {
        bodyTypeId = bodyTypeOptions[bodyType.selectedIndex].value;
    } catch (error) {
        alert("Выберите тип кузова");
        return;
    }
    const driveType = document.getElementById('driveType');
    const driveTypeOptions = driveType.querySelectorAll('option');
    let driveTypeId;
    try {
        driveTypeId = driveTypeOptions[driveType.selectedIndex].value;
    } catch (error) {
        alert("Выберите тип привода");
        return;
    }
    const engine = document.getElementById('engine');
    const engineOptions = engine.querySelectorAll('option');
    let engineId;
    try {
        engineId = engineOptions[engine.selectedIndex].value;
    } catch (error) {
        alert("Выберите двигатель");
        return;
    }
    const equipment = document.getElementById('equipment');
    const equipmentOptions = equipment.querySelectorAll('option');
    let equipmentId;
    try {
        equipmentId = equipmentOptions[equipment.selectedIndex].value;
    } catch (error) {
        alert("Выберите комплектацию");
        return;
    }
    const color = document.getElementById('color');
    const colorOptions = color.querySelectorAll('option');
    let colorId;
    try {
        colorId = colorOptions[color.selectedIndex].value;
    } catch (error) {
        alert("Выберите цвет");
        return;
    }
    const price = document.getElementById('price').value;
    if (price === '') {
        alert('Цвет не должен быть пустым');
        return;
    }
    let newEntity = {};
    if (entity) {
        newEntity = {
            "id": entity.id,
            "brandId": brandId,
            "autoModelId": autoModelId,
            "bodyTypeId": bodyTypeId,
            "driveTypeId": driveTypeId,
            "engineId": engineId,
            "equipmentId": equipmentId,
            "colorId": colorId,
            "price": price
        }
    } else {
        newEntity = {
            "brandId": brandId,
            "autoModelId": autoModelId,
            "bodyTypeId": bodyTypeId,
            "driveTypeId": driveTypeId,
            "engineId": engineId,
            "equipmentId": equipmentId,
            "colorId": colorId,
            "price": price
        }
    }
    const response = await fetch("http://localhost:7243/api/AutoConfig/add", {
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
    const config = await fetch('http://localhost:7243/api/AutoConfig/getByFilter', {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },  
        body: JSON.stringify({
            "brandId": brandId,
            "autoModelId": autoModelId,
            "bodyTypeId": bodyTypeId,
            "driveTypeId": driveTypeId,
            "engineId": engineId,
            "equipmentId": equipmentId,
            "colorId": colorId
        }),
        credentials: 'include',
    })
    const configJson = await config.json();
    console.log(configJson);
    const fileInput = document.getElementById('image');
    const file = fileInput.files[0];
    if (!file) {
        alert('Файл не выбран');
        return;
    }
    if (file.type !== 'image/png' && !file.name.endsWith('.png')) {
        alert('Картинка должна быть png');
        return;
    }
    const renamedFile = new File([file], `${entity.id}.png`, { type: file.type });
    await uploadImage(renamedFile);
    closeEntityDialog();
    await getEntities();
}

const uploadImage = async (image) => {
    const formData = new FormData();
    formData.append('file', image);

    const response = await fetch('http://localhost:7243/api/File/upload', {
        method: 'POST',
        body: formData
    });

    if (!response.ok) {
        const error = await response.json();
        alert(error);
        return;
    }
    await getEntities();
}