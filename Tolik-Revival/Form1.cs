using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Tolik_Revival
{
    public partial class GameForm : Form
    {
        public GameForm()
        {
            InitializeComponent();
        }

        bool right, left, up;
        bool direct = true;

        int attractionForce = 20;

        int upCounter = 0;
        int upMax = 5;
        int upSpeed = 20;

        int platformCounter = 0;
        int platformMax = 20;

        int score = 0;
        int walkspeed = 20;

        int enemyProbability = 50;

        int healthPoints = 100;


        Random rnd = new Random();



        void NewBullet(Control x, bool directF = false)
        {
            string tag = (directF) ? "0|30" : "0|-30";
            int positionComp = (directF) ? 40 : -40;

            PictureBox bullet = new PictureBox
            {
                Size = new Size(8, 8),
                Top = x.Top + 30,
                Left = x.Left + positionComp,
                Image = Properties.Resources.bullet,
                Tag = tag,
                Name = "bullet" + x.Name
            };
            this.Controls.Add(bullet);
        }

        void NewEnemy(int[] coord)
        {
            if (rnd.Next(0, 100) > enemyProbability)
            {
                PictureBox enemy = new PictureBox
                {
                    Size = new Size(70, 70),
                    Top = coord[0] - 70,
                    Left = coord[1],
                    Image = Properties.Resources.tolik_terr,
                    Tag = "",
                    Name = "enemy",
                    SizeMode = PictureBoxSizeMode.CenterImage
                };
                this.Controls.Add(enemy);
            }
        }

        void NewPlatform(int[] coord)
        {
            PictureBox platform = new PictureBox
            {
                Size = new Size(rnd.Next(100, 300), 30),
                Top = coord[0],
                Left = coord[1],
                Image = Properties.Resources.ground,
                Tag = "base",
                Name = "platform",
            };
            this.Controls.Add(platform);
        }

        void CreateElements()
        {
            if (platformCounter < platformMax)
            {
                int[] coord = { 623 - rnd.Next(1, 4) * 150, rnd.Next(1200, 3000) };
                NewPlatform(coord);
                NewEnemy(coord);
                platformCounter++;
            }
        }


        void MovePlayer()
        {
            if (right == true && player.Left < 900) player.Left += walkspeed;
            if (left == true && player.Left > 20) player.Left -= walkspeed;

            if (up == true && upCounter < upMax)
            {
                player.Top -= upSpeed;
                attractionForce = 30;
                upCounter++;
            }
            if (upCounter == upMax) up = false;
            if (up == false) player.Top += attractionForce;
        }

        void MovePlatform(Control x)
        {
            if (player.Bounds.IntersectsWith(x.Bounds))
            {
                player.Top = x.Top - player.Height;
                upCounter = 0;
            }

            if (right == true && player.Left > 800)
            {
                x.Left -= walkspeed;
            }

            if (x.Left < -200)
            {
                x.Dispose();
                platformCounter--;
            }
        }

        void MoveEnemy(Control x)
        {
            if (right == true && player.Left > 800)
            {
                x.Left -= walkspeed;
            }
            if (x.Left < -100)
            {
                x.Dispose();
            }
            if (x.Bounds.IntersectsWith(player.Bounds))
            {
                DamagePlayer(x);
                x.Dispose();
                score += 5;
                Score.Text = "Score: " + score.ToString();
            }

            if (x.Top == player.Top)
            {
                (x as PictureBox).Image = !(x.Left < player.Left) ? Properties.Resources.tolik_terr : Properties.Resources.Tolik;
            }

        }

        void MoveBullet(Control x)
        {
            if (x.Left > 0 && x.Left < 1200)
            {
                int[] coord = x.Tag.ToString().Split('|').Select(x => int.Parse(x)).ToArray();
                x.Top += coord[0];
                x.Left += coord[1];
            }
            else
            {
                x.Dispose();
            }
            if (x.Bounds.IntersectsWith(player.Bounds) && (x.Name == "bulletenemy"))
            {
                DamagePlayer(x);
                x.Dispose();
            }
            if (x.Name == "bulletplayer") KillEnemy(x);
        }


        void KillEnemy(Control x)
        {
            foreach (Control y in this.Controls)
            {
                if (y is PictureBox && y.Name == "enemy")
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

        void DamagePlayer(Control x)
        {
            if (x.Name == "enemy") healthPoints -= 10;
            if (x.Name == "bulletenemy") healthPoints -= 5;

            HP.Text = "HP: " + healthPoints;
            if (healthPoints <= 0)
            {
                GameOver();
            }
        }

        void GameOver()
        {
            this.Controls.Clear();

            string text = "GAME OVER\nYOUR SCORE: " + score;

            Label gameover = new Label
            {
                Text = text,
                Font = new Font("Unispace", 30, FontStyle.Bold),
                ForeColor = Color.Red,
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter

            };

            this.Controls.Add(gameover);
        }

        void EnemyShooting()
        {
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && x.Name == "enemy" && x.Top == player.Top && x.Left < 1200)
                {
                    bool enemyDirect = !(player.Left < x.Left);
                    NewBullet(x, enemyDirect);
                }
            }
        }

        void ElementProcessing()
        {
            CreateElements();
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox)
                {
                    if (x.Name == "platform") MovePlatform(x);
                    if (x.Name == "enemy") MoveEnemy(x);
                    if (x.Name == "bulletplayer" || x.Name == "bulletenemy") MoveBullet(x);
                }
            }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            MovePlayer();
            ElementProcessing();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            EnemyShooting();
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Right:
                    right = true;
                    direct = true;
                    player.Image = Properties.Resources.Tolik;

                    break;
                case Keys.Left:
                    left = true;
                    direct = false;
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
                    break;
                case Keys.Space:
                    NewBullet(player, direct);
                    break;
            }
        }
    }
}