namespace TwitchBot.Service.Implementation
{
	public class TextParser
	{
		private TwitchAPIClient _apiClient;

		public IObservable<string> CommandReceived;
		public TextParser(TwitchAPIClient twitchApiClient)
		{
			_apiClient = twitchApiClient;

		}
		public async Task Parse(string text)
		{
			if (text.StartsWith("!Axodor"))
			{
				await _apiClient.SendMessage("Is'n hengst!");
			}
		}
	}
}