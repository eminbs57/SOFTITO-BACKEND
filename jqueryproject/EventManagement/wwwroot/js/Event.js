$(document).ready(function () {
    ShowEventData();
LoadCategoriesForDropdown();
   
    $('#btnAddEvent').click(function () {
        console.log("Butona tıklandı!"); 
        $('#EventModal').modal('show');
        $('#eventHeading').text('Etkinlik Ekle');
        $('#AddEvent').css('display', 'block');
        $('#btnUpdate').css('display', 'none');
        ClearTextBox();
    });
});

function ShowEventData() {
    $.ajax({
        url: '/Event/EventList',
        type: 'GET',
        dataType: 'json',
        success: function (result) {
            var object = '';
            $.each(result, function (index, item) {
                object += '<tr>';
                object += '<td>' + item.title + '</td>';
                object += '<td><span class="badge bg-secondary">' + item.categoryName + '</span></td>';
                object += '<td>' + (item.date ? item.date.split('T')[0] : '') + '</td>';
                object += '<td>' + item.kota + '</td>';
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

function Delete(id) {
    if (confirm('Silmek istediğinize emin misiniz?')) {
        $.ajax({
            url: '/Event/Delete?id=' + id,
            success: function () {
                alert('Kayıt Silindi!');
                ShowEventData();
            }
        });
    }
}

function Edit(id) {
    $.ajax({
        url: '/Event/Edit?id=' + id,
        type: 'Get',
        success: function (response) {
            $('#EventModal').modal('show');
            $('#EventId').val(response.id);
            $('#Title').val(response.title);
            $('#CategoryName').val(response.categoryName);
            $('#Description').val(response.description);
            $('#Date').val(response.date.split('T')[0]);
            $('#Kota').val(response.kota);
            $('#AddEvent').css('display', 'none');
            $('#btnUpdate').css('display', 'block');
            $('#eventHeading').text('Etkinlik Güncelle');
        }
    });
}

function HideModalPopUp() {
    $('#EventModal').modal('hide');
}

function AddEvent() {
    var formData = new FormData();
    formData.append("Title", $('#Title').val());
    formData.append("CategoryName", $('#CategoryName').val());
    formData.append("Date", $('#Date').val());
    formData.append("Kota", $('#Kota').val());
    formData.append("Description", $('#Description').val());
    
    var imageFile = $('#ImageFile')[0].files[0];
    if (imageFile) {
        formData.append("imageFile", imageFile);
    }
    
    var kota = $('#Kota').val();
    if(!kota || kota <= 0) {
        alert("Lütfen geçerli bir kontenjan değeri giriniz.");
        return;
    }

    $.ajax({
        url: '/Event/AddEvent',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function () {
            alert('Başarıyla Kaydedildi');
            HideModalPopUp();
            ShowEventData();
        },
        error: function (xhr) {
            alert(xhr.responseText || 'Bir hata oluştu!');
        }
    });
}

function UpdateEvent() {
    var formData = new FormData();
    formData.append("Id", $('#EventId').val());
    formData.append("Title", $('#Title').val());
    formData.append("CategoryName", $('#CategoryName').val());
    formData.append("Date", $('#Date').val());
    formData.append("Kota", $('#Kota').val());
    formData.append("Description", $('#Description').val());

    var imageFile = $('#ImageFile')[0].files[0];
    if (imageFile) {
        formData.append("imageFile", imageFile);
    }

    var kota = $('#Kota').val();
    if(!kota || kota <= 0) {
        alert("Lütfen geçerli bir kontenjan değeri giriniz.");
        return;
    }

    $.ajax({
        url: '/Event/Update',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function () {
            alert('Başarıyla Güncellendi');
            HideModalPopUp();
            ShowEventData();
        },
        error: function (xhr) {
            alert(xhr.responseText || 'Bir hata oluştu!');
        }
    });
}

function ClearTextBox() {
    $('#EventId').val('');
    $('#Title').val('');
    $('#CategoryName').val('');
    $('#Date').val('');
    $('#Kota').val('');
    $('#Description').val('');
    $('#ImageFile').val('');
    $('#AddEvent').show();
    $('#btnUpdate').hide();
    $('#eventHeading').text('Yeni Etkinlik Ekle');
}

$('#searchInput').on('keyup', function () {
    var value = $(this).val();
    $.ajax({
        url: '/Event/SearchEvent?searchString=' + value,
        type: 'GET',
        success: function (result) {
            var object = '';
            $.each(result, function (index, item) {
                object += '<tr>';
                object += '<td>' + item.title + '</td>';
                object += '<td><span class="badge bg-secondary">' + item.categoryName + '</span></td>';
                object += '<td>' + (item.date ? item.date.split('T')[0] : '') + '</td>';
                object += '<td>' + item.kota + '</td>';
                object += '<td>';
                object += '<button class="btn btn-sm btn-outline-primary me-2" onclick="Edit(' + item.id + ')" title="Düzenle"><i class="bi bi-pencil-square"></i></button>';
                object += '<button class="btn btn-sm btn-outline-danger" onclick="Delete(' + item.id + ');" title="Sil"><i class="bi bi-trash"></i></button>';
                object += '</td>';
                object += '</tr>';
            });
            $('#table_data').html(object);
        }
    });
});
function LoadCategoriesForDropdown() {
    $.ajax({
        url: '/Category/CategoryList',
        type: 'GET',
        success: function (result) {
            console.log("Gelen Kategoriler:", result); 
            
            var dropdown = $('#CategoryName');
            dropdown.empty();
            dropdown.append('<option value="">Kategori Seçiniz...</option>');
            
            $.each(result, function (index, item) {
            
                var categoryName = item.name || item.Name; 
                if(categoryName) {
                    dropdown.append('<option value="' + categoryName + '">' + categoryName + '</option>');
                }
            });
        },
        error: function(err) {
            console.error("Kategorileri çekerken hata:", err);
        }
    });
}