const apiUrl = "http://localhost:5000/api/auth";

async function register() {
    const username = document.getElementById("reg-username").value;
    const password = document.getElementById("reg-password").value;

    const response = await fetch(`${apiUrl}/register`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify({ Username: username, PasswordHash: password, Role: "Student" }),
    });

    if (response.ok) {
        alert("Registration successful");
    } else {
        alert("Registration failed");
    }
}

async function login() {
    const username = document.getElementById("login-username").value;
    const password = document.getElementById("login-password").value;

    const response = await fetch(`${apiUrl}/login`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify({ Username: username, PasswordHash: password }),
    });

    if (response.ok) {
        const data = await response.json();
        alert("Login successful");
        localStorage.setItem("token", data.Token);
    } else {
        alert("Login failed");
    }
}
