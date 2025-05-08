using Domain.DTO.Request;
using Domain.DTO.Response;

namespace Domain.Interfaces
{
    public interface ITicketService
    {
        List<GetTicketResponse> GetTickets(GetTicketsRequest request);
        GetTicketResponse FindTicket(int ticketId);
        Task<BaseResponse> UpdateTicket(UpdateTicketRequest request);
        Task<BaseResponse<int>> CreateTicket(CreateTicketRequest request);


        List<ChartResponse> Last12MonthTickets();
        List<ChartResponse> ChartByCategory(string category);
        List<ChartResponse> GetSummary();
    }
}