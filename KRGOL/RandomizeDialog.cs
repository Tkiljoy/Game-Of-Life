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
        //Create EventHandler
        public delegate void ApplyEventHandler(object sender, ApplyEventArgs e);
        //Create Event
        public event ApplyEventHandler Apply;
        public RandomizeDialog()
        {
            InitializeComponent();
        }
        //Create Value For Seed
        public int Seed { get; set; }

        private void RandomizeDialog_Load(object sender, EventArgs e)
        {

        }

        //Apply Button
        private void button2_Click(object sender, EventArgs e)
        {
            // Publish the event if it is not null
            // and pass the information with the custom
            // event arguements class.
            if (Apply != null) Apply(this, new ApplyEventArgs(this.Seed));

        }

        //Randomize Button
        private void button1_Click(object sender, EventArgs e)
        {
            //Create New Random
            Random rand = new Random();
            //Randomize Between Min and Max
            int num = rand.Next(0, 999999999);
            //Set The Value = To The Random Number
            numericUpDown1.Value = num;
            //Set Seed equal to the randomized number
            Seed = num;

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            //Set Seed equal to changed value
            Seed = (int)numericUpDown1.Value;
        }
    }

    //Event Args
    public class ApplyEventArgs : EventArgs
    {
        int seed;
        public int Seed
        {
            get { return seed; }
            set { seed = value; }
        }
        //Send Seed To Event Listener
        public ApplyEventArgs(int seed)
        {
            this.seed= seed;
        }
    }
}
