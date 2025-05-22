using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace BasicAuthConsoleClient
{
    // A simplified DTO mirroring the UserDTO in your Web API
    public class UserDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class Program
    {
        // Change baseUrl to match your ASP.NET Core Web API address/port.
        private static readonly string baseUrl = "https://localhost:7163"; //WebApiBasicAuthentication_HostAddress
        // Replace with valid credentials for Basic Auth
        private static readonly string username = "john.doe@example.com";
        private static readonly string password = "password123";
        private static async Task Main(string[] args)
        {
            // Create and configure HttpClient
            using var httpClient = new HttpClient();
            // Attach Basic Authentication header
            var authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);
            Console.WriteLine("Basic Auth Console Client");
            // 1. GET all users
            await GetAllUsersAsync(httpClient);
            // 2. GET a single user by Id (e.g., Id = 1)
            await GetUserByIdAsync(httpClient, 1);
            // 3. CREATE (POST) a new user
            var newUserId = await CreateUserAsync(httpClient);
            // 4. UPDATE (PUT) the new user
            await UpdateUserAsync(httpClient, newUserId);
            // 5. DELETE the user
            await DeleteUserAsync(httpClient, newUserId);
            Console.WriteLine("All Operations are Completed");
            Console.ReadKey();
        }
        private static async Task GetAllUsersAsync(HttpClient httpClient)
        {
            Console.WriteLine("\n--- GET ALL USERS ---");
            try
            {
                var response = await httpClient.GetAsync($"{baseUrl}/api/User");
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                var users = JsonSerializer.Deserialize<List<UserDTO>>(responseBody);
                Console.WriteLine("Users:");
                if (users != null)
                {
                    foreach (var user in users)
                    {
                        Console.WriteLine($"Id: {user.Id}, Email: {user.Email}, Name: {user.FirstName} {user.LastName}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching users: {ex.Message}");
            }
        }
        private static async Task GetUserByIdAsync(HttpClient httpClient, int id)
        {
            Console.WriteLine($"\n--- GET USER BY ID: {id} ---");
            try
            {
                var response = await httpClient.GetAsync($"{baseUrl}/api/User/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Failed to get user. HTTP Status: {response.StatusCode}");
                    return;
                }
                var responseBody = await response.Content.ReadAsStringAsync();
                var user = JsonSerializer.Deserialize<UserDTO>(responseBody);
                if (user != null)
                {
                    Console.WriteLine($"User found - Id: {user.Id}, Email: {user.Email}, Name: {user.FirstName} {user.LastName}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user by Id: {ex.Message}");
            }
        }
        private static async Task<int> CreateUserAsync(HttpClient httpClient)
        {
            Console.WriteLine("\n--- CREATE NEW USER (POST) ---");
            var newUser = new UserDTO
            {
                FirstName = "Jane",
                LastName = "Smith",
                Email = $"jane.smith_{Guid.NewGuid()}@example.com",
                Password = "MyTest123"
            };
            try
            {
                var payload = JsonSerializer.Serialize(newUser);
                var content = new StringContent(payload, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync($"{baseUrl}/api/User", content);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Failed to create user. HTTP Status: {response.StatusCode}");
                    return 0;
                }
                var responseBody = await response.Content.ReadAsStringAsync();
                var createdUser = JsonSerializer.Deserialize<UserDTO>(responseBody);
                if (createdUser != null)
                {
                    Console.WriteLine($"Created user Id: {createdUser.Id}, Email: {createdUser.Email}");
                    return createdUser.Id;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating user: {ex.Message}");
            }
            return 0;
        }
        private static async Task UpdateUserAsync(HttpClient httpClient, int userId)
        {
            if (userId <= 0) return;
            Console.WriteLine("\n--- UPDATE USER (PUT) ---");
            var updatedUser = new UserDTO
            {
                Id = userId,
                FirstName = "Jane (updated)",
                LastName = "Smith (updated)",
                Email = $"jane.smith.updated_{Guid.NewGuid()}@example.com",
                Password = "MyUpdatedPassword123"
            };
            try
            {
                var payload = JsonSerializer.Serialize(updatedUser);
                var content = new StringContent(payload, Encoding.UTF8, "application/json");
                var response = await httpClient.PutAsync($"{baseUrl}/api/User/{userId}", content);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Failed to update user. HTTP Status: {response.StatusCode}");
                    return;
                }
                Console.WriteLine("User successfully updated.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user: {ex.Message}");
            }
        }
        private static async Task DeleteUserAsync(HttpClient httpClient, int userId)
        {
            if (userId <= 0) return;
            Console.WriteLine("\n--- DELETE USER ---");
            try
            {
                var response = await httpClient.DeleteAsync($"{baseUrl}/api/User/{userId}");
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Failed to delete user. HTTP Status: {response.StatusCode}");
                    return;
                }
                Console.WriteLine($"User with Id: {userId} deleted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting user: {ex.Message}");
            }
        }
    }
}
