using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalk.CustomActionFilters;
using NZWalk.Models.Domain;
using NZWalk.Models.DTO;
using NZWalk.Repositories;

namespace NZWalk.Controllers
{
    //https://localhost:portnumber/api/walks
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }

        // CREATE Walk
        //POST: https://localhost:portnumber/api/walks
        [HttpPost]
        [ValidateModel] //Custom validate model attribute
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
                //Map Input Dto to Domain Model
                var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);

                await walkRepository.CreateAsync(walkDomainModel);

                //Map Domain Model back to Dto and return
                return Ok(mapper.Map<WalkDto>(walkDomainModel));

        }

        //GET Walks
        //GET: https://localhost:portnumber/api/walks
        //GET: https://localhost:portnumber/api/walks?filterOn=Name&filterQuery=Track&sortBy=Name&isAscending=true&pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,   //Query parameters for filtering 
                                                [FromQuery] string? sortBy, [FromQuery] bool? isAscending,       //Query parameters for sorting
                                                [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000) //Query parameters for pagination    
        {
            var walkDomainModel = await walkRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);

            throw new Exception("New exception");

            return Ok(mapper.Map<List<WalkDto>>(walkDomainModel));

        }

        //GET Walk by Id
        //GET: https://localhost:portnumber/api/walks/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomainModel = await walkRepository.GetByIdAsync(id);

            if (walkDomainModel == null) { return NotFound(); }

            //Map Domain Model to Dto and return
            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }

        //Update Walk by Id
        //PUT: https://localhost:portnumber/api/walks/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel] //Custom validate model attribute
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalkRequestDto updateWalkRequestDto)
        {
            //Map Dto to Domain Model
            var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);

            walkDomainModel = await walkRepository.UpdateAsync(id, walkDomainModel);

            if (walkDomainModel == null) { return NotFound(); }

            return Ok(mapper.Map<WalkDto>(walkDomainModel));

        }

        //Delete Walk by Id
        //PUT: https://localhost:portnumber/api/walks/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deletedWalkDomainModel = await walkRepository.DeleteAsync(id);

            if (deletedWalkDomainModel == null) { return NotFound(); }

            return Ok(mapper.Map<WalkDto>(deletedWalkDomainModel));

        }
    }
}
