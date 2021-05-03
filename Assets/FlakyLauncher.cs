using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlakyLauncher : MonoBehaviour
{
    public GameObject  flakyPrefab;


    private Transform launchTransform; //set to a range of x value


    //for random positon spawn at the same time
     private float minX;
     private float minY;
     private float maxX;
     private float maxY;


    //for random time? (when do we spawn)



    // Start is called before the first frame update
    void Start()
    {
        launchTransform = GetComponent<Transform>();

        minX = launchTransform.position.x - launchTransform.localScale.x / 2;
        maxX = launchTransform.position.x + launchTransform.localScale.x / 2;
        minY = launchTransform.position.y - launchTransform.localScale.y / 2;
        maxY = launchTransform.position.y + launchTransform.localScale.y / 2;


        InvokeRepeating("SpawnFlaky", 4.0f, 2.0f);
    }
    // Update is called once per frame
    const int MaxSpawn = 30;
    int currentSpawn;
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
                InvokeRepeating("SpawnFlaky", 4.0f, 2.0f);
            }
        }

    }

    private Vector3 randomPosition(float minX, float minY, float maxX, float maxY)
    {
        float rdX = Random.Range(minX, maxX);

        float rdY = Random.Range(minY, maxY);

        Vector3 newVector = new Vector3(rdX, rdY,0);

        return newVector;
    }

    public int min;
    public int max;
    void SpawnFlaky()
    {
        //quantity is random generated outside function  
        int quantity = Random.Range(min, max);

       for(int i = 0; i<quantity; i++)
        {
            var newFlaky = Instantiate(flakyPrefab, randomPosition(minX, minY, maxX, maxY), launchTransform.rotation);

            newFlaky.transform.parent = gameObject.transform;

        }

    }
    
}
