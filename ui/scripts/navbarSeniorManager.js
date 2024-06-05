const navbar = document.getElementById('navbar')

navbar.innerHTML = `
    <ul class="nav__list">
        <li class="nav__item"><a href="../brandsManagement/brandsManagement.html">Бренды</a></li>
        <li class="nav__item"><a href="../modelsManagement/modelsManagement.html">Модели</a></li>
        <li class="nav__item"><a href="../equipmentsManagement/equipmentsManagement.html">Комплектации</a></li>
        <li class="nav__item"><a href="../featuresManagement/featuresManagement.html">Особенности<br>комплектаций</a></li>
        <li class="nav__item"><a href="../enginesManagement/enginesManagement.html">Двигатели</a></li>
        <li class="nav__item"><a href="../colorsManagement/colorsManagement.html">Цвета</a></li>
        <li class="nav__item"><a href="../autoConfigManagement/autoConfigManagement.html">Конфигурации</a></li>
        <li class="nav__item"><a href="../carsManagement/carsManagement.html">Машины</a></li>
        <li class="nav__item"><a href="../ordersManagement/ordersManagement.html">Заказы</a></li>
        <li class="nav__item"><a href="../myOrdersManagement/myOrdersManagement.html">Мои заказы</a></li>
        <li class="nav__item"><a href="../dealsManagement/dealsManagement.html">Сделки</a></li>
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