using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalk.Data;

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
            var region = dbContext.Regions.ToList();

            return Ok(region);
        }


        //GET SINGLE REGIONS (GET Region by Id)
        //GET: https://localhost:portnumber/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            //var region = dbContext.Regions.Find(id);

            var region = dbContext.Regions.FirstOrDefault(x => x.Id == id);

            if (region == null)
            { 
                return NotFound(); 
            }

            return Ok(region); 
        }

    }
}
