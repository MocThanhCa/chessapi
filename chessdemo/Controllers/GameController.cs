using chessdemo.Models.DTOs;
using chessdemo.Services;
using chessdemo.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace chessdemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        //[HttpGet("new")]
        //public IActionResult CreateNewGame()
        //{
        //    var gameId = _gameService.CreateNewGame();
        //    return Ok(new { GameId = gameId });
        //}
        [HttpPost("new")]
        public IActionResult CreateNewGame()
        {
            var gameId = _gameService.CreateNewGame();
            return CreatedAtAction(nameof(GetGameState), new { gameId }, new { GameId = gameId });
        }

        [HttpGet("{gameId}/state")]
        public IActionResult GetGameState(Guid gameId)
        {
            var state = _gameService.GetGameState(gameId);
            if (state == null)
                return NotFound();
            return Ok(state);
        }
        [HttpPatch("{gameId}/move")]
        public IActionResult MakeMove(Guid gameId, [FromBody] MoveRequest moveRequest)
        {
            try
            {
                if (moveRequest == null)
                    return BadRequest("Request body is missing or invalid");

                if (!_gameService.MakeMove(gameId, moveRequest))
                    return BadRequest("Invalid move");

                return Ok(_gameService.GetGameState(gameId));
            }
            catch (Exception)
            {
                return BadRequest("Invalid request format");
            }
        }

        //[HttpGet("{gameId}/move")]
        //public IActionResult MakeMove(Guid gameId, [FromQuery] string from, [FromQuery] string to)
        //{
        //    try
        //    {
        //        var fromCoords = from.Split(',').Select(int.Parse).ToArray();
        //        var toCoords = to.Split(',').Select(int.Parse).ToArray();

        //        var moveRequest = new MoveRequest
        //        {
        //            fromX = fromCoords[0],
        //            fromY = fromCoords[1],
        //            toX = toCoords[0],
        //            toY = toCoords[1]
        //        };

        //        if (!_gameService.MakeMove(gameId, moveRequest))
        //            return BadRequest("Invalid move");

        //        return Ok(_gameService.GetGameState(gameId));
        //    }
        //    catch (Exception)
        //    {
        //        return BadRequest("Invalid coordinates format");
        //    }
        //}
        /*
        return Ok(new
        {
            success = true,
            from = new { x = moveRequest.fromX, y = moveRequest.fromY
    },
            to = new { x = moveRequest.toX, y = moveRequest.toY
},
            nextTurn = !_gameService.GetGameState(gameId).IsRedTurn
        });
        */
    }
}
