using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrazyAuri.Models.Pieces;
using CrazyAuriLibrary.Models.Moves;
using CrazyAuriLibrary.Models.Moves.MoveTypes;
using CrazyAuriLibrary.Models.Pieces;

namespace CrazyAuri.Models
{
    public class Board
    {
        public Piece[,] array = new Piece[8, 8];
        public List<Piece> WhitePieces = new List<Piece>();
        public List<Piece> BlackPieces = new List<Piece>();
        public Piece WhiteKing;
        public Piece BlackKing;

        public bool CurrentColor = false;
        public bool CanWhiteCastleQueenside = false;
        public bool CanWhiteCastleKingside = false;
        public bool CanBlackCastleQueenside = false;
        public bool CanBlackCastleKingside = false;
        public (int, int) EnPassantSquare = (-1, -1);
        public ushort HalfMoveClock = 0;
        public ushort FullMoveClock = 1;
        public Dictionary<string,ushort> FormerPositions = new Dictionary<string,ushort>();
        public List<string> movehistory = new List<string>();

        public ushort WhiteCrazyHousePawns = 0;
        public ushort WhiteCrazyHouseKnights = 0;
        public ushort WhiteCrazyHouseBishops = 0;
        public ushort WhiteCrazyHouseRooks = 0;
        public ushort WhiteCrazyHouseQueens = 0;

        public ushort BlackCrazyHousePawns = 0;
        public ushort BlackCrazyHouseKnights = 0;
        public ushort BlackCrazyHouseBishops = 0;
        public ushort BlackCrazyHouseRooks = 0;
        public ushort BlackCrazyHouseQueens = 0;

        public Move lastmovemade;

        private BoardMove boardmove;
        public Board()
        {
            InitialiseBoard("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR/ w KQkq - 0 1");
            AddNewPosition(GetPositionHash());

        }

        public Board(string FEN)
        {
            InitialiseBoard(FEN);
            AddNewPosition(GetPositionHash());
        }

        public Board(string FEN, Dictionary<string, ushort> FormerPositions)
        {
            InitialiseBoard(FEN);
            foreach (var i in FormerPositions)
            {
                this.FormerPositions.Add(i.Key, i.Value);
            }
        }

        public Board(Board board)
        {
            InitialiseBoard(board.ToString());
            foreach (var i in board.FormerPositions)
            {
                this.FormerPositions.Add(i.Key, i.Value);
            }
        }

        protected void InitialiseBoard(string FEN)
        {
            var separatedFEN = FEN.Split(" ");
            int currenttile = 0;
            bool BoardIsBeingRead = true;
            Piece newpiece = new Pawn(true, (-1, -1));
            for (int i = 0; i < FEN.Length; i++)
            {
                if (BoardIsBeingRead == true)
                {
                    int x = (int)Math.Floor((decimal)currenttile / 8);
                    int y = currenttile % 8;
                    if (x < 8)
                    {
                        switch (FEN[i])
                        {
                            case 'p':
                                newpiece = new Pawn(true, (x, y));
                                array[x, y] = newpiece;
                                BlackPieces.Add(newpiece);
                                currenttile += 1;
                                break;
                            case 'P':
                                newpiece = new Pawn(false, (x, y));
                                array[x, y] = newpiece;
                                WhitePieces.Add(newpiece);
                                currenttile += 1;
                                break;
                            case 'r':
                                newpiece = new Rook(true, (x, y));
                                array[x, y] = newpiece;
                                BlackPieces.Add(newpiece);
                                currenttile += 1;
                                break;
                            case 'R':
                                newpiece = new Rook(false, (x, y));
                                array[x, y] = newpiece;
                                WhitePieces.Add(newpiece);
                                currenttile += 1;
                                break;
                            case 'b':
                                newpiece = new Bishop(true, (x, y));
                                array[x, y] = newpiece;
                                BlackPieces.Add(newpiece);
                                currenttile += 1;
                                break;
                            case 'B':
                                newpiece = new Bishop(false, (x, y));
                                array[x, y] = newpiece;
                                WhitePieces.Add(newpiece);
                                currenttile += 1;
                                break;
                            case 'n':
                                newpiece = new Knight(true, (x, y));
                                array[x, y] = newpiece;
                                BlackPieces.Add(newpiece);
                                currenttile += 1;
                                break;
                            case 'N':
                                newpiece = new Knight(false, (x, y));
                                array[x, y] = newpiece;
                                WhitePieces.Add(newpiece);
                                currenttile += 1;
                                break;
                            case 'q':
                                newpiece = new Queen(true, (x, y));
                                array[x, y] = newpiece;
                                BlackPieces.Add(newpiece);
                                currenttile += 1;
                                break;
                            case 'Q':
                                newpiece = new Queen(false, (x, y));
                                array[x, y] = newpiece;
                                WhitePieces.Add(newpiece);
                                currenttile += 1;
                                break;
                            case 'k':
                                newpiece = new King(true, (x, y));
                                array[x, y] = newpiece;
                                BlackPieces.Add(newpiece);
                                BlackKing = newpiece;
                                currenttile += 1;
                                break;
                            case 'K':
                                newpiece = new King(false, (x, y));
                                array[x, y] = newpiece;
                                WhitePieces.Add(newpiece);
                                WhiteKing = newpiece;
                                currenttile += 1;
                                break;
                            case '/':
                                break;
                            case ' ':
                                BoardIsBeingRead = false;
                                break;
                            default:
                                currenttile += int.Parse(FEN[i].ToString());
                                break;
                        }
                    }
                    else
                    {
                        switch (FEN[i])
                        {
                            case 'p':
                                BlackCrazyHousePawns += 1;
                                break;
                            case 'P':
                                WhiteCrazyHousePawns += 1;
                                break;
                            case 'r':
                                BlackCrazyHouseRooks += 1;
                                break;
                            case 'R':
                                WhiteCrazyHouseRooks += 1;
                                break;
                            case 'b':
                                BlackCrazyHouseBishops += 1;
                                break;
                            case 'B':
                                WhiteCrazyHouseBishops += 1;
                                break;
                            case 'n':
                                BlackCrazyHouseKnights += 1;
                                break;
                            case 'N':
                                WhiteCrazyHouseKnights += 1;
                                break;
                            case 'q':
                                BlackCrazyHouseQueens += 1;
                                break;
                            case 'Q':
                                WhiteCrazyHouseQueens += 1;
                                break;
                            case '/':
                                break;
                            case ' ':
                                BoardIsBeingRead = false;
                                break;
                        }
                    }
                }
                else
                {
                    break;
                }
            }

            if (separatedFEN[1] == "b")
            {
                CurrentColor = true;
            }

            if (separatedFEN[2].Contains("K"))
                CanWhiteCastleKingside = true;
            if (separatedFEN[2].Contains("Q"))
                CanWhiteCastleQueenside = true;
            if (separatedFEN[2].Contains("k"))
                CanBlackCastleKingside = true;
            if (separatedFEN[2].Contains("q"))
                CanBlackCastleQueenside = true;

            if (separatedFEN[3] != "-")
            {
                EnPassantSquare = ConvertTileNameToLocation(separatedFEN[3]);
            }

            HalfMoveClock = ushort.Parse(separatedFEN[4]);
            FullMoveClock = ushort.Parse(separatedFEN[5]);

            boardmove = new BoardMove(this);

        }

        public Board Copy()
        {
            return new Board(this);
        }

        public override string ToString()
        {
            return PrintFEN();
        }

        public void PrintBoard()
        {
            Console.WriteLine("+---+---+---+---+---+---+---+---+");
            for (int i = 0; i < 8; i++)
            {
                Console.Write("| ");
                for (int j = 0; j < 8; j++)
                {
                    Console.Write(array[i, j] != null ? array[i, j] : " ");
                    Console.Write(" | ");
                }
                Console.WriteLine("");
                Console.WriteLine("+---+---+---+---+---+---+---+---+");
            }
        }

        public string PrintFEN()
        {
            StringBuilder sb = new StringBuilder("", 120);

            for (int i = 0; i < 8; i++)
            {
                int currentspace = 0;
                for (int j = 0; j < 8; j++)
                {
                    Piece piece = GetPieceOnSquare((i, j));
                    if (piece != null)
                    {
                        if (currentspace != 0)
                            sb.Append(currentspace);
                        currentspace = 0;
                        sb.Append(piece.ToString());
                    }
                    else
                    {
                        currentspace += 1;
                    }
                }
                if (currentspace != 0)
                    sb.Append(currentspace);
                sb.Append("/");
            }

            FENCrazyHousePiecesInsert(sb, WhiteCrazyHousePawns, "P");
            FENCrazyHousePiecesInsert(sb, WhiteCrazyHouseKnights, "N");
            FENCrazyHousePiecesInsert(sb, WhiteCrazyHouseBishops, "B");
            FENCrazyHousePiecesInsert(sb, WhiteCrazyHouseRooks, "R");
            FENCrazyHousePiecesInsert(sb, WhiteCrazyHouseQueens, "Q");
            FENCrazyHousePiecesInsert(sb, BlackCrazyHousePawns, "p");
            FENCrazyHousePiecesInsert(sb, BlackCrazyHouseKnights, "n");
            FENCrazyHousePiecesInsert(sb, BlackCrazyHouseBishops, "b");
            FENCrazyHousePiecesInsert(sb, BlackCrazyHouseRooks, "r");
            FENCrazyHousePiecesInsert(sb, BlackCrazyHouseQueens, "q");

            sb.Append(" ");

            sb.Append(CurrentColor == true ? "b" : "w");
            sb.Append(" ");
            bool castlingExists = false;
            if (CanWhiteCastleKingside == true)
            {
                castlingExists = true;
                sb.Append("K");
            }
            if (CanWhiteCastleQueenside == true)
            {
                castlingExists = true;
                sb.Append("Q");
            }
            if (CanBlackCastleKingside == true)
            {
                castlingExists = true;
                sb.Append("k");
            }
            if (CanBlackCastleQueenside == true)
            {
                castlingExists = true;
                sb.Append("q");
            }
            sb.Append(castlingExists == true ? " " : "- ");
            sb.Append(EnPassantSquare!=(-1,-1) ? ConvertLocationToTileName(EnPassantSquare) : "-");
            sb.Append(" ");
            sb.Append(HalfMoveClock);
            sb.Append(" ");
            sb.Append(FullMoveClock);

            return sb.ToString();
        }

        private void FENCrazyHousePiecesInsert(StringBuilder sb, ushort piececount, string piece)
        {
            for (int i = 0; i < piececount; i++)
                sb.Append(piece);
        }

        public Piece GetPieceOnSquare((int, int) location)
        {
            int x = location.Item1;
            int y = location.Item2;
            if (array[x, y] != null)
            {
                    return array[x, y];
            }
            return null;
        }

        public List<Move> GetAllMoves()
        {
            return boardmove.GetAllMoves();
        }

        public List<Move> GetAllPieceMoves((int,int) location)
        {
            return boardmove.GetAllPieceMoves(location);
        }

        public bool MakeMove(Move move)
        {
            if (boardmove.MakeMove(move) == true)
            {
                boardmove = new BoardMove(this);
                AddNewPosition(GetPositionHash());
                return true;
            }
            return false;
        }

        public bool MakeMove(string move)
        {
            if (boardmove.MakeMove(move) == true)
            {
                boardmove = new BoardMove(this);
                AddNewPosition(GetPositionHash());
                return true;
            }
            return false;
        }

        private void AddNewPosition(string position)
        {
            if (FormerPositions.ContainsKey(position))
            {
                FormerPositions[position] += 1;
            }
            else
            {
                FormerPositions.Add(position, 1);
            }
        }

        public string GetWinner()
        {
            return boardmove.GetWinner();
        }

        public (int,int) ConvertTileNameToLocation(string tile)
        {
            return (-1*(tile[1] - 56), tile[0]-97);
        }

        public string ConvertLocationToTileName((int, int) location)
        {
            var x = location.Item1;
            var y = location.Item2;

            return char.ConvertFromUtf32(y + 97) + (8 - x).ToString();
        }

        public string GetPositionHash()
        {
            return PrintFEN().Split(" ")[0];
        }

    }
}
