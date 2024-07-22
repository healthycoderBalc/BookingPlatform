using AutoMapper;
using BookingPlatform.Application.Features.City.Commands.DeleteCity;
using BookingPlatform.Application.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.City.Commands.DeleteCity
{
    public class DeleteCityCommandHandler : IRequestHandler<DeleteCityCommand, DeleteCityResponse>
    {
        private readonly IRepository<Domain.Entities.City> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public DeleteCityCommandHandler(IRepository<Domain.Entities.City> repository, IMapper mapper, ILogger<DeleteCityCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<DeleteCityResponse> Handle(DeleteCityCommand request, CancellationToken cancellationToken)
        {
            var deleteCityResponse = new DeleteCityResponse();
            var validator = new DeleteCityValidator(_repository);

            try
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (validationResult.Errors.Count > 0)
                {
                    deleteCityResponse.Success = false;
                    deleteCityResponse.ValidationErrors = new List<string>();
                    foreach (var error in validationResult.Errors.Select(x => x.ErrorMessage))
                    {
                        deleteCityResponse.ValidationErrors.Add(error);
                        _logger.LogError($"Validation failed due to error- {error}");
                    }
                }
                else if (deleteCityResponse.Success)
                {
                    var cityEntity = await _repository.GetByIdAsync(request.Id);
                    await _repository.DeleteAsync(cityEntity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while due to error- {ex.Message}");
                deleteCityResponse.Success = false;
                deleteCityResponse.Message = ex.Message;
            }

            return deleteCityResponse;
        }
    }
}
