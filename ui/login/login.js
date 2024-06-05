document.addEventListener("DOMContentLoaded", async function() {
    await import('https://cdn.jsdelivr.net/npm/jwt-decode/build/jwt-decode.min.js');
    const form = document.getElementById('entityForm');
    form.onsubmit = async (event) => {
        event.preventDefault();
        const userName = document.getElementById('userName').value;
        const password = document.getElementById('password').value;
        if (userName === '' || password === '') {
            alert('Поля не должны быть пустыми');
            return;
        }   
        const entity = {
            "identifier": userName,
            "password": password
        };  
        const response = await fetch('http://localhost:7243/api/Users/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                "Accept": "application/json"
            },
            credentials: 'include',
            body: JSON.stringify(entity)
        }); 
        if (!response.ok) {
            const error = await response.json()
            alert(error);
            return;
        } else {
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
            console.log(decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"]);
            if (decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] === 'Admin') {
                window.location.href = "../admin/admin.html";
            } else if (decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] === 'User') {
                window.location.href = "../mainPage/mainPage.html";
            }
            else if (decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] === 'Manager' || 
                decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"].some(role => role === "Manager")
            ) {
                window.location.href = "../brandsManagement/brandsManagement.html";
            }
        }
    };
})