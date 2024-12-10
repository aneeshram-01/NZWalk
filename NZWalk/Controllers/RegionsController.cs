using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalk.API.Repositories;
using NZWalk.CustomActionFilters;
using NZWalk.Data;
using NZWalk.Models.Domain;
using NZWalk.Models.DTO;
using NZWalks.API.Models.DTO;

namespace NZWalk.Controllers
{
    //https://localhost:portnumber/api/regions
    [Route("api/[controller]")]
    [ApiController]
    
    public class RegionsController : ControllerBase
    {
        private readonly NZWalkDbContext dbContext; //Not needed since it is implementd in Repository
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(NZWalkDbContext dbContext, IRegionRepository regionRepository, IMapper mapper, ILogger<RegionsController> logger)
        {
            this.dbContext = dbContext; //Not needed since it is implementd in Repository
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        //GET ALL REGIONS
        //GET: https://localhost:portnumber/api/regions
        [HttpGet]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll()
        {
            
            //Get data from Database - Domain Models
            var regionsDomain = await regionRepository.GetAllAsync(); //Async call to Repository which in turn implements call to database

            //Map Domain to Dtos
            /*var regionsDto = new List<RegionDto>();
            foreach (var regionDomain in regionsDomain )
            {
                regionsDto.Add(new RegionDto()
                { 
                    Id = regionDomain.Id,
                    Name = regionDomain.Name,
                    Code = regionDomain.Code,
                    RegionImageUrl = regionDomain.RegionImageUrl
                });
            }*/

            //Map Domain to Dtos using Automapper
            var regionsDto = mapper.Map<List<RegionDto>>(regionsDomain);

            //Return Dtos
            logger.LogInformation($"Finished GetAllRegions request with data: {JsonSerializer.Serialize(regionsDomain)}");
            return Ok(regionsDto);

            /*
            //Logging
            logger.LogInformation("GetAllRegions action method was invoked"); //For MinimumLevel.Information()
            logger.LogWarning("This is a warning log"); //For MinimumLevel.Warning()
            logger.LogError("This is an error log");  //For MinimumLevel.Warning()
            */
            
        }


        //GET SINGLE REGIONS (GET Region by Id)
        //GET: https://localhost:portnumber/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //var region = dbContext.Regions.Find(id);
            // Get Region Domain Model from Database
            var regionsDomain = await regionRepository.GetByIdAsync(id); //Async call to Repository which in turn implements call to database

            if (regionsDomain == null)
            { 
                return NotFound(); 
            }

            //Map Region Domain to Region Dtos
            // var regionsDto = mapper.Map<RegionDto>(regionsDomain);

            //Return Dtos converted using Automapper
            return Ok(mapper.Map<RegionDto>(regionsDomain)); 
        }

        //POST to create new region
        // POST: https://localhost:portnumber/api/regions
        [HttpPost]
        [ValidateModel] //Custom validate model attribute
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto) 
        {
            //Map Dto to Domain Model using Automapper
            var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

            //Use Domain Model to create region
            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel); //Async call to Repository which in turn implements call to database

            //Map Domain Model back to DTO using Automapper
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);

        }

        //Update region
        // POST: https://localhost:portnumber/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel] //Custom validate model attribute
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute]Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            //Map Dto to Domain model
            var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

            regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel); //Async call to Repository which in turn implements call to database

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            //Convert Domain Model to Dto and 
            return Ok(mapper.Map<RegionDto>(regionDomainModel));

        }

        //Delete Region
        //DELETE: https://localhost:portnumber/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer,Reader")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id); //Async call to Repository which in turn implements call to database

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            //return deleted region back
            //Map Domain Model Dto
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return Ok(regionDto);
        }
    }
}
