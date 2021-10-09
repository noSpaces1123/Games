using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PlantFarm
{
    class Program
    {
        enum Tile { RedPlant, BluePlant, GreenPlant, YellowPlant, WhitePlant, None }
        static int score = 0;
        static int money = 0;
        static int[,] neighbours = new int[4, 2] { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };
        static void Main()
        {
            PlantFarm();
        }
        static void PlantFarm()
        {
            bool running = true;
            Dictionary<string, int> plant = new Dictionary<string, int>(3);

            plant.Add("red", 4);
            plant.Add("green", 1);
            plant.Add("blue", 1);
            plant.Add("yellow", 1);
            plant.Add("white", 1);


            Dictionary<string, int> energy = new Dictionary<string, int>();
            Tile[,] farm = new Tile[10, 10];
            for (int y = 0; y < farm.GetLength(1); y++)
            {
                for (int x = 0; x < farm.GetLength(0); x++)
                {
                    farm[x, y] = Tile.None;
                }
            }

            while (running)
            {
                Console.CursorVisible = false;
                Console.SetCursorPosition(0, 0);
                Draw(farm);
                if (Console.KeyAvailable)
                {
                    Console.Clear();
                    string[] incoords;
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.Q: // Shop
                            Console.WriteLine();
                            Console.WriteLine("Red (50) - 1\n" +
                                "Green (80) - 2\n" +
                                "Blue (60) - 3\n" +
                                "Yellow (90) - 4\n" +
                                "White (60) - 5\n" +
                                "Back (left arrow)");
                            switch (Console.ReadKey(true).Key)
                            {
                                case ConsoleKey.LeftArrow:
                                    break;
                                case ConsoleKey.D1:
                                    plant["red"]++;
                                    money -= 2;
                                    break;
                                case ConsoleKey.D2:
                                    plant["green"]++;
                                    money -= 5;
                                    break;
                                case ConsoleKey.D3:
                                    plant["blue"]++;
                                    money -= 3;
                                    break;
                                case ConsoleKey.D4:
                                    plant["yellow"]++;
                                    money -= 10;
                                    break;
                                case ConsoleKey.D5:
                                    plant["white"]++;
                                    money -= 6;
                                    break;
                            }
                            break;
                        case ConsoleKey.D1: // Red
                            Console.CursorVisible = true;
                            incoords = Console.ReadLine().Split(' ');
                            farm[int.Parse(incoords[0]), int.Parse(incoords[1])] = Tile.RedPlant;
                            if (!energy.ContainsKey(incoords[0] + "," + incoords[1]))
                            {
                                energy.Add(incoords[0] + "," + incoords[1], 4);
                            }
                            break;
                        case ConsoleKey.D2: // Blue
                            Console.CursorVisible = true;
                            incoords = Console.ReadLine().Split(' ');
                            farm[int.Parse(incoords[0]), int.Parse(incoords[1])] = Tile.BluePlant;
                            if (!energy.ContainsKey(incoords[0] + "," + incoords[1]))
                            {
                                energy.Add(incoords[0] + "," + incoords[1], 2);
                            }
                            break;
                        case ConsoleKey.D3: // Green
                            Console.CursorVisible = true;
                            incoords = Console.ReadLine().Split(' ');
                            farm[int.Parse(incoords[0]), int.Parse(incoords[1])] = Tile.GreenPlant;
                            if (!energy.ContainsKey(incoords[0] + "," + incoords[1]))
                            {
                                energy.Add(incoords[0] + "," + incoords[1], 6);
                            }
                            break;
                        case ConsoleKey.D4: // Yellow
                            Console.CursorVisible = true;
                            incoords = Console.ReadLine().Split(' ');
                            farm[int.Parse(incoords[0]), int.Parse(incoords[1])] = Tile.YellowPlant;
                            if (!energy.ContainsKey(incoords[0] + "," + incoords[1]))
                            {
                                energy.Add(incoords[0] + "," + incoords[1], 1);
                            }
                            break;
                        case ConsoleKey.D5: // White
                            Console.CursorVisible = true;
                            incoords = Console.ReadLine().Split(' ');
                            farm[int.Parse(incoords[0]), int.Parse(incoords[1])] = Tile.WhitePlant;
                            if (!energy.ContainsKey(incoords[0] + "," + incoords[1]))
                            {
                                energy.Add(incoords[0] + "," + incoords[1], 2);
                            }
                            break;
                        default:
                            break;
                    }
                }

                /*
                 * Red: 4-5 energy, +4 score, +1 money
                 * Green: 6-8 energy, +8 score, +3 money, destroys 1 energy from four plants around it
                 * Yellow: 1-5 energy, +2 score, +2 money, gives 1 energy to four plants around it
                 * Blue: 2-12 energy, +2 score, +1 money
                 * White: 2-6 energy, +3 score, +1 money, steals 1 energy from four plants around it
                */

                int getscore = 0;
                for (int y = 0; y < farm.GetLength(1); y++)
                {
                    for (int x = 0; x < farm.GetLength(0); x++)
                    {
                        switch (farm[x, y])
                        {
                            case Tile.RedPlant:
                                money++;
                                if (energy[x.ToString() + "," + y.ToString()] >= 4 && energy[x.ToString() + "," + y.ToString()] <= 5)
                                {
                                    getscore += 4;
                                }
                                else
                                {
                                    farm[x, y] = Tile.None;
                                }
                                break;
                            case Tile.GreenPlant:
                                money += 3;
                                if (energy[x.ToString() + "," + y.ToString()] >= 6 && energy[x.ToString() + "," + y.ToString()] <= 8)
                                {
                                    getscore += 8;
                                    for (int k = 0; k < neighbours.Length / 2; k++)
                                    {
                                        int nx = x + neighbours[k, 0];
                                        int ny = y + neighbours[k, 1];
                                        if (nx >= 0 && ny >= 0 && nx < farm.GetLength(0) && ny < farm.GetLength(1))
                                        {
                                            string neighbour = nx.ToString() + "," + ny.ToString();
                                            if (energy.ContainsKey(neighbour))
                                            {
                                                energy[neighbour]--;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    farm[x, y] = Tile.None;
                                }
                                break;
                            case Tile.YellowPlant:
                                money += 2;
                                if (energy[x.ToString() + "," + y.ToString()] >= 1 && energy[x.ToString() + "," + y.ToString()] <= 5)
                                {
                                    getscore += 2;
                                    for (int k = 0; k < neighbours.Length / 2; k++)
                                    {
                                        int nx = x + neighbours[k, 0];
                                        int ny = y + neighbours[k, 1];
                                        if (nx >= 0 && ny >= 0 && nx < farm.GetLength(0) && ny < farm.GetLength(1))
                                        {
                                            string neighbour = nx.ToString() + "," + ny.ToString();
                                            if (energy.ContainsKey(neighbour))
                                            {
                                                energy[neighbour]++;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    farm[x, y] = Tile.None;
                                }
                                break;
                            case Tile.BluePlant:
                                money++;
                                if (energy[x.ToString() + "," + y.ToString()] >= 2 && energy[x.ToString() + "," + y.ToString()] <= 24)
                                {
                                    getscore += 2;
                                }
                                else
                                {
                                    farm[x, y] = Tile.None;
                                }
                                break;
                            case Tile.WhitePlant:
                                money++;
                                if (energy[x.ToString() + "," + y.ToString()] >= 2 && energy[x.ToString() + "," + y.ToString()] <= 12)
                                {
                                    getscore += 3;
                                    for (int k = 0; k < neighbours.Length / 2; k++)
                                    {
                                        int nx = x + neighbours[k, 0];
                                        int ny = y + neighbours[k, 1];
                                        if (nx >= 0 && ny >= 0 && nx < farm.GetLength(0) && ny < farm.GetLength(1))
                                        {
                                            string neighbour = nx.ToString() + "," + ny.ToString();
                                            if (energy.ContainsKey(neighbour))
                                            {
                                                energy[neighbour]--;
                                            }
                                        }
                                    }
                                    energy[x.ToString() + "," + y.ToString()]++;
                                }
                                else
                                {
                                    farm[x, y] = Tile.None;
                                }
                                break;
                        }
                    }
                }
                if (money < 0)
                {
                    Console.Clear();
                    Console.WriteLine("Broke!");
                    Console.ReadKey(true);
                    return;
                }
                score = getscore;
                Thread.Sleep(500);
            }
        }
        static void Draw(Tile[,] farm)
        {
            Console.WriteLine($"Score: {score}\n" +
                $"Money: {money}\n");

            for (int y = 0; y < farm.GetLength(1); y++)
            {
                if (y == 0)
                {
                    Console.Write("  ");
                    for (int i = 0; i < farm.GetLength(0); i++)
                    {
                        Console.Write(i + " ");
                    }
                    Console.Write("\n");
                }
                Console.Write(y + " ");
                for (int x = 0; x < farm.GetLength(0); x++)
                {
                    switch (farm[x, y])
                    {
                        case Tile.RedPlant:
                            Console.Write("@");
                            break;
                        case Tile.BluePlant:
                            Console.Write("O");
                            break;
                        case Tile.GreenPlant:
                            Console.Write("$");
                            break;
                        case Tile.YellowPlant:
                            Console.Write("+");
                            break;
                        case Tile.WhitePlant:
                            Console.Write("W");
                            break;
                        case Tile.None:
                            Console.Write("_");
                            break;
                        default:
                            Console.Write("?");
                            break;
                    }
                    Console.Write(" ");
                }
                Console.Write("\n");
            }
        }
    }
}
