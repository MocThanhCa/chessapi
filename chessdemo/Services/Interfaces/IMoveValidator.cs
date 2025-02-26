using chessdemo.Models.Entities;

namespace chessdemo.Services.Interfaces
{
    public interface IMoveValidator
    {
        bool ValidateMove(ChessPieceType pieceType, bool isRed, int fromX, int fromY, int toX, int toY, Board board);
    }
}
