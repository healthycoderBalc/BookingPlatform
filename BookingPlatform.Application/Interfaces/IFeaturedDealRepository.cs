using BookingPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Interfaces
{
    public interface IFeaturedDealRepository : IRepository<FeaturedDeal>
    {
        public Task<ICollection<FeaturedDeal>> GetFeaturedDealsAsync();
    }
}
