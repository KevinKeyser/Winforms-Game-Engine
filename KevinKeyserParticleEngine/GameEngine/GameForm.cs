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

namespace GameEngine
{
    public partial class GameForm : Form
    {
        protected SpriteBatch spriteBatch;
        protected int totalGameTime = 0;
        protected int deltaGameTime = 0;
        protected int fps = 60;
        int frameCounter = 0;
        List<Keys> keysDown = new List<Keys>();

        public GameForm()
        {
            InitializeComponent();
            spriteBatch = new SpriteBatch(ClientSize, Canvas);
            frameTimer.Enabled = true;
            gameTimer.Enabled = true;
        }

        bool runOnce = false;

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            if (!runOnce)
            {
                foreach (var button in Controls.OfType<Button>())
                {
                    button.PreviewKeyDown += button_PreviewKeyDown;
                    button.KeyDown += button_KeyDown;
                }

                runOnce = true;
            }

            Update();
            Draw();
        }

        void button_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            this.OnKeyDown(new KeyEventArgs(e.KeyData));            
        }

        void button_KeyDown(object sender, KeyEventArgs e)
        {
            this.OnKeyDown(e);
        }

        protected virtual void Update()
        {
            frameCounter++;
            totalGameTime += gameTimer.Interval;
            deltaGameTime = gameTimer.Interval;
        }

        protected virtual void Draw()
        {

        }

        private void frameTimer_Tick(object sender, EventArgs e)
        {
            fps = frameCounter;
            frameCounter = 0;
        }

        private void GameForm_KeyUp(object sender, KeyEventArgs e)
        {
            keysDown.Remove(e.KeyCode);
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (!keysDown.Contains(e.KeyCode))
            {
                keysDown.Add(e.KeyCode);
            }
        }

        public bool IsKeyDown(Keys key)
        {
            return keysDown.Contains(key);
        }

        public bool IsKeyUp(Keys key)
        {
            return !keysDown.Contains(key);
        }

        private void GameForm_Resize(object sender, EventArgs e)
        {
            if (spriteBatch != null)
            {
                spriteBatch.Update(ClientSize);
            }
        }
    }
}
