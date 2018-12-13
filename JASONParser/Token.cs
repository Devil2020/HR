using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JASONParser
{
    public enum Token_Class
    {
        Begin, Call, Declare, End, Do, Else, EndIf, EndUntil, EndWhile, If, Integer,
        Parameters, Procedure, Program, Read, Real, Set, Then, Until, While, Write,
        Dot, Semicolon, Comma, LParanthesis, RParanthesis, EqualOp, LessThanOp,
        GreaterThanOp, NotEqualOp, PlusOp, MinusOp, MultiplyOp, DivideOp,
        Identifier, Constant
    }

    public class Token
    {
        public string lex;
        public Token_Class token_type;
    }
    public class TokenHelper
    {
        public static Dictionary<string, Token_Class> TokenMap = new Dictionary<string, Token_Class>();
        public static void Initialize()
        {
            TokenMap = new Dictionary<string, Token_Class>();
            TokenMap.Add("If", Token_Class.If);
            TokenMap.Add("Begin", Token_Class.Begin);
            TokenMap.Add("Call", Token_Class.Call);
            TokenMap.Add("Declare", Token_Class.Declare);
            TokenMap.Add("End", Token_Class.End);
            TokenMap.Add("Do", Token_Class.Do);
            TokenMap.Add("Else", Token_Class.Else);
            TokenMap.Add("EndIf", Token_Class.EndIf);
            TokenMap.Add("EndUntil", Token_Class.EndUntil);
            TokenMap.Add("EndWhile", Token_Class.EndWhile);
            TokenMap.Add("Integer", Token_Class.Integer);
            TokenMap.Add("Parameters", Token_Class.Parameters);
            TokenMap.Add("Procedure", Token_Class.Procedure);
            TokenMap.Add("Program", Token_Class.Program);
            TokenMap.Add("Read", Token_Class.Read);
            TokenMap.Add("Real", Token_Class.Real);
            TokenMap.Add("Set", Token_Class.Set);
            TokenMap.Add("Then", Token_Class.Then);
            TokenMap.Add("Until", Token_Class.Until);
            TokenMap.Add("While", Token_Class.While);
            TokenMap.Add("Write", Token_Class.Write);
            TokenMap.Add("Idenifier", Token_Class.Identifier);
            TokenMap.Add("Constant", Token_Class.Constant);

            TokenMap.Add("Dot", Token_Class.Dot);
            TokenMap.Add("Semicolon", Token_Class.Semicolon);
            TokenMap.Add("Comma", Token_Class.Comma);
            TokenMap.Add("LParanthesis", Token_Class.LParanthesis);
            TokenMap.Add("RParanthesis", Token_Class.RParanthesis);
            TokenMap.Add("EqualOp", Token_Class.EqualOp);
            TokenMap.Add("LessThanOp", Token_Class.LessThanOp);
            TokenMap.Add("GreaterThanOp", Token_Class.GreaterThanOp);
            TokenMap.Add("NotEqualOp", Token_Class.NotEqualOp);
            TokenMap.Add("PlusOp", Token_Class.PlusOp);
            TokenMap.Add("MinusOp", Token_Class.MinusOp);
            TokenMap.Add("MultiplyOp", Token_Class.MultiplyOp);
            TokenMap.Add("DivideOp", Token_Class.DivideOp);
        }
    }
    public class ParserHelper
    {
        public static Node root;
        public static Node ReadParseTree(string filePath)
        {
            TokenHelper.Initialize();
            StreamReader sr = new StreamReader(filePath);
            string wholeFile = sr.ReadToEnd();
            string removeCharacter = "\n";
            wholeFile = wholeFile.Replace(removeCharacter, string.Empty);
            string[] lines = wholeFile.Split('\r');
            if(lines.Length == 0)
                return null;
            root = new Node(lines[0]);         
            int k = 1;
            ParseLine(lines,ref k,root);
            return root;
        }
        static void ParseLine(string[] lines, ref int k,Node parent)
        {
            string[] lineParts = lines[k].Split('`');

            Node n;
            if (lineParts.Length == 1)
                return;
            string[] token = lineParts[1].Split('@');
            if (token.Length == 1)
            {
                n = new Node(lineParts[1]);
            }
            else
            {
                n = new Node(token[0]);
                n.token = new Token();
                n.token.token_type = TokenHelper.TokenMap[token[1]];
                n.token.lex = token[0];
            }
            parent.children.Add(n);
            k++;
            for (int i = k; i < lines.Length; i++)
            {
                k = i;
                string[] lineParts2 = lines[k].Split('`');
                if (lineParts[0].Length == lineParts2[0].Length)
                {
                    Node n2;
                    string[] token2 = lineParts2[1].Split('@');
                    if (token2.Length == 1)
                    {
                        n2 = new Node(lineParts2[1]);
                    }
                    else
                    {
                        n2 = new Node(token2[0]);
                        n2.token = new Token();
                        
                        n2.token.token_type = TokenHelper.TokenMap[token2[1]];
                        n2.token.lex = token2[0];
                    }
                    parent.children.Add(n2);
                }
                else if (lineParts[0].Length > lineParts2[0].Length)
                {
                    break;
                }
                else
                {
                    ParseLine(lines, ref i, parent.children[parent.children.Count-1]);
                    i--;
                }
            }
        }
    }
}
