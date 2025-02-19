﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace sk
{
    public partial class SeeClient : Form
    {
        Dictionary<int, Client> dict = ClientsCache.getCache();
        Client client1 = new Client();
        public SeeClient(Client s)
        {
            InitializeComponent();
            client1 = s;
            load();
        }
        private void load()
        {
            textBox1.Text = Convert.ToString(client1.id);
            textBox1.Enabled = false;
            textBox2.Text = client1.name;
            textBox2.Enabled = false;
            textBox3.Text = Convert.ToString(client1.phone);
            textBox3.Enabled = false;
            richTextBox1.Text = client1.notes;
            richTextBox1.Enabled = false;
            textBox4.Enabled = false;
            int count = 0;
            Dictionary<int, int> lst = ClientsCache.getOrders();
            foreach(KeyValuePair<int,int> kv in lst)
            {
                if(kv.Key==client1.id)
                {
                    count = kv.Value;
                }
            }
            textBox4.Text = Convert.ToString(count);
            if(client1.date_delete==Convert.ToDateTime("31.12.9999 12:00:00"))
            {
                label6.Visible = false;
            }
            else
            {
                label6.Text = "Клиент удален";
                button3.Visible = false;
                button1.Visible = false;
            }
            
        }
        private void change_CLick(object sender, EventArgs e)
        {
            AddOrChangeClient form = new AddOrChangeClient(client1, "change");
            form.StartPosition = FormStartPosition.CenterScreen;
            form.FormClosing += new FormClosingEventHandler(formCLosing);
            form.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FormVisit form = new FormVisit("detClient", client1.id);
            form.StartPosition = FormStartPosition.CenterScreen;
            form.ShowDialog();
        }
        void formCLosing(object sender, FormClosingEventArgs e)
        {
            load();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string time = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss");
            client1.date_delete = Convert.ToDateTime(time);
            using (ClientContext sc = new ClientContext())
            {
                sc.Entry(client1).State = System.Data.Entity.EntityState.Modified;
                sc.SaveChanges();
            }
            string text = "Клиент успешно удален.";
            string caption = "Уведомление";
            MessageBox.Show(text, caption);
        }
    }
}
