window.addEventListener('orientationchange', function () {
  window.location.reload()
});

// Загрузка файлов 

$('.js-fileType').change(function () {
  if (this.getAttribute('value') == 'Link') {
    $('.js-inputForFile').css('display', 'none')
    $('.js-inputForLink').css('display', 'block')
  } else {
    $('.js-inputForLink').css('display', 'none')
    $('.js-inputForFile').css('display', 'block')
  }

});
$('#AddFile').change(function () {
  if ($(this).val() != '') {
    $(this).prev().text('Файл выбран');

    readFileURL(this);
  } else $(this).prev().text('Выберите файлы');
});

$('#avatarInput').change(function () {
  if ($(this).val() != '') {
    $(this).prev().text('Файл выбран');
    var fileTypes = ['jpg', 'jpeg', 'png'];

    readURL(this, fileTypes);
  } else $(this).prev().text('Выберите файлы');
});

$("#avatarEmpty").change(function () {
  if ($(this).prop('checked')) {
    // $(this).clone().prependTo($('#right'));
    $('#avatar').removeAttr('src');
    $('#avatarInput').prev().text('Выберите файлы');
    $('#avatarInput').val('');
    $('#avatar').parent().css('display', 'none')
  } else {

    // $(this).detach();

  }

});


function readURL(input, fileTypes) {

  if (input.files && input.files[0]) {

    var extension = input.files[0].name.split('.').pop().toLowerCase(), //file extension from input file
      isSuccess = fileTypes.indexOf(extension) > -1;


    if (isSuccess) {
      var reader = new FileReader();
      var imgBlock = input.getAttribute('data-img-id')

      reader.onload = function (e) {

        $('#' + imgBlock).attr('src', e.target.result);
        $('#' + imgBlock).parent().css('display', 'block')
        $('#avatarEmpty').removeAttr('checked')
      };

      reader.readAsDataURL(input.files[0]);
    } else {
      $('#avatarInput').prev().text('Выберите файлы');
      $('#avatarInput').val('');
      alert('Данный формат запрещен. Доступные расширения: .jpg, .jpeg, .png')
    }
  }
}


function readFileURL(input) {

  if (input.files && input.files[0]) {

    // var extension = input.files[0].name.split('.').pop().toLowerCase(), //file extension from input file
    //   isSuccess = fileTypes.indexOf(extension) > -1;


    // if (isSuccess) {
    var reader = new FileReader();
    reader.onload = function (e) {
      document.querySelector('.file-list p').innerHTML = 'Имя файла: ' + input.files.item(0).name
    };

    reader.readAsDataURL(input.files[0]);
    // } else {
    //   $('#avatarInput').prev().text('Выберите файлы');
    //   $('#avatarInput').val('');
    //   alert('Данный формат запрещен. Доступные расширения: .jpg, .jpeg, .png')
    // }
  }
}


// плавная прокрутка 

const anchors = document.querySelectorAll('a[href*="#"]')
if (anchors.length != 0) {
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
}

// Страница списка групп (Роль = учитель, админ)

let grouplistItems = document.querySelectorAll('.grouplist-item')
let grouplistWrap = document.querySelector('.grouplist-wrap')

if (grouplistWrap) {
  grouplistWrap.addEventListener('click', function (e) {

    if (e.target.classList.contains('count')) {
      let parentItem = e.target.parentElement.parentElement.parentElement
      parentItem.classList.add('js-showStudents')
      parentItem.classList.add('opened')

      // Исключение пользователя из группы
      try {
        let groupId = parentItem.getAttribute("data-groupId")
        let excludeGroupId = parentItem.querySelector('.excludeGroupId')
        excludeGroupId.setAttribute('value', groupId)
      } catch {}
    }
    if (e.target.classList.contains('toList') || (e.target.tagName == 'SPAN' && e.target.parentElement.classList.contains('toList')) ||
      (e.target.tagName == 'IMG' && e.target.parentElement.classList.contains('toList'))) {
      let parentItem
      if (e.target.classList.contains('toList'))
        parentItem = e.target.parentElement.parentElement.parentElement
      else
        parentItem = e.target.parentElement.parentElement.parentElement.parentElement
      parentItem.classList.add('js-showStudents')
      parentItem.classList.add('opened')

      // Исключение пользователя из группы
      try {
        let groupId = parentItem.getAttribute("data-groupId")
        let excludeGroupId = parentItem.querySelector('.excludeGroupId')
        excludeGroupId.setAttribute('value', groupId)
      } catch {}
    }

    if (e.target.classList.contains('back') || (e.target.tagName == 'IMG' && e.target.parentElement.classList.contains('back'))) {
      let parentItem
      if (e.target.classList.contains('back'))
        parentItem = e.target.parentElement.parentElement.parentElement
      else
        parentItem = e.target.parentElement.parentElement.parentElement.parentElement

      parentItem.classList.remove('js-showStudents')
      // parentItem.classList.remove('opened')
    }

    if (e.target.classList.contains('open') || (e.target.tagName == 'IMG' && e.target.parentElement.classList.contains('open'))) {
      let parentItem
      if (e.target.classList.contains('open'))
        parentItem = e.target.parentElement.parentElement.parentElement
      else {
        parentItem = e.target.parentElement.parentElement.parentElement.parentElement
      }
      parentItem.classList.toggle('opened')
      parentItem.classList.remove('js-showStudents')

      // Исключение пользователя из группы
      try {
        let groupId = parentItem.getAttribute("data-groupId")
        let excludeGroupId = parentItem.querySelector('.excludeGroupId')
        excludeGroupId.setAttribute('value', groupId)
      } catch {}
    }

    // Статистика для каждого пользователя
    if ((e.target.tagName == 'SPAN' && e.target.parentElement.tagName == 'P' && e.target.parentElement.parentElement.classList.contains('list')) ||
      (e.target.tagName == 'P' && e.target.parentElement.classList.contains('list'))) {

      let user
      if (e.target.tagName == 'P') {
        user = e.target
      } else {
        user = e.target.parentElement
      }

      let userId = user.getAttribute('data-id')
      let userName = user.getAttribute('data-UserName')

      let addStatistic = user.parentElement.parentElement.querySelector('.js-toAddingStatistic')

      addStatistic.setAttribute('data-id', userId)
      addStatistic.setAttribute('data-user', userName)
      user.parentElement.parentElement.querySelector('.list-content').classList.add('active')

      let dataImg = user.getAttribute('data-img-url')
      let isEmpty = user.getAttribute('data-isEmpty')

      const imgContainer = user.parentElement.parentElement.querySelector('.list-content__avatar img')
      imgContainer.setAttribute('src', dataImg)
      if (isEmpty == "true")
        imgContainer.classList.add('empty')
      else
        imgContainer.classList.remove('empty')

      try {
        let excludeUserId = user.parentElement.parentElement.querySelector('.excludeUserId')
        excludeUserId.setAttribute('value', userId)
      } catch {}
      // Id группы передается, при отрктии карточки

    }

    AddStatistic(e)
    AddMessage(e)
    AddTheme(e)
  })

}
// Страница списка пользователей (Роль = учитель, админ)

let userlistWrap = document.querySelector('.userlist-wrap')

if (userlistWrap) {

  userlistWrap.addEventListener('click', function (e) {

    if (e.target.classList.contains('open') || (e.target.tagName == 'IMG' && e.target.parentElement.classList.contains('open'))) {

      let parentItem
      if (e.target.classList.contains('open'))
        parentItem = e.target.parentElement.parentElement.parentElement
      else {
        parentItem = e.target.parentElement.parentElement.parentElement.parentElement
      }
      parentItem.classList.toggle('opened')
    }

    AddStatistic(e)
    AddMessage(e)
  })


}
// Страница админ панели 

const adminNav = document.querySelectorAll('.admin-item ')
const adminContent = document.querySelector('.admin-content')

if (adminNav && adminContent) {

  adminNav.forEach(function (item) {
    item.addEventListener('click', function (e) {
      document.querySelector('.admin-item.current').classList.remove('current')
      item.classList.add('current')
      let locationClass = item.getAttribute('data-location')
      document.querySelectorAll('.admin-content__item').forEach(function (item) {
        item.classList.remove('opened')
      })
      document.querySelector('.' + locationClass).classList.add('opened')
    })
  })
}
const toAddingUser = document.querySelector('.js-AddUser')
const toAddingGroup = document.querySelector('.js-AddGroup')
const toUserList = document.querySelectorAll('.js-backToUserList')
const toGroupList = document.querySelectorAll('.js-backToGroupList')
const toAddingStatistic = document.querySelectorAll('.js-toAddingStatistic')

const toAddingMessageForAll = document.querySelector('.js-AddMessageForAll')
const toAllMessages = document.querySelectorAll('.js-backToAllMessages')

if (toAddingUser) {
  toAddingUser.addEventListener('click', function (e) {
    document.querySelectorAll('.admin-content__item').forEach(function (item) {
      item.classList.remove('opened')
    })
    document.querySelector('.js-admin-adduser').classList.add('opened')
  })
}
if (toUserList) {
  toUserList.forEach(item => {
    item.addEventListener('click', function (e) {
      let locationClass = item.getAttribute('data-location')
      document.querySelectorAll('.admin-content__item').forEach(function (item) {
        item.classList.remove('opened')
      })
      document.querySelector('.' + locationClass).classList.add('opened')
    })
  })

}
if (toAddingGroup) {
  toAddingGroup.addEventListener('click', function (e) {
    document.querySelectorAll('.admin-content__item').forEach(function (item) {
      item.classList.remove('opened')
    })
    document.querySelector('.js-admin-addgroup').classList.add('opened')
  })
}
if (toGroupList) {
  toGroupList.forEach(item => {
    item.addEventListener('click', function (e) {
      let locationClass = item.getAttribute('data-location')
      if (!document.querySelector('.' + locationClass))
        locationClass = item.getAttribute('data-second-location')
      document.querySelectorAll('.admin-content__item').forEach(function (item) {
        item.classList.remove('opened')
      })

      document.querySelector('.' + locationClass).classList.add('opened')
    })
  })

}

if (toAddingMessageForAll) {
  toAddingMessageForAll.addEventListener('click', function (e) {
    document.querySelectorAll('.admin-content__item').forEach(function (item) {
      item.classList.remove('opened')
    })
    document.querySelector('.js-admin-AddMessageForAll').classList.add('opened')
  })
}
if (toAllMessages) {
  toAllMessages.forEach(item => {
    item.addEventListener('click', function (e) {
      let locationClass = item.getAttribute('data-location')
      document.querySelectorAll('.admin-content__item').forEach(function (item) {
        item.classList.remove('opened')
      })
      document.querySelector('.' + locationClass).classList.add('opened')
    })
  })

}

function AddStatistic(e) {
  if (e.target.classList.contains('js-toAddingStatistic') ||
    (e.target.tagName == 'IMG' && e.target.parentElement.classList.contains('js-toAddingStatistic')) ||
    (e.target.tagName == 'SPAN' && e.target.parentElement.classList.contains('js-toAddingStatistic'))) {

    const userIdInput = document.querySelector('#js-UserIdAddStatistic')
    const userNameBlock = document.querySelector('#js-UserNameAddStatistic')

    document.querySelectorAll('.admin-content__item').forEach(function (item) {
      item.classList.remove('opened')
    })

    let btn
    if (e.target.classList.contains('js-toAddingStatistic'))
      btn = e.target
    else
      btn = e.target.parentElement
    userIdInput.setAttribute('value', btn.getAttribute('data-id'))
    userNameBlock.innerHTML = '(' + btn.getAttribute('data-user') + ')'
    document.querySelector('.js-admin-addstatistic').classList.add('opened')
  }
}

function AddMessage(e) {
  if (e.target.classList.contains('js-toAddingMessage') ||
    (e.target.tagName == 'IMG' && e.target.parentElement.classList.contains('js-toAddingMessage')) ||
    (e.target.tagName == 'SPAN' && e.target.parentElement.classList.contains('js-toAddingMessage'))) {

    const targetIdInput = document.querySelector('#js-TargetIdAddMessage')
    const targetTypeInput = document.querySelector('#js-TargetTypeAddMessage')
    const targetNameBlock = document.querySelector('#js-TargetNameAddMessage')

    document.querySelectorAll('.admin-content__item').forEach(function (item) {
      item.classList.remove('opened')
    })

    let btn
    if (e.target.classList.contains('js-toAddingMessage'))
      btn = e.target
    else
      btn = e.target.parentElement
    targetIdInput.setAttribute('value', btn.getAttribute('data-id'))
    targetTypeInput.setAttribute('value', "USER")
    targetNameBlock.innerHTML = '(' + btn.getAttribute('data-user') + ')'

    document.querySelector('.js-admin-addmessage').classList.add('opened')


  }
}

// Страница новостей (админ, учитель)
const informationWrap = document.querySelector('.information-wrap')

if (informationWrap) {
  informationWrap.addEventListener('click', function (e) {

    if (e.target.classList.contains('information-item') || (e.target.tagName == 'H3' && e.target.parentElement.classList.contains('information-item')) ||
      (e.target.tagName == 'P' && e.target.parentElement.classList.contains('information-item')) ||
      (e.target.tagName == 'SPAN' && e.target.parentElement.classList.contains('information-item'))) {

      let parentItem
      if (e.target.classList.contains('information-item'))
        parentItem = e.target
      else {
        parentItem = e.target.parentElement
      }
      informationWrap.querySelectorAll('.information-item').forEach(item => {
        item.classList.remove('opened')
      })
      parentItem.classList.toggle('opened')
    }

  })
}

// Страница тем (админ или учитель)
const themeAdminWrap = document.querySelector('.theme-admin')
const themeFileWrap = document.querySelector('.theme-file__wrap')
const toAddingFile = document.querySelectorAll('.js-AddFile')

if (toAddingFile) {
  toAddingFile.forEach(btn => {
    btn.addEventListener('click', function (e) {
      document.querySelectorAll('.admin-content__item').forEach(function (item) {
        item.classList.remove('opened')
      })
      document.querySelector('.js-admin-AddFile').classList.add('opened')
    })


  })
}

if (themeAdminWrap) {
  themeAdminWrap.addEventListener('click', function (e) {

    let className = e.target.classList
    let tagName = e.target.tagName
    if (className.contains('group-theme') || className.contains('group-theme__head') ||
      (className.contains('name') && e.target.parentElement.classList.contains('group-theme__head')) ||
      (tagName == 'SPAN' && e.target.parentElement.classList.contains('name') &&
        e.target.parentElement.parentElement.classList.contains('group-theme__head'))) {


      let parentItem
      if (e.target.classList.contains('group-theme__head'))
        parentItem = e.target.parentElement
      if (e.target.classList.contains('group-theme'))
        parentItem = e.target
      if (className.contains('name'))
        parentItem = e.target.parentElement.parentElement
      if (tagName == 'SPAN')
        parentItem = e.target.parentElement.parentElement.parentElement



      if (parentItem.classList.contains('opened')) {
        parentItem.classList.remove('opened')
      } else {
        themeAdminWrap.querySelectorAll('.group-theme').forEach(item => {
          item.classList.remove('opened')
        })
        parentItem.classList.add('opened')

      }





    }

    AddTheme(e)
    // ToTheme(e)
  })
}

if (themeFileWrap) {
  themeFileWrap.addEventListener('click', (e) => {
    if (e.target.classList.contains('theme-file__head') || e.target.parentElement.classList.contains('theme-file__head')) {

      let parentItem
      if (e.target.classList.contains('theme-file__head')) {
        parentItem = e.target.parentElement
      } else
        parentItem = e.target.parentElement.parentElement

      themeFileWrap.querySelectorAll('.theme-file__item').forEach(item => {
        item.classList.remove('opened')
      })
      parentItem.classList.toggle('opened')

    }


  })
}

function AddTheme(e) {
  if (e.target.classList.contains('js-toAddingTheme') ||
    (e.target.tagName == 'IMG' && e.target.parentElement.classList.contains('js-toAddingTheme')) ||
    (e.target.tagName == 'SPAN' && e.target.parentElement.classList.contains('js-toAddingTheme'))) {

    const returnIdInput = document.querySelector('#js-returnUrlIdAddTheme')
    const targetIdInput = document.querySelector('#js-TargetIdAddTheme')
    const targetNameBlock = document.querySelector('#js-TargetNameAddTheme')

    document.querySelectorAll('.admin-content__item').forEach(function (item) {
      item.classList.remove('opened')
    })

    let btn
    if (e.target.classList.contains('js-toAddingTheme'))
      btn = e.target
    else
      btn = e.target.parentElement
    targetIdInput.setAttribute('value', btn.getAttribute('data-id'))
    returnIdInput.setAttribute('value', btn.getAttribute('data-reternUrl'))
    targetNameBlock.innerHTML = '(' + btn.getAttribute('data-group') + ')'

    document.querySelector('.js-admin-addtheme').classList.add('opened')


  }
}


// Страница студента (задания)
const studentNav = document.querySelectorAll('.student-nav__item')
const studentBlocks = document.querySelectorAll('.student-block')
if (studentNav && studentBlocks) {

  studentNav.forEach(item => {
    item.addEventListener('click', () => {

      studentBlocks.forEach(block => block.classList.remove('opened'))

      let location = item.getAttribute('data-location')

      document.querySelector('.student-block.js-' + location).classList.add('opened')
      studentNav.forEach(nav => nav.classList.remove('current'))

      item.classList.add('current')
    })
  })

  studentBlocks.forEach(item => {
    item.addEventListener('click', (e) => {

      if (e.target.classList.contains('student-file__head') || e.target.parentElement.classList.contains('student-file__head')) {
        let target = (e.target.classList.contains('student-file__head')) ? e.target.parentElement : e.target.parentElement.parentElement;

        item.querySelectorAll('.student-file').forEach(item => item.classList.remove('opened'))

        target.classList.add('opened')

      }

    })
  })
}


$('#AddStatisticForm').validate({
  rules: {
    lessonsCount: {
      required: true,
      range: [1, Infinity]
    },
    loseLessons: {
      required: true,
      range: [0, Infinity]
      // minlenght: 18
    },
    homeworkCount: {
      required: true,
      range: [0, Infinity]
    },
    passedHomework: {
      required: true,
      range: [0, Infinity]
    },
    activity: {
      required: true,
      range: [1, 100]
    },
    concentration: {
      required: true,
      range: [1, 100],

    },
    Speaking: {
      required: true,
      range: [1, 100],
    },
    Listening: {
      required: true,
      range: [1, 100],
    },
    Writing: {
      required: true,
      range: [1, 100],
    },
    Reading: {
      required: true,
      range: [1, 100],
    },
  },
  messages: {
    lessonsCount: {
      required: jQuery.validator.format("Это обязательное поле"),
      range: jQuery.validator.format("Не может быть меньше 0"),
    },
    loseLessons: {
      required: jQuery.validator.format("Это обязательное поле"),
      range: jQuery.validator.format("Не может быть меньше 0"),
      // minlenght: 18
    },
    homeworkCount: {
      required: jQuery.validator.format("Это обязательное поле"),
      range: jQuery.validator.format("Не может быть меньше 0"),
    },
    passedHomework: {
      required: jQuery.validator.format("Это обязательное поле"),
      range: jQuery.validator.format("Не может быть меньше 0"),
    },
    activity: {
      required: jQuery.validator.format("Это обязательное поле"),
      range: jQuery.validator.format("Целое число от 1 до 100"),
    },
    concentration: {
      required: jQuery.validator.format("Это обязательное поле"),
      range: jQuery.validator.format("Целое число от 1 до 100"),

    },
    Speaking: {
      required: jQuery.validator.format("Это обязательное поле"),
      range: jQuery.validator.format("Целое число от 1 до 100"),
    },
    Listening: {
      required: jQuery.validator.format("Это обязательное поле"),
      range: jQuery.validator.format("Целое число от 1 до 100"),
    },
    Writing: {
      required: jQuery.validator.format("Это обязательное поле"),
      range: jQuery.validator.format("Целое число от 1 до 100"),
    },
    Reading: {
      required: jQuery.validator.format("Это обязательное поле"),
      range: jQuery.validator.format("Целое число от 1 до 100"),
    },
  },
  errorElement: "div",
  errorClass: "invalid",

});

$('#AddGroupForm').validate({
  rules: {
    language: {
      required: true,
    },
    isPersonal: {
      required: true,
    },
    GroupName: {
      required: true,
    },
    GroupLevel: {
      required: true,
    },
    days: {
      required: true,
    },
    textbook: {
      required: true,
    },

  },
  messages: {
    language: {
      required: jQuery.validator.format("Это обязательное поле"),
    },
    isPersonal: {
      required: jQuery.validator.format("Это обязательное поле"),
    },
    GroupName: {
      required: jQuery.validator.format("Это обязательное поле"),
    },
    GroupLevel: {
      required: jQuery.validator.format("Это обязательное поле"),
    },
    days: {
      required: jQuery.validator.format("Это обязательное поле"),
    },
    textbook: {
      required: jQuery.validator.format("Это обязательное поле"),
    },

  },
  errorElement: "div",
  errorClass: "invalid",

});

$('#AddUserForm').validate({
  rules: {
    role: {
      required: true,
    },
    UserFName: {
      required: true,
    },
    UserLName: {
      required: true,
    },
    level: {
      required: true,
      // maxlength: 2
    },

    date: {
      required: true,
    },
    Email: {
      required: true,
      email: true,
    },
    Password: {
      required: true,
    },
    PasswordConfirm: {
      required: true,
    },

  },
  messages: {
    role: {
      required: jQuery.validator.format("Это обязательное поле"),
    },
    UserFName: {
      required: jQuery.validator.format("Это обязательное поле"),
    },
    UserLName: {
      required: jQuery.validator.format("Это обязательное поле"),
    },
    level: {
      required: jQuery.validator.format("Это обязательное поле"),
      // maxlength: 2
    },
    date: {
      required: jQuery.validator.format("Это обязательное поле"),
    },
    Email: {
      required: jQuery.validator.format("Это обязательное поле"),
      email: jQuery.validator.format("Укажите E-mail"),
    },
    Password: {
      required: jQuery.validator.format("Это обязательное поле"),
    },
    PasswordConfirm: {
      required: jQuery.validator.format("Это обязательное поле"),
    },


  },
  errorElement: "div",
  errorClass: "invalid",

});

$('#ResetPasswordForm').validate({
  rules: {
    Email: {
      required: true,
      email: true,
    },
    Password: {
      required: true,
    },
    ConfirmPassword: {
      required: true,
    },

  },
  messages: {
    Email: {
      required: jQuery.validator.format("Это обязательное поле"),
      email: jQuery.validator.format("Укажите E-mail"),
    },
    Password: {
      required: jQuery.validator.format("Это обязательное поле"),
    },
    ConfirmPassword: {
      required: jQuery.validator.format("Это обязательное поле"),
    },
  },
  errorElement: "div",
  errorClass: "invalid",

});

$('#AddThemeForm').validate({
  rules: {
    color: {
      required: true,
    },
    themeName: {
      required: true,
    },

  },
  messages: {
    color: {
      required: jQuery.validator.format("Каким цветом будет выделена ему для студента?"),
    },
    themeName: {
      required: jQuery.validator.format("Это обязательное поле"),
      minlength: jQuery.validator.format("Слишком короткое название"),
      maxlength: jQuery.validator.format("Слишком длинное название"),
    },



  },
  errorElement: "div",
  errorClass: "invalid",

});

$('#AddTaskForTheme').validate({
  rules: {
    type: {
      required: true,
    },
    task: {
      required: true,
    },
    file: {
      required: true,
    },
    name: {
      required: true,
      minlength: jQuery.validator.format("Слишком короткое название"),
      maxlength: jQuery.validator.format("Слишком длинное название"),
    },
    link: {
      required: true,
    },

  },
  messages: {
    type: {
      required: jQuery.validator.format("Каким цветом будет выделена ему для студента?"),
    },
    task: {
      required: jQuery.validator.format("Это обязательное поле"),
    },
    file: {
      required: jQuery.validator.format("Это обязательное поле"),
    },
    name: {
      required: jQuery.validator.format("Это обязательное поле"),
    },
    link: {
      required: jQuery.validator.format("Это обязательное поле"),
    },


  },
  errorElement: "div",
  errorClass: "invalid",

});

$('#AddMessageForAllForm').validate({
  rules: {
    msgTitle: {
      required: true,
    },
    msgText: {
      required: true,
    },
    targetType: {
      required: true,
    },


  },
  messages: {
    msgTitle: {
      required: jQuery.validator.format("Каким цветом будет выделена ему для студента?"),
    },
    msgText: {
      required: jQuery.validator.format("Это обязательное поле"),
    },
    targetType: {
      required: jQuery.validator.format("Это обязательное поле"),
    },

  },
  errorElement: "div",
  errorClass: "invalid",

});

$('#AddMessage').validate({
  rules: {
    msgTitle: {
      required: true,
    },
    msgText: {
      required: true,
    },

  },
  messages: {
    msgTitle: {
      required: jQuery.validator.format("Каким цветом будет выделена ему для студента?"),
    },
    msgText: {
      required: jQuery.validator.format("Это обязательное поле"),
    },

  },
  errorElement: "div",
  errorClass: "invalid",

});

// Статистика 

const attendance = document.querySelector('.attendance')
const homework = document.querySelector('.homework')
const activity = document.querySelector('.activity')
const hours = document.querySelector('.hours')
const adventures = document.querySelector('.adventures')
const skills = document.querySelectorAll('.skill')
const level = document.querySelectorAll('.level')

function setAttendanceDiagram() {
  const totalBlock = document.querySelector('#totalAmountText')
  const totalCircle = attendance.querySelector('.total')
  const totalAmount = totalCircle.getAttribute('data-count')

  const missedBlock = document.querySelector('#missedText')
  const visitedCircle = attendance.querySelector('.visited')
  const visitedAmount = visitedCircle.getAttribute('data-count')

  const visitedPercent = attendance.querySelector('.js-attendance-percent')

  totalBlock.innerHTML = totalAmount
  missedBlock.innerHTML = totalAmount - visitedAmount

  visitedPercent.innerHTML = Math.floor(visitedAmount / totalAmount * 100) + '%'

  const radius = totalCircle.r.baseVal.value
  const circumference = 2 * Math.PI * radius

  visitedCircle.style.strokeDashoffset = circumference
  visitedCircle.style.strokeDasharray = `${circumference} ${circumference}`
  const offset = circumference - visitedAmount / totalAmount * circumference
  visitedCircle.style.strokeDashoffset = offset

}

function setHomeworkDiagram() {
  const totalBlock = document.querySelector('#HW-totalAmountText')
  const totalCircle = homework.querySelector('.HW-total')
  const totalAmount = totalCircle.getAttribute('data-count')

  const passedBlock = document.querySelector('#HW-passedText')
  const passedCircle = homework.querySelector('.HW-passed')
  const passedAmount = passedCircle.getAttribute('data-count')

  const passedPercent = homework.querySelector('.js-homework-percent')


  totalBlock.innerHTML = totalAmount
  passedBlock.innerHTML = passedAmount
  passedPercent.innerHTML = Math.floor(passedAmount / totalAmount * 100) + '%'

  const radius = totalCircle.r.baseVal.value
  const circumference = 2 * Math.PI * radius

  passedCircle.style.strokeDashoffset = circumference
  passedCircle.style.strokeDasharray = `${circumference} ${circumference}`
  const offset = circumference - passedAmount / totalAmount * circumference
  passedCircle.style.strokeDashoffset = offset

}

function setActivityDiagram() {
  const activityBlock = activity.querySelector('.activity-block')
  const concentrasionBlock = activity.querySelector('.concentrasion-block')

  const activityPercent = activityBlock.getAttribute('data-count')
  const concentrasionPercent = concentrasionBlock.getAttribute('data-count')
  activityBlock.style.height = activityPercent + '%'
  concentrasionBlock.style.height = concentrasionPercent + '%'
}

function setHoursDiagram() {
  // const totalHours = hours.querySelector('#TotalHours').getAttribute('data-count')

  setTimeout(() => {
    $('#TotalHours').spincrement({
      from: 0,
      duration: 2000,
      thousandSeparator: ' ',
      // complete: function (e) {
      //   if (totalHours % 10 )
      //   // if (e.text() % 10 == 0 || )
      //   // e.text( e.text() + " ok ")
      // }
    })
  }, 1500);

}

function openAdventures() {
  const more = document.querySelectorAll('.adventures-more')
  more.forEach(item => {
    item.addEventListener('click', function () {
      item.parentElement.querySelectorAll('p').forEach(p => {
        p.style.display = 'block'
        item.style.display = 'none'
      })
    })
  })
}

function setSkillDiagrams() {
  skills.forEach(skill => {
    // const totalCircle = skill.querySelector('.total')

    const skillCircle = skill.querySelector('.skill-data')
    const skillAmount = skillCircle.getAttribute('data-count')

    const skillPercent = skill.querySelector('.js-skill-percent')

    skillPercent.innerHTML = skillAmount + '%'

    const radius = skillCircle.r.baseVal.value
    const circumference = 2 * Math.PI * radius

    skillCircle.style.strokeDashoffset = circumference
    skillCircle.style.strokeDasharray = `${circumference} ${circumference}`
    const offset = circumference - skillAmount / 100 * circumference
    skillCircle.style.strokeDashoffset = offset
  })

}

function setLevelStatistic() {

  var levelSwiper = new Swiper('.level-container', {
    speed: 500,
    spaceBetween: 15,
    autoheight: true,
    slidesPerView: 1,
    direction: 'horizontal',
    navigation: {
      nextEl: '#levelNext',
      prevEl: '#levelPrev',
    },
    breakpoints: {

      700: {
        spaceBetween: 30,
        autoheight: false,
        slidesPerView: 3,
      },
      450: {
        spaceBetween: 30,
        autoheight: false,
        slidesPerView: 2,
      }
    }

  });


  const levelPrev = document.querySelector('#levelPrev')
  const levelNext = document.querySelector('#levelNext')

  const levelSlides = document.querySelectorAll('.level-slide')

  const levelProgress = document.querySelector('#levelProgress')

  const currentLevel = document.querySelector('.level-container').getAttribute('data-level')
  if (currentLevel || currentLevel >= 0)
    levelSwiper.slideTo(currentLevel - 1)

  let length = levelSlides.length - 2
  // if (window.innerWidth >= 500) {
  //   certificateSwiper.slideTo(1)

  //   certificateSlides[certificateSwiper.activeIndex + 1].style.transform = 'scale(1.2)'
  //   certificateSlides[certificateSwiper.activeIndex + 1].style.zIndex = '99'
  // }


  if (window.innerWidth >= 700)
    levelProgress.innerHTML = (levelSwiper.activeIndex + 1) + "<span>/" + (levelSlides.length - 2) + "</span>"
  if (window.innerWidth <= 700 && window.innerWidth >= 450)
    levelProgress.innerHTML = (levelSwiper.activeIndex + 1) + "<span>/" + (levelSlides.length - 1) + "</span>"
  if (window.innerWidth <= 450)
    levelProgress.innerHTML = (levelSwiper.activeIndex + 1) + "<span>/" + (levelSlides.length) + "</span>"

  if (levelSwiper.activeIndex == 0)
    levelPrev.classList.remove('clickable')
  else
    levelPrev.classList.add('clickable')


  levelSwiper.on('slideChange', function () {
    // scaling active slide
    // if (window.innerWidth >= 500) {
    //   certificateSlides.forEach(item => {
    //     item.style.transform = ''
    //     certificateSlides[certificateSwiper.activeIndex + 1].style.zIndex = '1'

    //   })
    //   certificateSlides[certificateSwiper.activeIndex + 1].style.transform = 'scale(1.2)'
    //   certificateSlides[certificateSwiper.activeIndex + 1].style.zIndex = '99'

    // } else {
    levelSlides.forEach(item => {
      item.style.transform = ''
      levelSlides[levelSwiper.activeIndex].style.zIndex = '1'

    })
    // }

    // make navigation clickable
    if (levelSwiper.activeIndex == 0)
      levelPrev.classList.remove('clickable')
    else
      levelPrev.classList.add('clickable')

    if (levelSwiper.activeIndex == (levelSlides.length - 3))
      levelNext.classList.remove('clickable')
    else
      levelNext.classList.add('clickable')

    // set slider progress 
    if (window.innerWidth >= 700)
      levelProgress.innerHTML = (levelSwiper.activeIndex + 1) + "<span>/" + (levelSlides.length - 2) + "</span>"
    if (window.innerWidth <= 700 && window.innerWidth >= 450)
      levelProgress.innerHTML = (levelSwiper.activeIndex + 1) + "<span>/" + (levelSlides.length - 1) + "</span>"
    if (window.innerWidth <= 450)
      levelProgress.innerHTML = (levelSwiper.activeIndex + 1) + "<span>/" + (levelSlides.length) + "</span>"

  });

  levelSlides.forEach(item => {
    if (!item.classList.contains('empty')) {
      let progress = item.querySelector('.level-diagram__progress')
      let progressCount = progress.getAttribute('data-progress')
      progress.style.height = progressCount + "%"

      let lastPoint = item.querySelector('.last-point')
      if (lastPoint) {
        let lastPointData = lastPoint.getAttribute('data-point')
        lastPointData = (lastPointData > 90) ? lastPointData - 15 : lastPointData
        lastPoint.style.bottom = lastPointData + '%'
      }
    }



  })
}
if (attendance)
  setAttendanceDiagram()
if (homework)
  setHomeworkDiagram()
if (activity)
  setActivityDiagram()
if (hours)
  setHoursDiagram()
if (adventures)
  openAdventures()
if (skills.length != 0)
  setSkillDiagrams()
if (level.length != 0)
  setLevelStatistic()