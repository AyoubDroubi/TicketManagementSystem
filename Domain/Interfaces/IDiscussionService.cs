using Domain.DTO.Request;
using Domain.DTO.Response;
namespace Domain.Interfaces
{
    public interface IDiscussionService
    {
        List<DiscussionResponse> GetDiscussions(int ticketId);
        Task<BaseResponse> Create(CreateDiscussionRequest request);
    }
}