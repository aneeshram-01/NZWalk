﻿using NZWalk.Models.Domain;

namespace NZWalk.Repositories
{
    public interface IWalkRepository
    {
        Task<Walk> CreateAsync(Walk walk);
        Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null, //Query parameters for filtering
                                     string? sortBy = null, bool isAscending = true,      //Query parameters for sorting
                                     int pageNumber = 1, int pageSize = 1000);            //Query parameters for pagination
        Task<Walk?> GetByIdAsync(Guid id);
        Task<Walk?> UpdateAsync(Guid id, Walk walk);
        Task<Walk?> DeleteAsync(Guid id);

    }
}
