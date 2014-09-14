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
    public partial class SaveLoadForm : Form
    {
        private ISapper sapper=null;
        private Actions action;

        public SaveLoadForm(Actions action,ref ISapper sapper, /*ref*/Draw draw=null)
        {
            InitializeComponent();
            this.sapper = sapper;
            this.action = action;
        }

        private void saveSapper(ISapper sapper,String name)
        {
            FileStream fs=null;

            try
            {
                if (!name.EndsWith(".ss"))
                {
                    name += ".ss";
                }
                fs = new FileStream("saves/"+name, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                BinaryFormatter bf = new BinaryFormatter();

                bf.Serialize(fs, sapper);
                fs.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Сохранение не удалось\r\n" + e.Message);
                if (fs != null)
                {
                    try
                    {
                        fs.Close();
                        File.Delete("saves/" + name);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        private ISapper loadSapper(String name)
        {
            ISapper sapper=null;

            try
            {
                FileStream fs = new FileStream("saves/"+name, FileMode.Open, FileAccess.Read, FileShare.Read);
                BinaryFormatter bf = new BinaryFormatter();
                sapper = (ISapper)bf.Deserialize(fs);
                fs.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Загрузка не удалась\r\n" + e.Message);
            }

            return sapper;
        }

        private void SaveLoadForm_Load(object sender, EventArgs e)
        {
            reloadTable();
        }

        private void load_Click(object sender, EventArgs e)
        {
            ((Form1)(this.Owner)).sapper = sapper = loadSapper((String)(saves.SelectedRows[0].Cells[0].Value));
            if (sapper != null) this.Close();
        }

        private void save_Click(object sender, EventArgs e)
        {
            if (!textBox1.Text.Equals(""))
            {
                saveSapper(sapper, textBox1.Text);
                reloadTable();
            }
        }

        private void reloadTable()
        {
            saves.Rows.Clear();

            if (!Directory.Exists("saves"))
            {
                Directory.CreateDirectory("saves");
            }

            String[] files = Directory.GetFiles("saves/", "*.ss");

            foreach (String s in files)
            {
                FileInfo file = new FileInfo(s);
                saves.Rows.Add(s.Substring(6,s.Length-6), file.LastWriteTime.ToString());
            }
        }

        private void remove_Click(object sender, EventArgs e)
        {
            File.Delete("saves/" + (String)(saves.SelectedRows[0].Cells[0].Value));
            reloadTable();
        }
    }

    public enum Actions
    {
        SAVE,
        LOAD
    }


}
