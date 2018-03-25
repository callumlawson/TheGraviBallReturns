//using JetBrains.Annotations;
//using Unity.Entities;
//using Unity.Mathematics;
//using Unity.Transforms;
//using UnityEngine;
//
//namespace Assets.Scripts.Systems
//{
//    [UsedImplicitly]
//    class GravityApplicationSystem : ComponentSystem
//    {
//        public float3 CenterPosition = new float3(0, 0, 0);
//        public float GravityStrength = 400.0f;
//        public float GravityHorizon = 5.0f;
//
//        public struct FocusData
//        {
//            public int Length;
//            public ComponentDataArray<Position> Position;
//            public ComponentDataArray<VelocityData> VelocityData;
//            public ComponentDataArray<AccelerationData> AccelerationData;
//        }
//
//        [Inject] private FocusData focii;
//
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
//    }
//}
