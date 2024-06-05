const navbar = document.getElementById('navbar')

navbar.innerHTML = `
    <ul class="nav__list">
        <li class="nav__item"><a href="../admin/admin.html">Менеджеры</a></li>
        <li class="nav__item" style="position: absolute; margin-left: 1800px; cursor: pointer"><a id="logout">Выход</a></li>
    </ul>

`;

const logout = async() => {
    const response = await fetch('http://localhost:7243/api/Users/logout', {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            "Accept": "application/json"
        },
        credentials: 'include',
    });
    if (!response.ok) {
        const error = await response.json()
        alert(error);
        return;
    } else {
        window.location.href = `../login/login.html`;
    }
}

document.getElementById('logout').addEventListener('click', (event) => {
    event.preventDefault();
    logout();
});