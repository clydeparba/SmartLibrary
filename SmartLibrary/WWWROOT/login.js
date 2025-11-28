const form = document.getElementById("loginForm");
const messageEl = document.getElementById("message");

form.addEventListener("submit", (e) => {
    e.preventDefault();

    const email = document.getElementById("email").value;
    const password = document.getElementById("password").value;

    // Simple validation - just check if fields are filled
    if (email && password) {
        // Success message
        messageEl.textContent = "Login successful! Redirecting...";
        messageEl.style.color = "green";

        // Redirect to home page
        setTimeout(() => {
            window.location.href = "home.html";
        }, 500);
    } else {
        messageEl.textContent = "Please enter both email and password!";
        messageEl.style.color = "red";
    }
});