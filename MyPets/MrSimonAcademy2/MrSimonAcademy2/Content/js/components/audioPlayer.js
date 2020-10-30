(function () {

  const fileWrap = document.querySelector('.theme-file__wrap')
  const auPlayer = document.querySelector('.player-audio')
  const closePlayer = document.querySelector('.player .close')
  const playerUrlInput = document.querySelector('.js-player-audio-src')

  const studentToAu = document.querySelectorAll('.js-StudentListen')
  let trackNameBlock = null
  if (auPlayer)
    trackNameBlock = auPlayer.querySelector('.au-name')

  // Динамическая ширина названия аудио
  if (window.innerWidth < 425 && trackNameBlock) {

    let name = trackNameBlock.textContent

    if (name.length < 20) {
      trackNameBlock.style.fontSize = 30 + 'px'
      trackNameBlock.style.lineHeight = 36 + 'px'
    }
    if (name.length >= 20 && name.length < 40) {
      trackNameBlock.style.fontSize = 28 + 'px'
      trackNameBlock.style.lineHeight = 34 + 'px'
    }
    if (name.length >= 40 && name.length < 60) {
      trackNameBlock.style.fontSize = 26 + 'px'
      trackNameBlock.style.lineHeight = 32 + 'px'
    }
    if (name.length >= 60 && name.length < 80) {
      trackNameBlock.style.fontSize = 24 + 'px'
      trackNameBlock.style.lineHeight = 30 + 'px'
    }
    if (name.length >= 80 && name.length < 100) {
      trackNameBlock.style.fontSize = 22 + 'px'
      trackNameBlock.style.lineHeight = 28 + 'px'
    }
    if (name.length >= 100 && name.length < 120) {
      trackNameBlock.style.fontSize = 20 + 'px'
      trackNameBlock.style.lineHeight = 26 + 'px'
    }

  }
  console.log(auPlayer)
  console.log(closePlayer)
  if (auPlayer && closePlayer) {
    document.querySelector('body').addEventListener('click', (e) => {
      if (e.target.classList.contains('js-toAuPlayer') || e.target.parentElement.classList.contains('js-toAuPlayer')) {
        let btn
        if (e.target.classList.contains('js-toAuPlayer'))
          btn = e.target
        else
          e.target.parentElement


        let url = btn.getAttribute('data-url')
        let trackName = btn.getAttribute('data-name')
        if (url == 'none' || url == null || url == '') { // Ссылка не установлнеа 
          auPlayer.classList.add('notFound')
        } else {
          if (isNotFound(url)) { // Если файл не найден 
            auPlayer.classList.add('notFound')
          } else {
            playerUrlInput.setAttribute('value', url)
            trackNameBlock.innerHTML = trackName
            auPlayer.classList.remove('notFound')
          }

        }


        closePlayer.parentElement.classList.add('active')
        document.querySelector('body').style.overflow = 'hidden'
        setAudioPlayer(auPlayer)
      }
    })




    closePlayer.addEventListener('click', () => {
      closePlayer.parentElement.classList.remove('active')
      document.querySelector('body').style.overflow = 'auto'

    })



  }

  function isNotFound(url) {
    // var http = new XMLHttpRequest();

    // http.open('HEAD', url, false);
    // http.send();

    // return (http.status == 404) ? true : false;
    return false
  }

  function setAudioPlayer(player) {
    let url = player.querySelector('.js-player-audio-src').getAttribute('value')

    var dragObject = {};
    var audio = new Audio(url);


    const elements = {
      playBtn: player.querySelector('.play'),
      pauseBtn: player.querySelector('.pause'),
      repeatBtn: player.querySelector('.repeat'),
      volumeOnBtn: player.querySelector('.volume-on'),
      volumeOffBtn: player.querySelector('.volume-off'),
      time: player.querySelector('.au-pos'),
      thumb: player.querySelector('.au-thumb')
    }

    elements.thumb.querySelector('.au-thumb__progress').style.width = 0
    const close = document.querySelector('.player .close')


    // Закрытие всего плеера 
    close.addEventListener('click', () => {
      pause(audio)
      elements.playBtn.classList.add('act')
      elements.pauseBtn.classList.remove('act')
      audio = null
    })

    if (audio != null) {
      // Управление аудио кнопками навигации
      elements.playBtn.addEventListener('click', () => {
        play(audio)
        elements.playBtn.classList.remove('act')
        elements.pauseBtn.classList.add('act')
      })
      elements.pauseBtn.addEventListener('click', () => {
        pause(audio)
        elements.playBtn.classList.add('act')
        elements.pauseBtn.classList.remove('act')
      })
      elements.repeatBtn.addEventListener('click', () => {
        repeat(audio)
      })
      elements.volumeOffBtn.addEventListener('click', () => {
        volumeOff(audio)
        elements.volumeOffBtn.classList.remove('act')
        elements.volumeOnBtn.classList.add('act')
      })
      elements.volumeOnBtn.addEventListener('click', () => {
        volumeOn(audio)
        elements.volumeOffBtn.classList.add('act')
        elements.volumeOnBtn.classList.remove('act')
      })

      // Управление аудио посредством ползунка (thumb)
      document.onmousedown = function (e) {

        if (e.oncontextmenu == 1) { // если клик правой кнопкой мыши
          return; // то он не запускает перенос
        }

        if (e.target.classList.contains('au-thumb') || e.target.parentElement.classList.contains('au-thumb')) { // Нажатие именно на ползунок

          play(audio)
          elements.playBtn.classList.remove('act')
          elements.pauseBtn.classList.add('act')
          // ползунок
          const progressThumb = elements.thumb.querySelector('.au-thumb__progress')

          // Расщет необходимой ширины ползунка и нового момента аудио
          let width = elements.thumb.offsetWidth
          let curPos = e.offsetX
          let progress = audio.duration / (width / curPos)
          progressThumb.style.width = curPos + 'px'
          // Перемотка аудио до нужного момента
          setProgress(audio, progress)



          if (!progressThumb) return; // не нашли, клик вне draggable-объекта

          // запомнить переносимый объект
          dragObject.elem = progressThumb;

          // запомнить координаты, с которых начат перенос объекта
          dragObject.downX = e.pageX;

        }
      }
      document.onmousemove = function (e) {
        if (!dragObject.elem) return; // элемент не зажат

        // Ползуное и его движение
        const progressThumb = elements.thumb.querySelector('.au-thumb__progress')
        let width = elements.thumb.offsetWidth
        let curPos = e.offsetX
        let progress = audio.duration / (width / curPos)

        // Вышли за пределы ползунка 
        if (curPos <= 0 || curPos > width) {
          setProgress(audio, dragObject.progress)
          dragObject = {};

        } else {
          progressThumb.style.width = curPos + 'px'

          setTime(elements.time, getTime(progress), getTime(audio.duration))

          dragObject.progress = progress
        }

      }
      document.onmouseup = function (e) {
        // (1) обработать перенос, если он идёт
        if (dragObject.elem && dragObject.progress) {
          setProgress(audio, dragObject.progress)
        }

        // в конце mouseup перенос либо завершился, либо даже не начинался
        // (2) в любом случае очистим "состояние переноса" dragObject
        dragObject = {};
      }

      // События аудио


      audio.addEventListener('timeupdate', (event) => {
        // Изменить показываемое время
        if (audio != null) {

          setTime(elements.time, getTime(audio.currentTime), getTime(audio.duration))
          if (dragObject.elem && dragObject.progress) {} else { // Изменение положения ползунка, если его сейчас не двигают 
            const progressThumb = elements.thumb.querySelector('.au-thumb__progress')
            progressThumb.style.width = (elements.thumb.offsetWidth / audio.duration) * audio.currentTime + 'px'
          }

          // Если аудио закончено
          if (audio.currentTime == audio.duration) {
            elements.playBtn.classList.add('act')
            elements.pauseBtn.classList.remove('act')
          }
        }
      });
      audio.addEventListener('durationchange', (event) => {
        // Изменить показываемое время
        if (audio != null) {

          setTime(elements.time, '00:00', getTime(audio.duration))
        }
      });
    }




  }

  function play(audio) {
    audio.play()
  }

  function pause(audio) {
    audio.pause()
  }

  function repeat(audio) {
    audio.currentTime = 0.0

  }

  function volumeOn(audio) {
    audio.volume = 0
  }

  function volumeOff(audio) {
    audio.volume = 1
  }

  function setTime(block, cur, dur) {
    block.innerHTML = cur + ' / ' + dur
  }

  function getTime(time) {
    let min = time / 60
    let sec = time % 60

    if (min > 60) {
      let hour = min / 60
      min = min % 60
      return '' + parseInt(hour) + ':' + parseInt(min / 10) + parseInt(min % 10) + ':' + parseInt(sec / 10) + parseInt(sec % 10);
    }


    return '' + parseInt(min / 10) + parseInt(min % 10) + ':' + parseInt(sec / 10) + parseInt(sec % 10);
  }

  function setProgress(audio, progress) {
    audio.currentTime = progress
  }


})()