using Domain.DTO.Request;
using Domain.DTO.Response;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Repository;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Net.Sockets;

namespace Infrastructure.Services
{
    public class DiscussionService : IDiscussionService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAccountService accountService;
        private readonly IWebHostEnvironment environment;
        private readonly IDiscussionRepository discussionRepository;

        public DiscussionService(IUnitOfWork unitOfWork,
            IAccountService accountService,
            IWebHostEnvironment environment,
            IDiscussionRepository discussionRepository
            )
        {
            this.unitOfWork = unitOfWork;
            this.accountService = accountService;
            this.environment = environment;
            this.discussionRepository = discussionRepository;
        }

        public async Task<BaseResponse> Create(CreateDiscussionRequest request)
        {
            BaseResponse response = new BaseResponse();
            response.IsSuccess = false;
            var uploadPath = Path.Combine(environment.WebRootPath, "uploads", "attachments");

            var currentUser = await accountService.GetCurrentUser();
            if (!currentUser.IsSuccess) {
                response.ErrorMessage = currentUser.ErrorMessage;
                return response;
            }

            Discussion discussion = new Discussion
            {
                CreatedDate = DateTime.Now,
                Message = request.Message,
                TicketId = request.TicketId,
                UserId = currentUser.Value.Id
            };

            unitOfWork.DiscussionRepository.Add(discussion);

            if (request.Attachments != null && request.Attachments.Count > 0)
            {
                foreach (var file in request.Attachments)
                {
                    var fileExt = Path.GetExtension(file.Name);
                    var actualName = Path.GetFileNameWithoutExtension(file.Name);

                    var fileName = $"{actualName}-{Guid.NewGuid().ToString()}{fileExt}";
                    var filePath = Path.Combine(uploadPath, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.OpenReadStream().CopyToAsync(fileStream);
                    }

                    Attachment attachment = new Attachment
                    {
                        Discussion = discussion,
                        Filename = Path.GetFileName(file.Name),
                        ServerFileName = fileName,
                        FileSize = file.Size,
                        CreatedDate = DateTime.Now,
                    };
                    unitOfWork.Repository<Attachment>().Add(attachment);
                }
            }


            var result = await unitOfWork.SaveChanges() > 0;

            if (result)
            {
                response.IsSuccess = true;
                return response;
            }
            else
            {
                response.ErrorMessage = "Failed when create discussion!";
                return response;
            }

        }

        public List<DiscussionResponse> GetDiscussions(int ticketId)
        {
            var uploadPath = Path.Combine("uploads", "attachments");

            var result = discussionRepository.GetDiscussions(ticketId);

            return result.Select(x => new DiscussionResponse
            {
                Message = x.Message,
                CreatedDate = x.CreatedDate,
                User = x.User,
                Attachments = x.Attachments.Select(x => new AttachmentResponse
                {
                    FileName = x.Filename,
                    ServerFileName = Path.Combine(uploadPath, x.ServerFileName),
                }).ToList()
            }).ToList();  
        }
    }
}