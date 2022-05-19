using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Tolik_Revival
{
    class Enemy
    {
        public Control CreateEnemy((int top,int left) coord)
        {
            PictureBox enemy = new PictureBox
            {
                Size = new Size(70, 70),
                Top = coord.top - 70,
                Left = coord.left+35,
                Image = Properties.Resources.tolik_terr,
                Tag = "",
                Name = "enemy",
                SizeMode = PictureBoxSizeMode.CenterImage
            };
            return enemy;
        }
    }
}
