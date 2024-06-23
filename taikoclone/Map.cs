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
    public class Map
    {
        public double Preempt { get; private set; }
        public double HitWindowGreat { get; private set; }
        public double HitWindowOk { get; private set; }
        public double HitWindowMiss => HitWindowGreat + HitWindowOk;
        public readonly List<HitObject> objects;
        public List<HitObject> activeObjects
            => objects.Where(obj => obj.Active).ToList();

        public double EndTime => objects.Last().time;
        public Map(double preempt, double hitWindowGreat, double hitWindowOk, List<HitObject> objects)
        {
            Preempt = preempt;
            HitWindowGreat = hitWindowGreat;
            HitWindowOk = hitWindowOk;
            this.objects = objects;
            foreach (var hitObject in objects)
                hitObject.map = this;
        }
        public (Judgement? judgement, double error) TapObject(double time, Keys key)
        {
            HitObject nextObject = NextObject(time - HitWindowMiss);
            if (nextObject is null || (!Gameplay.rightKeys.Contains(key) && ! Gameplay.leftKeys.Contains(key)))
                return (null, -1);
            double timeToNextObject = nextObject.time - time;
            if (timeToNextObject > HitWindowMiss * 2)
                return (null, -1);
            nextObject.Active = false;
            if (timeToNextObject > HitWindowMiss)
                return (Judgement.Miss, timeToNextObject);
            if (nextObject.type == ObjectType.LEFT && Gameplay.rightKeys.Contains(key)
                || nextObject.type == ObjectType.RIGHT && Gameplay.leftKeys.Contains(key))
                return (Judgement.Miss, timeToNextObject);
            if (timeToNextObject < -HitWindowGreat || timeToNextObject > HitWindowGreat)
                return (Judgement.Ok, timeToNextObject);
            return (Judgement.Great, timeToNextObject);
        }
        public void DrawObjects(double time, Graphics target)
        {
            var drawableObjects = objects.Where(obj => obj.shouldDraw(time)).ToList();
            if (drawableObjects.Count() == 0)
                return;
            foreach (var obj in drawableObjects)
                obj.Draw(time, target);
        }
        public HitObject NextObject(double time)
        {
            var futureObjects = objects.Where(obj => obj.time > time && obj.Active);
            if (futureObjects.Count() == 0)
                return null;
            return futureObjects.First();
        }
        public IEnumerable<Judgement> CheckMissedObjects(double time)
        {
            IEnumerable<HitObject> missedObjects = objects.Where(obj => obj.time < time - HitWindowMiss && obj.Active);
            foreach (HitObject missedObject in missedObjects)
            {
                missedObject.Active = false;
                yield return Judgement.Miss;
            }
        }
        private IEnumerable<double> getDeltaTimes()
        {
            foreach (HitObject obj in objects)
            {
                HitObject prev = obj.Previous(1);
                double prevTime = prev?.time ?? 0;
                yield return obj.time - prevTime;
            }
        }
        public double Difficulty()
        {
            double difficultyValue = 0;
            List<double> deltaTimes = getDeltaTimes().Where(t => t != 0).ToList();
            Console.WriteLine(deltaTimes.Count());
            for (int i = 0; i < deltaTimes.Count; i++)
                difficultyValue += (1 / deltaTimes[i]) / (i + 1);
            return Math.Pow(difficultyValue * 800, 3);
        }
        public void Restart()
        {
            foreach (var hitObj in objects)
                hitObj.Active = true;
        }
    }
}