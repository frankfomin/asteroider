using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace asteroider { 
    abstract class GameObject : IGameObject
    {
        public bool IsDead { get; set; }
        public Vector2 Position { get; set; }
        public float Radius { get; set; }
        public Vector2 Speed { get; set; }
        public float Rotation { get; set; }

        public bool CollidesWith(IGameObject other)
        {
            return (this.Position - other.Position).LengthSquared() <
                (Radius + other.Radius) * (Radius + other.Radius);
        }
    }
}
