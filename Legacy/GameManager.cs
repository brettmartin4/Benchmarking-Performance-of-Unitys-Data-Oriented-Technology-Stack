using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class OldGameManager : MonoBehaviour
{

    [SerializeField] private GameObject NPCPrefab;
    private int num_aircraft = 0;
    private int spawn_index = 10;


    private void spawnNPC()
    {
        for(int i=0; i < (spawn_index - num_aircraft); i++)
        {
            GameObject newNPC = Instantiate(NPCPrefab) as GameObject;
        }
        num_aircraft = spawn_index;
        spawn_index *= 10;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Nothing yet
    }


    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Space))
        {
            // Increment number of NPC aircraft
            spawnNPC();
            Debug.LogFormat("Spawned aircraft: total of {0} aircraft\n", num_aircraft);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            // Restart scene
            SceneManager.LoadScene("Legacy_Scene");
        }
    }
}
