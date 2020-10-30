const students = document.querySelector('.js-selectedItemWrap.students')
const teachers = document.querySelector('.js-selectedItemWrap.teachers')

$('.select').on('click', '.select__head', function () {
  if ($(this).hasClass('open')) {
    $(this).removeClass('open');
    $(this).next().fadeOut(5);
  } else {
    $('.select__head').removeClass('open');
    $('.select__list').fadeOut(5);
    $(this).addClass('open');
    $(this).next().fadeIn(5);
  }
});

$('.select').on('click', '.select__item', function () {
  // $('.select__head').removeClass('open');
  // $(this).parent().fadeOut();
  $(this).parent().prev().text($(this).text());
  let myInput = document.createElement('div')
  myInput.classList.add('selected')
  if (this.classList.contains('teacher')) {
    myInput.innerHTML = $(this).text() + '<input class="selected-input" id="teacher" name="teachers" type="hidden" value="' + $(this).attr('data-value') + '">'
    myInput.classList.add('teacher')

    if ($(teachers).find('.selected').length == 0) {
      teachers.appendChild(myInput)
    } else {
      const selectedTeacherWrap = document.querySelector('.js-selectedItemWrap.teachers')

      selectedTeacherWrap.removeChild(document.querySelector('.selected.teacher'))
      teachers.appendChild(myInput)

    }
  } else {
    myInput.classList.add('student')

    myInput.innerHTML = $(this).text() + '<input class="selected-input" id="student" name="students" type="hidden" value="' + $(this).attr('data-value') + '">'
    students.appendChild(myInput)
  }

  setListeners()

});

$(document).click(function (e) {
  if (!$(e.target).closest('.select').length) {
    $('.select__head').removeClass('open');
    $('.select__list').fadeOut(5);
  }
});

setListeners()

function setListeners() {
  let selectedStudent = document.querySelectorAll('.selected.student')
  let selectedTeacher = document.querySelectorAll('.selected.teacher')
  const selectedStudentWrap = document.querySelector('.js-selectedItemWrap.students')
  const selectedTeacherWrap = document.querySelector('.js-selectedItemWrap.teachers')

  selectedStudent.forEach(item => {
    item.addEventListener('click', () => {
      selectedStudentWrap.removeChild(item)
    })
  })
  selectedTeacher.forEach(item => {
    item.addEventListener('click', () => {
      selectedTeacherWrap.removeChild(item)
    })
  })
}