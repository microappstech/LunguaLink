using Langua.Models;
using Langua.Repositories.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Langua.Repositories.Services
{
    public class AIService: IAIService
    {
        private readonly HttpClient _httpClient;
        private const string token = "AIzaSyDsD8vONzus0kKJHXIjJeSxrTthW9UG008";
        private static readonly string endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={token}";
        public AIService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }   
        public async Task<GeminiApiResponse> AskGemini(GeminiRequest geminiRequest)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
            request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(geminiRequest), Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(request);
            var strJson = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<GeminiApiResponse>(strJson);
            return result;
        }
    }
}
