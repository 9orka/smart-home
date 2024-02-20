using MaterialSkin;
using MaterialSkin.Controls;
using Microsoft.Speech.Recognition;
using System.Speech.Synthesis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace smarthouse
{
    public partial class Form1 :Form
    {
        Thread th;
        public Form1()
        {
            InitializeComponent();
        }
       
  
     
        private void button1_Click(object sender, EventArgs e)
        {
            Close();
            th = new Thread(open);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }

        public void open(object obj)
        {
            Application.Run(new Form2());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Данное программное средство разработано с целью визуализации работы системы умного дома с возможностью голосового управления.\n\nПриятного пользования!","О программе");
        }
    }
}
