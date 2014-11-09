using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Notepad__;
using System.IO;


/*track text change event in all available textboxes at runtym
 *here i can do that when tabindex is changed(without it saving is also causing problem)
 */

/*using undo to detect word change
 * like in orignal notepad
 */

/*
 * right click on tab control
 * */




namespace Notepad__
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            tabControl1.TabPages.Clear();
            newtab();
            statusStrip1.Location = new System.Drawing.Point(0, 240);
        statusStrip1.Name = "statusStrip1";
        statusStrip1.Size = new System.Drawing.Size(284, 22);
        statusStrip1.TabIndex = 0;
        statusStrip1.Text = "statusStrip1";
        this.Controls.Add(statusStrip1);

        toolStripStatusLabel1.Name = "toolStripStatusLabel1";
        toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
        statusStrip1.Items.AddRange(new ToolStripItem[]{toolStripStatusLabel1});
            
        }
        List<notepad> n = new List<notepad>();
        public static StatusStrip statusStrip1 = new StatusStrip();
        public static ToolStripLabel toolStripStatusLabel1 = new ToolStripStatusLabel();


        public void newtab()
        {
            n.Add(new notepad());
            n[n.Count - 1].name = ("Untitled" + (n.Count - 1));
            tabControl1.TabPages.Add(n[n.Count - 1].tp);
            //tabControl1.TabPages[n.Count - 1].Focus();
            tabControl1.SelectedTab = tabControl1.TabPages[n.Count - 1];
            tabControl1.TabPages[n.Count - 1].Text = n[n.Count - 1].name;//to give tab its name
            Form1.toolStripStatusLabel1.Text = "Lines : " + n[n.Count - 1].r.Lines.Length + "   Columns : " + n[n.Count - 1].r.Text.Length;
        }

        void reftab()
        {
            int i = 0;
            foreach (notepad no in n)
            {
                if (no.defnam)
                {
                    tabControl1.TabPages[i].Text = no.Ref();

                }
                i++;
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newtab();
            //RichTextBox rtb = new RichTextBox();
            //rtb.Dock = DockStyle.Fill;
            //tp.Controls.Add(rtb);
            //tabControl.TabPages.Add(tp);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            if (o.ShowDialog() == DialogResult.OK)
            {
                newtab();
                n[n.Count - 1].r.Text = File.ReadAllText(o.FileName);
                n[n.Count - 1].name = o.FileName;
                string[] Split = n[n.Count - 1].name.Split(new Char[] { '\\' });
                tabControl1.TabPages[n.Count - 1].Text = Split[Split.Length - 1];
                n[n.Count - 1].defnam = true;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog x = new SaveFileDialog();
            if (n[tabControl1.SelectedIndex].defnam == false)
            {
                x.DefaultExt = ".txt";
                x.FileName = n[tabControl1.SelectedIndex].name;
                if (x.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(x.FileName, n[tabControl1.SelectedIndex].r.Text);
                    n[tabControl1.SelectedIndex].name = x.FileName;
                    n[tabControl1.SelectedIndex].defnam = true;
                }
            }
            else
            {
                File.WriteAllText(n[tabControl1.SelectedIndex].name, n[tabControl1.SelectedIndex].r.Text);
                n[tabControl1.SelectedIndex].defnam = true;
            }


        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog x = new SaveFileDialog();
            x.DefaultExt = ".txt";
            int i = tabControl1.SelectedIndex;
            x.FileName = n[i].name;
            if (x.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(x.FileName, n[i].r.Text);
                n[i].name = x.FileName;
                n[i].defnam = true;
                n[i].saved = true;
                reftab();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = 0;
            foreach (notepad no in n)
            {
                if ((!no.saved) || (!(no.r.Text == "")))
                {
                    tabControl1.SelectedIndex = i;
                    DialogResult oo = MessageBox.Show("Do you want to save changes to File " + no.name + " ?", "Notepad#", MessageBoxButtons.YesNo);
                    if (oo == DialogResult.Yes)
                    {
                        saveToolStripMenuItem_Click(sender, e);


                    }
                }
                i++;
            }
            Application.Exit();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            n[tabControl1.SelectedIndex].s.Push(n[tabControl1.SelectedIndex].r.Text);
            n[tabControl1.SelectedIndex].r.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (n[tabControl1.SelectedIndex].r.CanRedo == true)
            {
                if (n[tabControl1.SelectedIndex].s.Count > 0)
                {
                    n[tabControl1.SelectedIndex].r.Text = n[tabControl1.SelectedIndex].s.Pop();
                }

            }
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(n[tabControl1.SelectedIndex].r.SelectedText, true);
            SendKeys.Send("{DELETE}");
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(n[tabControl1.SelectedIndex].r.SelectedText, true);
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text))
                n[tabControl1.SelectedIndex].r.AppendText(Clipboard.GetDataObject().GetData(DataFormats.Text).ToString());

        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            n[tabControl1.SelectedIndex].r.SelectAll();
        }

        private void timeDateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            n[tabControl1.SelectedIndex].r.AppendText(DateTime.Now.ToString());
        }

        private void worldWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            worldWrapToolStripMenuItem.Checked = !worldWrapToolStripMenuItem.Checked;
            n[tabControl1.SelectedIndex].r.WordWrap = !n[tabControl1.SelectedIndex].r.WordWrap;

        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog f = new FontDialog();
            if (f.ShowDialog() == DialogResult.OK)
            {
                n[tabControl1.SelectedIndex].r.Font = f.Font;
            }
        }

        private void colourToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog c = new ColorDialog();
            if (c.ShowDialog() == DialogResult.OK)
            {
                n[tabControl1.SelectedIndex].r.ForeColor = c.Color;
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Notepad# By Ankit.. :D\n version 1.0.0", "ABOUT", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }



        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //n[tabControl1.SelectedIndex].saved = false;
            toolStripStatusLabel1.Text = "Lines : " + n[tabControl1.SelectedIndex].r.Lines.Length + "   Columns : " + n[tabControl1.SelectedIndex].r.Text.Length;

        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = tabControl1.SelectedIndex;

            if (tabControl1.TabCount == 1)
            {
                newToolStripMenuItem_Click(sender, e);
            }

            if ((!n[i].saved) || (!(n[i].r.Text == "")))
            {
                tabControl1.SelectedIndex = i;
                DialogResult oo = MessageBox.Show("Do you want to save changes to File " + n[i].name + " ?", "Notepad#", MessageBoxButtons.YesNo);
                if (oo == DialogResult.Yes)
                {
                    saveToolStripMenuItem_Click(sender, e);
                }

            }
            tabControl1.TabPages.RemoveAt(i);
            n.RemoveAt(i);
        }

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            newToolStripMenuItem_Click(sender, e);
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            openToolStripMenuItem_Click(sender, e);
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            saveToolStripMenuItem_Click(sender, e);
        }

        private void cutToolStripButton_Click(object sender, EventArgs e)
        {
            cutToolStripMenuItem_Click(sender, e);
        }

        private void copyToolStripButton_Click(object sender, EventArgs e)
        {
            copyToolStripMenuItem_Click(sender, e);
        }

        private void pasteToolStripButton_Click(object sender, EventArgs e)
        {
            pasteToolStripMenuItem_Click(sender, e);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            foreach (notepad no in n)
            {
                if (!no.saved)
                {
                    DialogResult oo = MessageBox.Show("Do you want to save changes to File " + no.name + " ?", "Notepad#", MessageBoxButtons.YesNo);
                    if (oo == DialogResult.Yes)
                    {
                        saveToolStripMenuItem_Click(sender, e);
                    }
                }
            }
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            closeAllButThisToolStripMenuItem_Click(sender, e);
        }

        private void closeAllButThisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = 0;
            int j = tabControl1.SelectedIndex;
            for (i = tabControl1.TabCount - 1; i >= 0; i--)
            {
                tabControl1.SelectedTab = tabControl1.TabPages[i];
                if (i != j)
                {
                    closeToolStripMenuItem_Click(sender, e);
                }
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            //Font currentFont = n[tabControl1.SelectedIndex].r.SelectionFont;
            //n[tabControl1.SelectedIndex].r.SelectionFont = new Font(currentFont, 7);

            Font currentFont = n[tabControl1.SelectedIndex].r.Font;
            float x=n[tabControl1.SelectedIndex].r.Font.Size;
            if((x+5)<72)
                x+=5;

            n[tabControl1.SelectedIndex].r.Font = new Font(currentFont.FontFamily, x, currentFont.Style);


        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Font currentFont = n[tabControl1.SelectedIndex].r.Font;
            float x = n[tabControl1.SelectedIndex].r.Font.Size;
            if ((x - 5) >8)
                x -= 5;

            n[tabControl1.SelectedIndex].r.Font = new Font(currentFont.FontFamily, x, currentFont.Style);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            closeToolStripMenuItem_Click(sender, e);
        }

        private void deleteFromDiskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (n[tabControl1.SelectedIndex].defnam)
            {
                File.Delete(n[tabControl1.SelectedIndex].name);
                closeToolStripMenuItem_Click(sender, e);
            }

        }

        



    }
}

