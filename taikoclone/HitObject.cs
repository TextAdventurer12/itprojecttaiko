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

        private int radius = 25;
        private Image sprite;
        public ObjectType type;
        public Map map;
        public int Index;
        public bool Active = true;

        public double drawTimeStart
            => time - map.Preempt;
        public double drawTimeEnd
            => time + map.HitWindowMiss;

        public HitObject(double time, ObjectType type)
        {
            this.time = time;
            this.type = type;
            sprite = type == ObjectType.LEFT ? Image.FromFile("../../hitcircleleft.png") : Image.FromFile("../../hitcircleright.png");
        }
        public double xPosition(double currentTime)
        {
            // proportion of duration remaining
            double dR = (currentTime - drawTimeStart) / (time - drawTimeStart);
            // linearly interpolate between the start of the playfield and the end
            return dR * Form1.playfieldStart + (1 - dR) * Form1.playfieldEnd;
        }
        public void Draw(double currentTime, Graphics target)
        {
            if (!shouldDraw(currentTime))
                return;
            double remainingTime = time - currentTime; 
            int x = (int)xPosition(currentTime);
            target.DrawImage(sprite, new Rectangle(x, Form1.tapCircleY + Form1.tapCircleRadius - radius, radius * 2, radius * 2));
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
        public bool shouldDraw(double time) 
            => this.time - time < map.Preempt && this.time - time > -map.HitWindowMiss && this.Active;

    }
    public enum ObjectType
    {
        RIGHT,
        LEFT
    }
}