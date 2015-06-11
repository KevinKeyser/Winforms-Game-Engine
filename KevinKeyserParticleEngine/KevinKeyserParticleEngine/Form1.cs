﻿using System;
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
        float rotation = 0;
        PointF velocity;
        Random randomGenerator;
        Color[] colors;
        List<Keys> keysDown = new List<Keys>();


        public GameForm()
        {
            InitializeComponent();
            spriteBatch = new SpriteBatch(ClientSize, Canvas);

            randomGenerator = new Random();
            colors = new Color[] { Extensions.Lerp(Color.Red, Color.Transparent, 0f) };//, Color.Purple, Color.Black };

            velocity = new PointF(5,5);
            particleEngine = new ParticleEngine(new PointF(ClientSize.Width/2, ClientSize.Height/2), 1, 100);
            particleEngine.RandomGenerator = randomGenerator;
            particleEngine.MinVelocity = new PointF(-1, -1);
            particleEngine.MaxVelocity = new PointF(1, 1);
            particleEngine.StartSize = 1;
            particleEngine.EndSize = 10;
            particleEngine.StartColors = new Color[] { Color.LightBlue, Color.Turquoise, Color.AliceBlue, Color.CadetBlue, Color.BlueViolet, Color.CornflowerBlue };
            particleEngine.EndColors = new Color[] { Color.Transparent };
            particleEngine.Shapes = new Shape[] { Shape.Star };
            Text = string.Format("FPS: {0} | Particle Count: {1}", 60, particleEngine.Particles.Count);
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            frameCounter++;
            totalTime += gameTimer.Interval;
            deltaTime = gameTimer.Interval;
            particleEngine.Position = new PointF(particleEngine.Position.X + velocity.X, particleEngine.Position.Y + velocity.Y);
            if (particleEngine.Position.X < 0)
            {
                velocity.X = Math.Abs(velocity.X);
            }
            if (particleEngine.Position.Y < 0)
            {
                velocity.Y = Math.Abs(velocity.Y);
            }
            if (particleEngine.Position.X > ClientSize.Width)
            {
                velocity.X = -Math.Abs(velocity.X);
            }
            if(particleEngine.Position.Y > ClientSize.Height)
            {
                velocity.Y = -Math.Abs(velocity.Y);
            }
            particleEngine.Update(deltaTime);

            rotation++;
            spriteBatch.Clear(Color.White);

            spriteBatch.Begin();

            Bitmap pic = Properties.Resources.KevinsShapes;
            /*spriteBatch.Draw(pic, new PointF(100, ClientSize.Height / 2), new Rectangle(0, 0, 100, 100), Color.White, rotation, new PointF(50, 50), new PointF(1, 1), SpriteEffect.None);
            spriteBatch.Draw(pic, new PointF(250, ClientSize.Height / 2), new Rectangle(100, 0, 100, 100), Color.White, rotation, new PointF(50, 50), new PointF(1, 1), SpriteEffect.None);
            spriteBatch.Draw(pic, new PointF(400, ClientSize.Height / 2), new Rectangle(200, 0, 100, 100), Color.White, rotation, new PointF(50, 50), new PointF(1, 1), SpriteEffect.None);
            spriteBatch.Draw(pic, new PointF(550, ClientSize.Height / 2), new Rectangle(300, 0, 100, 100), Color.White, rotation, new PointF(50, 50), new PointF(1, 1), SpriteEffect.None);
            spriteBatch.Draw(pic, new PointF(700, ClientSize.Height / 2), new Rectangle(400, 0, 100, 100), Color.White, rotation, new PointF(50, 50), new PointF(1, 1), SpriteEffect.None);
            */
            spriteBatch.Draw(Properties.Resources.Trees, new PointF(0, 0), colors[randomGenerator.Next(colors.Length)]);

            //particleEngine.Draw(spriteBatch);
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

        private void GameForm_KeyUp(object sender, KeyEventArgs e)
        {
            keysDown.Remove(e.KeyCode);
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            keysDown.Add(e.KeyCode);
        }

        public bool isKeyDown(Keys key)
        {
            return keysDown.Contains(key);
        }

        public bool isKeyUp(Keys key)
        {
            return !keysDown.Contains(key);
        }
    }
}
