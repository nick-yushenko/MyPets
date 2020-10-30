// меню на внутренних страницах


const burger = document.querySelector('.burger-back')
const menu = document.querySelector('.menu-back')
const menuClose = menu.querySelector('.menu-back .close')
const body = document.querySelector('body')
if (burger && menu && menuClose) {

  burger.addEventListener('click', function (param) {
    menu.style.display = 'block'
    body.style.overflow = 'hidden'
  })
  
  menuClose.addEventListener('click', function (param) {
    menu.style.display = 'none'
    body.style.overflow = 'auto'

  })

}
