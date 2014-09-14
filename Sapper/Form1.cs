using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;

namespace Sapper
{
    public partial class Form1 : Form
    {

        public ISapper sapper;
        private Draw draw;

        private Point prevTouchedCell;
        private ICell[][] prevField;

        private static Complexity EASY = new Complexity(7, 7, 3);
        private static Complexity MEDIUM = new Complexity(10, 10, 30);
        private static Complexity HARD = new Complexity(15, 20, 50);

        private Complexity complexity = MEDIUM;

        private void startNewGame()
        {
            prevField = null;
            prevTouchedCell = new Point(-1,-1);


            sapper = new Sapper(complexity.width, complexity.height, complexity.countOfBombs);
            Width = (int)((1+complexity.width * 2) * Draw.SIZE * (float)Math.Cos(Math.PI / 6) + 2 * Draw.START_POSITION_X);
            Height = (int)(complexity.height * (Draw.SIZE * (float)Math.Sin(Math.PI / 6) + Draw.SIZE) + 2 * Draw.START_POSITION_Y); 
            draw = new Draw(this.CreateGraphics(), complexity.width, complexity.height);

            prevField = copyTwoRankArray(sapper.Field);
            draw.drawField(prevField);

            ((ToolStripMenuItem)menuStrip1.Items[0]).DropDownItems[2].Enabled = true;
        }

        private bool isActive()
        {
            return sapper != null;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (isActive())
            {
                draw.drawField(sapper.Field);
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (isActive())
            {
                Point p = draw.getCellPoint(new PointF(e.X, e.Y));
                if (!p.Equals(new Point(-1, -1)))
                {
                    ICell[][] newField = sapper.recalculateField(p, e.Button.Equals(MouseButtons.Left));
                    if (sapper.Status == Statuses.WIN) draw.Mode = Statuses.WIN;
                    draw.drawChangedField(prevField, newField);
                    prevField = copyTwoRankArray(newField);

                    if (sapper.Status == Statuses.FAIL || sapper.Status == Statuses.WIN)
                    {
                        ResultForm form = new ResultForm(sapper.Status);
                        form.ShowDialog(this);
                        sapper = null;
                    }           
                }

              

            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isActive())
            {
                Point p = draw.getCellPoint(new PointF(e.X, e.Y));
                if (!prevTouchedCell.Equals(new Point(-1, -1)))
                {
                    if (!p.Equals(prevTouchedCell))
                    {
                        draw.drawUntouched(prevTouchedCell, sapper.Field);
                        if (!p.Equals(new Point(-1, -1))) draw.drawTouched(p);
                        prevTouchedCell = p;
                    }
                }
                else
                {
                    if (!p.Equals(new Point(-1, -1))) draw.drawTouched(p);
                    prevTouchedCell = p;
                }
            }
        }

        private void новаяИграToolStripMenuItem_Click(object sender, EventArgs e)
        {
            startNewGame();
        }


        private ICell[][] copyTwoRankArray(ICell[][] array)
        {
            ICell[][] result = new ICell[array.Length][];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new ICell[array[i].Length];
                for (int j = 0; j < result[i].Length; j++)
                {
                    result[i][j] = array[i][j].Copy();
                }
            }
            return result;
        }

        private struct Complexity
        {
            public Complexity(int width, int height, int countOfBombs)
            {
                this.width = width;
                this.height = height;
                this.countOfBombs = countOfBombs;
            }

            public int width;
            public int height;
            public int countOfBombs;
        }

        private void легкоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripItemCollection items = ((ToolStripMenuItem)this.menuStrip1.Items[1]).DropDownItems;
            for (int i = 0; i < items.Count; i++)
            {
                if (i != 0)
                {
                    ((ToolStripMenuItem)items[i]).Checked = false;
                }
            }
            ((ToolStripMenuItem)items[0]).Checked = true;
            complexity = EASY;
            startNewGame();
        }

        private void среднеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripItemCollection items = ((ToolStripMenuItem)this.menuStrip1.Items[1]).DropDownItems;
            for (int i = 0; i < items.Count; i++)
            {
                if (i != 1)
                {
                    ((ToolStripMenuItem)items[i]).Checked = false;
                }
            }
            ((ToolStripMenuItem)items[1]).Checked = true;
            complexity = MEDIUM;
            startNewGame();
        }

        private void сложноToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripItemCollection items = ((ToolStripMenuItem)this.menuStrip1.Items[1]).DropDownItems;
            for (int i = 0; i < items.Count; i++)
            {
                if (i != 2)
                {
                    ((ToolStripMenuItem)items[i]).Checked = false;
                }
            }
            ((ToolStripMenuItem)items[2]).Checked = true;
            complexity = HARD;
            startNewGame();
        }

        private void топИгроковToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TopListForm form = new TopListForm();
            form.ShowDialog(this);
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void загрузитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveLoadForm form = new SaveLoadForm(Actions.LOAD,ref sapper);
            form.ShowDialog(this);

            if (sapper != null)
            {

                draw = new Draw(this.CreateGraphics(), sapper.Field[0].Length, sapper.Field.Length);
                prevField = copyTwoRankArray(sapper.Field);
                draw.drawField(prevField);
            }
            
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveLoadForm form = new SaveLoadForm(Actions.SAVE, ref sapper);
            form.ShowDialog(this);
        }

        //private void Form1_Resize(object sender, EventArgs e)
        //{
        //    if (isActive())
        //    {
        //        this.Refresh();
        //        (new Draw(this.CreateGraphics(),sapper.Field[0].Length,sapper.Field.Length)).drawField(sapper.Field);
        //    }
        //}

       
    }
}
