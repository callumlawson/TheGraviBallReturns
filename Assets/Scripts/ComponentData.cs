using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Assets.Scripts
{
    public struct FocusPosition : ISharedComponentData
    {
        public Position position;
    }

    public struct VelocityData : IComponentData
    {
        public float3 Value;
    }

    public struct AccelerationData : IComponentData
    {
        public float3 Value;
    }
}
