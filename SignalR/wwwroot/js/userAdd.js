//create connection
var connectionUserCount = new signalR.HubConnectionBuilder().withUrl("/hubs/userAdd").build();
//connects to method that hub invokes aka receive notifications from hub
connectionUserCount.on("updateUsers", (users) => {
    const userTable = document.getElementById("userTable").getElementsByTagName('tbody')[0];
    userTable.innerHTML = "";

    users.forEach(user => {
        const row = userTable.insertRow(-1);
        const cell1 = row.insertCell(0);
        const cell2 = row.insertCell(1);
        const cell3 = row.insertCell(2);
        cell1.innerHTML = user.id;
        cell2.innerHTML = user.userName;
        cell3.innerHTML = user.email;

        const buttonsCell = row.insertCell(3);
        buttonsCell.innerHTML = `
            <button type="submit" class="editUserBtn" data-id="${user.id}">Edit</button>
            <button type="submit" class="deleteUserBtn" data-id="${user.id}">Delete</button>
            
        `;
    });

});

document.getElementById("addUserBtn").addEventListener("click", function (event) {
    event.preventDefault(); // Prevent the default form submission

    const userName = document.getElementsByName("userName")[0].value;
    const email = document.getElementsByName("email")[0].value;

    // Call the server-side method to add a new user
    connectionUserCount.invoke("NewUserInserted", { UserName: userName, Email: email });
});




// Add event listeners for edit and delete buttons
document.getElementById("userTable").addEventListener("click", function (event) {
    if (event.target.classList.contains("editUserBtn")) {
        // Edit button clicked
        const userIdString = event.target.getAttribute("data-id");
        const userId = parseInt(userIdString, 10);
        const userName = prompt("Enter new user name:");
        const email = prompt("Enter new email:");

        if (userName && email) {
            // Call the server-side method to update the user
            connectionUserCount.invoke("UpdateUser", { ID: userId, UserName: userName, Email: email });
        }
    } else if (event.target.classList.contains("deleteUserBtn")) {
        // Delete button clicked
        const userIdString = event.target.getAttribute("data-id");
        const userId = parseInt(userIdString, 10);
       // const userId = event.target.getAttribute("data-id");
        console.log("Delete button clicked for user ID:", userId);

        const confirmDelete = confirm("Are you sure you want to delete the user with ID " + userId + "?");

        if (confirmDelete) {
            console.log("Deleting user with ID:", userId);
            connectionUserCount.invoke("DeleteUser", userId)
                .then(() => console.log("Delete request sent successfully"))
                .catch(error => console.error("Error sending delete request:", error));
        }
    }
});


function fullfilled() {
    console.log("successfull connection build");
    //newWidowLoadedOnClient();
}
function rejected() {
    console.log("no connection build");
}
//start the connection
connectionUserCount.start().then(fullfilled, rejected);