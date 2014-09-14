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
    public partial class ResultForm : Form
    {
        public ResultForm(Statuses status)
        {
            InitializeComponent();
            if (status == Statuses.FAIL)
            {
                this.Text = "ПеЧаЛьКа...";
                label1.Text = "К сожалению, Вы проиграли.\r\nВы неудачник. \r\nНо удачи Вам в следующий раз. \r\nХотя это врядли Вам поможет.";
                textBox1.Visible = false;
            }
            else if (status == Statuses.WIN)
            {
                this.Text = "Победа!";
                label1.Text = "Вы восхитительны! \r\nВведите своё имя, \r\nесли хотите увековечить память о себе. \r\nНо не забывайтесь, \r\nвсегда есть к чему стремиться.";
                textBox1.Visible = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Visible && !textBox1.Text.Equals(""))
            {
                List<String> top = Utils.readTop();
                top.Add(textBox1.Text);
                Utils.writeTop(top);
            }
            this.Close();
        }
    }
}
