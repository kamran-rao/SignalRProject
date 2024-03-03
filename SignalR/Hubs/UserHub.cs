using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalR.Data;
using SignalR.Models;

namespace SignalR.Hubs
{
    public class UserHub:Hub
    {
        private readonly ApplicationDbContext _db;

        public static int TotalViews { get; set; } = 0;
        public static int TotalUsers { get; set; } = 0;
        public UserHub(ApplicationDbContext db)
        {
            _db = db;
            
        }
        public async Task NewWindowLoaded()
        {
            TotalViews++;
            await Clients.All.SendAsync("updateTotalViews", TotalViews);
        }

        public override Task OnConnectedAsync()
        {
            TotalUsers++;
            Clients.All.SendAsync("updateTotalUsers", TotalUsers).GetAwaiter().GetResult();
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            TotalUsers--;
            Clients.All.SendAsync("updateTotalUsers", TotalUsers).GetAwaiter().GetResult();
            return base.OnDisconnectedAsync(exception);

        }

        //custom work
        public async Task NewUserInserted(User newUser)
        {
            // Add logic to insert the new user into the database
            _db.Users.Add(newUser);
            await _db.SaveChangesAsync();

            // Notify all clients about the new user
            await Clients.All.SendAsync("updateUsers", await GetAllUsers());
        }

        public async Task<List<User>> GetAllUsers()
        {
            // Retrieve all users from the database
            var users = await _db.Users.ToListAsync(); // Adjust this based on your actual query

            // Notify all clients about the updated user list
            await Clients.All.SendAsync("updateUsers", users);
            return users; 
        }

        public async Task DeleteUser(int userId)
        {
            // Add logic to delete the user from the database
            var userToDelete = await _db.Users.FindAsync(userId);
            if (userToDelete != null)
            {
                _db.Users.Remove(userToDelete);
                await _db.SaveChangesAsync();

                // Notify all clients about the deleted user
                await Clients.All.SendAsync("updateUsers", await GetAllUsers());
            }
        }

        public async Task UpdateUser(User updatedUser)
        {
            // Add logic to update the user in the database
            var existingUser = await _db.Users.FindAsync(updatedUser.ID);
            if (existingUser != null)
            {
                existingUser.UserName = updatedUser.UserName;
                existingUser.Email = updatedUser.Email;
                await _db.SaveChangesAsync();

                // Notify all clients about the updated user
                await Clients.All.SendAsync("updateUsers", await GetAllUsers());
            }
        }
    }
}
