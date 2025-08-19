using DapperBeginnerCourse.Models;
using DapperBeginnerCourse.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DapperBeginnerCourse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoGamesController : ControllerBase
    {
        private readonly IVideoGameRepository _videoGameRepository;

        public VideoGamesController(IVideoGameRepository videoGameRepository)
        {
            _videoGameRepository = videoGameRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<VideoGame>>> GetAllAsync()
        {
            var videoGames = await _videoGameRepository.GetAllAsync();
            return Ok(videoGames);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VideoGame>> GetByIdAsync(int id)
        {
            var videoGame = await _videoGameRepository.GetByIdAsync(id);
            if (videoGame == null)
            {
                return NotFound("This Video Game does not exist");
            }
            return Ok(videoGame);
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync(VideoGame videoGame)
        {
            await _videoGameRepository.AddAsync(videoGame);
            return Ok("success");
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAsync(VideoGame videoGame)
        {
            var existVideoGame = await _videoGameRepository.GetByIdAsync(videoGame.Id);

            if (existVideoGame == null)
            {
                return NotFound("This Video Game does not exist");
            }
            else
            {
                await _videoGameRepository.UpdateAsync(videoGame);
                return Ok("success");
            }
        }
    }
}
