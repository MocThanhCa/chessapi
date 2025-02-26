using chessdemo.Models.DTOs;
using chessdemo.Models.Entities;
using chessdemo.Services.Interfaces;

namespace chessdemo.Services
{
    public class GameService : IGameService
    {
        private readonly IMoveValidator _moveValidator;
        private readonly Dictionary<Guid, GameState> _games;//hàm Dictionary dùng để ánh xạ key-value 

        public GameService(IMoveValidator moveValidator)
        {
            _moveValidator = moveValidator;
            _games = new Dictionary<Guid, GameState>();
        }

        public Guid CreateNewGame()
        {
            var gameId = Guid.NewGuid();
            _games[gameId] = new GameState
            {
                board = new Board(),
                IsRedTurn = true
            };
            return gameId;
        }

        public GameState GetGameState(Guid gameId)
        {
            return _games.GetValueOrDefault(gameId);
        }

        public bool MakeMove(Guid gameId, MoveRequest request)//kiểm tra tính hợp lệ và di chuyển 
        {
            if (!_games.TryGetValue(gameId, out var gameState))
                return false;

            var piece = gameState.board.GetCell(request.fromX, request.fromY);

            if (piece == null || piece.PieceType == ChessPieceType.None ||
                piece.IsRed != gameState.IsRedTurn)
                return false;

            if (!_moveValidator.ValidateMove(
                piece.PieceType,
                piece.IsRed,
                request.fromX,
                request.fromY,
                request.toX,
                request.toY,
                gameState.board))
                return false;

            gameState.board.MovePiece(request.fromX, request.fromY, request.toX, request.toY);
            gameState.IsRedTurn = !gameState.IsRedTurn;

            return true;
        }

        public bool IsValidMove(Guid gameId, MoveRequest request)//kiểm tra có hợp lệ hay không thôi
        {
            if (!_games.TryGetValue(gameId, out var gameState))//out ở đây có nghĩa là gán giá trị có thể thay đổi linh hoạt cho biến
                return false;

            var piece = gameState.board.GetCell(request.fromX, request.fromY);

            if (piece == null || piece.PieceType == ChessPieceType.None ||
                piece.IsRed != gameState.IsRedTurn)
                return false;

            return _moveValidator.ValidateMove(
                piece.PieceType,
                piece.IsRed,
                request.fromX,
                request.fromY,
                request.toX,
                request.toY,
                gameState.board);
        }
    }
}
