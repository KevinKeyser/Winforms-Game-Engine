using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KevinKeyserParticleEngine
{
    public partial class GameForm : Form
    {
        SpriteBatch spriteBatch;
        ParticleEngine particleEngine;
        int totalTime = 0;
        int deltaTime = 0;
        int frameCounter = 0;
        PointF velocity = new PointF(5, 5);

        public GameForm()
        {
            InitializeComponent();
            spriteBatch = new SpriteBatch(ClientSize, Canvas);

            particleEngine = new ParticleEngine(Properties.Resources.circle, new PointF(ClientSize.Width/2, ClientSize.Height/2), 1, 7);
            particleEngine.StartColors = new Color[] { Color.Purple, Color.Cyan, Color.Pink };
            particleEngine.EndColors = new Color[] { Color.Purple, Color.Cyan, Color.Pink, Color.Transparent };
            Text = string.Format("FPS: {0} | Particle Count: {1}", 60, particleEngine.Particles.Count);
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            frameCounter++;
            totalTime += gameTimer.Interval;
            deltaTime = gameTimer.Interval;
            particleEngine.Location = new PointF(particleEngine.Location.X + velocity.X, particleEngine.Location.Y + velocity.Y);
            if (particleEngine.Location.X < 0)
            {
                velocity.X = Math.Abs(velocity.X);
            }
            if (particleEngine.Location.Y < 0)
            {
                velocity.Y = Math.Abs(velocity.Y);
            }
            if (particleEngine.Location.X > ClientSize.Width)
            {
                velocity.X = -Math.Abs(velocity.X);
            }
            if(particleEngine.Location.Y > ClientSize.Height)
            {
                velocity.Y = -Math.Abs(velocity.Y);
            }
            particleEngine.Update(deltaTime);


            spriteBatch.Clear(Color.White);
            spriteBatch.Begin();
            particleEngine.Draw(spriteBatch);
            spriteBatch.Draw(Properties.Resources.circle, new PointF(0, 0), Color.Blue);
            spriteBatch.End();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            spriteBatch.Update(ClientSize);
        }

        private void frameTimer_Tick(object sender, EventArgs e)
        {
            Text = string.Format("FPS: {0} | Particle Count: {1}", frameCounter, particleEngine.Particles.Count);
            frameCounter = 0;
        }
    }
}
