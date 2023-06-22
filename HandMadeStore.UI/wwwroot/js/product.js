var isarabic = document.getElementById("culture").getAttribute("data-culture")
var ProductName = isarabic == "True" ? "arabicName" : "name";
var CategoryName = isarabic == "True" ? "category.arabicName" : "category.name";
var BrandName = isarabic == "True" ? "brand.arabicName" : "brand.name";

$(document).ready(function () {
    LoadData();
});
const LoadData = () => {
    $('#mydata').DataTable({
        "ajax": {
            "url": "/Admin/Product/GetAll"
        },
        columns: [
            { "data": ProductName, width: "25%" },
            { "data": "price", width: "10%" },
            { "data": CategoryName, width: "20%" },
            { "data": BrandName, width: "20%" },
            { "data": "createdDate", width: "10%" },
            {
                "data": "id",
                "render": function (data) {
                    return `    <div class="d-flex justify-content-center gap-3">
                                        <a class="btn btn-warning " href="/Admin/Product/Upsert?id=${data}">
                                        <i class="fa-solid fa-pen-to-square me-1"></i>
                                        </a>
                                        <a class="btn btn-danger " onclick=Deleteproduct(${data},event)>
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
                targets: 4,
                render: DataTable.render.date()
            },
            {
                targets: 0,
                className: 'dt-left'
            }
        ],
        dom: 'Bfrtip',
        responsive: true,
        lengthChange: false,
        autoWidth: false,
        buttons: ["copy",
            {
                extend: 'excel',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4]
                }
            },
            {
                extend: 'pdf',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4]
                }
            },
            {
                extend: 'print',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4]
                }
            }]
    });
    DataTable.buttons().container().prpendTo("#dataTable_Wrapper");
}

const Deleteproduct = async (e, event) => {
    Swal.fire({
        title: 'Are you sure?',
        text: "That You delete This Product!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#df4759',
        cancelButtonColor: '#42ba96',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            console.log("try");
            fetch(`/Admin/Product/Delete?id=${e}`, {
                method: 'DELETE',
            })
                .then(response => {
                    if (response.ok) {
                        Swal.fire({
                            position: 'top-end',
                            icon: 'success',
                            title: 'product Deleted Successfuly',
                            showConfirmButton: false,
                            timer: 1400
                        })

                        setTimeout(() => {
                            location.reload();
                        }, 1500)
                    }
                    else {
                        console.log("bad try");

                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...',
                            text: 'Something went wrong while Deleting this product!',
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