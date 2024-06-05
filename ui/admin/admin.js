document.addEventListener("DOMContentLoaded", async function() {
    await import('../scripts/navbarAdmin.js');
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
    if (decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] !== 'Admin') {
        alert('Вы не админ');
        window.location.href = "../catalog/catalog.html";
    }
    await getEntities();
});

const getEntities = async () => {
    const response = await fetch("http://localhost:7243/api/Users/getMgrs", {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        },
        credentials: 'include'
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
        entityCard.querySelector('#userNameTable').textContent = entity.userName;
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
    document.getElementById('password').value = '';
    document.getElementById('repeatPassword').value = '';
    document.getElementById('firstName').value = '';
    document.getElementById('lastName').value = '';
    document.getElementById('middleName').value = '';
    document.getElementById('phoneNumber').value = '';
    const position = document.getElementById('position').checked = false;
    entityForm.onsubmit = null;
    document.getElementById('addEntityDialog').close();
}

const deleteEntity = async (entity) => {
    await fetch(`http://localhost:7243/api/Users/deleteById/${entity.id}`, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        },
        credentials: 'include'  
    });
    await getEntities();
}

const addOrEditEntity = async (entity = null) => {
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
    if (decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] !== 'Admin') {
        alert('Вы не администратор');
    }
    const userName = document.getElementById('userName').value;
    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;
    const repeatPassword = document.getElementById('repeatPassword').value;
    const firstName = document.getElementById('firstName').value;
    const lastName = document.getElementById('lastName').value;
    const middleName = document.getElementById('middleName').value;
    const phoneNumber = document.getElementById('phoneNumber').value;
    if (userName === '' || email === '' || firstName === '' || lastName === '' || middleName === '' || phoneNumber === '' || password === '' || repeatPassword === '') {
        alert('Поля не должны быть пустыми');
        return;
    }
    if (password !== repeatPassword) {
        alert('Пароли не совпадают');
        return;
    }
    const position = document.getElementById('position').checked ? 1 : 0;
    let newEntity = {};
    if (entity) {
        newEntity = {
            "id": entity.id,
            "userName": userName,
            "email": email,
            "password": password,
            "firstName": firstName,
            "lastName": lastName,
            "middleName": middleName,
            "phoneNumber": phoneNumber
        }
    } else {
        newEntity = {
            "userName": userName,
            "email": email,
            "password": password,
            "firstName": firstName,
            "lastName": lastName,
            "middleName": middleName,
            "phoneNumber": phoneNumber
        }
    }
    await fetch("http://localhost:7243/api/Users/addmgr", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        },
        body: JSON.stringify(newEntity),
        credentials: 'include'
    });
    let mgrId;
    if (!entity) {
        const mgrs = await fetch('http://localhost:7243/api/Users/getMgrs', {
            method: 'GET',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            credentials: 'include',
        });
        const mgrsJson = await mgrs.json();
        mgrsJson.forEach(mgr => {
            if (mgr.userName === userName) {
                mgrId = mgr.id;
            }
        });
    } else {
        mgrId = entity.id;
    }
    if (position === 1) {
        await fetch(`http://localhost:7243/api/Users/assignSenior/${mgrId}`, {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            },
            credentials: 'include'
        });
    } else {
        await fetch(`http://localhost:7243/api/Users/suspendSenior/${mgrId}`, {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            },
            credentials: 'include'
        });
    }
    closeEntityDialog();
    await getEntities();
}