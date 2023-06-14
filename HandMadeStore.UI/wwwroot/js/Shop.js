$(document).ready(function () {
    LoadData();
});

const LoadData = () => {
    $('#mydata').DataTable({
        "ajax": {
            "url": "/Admin/Shop/GetAll"
        },
        columns: [
            { "data": "name", width: "25%" },
            { "data": "city", width: "10%" },
            { "data": "streetAddress", width: "20%" },
            { "data": "postalCode", width: "15%" },
            { "data": "phoneNumber", width: "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `    <div class="d-flex justify-content-center gap-3">
                                <a class="btn btn-warning " href="/Admin/Shop/Upsert?id=${data}">
                                <i class="fa-solid fa-pen-to-square me-1"></i>
                                </a>
                                <a class="btn btn-danger " onclick=DeleteShop(${data},event)>
                               <i class="fa-solid fa-trash-can me-1"></i>
                                </a>
                            </div>
                            `
                },
                width: "10%"
            }
        ],
        columnDefs: [
            {
                targets: 0,
                className: 'dt-left'
            }
        ]
    });
}
const DeleteShop = async (e, event) => {
    Swal.fire({
        title: 'Are you sure?',
        text: "That You delete This User!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#df4759',
        cancelButtonColor: '#42ba96',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            console.log("try");
            fetch(`/Admin/Shop/Delete?id=${e}`, {
                method: 'DELETE',
            })
                .then(response => {
                    if (response.ok) {
                        Swal.fire({
                            position: 'top-end',
                            icon: 'success',
                            title: 'Shop Deleted Successfuly',
                            showConfirmButton: false,
                            timer: 1400
                        })

                        setTimeout(() => {
                            location.reload();
                        }, 1400)
                    }
                    else {
                        console.log("bad try");

                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...',
                            text: 'Something went wrong while Deleting this Shop!',
                        })
                    }
                })
                .catch(error => {
                    console.error('Error:', error);
                    console.log("error happend")
                });
        }
    })
}