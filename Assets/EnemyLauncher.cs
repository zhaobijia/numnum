using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLauncher : MonoBehaviour
{
    public GameObject[] enemyPrefabs;

    private Transform launchTransform; //set to a range of x value

    //for random positon spawn at the same time
    private float minX;
    private float minY;
    private float maxX;
    private float maxY;


    //for random time? (when do we spawn)

    public int MaxSpawn;
    int currentSpawn;
    // Start is called before the first frame update
    void Start()
    {
        launchTransform = GetComponent<Transform>();

        minX = launchTransform.position.x - launchTransform.localScale.x / 2;
        maxX = launchTransform.position.x + launchTransform.localScale.x / 2;
        minY = launchTransform.position.y - launchTransform.localScale.y / 2;
        maxY = launchTransform.position.y + launchTransform.localScale.y / 2;


        InvokeRepeating("SpawnEnemy", 4.0f, 2.0f);
    }
    // Update is called once per frame
    bool countIsOver = false;
    void Update()
    {
        currentSpawn = transform.childCount;

        if (currentSpawn > MaxSpawn)
        {
            CancelInvoke();
            countIsOver = true;
        }

        if (countIsOver)
        {
            if (currentSpawn < MaxSpawn)
            {
                //resume
                countIsOver = false;
                InvokeRepeating("SpawnEnemy", 4.0f, 2.0f);
            }
        }

        
    }

    private Vector3 randomPosition(float minX, float minY, float maxX, float maxY)
    {
        float rdX = Random.Range(minX, maxX);

        float rdY = Random.Range(minY, maxY);

        Vector3 newVector = new Vector3(rdX, rdY, 0);

        return newVector;
    }

    void SpawnEnemy()
    {
        //quantity is random generated outside function  
        int quantity = Random.Range(3, 15);

        for (int i = 0; i < quantity; i++)
        {
            int enemySize = Random.Range(0, 4);
            var newEnemy = Instantiate(enemyPrefabs[enemySize], randomPosition(minX, minY, maxX, maxY), launchTransform.rotation);

            newEnemy.transform.parent = gameObject.transform;
        }

    }
}
