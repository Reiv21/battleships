using System;

namespace Battleships
{
    internal class Board
    {
        public Board()
        {
            PrepareBoard();
        }

        public int[,] board = new int[10, 10];
        Ship[] ships = new Ship[10];

        void PrepareBoard()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    board[i, j] = 0;
                }
            }
        }

        Ship FindFreeShip(Ship.ShipType type)
        {
            for (int i = 0; i < 10; i++)
            {
                if (ships[i] == null)
                {
                    ships[i] = new Ship(type);
                    return ships[i];
                }
            }

            // no free slots = error
            return null;
        }

        public void SetupShips()
        {
            int shipAmount = 0;
            while (shipAmount < 4)
            {
                Console.Clear();
                SetupShip(Ship.ShipType.one, "jeden");
                shipAmount++;
            }

            shipAmount = 0;
            while (shipAmount < 3)
            {
                Console.Clear();
                SetupShip(Ship.ShipType.two, "dwa");
                shipAmount++;
            }

            shipAmount = 0;
            while (shipAmount < 2)
            {
                if (!CanPlaceObject(1, 3) || !CanPlaceObject(3, 1))
                {
                    Console.Clear();
                    Console.WriteLine("Nie mozna postawic statku o dlugosci 3, sprobuj ustawic wszystko jeszcze raz:");
                    PrepareBoard();
                    ships = new Ship[10];
                    SetupShips();
                    return;
                }
                Console.Clear();
                SetupShip(Ship.ShipType.three, "trzy");
                shipAmount++;
            }

            shipAmount = 0;
            while (shipAmount < 1)
            {
                if (!CanPlaceObject(1, 4) || !CanPlaceObject(4, 1))
                {
                    Console.Clear();
                    Console.WriteLine("Nie mozna postawic statku o dlugosci 4, sprobuj ustawic wszystko jeszcze raz:");
                    PrepareBoard();
                    ships = new Ship[10];
                    SetupShips();
                    return;
                }
                Console.Clear();
                SetupShip(Ship.ShipType.four, "cztery");
                shipAmount++;
            }

            PrintBoard();
        }

        public void AutomaticSetupShips()
        {
            Random random = new Random();

            // Tablica do przechowywania długości statków
            int[] shipLengths = { 4, 3, 3, 2, 2, 2 };
            int tries = 0;
            for (int z = 0; z < shipLengths.Length; z++)
            {
                bool placed = false;
                int length = shipLengths[z];
                tries++;
                if (tries > 50)
                {
                    /*for (int i = 0; i < 10; i++)
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            if(board[i, j] == 0)
                            {
                                bool horizontal = random.Next(2) == 0;
                                if (CanPlaceShip(i, j, length, horizontal))
                                {
                                    PlaceShip( i, j, length, horizontal, FindFreeShip((Ship.ShipType )length));
                                    placed = true;
                                }
                            }
                        }
                    }*/

                    board = new int[10, 10];
                    ships = new Ship[10];
                    AutomaticSetupShips();

                    return;
                }


                // Losowanie położenia statku
                int tries2 = 0;
                while (!placed)
                {
                    tries2++;
                    if (tries2 > 50)
                    {
                        board = new int[10, 10];
                        ships = new Ship[10];
                        AutomaticSetupShips();
                        return;
                    }
                    int row = random.Next(10);
                    int col = random.Next(10);
                    bool horizontal = random.Next(2) == 0;

                    if (CanPlaceShip(col, row, length, horizontal))
                    {
                        PlaceShip(col, row, length, horizontal, FindFreeShip((Ship.ShipType)length));
                        placed = true;
                    }
                }
            }
            int placed1x1 = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int x = 0; x < 10; x++)
                {
                    for (int y = 0; y < 10; y++)
                    {
                        if (board[x, y] == 0)
                        {
                            int[] playerMove = new int[2];

                            playerMove[0] = y;
                            playerMove[1] = x;

                            Ship ship = FindFreeShip(Ship.ShipType.one);
                            ship.locationOfShipPart[0, 0] = y;
                            ship.locationOfShipPart[0, 1] = x;

                            int left = 0, right = 0, up = 0, down = 0;
                            left = playerMove[0] != 0 ? 1 : 0;
                            right = playerMove[0] != 9 ? 1 : 0;
                            down = playerMove[1] != 0 ? 1 : 0;
                            up = playerMove[1] != 9 ? 1 : 0;

                            board[playerMove[1] + up, playerMove[0]] = 4;
                            board[playerMove[1] + up, playerMove[0] + right] = 4;
                            board[playerMove[1], playerMove[0] + right] = 4;
                            board[playerMove[1] - down, playerMove[0] + right] = 4;
                            board[playerMove[1] - down, playerMove[0]] = 4;
                            board[playerMove[1] - down, playerMove[0] - left] = 4;
                            board[playerMove[1], playerMove[0] - left] = 4;
                            board[playerMove[1] + up, playerMove[0] - left] = 4;

                            board[x, y] = 1;

                            placed1x1++;
                            x = 10;

                            break;
                        }
                    }
                }
            }

            if (placed1x1 != 4)
            {
                board = new int[10, 10];
                ships = new Ship[10];
                AutomaticSetupShips();
                return;
            }
        }

        void PlaceShip(int row, int col, int length, bool horizontal, Ship ship)
        {
            ship.locationOfShipPart = new int[length, 2];
            ship.partIsHit = new bool[length];
            for (int i = 0; i < length; i++)
            {
                if (horizontal)
                {
                    board[row, col + i] = 1;
                    ship.locationOfShipPart[i, 0] = col + i;
                    ship.locationOfShipPart[i, 1] = row;
                }
                else
                {
                    board[row + i, col] = 1;
                    ship.locationOfShipPart[i, 0] = col;
                    ship.locationOfShipPart[i, 1] = row + i;
                }

            }

            // Blokowanie pól wokół statku
            int startRow = Math.Max(0, row - 1);
            int endRow = Math.Min(10, row + (horizontal ? 1 : length) + 1);
            int startCol = Math.Max(0, col - 1);
            int endCol = Math.Min(10, col + (horizontal ? length : 1) + 1);

            for (int i = startRow; i < endRow; i++)
            {
                for (int j = startCol; j < endCol; j++)
                {
                    if (board[i, j] == 0)
                        board[i, j] = 4;
                }
            }
        }


        bool CanPlaceShip(int row, int col, int length, bool horizontal)
        {
            if (horizontal)
            {
                if (col + length > 10)
                {
                    return false;
                }

                for (int c = col; c < col + length; c++)
                {
                    if (board[row, c] != 0)
                    {
                        return false;

                    }
                }
            }
            else
            {
                if (row + length > 10)
                {
                    return false;
                }

                for (int r = row; r < row + length; r++)
                {
                    if (board[r, col] != 0)
                    {
                        return false;
                    }
                }
            }

            // Sprawdzenie, czy statki nie stykają się z innymi statkami
            for (int i = Math.Max(0, row - 1); i < Math.Min(10, row + (horizontal ? length : 1) + 1); i++)
            {
                for (int j = Math.Max(0, col - 1); j < Math.Min(10, col + (horizontal ? length : 1) + 1); j++)
                {
                    if (board[i, j] != 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }


        void SetupShip(Ship.ShipType type, string shipLenghtToPrint, Ship ship = null)
        {
            if (ship == null)
            {
                ship = FindFreeShip(type);
            }
            //board[gora/dol + move[1], lewo/prawo + move[0] ]
            PrintBoard();

            Console.WriteLine("Podaj pozycję statku o dlugosci " + shipLenghtToPrint);
            int[] playerMove = ValidateAndGetPlayerMove();
            int playerMovePositionValue = board[playerMove[1], playerMove[0]];

            if (playerMovePositionValue == 1 || playerMovePositionValue == 4)
            {
                Console.Clear();
                Console.WriteLine("Nie mozna postawic tu statku: ");
                SetupShip(type, shipLenghtToPrint, ship);
                return;
            }

            if (type == Ship.ShipType.one) SetupShortShip(playerMove, ship);

            else SetupLongShip(playerMove, type, shipLenghtToPrint, ship);

        }

        void SetupShortShip(int[] playerMove, Ship ship)
        {
            int left = 0, right = 0, up = 0, down = 0;
            left = playerMove[0] != 0 ? 1 : 0;
            right = playerMove[0] != 9 ? 1 : 0;
            down = playerMove[1] != 0 ? 1 : 0;
            up = playerMove[1] != 9 ? 1 : 0;

            board[playerMove[1] + up, playerMove[0]] = 4;
            board[playerMove[1] + up, playerMove[0] + right] = 4;
            board[playerMove[1], playerMove[0] + right] = 4;
            board[playerMove[1] - down, playerMove[0] + right] = 4;
            board[playerMove[1] - down, playerMove[0]] = 4;
            board[playerMove[1] - down, playerMove[0] - left] = 4;
            board[playerMove[1], playerMove[0] - left] = 4;
            board[playerMove[1] + up, playerMove[0] - left] = 4;

            board[playerMove[1], playerMove[0]] = 1;

            ship.locationOfShipPart[0, 0] = playerMove[0];
            ship.locationOfShipPart[0, 1] = playerMove[1];

            PrintBoard();
        }

        void FirstPlayerChoices(int[] firstPlayerMove, Ship ship)
        {
            int left = 0, right = 0, up = 0, down = 0;
            left = firstPlayerMove[0] != 0 ? 1 : 0;
            right = firstPlayerMove[0] != 9 ? 1 : 0;
            down = firstPlayerMove[1] != 9 ? 1 : 0;
            up = firstPlayerMove[1] != 0 ? 1 : 0;

            if (board[firstPlayerMove[1] - up, firstPlayerMove[0]] != 4)
                board[firstPlayerMove[1] - up, firstPlayerMove[0]] = 5;
            if (board[firstPlayerMove[1] + down, firstPlayerMove[0]] != 4)
                board[firstPlayerMove[1] + down, firstPlayerMove[0]] = 5;
            if (board[firstPlayerMove[1], firstPlayerMove[0] - left] != 4)
                board[firstPlayerMove[1], firstPlayerMove[0] - left] = 5;
            if (board[firstPlayerMove[1], firstPlayerMove[0] + right] != 4)
                board[firstPlayerMove[1], firstPlayerMove[0] + right] = 5;

            board[firstPlayerMove[1], firstPlayerMove[0]] = 1;

            ship.locationOfShipPart[0, 0] = firstPlayerMove[0];
            ship.locationOfShipPart[0, 1] = firstPlayerMove[1];
        }

        void SetupLongShip(int[] firstPlayerMove, Ship.ShipType type, String shipLenghtToPrint, Ship ship)
        {

            int[,] backupBoard = new int[10, 10];
            backupBoard = board.Clone() as int[,];

            FirstPlayerChoices(firstPlayerMove, ship);

            int lengthOfShip = (int)type;
            // cuz firt move is already done
            lengthOfShip--;

            Console.Clear();
            bool firstMove = true;

            while (lengthOfShip > 0)
            {
                int value = TryPlacingPartOfShip(firstMove, firstPlayerMove, ship,
                    lengthOfShip);
                if (value != 0)
                {
                    if (value == 1)
                    {
                        Console.Clear();
                        Console.WriteLine("Nie mozna tu postawic statku, sprobuj jeszcze raz:");
                        board = backupBoard.Clone() as int[,];
                        PrintBoard();
                        Console.ReadLine();
                        SetupShip(type, shipLenghtToPrint);
                        return;
                    }

                    Console.Clear();
                    Console.WriteLine("Niestety twoj statek sie nie miesci, ustaw go jeszcze raz:");
                    board = backupBoard.Clone() as int[,];
                    PrintBoard();
                    Console.ReadLine();
                    SetupShip(type, shipLenghtToPrint);
                    return;
                }

                lengthOfShip--;
                firstMove = false;
            }

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (board[i, j] == 5) board[i, j] = 4;
                }
            }

        }

        int TryPlacingPartOfShip(bool firstMove, int[] firstPlayerMove, Ship ship, int index)
        {
            bool canPlayerPlaceShip = false;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (board[i, j] == 5) canPlayerPlaceShip = true;
                }
            }

            if (!canPlayerPlaceShip)
            {
                return 1;
            }
            // 0 = ok -1 = restart 1 = start again
            PrintBoard(true);
            Console.WriteLine("Podaj pozycję statku: ");
            int[] newPlayerMove = ValidateAndGetPlayerMove(true);

            if (board[newPlayerMove[1], newPlayerMove[0]] != 5)
            {
                Console.Clear();
                Console.WriteLine("Nie mozna postawic tu statku:");
                return 1;
            }

            if (firstMove)
            {
                TryChangeBoardPositionTo4(firstPlayerMove[1] + 1, firstPlayerMove[0] - 1);
                TryChangeBoardPositionTo4(firstPlayerMove[1] + 1, firstPlayerMove[0] + 1);
                TryChangeBoardPositionTo4(firstPlayerMove[1] - 1, firstPlayerMove[0] - 1);
                TryChangeBoardPositionTo4(firstPlayerMove[1] - 1, firstPlayerMove[0] + 1);

            }

            int xDiffPlayerMove = firstPlayerMove[0] - newPlayerMove[0];

            if (xDiffPlayerMove != 0)
            {
                if (firstMove)
                {
                    if (firstPlayerMove[0] != 9)
                    {
                        if (board[firstPlayerMove[1], firstPlayerMove[0] + 1] != 4) board[firstPlayerMove[1], firstPlayerMove[0] + 1] = 5;

                    }

                    if (firstPlayerMove[0] != 0)
                    {
                        if (board[firstPlayerMove[1], firstPlayerMove[0] - 1] != 4) board[firstPlayerMove[1], firstPlayerMove[0] - 1] = 5;

                    }
                }

                // tu zmienilem
                TryChangeBoardPositionTo4(newPlayerMove[1] - 1, newPlayerMove[0]);
                TryChangeBoardPositionTo4(newPlayerMove[1] + 1, newPlayerMove[0]);

                TryChangeBoardPositionTo4(newPlayerMove[1] - 1, newPlayerMove[0] - 1);
                TryChangeBoardPositionTo4(newPlayerMove[1] + 1, newPlayerMove[0] + 1);

                TryChangeBoardPositionTo4(newPlayerMove[1] - 1, newPlayerMove[0] + 1);
                TryChangeBoardPositionTo4(newPlayerMove[1] + 1, newPlayerMove[0] - 1);

                board[newPlayerMove[1], newPlayerMove[0]] = 1;

                ship.locationOfShipPart[index, 0] = newPlayerMove[0];
                ship.locationOfShipPart[index, 1] = newPlayerMove[1];

                if (newPlayerMove[0] + 1 < 10)
                {
                    if (board[newPlayerMove[1], newPlayerMove[0] + 1] != 1 &&
                        board[newPlayerMove[1], newPlayerMove[0] + 1] != 4)
                    {
                        board[newPlayerMove[1], newPlayerMove[0] + 1] = 5;
                    }
                    else if (newPlayerMove[0] - 1 >= 0)
                    {
                        if (board[newPlayerMove[1], newPlayerMove[0] - 1] != 1 &&
                            board[newPlayerMove[1], newPlayerMove[0] - 1] != 4)
                        {
                            board[newPlayerMove[1], newPlayerMove[0] - 1] = 5;
                        }
                    }
                }

                canPlayerPlaceShip = false;
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (board[i, j] == 5) canPlayerPlaceShip = true;
                    }
                }

                if (!canPlayerPlaceShip)
                {
                    return -1;
                }


            }
            else
            {
                if (firstMove)
                {
                    if (firstPlayerMove[1] != 9)
                        if (board[firstPlayerMove[1] + 1, firstPlayerMove[0]] != 4) board[firstPlayerMove[1] + 1, firstPlayerMove[0]] = 5;

                    if (firstPlayerMove[1] != 0)
                        if (board[firstPlayerMove[1] - 1, firstPlayerMove[0]] != 4) board[firstPlayerMove[1] - 1, firstPlayerMove[0]] = 5;

                }

                TryChangeBoardPositionTo4(newPlayerMove[1], newPlayerMove[0] + 1);
                TryChangeBoardPositionTo4(newPlayerMove[1], newPlayerMove[0] - 1);

                TryChangeBoardPositionTo4(newPlayerMove[1] - 1, newPlayerMove[0] - 1);
                TryChangeBoardPositionTo4(newPlayerMove[1] + 1, newPlayerMove[0] + 1);

                TryChangeBoardPositionTo4(newPlayerMove[1] - 1, newPlayerMove[0] + 1);
                TryChangeBoardPositionTo4(newPlayerMove[1] + 1, newPlayerMove[0] - 1);

                board[newPlayerMove[1], newPlayerMove[0]] = 1;
                ship.locationOfShipPart[index, 0] = newPlayerMove[0];
                ship.locationOfShipPart[index, 1] = newPlayerMove[1];

                if (newPlayerMove[1] + 1 < 10)
                {
                    if (board[newPlayerMove[1] + 1, newPlayerMove[0]] != 1 &&
                        board[newPlayerMove[1] + 1, newPlayerMove[0]] != 4)
                    {
                        board[newPlayerMove[1] + 1, newPlayerMove[0]] = 5;
                    }
                    else if (newPlayerMove[1] - 1 >= 0)
                    {
                        if (board[newPlayerMove[1] - 1, newPlayerMove[0]] != 1 &&
                            board[newPlayerMove[1] - 1, newPlayerMove[0]] != 4)
                        {
                            board[newPlayerMove[1] - 1, newPlayerMove[0]] = 5;
                        }
                    }
                    else
                    {
                        canPlayerPlaceShip = false;
                        for (int i = 0; i < 10; i++)
                        {
                            for (int j = 0; j < 10; j++)
                            {
                                if (board[i, j] == 5) canPlayerPlaceShip = true;
                            }
                        }

                        if (!canPlayerPlaceShip)
                        {
                            return -1;
                        }
                    }
                }

            }

            return 0;
        }

        void TryChangeBoardPositionTo4(int x, int y)
        {
            if (x < 0 || x > 9 || y < 0 || y > 9) return;
            if (board[x, y] != 1)
            {
                board[x, y] = 4;
            }
        }
        bool CanPlaceObject(int width, int height)
        {
            for (int i = 0; i <= board.GetLength(0) - width; i++)
            {
                for (int j = 0; j <= board.GetLength(1) - height; j++)
                {
                    bool horizontalFit = true;
                    bool verticalFit = true;

                    // Sprawdzanie poziomego ustawienia
                    for (int k = 0; k < width; k++)
                    {
                        if (board[i + k, j] != 0)
                        {
                            horizontalFit = false;
                            break;
                        }
                    }

                    // Sprawdzanie pionowego ustawienia
                    for (int k = 0; k < height; k++)
                    {
                        if (board[i, j + k] != 0)
                        {
                            verticalFit = false;
                            break;
                        }
                    }

                    if (horizontalFit || verticalFit)
                        return true;
                }
            }

            return false;
        }
        public int[] ValidateAndGetPlayerMove(bool forPlacingShipsLongerThanOne = false, bool attack = false)
        {
            int[] result = new int[2];

            String input;
            input = Console.ReadLine();
            if (input == null)
            {
                Console.Clear();
                Console.WriteLine("Nieprawidlowy input, sprobuj ponownie");
                if (attack)
                {
                    PrintBoardAttack(board);
                }
                else
                {
                    PrintBoard(forPlacingShipsLongerThanOne);
                }
                return ValidateAndGetPlayerMove(forPlacingShipsLongerThanOne);
            }

            Console.WriteLine(input);

            int inputLetter;
            int inputNumber;

            if (input.Length == 0)
            {
                Console.Clear();
                if (attack)
                {
                    PrintBoardAttack(board);
                }
                else
                {
                    PrintBoard(forPlacingShipsLongerThanOne);
                }
                Console.WriteLine("Nieprawidlowy input, sprobuj ponownie");
                return ValidateAndGetPlayerMove(forPlacingShipsLongerThanOne);
            }

            inputLetter = input.ToLower()[0] - 'a';
            try
            {
                inputNumber = Int32.Parse(input.Substring(1, input.Length - 1)) - 1;
            }
            catch (Exception e)
            {
                Console.Clear();
                if (attack)
                {
                    PrintBoardAttack(board);
                }
                else
                {
                    PrintBoard(forPlacingShipsLongerThanOne);
                }
                Console.WriteLine("Input nie jest na planszy: ");

                return ValidateAndGetPlayerMove(forPlacingShipsLongerThanOne);
            }

            if ((inputLetter < 0 || inputLetter > 9) || (inputNumber < 0 || inputNumber > 9))
            {
                return ValidateAndGetPlayerMove(forPlacingShipsLongerThanOne);
            }

            result[0] = inputLetter;
            result[1] = inputNumber;

            return result;
        }

        public void PrintBoard(bool forPlacingShipsLongerThanOne = false)
        {
            char[] letters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };
            Console.WriteLine("     A   B   C   D   E   F   G   H   I   J  ");
            for (int i = 0; i < 10; i++)
            {
                Console.ForegroundColor = ConsoleColor.White;
                if (i != 9) Console.Write(i + 1 + "  |");
                else Console.Write(i + 1 + " |");
                for (int j = 0; j < 10; j++)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    if (i == 9)
                    {
                        Console.Write(CharFromBoardForPlayer(board[i, j], letters[j] + (i + 1 + ""),
                            forPlacingShipsLongerThanOne));
                    }
                    else
                    {
                        Console.Write(CharFromBoardForPlayer(board[i, j], letters[j] + (i + 1 + " "),
                            forPlacingShipsLongerThanOne));
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("|");
                }

                Console.WriteLine();
                if (i == 9) return;
                Console.WriteLine("   |---|---|---|---|---|---|---|---|---|---| ");
            }
        }

        public void PrintBoardAttack(int[,] boardToShoot)
        {
            char[] letters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };
            Console.WriteLine("     A   B   C   D   E   F   G   H   I   J  " + "       " + "     A   B   C   D   E   F   G   H   I   J  ");
            for (int i = 0; i < 10; i++)
            {
                if (i != 9) Console.Write(i + 1 + "  |");
                else Console.Write(i + 1 + " |");
                for (int j = 0; j < 10; j++)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    if (i == 9)
                    {
                        Console.Write(CharFromBoardForEnemy(boardToShoot[i, j], letters[j] + (i + 1 + "")));
                    }
                    else
                    {
                        Console.Write(CharFromBoardForEnemy(boardToShoot[i, j], letters[j] + (i + 1 + " ")));
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("|");
                    continue;
                }

                if (i != 9) Console.Write("       " + (i + 1) + "  |");
                else Console.Write("       " + (i + 1) + " |");
                for (int j = 0; j < 10; j++)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    if (i == 9)
                    {
                        Console.Write(CharFromBoardForPlayer(board[i, j], letters[j] + (i + 1 + ""), false));
                    }
                    else
                    {
                        Console.Write(CharFromBoardForPlayer(board[i, j], letters[j] + (i + 1 + " "), false));
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("|");
                    continue;
                }

                Console.WriteLine();
                if (i == 9) return;
                Console.WriteLine("   |---|---|---|---|---|---|---|---|---|---| " + "       " + "  |---|---|---|---|---|---|---|---|---|---| ");
            }
        }

        string CharFromBoardForPlayer(int value, string position, bool forPlacingShipsLongerThanOne)
        {
            Console.ForegroundColor = ConsoleColor.White;
            switch (value)
            {
                // 0 - blank, 1 - ship, 2 - shot ship, 3 - missed shot, 4 - blocked by other ship apperance near
                // 5 - can place another ship part
                case 0:
                    if (forPlacingShipsLongerThanOne) return "   ";
                    return position;
                case 1:
                    Console.ForegroundColor = ConsoleColor.Red;
                    return " ~ ";
                case 2:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    return " x ";
                case 3:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    return " b ";
                case 4:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    return " b ";
                case 5:
                    Console.ForegroundColor = ConsoleColor.Green;
                    return position;
            }
            return " ";
        }
        string CharFromBoardForEnemy(int value, string position)
        {
            Console.ForegroundColor = ConsoleColor.White;
            switch (value)
            {
                // 0 - blank, 1 - ship, 2 - shot ship, 3 - missed shot, 4 - blocked by other ship apperance near
                // 5 - can place another ship part
                case 0:
                    return position;
                case 1:
                    return position;
                case 2:
                    Console.ForegroundColor = ConsoleColor.Red;
                    return " x ";
                case 3:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    return " o ";
                case 4:
                    return position;
            }
            return "ERR0R";
        }

        public bool CheckHit(int[] pos, Board boardToShoot)
        {
            if (board[pos[1], pos[0]] == 1)
            {
                board[pos[1], pos[0]] = 2;
                boardToShoot.board[pos[1], pos[0]] = 2;
                foreach (var ship in ships)
                {
                    if (ship == null) continue;
                    for (int i = 0; i < (int)ship.type; i++)
                    {
                        if (ship.locationOfShipPart[i, 0] == pos[0] && ship.locationOfShipPart[i, 1] == pos[1] && !ship.partIsHit[i])
                        {
                            ship.Hit(pos[1], pos[0], i, this, boardToShoot);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool AllShipsSunk()
        {
            bool allSunk = true;
            Console.WriteLine("Statki zatopione: ");
            foreach (var ship in ships)
            {
                if (ship != null && !ship.IsSunk())
                {
                    allSunk = false;
                }
            }
            return allSunk;
        }
    }
}
