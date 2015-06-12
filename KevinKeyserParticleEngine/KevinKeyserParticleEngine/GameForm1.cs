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
        float rotation = 0;
        PointF velocity;
        Random randomGenerator;
        Color[] colors;

        float paddle1y;
        float paddle2y;

        public GameForm1()
        {
            InitializeComponent();

            randomGenerator = new Random();
            colors = new Color[] { Extensions.Lerp(Color.Red, Color.Transparent, 0f) };//, Color.Purple, Color.Black };

            velocity = new PointF(5, 5);
            particleEngine = new ParticleEngine(new PointF(ClientSize.Width / 2, ClientSize.Height / 2), 1, 100);
            particleEngine.RandomGenerator = randomGenerator;
            particleEngine.MinVelocity = new PointF(-1, -1);
            particleEngine.MaxVelocity = new PointF(1, 1);
            particleEngine.StartSize = 1;
            particleEngine.EndSize = 10;
            particleEngine.StartColors = new Color[] { Color.LightBlue, Color.Turquoise, Color.AliceBlue, Color.CadetBlue, Color.BlueViolet, Color.CornflowerBlue };
            particleEngine.EndColors = new Color[] { Color.Transparent };
            particleEngine.Shapes = new Shape[] { Shape.Star };
            Text = string.Format("FPS: {0} | Particle Count: {1}", 60, particleEngine.Particles.Count);

            paddle1y = ClientSize.Height / 2;
            paddle2y = ClientSize.Height / 2;
        }

        protected override void Update()
        {
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
            if (particleEngine.Position.Y > ClientSize.Height)
            {
                velocity.Y = -Math.Abs(velocity.Y);
            }
            particleEngine.Update(deltaGameTime);

            rotation++;
            spriteBatch.Clear(Color.White);

            spriteBatch.Begin();

            Bitmap pic = Properties.Resources.KevinsShapes;
            spriteBatch.Draw(pic, new PointF(0, paddle1y), new Rectangle(0, 0, 100, 100), Color.Blue, 0, new PointF(0, 0), new PointF(.1f, 1), SpriteEffect.None);
            spriteBatch.Draw(pic, new PointF(ClientSize.Width - 10, paddle2y), new Rectangle(0, 0, 100, 100), Color.Blue, 0, new PointF(0, 0), new PointF(.1f, 1), SpriteEffect.None);
            //spriteBatch.Draw(pic, new PointF(250, ClientSize.Height / 2), new Rectangle(100, 0, 100, 100), Color.Blue, rotation, new PointF(50, 50), new PointF(1, 1), SpriteEffect.None);
            //spriteBatch.Draw(pic, new PointF(400, ClientSize.Height / 2), new Rectangle(200, 0, 100, 100), Color.Blue, rotation, new PointF(50, 50), new PointF(1, 1), SpriteEffect.None);
            //spriteBatch.Draw(pic, new PointF(550, ClientSize.Height / 2), new Rectangle(300, 0, 100, 100), Color.Blue, rotation, new PointF(50, 50), new PointF(1, 1), SpriteEffect.None);
            //spriteBatch.Draw(pic, new PointF(700, ClientSize.Height / 2), new Rectangle(400, 0, 100, 100), Color.Blue, rotation, new PointF(50, 50), new PointF(1, 1), SpriteEffect.None);
            
            //spriteBatch.Draw(Properties.Resources.Trees, new PointF(0, 0), colors[randomGenerator.Next(colors.Length)]);

            particleEngine.Draw(spriteBatch);
            spriteBatch.End();

            Text = string.Format("FPS: {0} | Particle Count: {1}", fps, particleEngine.Particles.Count);

            base.Update();
        }
    }
}
