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
        int groundCounter = 0;
        int walkspeed = 20;

        (int counter, int max, int speed) upParam = (0, 5, 20);
        (int counter, int max) platformParam = (0, 20);
        (int probability, int damage) enemyParam = (50, 5);

        int score = 0;
        int healthPoints = 100;
        int specialBullet = 0;


        Random rnd = new Random();

        Gun Gun = new Gun();
        Enemy Enemy = new Enemy();
        Platform Platform = new Platform();
        SpecialBox SpecialBox = new SpecialBox();

        void Shoot(Control x, bool directF = false) 
        {
            if (specialBullet==0)
            {
                player.Tag = "";
            }
            else if (x.Name==player.Name)
            {
                specialBullet--;
                StatusUpdate();
            }
            this.Controls.AddRange(Gun.Shoot(x, directF)); 
        }

        void CreateElements()
        {
            if (platformParam.counter < platformParam.max)
            {
                (int top, int left) coord = ( 623 - rnd.Next(1, 4) * 150, rnd.Next(1200, 3000));
                this.Controls.AddRange(new Control[] { Platform.CreatePlatform(coord), (rnd.Next(0, 100) < enemyParam.probability) ? Enemy.CreateEnemy(coord) : null });
                platformParam.counter++;
            }
        }


        void MovePlayer()
        {
            if (player.Top > 800) GameOver();
            if (right == true && player.Left < 900) player.Left += walkspeed;
            if (left == true && player.Left > 20) player.Left -= walkspeed;

            if (up == true && upParam.counter < upParam.max)
            {
                player.Top -= upParam.speed;
                attractionForce = 30;
                upParam.counter++;
            }
            if (upParam.counter == upParam.max) up = false;
            if (up == false) player.Top += attractionForce;
        }

        void MovePlatform(Control x)
        {
            if (player.Bounds.IntersectsWith(x.Bounds))
            {
                player.Top = x.Top - player.Height;
                upParam.counter = 0;
            }

            if (right == true && player.Left > 800)
            {
                x.Left -= walkspeed;
            }

            if (x.Left < -200 && x.Tag.ToString() == "base")
            {
                x.Dispose();
                platformParam.counter--;
            }

            if ((x.Left + x.Width) < 100 && x.Tag.ToString() == "ground")
            {
                x.Dispose();
                groundCounter--;
            }

            if (groundCounter < 2 && (x.Left+x.Width) == 1300)
            {
                this.Controls.AddRange(new Control[] { Platform.CreateGround(), Enemy.CreateEnemy((623, 2000)) });
                
                groundCounter++;
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
                StatusUpdate();
            }

            (x as PictureBox).Image = !(x.Left < player.Left) ? Properties.Resources.tolik_terr : Properties.Resources.TolikTerrBack;

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

        void MoveSpecialBox(Control x)
        {
            if (right == true && player.Left > 800)
            {
                x.Left -= walkspeed;
            }
            if (x.Left < -100)
            {
                x.Dispose();
            }
            if (x.Bounds.IntersectsWith(player.Bounds)) TakeSpecialBox(x);
        }


        void KillEnemy(Control x)
        {
            foreach (Control y in this.Controls)
            {
                if (y is PictureBox && y.Name == "enemy")
                {
                    if (x.Bounds.IntersectsWith(y.Bounds))
                    {         
                        if (rnd.Next(0, 100) > enemyParam.probability)
                        {
                            this.Controls.Add(SpecialBox.CreateSpecialBox(y));
                        }

                        if (!(x.Size == new Size(70, 70))) x.Dispose();

                        y.Dispose();
                        score += 5;
                        StatusUpdate();
                    }
                }
            }
        }

        void DamagePlayer(Control x)
        {
            if (x.Name == "enemy") healthPoints -= enemyParam.damage* 2;
            if (x.Name == "bulletenemy") healthPoints -= enemyParam.damage;

            StatusUpdate();

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
                if (x is PictureBox && x.Name == "enemy" && x.Top == player.Top && x.Left < 1130)
                {
                    bool enemyDirect = !(player.Left < x.Left);
                    Shoot(x, enemyDirect);
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
                    if (x.Name == "specialbox") MoveSpecialBox(x);
                }
            }
        }


        void StatusUpdate()
        {
            SB.Text = "Special bullet: " + specialBullet;
            Score.Text = "Score: " + score;
            HP.Text = "HP: " + healthPoints;
            DifficultyLevel();
        }

        void DifficultyLevel()
        {
            enemyParam.damage = 5 + (score / 100);
            enemyParam.probability = 50 + (score / 100);
            if (timer2.Interval > 100) timer2.Interval = 1000 - (score / 2);
        }

        void TakeSpecialBox(Control x)
        {
            if (x.Tag.ToString() == "HealthBox")
            {
                healthPoints += (healthPoints + 30 > 100) ? 100 - healthPoints : 30;

                StatusUpdate();
                x.Dispose();
            }

            if (x.Tag.ToString() == "BigBulletBox" || x.Tag.ToString() == "TripleGunBox")
            {
                specialBullet += 3;
                StatusUpdate();
                player.Tag = x.Tag;
                x.Dispose();
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
                    Shoot(player, direct);
                    break;
            }
        }
    }
}