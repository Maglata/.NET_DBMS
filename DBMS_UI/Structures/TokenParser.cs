using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMSPain.Structures
{
    public class TokenParser
    {
        public static List<Token> CreateTokens(string[] input)
        {
            List<Token> tokens = new List<Token>();

            for (int i = 0; i < input.Length; i++)
            {
                switch (input[i])
                {
                    case " ": break;
                    case "AND": tokens.Add(new Token(Token.Type.AND, input[i])); break;
                    case "OR": tokens.Add(new Token(Token.Type.OR, input[i])); break;
                    case "NOT": tokens.Add(new Token(Token.Type.NOT, input[i])); break;
                    case "(": tokens.Add(new Token(Token.Type.OPENBR, input[i])); break;
                    case ")": tokens.Add(new Token(Token.Type.CLOSEBR, input[i])); break;;
                    default:
                        {
                            string condition = string.Empty;

                            while (input[i] != "AND" && input[i] != "OR" && input[i] != "NOT" && input[i] != "(" && input[i] != ")" )
                            {
                                condition += input[i] + " ";
                                i++;

                                if (i == input.Length)
                                    break;
                            }
                            tokens.Add(new Token(Token.Type.CONDITION, condition));
                            i--;
                        }
                        break;
                }               
            }

            return tokens;
        }

        private static int CheckPriority(Token input)
        {
            switch(input.type) 
            {
                case Token.Type.NOT: return 3;
                case Token.Type.AND: return 2;
                case Token.Type.OR: return 1;
                default: return 0;
            }
        }

        public static List<Token> PolishNT(List<Token> tokens)
        {
            ImpStack<Token> stack = new ImpStack<Token>();
            List<Token> result = new List<Token>();

            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].type == Token.Type.AND || tokens[i].type == Token.Type.OR || tokens[i].type == Token.Type.NOT)
                {
                    while(stack.Count() > 0 && CheckPriority(stack.Peek()) >= CheckPriority(tokens[i])) 
                        result.Add(stack.Pop());
                    stack.Push(tokens[i]);
                }
                else if (tokens[i].type == Token.Type.OPENBR)
                    stack.Push(tokens[i]);
                else if (tokens[i].type == Token.Type.CLOSEBR)
                {
                    while (stack.Count() > 0 && stack.Peek().type != Token.Type.OPENBR)
                        result.Add(stack.Pop());

                    if (stack.Count() > 0 && stack.Peek().type != Token.Type.OPENBR)
                        return null;
                    else
                        stack.Pop();
                }
                else
                {
                    result.Add(tokens[i]);
                }
            }
            while(stack.Count() > 0)
                result.Add(stack.Pop());
            return result;
        }

    }
}
