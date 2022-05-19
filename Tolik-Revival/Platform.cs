using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
namespace Tolik_Revival
{
    class Platform
    {
        Random rnd = new Random();

        public Control CreatePlatform((int top, int left) coord, string tag = "base")
        {
            (int top, int left, int width, string tag) platformParam = (coord.top, coord.left, rnd.Next(100, 300), tag);
            return PlatformDesigner(platformParam);
        }

        public Control CreateGround()
        {
            (int top, int left, int width, string tag) platformParam = (623, 1500, 1500, "ground");
            return PlatformDesigner(platformParam);
        }

        private Control PlatformDesigner((int top, int left, int width, string tag) platformParam)
        {
            PictureBox platform = new PictureBox
            {
                Size = new Size(platformParam.width, 30),
                Top = platformParam.top,
                Left = platformParam.left,
                Image = Properties.Resources.ground,
                Tag = platformParam.tag,
                Name = "platform",
            };
            return platform;
        }
    }
}
