using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asteroider
{
    internal class Shot: GameObject 
    {
     
        public Shot()
        {
            Radius = 16;
        }

        public void Update(GameTime gameTime)
        {
            Position += Speed;
            Rotation += 0.08f;

            if (Rotation > MathHelper.TwoPi)
                Rotation = 0;
        }
    
    }
}
