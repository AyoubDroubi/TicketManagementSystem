using Domain.DTO.Request;
using Domain.DTO.Response;
using Domain.Entities;
using Microsoft.AspNetCore.Components.Forms;

namespace Domain.Interfaces
{
    public interface IAccountService
    {
        Task<BaseResponse<string>> VerifyUser(string email, string password);
        Task<BaseResponse> RegisterUser(RegisterUserRequest request); 
        List<GetUserResponse> GetUsers();
        Task<BaseResponse<User>> GetCurrentUser();
        Task<BaseResponse> RemoveUser(string email);
        Task<BaseResponse> ChangePassword(ChangePasswordRequest request);
        Task<BaseResponse<string>> UploadAvatar(IBrowserFile image);
        Task<BaseResponse> ResetAvatar();
    }
}
