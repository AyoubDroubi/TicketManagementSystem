using Microsoft.AspNetCore.Components.Forms;
using System;
using System.ComponentModel.DataAnnotations;
namespace Domain.DTO.Request
{
    public class CreateTicketRequest
    {
        [Required] public string Summary { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required] public int? ProductId { get; set; }
        [Required] public int? CategoryId { get; set; }
        [Required] public int? PriorityId { get; set; }
        [Required] public string? AssignedToId { get; set; }
        public IList<IBrowserFile> files { get; set; } = new List<IBrowserFile>();
    }
}