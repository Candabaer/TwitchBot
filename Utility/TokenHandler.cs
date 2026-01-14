using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using static System.Net.Mime.MediaTypeNames;

namespace TwitchBot.Utility
{
	public class TokenHandler : DelegatingHandler
	{
		private readonly IConfiguration _config;
		private readonly TwitchUserSession _userSession;

		public TokenHandler(TwitchUserSession userSession, IConfiguration config)
		{
			_config = config;
			_userSession = userSession;
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
		{
			var token = _userSession._accessToken.access_token;
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
			request.Headers.Remove("Client-Id");
			request.Headers.Add("Client-Id", _config["Twitch:ClientId"]!);

			var response = await base.SendAsync(request, ct);

			var body = await response.Content.ReadAsStringAsync();
			Console.WriteLine(body);

			if (response.StatusCode == HttpStatusCode.Unauthorized)
			{
				await _userSession.ForceRefresh();
				request.Headers.Authorization =
					new AuthenticationHeaderValue("Bearer", _userSession._accessToken.access_token);

				response = await base.SendAsync(request, ct);
			}

			return response;
		}
	}
}
