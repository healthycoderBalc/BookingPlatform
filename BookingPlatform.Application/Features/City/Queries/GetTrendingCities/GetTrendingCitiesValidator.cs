﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.City.Queries.GetTrendingCities
{
    public class GetTrendingCitiesValidator : AbstractValidator<GetTrendingCitiesQuery>
    {
        public GetTrendingCitiesValidator()
        {
            
        }
    }
}