using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace taikoclone
{
    public partial class Form1 : Form
    {
        bool leftKey;
        bool rightKey;
        double currentTime;
        Graphics graphics;
        List<HitObject> hitObjects;
        public Form1()
        {
            InitializeComponent();
            graphics = pictureBoxClickCircle.CreateGraphics();
            hitObjects = new List<HitObject>
            {
                new HitObject(2000),
            };
            foreach (var hitObject in hitObjects)
                this.Controls.Add(hitObject.box);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.R)
                leftKey = true;
            if (e.KeyCode == Keys.T)
                rightKey = true;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.R)
                leftKey = false;
            if (e.KeyCode == Keys.T)
                rightKey = false;
        }

        private void GameUpdate_Tick(object sender, EventArgs e)
        {
            currentTime += GameUpdate.Interval;
            label1.Text = currentTime.ToString();
            foreach (HitObject hitObject in hitObjects)
                hitObject.Draw(currentTime);
            Rectangle rect = new Rectangle(0, 0, 100, 100);
            graphics.FillPie(new SolidBrush(leftKey ? Color.Red : Color.DarkGray), rect, 90, 180);
            graphics.FillPie(new SolidBrush(rightKey ? Color.Blue : Color.DarkGray), rect, 270, 180);
        }
    }
}
