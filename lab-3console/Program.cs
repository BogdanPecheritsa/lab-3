using System;

namespace lab_3console
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Лабораторна робота №3: Структурні шаблони ===\n");

            AdapterDemo.RunDemo();
            Console.WriteLine();

            DecoratorDemo.RunDemo();
            Console.WriteLine();

            BridgeDemo.RunDemo();
            Console.WriteLine();

            ProxyDemo.RunDemo();
            Console.WriteLine();

            CompositeDemo.RunDemo();
            Console.WriteLine();

            FlyweightDemo.RunDemo();
            Console.WriteLine();

            Console.WriteLine("\nНатисніть будь-яку клавішу для виходу...");
            Console.ReadKey();
        }
    }
}