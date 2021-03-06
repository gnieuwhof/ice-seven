﻿namespace IceSeven
{
    using LexicalAnalysis;
    using System;
    using System.Diagnostics;
    using System.IO;

    public class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write(">>> ");
                string input = Console.ReadLine();

                string source = "<stdin>";

                if (File.Exists(input))
                {
                    source = input;
                    input = File.ReadAllText(input);
                    Console.WriteLine(input);
                }

                var stopwatch = new Stopwatch();

                stopwatch.Start();

                var lexer = new Lexer(source, input);

                var tokens = lexer.GetTokens(true, out LexicalError error);

                stopwatch.Stop();

                Console.WriteLine();

                if (error != null)
                {
                    Console.WriteLine(error);
                }
                else
                {
                    foreach (Token token in tokens)
                    {
                        Console.WriteLine(token);
                        if (token.Type == TokenType.ContentEnd)
                        {
                            break;
                        }
                    }
                }

                Console.WriteLine();
                Console.WriteLine($"Lexing time: {stopwatch.ElapsedMilliseconds} ms");
            }
        }
    }
}
