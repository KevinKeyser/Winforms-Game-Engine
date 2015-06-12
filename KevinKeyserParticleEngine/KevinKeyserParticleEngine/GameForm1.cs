using GameEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KevinKeyserParticleEngine
{
    public partial class GameForm1 : GameForm
    {
        ParticleEngine particleEngine;
        PointF velocity;
        Random randomGenerator;
        Color[] colors;

        float paddle1y;
        float paddle2y;

        int score1 = 0;
        int score2 = 0;

        RectangleF paddle1Bounds;
        RectangleF paddle2Bounds;

        Font font = new Font("Arial", 12);

        public GameForm1()
        {
            InitializeComponent();

            randomGenerator = new Random();
            colors = new Color[] { Extensions.Lerp(Color.Red, Color.Transparent, 0f) };//, Color.Purple, Color.Black };

            velocity = new PointF(2, 2);
            particleEngine = new ParticleEngine(new PointF(ClientSize.Width / 2, ClientSize.Height / 2), 1, 100);
            particleEngine.RandomGenerator = randomGenerator;
            particleEngine.MinVelocity = new PointF(-.5f, -.5f);
            particleEngine.MaxVelocity = new PointF(.5f, .5f);
            particleEngine.StartSize = 1;
            particleEngine.EndSize = 10;
            particleEngine.StartColors = new Color[] { Color.LightBlue, Color.Turquoise, Color.AliceBlue, Color.CadetBlue, Color.BlueViolet, Color.CornflowerBlue };
            particleEngine.EndColors = new Color[] { Color.Transparent };
            particleEngine.Shapes = new Shape[] { Shape.Star, Shape.Circle, Shape.Triangle };
            Text = string.Format("FPS: {0} | Particle Count: {1}", 60, particleEngine.Particles.Count);

            paddle1y = ClientSize.Height / 2;
            paddle2y = ClientSize.Height / 2;
        }

        protected override void Update()
        {
            particleEngine.Position = new PointF(particleEngine.Position.X + velocity.X, particleEngine.Position.Y + velocity.Y);
            if (particleEngine.Position.X < 0)
            {
                score2++;
                particleEngine.Position = new PointF(ClientSize.Width / 2, ClientSize.Height / 2);
            }
            if (particleEngine.Position.Y < 0)
            {
                velocity.Y = Math.Abs(velocity.Y);
            }
            if (particleEngine.Position.X > ClientSize.Width)
            {
                score1++;
                particleEngine.Position = new PointF(ClientSize.Width / 2, ClientSize.Height / 2);
            }
            if (particleEngine.Position.Y > ClientSize.Height)
            {
                velocity.Y = -Math.Abs(velocity.Y);
            }

            paddle1Bounds = new RectangleF(new PointF(0, paddle1y - 50), new SizeF(20, 100));
            paddle2Bounds = new RectangleF(new PointF(ClientSize.Width - 20, paddle2y - 50), new SizeF(20, 100));
            if (paddle1Bounds.Contains(particleEngine.Position))
            {
                velocity.X = Math.Abs(velocity.X);
            }
            if (paddle2Bounds.Contains(particleEngine.Position))
            {
                velocity.X = -Math.Abs(velocity.X);
            }

            if (IsKeyDown(Keys.W))
            {
                paddle1y -= 2;
            }
            if (IsKeyDown(Keys.S))
            {
                paddle1y += 2;
            }
            if (IsKeyDown(Keys.Up))
            {
                paddle2y -= 2;
            }
            if (IsKeyDown(Keys.Down))
            {
                paddle2y += 2;
            }
            paddle1y = Extensions.Clamp(paddle1y, 50, ClientSize.Height - 50);
            paddle2y = Extensions.Clamp(paddle2y, 50, ClientSize.Height - 50);

            particleEngine.Update(deltaGameTime);

            Text = string.Format("FPS: {0} | Particle Count: {1}", fps, particleEngine.Particles.Count);

            base.Update();
        }

        protected override void Draw()
        {
            spriteBatch.Clear(Color.White);

            spriteBatch.Begin();

            spriteBatch.Draw(Properties.Resources.Trees, new RectangleF(0,0,ClientSize.Width, ClientSize.Height), Color.Red);

            spriteBatch.Draw(Properties.Resources.KevinsShapes, new PointF(10, paddle1y), new Rectangle(0, 0, 100, 100), Color.Blue, 0, new PointF(50, 50), new PointF(.2f, 1), SpriteEffect.None);
            spriteBatch.Draw(Properties.Resources.KevinsShapes, new PointF(ClientSize.Width - 10, paddle2y), new Rectangle(0, 0, 100, 100), Color.Blue, 0, new PointF(50, 50), new PointF(.2f, 1), SpriteEffect.None);

            particleEngine.Draw(spriteBatch);

            spriteBatch.DrawString(font, score1.ToString(), new PointF(25, 25), Color.Black);
            spriteBatch.DrawString(font, score2.ToString(), new PointF(ClientSize.Width - 25, 25), Color.Black);


            spriteBatch.End();

            base.Draw();
        }
    }
}
