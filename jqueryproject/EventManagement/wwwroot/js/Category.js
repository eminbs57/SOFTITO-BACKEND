$(document).ready(function () {

    ShowCategoryData();

   
    $('#btnAddCategory').click(function () {
        ClearTextBox();
        $('#CategoryModal').modal('show');
        $('#categoryHeading').text('Kategori Ekle');
        $('#AddCategory').css('display', 'block');
        $('#btnUpdate').css('display', 'none');
    });

    
    $('#searchInput').on('keyup', function () {
        var value = $(this).val();
        $.ajax({
            url: '/Category/SearchCategory?searchString=' + value,
            type: 'GET',
            success: function (result) {
                var object = '';
                $.each(result, function (index, item) {
                    object += '<tr>';
                    object += '<td>' + item.name + '</td>';
                    object += '<td class="text-end pe-4">';
                    object += '<button class="btn btn-sm btn-outline-primary me-2" onclick="Edit(' + item.id + ')" title="Düzenle"><i class="bi bi-pencil-square"></i></button>';
                    object += '<button class="btn btn-sm btn-outline-danger" onclick="Delete(' + item.id + ');" title="Sil"><i class="bi bi-trash"></i></button>';
                    object += '</td>';
                    object += '</tr>';
                });
                $('#table_data').html(object);
            }
        });
    });
});



// Tabloyu Getir
function ShowCategoryData() {
    $.ajax({
        url: '/Category/CategoryList',
        type: 'GET',
        dataType: 'json',
        success: function (result) {
            var object = '';
            $.each(result, function (index, item) {
                object += '<tr>';
                object += '<td>' + item.name + '</td>';
                object += '<td class="text-end pe-4">';
                object += '<button class="btn btn-sm btn-outline-primary me-2" onclick="Edit(' + item.id + ')" title="Düzenle"><i class="bi bi-pencil-square"></i></button>';
                object += '<button class="btn btn-sm btn-outline-danger" onclick="Delete(' + item.id + ');" title="Sil"><i class="bi bi-trash"></i></button>';
                object += '</td>';
                object += '</tr>';
            });
            $('#table_data').html(object);
        }
    });
}


function AddCategory() {
    var objData = {
        Name: $('#Name').val()
    };

    $.ajax({
        url: '/Category/AddCategory',
        type: 'POST',
        data: objData,
        success: function () {
            alert('Kategori başarıyla eklendi!');
            HideModalPopUp();
            ShowCategoryData(); 
        },
        error: function () {
            alert('Kategori eklenirken bir hata oluştu.');
        }
    });
}

// Kategori Sil
function Delete(id) {
    if (confirm('Bu kategoriyi silmek istediğinize emin misiniz?')) {
        $.ajax({
            url: '/Category/Delete?id=' + id,
            success: function () {
                alert('Kayıt Silindi!');
                ShowCategoryData(); 
            }
        });
    }
}


function Edit(id) {
    $.ajax({
        url: '/Category/Edit?id=' + id,
        type: 'GET',
        success: function (response) {
            $('#CategoryModal').modal('show');
            $('#CategoryId').val(response.id);
            $('#Name').val(response.name);
            
         
            $('#AddCategory').css('display', 'none');
            $('#btnUpdate').css('display', 'block');
            $('#categoryHeading').text('Kategori Güncelle');
        }
    });
}


function UpdateCategory() {
    var objData = {
        Id: $('#CategoryId').val(),
        Name: $('#Name').val()
    };

    $.ajax({
        url: '/Category/Update',
        type: 'POST',
        data: objData,
        success: function () {
            alert('Kategori başarıyla güncellendi!');
            HideModalPopUp();
            ShowCategoryData(); 
        }
    });
}

function HideModalPopUp() {
    $('#CategoryModal').modal('hide');
}


function ClearTextBox() {
    $('#CategoryId').val('');
    $('#Name').val('');
}