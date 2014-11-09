using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Notepad__
{
    class notepad
    {

        public string name { set; get; }
        public RichTextBox r = new RichTextBox();
        public bool saved;
        public bool defnam{set;get;}//has default name at start,ie 1
        public TabPage tp = new TabPage();
        public Stack<string> s=new Stack<string>();
        public notepad()
        {
            name = null;
            r.Text = null;
            defnam = false;
            saved = true;
            tp.Text = name;
            tp.Controls.Add(r);
            r.Dock = DockStyle.Fill;
            r.AcceptsTab = true;
            r.DetectUrls = true;
            r.WordWrap = false;
            r.ScrollBars = RichTextBoxScrollBars.Both;
            r.TextChanged += new EventHandler(r_TextChanged);
        }

        void r_TextChanged(object sender, EventArgs e)
        {
            Form1.toolStripStatusLabel1.Text = "Lines : " + r.Lines.Length + "   Columns : " + r.Text.Length;
            saved = false;
            if (r.CanRedo == false && s.Count > 0)
            {
                s.Clear();
            }


        }
        public string Ref()
        {
            string[] Split = name.Split(new Char[] { '\\' });
            return Split[Split.Length-1];

        }

        
    }
}
