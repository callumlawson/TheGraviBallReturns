using JetBrains.Annotations;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    [UsedImplicitly]
    class GravityApplicationJobSystem : JobComponentSystem
    {
//        public float GravityStrength = 400.0f;
//        public float GravityHorizon = 5.0f;

        public struct FocusData
        {
            public int Length;
            public ComponentDataArray<Position> Position;
            public ComponentDataArray<VelocityData> VelocityData;
        }

        [Inject] private FocusData focii;

//        protected override void OnUpdate()
//        {
//            var deltaTime = Time.deltaTime;
//
//            for (int i = 0; i < focii.Length; i++)
//            {
//                //calculate acceleration
//                //get distance to center
//                var squareDistanceToCenter = Util.SquareDistance(focii.Position[i].Value, CenterPosition);
//                var directionToCenter = CenterPosition - focii.Position[i].Value;
//                var normalizedVectorToCenter = directionToCenter / Mathf.Sqrt(squareDistanceToCenter);
//                var acceleration = (GravityStrength / Mathf.Max(squareDistanceToCenter, GravityHorizon)) * normalizedVectorToCenter;
//
//                //calculate velocity: v = u + at
//                focii.VelocityData[i] = new VelocityData{Value = focii.VelocityData[i].Value + acceleration * deltaTime};
//
//                //calculate new position - only velocity term: s = s' + vt
//                focii.Position[i] = new Position{Value = focii.Position[i].Value + focii.VelocityData[i].Value * deltaTime};
//            }
//        }

        [ComputeJobOptimization]
        private struct PositionUpdateJob : IJobParallelFor
        {
            public ComponentDataArray<VelocityData> velocities;
            public ComponentDataArray<Position> positions;
            public float deltaTime;

            public void Execute(int i)
            {
                //calculate acceleration
                //get distance to center
                var squareDistanceToCenter = Util.SquareDistance(positions[i].Value, new float3());
                var directionToCenter = -positions[i].Value;
                var normalizedVectorToCenter = directionToCenter / Mathf.Sqrt(squareDistanceToCenter);
                var acceleration = (400.0f / Mathf.Max(squareDistanceToCenter, 5.0f)) * normalizedVectorToCenter;

                //calculate velocity: v = u + at
                velocities[i] = new VelocityData{Value = velocities[i].Value + acceleration * deltaTime};
                
                //calculate new position - only velocity term: s = s' + vt
                positions[i] = new Position{Value = positions[i].Value + velocities[i].Value * deltaTime};
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var job = new PositionUpdateJob()
            {
                velocities = focii.VelocityData,
                positions = focii.Position,
                deltaTime = Time.deltaTime
            }.Schedule(focii.Length, 512, inputDeps);
            return job;
        }
    }
}
