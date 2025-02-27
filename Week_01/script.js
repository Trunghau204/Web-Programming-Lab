function toggleMenu() {
    let navLinks = document.querySelector('.nav-links');
    navLinks.classList.toggle('active');
    document.querySelectorAll('.nav-links a').forEach(link => {
        link.addEventListener('click', () => {
            navLinks.classList.remove('active');
        });
    });
}
function toggleSidebar() {
    document.querySelector('.sidebar').classList.toggle('active');
}