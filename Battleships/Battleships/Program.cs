using System;

namespace Battleships
{
    internal class Program
    {
        public static string name1, name2;
        public static bool PlayAgain()
        {
            Console.WriteLine("Czy chcesz zagrac ponownie? (t/n)");
            string input = Console.ReadLine().ToLower();
            if (input == "t")
            {
                return true;
            }
            else if (input == "n")
            {
                return false;
            }
            else
            {
                Console.WriteLine("Nieprawidlowa odpowiedz, sprobuj ponownie: ");
                return PlayAgain();
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Witaj w statkach!\n Wybierz opcje: \n  1. Gracz vs Gracz \n   2. Gracz vs Komputer \n    3. Wyjscie");
            string input = Console.ReadLine();
            if (input == "1")
            {
                PlayerVsPlayer();
            }
            else if (input == "2")
            {
                PlayerVsComputer();
            }
            else if (input == "3")
            {
                return;
            }
            else
            {
                Console.WriteLine("Nieprawidlowa odpowiedz, sprobuj ponownie: ");
                Main(args);
            }

        }

        static void PlayerVsComputer()
        {
            int player1Wins = 0;
            int player2Wins = 0;

            while (true)
            {
                Player ai, player;
                if (name1 != null)
                {
                    player = new Player(false, name1);
                }
                else
                {
                    player = new Player(false);
                    name1 = player.name;
                }
                if (name2 != null)
                {
                    ai = new Player(true, name2);

                }
                else
                {
                    ai = new Player(true);
                    name2 = ai.name;
                }

                ai.board.AutomaticSetupShips();
                Console.WriteLine("Kliknij aby zaczac runde dla gracza: " + player.name);
                Console.ReadLine();
                Console.Clear();








                player.board.SetupShips();










                while (true)
                {
                    Console.WriteLine("Kliknij aby zaczac runde ataku dla gracza: " + player.name);
                    Console.ReadLine();
                    Console.Clear();
                    Console.Clear();
                    player.Attack(ai);
                    if (ai.board.AllShipsSunk())
                    {
                        player1Wins++;
                        Console.WriteLine(player.name + " wygral!\n Gracz " + player.name + " ma wygranych: " + player1Wins + " razy\n Komputer ma wygranych: " + player2Wins + " razy");

                        if (PlayAgain())
                        {
                            break;
                        }

                        return;
                    }

                    Console.WriteLine("Kliknij aby zaczac runde ataku dla komputera: ");
                    Console.ReadLine();
                    Console.Clear();
                    ai.AutomaticAttack(player);
                    if (player.board.AllShipsSunk())
                    {
                        player2Wins++;

                        Console.WriteLine(" Komputer wygral!\n Gracz " + player.name + " ma wygranych: " + player1Wins + " razy\n Komputer ma wygranych: " + player2Wins + " razy");
                        if (PlayAgain())
                        {
                            break;
                        }

                        return;
                    }
                }
            }
        }
        static void PlayerVsPlayer()
        {
            int player1Wins = 0;
            int player2Wins = 0;


            while (true)
            {
                Player player1, player2;

                if (name1 != null)
                {
                    player1 = new Player(false, name1);
                }
                else
                {
                    Console.WriteLine("Gracz nr1:");
                    player1 = new Player(false);
                    name1 = player1.name;
                }
                if (name2 != null)
                {
                    player2 = new Player(false, name2);
                }
                else
                {
                    Console.WriteLine("Gracz nr2:");
                    player2 = new Player(false, null, name1);
                    name2 = player2.name;

                }

                Console.WriteLine("Kliknij aby zaczac runde dla gracza: " + player1.name);
                Console.ReadLine();
                Console.Clear();
                player1.board.SetupShips();
                Console.WriteLine("Kliknij aby zaczac runde dla gracza: " + player2.name);
                Console.ReadLine();
                Console.Clear();
                player2.board.SetupShips();
                while (true)
                {
                    Console.WriteLine("Kliknij aby zaczac runde ataku dla gracza: " + player1.name);
                    Console.ReadLine();
                    Console.Clear();
                    Console.Clear();
                    player1.Attack(player2);
                    if (player2.board.AllShipsSunk())
                    {
                        player1Wins++;

                        Console.WriteLine(player1.name + " wygral!\n Gracz nr1 ma wygranych: " + player1Wins + " razy\n Gracz nr2 ma wygranych: " + player2Wins + " razy");
                        if (PlayAgain())
                        {
                            break;
                        }

                        return;
                    }

                    Console.WriteLine("Kliknij aby zaczac runde ataku dla gracza: " + player2.name);
                    Console.ReadLine();
                    Console.Clear();
                    player2.Attack(player1);
                    if (player1.board.AllShipsSunk())
                    {
                        player2Wins++;

                        Console.WriteLine(player2.name + " wygral!\n Gracz nr1 ma wygranych: " + player1Wins + " razy\n Gracz nr2 ma wygranych: " + player2Wins + " razy");

                        if (PlayAgain())
                        {
                            break;
                        }

                        return;
                    }
                }

                Console.ReadLine();
            }
        }
    }
}

