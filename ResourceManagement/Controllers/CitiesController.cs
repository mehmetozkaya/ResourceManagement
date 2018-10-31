using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ResourceManagement.Helpers;
using ResourceManagement.Models;
using ResourceManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceManagement.Controllers
{
    [Route("api/cities")]
    public class CitiesController : Controller
    {
        private ICityInfoRepository _cityInfoRepository;
        private IUrlHelper _urlHelper;

        public CitiesController(ICityInfoRepository cityInfoRepository, IUrlHelper urlHelper)
        {
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _urlHelper = urlHelper;
        }

        [HttpGet()]
        public IActionResult GetCities()
        {
            var cityEntities = _cityInfoRepository.GetCities();
            var results = AutoMapper.Mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities);

            return Ok(results);
        }

        [HttpGet(Name = "GetCitiesWithPaging")]
        public IActionResult GetCitiesWithPaging(CityResourceParameters cityResourceParameters)
        {
            var cityEntitiesFromRepo = _cityInfoRepository.GetCitiesWithPaging(cityResourceParameters);

            var previousPageLink = cityEntitiesFromRepo.HasPrevious ? CreateCityResourceUri(cityResourceParameters, ResourceUriType.PreviousPage) : null;
            var nextPageLink = cityEntitiesFromRepo.HasNext ? CreateCityResourceUri(cityResourceParameters, ResourceUriType.NextPage) : null;

            var paginationMetadata = new
            {
                totalCount = cityEntitiesFromRepo.TotalCount,
                pageSize = cityEntitiesFromRepo.PageSize,

            };

            var results = AutoMapper.Mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities);

            return Ok(results);
        }

        private string CreateCityResourceUri(CityResourceParameters cityResourceParameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetCitiesWithPaging",
                        new
                        {
                            pageNumber = cityResourceParameters.PageNumber - 1,
                            pageSize = cityResourceParameters.PageSize
                        });                    
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetCitiesWithPaging",
                       new
                       {
                           pageNumber = cityResourceParameters.PageNumber + 1,
                           pageSize = cityResourceParameters.PageSize
                       });
                default:
                    return _urlHelper.Link("GetCitiesWithPaging",
                      new
                      {
                          pageNumber = cityResourceParameters.PageNumber,
                          pageSize = cityResourceParameters.PageSize
                      });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id, bool includePointsOfInterest = false)
        {
            var city = _cityInfoRepository.GetCity(id, includePointsOfInterest);

            if (city == null)
            {
                return NotFound();
            }

            if (includePointsOfInterest)
            {
                var cityResult = Mapper.Map<CityDTO>(city);
                return Ok(cityResult);
            }

            var cityWithoutPointsOfInterestResult = Mapper.Map<CityWithoutPointsOfInterestDto>(city);
            return Ok(cityWithoutPointsOfInterestResult);
        }

    }

    public enum ResourceUriType
    {
        PreviousPage,
        NextPage
    }
}
