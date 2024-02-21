using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace asteroider
{
    internal class Globals
    {
        public static int ScreenWidth = 1280;
        public static int ScreenHeight = 720;

        public static Rectangle GameArea
        {
            get
            {
                return new Rectangle(-80, -80, ScreenWidth + 160,
                    ScreenHeight + 160);
            }
        }

        public static Rectangle RespawnArea
        {
            get
            {
                return new Rectangle((int)CenterScreen.X - 200,
                    (int)CenterScreen.Y - 200, 400, 400);
            }
        }

        public static Vector2 CenterScreen
        {
            get
            {
                return new Vector2(ScreenHeight / 2,
                ScreenHeight / 2);
            }
        }
    }
}
