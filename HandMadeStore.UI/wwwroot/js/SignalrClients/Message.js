$(() => {
    var connection = new signalR.HubConnectionBuilder().withUrl("/Message").build();
    connection.start();
    connection.on("ReciveMessage", (message) => {
        toastr.success(`${message.sender} send (${message.body})`, 'Handmade Store', { timeOut: 2000 })
    })
})