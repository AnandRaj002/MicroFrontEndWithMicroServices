const navSlide = () => {
    const burger = document.querySelector('.burger');
    const nav = document.querySelector('.nav-links');
    const navLinks = document.querySelectorAll('.nav-links li');

    burger.addEventListener('click', () => {
        // Toggle navigation links class
        nav.classList.toggle('nav-active');

        // Toggle each navigation links
        navLinks.forEach((link, index) => {
            if (link.style.animation) {
                link.style.animation = '';
            }
            else {
                link.style.animation = `navLinkFade 0.5s ease forwards ${index / 7 + 0.4}s`;
            }            
        });

        // Toggle burger lines
        burger.classList.toggle('toggle');
    });
};

const app = () => {
    navSlide();
};

app();