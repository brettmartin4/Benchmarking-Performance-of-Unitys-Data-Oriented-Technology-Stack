using System.Collections;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
//using Unity.Physics;
using Unity.Transforms;
using Random = UnityEngine.Random;


public class GameManager : MonoBehaviour
{
    private BlobAssetStore blobAssetStore;
    private int num_aircraft = 0;
    private int spawn_index = 10;

    public static GameManager main;

    public GameObject planePrefab;
    public GameObject cameraFollowObj;

    public Properties prop = new Properties(
            16.2f,          // wing area
            10.9f,          // wing span
            2.0f,           // tail area
            0.0889f,        // slope of Cl-alpha curve
            0.178f,         // intercept of Cl-alpha curve
            -0.1f,          // post-stall slope of Cl-alpha curve
            3.2f,           // post-stall intercept of Cl-alpha curve
            16.0f,          // alpha when Cl=Clmax
            0.034f,         // parasite drag coefficient
            0.77f,          // induced drag efficiency coefficient
            1114.0f,        // mass
            119310.0f,      // engine power
            40.0f,          // revolutions per second
            1.905f,         // propeller diameter
            1.83f,          // propeller efficiency coefficient
            -1.32f          // propeller efficiency coefficient
            );

    Entity planeEntityPrefab;
    EntityManager manager;


    private void Awake()
    {
        if (main != null && main != this)
        {
            Destroy(gameObject);
            return;
        }

        main = this;

        manager = World.DefaultGameObjectInjectionWorld.EntityManager;

        blobAssetStore = new BlobAssetStore();

        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore);
        planeEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(planePrefab, settings);

        SpawnEntity();
    }

    private void OnDestroy()
    {
        blobAssetStore.Dispose();
    }

    void SpawnEntity()
    {
        Entity plane = manager.Instantiate(planeEntityPrefab);
        manager.AddComponentData(plane, new PlayerTag());
        manager.AddComponentData(plane, new State{
            time = 0.0f,            // time
            q0 = 0.0f,              // ODE results, x velocity
            q1 = 0.0f,              // x
            q2 = 0.0f,              // z velocity
            q3 = 0.0f,              // z
            q4 = 0.0f,              // y velocity
            q5 = 0.0f,              // y
            bank = 0.0f,            // roll angle
            alpha = 4.0f,           // pitch angle
            throttle = 0.0f,        // throttle percentage
            flap = 0.0f             // flap deflection
        });
        float3 position = new float3(0.0f, 0.0f, 0.0f);
        manager.SetComponentData(plane, new Translation { Value = position });

        //manager.DestroyEntity(planeEntityPrefab);

    }

    public void SpawnNPC()
    {
        for (int i=0; i < (spawn_index - num_aircraft); i++) {
            Entity npcPlane = manager.Instantiate(GameManager.main.planeEntityPrefab);
            manager.AddComponentData(npcPlane, new NPCTag());
            manager.AddComponentData(npcPlane, new AircraftData
            {
                speed = Random.Range(0.1f, 0.5f),           // Aircraft speed
                radian = 0.0f,                              // Radian, or point along flight path
                radius = Random.Range(10.0f, 100.0f),       // Flight path radius
                height = Random.Range(10.0f, 100.0f),       // Cruising altitude
                centerX = Random.Range(-2300.0f, 2300.0f),  // Aircraft orbit center X coordinate
                centerZ = Random.Range(-2300.0f, 2300.0f)               // Aircraft orbit center Z coordinate
            });
            manager.SetComponentData(npcPlane, new Rotation { Value = quaternion.Euler(-90f * Mathf.Deg2Rad, 0, -90f * Mathf.Deg2Rad) });
        }
        num_aircraft = spawn_index;
        spawn_index *= 10;
        Debug.Log("Spawned aircraft\n");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnNPC();
        }
    }

}
