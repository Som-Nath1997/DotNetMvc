$(document).ready(function () {
    showData();
});

function showData() {
    $.ajax({
        url: '/Account/ShowData1',
        type: 'GET',
        dataType: 'json',
        success: function (result, status, xhr) {
            console.log(result);
            var tableBody = $('#table-data');

            // Clear existing data
            tableBody.empty();

            $.each(result, function (index, item) {
                var row = $('<tr>');
                row.append('<td>' + item.name + '</td>');
                row.append('<td>' + item.email + '</td>');
                row.append('<td>' + item.address + '</td>');

                // Add an "Edit" button
                var editButton = $('<button>').text('Edit').attr('data-id', item.id).addClass('btn btn-primary btn-sm edit-btn');
                row.append($('<td>').append(editButton));
                debugger
                tableBody.append(row);
            });

            // Attach click event to the "Edit" buttons
            $('.edit-btn').on('click', function () {
                var userId = $(this).data('id');
                // Perform edit action using userId
                editUser(userId);
            });
        },
        error: function () {
            alert('Unable to retrieve data. An error occurred.');
        }
    });
}

// Example function to handle the edit action
function editUser(userId) {
    // Implement your edit logic using the userId
    console.log('Edit user with ID:', userId);
    // Redirect to the edit page or show a modal, etc.
}
