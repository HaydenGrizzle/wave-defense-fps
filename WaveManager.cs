using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveManager : MonoBehaviour
{
    [Header("Troops")]
    public GameObject larry;
    public GameObject spearGob;
    public GameObject knight;
    public GameObject PEKA;
    public GameObject Giant;
    public GameObject eBarbs;
    public GameObject megaKnight;
    public GameObject golem;
    public GameObject musketeers;
    public GameObject kingsVessle;

    [Header("Wave Settings")]
    public Transform leftSpawn;
    public Transform rightSpawn;
    public Transform golemSpawn;
    public float waveDuration = 45f;
    public float spawnIntervalMin = 3f;
    public float spawnIntervalMax = 6f;
    private int waveNumber = 1;
    private bool waveActive = false;

    [Header("Other")]
    public ParticleSystem landSmoke;
    public ParticleSystem divineSpawn;
    public bool golemSpawned = true;
    public bool kingSpawned = true;
    public TMP_Text waveText;
    public GameObject winUI;

    void Start()
    {
        StartCoroutine(GameLoop()); // Start Waves 🦍
    }

    void Update()
    {
        waveText.text = "Wave_" + waveNumber;
    }

    IEnumerator GameLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(15f);
            // Wave start
            waveActive = true;
            golemSpawned = false; // reset for new wave
            kingSpawned = false;
            Debug.Log("Wave started");
            yield return StartCoroutine(RunWave());

            // Wait until all enemies from this wave are dead
            Debug.Log("Wave spawn finished. Waiting for remaining enemies to be defeated...");
            yield return new WaitUntil(() => GetActiveEnemyCount() == 0);

            // Wave end
            waveActive = false;
            waveNumber++;

            Debug.Log("Wave ended");

            // Time between waves
            yield return new WaitForSeconds(10f);
        }
    }

    IEnumerator RunWave()
    {
        float elapsed = 0f;

        while (elapsed < waveDuration)
        {
            SpawnEnemy();

            // Wait time before spawns
            float wait = Random.Range(spawnIntervalMin, spawnIntervalMax);
            yield return new WaitForSeconds(wait);

            elapsed += wait;
        }
    }



    void SpawnEnemy()
    {
        if (waveNumber == 1)
        {
            SpawnLarry();
        }

        if (waveNumber == 2)
        {
            Wave2Spawner();
        }

        if (waveNumber == 3)
        {
            Wave3Spawner();
        }

        if (waveNumber == 4)
        {
            Wave4Spawner();
        }

        if (waveNumber == 5)
        {
            Wave5Spawner();
        }

        if (waveNumber == 6)
        {
            Wave6Spawner();
        }

        if (waveNumber == 7)
        {
            Wave7Spawner();
        }

        if (waveNumber == 8)
        {
            if (!golemSpawned)
            {
                SpawnGolem();
                golemSpawned = true;
            }
        }

        if (waveNumber == 9)
        {
            Wave9Spawner();
        }

        if (waveNumber == 10)
        {
            if (!kingSpawned)
            {
                SpawnKingVessle();
                kingSpawned = true;
            }
        }

        if (waveNumber == 11)
        {
            Win();
        }
    }



    //------------------
    // TROOP SPAWNERS
    //------------------
    void SpawnLarry()
    {
        Transform spawnPoint = (Random.value > 0.5f) ? leftSpawn : rightSpawn;
        // Spawn group parent up in the air
        Vector3 dropPosition = spawnPoint.position + Vector3.up * 10f;
        GameObject enemyGroup = Instantiate(larry, dropPosition, spawnPoint.rotation);
        // Assign player to every Enemy in the group
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        Enemy[] enemies = enemyGroup.GetComponentsInChildren<Enemy>();
        foreach (Enemy e in enemies)
        {
            e.player = playerTransform;
        }
        // For each child in the group, disable its agent and kick off a drop to its formation spot
        foreach (Transform child in enemyGroup.transform)
        {
            var agent = child.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (agent != null)
            {
                agent.enabled = false;
                // Calculate the child's final world position based on the spawnPoint + child's local offset
                Vector3 childFinalPos = spawnPoint.TransformPoint(child.localPosition);
                // Start drop coroutine for this child
                StartCoroutine(SimpleDrop(child, childFinalPos, agent));
            }
        }
    }


    void SpawnSpearGobs()
    {
        Transform spawnPoint = (Random.value > 0.5f) ? leftSpawn : rightSpawn;
        // Spawn group parent up in the air
        Vector3 dropPosition = spawnPoint.position + Vector3.up * 10f;
        GameObject enemyGroup = Instantiate(spearGob, dropPosition, spawnPoint.rotation); // TROOP PICK
        // Assign player to every Enemy in the group
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        RangedEnemy[] rangedEnemies = enemyGroup.GetComponentsInChildren<RangedEnemy>();
        foreach (RangedEnemy r in rangedEnemies)
        {
            r.player = playerTransform;
        }
        // For each child in the group, disable its agent and kick off a drop to its formation spot
        foreach (Transform child in enemyGroup.transform)
        {
            var agent = child.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (agent != null)
            {
                agent.enabled = false;
                // Calculate the child's final world position based on the spawnPoint + child's local offset
                Vector3 childFinalPos = spawnPoint.TransformPoint(child.localPosition);
                // Start drop coroutine for this child
                StartCoroutine(SimpleDrop(child, childFinalPos, agent));
            }
        }
    }


    void SpawnKnight()
    {
        // For Single Troop
        Transform spawnPoint = (Random.value > 0.5f) ? leftSpawn : rightSpawn;
        Vector3 dropPosition = spawnPoint.position + Vector3.up * 5f;
        GameObject enemy = Instantiate(knight, dropPosition, spawnPoint.rotation);
        enemy.GetComponent<Enemy>().player = GameObject.FindGameObjectWithTag("Player").transform;
        UnityEngine.AI.NavMeshAgent agent = enemy.GetComponent<UnityEngine.AI.NavMeshAgent>();
        // Check dat
        if (agent != null)
        {
            agent.enabled = false;
        }
        StartCoroutine(SimpleDrop(enemy.transform, spawnPoint.position, agent));
    }


    void SpawnPEKA()
    {
        Transform spawnPoint = (Random.value > 0.5f) ? leftSpawn : rightSpawn;
        Vector3 dropPosition = spawnPoint.position + Vector3.up * 5f;
        GameObject enemy = Instantiate(PEKA, dropPosition, spawnPoint.rotation);
        RangedEnemy rangedEnemy = enemy.GetComponent<RangedEnemy>();
        if (rangedEnemy != null)
        {
            rangedEnemy.player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        UnityEngine.AI.NavMeshAgent agent = enemy.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
        {
            agent.enabled = false;
        }
        StartCoroutine(SimpleDrop(enemy.transform, spawnPoint.position, agent));
    }


    void SpawnGiant()
    {
        Transform spawnPoint = (Random.value > 0.5f) ? leftSpawn : rightSpawn;
        Vector3 dropPosition = spawnPoint.position + Vector3.up * 5f;
        GameObject enemy = Instantiate(Giant, dropPosition, spawnPoint.rotation);
        WinCondition winCondition = enemy.GetComponent<WinCondition>();
        if (winCondition != null)
        {
            winCondition.tower = GameObject.FindGameObjectWithTag("Tower").transform;
        }

        UnityEngine.AI.NavMeshAgent agent = enemy.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
        {
            agent.enabled = false;
        }
        StartCoroutine(SimpleDrop(enemy.transform, spawnPoint.position, agent));
    }


    void SpawnE_Barb()
    {
        Transform spawnPoint = (Random.value > 0.5f) ? leftSpawn : rightSpawn;
        // Spawn group parent up in the air
        Vector3 dropPosition = spawnPoint.position + Vector3.up * 10f;
        GameObject enemyGroup = Instantiate(eBarbs, dropPosition, spawnPoint.rotation); // TROOP PICK
        // Assign player to every Enemy in the group
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        ExplosiveEnemy[] explosiveEnemies = enemyGroup.GetComponentsInChildren<ExplosiveEnemy>();
        foreach (ExplosiveEnemy r in explosiveEnemies)
        {
            r.player = playerTransform;
        }
        // For each child in the group, disable its agent and kick off a drop to its formation spot
        foreach (Transform child in enemyGroup.transform)
        {
            var agent = child.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (agent != null)
            {
                agent.enabled = false;
                // Calculate the child's final world position based on the spawnPoint + child's local offset
                Vector3 childFinalPos = spawnPoint.TransformPoint(child.localPosition);
                // Start drop coroutine for this child
                StartCoroutine(SimpleDrop(child, childFinalPos, agent));
            }
        }
    }


    void SpawnMegaKnight()
    {
        // For Single Troop
        Transform spawnPoint = (Random.value > 0.5f) ? leftSpawn : rightSpawn;
        Vector3 dropPosition = spawnPoint.position + Vector3.up * 5f;
        GameObject enemy = Instantiate(megaKnight, dropPosition, spawnPoint.rotation);
        enemy.GetComponent<Enemy>().player = GameObject.FindGameObjectWithTag("Player").transform;
        UnityEngine.AI.NavMeshAgent agent = enemy.GetComponent<UnityEngine.AI.NavMeshAgent>();
        // Check dat
        if (agent != null)
        {
            agent.enabled = false;
        }
        StartCoroutine(SimpleDrop(enemy.transform, spawnPoint.position, agent));
    }


    void SpawnGolem()
    {
        Transform spawnPoint = golemSpawn;
        Vector3 undergroundPos = spawnPoint.position + Vector3.down * 18f; // start underground
        GameObject enemy = Instantiate(golem, undergroundPos, spawnPoint.rotation);

        // Assign win condition
        WinCondition winCondition = enemy.GetComponent<WinCondition>();
        if (winCondition != null)
        {
            winCondition.tower = GameObject.FindGameObjectWithTag("Tower").transform;
        }

        // Disable NavMesh until risen
        UnityEngine.AI.NavMeshAgent agent = enemy.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
        {
            agent.enabled = false;
        }

        // Raise him out of the ground over time
        StartCoroutine(DivineDrop(enemy.transform, spawnPoint.position, agent));
    }


    void SpawnMusketeers()
    {
        Transform spawnPoint = (Random.value > 0.5f) ? leftSpawn : rightSpawn;
        // Spawn group parent up in the air
        Vector3 dropPosition = spawnPoint.position + Vector3.up * 10f;
        GameObject enemyGroup = Instantiate(musketeers, dropPosition, spawnPoint.rotation); // TROOP PICK
        // Assign player to every Enemy in the group
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        RangedEnemy[] rangedEnemies = enemyGroup.GetComponentsInChildren<RangedEnemy>();
        foreach (RangedEnemy r in rangedEnemies)
        {
            r.player = playerTransform;
        }
        // For each child in the group, disable its agent and kick off a drop to its formation spot
        foreach (Transform child in enemyGroup.transform)
        {
            var agent = child.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (agent != null)
            {
                agent.enabled = false;
                // Calculate the child's final world position based on the spawnPoint + child's local offset
                Vector3 childFinalPos = spawnPoint.TransformPoint(child.localPosition);
                // Start drop coroutine for this child
                StartCoroutine(SimpleDrop(child, childFinalPos, agent));
            }
        }
    }


    //-------------------
    // CHECK DIS
    //-------------------

    void SpawnKingVessle()
    {
        // Randomly choose left or right spawn
    Transform spawnPoint = (Random.value > 0.5f) ? leftSpawn : rightSpawn;

    // Start above the ground for drop animation
    Vector3 dropPosition = spawnPoint.position + Vector3.up * 8f;

    // Spawn it
    GameObject enemy = Instantiate(kingsVessle, dropPosition, spawnPoint.rotation);

    // Assign player to the KingsVessle script
    KingsVessle kv = enemy.GetComponent<KingsVessle>();
        if (kv != null)
        {
            kv.player = GameObject.FindGameObjectWithTag("Player").transform;
        }

    // Disable NavMesh until drop finishes
    UnityEngine.AI.NavMeshAgent agent = enemy.GetComponent<UnityEngine.AI.NavMeshAgent>();
    if (agent != null)
    {
        agent.enabled = false;
    }

        // Start the drop animation
        StartCoroutine(SimpleDrop(enemy.transform, spawnPoint.position, agent));
    }

    //------------------
    // Test Larry
    //------------------
    void TestLarry()
    {
        // For Single Troop
        Transform spawnPoint = (Random.value > 0.5f) ? leftSpawn : rightSpawn;
        Vector3 dropPosition = spawnPoint.position + Vector3.up * 5f;
        GameObject enemy = Instantiate(larry, dropPosition, spawnPoint.rotation);
        enemy.GetComponent<Enemy>().player = GameObject.FindGameObjectWithTag("Player").transform;
        UnityEngine.AI.NavMeshAgent agent = enemy.GetComponent<UnityEngine.AI.NavMeshAgent>();
        // Check dat
        if (agent != null)
        {
            agent.enabled = false;
        }
        StartCoroutine(SimpleDrop(enemy.transform, spawnPoint.position, agent));
    }


    //------------------
    // DROP ANIMATIONS
    //------------------
    IEnumerator SimpleDrop(Transform target, Vector3 finalPos, UnityEngine.AI.NavMeshAgent agent)
    {
        float speed = 11.5f;
        // Move down until we reach the final world position (uses world positions)
        while (target.position.y > finalPos.y + 0.01f) // tiny epsilon to avoid float issues
        {
            target.position = Vector3.MoveTowards(target.position, finalPos, speed * Time.deltaTime);
            yield return null;
        }
        // Snap exactly to final
        target.position = finalPos;
        // Per-child smoke
        if (landSmoke != null)
        {
            Instantiate(landSmoke, finalPos, Quaternion.identity);
        }
        yield return new WaitForSeconds(0.05f);
        // Ensure agent internal position matches the transform, then enable it
        if (agent != null)
        {
            // Warp places the agent cleanly on the NavMesh at the given position
            agent.Warp(finalPos);
            agent.enabled = true;
        }
    }

    IEnumerator DivineDrop(Transform target, Vector3 finalPos, UnityEngine.AI.NavMeshAgent agent)
    {
        float duration = 6.66f;
        float elapsed = 0f;
        Vector3 startPos = target.position; // underground position

        // Spawn the particle in the scene and keep a reference
        ParticleSystem ps = null;
        if (divineSpawn != null)
        {
            ps = Instantiate(divineSpawn, finalPos, Quaternion.identity);
        }

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            target.position = Vector3.Lerp(startPos, finalPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        target.position = finalPos;

        // Stop the particle and enable the agent
        if (agent != null)
        {
            if (ps != null) ps.Stop(); // stops the spawned particle in the scene
            agent.Warp(finalPos);
            agent.enabled = true;
        }
    }

    int GetActiveEnemyCount()
    {
        int melee = FindObjectsOfType<Enemy>().Length;
        int ranged = FindObjectsOfType<RangedEnemy>().Length;
        int winConditions = FindObjectsOfType<WinCondition>().Length;
        int explosiveTroops = FindObjectsOfType<ExplosiveEnemy>().Length;
        int divineTroops = FindObjectsOfType<Divine>().Length;
        return melee + ranged + winConditions + explosiveTroops + divineTroops;
    }


    void Wave2Spawner()
    {
        int rand = Random.Range(0, 2);

        if (rand == 1)
        {
            SpawnLarry();
        }
        else
        {
            SpawnSpearGobs();
        }
    }

    void Wave3Spawner()
    {
        int rand = Random.Range(0, 3);

        switch (rand)
        {
            case 0:
                SpawnLarry();
                break;
            case 1:
                SpawnSpearGobs();
                break;
            case 2:
                SpawnKnight();
                break;
        }
    }

    void Wave4Spawner()
    {
        int rand = Random.Range(0, 4);

        switch (rand)
        {
            case 0:
                SpawnLarry();
                break;
            case 1:
                SpawnSpearGobs();
                break;
            case 2:
                SpawnKnight();
                break;
            case 3:
                SpawnPEKA();
                break;
        }
    }

    void Wave5Spawner()
    {
        int rand = Random.Range(0, 5);

        switch (rand)
        {
            case 0:
                SpawnLarry();
                break;
            case 1:
                SpawnSpearGobs();
                break;
            case 2:
                SpawnKnight();
                break;
            case 3:
                SpawnPEKA();
                break;
            case 4:
                SpawnGiant();
                break;
        }
    }

    void Wave6Spawner()
    {
        int rand = Random.Range(0, 6);

        switch (rand)
        {
            case 0:
                SpawnLarry();
                break;
            case 1:
                SpawnSpearGobs();
                break;
            case 2:
                SpawnKnight();
                break;
            case 3:
                SpawnPEKA();
                break;
            case 4:
                SpawnGiant();
                break;
            case 5:
                SpawnE_Barb();
                break;
        }
    }

    void Wave7Spawner()
    {
        int rand = Random.Range(0, 7);

        switch (rand)
        {
            case 0:
                SpawnLarry();
                break;
            case 1:
                SpawnSpearGobs();
                break;
            case 2:
                SpawnKnight();
                break;
            case 3:
                SpawnPEKA();
                break;
            case 4:
                SpawnGiant();
                break;
            case 5:
                SpawnE_Barb();
                break;
            case 6:
                SpawnMegaKnight();
                break;
        }
    }

    void Wave9Spawner()
    {
        int rand = Random.Range(0, 8);

        switch (rand)
        {
            case 0:
                SpawnLarry();
                break;
            case 1:
                SpawnSpearGobs();
                break;
            case 2:
                SpawnKnight();
                break;
            case 3:
                SpawnPEKA();
                break;
            case 4:
                SpawnGiant();
                break;
            case 5:
                SpawnE_Barb();
                break;
            case 6:
                SpawnMegaKnight();
                break;
            case 7:
                SpawnMusketeers();
                break;
        }
    }

    void Win()
    {
        winUI.SetActive(true);    
    }
}
