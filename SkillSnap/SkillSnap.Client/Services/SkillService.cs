using SkillSnap.Client.Models;
using System.Net.Http.Json;

namespace SkillSnap.Client.Services {
  public class SkillService {
    private readonly HttpClient _httpClient;

    public SkillService(HttpClient httpClient) {
      _httpClient = httpClient;
    }
    public async Task<List<Skill>> GetSkillsAsync() {
      return await _httpClient.GetFromJsonAsync<List<Skill>>("http://localhost:5226/api/skills");
    }

    public async Task AddSkillAsync(Skill newSkill) {
      await _httpClient.PostAsJsonAsync("api/skills", newSkill);
    }

  }
}