using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KevinKeyserParticleEngine
{
    public class ParticleEngine
    {
        private List<Particle> particles;

        public List<Particle> Particles
        {
            get { return particles; }
            set { particles = value; }
        }

        private PointF location;

        public PointF Location
        {
            get { return location; }
            set { location = value; }
        }

        private int spawnRate;

        public int SpawnRate
        {
            get { return spawnRate; }
            set { spawnRate = value; }
        }

        private int elaspedTime;

        private int spawnAmount;

        public int SpawnAmount
        {
            get { return spawnAmount; }
            set { spawnAmount = value; }
        }

        private PointF minVelocity;

        public PointF MinVelocity
        {
            get { return minVelocity; }
            set { minVelocity = value; }
        }

        private PointF maxVelocity;

        public PointF MaxVelocity
        {
            get { return maxVelocity; }
            set { maxVelocity = value; }
        }

        private Bitmap texture;

        public Bitmap Texture
        {
            get { return texture; }
            set { texture = value; }
        }
        

        private Color[] startColors;

        public Color[] StartColors
        {
            get { return startColors; }
            set { startColors = value; }
        }

        private Color[] endColors;

        public Color[] EndColors
        {
            get { return endColors; }
            set { endColors = value; }
        }

        Random randomGenerator;

        public ParticleEngine(Bitmap texture, PointF location, int spawnRate, int spawnAmount)
        {
            particles = new List<Particle>();
            this.texture = texture;
            this.location = location;
            elaspedTime = 0;
            this.spawnRate = spawnRate;
            this.spawnAmount = spawnAmount;
            minVelocity = new PointF(-1, -1);
            maxVelocity = new PointF(1, 1);
            startColors = new Color[] { Color.White };
            endColors = new Color[] { Color.Transparent };
            randomGenerator = new Random();
        }

        public void Update(int deltaTime)
        {
            elaspedTime += deltaTime;
            if (elaspedTime >= spawnRate)
            {
                elaspedTime = 0;
                for (int i = 0; i < spawnAmount; i++)
                {
                    particles.Add(new Particle(texture, location, randomGenerator.Next(10, 50), startColors[randomGenerator.Next(startColors.Length)], endColors[randomGenerator.Next(endColors.Length)], new PointF((float)randomGenerator.NextDouble() * (maxVelocity.X - minVelocity.X) + minVelocity.X, (float)randomGenerator.NextDouble() * (maxVelocity.Y - minVelocity.Y) + minVelocity.Y), randomGenerator.Next(50, 250)));
                }
            }
            for(int i = 0; i <particles.Count; i++)
            {
                particles[i].Update(deltaTime);
                if (particles[i].IsDead)
                {
                    particles.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Particle particle in particles)
            {
                particle.Draw(spriteBatch);
            }
        }
    }
}
