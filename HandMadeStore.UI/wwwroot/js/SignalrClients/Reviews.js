$(() => {
    var connection = new signalR.HubConnectionBuilder().withUrl("/Reviews").build();
    connection.start();
    connection.on("LoadReviews", (id) => {
        if (window.location.search == `?productId=${id}`)
            location.reload();
    })
})