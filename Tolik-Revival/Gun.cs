using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Tolik_Revival
{
    class Gun
    {
        private int bulletSpeed = 30;
        private int positionComp = 40;

        public Control[] Shoot(Control shooter, bool direct = false)
        {
            int directInt = (direct) ? 1 : -1;
            Control[] Gun;

            switch (shooter.Tag)
            {
                case "BigBulletBox":
                    Gun = BigBullet(shooter, directInt);
                    break;
                case "TripleGunBox":
                    Gun = TripleBullet(shooter, directInt);
                    break;
                default:
                    Gun = RegularBullet(shooter, directInt);
                    break;
            }

            return Gun;
        }

        private Control[] RegularBullet(Control shooter, int direct)
        {
            string tag = string.Format("0|{0}", bulletSpeed * direct);
            return new Control[] { BulletDesigner(shooter, (new Size(8, 8), Properties.Resources.bullet, tag, direct, 30)) };
        }

        private Control[] BigBullet(Control shooter, int direct)
        {
            string tag = string.Format("0|{0}", bulletSpeed * direct);
            return new Control[] { BulletDesigner(shooter, (new Size(70, 70), Properties.Resources.BigBullet, tag, direct, 0)) };
        }

        private Control[] TripleBullet(Control shooter, int direct)
        {
            List<Control> bullets= new List<Control>();

            for (int i = -8; i < 9; i+=8)
            {
                string tag = string.Format("{0}|{1}", i.ToString(), bulletSpeed * direct);
                bullets.Add(BulletDesigner(shooter, (new Size(8, 8), Properties.Resources.bullet, tag, direct, 30)));
            }
            return bullets.ToArray();
        }

        private Control BulletDesigner(Control shooter, (Size size, Image image, string tag, int direct, int top) bulletParam)
        {        
            PictureBox bullet = new PictureBox
            {
                Size = bulletParam.size,
                Top = shooter.Top + bulletParam.top,
                Left = shooter.Left + (positionComp*bulletParam.direct),
                Image = bulletParam.image,
                Tag = bulletParam.tag,
                Name = "bullet" + shooter.Name
            };
            return bullet;
        }
    }
}
