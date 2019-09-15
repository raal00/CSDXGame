using System;
using DXFormHandler;
using DXFormHandler.Controller;

namespace CSharpDX_HowTo
{
    class Program
    {
        static void Main(string[] args)
        {
            _2DGame game = new _2DGame();
            game.Play();

            Console.ReadKey();
        }
    }
}
