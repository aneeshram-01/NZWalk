using NZWalk.Models.Domain;

namespace NZWalk.API.Repositories
{
    public interface IRegionRepository //To list out all the methods that need to be implemented
    {
        Task<List<Region>> GetAllAsync();
        Task<Region?> GetByIdAsync(Guid Id);
        Task<Region> CreateAsync(Region region);
        Task<Region?> UpdateAsync(Guid id, Region region);
        Task<Region?> DeleteAsync(Guid id);
    }
}
