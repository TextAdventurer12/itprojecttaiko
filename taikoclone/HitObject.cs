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

        private int y = 25;
        private int radius = 25;
        private Image sprite;
        public PictureBox box;
        public ObjectType type;
        public Map map;
        public int Index;
        public bool Active = true;
        public double LastTime
            => time + map.HitWindowMiss;

        public HitObject(double time, ObjectType type)
        {
            this.time = time;
            this.type = type;
            sprite = type == ObjectType.LEFT ? Image.FromFile("../../hitcircleleft.png") : Image.FromFile("../../hitcircleright.png");
            box = new PictureBox();
            box.Image = sprite;
            box.Location = new Point(0, 0);
            box.Size = new Size(radius * 2, radius * 2);
            box.Visible = false;
            box.SizeMode = PictureBoxSizeMode.Zoom;
        }
        private double xPosition(double remainingTime)
        {
            // proportion of duration remaining
            double dR = remainingTime / map.Preempt;
            return (1 - dR) * Form1.playfieldStart + dR * Form1.playfieldEnd;
        }
        public void Draw(double currentTime)
        {
            double remainingTime = time - currentTime; 
            if (remainingTime < -map.HitWindowGreat || remainingTime > map.Preempt)
            {
                box.Visible = false;
                return;
            }
            else box.Visible = true;
            int x = (int)xPosition(remainingTime);
            box.Location = new Point(x, y);
        }
        public HitObject Previous(int index)
        {
            if (Index - index < 0)
                return null;
            return map.objects[Index - index];
        }
        public HitObject Next(int index)
        {
            if (Index + index >= map.objects.Count())
                return null;
            return map.objects[Index + index];
        }
    }
    public enum ObjectType
    {
        RIGHT,
        LEFT
    }
}