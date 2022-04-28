using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KRGOL
{
    public partial class UniverseSize : Form
    {
        //Create EventHandler
        public delegate void ApplyEventHandler(object sender, ApplyEventArgs2 e);
        //Create Event
        public event ApplyEventHandler Apply;
        public UniverseSize()
        {
            InitializeComponent();
        }
        //Create Value For Width
        public int width { get; set; }
        //Create Value For Height
        public int height { get; set; }

        //Apply Button
        private void button1_Click(object sender, EventArgs e)
        {
            if (Apply != null) Apply(this, new ApplyEventArgs2(this.width,this.height));
        }
        //Event Args
        public class ApplyEventArgs2 : EventArgs
        {
            int width;
            int height;
            public int Width
            {
                get { return width; }
                set { width = value; }
            }
            public int Height
            {
                get { return height; }
                set { height = value; }
            }
            //Set Width and Height
            public ApplyEventArgs2(int width, int height)
            {
                this.width = width;
                this.height = height;
            }
        }

        //When Width Goes Up In Dialog So Does The Value in Script
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            width = (int)numericUpDown1.Value;
        }
        //When Height Goes Up In Dialog So Does The Value in Script
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            height = (int)numericUpDown2.Value;
        }
    }
}
