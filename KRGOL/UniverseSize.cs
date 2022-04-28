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
        public delegate void ApplyEventHandler(object sender, ApplyEventArgs2 e);
        public event ApplyEventHandler Apply;
        public UniverseSize()
        {
            InitializeComponent();
        }
        public int width { get; set; }
        public int height { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Apply != null) Apply(this, new ApplyEventArgs2(this.width,this.height));
        }
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
            public ApplyEventArgs2(int width, int height)
            {
                this.width = width;
                this.height = height;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            width = (int)numericUpDown1.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            height = (int)numericUpDown2.Value;
        }
    }
}
