using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace LTrainorFirstPlayable
{
    internal class Program
    {
        static string tile = @"Map.txt";
        static string[] map = File.ReadAllLines(tile);
        static Encoding asciiEncoder = Encoding.GetEncoding("IBM437");
        static string borderHorizontal = ($"{asciiEncoder.GetString(new byte[1] { 205 })}");
        static string borderVertical = ($"{asciiEncoder.GetString(new byte[1] { 186 })}");
        static string borderTL = ($"{asciiEncoder.GetString(new byte[1] { 214 })}");
        static string borderTR = ($"{asciiEncoder.GetString(new byte[1] { 184 })}");
        static string borderL = ($"{asciiEncoder.GetString(new byte[1] { 204 })}");
        static string borderBR = ($"{asciiEncoder.GetString(new byte[1] { 189 })}");
        static string borderBL = ($"{asciiEncoder.GetString(new byte[1] { 212 })}");
        static string borderR = ($"{asciiEncoder.GetString(new byte[1] { 181 })}");
        static string topHalf = ($"{asciiEncoder.GetString(new byte[1] { 223 })}");
        static string botHalf = ($"{asciiEncoder.GetString(new byte[1] { 220 })}");
        static string roadLine = ($"{asciiEncoder.GetString(new byte[1] { 179 })}");
        static string flower = ($"{asciiEncoder.GetString(new byte[1] { 145 })}");
        static string grass = ($"{asciiEncoder.GetString(new byte[1] { 176 })}");
        static string solid = ($"{asciiEncoder.GetString(new byte[1] { 221 })}");
        static string player = ($"{asciiEncoder.GetString(new byte[1] { 167 })}");
        static string enemy = ($"{asciiEncoder.GetString(new byte[1] { 241 })}");
        static char heart = Convert.ToChar(3);
        static int playerX = 19;
        static int playerY = 8;
        static int enemyX = 24;
        static int enemyY = 21;
        static int playerHealth = 100;
        static int enemyHealth = 100;
        static int lives = 3;

        
        static void Main(string[] args)
        {
            DisplayControls();
            while (lives > 0 || enemyHealth > 0)
            {
                DisplayMap();
                ShowHealth();
                EnemyHealth();
                PlayerMove();
                EnemyMove();
                Console.SetCursorPosition(playerX, playerY);
                if (lives < 0 || enemyHealth < 0) 
                {
                    break;
                }
            }
            Console.SetCursorPosition(19, 13);
            Console.ReadKey(true);
        }

        static void PlayerMove()
        {
            ConsoleKey key = Console.ReadKey(true).Key;

            if (key.Equals(ConsoleKey.W))
            {
                playerY--;
            }
            else if (key.Equals(ConsoleKey.S))
            {
                playerY++;
            }
            else if (key.Equals(ConsoleKey.D))
            {
                playerX++;
            }
            else if (key.Equals(ConsoleKey.A))
            {
                playerX--;
            }
            else if (key.Equals(ConsoleKey.E))
            {
                EnemyDamage();
            }
            if (playerX > map[0].Length)
            {
                playerX = map[0].Length;
            }
            if (playerY > map.Length)
            {
                playerY = map.Length;
            }
            if (playerX < 1)
            {
                playerX = 1;
            }
            if (playerY < 1)
            {
                playerY = 1;
            }
        }

        static void EnemyMove()
        {
            Console.SetCursorPosition(enemyX, enemyY);
            Random rnd = new Random();
            int pos = rnd.Next(4);
            if (pos == 0)
            {
                enemyY--;
            }
            else if (pos == 1)
            {
                enemyY++;
            }
            else if (pos == 2)
            {
                enemyX++;
            }
            else if (pos == 3)
            {
                enemyX--;
            }
            if (enemyX > map[0].Length)
            {
                enemyX = map[0].Length;
            }
            if (enemyY > map.Length)
            {
                enemyY = map.Length;
            }
            if (enemyX < 1)
            {
                enemyX = 1;
            }
            if (enemyY < 1)
            {
                enemyY = 1;
            }
            TakeDamage();
        }

        static void TakeDamage()
        {
            Random rnd = new Random();
            int dmg = rnd.Next(5, 25);
            if (enemyX == playerX && enemyY == playerY)
            {
                playerHealth -= dmg;
            }
            if (playerHealth <= 0)
            {
                Console.SetCursorPosition(26,12);
                ColorChange(ConsoleColor.Black, ConsoleColor.DarkRed);
                Console.Write("You died!");
                Console.SetCursorPosition(16, 13);
                Console.Write("Press Any Key To Continue...");
                Console.ReadKey(true);
                playerX = 19;
                playerY = 8;
                playerHealth = 100;
                lives--;
            }
        }

        static void EnemyDamage()
        {
            Random rnd = new Random();
            int dmg = rnd.Next(5, 25);
            if (enemyX >= playerX - 1 && enemyY >= playerY - 1 && enemyX <= playerX + 1 && enemyY <= playerY + 1)
            {
                enemyHealth -= dmg;
            }
            if (enemyHealth <= 0)
            {
                Console.SetCursorPosition(26,12);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Enemy Defeated!");
            }
        }

        static void EnemyHealth()
        {
            Console.SetCursorPosition(41, map.Length + 2);
            float healthBar = enemyHealth / 5;
            if (healthBar < 20)
            {
                ColorChange(ConsoleColor.White, ConsoleColor.White);
                for (int j = 0; j < 20 - healthBar; j++)
                {
                    Console.Write(solid);
                }
            }
            for (int i = 0; i < Math.Floor(healthBar); i++)
            {
                ColorChange(ConsoleColor.DarkRed, ConsoleColor.DarkRed);
                Console.Write(solid);
            }
            if (enemyHealth <= 0)
            {
                Console.SetCursorPosition(41, map.Length + 2);
                for (int j = 0; j < 20; j++)
                {
                    ColorChange(ConsoleColor.White, ConsoleColor.White);
                    Console.Write(solid);
                }
            }
            ColorChange(ConsoleColor.Black, ConsoleColor.Black);
            Console.Write(solid);
            ColorChange(ConsoleColor.Black, ConsoleColor.White);
            Console.SetCursorPosition(44, map.Length + 3);
            Console.Write("Enemy Health: " + enemyHealth + " ");
            Console.SetCursorPosition(map[0].Length + 1, map.Length + 2);
            Console.Write(borderVertical);
            Console.SetCursorPosition(map[0].Length + 1, map.Length + 3);
            Console.Write(borderVertical);
        }

        static void ShowHealth()
        {
            Console.SetCursorPosition(0, map.Length + 2);
            ColorChange(ConsoleColor.Black, ConsoleColor.White);
            Console.Write(borderVertical);
            Console.SetCursorPosition (1, map.Length + 2);
            float healthBar = playerHealth / 5;
            for (int i = 0; i < Math.Floor(healthBar); i++) 
            {
                ColorChange(ConsoleColor.Red, ConsoleColor.Red);
                Console.Write(solid);
            }
            if (healthBar < 20 && healthBar > 0)
            {
                ColorChange(ConsoleColor.White, ConsoleColor.White);
                for (int j = 0; j < 20 - healthBar; j++)
                {
                    Console.Write(solid);
                }
            }
            if (playerHealth <= 0)
            {
                
                for(int j = 0;j < 20; j++)
                {
                    Console.SetCursorPosition(1, map.Length + 2);
                    ColorChange(ConsoleColor.White, ConsoleColor.White);
                    Console.Write(solid);
                }
            }
            ColorChange(ConsoleColor.Black, ConsoleColor.Black);
            Console.Write(solid);
            ColorChange(ConsoleColor.Black, ConsoleColor.White);
            Console.Write("\n" + borderVertical + "Health: " + playerHealth + " ");
            Console.SetCursorPosition(17, map.Length + 3);
            ColorChange(ConsoleColor.Black, ConsoleColor.DarkRed);
            for(int j = 0; j < lives; j++)
            {
                Console.Write(heart);
            }
            Console.Write(" ");
        }

        static void DisplayMap()
        {
            Console.SetCursorPosition(0, 0);
            ColorChange(ConsoleColor.Black, ConsoleColor.White);
            DisplayBorderTop();
            for (int x = 0; x < map.Length; x++)
            {
                Console.Write(borderVertical);
                string mapRow = map[x];
                for (int y = 0; y < mapRow.Length; y++)
                {
                    if (map[x][y] == '`')
                    {
                        ColorChange(ConsoleColor.Black, ConsoleColor.White);
                        Console.Write(topHalf);
                    }
                    else if (map[x][y] == '~')
                    {
                        ColorChange(ConsoleColor.Black, ConsoleColor.White);
                        Console.Write(botHalf);
                    }
                    else if (map[x][y] == '>')
                    {
                        ColorChange(ConsoleColor.Gray, ConsoleColor.Gray);
                        Console.Write(solid);
                    }
                    else if (map[x][y] == '!')
                    {
                        ColorChange(ConsoleColor.DarkGray, ConsoleColor.DarkGray);
                        Console.Write(solid);
                    }
                    else if (map[x][y] == '?')
                    {
                        ColorChange(ConsoleColor.DarkYellow, ConsoleColor.DarkYellow);
                        Console.Write(solid);
                    }
                    else if (map[x][y] == '^')
                    {
                        ColorChange(ConsoleColor.DarkYellow, ConsoleColor.Red);
                        Console.Write(topHalf);
                    }
                    else if (map[x][y] == '*')
                    {
                        ColorChange(ConsoleColor.DarkGray, ConsoleColor.Red);
                        Console.Write(botHalf);
                    }
                    else if (map[x][y] == '#')
                    {
                        ColorChange(ConsoleColor.DarkGreen, ConsoleColor.Green);
                        Console.Write(grass);
                    }
                    else if (map[x][y] == '<')
                    {
                        ColorChange(ConsoleColor.DarkGray, ConsoleColor.DarkYellow);
                        Console.Write(botHalf);
                    }
                    else if (map[x][y] == '@')
                    {
                        ColorChange(ConsoleColor.Gray, ConsoleColor.Black);
                        Console.Write(topHalf);
                    }
                    else if (map[x][y] == '$')
                    {
                        ColorChange(ConsoleColor.Gray, ConsoleColor.White);
                        Console.Write(topHalf);
                    }
                    else if (map[x][y] == 'r')
                    {
                        ColorChange(ConsoleColor.Black, ConsoleColor.Black);
                        Console.Write(solid);
                    }
                    else if (map[x][y] == 'p')
                    {
                        ColorChange(ConsoleColor.Red, ConsoleColor.Red);
                        Console.Write('p');
                    }
                    else if (map[x][y] == 'b')
                    {
                        ColorChange(ConsoleColor.DarkBlue, ConsoleColor.DarkBlue);
                        Console.Write('b');
                    }
                    else if (map[x][y] == 'v')
                    {
                        ColorChange(ConsoleColor.Magenta, ConsoleColor.Magenta);
                        Console.Write('v');
                    }
                    else if (map[x][y] == 'l')
                    {
                        ColorChange(ConsoleColor.Blue, ConsoleColor.Blue);
                        Console.Write('l');
                    }
                    else if (map[x][y] == 'i')
                    {
                        ColorChange(ConsoleColor.Black, ConsoleColor.White);
                        Console.Write(roadLine);
                    }
                    else if (map[x][y] == 'f')
                    {
                        ColorChange(ConsoleColor.DarkGreen, ConsoleColor.White);
                        Console.Write(flower);
                    }
                }
                ColorChange(ConsoleColor.Black, ConsoleColor.White);
                Console.Write(borderVertical);
                Console.Write("\n");
            }
            DisplayBorderBottom();
            ColorChange(ConsoleColor.DarkCyan, ConsoleColor.White);
            Console.SetCursorPosition(playerX, playerY);
            Console.Write(player);
            ColorChange(ConsoleColor.DarkRed, ConsoleColor.White);
            Console.SetCursorPosition(enemyX, enemyY);
            Console.Write(enemy);
        }

        static void DisplayControls()
        {
            ColorChange(ConsoleColor.White, ConsoleColor.Black);
            Console.SetCursorPosition(map[0].Length + 5, 1);
            Console.Write(borderTL);
            for (int i = 1; i < 11; i++)
            {
                Console.Write(borderHorizontal);
            }
            Console.Write(borderTR);
            ColorChange(ConsoleColor.Black, ConsoleColor.Black);
            Console.Write(solid);
            ColorChange(ConsoleColor.White, ConsoleColor.Black);
            Console.SetCursorPosition(map[0].Length + 5, 2);
            Console.Write(borderVertical + "Controls: " + borderVertical);
            ColorChange(ConsoleColor.Black, ConsoleColor.Black);
            Console.Write(solid);
            Console.SetCursorPosition(map[0].Length + 5, 3);
            ColorChange(ConsoleColor.White, ConsoleColor.Black);
            Console.Write(borderL);
            for (int i = 1; i < 11; i++)
            {
                Console.Write(borderHorizontal);
            }
            Console.Write(borderR);
            ColorChange(ConsoleColor.Black, ConsoleColor.Black);
            Console.Write(solid);
            Console.SetCursorPosition(map[0].Length + 5, 4);
            ColorChange(ConsoleColor.White, ConsoleColor.Black);
            Console.Write(borderVertical + "W - Up    " + borderVertical);
            ColorChange(ConsoleColor.Black, ConsoleColor.Black);
            Console.Write(solid);
            Console.SetCursorPosition(map[0].Length + 5, 5);
            ColorChange(ConsoleColor.White, ConsoleColor.Black);
            Console.Write(borderVertical + "A - Left  " + borderVertical);
            ColorChange(ConsoleColor.Black, ConsoleColor.Black);
            Console.Write(solid);
            Console.SetCursorPosition(map[0].Length + 5, 6);
            ColorChange(ConsoleColor.White, ConsoleColor.Black);
            Console.Write(borderVertical + "S - Down  " + borderVertical);
            ColorChange(ConsoleColor.Black, ConsoleColor.Black);
            Console.Write(solid);
            Console.SetCursorPosition(map[0].Length + 5, 7);
            ColorChange(ConsoleColor.White, ConsoleColor.Black);
            Console.Write(borderVertical + "D - Right " + borderVertical);
            ColorChange(ConsoleColor.Black, ConsoleColor.Black);
            Console.Write(solid);
            Console.SetCursorPosition(map[0].Length + 5, 8);
            ColorChange(ConsoleColor.White, ConsoleColor.Black);
            Console.Write(borderVertical + "E - Attack" + borderVertical);
            ColorChange(ConsoleColor.Black, ConsoleColor.Black);
            Console.Write(solid);
            ColorChange(ConsoleColor.White, ConsoleColor.Black);
            Console.SetCursorPosition(map[0].Length + 5, 9);
            Console.Write(borderBL);
            for (int i = 1; i < 11; i++)
            {
                Console.Write(borderHorizontal);
            }
            Console.Write(borderBR);
            ColorChange(ConsoleColor.Black, ConsoleColor.Black);
            Console.Write(solid);
        }

        static void DisplayBorderTop()
        {
            Console.Write(borderTL);
            for (int i = 0; i < map[0].Length; i++)
            {
                Console.Write(borderHorizontal);
            }
            Console.Write(borderTR);
            ColorChange(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine();
        }

        static void DisplayBorderBottom()
        {
            Console.Write(borderL);
            for (int i = 0; i < map[0].Length; i++)
            {
                Console.Write(borderHorizontal);
            }
            Console.Write(borderR);
            ColorChange(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine();
            Console.SetCursorPosition(0, map.Length + 4);
            Console.Write(borderBL);
            for (int i = 0;i < map[0].Length; i++)
            {
                Console.Write(borderHorizontal);
            }
            Console.Write(borderBR);
        }
        static void ColorChange(ConsoleColor background, ConsoleColor foreground)
        {
            Console.BackgroundColor = background;
            Console.ForegroundColor = foreground;
        }

        //    static char[,] map = new char[,] // dimensions defined by following data:
        //{
        //        {'`','~','`','~','`','~','`','~','`','~','!','!','?','?','?','?','?','?','?','?','?','?','?','!','?','?','?','?','?','?'},
        //        {'`','~','`','~','`','~','`','~','`','~','!','!','?','?','?','?','?','?','?','?','?','?','?','!','?','?','?','?','?','?'},
        //        {'`','~','`','~','`','~','`','~','`','~','>','?','?','?','?','?','?','?','?','?','?','?','?','>','?','?','?','?','?','?'},
        //        {'!','!','!','!','!','!','!','!','`','~','!','!','?','?','?','?','?','!','?','?','?','?','?','!','!','!','!','!','!','!'},
        //        {'?','?','?','?','?','?','!','!','`','~','!','!','?','?','?','?','?','!','?','?','?','?','?','!','!','~','`','~','`','~'},
        //        {'?','?','?','?','?','?','?','>','`','~','!','!','?','?','?','?','?','!','?','?','?','?','?','!','!','~','`','~','`','~'},
        //        {'?','?','?','?','?','?','!','!','`','~','!','!','?','?','?','?','?','!','!','!','!','!','!','!','!','~','`','~','`','~'},
        //        {'!','!','!','!','!','!','!','!','`','~','!','!','!','!','!','!','!','!','`','~','`','~','`','~','`','~','`','~','`','~'},
        //        {'`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~'},
        //        {'`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~'},
        //        {'`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~'},
        //        {'`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~'},
        //       };
        //}
        //    static char[,] map = new char[,] // dimensions defined by following data:
        //    {
        //        {'#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','f','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#',},
        //        {'#','#','#','#','#','#','#','#','#','#','#','#','#','#','f','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#',},
        //        {'#','#','#','f','#','#','#','#','#','#','#','#','#','#','f','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','!','!','!','!','!','!','!','!','!','!','!','!','!','!','!','!','#','#','#','#','#','#','#','#','#','#','f','#','#',},
        //        {'#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','!','!','!','!','!','!','!','?','?','?','?','?','?','?','?','?','?','?','?','?','!','!','#','#','#','#','#','#','#','#','#','#','#','#','#',},
        //        {'#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','!','!','?','?','?','?','!','?','?','?','?','?','?','?','?','?','?','?','?','?','!','!','#','#','#','#','#','#','#','#','#','#','#','#','#',},
        //        {'#','#','#','#','#','#','#','#','#','#','#','#','#','!','!','!','!','!','!','!','!','!','!','!','!','!','!','?','?','?','?','!','?','?','?','?','?','?','!','<','<','*','*','<','<','!','!','#','#','#','#','#','#','#','#','#','#','#','#','#',},
        //        {'#','#','#','#','#','#','f','#','#','#','#','#','#','!','!','`','~','`','~','`','~','`','~','`','~','!','!','?','?','?','?','?','?','?','?','?','?','?','!','?','?','?','?','?','?','!','!','#','#','#','#','#','#','#','#','#','#','#','#','#',},
        //        {'#','#','#','#','#','#','#','#','#','f','#','#','#','!','!','`','~','`','~','`','~','`','~','`','~','!','!','?','?','?','?','?','?','?','?','?','?','?','!','?','?','?','?','?','?','!','!','#','#','#','#','f','#','#','#','#','#','#','#','#',},
        //        {'#','#','#','#','#','#','#','#','#','#','#','#','#','!','!','`','~','`','~','`','~','`','~','`','~','>','?','?','?','?','?','?','?','?','?','?','?','?','>','?','?','?','?','?','?','!','!','#','#','#','#','#','#','#','#','#','#','#','#','#',},
        //        {'#','#','#','#','#','#','#','#','#','#','#','#','#','!','!','!','!','!','!','!','!','!','!','`','~','!','!','?','?','?','?','?','!','?','?','?','?','?','!','!','!','!','!','!','!','!','!','#','#','#','#','#','#','#','#','#','#','#','#','#',},
        //        {'#','#','#','#','#','#','#','#','#','#','#','#','#','!','!','?','?','?','?','?','?','!','!','`','~','!','!','?','?','?','?','?','!','?','?','?','?','?','!','!','~','`','~','`','~','!','!','#','#','#','#','#','#','f','#','#','#','#','#','#',},
        //        {'#','#','#','#','f','#','#','#','#','#','#','#','#','!','!','?','?','?','?','?','?','?','>','`','~','!','!','?','?','?','?','?','!','?','?','?','?','?','!','!','~','`','~','`','~','!','!','#','#','#','#','#','#','#','#','#','#','#','#','#',},
        //        {'#','#','#','#','#','#','#','#','#','#','#','#','#','!','!','?','?','?','?','?','?','!','!','`','~','!','!','?','?','?','?','?','!','!','!','!','!','!','!','!','~','`','~','`','~','!','!','#','f','#','#','#','#','#','f','#','#','#','#','#',},
        //        {'#','#','#','#','#','#','#','#','#','#','#','#','#','!','!','!','!','!','!','!','!','!','!','`','~','!','!','!','!','!','!','!','!','`','~','`','~','`','~','`','~','`','~','`','~','!','!','#','#','#','#','#','#','#','#','#','#','#','#','#',},
        //        {'#','#','#','#','#','f','#','#','#','#','#','#','#','!','!','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','!','!','#','#','#','#','#','#','#','#','#','#','#','#','#',},
        //        {'#','#','#','#','#','#','#','#','#','#','#','#','#','!','!','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','!','!','#','#','#','#','#','#','#','#','#','#','#','#','#',},
        //        {'#','#','#','#','#','#','#','#','#','#','#','#','#','!','!','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','!','!','#','#','#','#','#','#','#','#','#','#','#','#','#',},
        //        {'#','#','#','#','#','#','#','#','f','#','#','#','#','!','!','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','`','~','!','!','#','#','#','#','#','#','#','#','#','#','#','#','#',},
        //        {'#','#','#','#','#','#','#','#','#','#','#','#','#','!','!','!','!','!','!','!','!','!','!','!','!','!','!','!','@','$','@','$','!','!','!','!','!','!','!','!','!','!','!','!','!','!','!','#','#','#','#','#','#','#','#','#','#','#','#','#',},
        //        {'#','f','f','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','r','r','r','r','#','#','#','#','#','#','#','r','r','r','r','i','b','b','b','i','p','p','p','i','r','r','r','i','r','r','r','i',},
        //        {'#','#','#','#','#','#','#','#','#','#','#','f','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','r','r','r','r','#','#','#','#','#','#','#','r','r','r','r','i','b','b','b','i','p','p','p','i','r','r','r','i','r','r','r','i',},
        //        {'#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','r','r','r','r','r','r','r','r','r','r','r','r','r','r','r','r','r','r','r','r','r','r','r','r','r','r','r','r','r','r','r','r',},
        //        {'#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','r','r','r','r','#','#','#','#','#','#','#','r','r','r','r','r','r','r','r','r','r','r','r','r','r','r','r','r','r','r','r','r',},
        //        {'#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','r','r','r','r','#','#','#','#','#','#','#','r','r','r','r','i','l','l','l','i','r','r','r','i','v','v','v','i','r','r','r','i',},
        //           };
        //}
    }
}
