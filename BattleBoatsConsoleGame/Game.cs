using System;

class Program
    {
        public static int parseCheck(string input)
        {
            Boolean success = false;
            Boolean parsed = false;
            int result = -1;

            while (!success)
            {
                parsed = Int32.TryParse(input, out result);
                if (parsed)
                {
                    success = true;
                }
                else
                {
                    Console.Write("Invalid entry. Please enter an integer: ");
                    input = Console.ReadLine();
                }
            }
            return result;
        }

        public static void Main(string[] args)
        {
            Boolean debug;
            string debugchoice;
            int cols;
            int rows;
            Boolean turn;
            string playerchoice;
            int row;
            int col;
            Boolean validdirection;
            string directionchoice;
            int direction;
            Boolean validscan;
            int scanchoice;
            string quitchoice;
            string again;
            Boolean repeat = true;

            while (repeat)
            {
                Console.WriteLine("BATTLEBOATS - Written by Vinayak Rajesh");
                Console.WriteLine("\nGameplay:\n    type 'fire' or press any key to fire\n    type 'missile' to launch a missile\n    type 'drone' to scan a row or column\n    type 'quit' at any time to end the game\n");
                Console.WriteLine("- Selecting an invalid target will skip a turn as a penalty.\n- Try to finish in as few turns as possible.\n- The game will end when all boats have been sunk.\n");
                debug = false;
                Console.Write("Would you like to enable Debug Mode? (y/n): ");
                debugchoice = Console.ReadLine();
                if (debugchoice == "y" || debugchoice == "Y")
                {
                    Console.WriteLine("Debug Mode Enabled.");
                    debug = true;
                }
                else
                {
                    Console.WriteLine($"Regular Gameplay Mode Selected.");
                }

                Console.WriteLine(); // Debug Selection -> Gameboard Setup

                Console.WriteLine("Gameboard Setup:");
                Console.Write("Enter a desired number of columns, between 3 and 10: ");
                cols = parseCheck(Console.ReadLine());

                while ((cols < 3) || (cols > 10))
                {
                    Console.Write("Invalid number of columns specified. Please enter an integer between 3 and 10: ");
                    cols = parseCheck(Console.ReadLine());
                }
                Console.Write("Enter a desired number of rows, between 3 and 10: ");
                rows = parseCheck(Console.ReadLine());
                while ((rows < 3) || (rows > 10))
                {
                    Console.Write("Invalid number of rows specified. Please enter an integer between 3 and 10: ");
                    rows = parseCheck(Console.ReadLine());
                }
                Board gameplay = new Board(cols, rows);
                gameplay.placeBoats();

                Console.WriteLine(); // Gameboard Setup -> Gameplay            

                if (debug)
                {
                    Console.Write("Specify number of missiles: ");
                    gameplay.setMissiles(parseCheck(Console.ReadLine()));
                    Console.Write("Specify number of drones: ");
                    gameplay.setDrones(parseCheck(Console.ReadLine()));
                }

                while (gameplay.getRemainingShips() != 0)
                {
                    Console.WriteLine();
                    turn = true;
                    gameplay.addTurn();
                    Console.WriteLine($"TURN {gameplay.getTurns()}");
                    if (debug)
                    {
                        gameplay.print();
                    }
                    else
                    {
                        gameplay.display();
                    }
                    Console.WriteLine($"Ships Remaining: {gameplay.getRemainingShips()}");
                    Console.WriteLine($"Missiles Available: {gameplay.getMissiles()}");
                    Console.WriteLine($"Drones Available: {gameplay.getDrones()}");
                    Console.WriteLine($"Shots taken: {gameplay.getShots()}");
                    Console.WriteLine();

                    while (turn)
                    {
                        validdirection = false;
                        validscan = false;
                        Console.Write("What would you like to do? (fire/missile/drone/quit): ");
                        playerchoice = Console.ReadLine();

                        if ((playerchoice == "missile") || (playerchoice == "MISSILE") || (playerchoice == "m") || (playerchoice == "M"))
                        {
                            Console.WriteLine("Missile Selected.");
                            Console.Write("Enter a target column: ");
                            col = parseCheck(Console.ReadLine());
                            Console.Write("Enter a target row: ");
                            row = parseCheck(Console.ReadLine());

                            if (gameplay.getMissiles() > 0)
                            {
                                gameplay.missile(col, row);
                            }
                            else
                            {
                                Console.WriteLine("No missiles remaining! Firing on coordinates instead.");
                                gameplay.fire(col, row);
                            }
                            turn = false;
                        }
                        else if ((playerchoice == "drone") || (playerchoice == "DRONE") || (playerchoice == "d") || (playerchoice == "D"))
                        {
                            Console.WriteLine("Drone Selected.");
                            if (gameplay.getDrones() > 0)
                            {
                                direction = -1;
                                while (!validdirection)
                                {
                                    Console.Write("Enter drone instruction to scan a row or column (c/r): ");
                                    directionchoice = Console.ReadLine();
                                    if ((directionchoice == "r") || (directionchoice == "R"))
                                    {
                                        direction = 0;
                                        validdirection = true;
                                    }
                                    else if ((directionchoice == "c") || (directionchoice == "C"))
                                    {
                                        direction = 1;
                                        validdirection = true;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid scan direction specified.");
                                    }
                                }
                                while (!validscan)
                                {
                                    Console.Write("Enter column/row number to be scanned: ");
                                    scanchoice = parseCheck(Console.ReadLine());
                                    if ((direction == 0) && (scanchoice >= 0) && (scanchoice < gameplay.getRowLength()) || ((direction == 1) && (scanchoice >= 0) && (scanchoice < gameplay.getRowLength())))
                                    {
                                        gameplay.drone(direction, scanchoice);
                                        validscan = true;
                                    }
                                    else if (direction == 0)
                                    {
                                        Console.WriteLine($"Invalid input. Please type a number between 0 and {gameplay.getColumnLength() - 1}.");
                                    }
                                    else if (direction == 1)
                                    {
                                        Console.WriteLine($"Invalid input. Please type a number between 0 and {gameplay.getRowLength() - 1}.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid input. Please type a number within the boundaries of the board.");
                                    }
                                }
                                turn = false;
                            }
                            else
                            {
                                Console.WriteLine("No drones remaining! Please select another option.");
                            }
                        }
                        else if ((playerchoice == "quit") || (playerchoice == "QUIT") || (playerchoice == "q") || (playerchoice == "Q"))
                        {
                            Console.WriteLine("Are you sure you would like to quit? (y/n)");
                            quitchoice = Console.ReadLine();
                            if (quitchoice == "y" || quitchoice == "Y")
                            {
                                System.Environment.Exit(1);
                            }
                            else
                            {
                                Console.WriteLine("Quit Cancelled");
                                Console.WriteLine();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Basic Shot Selected.");
                            Console.Write("Enter a target column: ");
                            col = parseCheck(Console.ReadLine());
                            Console.Write("Enter a target row: ");
                            row = parseCheck(Console.ReadLine());
                            gameplay.fire(col, row);
                            turn = false;
                        }
                    }
                }

                Console.WriteLine(); // Gameplay -> Stats

                Console.WriteLine($"Congratulations! You have sunk {gameplay.totalNumBoats()} boat(s).");
                if (debug)
                {
                    gameplay.print();
                }
                else
                {
                    gameplay.display();
                }
                Console.WriteLine($"Total Turns: {gameplay.getTurns()}\nTotal Shots Fired: {gameplay.getShots()}\n");
                Console.Write("Enter 'y' to play again: ");
                again = Console.ReadLine();
                if ((again != "y") && (again != "Y"))
                {
                    repeat = false;
                }
                Console.WriteLine();
                Console.WriteLine("------------------------------------------------------------");
                Console.WriteLine();
            }
        }
    }