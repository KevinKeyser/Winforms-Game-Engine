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
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            Update();
        }

        protected virtual void Update()
        {
            frameCounter++;
            totalGameTime += gameTimer.Interval;
            deltaGameTime = gameTimer.Interval;
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
            keysDown.Add(e.KeyCode);
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
            spriteBatch.Update(ClientSize);
        }
    }
}
