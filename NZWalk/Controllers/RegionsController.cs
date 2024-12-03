using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalk.Data;
using NZWalk.Models.DTO;

namespace NZWalk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalkDbContext dbContext;

        public RegionsController(NZWalkDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        //GET ALL REGIONS
        //GET: https://localhost:portnumber/api/regions
        [HttpGet]
        public IActionResult GetAll()
        {
            //Get data from Database - Domain Models
            var regionsDomain = dbContext.Regions.ToList();

            //Map Domain to Dtos
            var regionsDto = new List<RegionDto>();
            foreach (var regionDomain in regionsDomain )
            {
                regionsDto.Add(new RegionDto()
                { 
                    Id = regionDomain.Id,
                    Name = regionDomain.Name,
                    Code = regionDomain.Code,
                    RegionImageUrl = regionDomain.RegionImageUrl
                });
            }

            //Return Dtos
            return Ok(regionsDto);
        }


        //GET SINGLE REGIONS (GET Region by Id)
        //GET: https://localhost:portnumber/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            //var region = dbContext.Regions.Find(id);
            // Get Region Domain Model from Database
            var regionsDomain = dbContext.Regions.FirstOrDefault(x => x.Id == id);

            if (regionsDomain == null)
            { 
                return NotFound(); 
            }

            //Map Region Domain to Region Dtos
            var regionsDto = new RegionDto
            {
                Id = regionsDomain.Id,
                Name = regionsDomain.Name,
                Code = regionsDomain.Code,
                RegionImageUrl = regionsDomain.RegionImageUrl
            };

            //Return Dtos
            return Ok(regionsDto); 
        }

    }
}
