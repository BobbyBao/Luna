using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    public partial class ConfirmBox : Form
    {
        public Action<bool> onConfirm;

        public ConfirmBox()
        {
            InitializeComponent();
        }

        public void Show(string message, string title, Action<bool> onFinished = null)
        {
            label1.Text = message;
            Text = title;
            onConfirm = onFinished;
            Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            onConfirm?.Invoke(true);         
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            onConfirm?.Invoke(false); 
            this.Hide();
        }
    }
}
