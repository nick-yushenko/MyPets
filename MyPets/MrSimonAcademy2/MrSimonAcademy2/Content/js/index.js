// плавная прокрутка 

// $("a[href*='#']").on("click", function (e) {
//   var anchor = $(this);
//   $('html, body').stop().animate({
//     scrollTop: $(anchor.attr('href')).offset().top
//   }, 777);
//   e.preventDefault();
//   return false;
// });
const anchors = document.querySelectorAll('a[href*="#"]')
anchors.forEach(function (item) {
  item.addEventListener('click', function () {
    let blockID = item.getAttribute('href').substring(1)
    $("html, body").animate({
      scrollTop: $('#' + blockID).offset().top + "px"
    }, {
      duration: 500,
      easing: "swing"
    });
  })
})


// Меню 

const burger = document.querySelector('.burger')
const menu = document.querySelector('.menu')
const menuClose = menu.querySelector('.close')


burger.addEventListener('click', function (param) {
  menu.style.display = 'block'
  body.classList.add('hidden')
  // body.style.position = 'fixed'
  // document.querySelector('html').style.overflow = 'hidden'
})

menuClose.addEventListener('click', function (param) {
  menu.style.display = 'none'
  body.classList.remove('hidden')
  // body.style.position = 'static'

  // document.querySelector('html').style.overflow = 'auto'
  isMenuOpen = false

})

const menuItems = document.querySelectorAll('.menu-item')

menuItems.forEach(item => {
  item.addEventListener('click', function (param) {
    const blockID = item.getAttribute('href').substr(1)
    body.classList.remove('hidden')
    // body.style.position = 'static'
    document.getElementById(blockID).scrollIntoView({
      behavior: 'smooth',
      block: 'start'
    })
    menu.style.display = 'none'


    // document.querySelector('html').style.overflow = 'auto'
    isMenuOpen = false

  })
})

// modal window/ Opening and closing 

const toModalButtons = document.querySelectorAll('.toModal')
const closeModalButtons = document.querySelectorAll('.close-modal')
const modalBgs = document.querySelectorAll('.modal-bg')
const toPrivacyButtons = document.querySelectorAll('.toPrivacy')

// const sended = document.querySelector('.modal.sended')
const modal = document.querySelector('.modal-brif')
const privacy = document.querySelector('.modal.privacy')
const body = document.querySelector('html body')

const courseHiddenInput = document.querySelector('#courseHidden')

toModalButtons.forEach(function (item) {
  item.addEventListener('click', function (e) {
    if (item.hasAttribute('data-value')) {
      let value = item.getAttribute('data-value')
      courseHiddenInput.setAttribute('value', value)
      console.log(courseHiddenInput)
    }
    modal.classList.add('active')
    body.classList.add('hidden')

  })
})

toPrivacyButtons.forEach(item => {
  item.addEventListener('click', function (e) {
    // closeAllModal()
    modal.classList.remove('active')
    privacy.classList.add('active')
    body.classList.add('hidden')
    // document.querySelector('#privacy').removeAttribute('checked')
  })
})


closeModalButtons.forEach(function (item) {
  item.addEventListener('click', closeAllModal)

})

function closeAllModal() {
  modal.classList.remove('active')
  // sended.classList.remove('active')
  privacy.classList.remove('active')
  body.classList.remove('hidden')
  // item.style.transform = 'rotate(90deg)'
}

modalBgs.forEach(item => {
  item.addEventListener('click', () => {
    modal.classList.remove('active')
    // sended.classList.remove('active')
    privacy.classList.remove('active')
    body.classList.remove('hidden')
  })
})


// Аккардеон для методик

const accardeonItems = document.querySelectorAll('.accardeon-item')

accardeonItems.forEach(item => {
  item.addEventListener('click', function () {

    item.classList.toggle('open')
    item.classList.toggle('close')

  })

})

// Появление мини-шапки

const headerMini = document.querySelector('.header-mini')
window.addEventListener('scroll', function () {
  if (pageYOffset >= 80)
    headerMini.classList.add('scrolled')
  else
    headerMini.classList.remove('scrolled')
  if (window.innerWidth <= 960 && pageYOffset >= 100)
    document.querySelector('.header').classList.add('scrolled_mob')
  else
    document.querySelector('.header').classList.remove('scrolled_mob')
});


// Читать дальше о нас для мобилок 
const aboutMore = document.querySelector('.about-more')

aboutMore.addEventListener('click', function (param) {
  const parent = aboutMore.parentNode
  const text = parent.querySelectorAll('p')
  text.forEach(p => {
    p.style.display = 'block'
  })
  aboutMore.style.display = 'none'
})

// Блок с курсами: 

const coursesItem = document.querySelectorAll('.courses-nav__item')

coursesItem.forEach(item => {
  item.addEventListener('click', function () {


    const lastNavBlock = document.querySelector('.courses-nav__item.current')

    let dataLastBlock = lastNavBlock.getAttribute('data-courseId')
    let dataCurrentBlock = item.getAttribute('data-courseId')

    lastNavBlock.classList.remove('current')
    item.classList.add('current')

    const lastBlock = document.querySelector('#' + dataLastBlock)
    const currentBlock = document.querySelector('#' + dataCurrentBlock)

    // lastBlock.style.opacity = '0' 
    lastBlock.classList.remove('current')
    currentBlock.classList.add('current')

    let blockID = 'none'


    if (window.innerWidth > 1024) {
      blockID = 'coursesNav'

    } else {
      blockID = 'coursesLastNavItem'

    }
    if (window.innerWidth > 1024 && window.innerHeight > 700)
      blockID = 'none'

    if (blockID != 'none')
      $("html, body").animate({
        scrollTop: $('#' + blockID).offset().top + "px"
      }, {
        duration: 500,
        easing: "swing"
      });
    // document.getElementById(blockID).scrollIntoView({
    //   behavior: 'smooth',
    //   block: 'start'
    // })


    closeCoursesMore()

  })
})

// CoursesMoreItem 

const coursesMoreItems = document.querySelectorAll('.courses-more')


coursesMoreItems.forEach(item => {
  item.addEventListener('click', function (param) {
    if (window.innerWidth <= 768) {
      const parent = item.parentNode
      console.log(parent)
      parent.querySelectorAll('.hidden').forEach(text => {
        text.classList.remove('hidden')
      })
      item.style.display = 'none'
    }

  })
})

function closeCoursesMore() {
  if (window.innerWidth <= 768) {
    coursesMoreItems.forEach(item => {
      const parent = item.parentNode
      parent.querySelectorAll('.for-hidden').forEach(text => {
        text.classList.add('hidden')
      })
      item.style.display = 'block'
    })
  }

}


// Слайдер для блока "Мы становимся семьёй"

const familyPrev = document.querySelector('#familyPrev')
const familyNext = document.querySelector('#familyNext')

const familySlides = document.querySelectorAll('.family-slide')

const familyProgress = document.querySelector('#familyProgress')
let i = 5


const familyPrevMobile = document.querySelector('#familyPrevMobile')
const familyNextMobile = document.querySelector('#familyNextMobile')
const familySlidesMobile = document.querySelectorAll('.family-slide_mobile')
const familyProgressMobile = document.querySelector('#familyProgressMobile')



var familySwiperMobile = new Swiper('.family-container.mobile', {
  speed: 500,
  spaceBetween: 100,
  centeredSlides: true,
  direction: 'horizontal',

  navigation: {
    nextEl: '#familyNextMobile',
    prevEl: '#familyPrevMobile',
  },
});

familySwiperMobile.on('slideChange', function () {
  if (familySwiperMobile.activeIndex == 0)
    familyPrevMobile.classList.remove('clickable')
  else
    familyPrevMobile.classList.add('clickable')
  if (familySwiperMobile.activeIndex == (familySlidesMobile.length - 1))
    familyNextMobile.classList.remove('clickable')
  else
    familyNextMobile.classList.add('clickable')

  console.log(familySwiperMobile.activeIndex)
  familyProgressMobile.innerHTML = (familySwiperMobile.activeIndex + 1) + "<span>/" + familySlidesMobile.length + "</span>"

});



// Слайдер для сертификатов

var certificateSwiper = new Swiper('.certificate-container', {
  speed: 500,
  spaceBetween: 30,
  slidesPerView: 1,
  direction: 'horizontal',
  navigation: {
    nextEl: '#certificateNext',
    prevEl: '#certificatePrev',
  },
  breakpoints: {

    500: {
      spaceBetween: 30,

      slidesPerView: 3,
    }
  }
});


const certificatePrev = document.querySelector('#certificatePrev')
const certificateNext = document.querySelector('#certificateNext')

const certificateSlides = document.querySelectorAll('.certificate-slide')

const certificateProgress = document.querySelector('#certificateProgress')

let length = certificateSlides.length - 2
if (window.innerWidth >= 500) {
  certificateSwiper.slideTo(1)

  certificateSlides[certificateSwiper.activeIndex + 1].style.transform = 'scale(1.2)'
  certificateSlides[certificateSwiper.activeIndex + 1].style.zIndex = '99'
}


certificateProgress.innerHTML = (certificateSwiper.activeIndex + 1) + "<span>/" + (certificateSlides.length - 2) + "</span>"


certificateSwiper.on('slideChange', function () {
  // scaling active slide
  if (window.innerWidth >= 500) {
    certificateSlides.forEach(item => {
      item.style.transform = ''
      certificateSlides[certificateSwiper.activeIndex + 1].style.zIndex = '1'

    })
    certificateSlides[certificateSwiper.activeIndex + 1].style.transform = 'scale(1.2)'
    certificateSlides[certificateSwiper.activeIndex + 1].style.zIndex = '99'

  } else {
    certificateSlides.forEach(item => {
      item.style.transform = ''
      certificateSlides[certificateSwiper.activeIndex].style.zIndex = '1'

    })
  }

  // make navigation clickable
  if (certificateSwiper.activeIndex == 0)
    certificatePrev.classList.remove('clickable')
  else
    certificatePrev.classList.add('clickable')

  if (certificateSwiper.activeIndex == (certificateSlides.length - 3))
    certificateNext.classList.remove('clickable')
  else
    certificateNext.classList.add('clickable')

  // set slider progress 
  certificateProgress.innerHTML = (certificateSwiper.activeIndex + 1) + "<span>/" + (certificateSlides.length - 2) + "</span>"
});



// Слайдер отзывов 

var flampSwiper = new Swiper('.flamp-container', {
  speed: 500,
  spaceBetween: 10,
  centeredSlides: false,
  direction: 'horizontal',
  // slidesPerView: 3,
  // autoHeight: true, 
  slidesPerView: 'auto',
  navigation: {
    nextEl: '#flampNext',
    prevEl: '#flampPrev',
  },
  breakpoints: {

    768: {
      spaceBetween: 30,
      centeredSlides: true,
    }
  }
});



const flampPrev = document.querySelector('#flampPrev')
const flampNext = document.querySelector('#flampNext')

const flampSlides = document.querySelectorAll('.flamp-slide')

const flampProgress = document.querySelector('#flampProgress')

let flampLength = flampSlides.length + 1

flampSwiper.slideTo(2)
flampProgress.innerHTML = (flampSwiper.activeIndex + 1) + "<span>/" + (flampSlides.length) + "</span>"



flampSwiper.on('slideChange', function () {
  // make navigation clickable
  if (flampSwiper.activeIndex == 0)
    flampPrev.classList.remove('clickable')
  else
    flampPrev.classList.add('clickable')

  if (flampSwiper.activeIndex == (flampSlides.length - 1))
    flampNext.classList.remove('clickable')
  else
    flampNext.classList.add('clickable')

  // set slider progress 
  flampProgress.innerHTML = (flampSwiper.activeIndex + 1) + "<span>/" + (flampSlides.length) + "</span>"

  const activeSlide = document.querySelector('.flamp-slide.swiper-slide-active')
  const more = activeSlide.querySelector('.flamp-more')
  const moreText = activeSlide.querySelector('.flamp-more-text')
  if (more) {

    more.classList.remove('hidden')
    moreText.classList.add('hidden')

  }
});



flampSlides.forEach(item => {

  const more = item.querySelector('.flamp-more')
  const moreText = item.querySelector('.flamp-more-text')
  if (more)
    more.addEventListener('click', function (param) {
      more.classList.add('hidden')
      moreText.classList.remove('hidden')
    })
})


// Валидация формы 
$('input[name="phone"').mask('0 (000) 000 00-00')

$('#brif').validate({
  rules: {
    Name: {
      required: true
    },
    phone: {
      required: true,
      // minlenght: 18
    },
    privacy: {
      required: true,
    },
    language: {
      required: true,
    },


  },
  messages: {
    Name: {
      required: jQuery.validator.format("Укажите имя")
    },
    phone: {
      minlength: jQuery.validator.format("Номер указан не полностью"),
      required: jQuery.validator.format("Укажите телефон")
    },
    privacy: {
      required: jQuery.validator.format("Прочтите это")
    },
    language: {
      required: jQuery.validator.format("Выберете язык")
    }
  },
  errorElement: "div",
  errorClass: "invalid",

});



$('#modalBrif').validate({
  rules: {
    Name: {
      required: true
    },
    phone: {
      required: true,
      // minlenght: 18
    },
    privacy: {
      required: true,
    },
    language: {
      required: true,
    },


  },
  messages: {
    Name: {
      required: jQuery.validator.format("Укажите имя")
    },
    phone: {
      minlength: jQuery.validator.format("Номер указан не полностью"),
      required: jQuery.validator.format("Укажите телефон")
    },
    privacy: {
      required: jQuery.validator.format("Прочтите это")
    },
    language: {
      required: jQuery.validator.format("Выберете язык")
    }
  },
  errorElement: "div",
  errorClass: "invalid",

});