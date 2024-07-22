using System;
using System.Collections.Generic;
using System.Drawing;
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
        public string UserId { get; set; }
        public DateTime ExpiredAt {  get; set; } 
    }
    public class MessageResponse
    {
        public int Id { get; set; }
        public string? MessageContent { get; set; }
        public int GroupId { get; set; }
        public Color Color{ get; set; }
        public bool IsFile { get; set; }
        public byte[]? ContentFile { get; set; }
        public bool SuccessSended { get; set; }
        public DtoUser User { get; set; }

    }
    public class DtoUser
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
    }
    public class ResponseProfile
    {
        public string? FullName { get; set; }
        public string? UserId { get; set; }
        public string? Photo { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? SubjectName { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsConnected { get; set; }
        public string? DepartementName { get; set; }
        public int DepartementId { get; set; }
        public string? GroupName { get; set; }
        public int? GroupId { get; set; }
        public bool ConfirmedMail { get; set; }

    }
    public class ApiSendedMessageState
    {
        public bool Sended { get; set; }
        public bool Recieved { get; set; }
        public string? Detail { get; set; }
    }
    public class ApiSendedMessage
    {
        public int GroupId { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
    }
}
