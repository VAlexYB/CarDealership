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
        <li class="nav__item">
            <button class="btn__reset nav__iconLog">
                <a href="../login/login.html">
                    <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="36" height="36" viewBox="0 0 36 36">
                        <image id="account_1_" data-name="account (1)"  xlink:href="data:img/png;base64,iVBORw0KGgoAAAANSUhEUgAAACQAAAAkCAYAAADhAJiYAAACvUlEQVRYhc2Xy0tVURjFf93A1CgQqUzsnU0CIU1HJf0BYY20Jg2CDBrloFGU0GNQDWrcTIKSQGoWUQN7miMHFUQPKYgsmxRRKZFf7FgXLmfvc+45p+u1Bct7+R7rW+5z7t7nYGZ5edDM7pmPUeUKeXQXuT8ZsRcYAparbRR4ru9bgV36/g04ANzMJB9ymcDTWo9pM9sXswqLzWy/ahxOBWpiGZsI8KQGXA3k4nhNPSdi8h69QAw7JHwrJp/E2+ptT6jJfA9NAhuAgrvKXjYZrue3NDYlVqq4HDpkZjCHGYc59W4E2r1sFKFli/CSlnxFIJeWK6VxsVx9mhXqBn4Bn71MekxLo7tcRxpDTcAnL5rPVJMXzWHoB7DUi2ZHPfCzEoaeAQ0pa+NQkMbTmHwmQzf0ucPLpMfOiFY8Qnd6hPX6hYwFcmn5RBp1ldqpz0uwJ5Arxz3qPVemLvNZNinhzkAujp3qeROT9+gFEthkZl804EhCXZGHVftVvaEaj16gDJeZ2YgGTZjZUTPbbmYNYqdiE6oZUU/qGV4gBTeb2QvvOdHHS9Vm0k972tcBh4BjQItir4C3wEdgVrEl2o3XA62KvQcuAJeBGU85ipDLCPtL/vcpMztuZm2BuijbVPuhpL8/UJf6kq0ys4cSeq2fb6guDXt0CR0eJd3kXkB0T3dzEhgI5PNyQJpOe1tIwwuUPK46dAXy/8quEv2OcoaaVThjZmvnwUyRTntWs5qTDBV349aASKW5JbSLlw4pnle9VTBTZF/0nCsmGpW4H2iabz7Q7MZSQ1cUbFkAQ2s0e6hoqEaBx4HianFMHmrcE2OPNu8z3jZePZzVpN3uLBsG+oDakjOp2qjVC8CwMzSlN9LmBVwhh78+Cjqd73jp6uMusNqtkFudd8D1Cr1/5cF3oBdY5wyNA13/wQoBjP8BHySAqMJVs14AAAAASUVORK5CYII="/>
                    </svg>
                </a>
            </button>
        </li>
    </ul>
`;