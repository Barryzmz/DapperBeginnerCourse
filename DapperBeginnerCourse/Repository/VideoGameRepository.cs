using Dapper;
using DapperBeginnerCourse.Models;
using DapperBeginnerCourse.Repository.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using System.Data.SqlTypes;
using static Dapper.SqlMapper;

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

        public async Task<VideoGame> GetByIdAsync(int id)
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
                WHERE Id = @Id";
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@Id", id);
                var videoGame = await connection.QuerySingleOrDefaultAsync<VideoGame>(sql, dynamicParameters);
                return videoGame;
            }
        }

        public async Task AddAsync(VideoGame videoGame)
        {
            const string sql = @"
                INSERT INTO VideoGames
                    (Title, Publisher, Developer, Platform, ReleaseDate)     
                VALUES
                    (@Title, @Publisher, @Developer, @Platform, @ReleaseDate)";
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();
                DynamicParameters dynamicParameters = new DynamicParameters(videoGame);
                //dynamicParameters.Add();
                //await connection.ExecuteByParamsAsync(sql, dynamicParameters);
                var result = await connection.ExecuteAsync(sql, dynamicParameters);
            }
        }

        public async Task UpdateAsync(VideoGame videoGame)
        {
            const string sql = @"
                UPDATE VideoGames
                    SET Title = @Title, Publisher = @Publisher, Developer = @Developer, Platform =  @Platform, ReleaseDate = @ReleaseDate  
                WHERE
                    Id = @Id";
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();
                DynamicParameters dynamicParameters = new DynamicParameters(videoGame);
                var result = await connection.ExecuteAsync(sql, dynamicParameters);
            }
        }
    }
}
