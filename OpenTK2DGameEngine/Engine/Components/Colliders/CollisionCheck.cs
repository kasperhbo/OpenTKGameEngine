using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MarioGabeKasper.Engine.Components.Colliders
{
    public static class CollisionCheck
    {
        public static bool PointInBox(double x1, double y1, double x2, double y2, double px, double py)
        {
            return px >= x1 && px <= x2 && py >= y1 && py <= y2;
        }
    }
}
