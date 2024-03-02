//create connection
var connectionUserCount = new signalR.HubConnectionBuilder().withUrl("/hubs/userCount").build();
//connects to method that hub invokes aka receive notifications from hub
connectionUserCount.on("updateTotalViews", (value) => {
    newCountSpan = document.getElementById("totalViewsCounter");
    newCountSpan.innerText = value.toString();

});

connectionUserCount.on("updateTotalUsers", (value) => {
    newCountSpan = document.getElementById("totalUsersCounter");
    newCountSpan.innerText = value.toString();

});
//invoke hub methods aka send notification to hub
function newWidowLoadedOnClient() {
    connectionUserCount.send("NewWindowLoaded");
}
function fullfilled() {
    console.log("successfull connection build");
    newWidowLoadedOnClient();
}
function rejected() {
    console.log("no connection build");
}
//start the connection
connectionUserCount.start().then(fullfilled, rejected);