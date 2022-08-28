using System;
using System.IO;

namespace OneMillionLights
{
    class Program
    {
        static void Main(string[] args)
        {
            LightsGrid grid = new LightsGrid();
            //grid.executeoperation("turn on", 1, 3, 4, 6);
            //grid.printgridtenonten(0, 0);

            Streamer.SetFileForReader("06.txt");
            while (!Streamer.EndOFStream)
            {
                var line = Streamer.GetParsedValue();
                grid.ExecuteOperation(line.Item1, line.Item2, line.Item3, line.Item4, line.Item5);
            }
            Console.WriteLine(grid.Count);
        }
    }

    static class Streamer
    {
        static StreamReader reader;

        public static void SetFileForReader(string fileName) => reader = new StreamReader(fileName);

        public static bool EndOFStream { get => reader.EndOfStream; }

        public static (string, int, int, int, int) GetParsedValue()
        {
            if (reader is null)
                throw new Exception("File not found");
            else if (reader.EndOfStream)
                throw new Exception("File has ending");
            else
            {
                string[] line = reader.ReadLine().Split();
                if (reader.EndOfStream)
                    Console.WriteLine("HEll");
                int indexNum;
                string operation;
                if (line[0] is "toggle")
                {
                    operation = line[0];
                    indexNum = 1;
                }
                else
                {
                    operation = line[0] + " " + line[1];
                    indexNum = 2;
                }

                string[] num1 = line[indexNum].Split(',');
                string[] num2 = line[indexNum + 2].Split(',');
                return (operation, int.Parse(num1[0]), int.Parse(num1[1]), int.Parse(num2[0]), int.Parse(num2[1]));
            }
        }
    }

    class LightsGrid
    {
        public byte[,] Grid { get; set; }

        public LightsGrid() => Grid = new byte[1000, 1000];

        public void TurnOn(int i, int j) => Grid[i, j] += 1;

        public void TurnOff(int i, int j) => Grid[i, j] -= (byte)(Grid[i, j] > 0 ? 1 : 0);

        public void Toggle(int i, int j) => Grid[i, j] += 2;

        public Action<int, int> GetOperation(string type)
        {
            switch (type)
            {
                case "turn on":
                    return TurnOn;
                case "turn off":
                    return TurnOff;
                case "toggle":
                    return Toggle;
                default:
                    throw new Exception("Unexpected operation's type");
            }
        }

        public void ExecuteOperation(string type, int x1, int y1, int x2, int y2)
        {
            var operation = GetOperation(type);
            for (int i = x1; i <= x2; i++)
                for (int j = y1; j <= y2; j++)
                    operation(i, j);
        }

        public void PrintGridTenOnTen(int startX, int startY)
        {
            for (int i = startX; i < startX + 10 && i < 1000; i++)
            {
                for (int j = startY; j < startY + 10 && j < 1000; j++)
                    Console.Write(Grid[i, j] + " ");
                Console.WriteLine();
            }
        }

        public void PrintAllGrid()
        {
            for (int i = 0; i < 1000; i++)
            {
                for (int j = 0; j < 1000; j++)
                    Console.Write(Grid[i, j] + " ");
                Console.WriteLine();
            }
        }

        public int Count { get
            {
                int ans = 0;
                for (int i = 0; i < 1000; i++)
                    for (int j = 0; j < 1000; j++)
                        ans += Grid[i, j];
                return ans;
            }
        }
    }
}
