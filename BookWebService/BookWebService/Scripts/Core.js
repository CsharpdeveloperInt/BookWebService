$(document).ready(function ()
{

    GetAllBooks();

    $("#showAddBook").click(function (event) {
        event.preventDefault();
        $('#ModalForm').modal();
        $("#captionHeader").html('Добавление книги');
        $("#editBlock").css('display', 'none');
        $("#createBlock").css('display', 'block');

    });

    $("#editBook").click(function (event) {
        event.preventDefault();
        EditBook();
    });

    $("#addBook").click(function (event) {
        event.preventDefault();
        AddBook();
    });



});

// Получение всех книг по ajax-запросу
function GetAllBooks() {

    $("#createBlock").css('display', 'none');
    $("#editBlock").css('display', 'none');
    $.ajax({
        url: '/api/books',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            WriteResponse(data);
        },
        error: function (error) {
            alert("Произошла ошибка при обработке запроса.Смотри консоль");
            Console.log(error);
        }
    });
}

// Добавление новой книги
function AddBook() {
    // получаем значения для добавляемой книги
    var book = {
        Name: $('#addName').val(),
        Author: $('#addAuthor').val(),
        YearPublic: $('#addYear').val()
    };

    $.ajax({
        url: '/api/books/',
        type: 'POST',
        data: JSON.stringify(book),
        contentType: "application/json;charset=utf-8",
        success: function () {
            $("#createBlock").css('display', 'none');
            $('#ModalForm').modal('hide');
            GetAllBooks();
        },
        error: function (error) {
            alert("Произошла ошибка при обработке запроса.Смотри консоль");
            Console.log(error);
        }
    });
}


// Удаление книги
function DeleteBook(id) {
    $.ajax({
        url: '/api/books/' + id,
        type: 'DELETE',
        contentType: "application/json;charset=utf-8",
        success: function () {
            GetAllBooks();
        },
        error: function (error) {
            alert("Произошла ошибка при обработке запроса.Смотри консоль");
            Console.log(error);
        }
    });
}

// обработчик удаления
function DeleteItem(el) {
    // получаем id удаляемого объекта
    var id = $(el).attr('data-item');
    DeleteBook(id);
}


// редактирование книги
function EditBook() {
    var id = $('#editId').val();
    // получаем новые значения для редактируемой книги
    var book = {
        Id: $('#editId').val(),
        Name: $('#editName').val(),
        Author: $('#editAuthor').val(),
        YearPublic: $('#editYear').val()
    };
    $.ajax({
        url: '/api/books/' + id,
        type: 'PUT',
        data: JSON.stringify(book),
        contentType: "application/json;charset=utf-8",
        success: function () {
            GetAllBooks();
            $('#ModalForm').modal('hide');
        },
        error: function (error) {
            alert("Произошла ошибка при обработке запроса.Смотри консоль");
            Console.log(error);
        }
    });
}


// обработчик редактирования
function EditItem(el) {
    // получаем id редактируемого объекта
    var id = $(el).attr('data-item');
    GetBook(id);
}


// вывод данных редактируемой книги в поля для редактирования
function ShowBook(book) {
    if (book != null) {
        $("#createBlock").css('display', 'none');
        $('#ModalForm').modal();
        $("#captionHeader").html('Редактирование книги');
        $("#editBlock").css('display', 'block');
        $("#editId").val(book.Id);
        $("#editName").val(book.Name);
        $("#editAuthor").val(book.Author);
        $("#editYear").val(book.YearPublic);
    }
    else {
        alert("Такая книга не существует");
    }
}


// запрос книги на редактирование
function GetBook(id) {
    $.ajax({
        url: '/api/books/' + id,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            ShowBook(data);
        },
        error: function (error) {
            alert("Произошла ошибка при обработке запроса.Смотри консоль");
            Console.log(error);
        }
    });
}


function WriteResponse(books) {
    var strResult = "<table class='table table-striped'><th>Название</th><th>Автор</th><th>Год издания</th>";
    $.each(books, function (index, book) {
        strResult += "<tr><td> " + book.Name + "</td><td>" + book.Author + "</td><td>" + book.YearPublic + "</td>" +
            "<td><a class='btn btn-default' id='editItem' data-item='" + book.Id + "' onclick='EditItem(this);' >Редактировать</a></td>" +
            "<td><a class='btn btn-danger' id='delItem' data-item='" + book.Id + "' onclick='DeleteItem(this);' >Удалить</a></td></tr>";
    });
    strResult += "</table>";
    $("#tableBlock").html(strResult);

}