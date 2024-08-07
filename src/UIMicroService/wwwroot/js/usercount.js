

////create connection
//var connectionUserCount = new signalR.HubConnectionBuilder()
//    //.configureLogging(signalR.LogLevel.Information)
//    .withAutomaticReconnect()
//    .withUrl("https://localhost/notificationservice/realtimehub", signalR.HttpTransportType.WebSockets).build();

////connect to methods that hub invokes aka receive notfications from hub
//connectionUserCount.on("ReceiveMessage", (value) => {
//    var newCountSpan = document.getElementById("totalViewsCounter");
//    newCountSpan.innerText = value.toString();
//});

////connectionUserCount.on("updateTotalUsers", (value) => {
////    var newCountSpan = document.getElementById("totalUsersCounter");
////    newCountSpan.innerText = value.toString();
////});

////invoke hub methods aka send notification to hub
//function newWindowLoadedOnClient() {
//    connectionUserCount.invoke("NewWindowLoaded", "Bhrugen").then((value) => console.log(value));
//}

////start connection
//function fulfilled() {
//    //do something on start
//    console.log("Connection to User Hub Successful");
//    newWindowLoadedOnClient();
//}
//function rejected() {
//    //rejected logs
//}

//connectionUserCount.onclose((error) => {
//    document.body.style.background = "red";
//});

//connectionUserCount.onreconnected((connectionId) => {
//    document.body.style.background = "green";
//});

//connectionUserCount.onreconnecting((error) => {
//    document.body.style.background = "orange";
//});

//connectionUserCount.start().then(fulfilled, rejected);
//////create connection
////debugger;
//////var connectionUserCount = new signalR.HubConnectionBuilder().withUrl(`/hub/connection`).build();
//////var connectionUserCount = new signalR.HubConnectionBuilder().withUrl(`https://localhost/apigateway/Browser/hub/GetNotification`).build();
//////create connection
////var connectionUserCount = new signalR.HubConnectionBuilder()
////    //.configureLogging(signalR.LogLevel.Information)
////    .withAutomaticReconnect()
////    .withUrl("/hub/connection", signalR.HttpTransportType.WebSockets).build();
//////connection to methods that hub invokes aka receive notification from hub

////connectionUserCount.on(`updateTotalViews`, (value) => {
////    var newCountSpan = document.getElementById(`pageCount`);
////    newCountSpan.innerText = value.toString();
////})
//////invoke hub methods aka send notification to hub
////function newWindowLoadedClinet() {
////    connectionUserCount.send(`NewWindowLoaded`);
////}

//////start connection.

////function fullFilled() {
////    console.log("connection successful!");
////}
////function rejected() {
////    console.log("connection failed!");
////}
////connectionUserCount.start(fullFilled, rejected).then();