using System;
namespace Domain.DTO.Response
{
    public class GetUserResponse
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string? Role { get; set; }
        public bool AccountConfirmed { get; set; }
    }
}