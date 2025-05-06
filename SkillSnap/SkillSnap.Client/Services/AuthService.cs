using System.Net.Http.Json;
using System.Text.Json;
using Blazored.LocalStorage;

namespace SkillSnap.Client.Services {
  public class AuthService {
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    public AuthService(HttpClient httpClient, ILocalStorageService localStorage) {
      _httpClient = httpClient;
      _localStorage = localStorage;
    }

    public async Task<bool> LoginAsync(string email, string password) {
      var response = await _httpClient.PostAsJsonAsync("http://localhost:5226/api/auth/login", new { Email = email, Password = password });

      if(!response.IsSuccessStatusCode) return false;

      var content = await response.Content.ReadFromJsonAsync<JsonElement>();
      var token = content.GetProperty("Token").GetString();

      await _localStorage.SetItemAsync("authToken", token);
      return true;
    }

    public async Task<bool> RegisterAsync(string fullName, string email, string password) {
      var response = await _httpClient.PostAsJsonAsync("http://localhost:5226/api/auth/register", new {
        FullName = fullName,
        Email = email,
        Password = password
      });

      return response.IsSuccessStatusCode;
    }

    public async Task LogoutAsync() {
      await _localStorage.RemoveItemAsync("authToken");
    }
  }
}
