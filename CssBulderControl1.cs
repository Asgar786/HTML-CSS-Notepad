using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
namespace Html_Css
{
    public partial class CssBulderControl1 : UserControl
    {
       readonly HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
        public CssBulderControl1()
        {
         
            InitializeComponent();
            CssPropertiesList list = new CssPropertiesList();
            checkedListBox2.Items.AddRange(list.CssProp.ToArray());
            listBox1.Items.Add("*");
            document.Load(Application.StartupPath + "/tempHtml.txt");
            splitContainer2.Panel2Collapsed = true;
            
        }
        private void GetAttributes(HtmlNode node, ref List<string> collection, string name)
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
                            if (attr.Name == name)
                                if (!collection.Contains(attr.Value))
                                {
                                    if (attr.Name == "class")
                                    {
                                        attr.Value = attr.Value.Insert(0, ".");
                                        collection.Add(attr.Value);
                                    }
                                    else if (attr.Name == "id")
                                    {
                                        attr.Value = attr.Value.Insert(0, "#");
                                        collection.Add(attr.Value);
                                    }
                                }
                        }
                    }
                    this.GetAttributes(n, ref collection, name);
                }
            }
        }
        private void GetHtmlTags(HtmlNode node, ref List<string> taglist)
        {

            if (node.HasChildNodes)
            {

                foreach (HtmlNode n in node.ChildNodes)
                {
                    if (n.Name == "#text" || n.Name == "head") continue;
                    else
                    {
                        if (!taglist.Contains(n.Name)) taglist.Add(n.Name);
                    }
                    this.GetHtmlTags(n, ref taglist);
                }
            }
        }
        List<string> tagList = new List<string>();
        List<string> idList = new List<string>();
        List<string> classList = new List<string>();

        private void checkedListBox2_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                var ch = (CheckedListBox)sender;
                int y = ch.GetItemHeight(e.Index) * (e.Index - ch.TopIndex);
                Point p1 = new Point(300, y);
                TextBox tb = new TextBox
                {
                    Name = "textBox" + (e.Index + 1).ToString(),

                    Height = ch.GetItemHeight(e.Index),
                    Width = 200,
                    MaximumSize = new Size(300, 18),
                    AutoSize = false
                };
                checkedListBox2.Controls.Add(tb);
                tb.Location = p1;
                tb.BringToFront();
                Label lbl = new Label
                {
                    Name = "label" + (e.Index + 1).ToString(),
                    Text = "Value",
                    Location = new Point(250, y)
                };
                ch.Controls.Add(lbl);
            }
            else if (e.NewValue == CheckState.Unchecked)
            {
                checkedListBox2.Controls.RemoveByKey("textBox" + (e.Index + 1).ToString());
                checkedListBox2.Controls.RemoveByKey("label" + (e.Index + 1).ToString());

            }
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {


                switch (e.Index)
                {
                    case 0:

                        GetHtmlTags(document.DocumentNode.SelectSingleNode("//html"), ref tagList);
                        listBox1.Items.AddRange(tagList.ToArray());
                        break;

                    case 2:

                        GetAttributes(document.DocumentNode, ref idList, "id");
                        listBox1.Items.AddRange(idList.ToArray());
                        break;
                    case 1:

                        GetAttributes(document.DocumentNode.SelectSingleNode("//html"), ref classList, "class");
                        listBox1.Items.AddRange(classList.ToArray());
                        break;
                    case 3:
                        var ch = (CheckedListBox)sender;
                        int y = ch.GetItemHeight(0) * (e.Index + 1);
                        int y1 = ch.GetItemHeight(0) * (e.Index + 2);
                        Point p1 = new Point(0, y1);

                        TextBox tb = new TextBox
                        {
                            Name = "tbox",
                            Height = ch.GetItemHeight(e.Index),
                            Width = splitContainer1.Panel1.Width,
                            MaximumSize = new Size(300, 18),
                            AutoSize = false,
                            Location = p1
                        };
                        ch.Controls.Add(tb);
                        Label lbl = new Label
                        {
                            Name = "lbl",
                            AutoSize = true,
                            Text = "Custom Selector",
                            Location = new Point(0, y)
                        };
                        ch.Controls.Add(lbl);
                        Button btn = new Button
                        {
                            Name = "btn",
                            Width = this.splitContainer1.Panel1.Width,
                            Height = 24,
                            Text = "Add Custom Selector",
                            BackColor = Color.YellowGreen,
                            Location = new Point(0, ch.GetItemHeight(0) * (e.Index + 3))
                        };
                        btn.Click += Btn_Click;
                        ch.Controls.Add(btn);

                        break;
                }

            }
            else if (e.NewValue == CheckState.Unchecked)
            {
                switch (e.Index)
                {
                    case 0:
                        foreach (var item in tagList)
                        { listBox1.Items.Remove(item); }
                        tagList.Clear();
                        break;
                    case 2:
                        foreach (var item in idList)
                        { listBox1.Items.Remove(item); }
                        idList.Clear();
                        break;
                    case 1:
                        foreach (var item in classList)
                        { listBox1.Items.Remove(item); }
                        classList.Clear();
                        break;
                    case 3:
                        checkedListBox1.Controls.RemoveByKey("tbox");
                        checkedListBox1.Controls.RemoveByKey("lbl");
                        checkedListBox1.Controls.RemoveByKey("btn");
                        foreach (var item in customselector)
                        { listBox1.Items.Remove(item); }
                        customselector.Clear();
                        break;
                }
                if (listBox1.Items.Count == 1 && listBox1.SelectedIndex != 0) splitContainer2.Panel2Collapsed = true;
            }
        }
       readonly List<string> customselector = new List<string>();
        private void Btn_Click(object sender, EventArgs e)
        {
            Control[] ctl = checkedListBox1.Controls.Find("tbox", true);
            TextBox textBox1 = (TextBox)ctl[0];
            if (textBox1.Text != string.Empty) 
            {
                customselector.Add(textBox1.Text);
                listBox1.Items.Add(textBox1.Text);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex !=-1)
            splitContainer2.Panel2Collapsed = false;
            while (checkedListBox2.CheckedIndices.Count > 0)
            {
                checkedListBox2.SetItemChecked(checkedListBox2.CheckedIndices[0], false);
            }
            
          }

    }
}
