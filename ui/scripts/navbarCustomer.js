const navbar = document.getElementById('navbar')

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

navbar.innerHTML = `
    <ul class="nav__list">
        <li class="nav__item"><a href="../catalog/catalog.html">Автомобили в наличии</a></li>
        <li class="nav__item"><a href="../configurator/configurator.html#config_step_1">Конфигуратор</a></li>
        <li class="nav__item"><a href="../orders/orders.html">Мои заказы</a></li>
        <li class="nav__item"><a href="../deals/deals.html">Мои сделки</a></li>
        <li class="nav__item" style="position: absolute; margin-left: 1400px">
            <button class="btn__reset nav__iconLog">
                <a id="account">
                    <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="36" height="36" viewBox="0 0 36 36">
                        <image id="account_1_" data-name="account (1)"  xlink:href="data:img/png;base64,iVBORw0KGgoAAAANSUhEUgAAACQAAAAkCAYAAADhAJiYAAACvUlEQVRYhc2Xy0tVURjFf93A1CgQqUzsnU0CIU1HJf0BYY20Jg2CDBrloFGU0GNQDWrcTIKSQGoWUQN7miMHFUQPKYgsmxRRKZFf7FgXLmfvc+45p+u1Bct7+R7rW+5z7t7nYGZ5edDM7pmPUeUKeXQXuT8ZsRcYAparbRR4ru9bgV36/g04ANzMJB9ymcDTWo9pM9sXswqLzWy/ahxOBWpiGZsI8KQGXA3k4nhNPSdi8h69QAw7JHwrJp/E2+ptT6jJfA9NAhuAgrvKXjYZrue3NDYlVqq4HDpkZjCHGYc59W4E2r1sFKFli/CSlnxFIJeWK6VxsVx9mhXqBn4Bn71MekxLo7tcRxpDTcAnL5rPVJMXzWHoB7DUi2ZHPfCzEoaeAQ0pa+NQkMbTmHwmQzf0ucPLpMfOiFY8Qnd6hPX6hYwFcmn5RBp1ldqpz0uwJ5Arxz3qPVemLvNZNinhzkAujp3qeROT9+gFEthkZl804EhCXZGHVftVvaEaj16gDJeZ2YgGTZjZUTPbbmYNYqdiE6oZUU/qGV4gBTeb2QvvOdHHS9Vm0k972tcBh4BjQItir4C3wEdgVrEl2o3XA62KvQcuAJeBGU85ipDLCPtL/vcpMztuZm2BuijbVPuhpL8/UJf6kq0ys4cSeq2fb6guDXt0CR0eJd3kXkB0T3dzEhgI5PNyQJpOe1tIwwuUPK46dAXy/8quEv2OcoaaVThjZmvnwUyRTntWs5qTDBV349aASKW5JbSLlw4pnle9VTBTZF/0nCsmGpW4H2iabz7Q7MZSQ1cUbFkAQ2s0e6hoqEaBx4HianFMHmrcE2OPNu8z3jZePZzVpN3uLBsG+oDakjOp2qjVC8CwMzSlN9LmBVwhh78+Cjqd73jp6uMusNqtkFudd8D1Cr1/5cF3oBdY5wyNA13/wQoBjP8BHySAqMJVs14AAAAASUVORK5CYII="/>
                    </svg>
                </a>
            </button>
        </li>
        <li class="nav__item" style="position: absolute; margin-left: 1800px; cursor: pointer"><a id="logout">Выход</a></li>
    </ul>
`;

const account = document.getElementById('account');

if (alterTroubleSuckyKey) {
    account.addEventListener('click', function() {
        window.location.href = `../profile/profile.html`;
    });
}
else {
    account.addEventListener('click', function() {
        window.location.href = `../login/login.html`;
    });
}

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