﻿namespace LexicalAnalysis.Scanners
{
    using System;
    using System.Linq;

    public static class NumberScanner
    {
        public static bool Scan(Lexer lexer, ref Token token)
        {
#if DEBUG
            if (lexer == null)
                throw new ArgumentNullException(nameof(lexer));

            Scanner.EnsureCurrent(lexer,
                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.');
#endif

            Source src = lexer.Src;
            string value = string.Empty;
            bool dotEncountered = false;
            bool success = true;
            var validNexts = new[] {
                ';',
                ',',
                ')',
                ']',
                ' ',
                '+',
                '-',
                '*',
                '/',
                '\n',
                '\t',
                '\r',
            };

            while (!src.ReachedEnd())
            {
                char current = src.Current;
                char next = src.Peek();

                if (dotEncountered && (next == '.'))
                {
                    src.Advance();
                    success = false;
                    break;
                }

                if (current == '.')
                {
                    dotEncountered = true;

                    if (!char.IsDigit(next))
                    {
                        success = false;
                        break;
                    }
                }
                else if (!char.IsDigit(current))
                {
                    break;
                }

                value += current;

                src.Advance();
            }

            if (success)
            {
                if (!src.ReachedEnd() && !validNexts.Contains(src.Current))
                {
                    success = false;
                }
                else
                {
                    src.Rewind();
                }
            }

            TokenType tokenType = dotEncountered
                ? TokenType.Float
                : TokenType.Int;

            token.Type = tokenType;
            token.Value = value;

            return success;
        }
    }
}
