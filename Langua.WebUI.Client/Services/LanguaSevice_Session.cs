using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

using Radzen;
using System.Text;
using System.Text.Encodings.Web;

namespace Langua.WebUI.Client.Services
{
    public partial class LangClientService
    {
        public async Task ExportSessionsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/languadb/sessions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/languadb/sessions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportSessionsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/languadb/sessions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/languadb/sessions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetSessions(HttpRequestMessage requestMessage);

        public async Task<IEnumerable<Models.Session>> GetSessions(Query query)
        {
            return await GetSessions(filter: $"{query.Filter}", orderby: $"{query.OrderBy}", top: query.Top, skip: query.Skip, count: query.Top != null && query.Skip != null);
        }

        public async Task<IEnumerable<Models.Session>> GetSessions(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Sessions");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter: filter, top: top, skip: skip, orderby: orderby, expand: expand, select: select, count: count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetSessions(httpRequestMessage);

            httpRequestMessage.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
            var response = await httpClient.SendAsync(httpRequestMessage);
            var str = await response.Content.ReadAsStringAsync();
            return await Radzen.HttpResponseMessageExtensions.ReadAsync<IEnumerable<Langua.Models.Session>>(response);
        }
        public async Task GetNSessions()
        {
            var uri = new Uri(baseUri, $"Sessions");
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            requestMessage.SetBrowserRequestCredentials(BrowserRequestCredentials.SameOrigin);
            var response = await httpClient.SendAsync(requestMessage);
            
            var str = await response.Content.ReadAsStringAsync();
        }

        partial void OnCreateSession(HttpRequestMessage requestMessage);

        public async Task<Models.Session> CreateSession(Models.Session session = default(Models.Session))
        {
            var uri = new Uri(baseUri, $"Sessions");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(session), Encoding.UTF8, "application/json");

            OnCreateSession(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Models.Session>(response);
        }

        partial void OnDeleteSession(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteSession(int id = default(int))
        {
            var uri = new Uri(baseUri, $"Sessions(Id={id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteSession(httpRequestMessage);
            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetSessionById(HttpRequestMessage requestMessage);

        public async Task<Models.Session> GetSessionById(string expand = default(string), int id = default(int))
        {
            var uri = new Uri(baseUri, $"Sessions(Id={id})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter: null, top: null, skip: null, orderby: null, expand: expand, select: null, count: null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetSessionById(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Models.Session>(response);
        }

        partial void OnUpdateSession(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> UpdateSession(int id = default(int), Models.Session session = default(Models.Session))
        {
            var uri = new Uri(baseUri, $"Update?Id={id}");
            //var postUri = new Uri( $"https://localhost:44317/api/Global/Update?Id={id}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            //httpRequestMessage.Headers.Add("If-Match", session.ETag);

            httpRequestMessage.Content = new StringContent(ODataJsonSerializer.Serialize(session), Encoding.UTF8, "application/json");

            OnUpdateSession(httpRequestMessage);
            //var r = await httpClient.PostAsync(postUri, httpRequestMessage.Content);
            var response = await httpClient.SendAsync(httpRequestMessage);
            return response;
        }

        public async Task<IEnumerable<Models.Groups>> GetGroups(Query query)
        {
            return await GetGroups(filter: $"{query.Filter}", orderby: $"{query.OrderBy}", top: query.Top, skip: query.Skip, count: query.Top != null && query.Skip != null);
        }

        public async Task<IEnumerable<Models.Groups>> GetGroups(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Groups");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter: filter, top: top, skip: skip, orderby: orderby, expand: expand, select: select, count: count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            var response = await httpClient.SendAsync(httpRequestMessage);
            var re = await HttpResponseMessageExtensions.ReadAsync<IEnumerable<Models.Groups>>(response);
            return re;
            //return await HttpResponseMessageExtensions.ReadAsync<ODataServiceResult<Models.Groups>>(response);
        }

        public async Task<IEnumerable<Models.Teacher>> GetTeachers(Query query)
        {
            return await GetTeachers(filter: $"{query.Filter}", orderby: $"{query.OrderBy}", top: query.Top, skip: query.Skip, count: query.Top != null && query.Skip != null);
        }

        public async Task<IEnumerable<Models.Teacher>> GetTeachers(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Teachers");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter: filter, top: top, skip: skip, orderby: orderby, expand: expand, select: select, count: count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            var response = await httpClient.SendAsync(httpRequestMessage);
            var r = await Radzen.HttpResponseMessageExtensions.ReadAsync<IEnumerable<Models.Teacher>>(response);
            return r;
            //return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<Models.Teacher>>(response);
        }

    }
}
