using DapperBeginnerCourse.Models;


namespace DapperBeginnerCourse.Repository.Interface
{
    public interface IVideoGameRepository
    {
        Task<List<VideoGame>> GetAllAsync();
    }
}
