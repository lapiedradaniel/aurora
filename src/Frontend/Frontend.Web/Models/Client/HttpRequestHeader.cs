﻿using Backend.Domain.Entities.Authentication.Users.UserContext;
using Frontend.Web.Util.Enums.ContentTypeEnums;
using Frontend.Web.Util.Environments;
using Frontend.Web.Util.Session;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;

namespace Frontend.Web.Models.Client
{
    public class HttpRequestHeader
    {
        private readonly IConfiguration _configuration;
        private readonly SessionStorageAccessor _sessionProvider;
        private readonly EnvironmentHandler _environmentHandler;
        public HttpRequestHeader(IConfiguration configuration, SessionStorageAccessor sessionProvider, EnvironmentHandler environmentHandler) 
        {
            _configuration = configuration;
            _sessionProvider = sessionProvider;
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
            var userContext = await _sessionProvider.GetValueAsync<UserSessionContext>("UserSession");
            return new AuthenticationHeaderValue("Bearer", userContext.Token);
        }

        /// <summary>
        /// Build client HTTP Request header
        /// </summary>
        /// <returns>HTTP Header Settings</returns>
        public async Task<HttpRequestHeader> BuildHttpRequestHeader(HttpMethod method, bool isPublic, string contentType)
        {
            HttpRequestHeader httpRequestHeader = new HttpRequestHeader(_configuration, _sessionProvider, _environmentHandler)
            {
                Endpoint = _environmentHandler.GetEndpoint(),
                Authorization = isPublic ? null : await GetToken(),
                Method = method,
                Encoding = Encoding.UTF8,
                ContentType = contentType
            };
            return httpRequestHeader;
        }

        /// <summary>
        /// HTTP Request header properties
        /// </summary>
        public string Endpoint { get; set; }
        public HttpMethod Method { get; set; }
        public AuthenticationHeaderValue Authorization { get; set; }
        public Encoding Encoding { get; set; }
        public string ContentType { get; set; }
    }

}