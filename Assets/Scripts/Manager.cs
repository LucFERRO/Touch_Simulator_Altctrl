using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [Header("Difficulty Settings")]
    public float timePassed;
    public float difficultyScaling;
    public float startingDifficulty;
    public float chancesOfDoubleProjectiles;
    public float chancesOfDoubleSaws;

    [Header("Spawners Position")]
    public bool areSpawnerVisible;
    public float midRowHeight;
    public float sideDistance;
    public float botSideDistance;
    public Transform playerTransform;
    public Transform spawnersPos;
    public GameObject topSpawner;
    public GameObject leftSpawner;
    public GameObject midSpawner;
    public GameObject rightSpawner;
    public GameObject botLeftSpawner;
    public GameObject botRightSpawner;
    public GameObject[] smallProjectileSpawners;

    [Header("Projectiles")]
    public GameObject midProjectile;
    public GameObject[] grenades;
    public GameObject saw;
    //Archives
    public GameObject sawTest;
    public GameObject projectile1;
    public GameObject topProjectile;

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
            Vector3 botRowPos = Vector3.zero;
            midRowPos.x = i * sideDistance;
            botRowPos.x = i * botSideDistance;
            if (i == -1)
            {
                botLeftSpawner.transform.position = spawnersPos.position + botRowPos;
            }
            if (i == 1)
            {
                botRightSpawner.transform.position = spawnersPos.position + botRowPos;
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
            if (randomSpawnerInt == 0)
            {
                createdProjectile = SpawnMidProjectile();
            }
            else if (randomSpawnerInt < 3)
            {
                bool leftOrRightProjectile = randomSpawnerInt == 1;
                ExtraSpawn(RandomGrenadeChoice(), chancesOfDoubleProjectiles, leftOrRightProjectile);
                createdProjectile = SpawnSideProjectile(leftOrRightProjectile ? 2 : 0);
            }
            else if (randomSpawnerInt < 5)
            {
                bool leftOrRightSaw = randomSpawnerInt == 3;
                ExtraSpawn(saw, chancesOfDoubleSaws, !leftOrRightSaw);
                createdProjectile = SpawnSaw(leftOrRightSaw);
            }
            else
            {
                createdProjectile = SpawnTopProjectile();
            }

            createdProjectile.GetComponent<Projectile>().playerTransform = playerTransform;
            createdProjectile.GetComponent<Projectile>().speed = startingDifficulty + difficultyScaling;

            IncreaseDifficulty();
            timePassed = 0f;

        }

        void IncreaseDifficulty()
        {
            if (difficultyScaling < 40)
            {
                difficultyScaling += 2f;
            }
        }

        void ExtraSpawn(GameObject item, float chancesOfDouble, bool isLeftOrRight)
        {
            if (difficultyScaling < 20){
                return;
            }

            bool doubledProj = Random.Range(0, 100) < chancesOfDouble;
            if (!doubledProj)
            {
                return;
            }

            GameObject extraProjectile;

            if (item == saw)
            {
                extraProjectile = SpawnSaw(isLeftOrRight);
            } else
            {
                extraProjectile = SpawnSideProjectile(isLeftOrRight ? 0 : 2);
            }

            extraProjectile.GetComponent<Projectile>().playerTransform = playerTransform;
            extraProjectile.GetComponent<Projectile>().speed = startingDifficulty + difficultyScaling;
        }

        GameObject SpawnTopProjectile()
        {
            return Instantiate(topProjectile, topSpawner.transform.position, Quaternion.identity);
        }        
        GameObject SpawnMidProjectile()
        {
            return Instantiate(midProjectile, smallProjectileSpawners[1].transform.position, Quaternion.identity);
        }

        GameObject SpawnSideProjectile(int spawnerIndex)
        {
            int randomProjectileIndex = Random.Range(0, 5);
            return Instantiate(RandomGrenadeChoice(), smallProjectileSpawners[spawnerIndex].transform.position, Quaternion.identity);
        }

        GameObject RandomGrenadeChoice()
        {
            int randomProjectileIndex = Random.Range(0, 5);
            return grenades[randomProjectileIndex];
        }

        GameObject SpawnSaw(bool leftOrRightBool)
        {
            Vector3 sawSpawningPoint = leftOrRightBool ? botLeftSpawner.transform.position : botRightSpawner.transform.position;
            GameObject spawnedSaw = Instantiate(saw, sawSpawningPoint, Quaternion.identity);
            spawnedSaw.GetComponent<RotatingSaw>().spinDirection = leftOrRightBool;
            return spawnedSaw;
        }
    }
}
