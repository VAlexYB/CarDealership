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
    if (decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] !== 'User') {
        alert('Вы не клиент');
        window.location.href = "../catalog/catalog.html";
    }

    const user = await getUser(decoded.userId);

    const form = document.getElementById('entityForm');
    console.log(user)
    document.getElementById('userName').value = user.userName;
    document.getElementById('email').value = user.email;
    document.getElementById('phone').value = user.phoneNumber;
    document.getElementById('firstName').value = user.firstName;
    document.getElementById('lastName').value = user.lastName;
    document.getElementById('middleName').value = user.middleName;

    form.onsubmit = async (event) => {
        event.preventDefault();
        const userName = document.getElementById('userName').value;
        const email = document.getElementById('email').value;
        const phone = document.getElementById('phone').value;
        const firstName = document.getElementById('firstName').value;
        const lastName = document.getElementById('lastName').value;
        const middleName = document.getElementById('middleName').value;
        const card = document.getElementById('card').value;
        const password = document.getElementById('password').value;
        const passwordRepeat = document.getElementById('passwordRepeat').value;
        if (userName === '' || email === '' || phone === '' || firstName === '' || lastName === '' || middleName === '' || card === '' || password === '' || passwordRepeat === '') {
            alert('Поля не должны быть пустыми');
            return;
        }   
        if (password !== passwordRepeat) {
            alert('Пароли не совпадают');
            return;
        }
        const entity = {
            "id": user.id,
            "userName": userName,
            "email": email,
            "password": password,
            "phoneNumber": phone,
            "firstName": firstName,
            "lastName": lastName,
            "middleName": middleName,
            "firstCardDigits": card.slice(0, 4),
            "lastCardDigits": card.slice(12, 16),
        };  
        const response = await fetch('http://localhost:7243/api/Users/register', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                "Accept": "application/json"
            },
            body: JSON.stringify(entity),
            credentials: 'include',
        }); 
        if (!response.ok) {
            const error = await response.json()
            alert(error);
            return;
        } else {
            alert('Данные изменены');
        }
    };
})

const getUser = async (userId) => {
    const response = await fetch(`http://localhost:7243/api/Users/getById/${userId}`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        credentials: 'include',
    });
    const user = await response.json();
    return user;
}

// const validateCardId = (cardId) => {
//     const digits = cardId.replace(/\D/g, '').split('').map(Number);

//     if (!/^\d+$/.test(cardId) || digits.length !== 16) {
//         return false;
//     }

//     let sum = 0;
//     let even = false;
  
//     for (let i = digits.length - 1; i >= 0; i--) {
//         let digit = digits[i];
//         if (even) {
//             digit *= 2;
//             if (digit > 9) {
//                 digit -= 9;
//             }
//         }
//         sum += digit;
//         even = !even; 
//     }

//     return sum % 10 === 0;
// }
