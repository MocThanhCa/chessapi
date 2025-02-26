using chessdemo.Models.Entities;
using chessdemo.Services.Interfaces;

namespace chessdemo.Services
{
    public class MoveValidator : IMoveValidator
    {
        public bool ValidateMove(ChessPieceType pieceType, bool isRed, int fromX, int fromY, int toX, int toY, Board board)
        {
            switch (pieceType)
            {
                case ChessPieceType.Xe:
                    return ValidateXeMove(fromX, fromY, toX, toY, board);
                case ChessPieceType.Ma:
                    return ValidateMaMove(fromX, fromY, toX, toY, board);
                case ChessPieceType.Tinh:
                    return ValidateTinhMove(fromX, fromY, toX, toY, board);
                case ChessPieceType.Si:
                    return ValidateSiMove(fromX, fromY, toX, toY, board);
                case ChessPieceType.Tuong:
                    return ValidateTuongMove(fromX, fromY, toX, toY, board);
                case ChessPieceType.Phao:
                    return ValidatePhaoMove(fromX, fromY, toX, toY, board);
                case ChessPieceType.Tot:
                    return ValidateTotMove(fromX, fromY, toX, toY, board);
                default:
                    return false;
            }
        }
        //logic di chuyển của Xe
        private bool ValidateXeMove(int fromX, int fromY, int toX, int toY, Board board)
        {
            var sourceCell = board.GetCell(fromX, fromY);
            var targetCell = board.GetCell(toX, toY);
            if (targetCell.PieceType != ChessPieceType.None && targetCell.IsRed == sourceCell.IsRed)
                return false;
            // Xe chỉ đi thẳng theo hàng ngang hoặc dọc
            if (fromX != toX && fromY != toY) return false;

            if (fromY == toY)//di chuyen doc
            {
                int start = Math.Min(fromX, toX);
                int end = Math.Max(fromX, toX);
                for (int x = start + 1; x <= end; x++)//kiem tra xem co quan nao chan khong
                {
                    if (targetCell.IsRed != sourceCell.IsRed)
                        return true;
                }
            }
            else // Di chuyển ngang
            {
                int start = Math.Min(fromY, toY);
                int end = Math.Max(fromY, toY);
                for (int y = start + 1; y < end; y++)
                {
                    if (targetCell.IsRed != sourceCell.IsRed)
                        return true;
                }
            }
            return true;
        }
        //Logic di chuyển của pháo
        private bool ValidatePhaoMove(int fromX, int fromY, int toX, int toY, Board board)
        {
            var sourceCell = board.GetCell(fromX, fromY);
            var targetCell = board.GetCell(toX, toY);
            // Pháo đi thẳng như xe
            if (fromX != toX && fromY != toY) return false;

            int pieceCount = 0;
            if (fromY == toY) // Di chuyển dọc
            {
                int start = Math.Min(fromX, toX);
                int end = Math.Max(fromX, toX);
                for (int x = start + 1; x < end; x++)
                {
                    if (board.GetCell(x, fromY).PieceType != ChessPieceType.None) pieceCount++;
                }
            }
            else // Di chuyển ngang
            {
                int start = Math.Min(fromY, toY);
                int end = Math.Max(fromY, toY);
                for (int y = start + 1; y < end; y++)
                {
                    if (board.GetCell(fromX, y).PieceType != ChessPieceType.None) pieceCount++;
                }
            }
            if (targetCell.IsRed != sourceCell.IsRed && pieceCount == 1 && targetCell.PieceType != ChessPieceType.None)
            {
                return true;
            }
            return board.GetCell(toX, toY).PieceType != ChessPieceType.None ? pieceCount == 1 : pieceCount == 0;
        }
        //Logic di chuyển của Mã
        private bool ValidateMaMove(int fromX, int fromY, int toX, int toY, Board board)
        {
            var sourceCell = board.GetCell(fromX, fromY);
            var targetCell = board.GetCell(toX, toY);
            int deltaX = Math.Abs(fromX - toX);
            int deltaY = Math.Abs(fromY - toY);
            //kiem tra dich den hop le theo phuong thuc di chuyen
            if (!((deltaX == 1 && deltaY == 2) || (deltaX == 2 && deltaY == 1)))
                return false;
            //kiem tra chan
            if (deltaX == 2)//theo chieu doc
            {
                if (fromX > toX)
                {
                    int blockX = fromX - 1;
                    if (board.GetCell(blockX, fromY).PieceType != ChessPieceType.None) return false;
                }
                else
                {
                    int blockX = fromX + 1;
                    if (board.GetCell(blockX, fromY).PieceType != ChessPieceType.None) return false;
                }
            }
            else//theo chieu ngang
            {
                if (fromY > toY)
                {
                    int blockY = fromY - 1;
                    if (board.GetCell(fromX, blockY).PieceType != ChessPieceType.None) return false;
                }
                else
                {
                    int blockY = fromY + 1;
                    if (board.GetCell(fromX, blockY).PieceType != ChessPieceType.None) return false;
                }
            }
            if (targetCell.PieceType != ChessPieceType.None)
            {
                if (targetCell.IsRed != sourceCell.IsRed) return true;
                else return false;
            }
            return true;
        }
        //Logic di chuyển cho tượng / tịnh 
        private bool ValidateTinhMove(int fromX, int fromY, int toX, int toY, Board board)
        {
            var sourceCell = board.GetCell(fromX, fromY);
            var targetCell = board.GetCell(toX, toY);
            int deltaX = Math.Abs(fromX - toX);
            int deltaY = Math.Abs(fromY - toY);
            //kiem tra dich den hop le theo phuong thuc di chuyen
            if (!(deltaX == 2 && deltaY == 2))
                return false;
            if (fromX == 4 && toX > 4) return false;//khong duoc sang song
            if (fromX == 5 && toX < 5) return false;

            //kiem tra chan
            if (toX > fromX)
            {
                if (toY > fromY)
                {
                    if (board.GetCell(fromX + 1, fromY + 1).PieceType != ChessPieceType.None) return false;
                }
                else
                {
                    if (board.GetCell(fromX + 1, fromY - 1).PieceType != ChessPieceType.None) return false;
                }
            }
            else
            {
                if (toY > fromY)
                {
                    if (board.GetCell(fromX - 1, fromY + 1).PieceType != ChessPieceType.None) return false;
                }
                else
                {
                    if (board.GetCell(fromX - 1, fromY - 1).PieceType != ChessPieceType.None) return false;
                }
            }
            //kiem tra dich
            if (targetCell.PieceType != ChessPieceType.None)
            {
                if (targetCell.IsRed != sourceCell.IsRed) return true;
                else return false;
            }
            return true;
        }
        //Logic di chuyển cho Sĩ
        private bool ValidateSiMove(int fromX, int fromY, int toX, int toY, Board board)
        {
            var sourceCell = board.GetCell(fromX, fromY);
            var targetCell = board.GetCell(toX, toY);
            int deltaX = Math.Abs(fromX - toX);
            int deltaY = Math.Abs(fromY - toY);
            if (!(toY <= 5 && toY >= 3 && deltaX == 1 && deltaY == 1) && (toX <= 2 || toX >= 7))
                return false;

            if (targetCell.PieceType != ChessPieceType.None)
            {
                if (targetCell.IsRed != sourceCell.IsRed) return true;
                else return false;
            }
            return true;
        }
        //Logic di chuyển của tướng
        private bool ValidateTuongMove(int fromX, int fromY, int toX, int toY, Board board)
        {
            var sourceCell = board.GetCell(fromX, fromY);
            var targetCell = board.GetCell(toX, toY);
            int deltaX = Math.Abs(fromX - toX);
            int deltaY = Math.Abs(fromY - toY);
            if (!(toY <= 5 && toY >= 3 && (toX <= 2 || toX >= 7) && ((deltaX == 1 && deltaY == 0) || (deltaX == 0 && deltaY == 1))))
                return false;
            if (targetCell.PieceType != ChessPieceType.None)
            {
                if (targetCell.IsRed != sourceCell.IsRed) return true;
                else return false;
            }
            return true;
        }
        //Logic di chuyển của Tốt

        private bool ValidateTotMove(int fromX, int fromY, int toX, int toY, Board board)
        {
            var sourceCell = board.GetCell(fromX, fromY);
            var targetCell = board.GetCell(toX, toY);
            int deltaXDo = toX - fromX;
            int deltaXDen = fromX - toX;
            //int deltaX = Math.Abs(fromX - toX);//sai o day
            int deltaY = Math.Abs(fromY - toY);
            if (sourceCell.IsRed)
            {
                if (toX > 4)
                {
                    if (!(deltaXDo == 1 && deltaY == 0) || (deltaXDo == 0 && deltaY == 1)) return false;
                }
                else
                {
                    if (!(deltaXDo == 1 && deltaY == 0)) return false;
                }
            }
            else
            {
                if (toX < 5)//xet ban co huong con lai
                {
                    if (!((deltaXDen == 1 && deltaY == 0) || (deltaXDen == 0 && deltaY == 1))) return false;
                }
                else
                {
                    if (!(deltaXDen == 1 && deltaY == 0)) return false;
                }
            }
            if (targetCell.PieceType != ChessPieceType.None)
            {
                if (targetCell.IsRed != sourceCell.IsRed) return true;
                else return false;
            }
            return true;
        }
    }
}
