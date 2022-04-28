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
    public partial class RandomizeDialog : Form
    {
        public delegate void ApplyEventHandler(object sender, ApplyEventArgs e);
        public event ApplyEventHandler Apply;
        public RandomizeDialog()
        {
            InitializeComponent();
        }
        public int Seed { get; set; }

        private void RandomizeDialog_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Publish the event if it is not null
            // and pass the information with the custom
            // event arguements class.
            if (Apply != null) Apply(this, new ApplyEventArgs(this.Seed));

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            int num = rand.Next(0, 999999999);
            numericUpDown1.Value = num;
            Seed = num;

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Seed = (int)numericUpDown1.Value;
        }
    }

    public class ApplyEventArgs : EventArgs
    {
        int seed;
        public int Seed
        {
            get { return seed; }
            set { seed = value; }
        }
        public ApplyEventArgs(int seed)
        {
            this.seed= seed;
        }
    }
}
