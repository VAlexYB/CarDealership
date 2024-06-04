document.addEventListener("DOMContentLoaded", async function() {
    await import('../scripts/navbarAdmin.js');
    await getEntities();
});

const getEntities = async () => {
    const response = await fetch("https://localhost:7243/api/Users/getMgrs", {
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
        entityCard.querySelector('#userNameable').textContent = entity.userName;
        entityCard.querySelector('#emailTable').textContent = entity.email;
        entityCard.querySelector('#firstNameTable').textContent = entity.firstName;
        entityCard.querySelector('#lastNameTable').textContent = entity.lastName;
        entityCard.querySelector('#middleNameTable').textContent = entity.middleName;
        entityCard.querySelector('#phoneNumberTable').textContent = entity.phoneNumber;
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
        document.getElementById('userName').value = entity.userName;
        document.getElementById('email').value = entity.email;
        document.getElementById('firstName').value = entity.firstName;
        document.getElementById('lastName').value = entity.lastName;
        document.getElementById('middleName').value = entity.middleName;
        document.getElementById('phoneNumber').value = entity.phoneNumber;
        document.getElementById('dialogTitle').textContent = 'Редактировать сотрудника';
        document.getElementById('dialogSubmitBtn').textContent = 'Сохранить';
        entityForm.onsubmit = (event) => {
            event.preventDefault();
            addOrEditEntity(entity);
        };;
    }
    else {
        document.getElementById('dialogTitle').textContent = 'Добавить сотрудника';
        document.getElementById('dialogSubmitBtn').textContent = 'Добавить';
        entityForm.onsubmit = (event) => {
            event.preventDefault();
            addOrEditEntity();
        };
    }
}

const closeEntityDialog = () => {
    const entityForm = document.getElementById('entityForm');
    document.getElementById('userName').value = '';
    document.getElementById('email').value = '';
    document.getElementById('firstName').value = '';
    document.getElementById('lastName').value = '';
    document.getElementById('middleName').value = '';
    document.getElementById('phoneNumber').value = '';
    entityForm.onsubmit = null;
    document.getElementById('addEntityDialog').close();
}

const deleteEntity = async (entity) => {
    await fetch(`https://localhost:7243/api/Users/deleteById/${entity.id}`);
    await getEntities();
}

const addOrEditEntity = async (entity = null) => {
    const userName = document.getElementById('userName').value;
    const email = document.getElementById('email').value;
    const firstName = document.getElementById('firstName').value;
    const lastName = document.getElementById('lastName').value;
    const middleName = document.getElementById('middleName').value;
    const phoneNumber = document.getElementById('phoneNumber').value;
    const cardDigits = document.getElementById('cardDigits').value;
    if (userName === '' || email === '' || firstName === '' || lastName === '' || middleName === '' || phoneNumber === '' || cardDigits === '') {
        alert('Поля не должны быть пустыми');
        return;
    }
    let newEntity = {};
    if (entity) {
        newEntity = {
            "id": entity.id,
            "userName": userName,
            "email": email,
            "firstName": firstName,
            "lastName": lastName,
            "middleName": middleName,
            "phoneNumber": phoneNumber,
            "cardDigits": cardDigits
        }
    } else {
        newEntity = {
            "userName": userName,
            "email": email,
            "firstName": firstName,
            "lastName": lastName,
            "middleName": middleName,
            "phoneNumber": phoneNumber,
            "cardDigits": cardDigits
        }
    }
    await fetch("https://localhost:7243/api/Users/addmgr", {
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