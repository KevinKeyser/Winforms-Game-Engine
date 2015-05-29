using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KevinKeyserParticleEngine
{
    public class Particle
    {
        private Bitmap texture;

        public Bitmap Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        private PointF location;

        public PointF Location
        {
            get { return location; }
            set { location = value; }
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

        public Particle(Bitmap texture, PointF location, float size, Color startColor, Color endColor, PointF velocity, int life)
        {
            this.texture = texture;
            this.location = location;
            this.size = size;
            color = startColor;
            this.startColor = startColor;
            this.endColor = endColor;
            this.velocity = velocity;
            this.life = life;
            elapsedLife = 0;
            isDead = false;
        }

        public void Update(int deltaTime)
        {
            elapsedLife += deltaTime;
            if (elapsedLife >= life)
            {
                isDead = true;
            }
            location.X += velocity.X;
            location.Y += velocity.Y;
            float colorAmount = elapsedLife / (float)life;
            color = Extensions.Lerp(startColor, endColor, colorAmount);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, location, color);
        }
    }
}
