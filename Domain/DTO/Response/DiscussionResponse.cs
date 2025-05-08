using Domain.Entities;
namespace Domain.DTO.Response
{
    public class DiscussionResponse
    {
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public User User { get; set; } = new User();
        public List<AttachmentResponse> Attachments { get; set; } = new List<AttachmentResponse>();
    }
}