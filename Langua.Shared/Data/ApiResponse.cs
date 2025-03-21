﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public ApiResponse()
        {
            
        }
        public ApiResponse( bool success, T data, string message="")
        {
            this.Data = data;
            this.Success = success;
            this.Message = message;
        }
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
    public class SessionResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        //public Groups? Group { get; set; }
        public int GroupId { get; set; }
        public string? TeacherName { get; set; }
        public int TeacherId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End
        {
            get; set;
        }
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
    public class ContenuResponse
    {

        public int Id { get; set; }
        public string? Name { get; set; }
        public string Description { get; set; }
        public byte[]? ContentBytes { get; set; }
        public string? ContentFile { get; set; }
        public int RessourceType { get; set; }
        public string? Url { get; set; }
        public DateTime? CreatedAt { get; set; }

    }
}
