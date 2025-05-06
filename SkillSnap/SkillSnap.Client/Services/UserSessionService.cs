namespace SkillSnap.Client.Services {
  public class UserSessionService {
    public string UserId { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string CurrentProjectId { get; set; } = string.Empty;

    public event Action OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();

    public void SetUserSession(string userId, string role) {
      UserId = userId;
      Role = role;
      NotifyStateChanged();
    }
    public void SetCurrentProject(string projectId) {
      CurrentProjectId = projectId;
      NotifyStateChanged();
    }
  }
}
