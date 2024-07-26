using BookingPlatform.Application.Features.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.City.Dtos
{
    public class CityFilterDto : BaseDto
    {
        public string? Name { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }
        public int? NumberOfHotels { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }

        public CityFilterDto()
        {
            
        }
        public CityFilterDto(string? name, string? country, string? postalCode, int? numberOfHotels, DateTime? creationDate, DateTime? modificationDate )
        {
            Name = name;
            Country = country;
            PostalCode = postalCode;
            NumberOfHotels = numberOfHotels;
            CreationDate = creationDate;
            ModificationDate = modificationDate;
        }
    }
}
