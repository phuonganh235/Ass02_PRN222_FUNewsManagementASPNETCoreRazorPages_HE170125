// Tạo kết nối tới NotificationHub
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .build();

// Lắng nghe sự kiện ReceiveNotification
connection.on("ReceiveNotification", function (message) {
    if (window.toastr) {
        toastr.info(message);
    } else {
        alert(message);
    }
});

// Kết nối hub
connection.start().catch(err => console.error(err.toString()));
