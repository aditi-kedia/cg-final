using UnityEngine;
using System.Collections.Generic;

public class DistractionManager : MonoBehaviour
{
    public static DistractionManager Instance { get; private set; }

    [Header("NPC Settings")]
    [SerializeField] private GameObject npcPrefab;        // Your NPC prefab
    [SerializeField] private Transform[] waypointGroup1;  // Waypoints NPCs can walk between
    [SerializeField] private Transform[] waypointGroup2;

    [Header("Level Counts")]
    [SerializeField] private int level1NPCs = 0;
    [SerializeField] private int level2NPCs = 2;
    [SerializeField] private int level3NPCs = 5;

    private List<GameObject> spawnedNPCs = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }
    }

    public void SetLevel(int level)
    {
        DespawnAll();

        int count = level switch
        {
            1 => level1NPCs,
            2 => level2NPCs,
            3 => level3NPCs,
            _ => 0
        };

        for (int i = 0; i < count; i++)
            SpawnNPC();
    }

private void SpawnNPC()
{
    if (npcPrefab == null) return;

    // Pick a random waypoint as spawn position
    Transform spawnPoint = waypointGroup1[Random.Range(0, waypointGroup1.Length)];
    
    // Add random offset so they don't spawn in exact same spot
    Vector3 randomOffset = new Vector3(
        Random.Range(-0.5f, 0.5f), 
        0, 
        Random.Range(-0.5f, 0.5f)
    );
    
    GameObject npc = Instantiate(npcPrefab, spawnPoint.position + Vector3.up * 1f + randomOffset, Quaternion.identity);

    // Give it a waypoint walker
    NPCWalker walker = npc.GetComponent<NPCWalker>();
    if (walker != null)
        walker.SetWaypoints(waypointGroup1);

    spawnedNPCs.Add(npc);
}

    private void DespawnAll()
    {
        foreach (var npc in spawnedNPCs)
            if (npc != null) Destroy(npc);
        spawnedNPCs.Clear();
    }
}