using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sapper
{
    public partial class TopListForm : Form
    {
        public TopListForm()
        {
            InitializeComponent();
        }

        private void TopListForm_Load(object sender, EventArgs e)
        {
            label1.Text = "Список побкдителей\r\n";
            List<String> top = Utils.readTop();
            foreach (String s in top)
            {
                label1.Text += s + "\r\n";
            }
        }
    }
}
