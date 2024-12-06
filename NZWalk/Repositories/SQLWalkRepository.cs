using Microsoft.EntityFrameworkCore;
using NZWalk.Data;
using NZWalk.Models.Domain;

namespace NZWalk.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalkDbContext dbContext;

        public SQLWalkRepository(NZWalkDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
                                                  string? sortBy = null, bool isAscending = true)
        {
            var walks = dbContext.Walks.Include("Difficulty")
                                       .Include("Region")
                                       .AsQueryable();

            //Filtering
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            { 
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))  //Filter based on Name
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                }
            }

            //Sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))  //Filter based on Name
                {
                    walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                }

                else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))  //Filter based on Length
                {
                    walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
                }
            }


            return await walks.ToListAsync();

            /*return await dbContext.Walks.Include("Difficulty")
                                        .Include("Region") //To make it Typesafe use .Include(x => x.Region)
                                        .ToListAsync(); */
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            return await dbContext.Walks.Include("Difficulty")
                                 .Include("Region")
                                 .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var exisitingWalk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id); 

            if (exisitingWalk == null) { return null; }

            exisitingWalk.Name = walk.Name;
            exisitingWalk.Description = walk.Description;
            exisitingWalk.LengthInKm = walk.LengthInKm;
            exisitingWalk.WalkImgUrl = walk.WalkImgUrl;
            exisitingWalk.DifficultyId = walk.DifficultyId;
            exisitingWalk.RegionId = walk.RegionId;

            await dbContext.SaveChangesAsync();

            return exisitingWalk;
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var exisitingWalk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if (exisitingWalk == null) { return null; }

            dbContext.Walks.Remove(exisitingWalk);
            await dbContext.SaveChangesAsync();
            return exisitingWalk;
        }
    }
}
