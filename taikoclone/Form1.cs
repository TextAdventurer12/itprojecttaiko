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
        public static double playfieldStart = 112;
        public static double playfieldEnd = 700;
        Dictionary<Keys, bool> keyboard;
        double currentTime;
        List<Judgement> judgements = new List<Judgement>();
        public const double hitWindow = 100;
        public const double hitWindowMiss = 100;
        public const double preempt = 200;
        double clockRate = 1;
        Map map;
        public Form1()
        {
            InitializeComponent();
            map = MapParser.FromFile("../../2144235 SHIKI - Pure Ruby/SHIKI - Pure Ruby (youtune3) [Kantan].osu");
            keyboard = new Dictionary<Keys, bool>
            {
                { Keys.D, false },
                { Keys.F, false },
                { Keys.J, false },
                { Keys.K, false }
            };
            canvas.SendToBack();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (keyboard.Keys.Contains(e.KeyCode))
                keyboard[e.KeyCode] = true;

            Judgement? outcome = map.TapObject(currentTime, e.KeyCode);
            if (!(outcome is null))
                judgements.Add(outcome.Value);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (keyboard.Keys.Contains(e.KeyCode))
                keyboard[e.KeyCode] = false;
        }

        private void GameUpdate_Tick(object sender, EventArgs e)
        {
            currentTime += GameUpdate.Interval * clockRate;
            IEnumerable<Judgement> missedJudgements = map.CheckMissedObjects(currentTime);
            foreach (Judgement missedJudgement in missedJudgements)
                judgements.Add(missedJudgement);
            canvas.Invalidate();
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Black);
            Rectangle rect = new Rectangle(50, 50, 100, 100);
            SolidBrush brush = new SolidBrush(Color.DarkGray);
            if (keyboard[Keys.F] || keyboard[Keys.D])
                brush.Color = Color.Red;
            e.Graphics.FillPie(brush, rect, 90, 180);
            brush.Color = (keyboard[Keys.J] || keyboard[Keys.K]) ? Color.Blue : Color.DarkGray;
            e.Graphics.FillPie(brush, rect, 270, 180);
            if (judgements.Count != 0)
            {
                e.Graphics.FillEllipse(new SolidBrush(
                    judgements.Last() == Judgement.Great ? Color.Cyan
                    : judgements.Last() == Judgement.Ok ? Color.Green
                    : Color.IndianRed)
                    , new Rectangle(33 + 50, 33 + 50, 33, 33));
            }
            map.DrawObjects(currentTime, e.Graphics);
        }
    }
    public enum Judgement
    {
        Great = 300,
        Ok = 100,
        Miss = 0
    };
}