using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;

namespace TwitchBot.Utility
{
	public class TokenHandler : DelegatingHandler
	{
		private readonly AccessTokenService _tokenService;

		public TokenHandler(AccessTokenService tokens)
		{
			_tokenService = tokens;
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
		{
			var token = _tokenService._accessToken.access_token;
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

			var response = await base.SendAsync(request, ct);

			if (response.StatusCode == HttpStatusCode.Unauthorized)
			{
				await _tokenService.ForceRefresh();
				request.Headers.Authorization =
					new AuthenticationHeaderValue("Bearer", _tokenService._accessToken.access_token);

				response = await base.SendAsync(request, ct);
			}

			return response;
		}
	}
}
