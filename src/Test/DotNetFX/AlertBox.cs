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
    public partial class AlertBox : Form
    {
        public Action onClick;
        public AlertBox()
        {
            InitializeComponent();
        }

        public void Show(string message, string title, Action onFinished = null)
        {
            label1.Text = message;
            Text = title;
            onClick = onFinished;
            Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            onClick?.Invoke();
            this.Hide();
            onClick = null;
        }
    }
}
