using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class ParticleEngine
    {
        private List<Particle> particles;

        public List<Particle> Particles
        {
            get { return particles; }
            set { particles = value; }
        }

        private PointF position;

        public PointF Position
        {
            get { return position; }
            set { position = value; }
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

        private Shape[] shapes;

        public Shape[] Shapes
        {
            get { return shapes; }
            set { shapes = value; }
        }

        private float startSize;

        public float StartSize
        {
            get { return startSize; }
            set { startSize = value; }
        }

        private float endSize;

        public float EndSize
        {
            get { return endSize; }
            set { endSize = value; }
        }

        private Random randomGenerator;

        public Random RandomGenerator
        {
            get { return randomGenerator; }
            set { randomGenerator = value; }
        }

        private int minLife;

        public int MinLife
        {
            get { return minLife; }
            set { minLife = value; }
        }

        private int maxLife;

        public int MaxLife
        {
            get { return maxLife; }
            set { maxLife = value; }
        }

        public ParticleEngine(PointF location, int spawnRate, int spawnAmount)
        {
            particles = new List<Particle>();
            shapes = new Shape[] { Shape.Circle };
            this.position = location;
            elaspedTime = 0;
            this.spawnRate = spawnRate;
            this.spawnAmount = spawnAmount;
            minVelocity = new PointF(-1, -1);
            maxVelocity = new PointF(1, 1);
            startColors = new Color[] { Color.White };
            endColors = new Color[] { Color.Transparent };
            randomGenerator = new Random();
            startSize = 1;
            endSize = 40;
            minLife = 10;
            maxLife = 40;
        }

        public void Update(int deltaGameTime)
        {
            elaspedTime += deltaGameTime;
            if (elaspedTime >= spawnRate)
            {
                elaspedTime = 0;
                for (int i = 0; i < spawnAmount; i++)
                {
                    particles.Add(new Particle(shapes[randomGenerator.Next(shapes.Length)], position, startSize, endSize, startColors[randomGenerator.Next(startColors.Length)], endColors[randomGenerator.Next(endColors.Length)], new PointF((float)randomGenerator.NextDouble() * (maxVelocity.X - minVelocity.X) + minVelocity.X, (float)randomGenerator.NextDouble() * (maxVelocity.Y - minVelocity.Y) + minVelocity.Y), randomGenerator.Next(minLife, maxLife)));
                }
            }
            for(int i = 0; i < particles.Count; i++)
            {
                particles[i].Update(deltaGameTime);
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
