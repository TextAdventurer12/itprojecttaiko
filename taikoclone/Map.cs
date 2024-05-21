﻿using System;
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
    public class Map
    {
        public double Preempt { get; private set; }
        public double HitWindowGreat { get; private set; }
        public double HitWindowOk { get; private set; }
        public double HitWindowMiss => HitWindowGreat + HitWindowOk;
        public readonly List<HitObject> objects;
        public List<HitObject> activeObjects
            => objects.Where(obj => obj.Active).ToList();
        public Map(double preempt, double hitWindowGreat, double hitWindowOk, List<HitObject> objects)
        {
            Preempt = preempt;
            HitWindowGreat = hitWindowGreat;
            HitWindowOk = hitWindowOk;
            this.objects = objects;
            foreach (var hitObject in objects)
                hitObject.map = this;
        }
        public Judgement? TapObject(double time, Keys key)
        {
            HitObject nextObject = NextObject(time - HitWindowMiss);
            if (nextObject is null)
                return null;
            double timeToNextObject = nextObject.time - time;
            nextObject.box.Visible = false;
            nextObject.Active = false;
            Console.WriteLine(timeToNextObject);
            if (timeToNextObject > HitWindowMiss)
                return Judgement.Miss;
            if (nextObject.type == ObjectType.LEFT && key == Keys.T
                || nextObject.type == ObjectType.RIGHT && key == Keys.R)
                return Judgement.Miss;
            if (timeToNextObject < -HitWindowGreat || timeToNextObject > HitWindowGreat)
                return Judgement.Ok;
            return Judgement.Great;
        }
        public void DrawObjects(double time)
        {
            bool shouldDraw(HitObject obj) => obj.time - time < Preempt && obj.time - time > -HitWindowMiss;
            if (objects.Where(obj => shouldDraw(obj)).Count() == 0)
                return;
            foreach (var obj in objects.Where(obj => shouldDraw(obj)))
                obj.Draw(time);
        }
        public HitObject NextObject(double time)
        {
            HitObject nextObject = objects.First(obj => obj.time > time);
            return nextObject;
        }
        public IEnumerable<Judgement> CheckMissedObjects(double time)
        {
            IEnumerable<HitObject> missedObjects = objects.Where(obj => obj.LastTime < time && obj.Active);
            foreach (HitObject missedObject in missedObjects)
            {
                missedObject.Active = false;
                yield return Judgement.Miss;
            }
        }
    }
}
