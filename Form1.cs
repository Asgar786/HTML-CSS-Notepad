using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TidyManaged;
using HtmlAgilityPack;
using System.Threading;
using System.IO;
using System.Xml.XPath;
using Microsoft.Web.WebView2;
using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Core;
namespace Html_Css
{
    public partial class Form1 : Form
    { 
       readonly RichTextBox rtb1 = new RichTextBox();
        readonly WebView2 view = new WebView2();
        HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
        public Form1() 
        {
            InitializeComponent();
           
        }
            
        private void htmlSourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
              
            splitContainer1.BringToFront();
            
            richTextBox1.Clear(); 
            richTextBox1.LoadFile(Application.StartupPath + "/tempHtml.txt", RichTextBoxStreamType.PlainText);
            ColorHtmlTag();
            treeView1.Nodes.Clear();
            document.Load(Application.StartupPath + "/tempHtml.txt");
            HtmlNode node = document.DocumentNode;
            treeView1.Nodes.Add(node.Name);
            TreeViewMethods.BindTreeView(node, treeView1.Nodes[0]);
            treeView1.ExpandAll();
             htmlElementToolStripMenuItem1.Enabled = false;
        

        }

        private async void htmlToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            view.Dock = DockStyle.Fill;
            this.Controls.Add(view);
            view.BringToFront();
           await view.EnsureCoreWebView2Async(null);
           view.CoreWebView2.NavigateToString(richTextBox1.Text);
            htmlElementToolStripMenuItem1.Enabled = false;
        } 
        private void webBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtb1.Dock = DockStyle.Fill;
            this.Controls.Add(rtb1);
            rtb1.BringToFront();

            htmlElementToolStripMenuItem1.Enabled = false;

        }

        private void htmlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HtmlCssData hs = new HtmlCssData();
            document = hs.GetHtmlStructure;
            string filePath = Application.StartupPath;
            document.Save(filePath + "/tempHtml.txt");

            richTextBox1.LoadFile(filePath + "/tempHtml.txt", RichTextBoxStreamType.PlainText);
            using (Document d = Document.FromString(richTextBox1.Text))
            {
                d.IndentBlockElements = AutoBool.Yes;
                d.IndentSpaces = 2;
                d.AddTidyMetaElement = false;
                d.CleanAndRepair();
                d.Save(filePath + "/tempHtml.txt");
            }
            
            document.Load(filePath + "/tempHtml.txt");
            richTextBox1.Clear();
            richTextBox1.LoadFile(filePath + "/tempHtml.txt", RichTextBoxStreamType.PlainText);
            this.ColorHtmlTag();
            //view.NavigateToString(richTextBox1.Text);
            treeView1.Nodes.Clear();
            treeView1.Nodes.Add(document.DocumentNode.Name);
            TreeViewMethods.BindTreeView(document.DocumentNode, treeView1.Nodes[0]);
            htmlElementToolStripMenuItem1.Enabled = false;
            builder.Dispose();
            builder = new CssBulderControl1();
            //builder.Dock = DockStyle.Fill ;
           // this.Controls.Add(builder); 
            //  builder.BringToFront();

        }
       readonly HtmlelementControl ctl = new HtmlelementControl();
        private void htmlElementToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            ctl.Dock = DockStyle.Fill;
            this.Controls.Add(ctl);
            ctl.BringToFront();
            
            htmlElementToolStripMenuItem1.Enabled = true;
        }

        private void HtmlelementControl_Leave(object sender, EventArgs e)
        {
            htmlElementToolStripMenuItem1.Enabled = false;
        }

        private void htmlElementToolStripMenuItem1_Click(object sender, EventArgs e)
        { 
            
            HtmlNode nd = ctl.node;
            Control[] control = ctl.Controls.Find("checkedListBox1", true);
            document.Load(Application.StartupPath + "/tempHtml.txt");
           CheckedListBox  checkedListBox1 = (CheckedListBox)control[0];
            lock (checkedListBox1.CheckedItems)
            {
                foreach (var item in checkedListBox1.CheckedItems)
                {
                    string displayname = item.ToString();
                    int index = checkedListBox1.FindStringExact(displayname);
                    string name = "textBox" + (index + 1).ToString();
                    Control[] c = checkedListBox1.Controls.Find(name, true);
                    TextBox tb = (TextBox)c[0];
                    HtmlAttribute attr = document.CreateAttribute(displayname, tb.Text);
                    nd.Attributes.Add(attr);


                }
            }
            Control[] ctl1 = ctl.Controls.Find("innerHtmlTextBox", true);
            TextBox tb1 = (TextBox)ctl1[0];
            Control[] ctl2 = ctl.Controls.Find("pathTextBox", true);
            TextBox tb2 = (TextBox)ctl2[0];
            nd.InnerHtml = tb1.Text;
             XPathNavigator nav  = document.DocumentNode.CreateRootNavigator();
            if (tb2.Text != string.Empty)
            {
                try
               {
                   var ob = (XPathNodeIterator)nav.Evaluate(tb2.Text);
                   if (ob.Count == 0) { MessageBox.Show("The path is not correct"); return; }
                }

                catch(XPathException) { MessageBox.Show("The Path is Not Correct"); return; }
                HtmlNode node = document.DocumentNode.SelectSingleNode(tb2.Text);
                    node.AppendChild(nd);
                    document.Save(Application.StartupPath + "/tempHtml.txt");
                
            }
            else if (MessageBox.Show("Please Enter A Path") == DialogResult.OK)tb2.Select();
            richTextBox1.LoadFile(Application.StartupPath + "/tempHtml.txt", RichTextBoxStreamType.PlainText);
            toolStripStatusLabel1.Text = "Html Element <" + nd.Name + "> added";
            string filePath = Application.StartupPath;
            using (Document d = Document.FromString(richTextBox1.Text)) 
            {
                d.IndentBlockElements = AutoBool.Yes;
                d.IndentSpaces = 2;
                d.AddTidyMetaElement = false;
                d.CleanAndRepair(); 
                d.Save(filePath + "/tempHtml.txt");
               
            }
            document.LoadHtml(richTextBox1.Text);
            richTextBox1.Clear();
            richTextBox1.LoadFile(filePath + "/tempHtml.txt", RichTextBoxStreamType.PlainText);
            

        }
        CssBulderControl1 builder = new CssBulderControl1();
        private void cSSBlockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            builder.Dock = DockStyle.Fill;
            this.Controls.Add(builder);
            builder.BringToFront();
           cSSBlockToolStripMenuItem1.Enabled= true;
            htmlElementToolStripMenuItem1.Enabled = false;

        }
       readonly StringBuilder sb = new StringBuilder();
        private void cSSBlockToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Control[] ctl = builder.Controls.Find("listBox1", true);
            Control[] ctl1 = builder.Controls.Find("checkedListBox2", true);
            ListBox listBox1 = (ListBox)ctl[0];
            if (listBox1.SelectedItem != null)  
            {
                sb.AppendLine(listBox1.SelectedItem.ToString() + "\t" + "{");
            }
            CheckedListBox clb = (CheckedListBox)ctl1[0];
            foreach (var item in clb.CheckedItems)
            {
                string displayname = item.ToString();
                int index = clb.FindStringExact(displayname);
                string name = "textBox" + (index + 1).ToString();
                Control[] c = clb.Controls.Find(name, true);
                TextBox tb = (TextBox)c[0];
                sb.AppendLine(displayname + ":" + tb.Text + ";");
            }
            sb.AppendLine("}");
            rtb1.AppendText(sb.ToString());
            sb.Clear();
            toolStripStatusLabel1.Text = "CSS For " + listBox1.SelectedItem.ToString() + " added";
            cSSBlockToolStripMenuItem1.Enabled = false;
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            
            richTextBox1.LoadFile(Application.StartupPath + "/tempHtml.txt", RichTextBoxStreamType.PlainText);
            treeView1.Nodes.Clear();
            document.Load(Application.StartupPath + "/tempHtml.txt");
            HtmlNode node = document.DocumentNode;
            treeView1.Nodes.Add(node.Name);
            TreeViewMethods.BindTreeView(node, treeView1.Nodes[0]);
            
            ColorHtmlTag();
            HtmlNode nd = null;
            XPathNavigator nav = document.DocumentNode.CreateRootNavigator();
            if (nav.Evaluate("//head/link") != null)
            {
                 nd = document.DocumentNode.SelectSingleNode("//head/link");
              }
            if (nd != null)
            {
                string cssfilePath = nd.GetAttributeValue("href", string.Empty);
                rtb1.LoadFile(cssfilePath,RichTextBoxStreamType.PlainText);
            }
            else return;
        }

        private async void htmlToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Html File(.html)|*.html";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.LoadFile(openFileDialog1.FileName, RichTextBoxStreamType.PlainText);
                document.LoadHtml(richTextBox1.Text);
                document.Save(Application.StartupPath + "/tempHtml.txt");

                Thread.Sleep(1000);
               this.ColorHtmlTag();
                await view.EnsureCoreWebView2Async(null);
               view.CoreWebView2.NavigateToString(richTextBox1.Text);
                treeView1.Nodes.Clear();
                treeView1.Nodes.Add(document.DocumentNode.Name);
                TreeViewMethods.BindTreeView(document.DocumentNode, treeView1.Nodes[0]);
                htmlElementToolStripMenuItem1.Enabled = false;
                builder.Dispose(); 
                builder = new CssBulderControl1();
            }
        }
         
        private void htmlToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Html File(.html)|*.html";
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {

                richTextBox1.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.PlainText);

            }
            htmlElementToolStripMenuItem1.Enabled = false; 
        }

        private void cssToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "CSS File(.css)|*.css";
            saveFileDialog1.RestoreDirectory = true; 
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                rtb1.LoadFile(openFileDialog1.FileName, RichTextBoxStreamType.PlainText);
                document.Save(Application.StartupPath + "/tempcss.txt"); 
            }
            htmlElementToolStripMenuItem1.Enabled = false;
        }

        private void cssToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "CSS File(.css)|*.css";
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {

                rtb1.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.PlainText);

            }
            htmlElementToolStripMenuItem1.Enabled = false; 
        }
        private void GetHtmlTags(HtmlNode node, ref List<HtmlNode> taglist)
        {

            if (node.HasChildNodes)
            {

                foreach (HtmlNode n in node.ChildNodes)
                {
                    if (n.Name == "#text" ) continue;
                    else taglist.Add(n);
                    this.GetHtmlTags(n, ref taglist);
                }
            }
        }
        List<HtmlNode> HtmlTagList = new List<HtmlNode>();
         
       private void ColorHtmlTag()
        {
           
                FindAndHighLight("<", Color.Brown);
                FindAndHighLight(">", Color.Brown);
                FindAndHighLight("/", Color.Brown);
                FindAndHighLight("!DOCTYPE", Color.Brown);
               
                this.GetHtmlTags(document.DocumentNode, ref HtmlTagList);
                foreach (HtmlNode node in HtmlTagList)  
                {
                   
                   FindAndHighLight(node.Name, Color.Brown);    
                   //FindAndHighLight(node.InnerHtml, Color.Olive);
                }
            
                this.GetAttributes(document.DocumentNode, ref attrList);
            foreach (HtmlAttribute attr in attrList) 
            {
               FindAndHighLight(attr.Name, Color.Red);
               //FindAndHighLight(attr.Value, Color.Blue);
            }
           
        } 
        private  void FindAndHighLight(string str,Color clr)
        {
          
                 int index = richTextBox1.Text.IndexOf(str);
                 while (index != -1)
                 {
                     richTextBox1.SelectionStart = index;
                     richTextBox1.SelectionLength = str.Length;
                     richTextBox1.SelectionColor = clr;
                     index = richTextBox1.Text.IndexOf(str, index + str.Length);
                 }
            
        }
        private void GetAttributes(HtmlNode node, ref List<HtmlAttribute> collection)
        {
            if (node.HasChildNodes)
            {
                foreach (HtmlNode n in node.ChildNodes)
                {
                    if (n.Name == "#text") continue;
                    else
                    {
                        foreach (HtmlAttribute attr in n.Attributes)
                        {
                                    collection.Add(attr);
                        }
                    }
                    this.GetAttributes(n, ref collection);
                }
            }
        }
        List<HtmlAttribute> attrList = new List<HtmlAttribute>();

        private void cssToolStripMenuItem_Click(object sender, EventArgs e)
        {
            htmlElementToolStripMenuItem1.Enabled = false;
            rtb1.Clear();
        }
    }
}