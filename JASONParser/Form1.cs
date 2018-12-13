using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JASONParser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            string code = "PROGRAM EvalFormula;\r\n{Calculates the result of a basic formula\r\nIllustrates the use of procedures in JASON}\r\nDECLARE\r\nINTEGER a;\r\nREAL b;\r\n{The procedure which evaluates the formula repeatedly}\r\nPROCEDURE FindFormula;\r\nPARAMETERS\r\nINTEGER x;\r\nREAL y;\r\nBEGIN\r\nWHILE x ! 0 DO\r\nIF x < 0 THEN SET y=10 - 4*x\r\nELSE SET y=4.5*x + 10\r\nENDIF;\r\nWRITE y;\r\nREAD x;\r\nENDWHILE\r\nEND;\r\n{ Main program }\r\nBEGIN\r\nREAD a;\r\nCALL FindFormula(a, b);\r\nWRITE b\r\nEND.\r\n";
            string[] reservedKeys = { "PROGRAM", "DECLARE", "INTEGER", "REAL", "PROCEDURE", "PARAMETERS", "BEGIN", "END", "IF", "THEN", "ELSE", "WHILE", "DO", "SET", "ENDIF", "ENDWHILE", "READ", "WRITE", "CALL" };
            string[] comments = { "{Calculates the result of a basic formula","Illustrates the use of procedures in JASON}", "{The procedure which evaluates the formula repeatedly}", "{ Main program }" };
            richTextBox1.Text = code;
            HighlightReserverdKeywords(Color.DarkBlue, reservedKeys);
            HighlightReserverdKeywords(Color.Green, comments);
            richTextBox1.ReadOnly = true;
            SemanticAnalyser.treeroot = ParserHelper.ReadParseTree("CurrentParseTree.txt");
            SemanticAnalyser.TraverseTree(SemanticAnalyser.treeroot);
            treeView1.Nodes.Add(SemanticAnalyser.PrintSemanticTree(SemanticAnalyser.treeroot));
            listBox1.Items.Add("Symbol Table:");
            foreach (var item in SemanticAnalyser.SymbolTable)
            {
                listBox1.Items.Add(item.Value.scope + " " +item.Value.datatype + " " + item.Key + "=" + item.Value.val.ToString());
            }
            foreach (var item in SemanticAnalyser.FunctionTable)
            {
                listBox1.Items.Add(item.Value.paramNumber + " " + item.Key);
            }


        }

        void HighlightReserverdKeywords(Color color,string[] keywords)
        {


            for (int i = 0; i < keywords.Length; i++)
            {
                int pointer = 0;
                int index = 0;
                string keyword = keywords[i];
                while (true)
                {
                    index = richTextBox1.Text.IndexOf(keyword, pointer);
                    //if keyword not found
                    if (index == -1)
                    {
                        break;
                    }
                    richTextBox1.Select(index, keyword.Length);
                    //richTextBox1.SelectionFont = new System.Drawing.Font(richTextBox1.Font, FontStyle.Bold);
                    richTextBox1.SelectionColor = color;
                    pointer = index + keyword.Length;
                }
            }

            
        }
    }
}
