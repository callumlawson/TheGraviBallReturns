using JetBrains.Annotations;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

namespace Assets.Scripts
{
    [UsedImplicitly]
    public sealed class GraviballBootstrap
    {
        public const int NumChunks = 1000;

        public static EntityArchetype ChunkArchetype;
        public static MeshInstanceRenderer ChunkLook;

        private static GraviballSettings Settings;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        [UsedImplicitly]
        public static void Initialize()
        {
            // This method creates archetypes for entities we will spawn frequently in this game.
            // Archetypes are optional but can speed up entity spawning substantially.
            var entityManager = World.Active.GetOrCreateManager<EntityManager>();
            // Create player archetype
            ChunkArchetype = entityManager.CreateArchetype(typeof(TransformMatrix), typeof(Position), typeof(VelocityData), typeof(AccelerationData));
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        [UsedImplicitly]
        public static void InitializeWithScene()
        {
            var settingsGo = GameObject.Find("Settings");
            Settings = settingsGo?.GetComponent<GraviballSettings>();
            if (!Settings)
                return;
            ChunkLook = GetLookFromPrototype("Chunk");
            NewGame();
        }

        // Begin a new game.
        public static void NewGame()
        {
            // Access the ECS entity manager
            var entityManager = World.Active.GetOrCreateManager<EntityManager>();

            //TODO: Use BatchAPI
            for (var i = 0; i < 500000; i++)
            {
                var spherePosition = Random.onUnitSphere * (Random.value * 30 + 5);
                var chunk = entityManager.CreateEntity(ChunkArchetype);
                entityManager.SetComponentData(chunk, new Position { Value = new float3(spherePosition.x, spherePosition.y, spherePosition.z) });
                entityManager.SetComponentData(chunk, new VelocityData { Value = new float3(2.0f, 0, 2.0f) });
                //entityManager.AddSharedComponentData(chunk, ChunkLook);
                entityManager.AddSharedComponentData(chunk, new FocusPosition());
            }
        }

        private static MeshInstanceRenderer GetLookFromPrototype(string protoName)
        {
            var proto = GameObject.Find(protoName);
            var result = proto.GetComponent<MeshInstanceRendererComponent>().Value;
            Object.Destroy(proto);
            return result;
        }
    }
}
