using NZWalk.Models.Domain;

namespace NZWalk.Repositories
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image image);
    }
}
