using Kellys.Models;
using Kellys.Models.Domain;
using Kellys.Models.Requests;
using System.Collections.Generic;

namespace Kellys.Services
{
    public interface IInfluencerService
    {
        void Delete(int userId);
        int Insert(InfluencerAddRequest influencer, int userId);
        Paged<Influencer> Pagination(int pageIndex, int pageSize);
        Paged<Influencer> SearchPaginated(string search, int pageIndex, int pageSize);
        List<Influencer> SelectAll();
        Influencer SelectById(int id);
        Influencer SelectByUserId(int userId);
        void Update(InfluencerUpdateRequest influencer, int id);
    }
}