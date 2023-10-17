﻿using Backend.Domain.Entities.Authentication.Users.UserContext;
using Backend.Domain.Entities.Products;
using Frontend.Web.Models.Client;
using Frontend.Web.Util.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using System.Net;
using HttpRequestHeader = Frontend.Web.Models.Client.HttpRequestHeader;
using Frontend.Web.Util.Enums.ContentTypeEnums;
using Backend.Infrastructure.Enums.Modules;
using Frontend.Web.Models.Route;
using Microsoft.AspNetCore.Routing;
using RouteBuilder = Frontend.Web.Models.Route.RouteBuilder;

namespace Frontend.Web.Repository.Client
{
    public class HttpClientRepository
    {
        private readonly HttpClient _httpClient;
        private readonly HttpRequestHeader _httpRequestHeader;
        public HttpClientRepository(HttpClient httpClient, HttpRequestHeader httpRequestHeader)
        {
            _httpClient = httpClient;
            _httpRequestHeader = httpRequestHeader;
        }

        /* Private Methods */
        /// <summary>
        /// Get method to list all items.
        /// </summary>
        /// <typeparam name="T">Type of return</typeparam>
        /// <param name="key">Tenant Id</param>
        /// <returns>Returns a list of T</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<T>> Get<T>(RouteBuilder route)
        {
            HttpRequestHeader httpRequestHeader = await _httpRequestHeader.BuildHttpRequestHeader(HttpMethod.Get, false, ContentTypeEnum.JSON);
            _httpClient.DefaultRequestHeaders.Authorization = httpRequestHeader.Authorization;
            return await _httpClient.GetFromJsonAsync<List<T>>($"{httpRequestHeader.Uri}/{route.Endpoint}?{route.Parameters}");
        }

        /// <summary>
        /// Get method to list all items.
        /// </summary>
        /// <typeparam name="T">Type of return</typeparam>
        /// <param name="key">Tenant Id</param>
        /// <returns>Returns a list of T</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<T> GetById<T>(RouteBuilder route)
        {
            HttpRequestHeader httpRequestHeader = await _httpRequestHeader.BuildHttpRequestHeader(HttpMethod.Get, false, ContentTypeEnum.JSON);
            _httpClient.DefaultRequestHeaders.Authorization = httpRequestHeader.Authorization;
            return await _httpClient.GetFromJsonAsync<T>($"{httpRequestHeader.Uri}/{route.Endpoint}?{route.Parameters}");
        }

        /// <summary>
        /// Post method, private only with authorization.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> Post<T>(RouteBuilder route)
        {
            HttpRequestHeader httpRequestHeader = await _httpRequestHeader.BuildHttpRequestHeader(HttpMethod.Post, false, ContentTypeEnum.JSON);
            var request = new HttpRequestMessage(httpRequestHeader.Method, $"{httpRequestHeader.Uri}/{route.Endpoint}");
            request.Content = new StringContent(JsonSerializer.Serialize(route.Model), httpRequestHeader.Encoding, httpRequestHeader.ContentType);
            return await _httpClient.SendAsync(request);
        }

        /// <summary>
        /// Put method, private only with authorization.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> Put<T>(T model)
        {
            HttpRequestHeader httpRequestHeader = await _httpRequestHeader.BuildHttpRequestHeader(HttpMethod.Put, false, ContentTypeEnum.JSON);
            var request = new HttpRequestMessage(httpRequestHeader.Method, $"{httpRequestHeader.Uri}/RouteGoHere");
            request.Content = new StringContent(JsonSerializer.Serialize(model), httpRequestHeader.Encoding, httpRequestHeader.ContentType);
            return await _httpClient.SendAsync(request);
        }

        /// <summary>
        /// Delete method, private only with authorization.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<HttpRequestMessage> Delete<T>(Guid key1) // review
        {
            HttpRequestHeader httpRequestHeader = await _httpRequestHeader.BuildHttpRequestHeader(HttpMethod.Put, false, ContentTypeEnum.JSON);
            return new HttpRequestMessage(httpRequestHeader.Method, $"{httpRequestHeader.Uri}/Products/Delete?Id={key1}");
            
        }

        /* Public Methods */
        /// <summary>
        /// Post method, public not necessary to authenticate first.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="isPublic"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> Post<T>(RouteBuilder route, bool isPublic = true)
        {
            HttpRequestHeader httpRequestHeader = await _httpRequestHeader.BuildHttpRequestHeader(HttpMethod.Post, false, ContentTypeEnum.JSON);
            var request = new HttpRequestMessage(httpRequestHeader.Method, $"{httpRequestHeader.Uri}/{route.Endpoint}");
            request.Content = new StringContent(JsonSerializer.Serialize(route.Model), httpRequestHeader.Encoding, httpRequestHeader.ContentType);
            return await _httpClient.SendAsync(request);
        }
    }
}
