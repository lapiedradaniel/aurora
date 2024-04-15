﻿using Backend.Domain.Entities.Authentication.Users.UserContext;
using Frontend.Web.Models.Route;
using Frontend.Web.Util.Cookie;
using Frontend.Web.Util.Enums.ContentTypeEnums;
using Frontend.Web.Util.Environments;
using Frontend.Web.Util.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;

namespace Frontend.Web.Models.Client
{
    public class HttpRequestHeader
    {
        private readonly IConfiguration _configuration;
        private readonly CookieHandler _cookies;
        private readonly EnvironmentHandler _environmentHandler;
        public HttpRequestHeader(IConfiguration configuration, CookieHandler cookies, EnvironmentHandler environmentHandler)
        {
            _configuration = configuration;
            _cookies = cookies;
            _environmentHandler = environmentHandler;
        }

        /// <summary>
        /// Get user Bearer Token authorization stored in the session
        /// and converts it to proper auth header token type to build
        /// the request.
        /// </summary>
        /// <returns>Authentication Header Value</returns>
        public async Task<AuthenticationHeaderValue> GetToken()
        {
            var userContext = await _cookies.GetValueAsync<UserSessionContext>("UserSession");
            return new AuthenticationHeaderValue("Bearer", userContext.Token);
        }

        /// <summary>
        /// Build client HTTP Request header
        /// </summary>
        /// <returns>HTTP Header Settings</returns>
        public async Task<HttpRequestHeader> BuildHttpRequestHeader(HttpMethod method, bool isPublic, string? contentType)
        {
            HttpRequestHeader httpRequestHeader = new HttpRequestHeader(_configuration, _cookies, _environmentHandler)
            {
                Uri = _environmentHandler.GetEndpoint(),
                Authorization = isPublic ? null : await GetToken(),
                Method = method,
                Encoding = Encoding.UTF8,
                ContentType = contentType
            };
            return httpRequestHeader;
        }

        // TODO: Improve here
        public string BuildRequestUri<T>(HttpRequestHeader httpRequestHeader, RouteBuilder<T> route)
        {
            if (route.Parameters != null)
                return $"{httpRequestHeader.Uri}/{route.Endpoint}/{route.ActionName}?{route.Parameters}";
            return $"{httpRequestHeader.Uri}/{route.Endpoint}/{route.ActionName}";
        }

        /// <summary>
        /// HTTP Request header properties
        /// </summary>
        public string Uri { get; set; }
        public HttpMethod Method { get; set; }
        public AuthenticationHeaderValue Authorization { get; set; }
        public Encoding Encoding { get; set; }
        public string ContentType { get; set; }
    }

}
