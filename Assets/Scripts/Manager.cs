using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public float difficultyScaling;
    public float startingDifficulty;
    public Transform playerTransform;
    public Transform spawnersPos;
    public GameObject topSpawner;
    public GameObject leftSpawner;
    public GameObject midSpawner;
    public GameObject rightSpawner;
    public GameObject botLeftSpawner;
    public GameObject botRightSpawner;
    public GameObject saw;
    public GameObject projectile1;
    public GameObject topProjectile;
    public GameObject[] smallProjectileSpawners;
    public float timePassed;
    [Header("Spawners Position")]
    public bool areSpawnerVisible;
    public float midRowHeight;
    public float sideDistance;

    void Start()
    {
        foreach (GameObject spawner in smallProjectileSpawners)
        {
            spawner.GetComponent<MeshRenderer>().enabled = areSpawnerVisible;
        }
        topSpawner.GetComponent<MeshRenderer>().enabled = areSpawnerVisible;
        botLeftSpawner.GetComponent<MeshRenderer>().enabled = areSpawnerVisible;
        botRightSpawner.GetComponent<MeshRenderer>().enabled = areSpawnerVisible;

        for (int i = -1; i < 2; i++)
        {
            Vector3 midRowPos = Vector3.zero;
            midRowPos.x = i * sideDistance;
            if (i == -1)
            {
                botLeftSpawner.transform.position = spawnersPos.position + midRowPos;
            }
            if (i == 1)
            {
                botRightSpawner.transform.position = spawnersPos.position + midRowPos;
            }
            midRowPos.y = midRowHeight;
            smallProjectileSpawners[i + 1].transform.position = spawnersPos.position + midRowPos;
        }



        timePassed = 4f;
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime * (1 + difficultyScaling / 10);
        if (timePassed > 5f)
        {
            GameObject createdProjectile = null;
            int randomSpawnerInt = Random.Range(0, 6);
            if (randomSpawnerInt < 3)
            {
                createdProjectile = SpawnSmallProjectile(randomSpawnerInt);
            }
            else if (randomSpawnerInt % 2 == 1)
            {
                createdProjectile = SpawnSaw(randomSpawnerInt == 3);
            }
            else
            {
                createdProjectile = SpawnTopProjectile();
            }
            createdProjectile.GetComponent<Projectile>().playerTransform = playerTransform;
            createdProjectile.GetComponent<Projectile>().speed = startingDifficulty + difficultyScaling;
            if (difficultyScaling < 40)
            {
                difficultyScaling += 2f;
            }
            timePassed = 0f;

        }

        GameObject SpawnTopProjectile()
        {
            return Instantiate(topProjectile, topSpawner.transform.position, Quaternion.identity);
        }

        GameObject SpawnSmallProjectile(int spawnerIndex)
        {
            return Instantiate(projectile1, smallProjectileSpawners[spawnerIndex].transform.position, Quaternion.identity);
        }

        GameObject SpawnSaw(bool leftOrRightBool)
        {
            Vector3 sawSpawningPoint = leftOrRightBool ? botLeftSpawner.transform.position : botRightSpawner.transform.position;
            return Instantiate(saw, sawSpawningPoint, Quaternion.identity);
        }
    }
}
