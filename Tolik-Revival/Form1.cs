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
        int score = 0;
        bool direct = true;
        int picCounter = 1;
        Random rnd = new Random();

        void CreatBullet()
        {
            string tag = (direct)?"bulletR":"bulletL";

            PictureBox bullet = new PictureBox
            {
                Size = new Size(8, 8),
                Top = player.Top,
                Left = player.Left,
                Image = Properties.Resources.bullet,
                Tag = tag

            };
            this.Controls.Add(bullet);
        }

        void CreatEnemy(int Top,int left)
        {
            PictureBox enemy1 = new PictureBox
            {
                Size = new Size(70, 70),
                Top=Top-70,
                Left=left,
                Image = Properties.Resources.tolik_terr,
                Tag="Enemy"
            };
            this.Controls.Add(enemy1);
        }

        void Base_move()
        {
            if (player.Bounds.IntersectsWith(ground.Bounds))
            {
                player.Top = ground.Top - player.Height;
                force = 0;
            }
            

            foreach (Control x in this.Controls)
            {

            
                int rnd_Top = 623 - rnd.Next(1, 4) * 150;
                int rnd_Left = rnd.Next(1200, 3000);


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

                            x.Top = rnd_Top;
                            x.Left = rnd_Left;
                            x.Width = rnd.Next(100, 300);

                            if (rnd.Next(0, 10)>5)
                            {
                                CreatEnemy(rnd_Top, rnd_Left);
                            }

                        }

                    }
                }
                if (x is PictureBox && x.Tag == "Enemy")
                {
                    if (right == true && player.Left > 800)
                    {
                        x.Left -= 15;
                    }
                    if (x.Left < -100)
                    {
                        x.Dispose();
                    }
                }
                if (x is PictureBox && (x.Tag == "bulletR" || x.Tag=="bulletL"))
                {
                    if (x.Left>0 && x.Left<1200)
                    {
                        if (x.Tag == "bulletR") x.Left += 30;

                        if (x.Tag == "bulletL")  x.Left -= 30;

                    }
                    else
                    {
                        x.Dispose();
                    }

                    foreach (Control y in this.Controls)
                    {
                        if (y is PictureBox && y.Tag == "Enemy")
                        {
                            if (x.Bounds.IntersectsWith(y.Bounds))
                            {
                                x.Dispose();
                                y.Dispose();
                                score += 5;
                                Score.Text = "Score: " + score.ToString();

                            }

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
                    force = 50;
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
            enemy_Bullets();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            Bullets();
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Right:
                    
                    right = true;
                    direct = true;
                    if (picCounter < 0) picCounter = 0;
                    picCounter++;
                    if (picCounter==1) player.Image = Properties.Resources.Tolik;

                    break;
                case Keys.Left:
                    left = true;
                    player.Image = Properties.Resources.TolikBack;
                    direct = false;
                    if (picCounter > 0) picCounter = 0;
                    picCounter--;
                    if (picCounter == -1) player.Image = Properties.Resources.TolikBack;
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
                case Keys.Space:
                    CreatBullet();
                    break;
            }
        }
    }
}
