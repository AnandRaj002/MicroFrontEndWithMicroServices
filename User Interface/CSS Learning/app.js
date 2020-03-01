document.querySelector('.menu-btn').addEventListener('click', () => {
    document.querySelector('.main-menu').classList.toggle('show');
    document.querySelector('.menu-btn').style.display = 'none';
    document.querySelector('.menu-btn-close').style.display = 'block';
});

document.querySelector('.menu-btn-close').addEventListener('click', () => {
    document.querySelector('.main-menu').classList.toggle('show');
    document.querySelector('.menu-btn').style.display = 'block';
    document.querySelector('.menu-btn-close').style.display = 'none';
});