
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using TwitchBot.Model;
using TwitchBot.Utility;

namespace TwitchBot.Service.Implementation
{
	public class TwitchAuthService
	{
		HttpClient _client;
		StartBrowser _browser;

		IConfiguration _config;

		private readonly string _deviceCodeUrl;
		private readonly string _tokenUrl;

		private Dictionary<string, string> _deviceCode = new();
		private Dictionary<string, string> _accessToken = new();

		public TwitchAuthService(IHttpClientFactory httpClientFactory, StartBrowser browser, IConfiguration config)
		{
			_config = config;
			_client = httpClientFactory.CreateClient("TwitchAuthClient");
			_browser = browser;

			_deviceCodeUrl = config["Twitch:DeviceCodeUrl"]!;
			_tokenUrl = config["Twitch:TokenUrl"]!;
			_deviceCode = new Dictionary<string, string>
			{
				["client_id"] = config["Twitch:ClientId"]!,
				["scope"] = config["Twitch:Scopes"]!
			};

			_accessToken = new Dictionary<string, string>(_deviceCode)
			{
				["grant_type"] = config["Twitch:grant_type"]!
			};
		}

		public async Task<DeviceCodeResponse> RequestDeviceCodeAsync()
		{
			DeviceCodeResponse deviceResponse = null!;

			var response = await _client.PostAsync(_deviceCodeUrl, new FormUrlEncodedContent(_deviceCode));
			response.EnsureSuccessStatusCode();
			deviceResponse = await response.Content.ReadFromJsonAsync<DeviceCodeResponse>();

			if (deviceResponse == null)
				throw new InvalidOperationException("Twitch returned an empty OAuth device response");
			_browser.RunDefaultBrowser(deviceResponse.verification_uri);

			return deviceResponse;
		}

		public async Task<AccessToken> PollForAccessToken(DeviceCodeResponse deviceResponse)
		{
			_accessToken.Add("device_code", deviceResponse.device_code);
			while (true)
			{
				var response = await _client.PostAsync(_tokenUrl,
					 new FormUrlEncodedContent(_accessToken));

				var accessToken = await response.Content.ReadFromJsonAsync<AccessToken>();

				if (accessToken != null && !String.IsNullOrEmpty(accessToken.access_token))
					return accessToken;

				await Task.Delay(deviceResponse.interval * 1000);
			}
		}

		public async Task<AccessToken> RefreshAccessToken(AccessToken expiredToken)
		{
			while (true)
			{
				var response = await _client.PostAsync(_tokenUrl,
						 new FormUrlEncodedContent(new Dictionary<string, string>
						 {
							 ["client_id"] = _config["Twitch:ClientId"]!,
							 ["grant_type"] = "refresh_token",
							 ["refresh_token"] = expiredToken.refresh_token,
						 }));
				var refreshedToken = await response.Content.ReadFromJsonAsync<AccessToken>();
				if (refreshedToken != null)
					return refreshedToken;
			}
		}


		public async Task<IdentityToken> GetIdentityTokenAsync(AccessToken token)
		{
			using var request = new HttpRequestMessage(HttpMethod.Get, "https://id.twitch.tv/oauth2/validate");
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.access_token);
			request.Headers.Add("Client-Id", _config["Twitch:ClientId"]!);

			var response = await _client.SendAsync(request);

			var result = await response.Content.ReadFromJsonAsync<IdentityToken>();
			if (result == null)
				throw new Exception("Twitch returned an empty OAuth validation response");
			return result;
		}

	}
}
