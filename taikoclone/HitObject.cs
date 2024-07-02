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

        private int radius = Gameplay.tapCircleRadius / 2;
        private SolidBrush colour;
        private Pen whitePen = new Pen(new SolidBrush(Color.White));
        public ObjectType type;
        public Map map;
        public int Index;
        public bool Active = true;

        public double drawTimeStart
            => time - map.Preempt;
        public double drawTimeEnd
            => time + map.HitWindowMiss;

        public HitObject(double time, ObjectType type, int index)
        {
            this.time = time;
            this.type = type;
            colour = new SolidBrush(Color.White);
            colour.Color = type == ObjectType.LEFT ? Color.Red : Color.Blue;
            this.Index = index;
        }
        public double xPosition(double currentTime, double playfieldEnd)
        {
            // proportion of duration remaining
            double dR = (currentTime - drawTimeStart) / (time - drawTimeStart);
            // linearly interpolate between the start of the playfield and the end
            return dR * Gameplay.playfieldStart + (1 - dR) * playfieldEnd;
        }
        public void Draw(double currentTime, Graphics target, double playfieldEnd)
        {
            if (!shouldDraw(currentTime))
                return;
            double remainingTime = time - currentTime;
            int x = (int)xPosition(currentTime, playfieldEnd);
            Rectangle drawBox = new Rectangle(x, Gameplay.tapCircleY - radius, radius * 2, radius * 2);
            if (remainingTime < map.HitWindowMiss)
                target.DrawEllipse(whitePen, drawBox);
            target.FillEllipse(colour, drawBox);
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