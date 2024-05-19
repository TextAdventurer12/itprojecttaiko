using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace taikoclone
{
    public class HitObject
    {
        /// <summary>
        /// The time at which this object should be hit
        /// </summary>
        public double time;

        private double preempt = 500;
        private double playfieldStart = 112;
        private double playfieldEnd = 700;
        private double hitWindowGreat = 100;
        private double hitWindowMiss = 100;
        private int y = 25;
        private int radius = 25;
        private Image sprite;
        public PictureBox box;
        public ObjectType type;

        public HitObject(double time, ObjectType type)
        {
            this.time = time;
            this.type = type;
            sprite = type == ObjectType.LEFT ? Image.FromFile("../../hitcircleleft.png") : Image.FromFile("../../hitcircleright.png");
            box = new PictureBox();
            box.Image = sprite;
            box.Location = new System.Drawing.Point(0, 0);
            box.Size = new System.Drawing.Size(radius * 2, radius * 2);
            box.Visible = false;
            box.SizeMode = PictureBoxSizeMode.Zoom;
        }
        private double xPosition(double remainingTime)
        {
            // proportion of duration remaining
            double dR = remainingTime / preempt;
            return (1 - dR) * playfieldStart + dR * playfieldEnd;
        }
        public void Draw(double currentTime)
        {
            double remainingTime = time - currentTime;
            if (remainingTime < -hitWindowGreat || remainingTime > preempt)
            {
                box.Visible = false;
                return;
            }
            else box.Visible = true;
            int x = (int)xPosition(remainingTime);
            box.Location = new Point(x, y);
        }
    }
    public enum ObjectType
    {
        RIGHT,
        LEFT
    }
}