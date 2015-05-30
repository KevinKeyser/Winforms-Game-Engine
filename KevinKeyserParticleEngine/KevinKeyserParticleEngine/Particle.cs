using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KevinKeyserParticleEngine
{
    public enum Shape
    {
        Circle,
        Square,
        Star,
        Triangle
    }

    public class Particle
    {
        private Shape shape;

        public Shape Shape
        {
            get { return shape; }
            set { shape = value; }
        }

        private PointF position;

        public PointF Position
        {
            get { return position; }
            set { position = value; }
        }

        private float size;

	    public float Size
	    {
		    get { return size;}
		    set { size = value;}
	    }
	
        private Color color;

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        private Color startColor;

        public Color StartColor
        {
            get { return startColor; }
            set { startColor = value; }
        }

        private Color endColor;

        public Color EndColor
        {
            get { return endColor; }
            set { endColor = value; }
        }
        
        private PointF velocity;

        public PointF Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        private int life;

        public int Life
        {
            get { return life; }
            set { life = value; }
        }

        private int elapsedLife;

        private bool isDead;

        public bool IsDead
        {
            get { return isDead; }
            set { isDead = value; }
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
        
        public Particle(Shape shape, PointF location, float startSize, float endSize, Color startColor, Color endColor, PointF velocity, int life)
        {
            this.shape = shape;
            this.position = location;
            this.size = startSize;
            color = startColor;
            this.startColor = startColor;
            this.endColor = endColor;
            this.velocity = velocity;
            this.life = life;
            elapsedLife = 0;
            isDead = false;
            this.startSize = startSize;
            this.endSize = endSize;
        }

        public void Update(int deltaTime)
        {
            elapsedLife += deltaTime;
            if (elapsedLife >= life)
            {
                isDead = true;
            }
            position.X += velocity.X;
            position.Y += velocity.Y;
            float amount = elapsedLife / (float)life;
            color = Extensions.Lerp(startColor, endColor, amount);
            size = Extensions.Lerp(startSize, endSize, amount);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (shape)
	        {
		        case Shape.Circle:
                    spriteBatch.Graphics.FillEllipse(new SolidBrush(color), position.X, position.Y, size, size);
                    break;
                case Shape.Square:
                    spriteBatch.Graphics.FillRectangle(new SolidBrush(color), position.X, position.Y, size, size);
                    break;
                case Shape.Star:
                    System.Diagnostics.Debugger.Log(1, "Rendering", "Shape.Star is not supported yet.");
                    break;
                case Shape.Triangle:
                    System.Diagnostics.Debugger.Log(1, "Rendering", "Shape.Triangle is not supported yet.");
                    break;
                default:
                    System.Diagnostics.Debugger.Log(1, "Rendering", "Unknown Shape");
                    break;
	        }
        }
    }
}
