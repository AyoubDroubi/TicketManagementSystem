using System.ComponentModel.DataAnnotations;
namespace Domain.DTO.Request
{
    public class UpdateTicketRequest
    {
        [Required] public int TicketId { get; set; }
        [Required] public int? ProductId { get; set; }
        [Required] public int? CategoryId { get; set; }
        [Required] public int? PriorityId { get; set; }
        [Required] public string? AssignedToId { get; set; }
        public string Status { get; set; }
    }
}