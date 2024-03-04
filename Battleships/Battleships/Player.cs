using System;

namespace Battleships
{
    internal class Player
    {
        public string name;
        public Board board;
        public Board boardToShoot;
        public Player(bool isAi = false, string nameParameter = null, string otherPlayerName = null)
        {
            if (isAi)
            {
                name = "Komputer";
                board = new Board();
                boardToShoot = new Board();
                return;
            }

            if (nameParameter != null)
            {
                board = new Board();
                boardToShoot = new Board();
                this.name = nameParameter;
                return;
            }

            String input;
            while (true)
            {
                Console.WriteLine("Wprowadz swoja nazwe: ");
                input = Console.ReadLine();
                if (input.Length <= 0 || input.Length > 20)
                {
                    Console.WriteLine("Nieprawidlowa nazwa, wprowadz nazwe pomiedzy 0 a 20 znakow: ");
                    continue;
                }
                else if (otherPlayerName != null)
                {
                    if (otherPlayerName == input)
                    {
                        Console.WriteLine("Nazwa juz uzywana, sprobuj ponownie: ");
                        continue;
                    }
                    else break;
                }
                else break;
            }
            board = new Board();
            boardToShoot = new Board();
            name = input;
        }

        public void AutomaticAttack(Player enemy)
        {
            Random random = new Random();
            int[] pos = new int[2];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (boardToShoot.board[i, j] == 2)
                    {
                        if (i + 1 < 10 && boardToShoot.board[i + 1, j] == 0)
                        {
                            pos[1] = i + 1;
                            pos[0] = j;
                            if (enemy.board.CheckHit(pos, boardToShoot))
                            {
                                if (enemy.board.AllShipsSunk())
                                {
                                    return;
                                }
                                AutomaticAttack(enemy);
                                return;
                            }
                            else
                            {
                                boardToShoot.board[pos[1], pos[0]] = 2;
                                return;
                            }
                        }
                        if (i - 1 >= 0 && boardToShoot.board[i - 1, j] == 0)
                        {
                            pos[1] = i - 1;
                            pos[0] = j;
                            if (enemy.board.CheckHit(pos, boardToShoot))
                            {
                                if (enemy.board.AllShipsSunk())
                                {
                                    return;
                                }
                                AutomaticAttack(enemy);
                                return;
                            }
                            else
                            {
                                boardToShoot.board[pos[1], pos[0]] = 2;
                                return;
                            }
                        }
                        if (j + 1 < 10 && boardToShoot.board[i, j + 1] == 0)
                        {
                            pos[1] = i;
                            pos[0] = j + 1;
                            if (enemy.board.CheckHit(pos, boardToShoot))
                            {
                                if (enemy.board.AllShipsSunk())
                                {
                                    return;
                                }
                                AutomaticAttack(enemy);
                                return;
                            }
                            else
                            {
                                boardToShoot.board[pos[1], pos[0]] = 2;
                                return;
                            }
                        }
                        if (j - 1 >= 0 && boardToShoot.board[i, j - 1] == 0)
                        {
                            pos[1] = i;
                            pos[0] = j - 1;
                            if (enemy.board.CheckHit(pos, boardToShoot))
                            {
                                if (enemy.board.AllShipsSunk())
                                {
                                    return;
                                }
                                AutomaticAttack(enemy);
                                return;
                            }
                            else
                            {
                                boardToShoot.board[pos[1], pos[0]] = 2;
                                return;
                            }
                        }
                    }
                }
            }
            pos[1] = random.Next(10);
            pos[0] = random.Next(10);

            if (boardToShoot.board[pos[1], pos[0]] == 3 || boardToShoot.board[pos[1], pos[0]] == 2)
            {
                AutomaticAttack(enemy);
                return;
            }
            if (enemy.board.CheckHit(pos, boardToShoot))
            {
                if (enemy.board.AllShipsSunk())
                {
                    return;
                }

                AutomaticAttack(enemy);
            }
            else
            {
                boardToShoot.board[pos[1], pos[0]] = 2;
            }
            Console.WriteLine("Komputer strzela na pozycje: " + pos[0] + " " + pos[1]);
            Console.ReadLine();
        }
        public void Attack(Player enemy)
        {
            Console.WriteLine("Atak gracza: " + name);
            board.PrintBoardAttack(boardToShoot.board);
            enemy.board.PrintBoard();
            int[] pos = board.ValidateAndGetPlayerMove();
            if (boardToShoot.board[pos[1], pos[0]] == 3 || boardToShoot.board[pos[1], pos[0]] == 2)
            {
                Console.Clear();
                Console.WriteLine("Juz tam strzelales, sproboj ponownie: ");
                Attack(enemy);
                return;
            }
            Console.Clear();

            if (enemy.board.CheckHit(pos, boardToShoot))
            {
                if (enemy.board.AllShipsSunk())
                {
                    return;
                }
                Console.WriteLine("Trafiony! Masz dodatkowy strzal: ");
                Attack(enemy);
            }
            else
            {
                Console.WriteLine("Pudlo!...");
                Console.ReadLine();
                boardToShoot.board[pos[1], pos[0]] = 2;
            }
        }
    }
}
