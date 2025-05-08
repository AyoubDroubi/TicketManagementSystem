using System;

namespace Domain.DTO.Response
{
    public class GetTicketResponse
    {
        public int TicketId { get; set; }
        public string TicketIdView
        {
            get
            {
                return "T" + TicketId.ToString().PadLeft(5, '0');
            }
        }

        public string Summary { get; set; } = string.Empty;
        public string Description { get; set; }= string.Empty;

        public string Product { get; set; }= string.Empty;
        public int ProductId { get; set; }

        public string Category { get; set; }= string.Empty;
        public int CategoryId { get; set; }
        public string Priority { get; set; }= string.Empty;
        public int PriorityId { get; set; }

        public string Status { get; set; }= string.Empty;
        public string AssignedToId { get; set; }= string.Empty;
        public string RaisedBy { get; set; }= string.Empty;
        public string RaisedByName { get; set; }= string.Empty;
        public DateTime ExpectedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ClosedBy { get; set; }
        public DateTime? ClosedDate { get; set; }

        public List<AttachmentResponse> Attachments { get; set; } = new List<AttachmentResponse>();
        public string? RaisedByAvatar { get; set; }
    }
}

public class AttachmentResponse
{
    public string FileName { get; set; } = string.Empty;
    public string ServerFileName { get; set; } = string.Empty;
}