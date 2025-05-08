using Domain.DTO.Request;
using Domain.DTO.Response;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Repository;
using Infrastructure.Common;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly SignInManager<User> signInManager;
        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IWebHostEnvironment environment;

        public AccountService(
            SignInManager<User> signInManager, 
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment environment
            )
        {
            this.signInManager = signInManager;
            this.unitOfWork = unitOfWork;
            this.httpContextAccessor = httpContextAccessor;
            this.environment = environment;
        }

        public async Task<BaseResponse> ChangePassword(ChangePasswordRequest request)
        {
            BaseResponse response = new BaseResponse();
            response.IsSuccess = false;

            var user = await GetCurrentUser();
            if (!user.IsSuccess)
            {
                response.ErrorMessage = user.ErrorMessage;
                return response;
            }

            var changePasswordResult = await signInManager.UserManager.ChangePasswordAsync(user.Value, request.CurrentPassword, request.NewPassword);
            if (changePasswordResult.Succeeded)
            {
                response.IsSuccess = true;

                //confirm account
                await ConfirmAccount(user.Value);
            } 
                else
            {
                response.ErrorMessage = changePasswordResult.Errors.FirstOrDefault()?.Description;
            }

            return response;
        }

        private async Task ConfirmAccount(User user)
        {
            if(user.AccountConfirmed == false)
            {
                user.AccountConfirmed = true;

                unitOfWork.Repository<User>().Update(user);
                await unitOfWork.SaveChanges();
            }
        }

        public async Task<BaseResponse<User>> GetCurrentUser()
        {
            BaseResponse<User> response = new BaseResponse<User>();
            response.IsSuccess = false;

            var currentUser = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
            if (currentUser == null) { 
                response.ErrorMessage = "Invalid User Account!";
                return response;
            }

            var user = await signInManager.UserManager.FindByEmailAsync(currentUser);
            if (user == null)
            {
                response.ErrorMessage = "Invalid User Account!";
                return response;
            }

            response.IsSuccess = true;
            response.Value = user;
            return response;
        }

        public List<GetUserResponse> GetUsers()
        {
            var roles = unitOfWork.Repository<IdentityUserRole<string>>().ListAll()
                .Select(x => new
                {
                    x.UserId,
                    x.RoleId,
                    Role = Constants.UserRoles[x.RoleId]
                });

            return unitOfWork.Repository<User>().ListAll()
                .Where(x => x.IsDeleted == false)
                .Select(x => new GetUserResponse { 
                    Id = x.Id,
                    Email = x.Email,
                    Avatar = x.Avatar,
                    Role = roles.FirstOrDefault(r => r.UserId == x.Id)?.Role,
                    AccountConfirmed = x.AccountConfirmed
                }).ToList();
            }

        public async Task<BaseResponse> RegisterUser(RegisterUserRequest request)
        {
            User user = new User
            {
                UserName = request.Email,
                Email = request.Email,
                AccountConfirmed = false,
            };

            string password = Constants.DEFAULT_PASSWORD;

            var result = await signInManager.UserManager.CreateAsync(user, password);

            if (result.Succeeded) {
                await signInManager.UserManager.AddToRoleAsync(user, request.Role);

                return new BaseResponse
                {
                    IsSuccess = true
                };
            } else
            {
                return new BaseResponse
                {
                    IsSuccess = false,
                    ErrorMessage = result.Errors.FirstOrDefault()?.Description
                };
            }
        }

        public async Task<BaseResponse> RemoveUser(string email)
        {
            BaseResponse response = new BaseResponse();
            response.IsSuccess = false;

            var user = await signInManager.UserManager.FindByEmailAsync(email);
            if (user == null)
            {
                response.ErrorMessage = "User not found! - " + email;
                return response;
            }

            user.IsDeleted = true;
            var result = await signInManager.UserManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                response.ErrorMessage = result.Errors.FirstOrDefault().Description;
                return response;
            }

            response.IsSuccess = true;
            return response;
        }

        public async Task<BaseResponse> ResetAvatar()
        {
            BaseResponse response = new BaseResponse();
            response.IsSuccess = false;
            var uploadPath = Path.Combine(environment.WebRootPath, "uploads", "avatars");

            var currentUser = await GetCurrentUser();
            if (!currentUser.IsSuccess)
            {
                response.ErrorMessage = currentUser.ErrorMessage;
                return response;
            }

            string previousAvatar;

            if (currentUser.Value.Avatar != Constants.DEFAULT_AVATAR) {

                previousAvatar = currentUser.Value.Avatar;

                if (!string.IsNullOrEmpty(previousAvatar)) {
                    previousAvatar = Path.Combine(uploadPath, previousAvatar);
                    if (File.Exists(previousAvatar))
                    {
                        File.Delete(previousAvatar);
                    }
                }

                currentUser.Value.Avatar = Constants.DEFAULT_AVATAR;
                var updateResult = await signInManager.UserManager.UpdateAsync(currentUser.Value);
                if (updateResult.Succeeded)
                {
                    response.IsSuccess = true;
                }
                else
                {
                    response.ErrorMessage = updateResult.Errors.FirstOrDefault().Description;
                }
            }

            return response;
        }

        public async Task<BaseResponse<string>> UploadAvatar(IBrowserFile image)
        {
            BaseResponse<string> response = new BaseResponse<string>();
            response.IsSuccess = false;
            string previousAvatar;
            var uploadPath = Path.Combine(environment.WebRootPath, "uploads", "avatars");


            var currentUser = await GetCurrentUser();
            if (!currentUser.IsSuccess)
            {
                response.ErrorMessage = currentUser.ErrorMessage;
                return response;
            }

            if (image != null)
            {
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                if (currentUser.Value.Avatar != null && currentUser.Value.Avatar != Constants.DEFAULT_AVATAR)
                {
                    previousAvatar = currentUser.Value.Avatar;
                    previousAvatar = Path.Combine(uploadPath, previousAvatar);
                    if (File.Exists(previousAvatar))
                    {
                        File.Delete(previousAvatar);
                    }
                }

                var fileExtension = Path.GetExtension(image.Name);

                string fileName = $"{currentUser.Value.Email}{fileExtension}";
                var filePath = Path.Combine(uploadPath, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await image.OpenReadStream().CopyToAsync(fileStream);
                }

                currentUser.Value.Avatar = fileName;

                var updateResult = await signInManager.UserManager.UpdateAsync(currentUser.Value);
                if (updateResult.Succeeded)
                {
                    response.IsSuccess = true;
                    response.Value = fileName;
                } else
                {
                    response.ErrorMessage = updateResult.Errors.FirstOrDefault().Description;
                }
            }

            return response;
        }

        public async Task<BaseResponse<string>> VerifyUser(string email, string password)
        {
            BaseResponse<string> response = new();

            var user = await signInManager.UserManager.FindByEmailAsync(email);
            if(user is null || user.IsDeleted)
            {
                response.ErrorMessage = "User is not found!";
                response.IsSuccess = false;
                return response;
            }

            var result = await signInManager.UserManager.CheckPasswordAsync(user, password);
            response.IsSuccess = result;
            if (!result)
            {
                response.ErrorMessage = "Invalid Email / Password";
            } else
            {
                response.Value = user.UserName;
            }
            return response;
        }
    }
}
