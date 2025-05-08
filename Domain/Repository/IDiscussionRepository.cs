using Domain.Entities;
using System;

namespace Domain.Repository
{
    public interface IDiscussionRepository : IGenericRepository<Discussion>
    {
        List<Discussion> GetDiscussions(int ticketId);
    }
}