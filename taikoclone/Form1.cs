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
        List<Judgement> judgements = new List<Judgement>();
        double hitWindow = 100;
        double hitWindowMiss = 100;
        double preempt = 500;
        public Form1()
        {
            InitializeComponent();
            graphics = pictureBoxClickCircle.CreateGraphics();
            hitObjects = new List<HitObject>
            {
                new HitObject(1500),
                new HitObject(1600),
                new HitObject(1700),
                new HitObject(1800),
                new HitObject(1850)
            };
            foreach (var hitObject in hitObjects)
                this.Controls.Add(hitObject.box);
            pictureBoxClickCircle.SendToBack();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.R)
                leftKey = true;
            if (e.KeyCode == Keys.T)
                rightKey = true;
            if (hitObjects.Count == 0)
                return;
            double timeToNextObject = hitObjects.Min(obj => obj.time < currentTime - preempt ? double.MaxValue : obj.time - currentTime);
            if (timeToNextObject == double.MaxValue)
                return;
            if (timeToNextObject > hitWindow + hitWindowMiss)
                return;
            HitObject nextObject = hitObjects.First(obj => obj.time - currentTime == timeToNextObject);
            nextObject.box.Visible = false;
            hitObjects.Remove(nextObject);
            Console.WriteLine(timeToNextObject);
            judgements.Add(timeToNextObject > hitWindow ? Judgement.Miss : Judgement.Great);
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
            Rectangle rect = new Rectangle(0, 0, 100, 100);
            graphics.FillPie(new SolidBrush(leftKey ? Color.Red : Color.DarkGray), rect, 90, 180);
            graphics.FillPie(new SolidBrush(rightKey ? Color.Blue : Color.DarkGray), rect, 270, 180);
            if (judgements.Count != 0)
            {
                label1.Text = judgements.Where(j => j == Judgement.Great).Count().ToString();
                graphics.FillEllipse(new SolidBrush(judgements.Last() == Judgement.Great ? Color.Green : Color.IndianRed)
                    , new Rectangle(33, 33, 33, 33));
            }
            if (hitObjects.Count == 0)
                return;
            if (hitObjects[0].time < currentTime - hitWindow)
            {
                hitObjects[0].box.Visible = false;
                hitObjects.RemoveAt(0);
                judgements.Add(Judgement.Miss);
            }
            if (hitObjects.Count == 0)
                return;
            foreach (HitObject hitObject in hitObjects)
                hitObject.Draw(currentTime);
        }
        public enum Judgement
        {
            Great,
            Miss
        };
    }
}