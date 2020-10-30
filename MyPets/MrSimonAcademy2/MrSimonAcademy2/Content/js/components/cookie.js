try {
  document.querySelector('#AcceptCookie').addEventListener('click', function () {
    $('.cookie').fadeOut(200);
    $('.cookie-form').fadeOut(200);
  })
} catch {
  console.log("Разрешение на использование файлов Cookie получено")
}