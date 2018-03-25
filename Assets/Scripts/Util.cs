using System;
using Unity.Mathematics;

namespace Assets.Scripts
{
    class Util
    {
        public static float SquareDistance(float3 a, float3 b)
        {
            return (float) (Math.Pow(a.x - b.x, 2) + Math.Pow(a.y - b.y, 2) + Math.Pow(a.y - b.y, 2));
        }
    }
}
