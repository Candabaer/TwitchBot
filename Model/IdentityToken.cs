namespace TwitchBot.Model
{
	public class IdentityToken
	{
		public string client_id { get; set; } = string.Empty;
		public string login { get; set; } = string.Empty;
		public string[] scopes { get; set; } = Array.Empty<string>();
		public string user_id { get; set; } = string.Empty;
		public int expires_in { get; set; }
	}
}