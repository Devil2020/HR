using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JASONParser
{
    public class Node
    {
        public List<Node> children = new List<Node>();
        public int value = Int32.MinValue;
        public string datatype = "";
        public string Name;
        public Token token;
        public Node(string N)
        {
            this.Name = N;
        }
    }
    class FunctionVal
    {
        public List<string> paramDatatype = new List<string>();
        public int paramNumber = 0;
    }
    class SymbolVal
    {
        public string datatype;
        public object val;
        public string scope;
    }
    class SemanticAnalyser
    {
        public static Dictionary<string, SymbolVal> SymbolTable = new Dictionary<string, SymbolVal>();
        public static Dictionary<string, FunctionVal> FunctionTable = new Dictionary<string, FunctionVal>();


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////SEMANTIC CODE HERE///////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static string currentScope = "";
        public static Node treeroot;
        public static int count = 0;
        public static List<string> dts;
        public static void TraverseTree(Node root)
        {

            if (root.Name == "Proc header")
            {
                currentScope = root.children[1].Name;
                if (FunctionTable.ContainsKey(currentScope))
                { MessageBox.Show("ERROR"); }
                else
                {
                    FunctionVal fv = new FunctionVal();
                    FunctionTable.Add(currentScope,fv);
                }
            }
            if (root.Name == "paramdecls")
            {
                count = 0;
                dts = new List<string>();
                handleParam(root);
                FunctionTable[currentScope].paramNumber = count;
                FunctionTable[currentScope].paramDatatype = dts;
            }
            for (int i = 0; i < root.children.Count; i++)
            {
                TraverseTree(root.children[i]);
            }
            if (root.Name == "vardecl")
            {
                handleVarDecl(root);
            }
            if (root.Name == "Param decl")
            {
                if (SymbolTable.ContainsKey(root.children[1].Name))
                {
                    MessageBox.Show("variable already declared");
                }
                else
                {
                    SymbolVal sv = new SymbolVal();
                    sv.datatype = root.children[0].children[0].Name;
                    sv.val = 0;
                    sv.scope = currentScope;
                    SymbolTable.Add(root.children[1].Name, sv);
                }
            }
        }
        public static void handleParam(Node node)
        {
            for (int i = 0; i < node.children.Count; i++)
            {
                handleParam(node.children[i]);
            }
            if (node.Name == "datatype")
            {
                count++;
                dts.Add(node.children[0].Name);
            }
            //if (node.Name == "datatype")
            //{

            //}
        }
        public static void handleVarDecl(Node node)
        {
            node.children[0].datatype = node.children[0].children[0].Name;
            node.children[1].datatype = node.children[0].datatype;
            handleIDlist(node.children[1]);
        }
        public static void handleIDlist(Node node)
        {
            node.children[0].datatype = node.datatype;
            if (SymbolTable.ContainsKey(node.children[0].Name))
            {
                MessageBox.Show("variable already declared");
            }
            else
            {
                SymbolVal sv = new SymbolVal();
                sv.datatype = node.children[0].datatype;
                sv.val = 0;
                sv.scope = "Global";
                SymbolTable.Add(node.children[0].Name, sv);
            }
            if (node.children.Count > 1)
            {
                node.children[2].datatype = node.datatype;
                handleIDlist(node.children[2]);
            }
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public static TreeNode PrintSemanticTree(Node root)
        {
            TreeNode tree = new TreeNode("Annotated Tree");
            TreeNode treeRoot = PrintAnnotatedTree(root);
            tree.Expand();
            if (treeRoot != null)
                tree.Nodes.Add(treeRoot);
            return tree;
        }
        static TreeNode PrintAnnotatedTree(Node root)
        {
            if (root == null)
                return null;

            TreeNode tree;
            if(root.value == Int32.MinValue && root.datatype == "")
                tree = new TreeNode(root.Name);
            else if(root.value != Int32.MinValue && root.datatype == "")
                tree = new TreeNode(root.Name + " & its value is: " + root.value);
            else if (root.value == Int32.MinValue && root.datatype != "")
                tree = new TreeNode(root.Name + " & its datatype is: " + root.datatype);
            else
                tree = new TreeNode(root.Name + " & its value is: " + root.value + " & datatype is: " + root.datatype);
            tree.Expand();
            if (root.children.Count == 0)
                return tree;
            foreach (Node child in root.children)
            {
                if (child == null)
                    continue;
                tree.Nodes.Add(PrintAnnotatedTree(child));
            }
            return tree;
        }
    }
}
