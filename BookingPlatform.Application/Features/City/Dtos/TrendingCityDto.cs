using BookingPlatform.Application.Features.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.City.Dtos
{
    public class TrendingCityDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string ThumbnailUrl { get; set; } = string.Empty;

        public int NumberOfVisits { get; set; }
    }
}
