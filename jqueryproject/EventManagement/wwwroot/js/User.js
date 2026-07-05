$(document).ready(function () {
    ShowUserData();

    $('#btnAddUser').click(function () {
        ClearTextBox();
        $('#UserModal').modal('show');
        $('#userHeading').text('Kullanıcı Ekle');
        $('#passwordGroup').show(); // Show password on add
        $('#AddUser').css('display', 'block');
        $('#btnUpdate').css('display', 'none');
    });

    $('#searchInput').on('keyup', function () {
        var value = $(this).val();
        $.ajax({
            url: '/User/SearchUser?searchString=' + value,
            type: 'GET',
            success: function (result) {
                var object = '';
                $.each(result, function (index, item) {
                    object += '<tr>';
                    object += '<td>' + item.username + '</td>';
                    object += '<td><span class="badge ' + (item.role === 'Admin' ? 'bg-danger' : 'bg-primary') + '">' + item.role + '</span></td>';
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

function ShowUserData() {
    $.ajax({
        url: '/User/UserList',
        type: 'GET',
        dataType: 'json',
        success: function (result) {
            var object = '';
            $.each(result, function (index, item) {
                object += '<tr>';
                object += '<td>' + item.username + '</td>';
                object += '<td><span class="badge ' + (item.role === 'Admin' ? 'bg-danger' : 'bg-primary') + '">' + item.role + '</span></td>';
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

function AddUser() {
    var objData = {
        Username: $('#Username').val(),
        Password: $('#Password').val(),
        Role: $('#Role').val()
    };

    $.ajax({
        url: '/User/AddUser',
        type: 'POST',
        data: objData,
        success: function () {
            alert('Kullanıcı Eklendi!');
            HideModalPopUp();
            ShowUserData();
        }
    });
}

function Delete(id) {
    if (confirm('Emin misiniz?')) {
        $.ajax({
            url: '/User/Delete?id=' + id,
            success: function () {
                alert('Kayıt Silindi!');
                ShowUserData();
            }
        });
    }
}

function Edit(id) {
    $.ajax({
        url: '/User/Edit?id=' + id,
        type: 'GET',
        success: function (response) {
            $('#UserModal').modal('show');
            $('#UserId').val(response.id);
            $('#Username').val(response.username);
            $('#Password').val('');
            $('#passwordGroup').hide(); // Hide password on edit
            $('#Role').val(response.role);
            
            $('#AddUser').css('display', 'none');
            $('#btnUpdate').css('display', 'block');
            $('#userHeading').text('Kullanıcı Güncelle');
        }
    });
}

function UpdateUser() {
    var objData = {
        Id: $('#UserId').val(),
        Username: $('#Username').val(),
        Password: $('#Password').val(),
        Role: $('#Role').val()
    };

    $.ajax({
        url: '/User/Update',
        type: 'POST',
        data: objData,
        success: function () {
            alert('Kullanıcı Güncellendi!');
            HideModalPopUp();
            ShowUserData();
        }
    });
}

function HideModalPopUp() {
    $('#UserModal').modal('hide');
}

function ClearTextBox() {
    $('#UserId').val('');
    $('#Username').val('');
    $('#Password').val('');
    $('#Role').val('');
}