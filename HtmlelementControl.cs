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
using System.Collections;

namespace Html_Css
{
    public partial class HtmlelementControl : UserControl
    {
       readonly HtmlElementsAndAttributes attr = new HtmlElementsAndAttributes();
       readonly HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
        public HtmlNode node { get; set; }
        public HtmlelementControl()
        {
            InitializeComponent();
            HtmlElementsList list = new HtmlElementsList();
            this.BackColor = Color.White;
            this.comboBox1.DataSource = list.HtmlElem;
            this.comboBox1.DropDownHeight = this.Height - 42;
            }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        { 
            if (comboBox1.SelectedItem != null)
            {
                checkedListBox1.Items.Clear();
                checkedListBox1.Controls.Clear();
                innerHtmlTextBox.Clear();
                pathTextBox.Clear();
                string[] arr;
                document.Load(Application.StartupPath + "/tempHtml.txt");
                string[] elem = comboBox1.SelectedItem.ToString().Split("<>".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                node = document.CreateElement(elem[0]);
               while(checkedListBox1.CheckedIndices.Count> 0) 
                {
                    checkedListBox1.SetItemChecked(checkedListBox1.CheckedIndices[0], false);
                }
               if(attr.AttrDict.ContainsKey(elem[0]))
                {
                     arr = attr.AttrDict[elem[0]];
                    checkedListBox1.Items.AddRange(arr);
                }
               // Label lbl = new Label();
               // lbl.AutoSize = true;
               // lbl.Text = "Global Attributes";
               // lbl.Location = new Point(0, arr.Length * 15);
               // checkedListBox1.Controls.Add(lbl);
                checkedListBox1.Items.AddRange(attr.AttrDict["Global"]);
                checkedListBox1.Items.AddRange(attr.AttrDict["events"]);
            }       
            else return;

        }
        
        private void comboBox1_DropDown(object sender, EventArgs e)
        {
           
        }
        private object FindItemBySubstring(IEnumerable items, string target)
        {

            foreach (object item in items)
            {
                if (item.ToString().TrimStart("<".ToCharArray()).StartsWith(target))
                    return item;
            }
            return null;
        }

        private void checkedListBox1_ItemCheck_1(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
               
                var ch = (CheckedListBox)sender;
                TextBox tb = new TextBox();
                tb.Name = "textBox" + (e.Index + 1).ToString();
                int y = ch.GetItemHeight(e.Index) * (e.Index - ch.TopIndex);
                Point p1 = new Point(200, y);
                tb.Height = ch.GetItemHeight(e.Index);
                tb.Width = 200;
                tb.MaximumSize = new Size(200, 18);
                tb.AutoSize = false;
                checkedListBox1.Controls.Add(tb);
                tb.Location = p1;
                tb.BringToFront();
                Label lbl = new Label();
                lbl.Name = "label" + (e.Index + 1).ToString();
                lbl.Text = "Value";
                lbl.Location = new Point(150, y);
                ch.Controls.Add(lbl);
            }
            else if (e.NewValue == CheckState.Unchecked)
            {
                checkedListBox1.Controls.RemoveByKey("textBox" + (e.Index + 1).ToString());
                checkedListBox1.Controls.RemoveByKey("label" + (e.Index + 1).ToString());

            }
        }

        private void HtmlelementControl_Resize(object sender, EventArgs e)
        {
            if(this.Height!= 0)
            this.comboBox1.DropDownHeight = this.Height - 42;
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                checkedListBox1.Items.Clear();
                checkedListBox1.Controls.Clear();
                innerHtmlTextBox.Clear();
                pathTextBox.Clear();
                string[] arr;
                document.Load(Application.StartupPath + "/tempHtml.txt");
                string[] elem = comboBox1.SelectedItem.ToString().Split("<>".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                node = document.CreateElement(elem[0]);
                while (checkedListBox1.CheckedIndices.Count > 0)
                {
                    checkedListBox1.SetItemChecked(checkedListBox1.CheckedIndices[0], false);
                }
                if (attr.AttrDict.ContainsKey(elem[0]))
                {
                    arr = attr.AttrDict[elem[0]];
                    checkedListBox1.Items.AddRange(arr);
                }
                checkedListBox1.Items.AddRange(attr.AttrDict["Global"]);
                checkedListBox1.Items.AddRange(attr.AttrDict["events"]);
            }
        }

        private void comboBox1_DropDown_1(object sender, EventArgs e)
        {
            ComboBox cbox = (ComboBox)sender;
            cbox.SelectedItem = FindItemBySubstring(cbox.Items, cbox.Text);
        }
    }
}
