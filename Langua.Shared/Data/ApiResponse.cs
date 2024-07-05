using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langua.Api
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
    public class ApiResponse<T> :ApiResponse
    {
        public T Data { get;set; }
    }
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiredAt {  get; set; } 
}
