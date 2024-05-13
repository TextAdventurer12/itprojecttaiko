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
        private double time;

        private double preempt = 500;
        private double playfieldStart = 112;
        private double playfieldEnd = 700;
        private double hitWindow = 100;
        private int y = 12;
        private int radius = 25;
        private Image sprite;
        public PictureBox box;

        public HitObject(double time)
        {
            this.time = time;
            sprite = Image.FromFile("../../taikohitcircle.png");
            box = new PictureBox();
            box.Image = sprite;
            box.Location = new System.Drawing.Point(0, 0);
            box.Size = new System.Drawing.Size(radius * 2, radius * 2);
            box.Visible = true;
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
            if (remainingTime < -hitWindow || remainingTime > preempt)
            {
                //box.Visible = false;
                return;
            }
            else box.Visible = true;
            int x = (int)xPosition(remainingTime);
            box.Location = new Point(x, y);
        }
    }
}