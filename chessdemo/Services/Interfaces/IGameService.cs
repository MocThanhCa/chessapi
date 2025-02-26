using chessdemo.Models.DTOs;
using chessdemo.Models.Entities;

namespace chessdemo.Services.Interfaces
{
    public interface IGameService
    {
        Guid CreateNewGame();//ban chat la string
        GameState GetGameState(Guid gameId);
        bool MakeMove(Guid gameId, MoveRequest request);
        bool IsValidMove(Guid gameId, MoveRequest request);
    }
}
