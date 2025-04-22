namespace SafeVault.Services {
  public static class InputSanitizer {
    public static string Sanitize(string input) {
      return System.Net.WebUtility.HtmlEncode(input);
    }
  }
}