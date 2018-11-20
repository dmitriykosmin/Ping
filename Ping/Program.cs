using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using MyPingLibrary;

namespace PingConsole
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WindowWidth = 75;
            Console.BufferWidth = 75;
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            ConsoleKeyInfo cki = new ConsoleKeyInfo();
            PingSender pingSender = new PingSender();
            do
            {
                Console.Clear();
                Console.WriteLine("Введите адресс для команды ping\n" +
                "(Оставьте строку пустой для запроса к google.com):");
                string address = Console.ReadLine();
                do
                {
                    Console.WriteLine("\nНажмите \"Enter\" для остановки ping, " +
                        "\"Q\" - для выхода из приложения.\n");
                    while (Console.KeyAvailable == false)
                    {
                        Console.Write(pingSender.SendRequest(address: address));
                    }
                    cki = Console.ReadKey(true);
                    if (cki.Key == ConsoleKey.Q)
                    {
                        return;
                    }
                } while (cki.Key != ConsoleKey.Enter);
                Console.Write(pingSender.GetPingStatistics());
                Console.WriteLine("\nДля продолжения нажмите \"Enter\". Для выхода - \"Q\".\n");
                cki = Console.ReadKey(true);
            } while (cki.Key != ConsoleKey.Q);
        }
    }
}
