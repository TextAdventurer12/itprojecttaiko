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
using NAudio;
using NAudio.Wave;

namespace taikoclone
{
    public partial class Gameplay : Form
    {
        /// <summary>
        /// The X Position of the left side of the playfield
        /// </summary>
        public const double playfieldStart = 80;

        /// <summary>
        /// The X position of the right side of the playfield
        /// </summary>
        public const double playfieldEnd = 700;

        /// <summary>
        /// Whether the keys contained are pressed
        /// </summary>
        Dictionary<Keys, bool> keyboard;

        /// <summary>
        /// The keys which map to a RIGHT circle
        /// </summary>
        public static List<Keys> rightKeys = new List<Keys> { Keys.J, Keys.K };

        /// <summary>
        /// the keys which map to a LEFT circle
        /// </summary>
        public static List<Keys> leftKeys  = new List<Keys> { Keys.D, Keys.F };
        public const double initial_delay = 3000;
        public const double offset = 0;

        /// <summary>
        /// The current tick that the form is on
        /// The initial value is an offset used to ensure audio is synced
        /// </summary>
        double currentTime = -initial_delay;

        /// <summary>
        /// The judgements obtained throughout gameplay
        /// </summary>
        List<Judgement> judgements = new List<Judgement>();
        List<double> hitTimes = new List<double>();

        /// <summary>
        /// The size, in ms, of the great hit window
        /// A hit which is hitWindow ms early or hitWindow ms late will be a great
        /// </summary>
        public const double hitWindow = 100;

        /// <summary>
        /// The size, in ms, of the miss hit window
        /// Extends out of the great hit window, any taps within this window will result in an OK, and anything outside will result in a miss
        /// </summary>
        public const double hitWindowMiss = 100;

        /// <summary>
        /// The amount of ms that circles are visible for before they should be pressed
        /// </summary>
        public const double preempt = 2000;

        /// <summary>
        /// The size of the input circle
        /// </summary>
        public const int tapCircleRadius = 50;

        /// <summary>
        /// The Y Position of the input circle
        /// </summary>
        public const int tapCircleY = 50;

        /// <summary>
        /// The clock rate of gameplay
        /// </summary>
        double clockRate = 1;

        /// <summary>
        /// The current map being played
        /// </summary>
        Map map;

        public static Font UIFont = new Font("Arial", 25);

        IWavePlayer waveOutDevice;
        System.Diagnostics.Stopwatch cumWatch = new System.Diagnostics.Stopwatch();
        public Gameplay(MapInfo selectedMap)
        {
            InitializeComponent();
            map = selectedMap.map;
            keyboard = new Dictionary<Keys, bool>
            {
                { Keys.D, false },
                { Keys.F, false },
                { Keys.J, false },
                { Keys.K, false }
            };
            canvas.SendToBack();
            waveOutDevice = new WaveOut();
            AudioFileReader audioFileReader = new AudioFileReader(selectedMap.AudioFile);
            waveOutDevice.Init(audioFileReader);
            cumWatch.Start();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (keyboard.Keys.Contains(e.KeyCode))
                keyboard[e.KeyCode] = true;

            (Judgement? judgement, double error) outcome = map.TapObject(currentTime, e.KeyCode);
            if (!(outcome.judgement is null))
            {
                judgements.Add(outcome.judgement.Value);
                hitTimes.Add(outcome.error);
                //Console.WriteLine($"{CurrentAccuracy() * 100:F2}");
                Console.WriteLine($"Real: {cumWatch.ElapsedMilliseconds}. Expected: {currentTime / clockRate}");
                Console.WriteLine($"Error: {outcome.error}, Current Mean Error: {hitTimes.Average()}");
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (keyboard.Keys.Contains(e.KeyCode))
                keyboard[e.KeyCode] = false;
        }

        private void GameUpdate_Tick(object sender, EventArgs e)
        {
            currentTime = cumWatch.ElapsedMilliseconds - initial_delay;
            if (currentTime > offset - 25 && currentTime < offset + 25 && !(waveOutDevice.PlaybackState == PlaybackState.Playing))
                waveOutDevice.Play();
            IEnumerable<Judgement> missedJudgements = map.CheckMissedObjects(currentTime);
            foreach (Judgement missedJudgement in missedJudgements)
                judgements.Add(missedJudgement);
            canvas.Invalidate();
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Black);
            Rectangle tapCircle = new Rectangle((int)playfieldStart - tapCircleRadius, tapCircleY, tapCircleRadius * 2, tapCircleRadius * 2);
            SolidBrush brush = new SolidBrush(Color.DarkGray);
            if (keyboard[Keys.F] || keyboard[Keys.D])
                brush.Color = Color.Red;
            e.Graphics.FillPie(brush, tapCircle, 90, 180);
            brush.Color = (keyboard[Keys.J] || keyboard[Keys.K]) ? Color.Blue : Color.DarkGray;
            e.Graphics.FillPie(brush, tapCircle, 270, 180);
            if (judgements.Count != 0)
            {
                e.Graphics.FillEllipse(new SolidBrush(
                    judgements.Last() == Judgement.Great ? Color.Cyan
                    : judgements.Last() == Judgement.Ok ? Color.Green
                    : Color.IndianRed)
                    , new Rectangle((int)playfieldStart - 17, tapCircleY + tapCircleRadius - 17, 34, 34));
            }
            e.Graphics.DrawString((CurrentAccuracy() * 100).ToString("F2"), UIFont, brush, new Point(canvas.Width - 25 * 6, 12));
            map.DrawObjects(currentTime, e.Graphics);
        }
        private double CurrentAccuracy()
        {
            if (judgements.Count() == 0)
                return 1;
            return judgements.Average(j => (int)j) / 300;
        }
    }
    public enum Judgement
    {
        Great = 300,
        Ok = 100,
        Miss = 0,
    };
}