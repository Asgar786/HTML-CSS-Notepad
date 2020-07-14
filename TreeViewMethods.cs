using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Windows.Forms;
namespace Html_Css
{
    class TreeViewMethods
    {
        public static void BindTreeView(HtmlNode node, TreeNode tnode)
        {
            //tnode.Tag = node;
            TreeNode tn;
            if (node.HasChildNodes)
            {
                foreach (HtmlNode n in node.ChildNodes) 
                {
                    if (n.Name == "#text") continue;
                    else
                    {
                        tn = tnode.Nodes.Add(n.Name);
                        BindTreeView(n, tn);
                    }
                }
            }
        }
    }
}
