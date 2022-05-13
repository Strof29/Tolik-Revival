using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tolik_Revival
{
    public partial class GameForm : Form
    {
        public GameForm()
        {
            InitializeComponent();
        }

        bool right, left, up;
        int speed = 20;
        int force = 20;
        int upCounter = 0;
        int jump = 5;
        int walkspeed = 20;
        Random rnd = new Random();

        void Base_move()
        {
            if (player.Bounds.IntersectsWith(ground.Bounds))
            {
                player.Top = ground.Top - player.Height;
                force = 0;
            }

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && x.Tag == "base")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        player.Top = x.Top - player.Height;
                    }

                    if (right == true && player.Left > 800)
                    {
                        x.Left -= 15;

                        if (x.Left < -200)
                        {
                            x.Top = 623 - rnd.Next(1, 4) * 150;
                            x.Left = rnd.Next(1200, 3000);
                            x.Width = rnd.Next(100, 300);
                        }

                    }
                }
            }
        }

        void Player_move()
        {
            if (right == true) if (player.Left < 900) player.Left += walkspeed;
            if (left == true) if (player.Left > 20) player.Left -= walkspeed;

            if (up == true && upCounter < jump)
            {
                if (player.Top > 50)
                {
                    player.Top -= speed;
                    force = 30;
                }
                upCounter++;
            }
            if (upCounter == jump) up = false;
            if (up == false) player.Top += force;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Player_move();
            Base_move();
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Right:
                    player.Image = Properties.Resources.Tolik;
                    right = true;
                    break;
                case Keys.Left:
                    left = true;
                    player.Image = Properties.Resources.TolikBack;
                    break;
                case Keys.Up:
                    up = true;
                    break;
            }
        }

        private void GameForm_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Right:
                    right = false;
                    break;
                case Keys.Left:
                    left = false;
                    break;
                case Keys.Up:
                    up = false;
                    upCounter = 0;
                    break;
            }
        }
    }
}
