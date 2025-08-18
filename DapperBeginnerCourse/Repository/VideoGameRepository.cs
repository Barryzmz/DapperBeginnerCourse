using Dapper;
using DapperBeginnerCourse.Models;
using DapperBeginnerCourse.Repository.Interface;
using Microsoft.Data.SqlClient;
using System.Data.SqlTypes;

namespace DapperBeginnerCourse.Repository
{
    public class VideoGameRepository : IVideoGameRepository
    {
        private readonly IConfiguration _configuration;
        public VideoGameRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task<List<VideoGame>> GetAllAsync()
        {
            const string sql = @"
                SELECT 
                    Id,        
                    Title,
                    Publisher,
                    Developer,
                    Platform,
                    ReleaseDate
                FROM VideoGames
                ORDER BY Id";

            using var connection = GetConnection();
            await connection.OpenAsync();

            var rows = await connection.QueryAsync<VideoGame>(sql);
            return rows.ToList(); // 轉成 List<VideoGame>
        }
    }
}
