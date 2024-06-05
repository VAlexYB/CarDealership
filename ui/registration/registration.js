document.addEventListener("DOMContentLoaded", async function() {
    const form = document.getElementById('entityForm');
    form.onsubmit = async (event) => {
        event.preventDefault();
        const userName = document.getElementById('userName').value;
        const email = document.getElementById('email').value;
        const password = document.getElementById('password').value;
        const passwordRepeat = document.getElementById('passwordRepeat').value;
        if (userName === '' || email === '' || password === '' || passwordRepeat === '') {
            alert('Поля не должны быть пустыми');
            return;
        }   
        if (password !== passwordRepeat) {
            alert('Пароли не совпадают');
            return;
        }
        const entity = {
            "userName": userName,
            "email": email,
            "password": password
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
            alert('Вы зарегистрировались');
            window.location.href = '../login/login.html';
        }
    };
})