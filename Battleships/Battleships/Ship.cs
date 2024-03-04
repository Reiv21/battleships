using System;

namespace Battleships
{
    internal class Ship
    {
        public enum ShipType
        {
            one = 1,
            two = 2,
            three = 3,
            four = 4
        };
        public ShipType type;
        public int[,] locationOfShipPart;
        public bool[] partIsHit;
        public int hits;
        // -2 - shoted
        public Ship(ShipType type)
        {
            this.type = type;
            locationOfShipPart = new int[(int)type, 2];
            partIsHit = new bool[(int)type];
        }

        public void Hit(int x, int y, int index, Board board, Board boardToShoot)
        {
            hits++;
            partIsHit[index] = true;
            if (IsSunk())
            {
                Console.WriteLine("Zatopiony!");
                for (int i = 0; i < locationOfShipPart.GetLength(0); i++)
                {
                    bool left = false, right = false, up = false, down = false;
                    if (locationOfShipPart[i, 1] + 1 < 10)
                    {
                        right = true;
                        if (board.board[locationOfShipPart[i, 1] + 1, locationOfShipPart[i, 0]] != 2)
                        {
                            board.board[locationOfShipPart[i, 1] + 1, locationOfShipPart[i, 0]] = 3;
                            boardToShoot.board[locationOfShipPart[i, 1] + 1, locationOfShipPart[i, 0]] = 3;
                        }
                    }
                    if (locationOfShipPart[i, 1] - 1 >= 0)
                    {
                        left = true;
                        if (board.board[locationOfShipPart[i, 1] - 1, locationOfShipPart[i, 0]] != 2)
                        {
                            board.board[locationOfShipPart[i, 1] - 1, locationOfShipPart[i, 0]] = 3;
                            boardToShoot.board[locationOfShipPart[i, 1] - 1, locationOfShipPart[i, 0]] = 3;

                        }


                    }
                    if (locationOfShipPart[i, 0] + 1 < 10)
                    {
                        down = true;
                        if (board.board[locationOfShipPart[i, 1], locationOfShipPart[i, 0] + 1] != 2)
                        {
                            board.board[locationOfShipPart[i, 1], locationOfShipPart[i, 0] + 1] = 3;
                            boardToShoot.board[locationOfShipPart[i, 1], locationOfShipPart[i, 0] + 1] = 3;

                        }

                    }
                    if (locationOfShipPart[i, 0] - 1 >= 0)
                    {
                        up = true;
                        if (board.board[locationOfShipPart[i, 1], locationOfShipPart[i, 0] - 1] != 2)
                        {
                            board.board[locationOfShipPart[i, 1], locationOfShipPart[i, 0] - 1] = 3;
                            boardToShoot.board[locationOfShipPart[i, 1], locationOfShipPart[i, 0] - 1] = 3;

                        }

                    }
                    if (up && right)
                    {
                        if (board.board[locationOfShipPart[i, 1] + 1, locationOfShipPart[i, 0] - 1] != 2)
                        {
                            board.board[locationOfShipPart[i, 1] + 1, locationOfShipPart[i, 0] - 1] = 3;
                            boardToShoot.board[locationOfShipPart[i, 1] + 1, locationOfShipPart[i, 0] - 1] = 3;
                        }

                    }
                    if (down && left)
                    {
                        if (board.board[locationOfShipPart[i, 1] - 1, locationOfShipPart[i, 0] + 1] != 2)
                        {
                            board.board[locationOfShipPart[i, 1] - 1, locationOfShipPart[i, 0] + 1] = 3;
                            boardToShoot.board[locationOfShipPart[i, 1] - 1, locationOfShipPart[i, 0] + 1] = 3;
                        }

                    }
                    if (up && left)
                    {
                        if (board.board[locationOfShipPart[i, 1] - 1, locationOfShipPart[i, 0] - 1] != 2)
                        {
                            board.board[locationOfShipPart[i, 1] - 1, locationOfShipPart[i, 0] - 1] = 3;
                            boardToShoot.board[locationOfShipPart[i, 1] - 1, locationOfShipPart[i, 0] - 1] = 3;
                        }

                    }
                    if (down && right)
                    {
                        if (board.board[locationOfShipPart[i, 1] + 1, locationOfShipPart[i, 0] + 1] != 2)
                        {
                            board.board[locationOfShipPart[i, 1] + 1, locationOfShipPart[i, 0] + 1] = 3;
                            boardToShoot.board[locationOfShipPart[i, 1] + 1, locationOfShipPart[i, 0] + 1] = 3;
                        }

                    }
                }
            }
        }
        public bool IsSunk()
        {
            return hits >= (int)type;
        }
    }
}
