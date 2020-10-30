// Прелоадер с минимальной продолжительностью в 1.5 секунды

// $(window).load(function() {
//   window.setTimeout(function () {
//     $('.preloader').fadeOut().end().delay(400).fadeOut('slow');
//   }, 1000);
// });

let pageLoaded = false
const duration = 1.5
window.onload = function () {

  pageLoaded = true
  window.setTimeout(function () {
    document.body.classList.add('loaded_hiding');
    // document.body.style.overflow = 'hidden'
    document.querySelector('body').style.overflow = 'auto'
    // document.body.classList.add('loaded');
    // document.body.classList.remove('loaded_hiding');
  }, 1000 * duration);

  window.setTimeout(function () {
    document.body.classList.add('loaded');
    document.querySelector('body').style.overflow = 'auto'

    // document.body.classList.remove('loaded_hiding');
  }, 1000 * duration + 300);

  window.setTimeout(() => {
    $('.cookie').fadeIn(500);
    // document.querySelector('.cookie').
  }, 1000 * 5);
}


// $(window).on('load', function () {
//   $('body').addClass('loaded_hiding');
//   setTimeout(function () {
//     $('body').addClass('loaded');
//     $('body').removeClass('loaded_hiding');
//   }, 5000);
// })