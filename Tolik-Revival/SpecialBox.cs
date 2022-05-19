using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Tolik_Revival
{
    class SpecialBox
    {
        Random rnd = new Random();
        public Control CreateSpecialBox(Control x)
        {
            (Image image, string tag)[] Boxes = { (Properties.Resources.HealthBox, "HealthBox"), (Properties.Resources.BigBulletBox, "BigBulletBox"), (Properties.Resources.TripleGunBox, "TripleGunBox") };

            return SpecialBoxDesigner(x,Boxes[rnd.Next(0, 3)]);
        }

        private Control SpecialBoxDesigner(Control x, (Image image, string tag) SpecialBoxParam)
        {
            PictureBox SpecialBox = new PictureBox
            {
                Size = new Size(70, 70),
                Top = x.Top,
                Left = x.Left,
                Image = SpecialBoxParam.image,
                Tag = SpecialBoxParam.tag,
                Name = "specialbox"
            };
            return SpecialBox;
        }
    }
}
