using UnityEngine;
using System.Collections.Generic;

public class SurfaceTrigger : MonoBehaviour
{
    public List<GameObject> objectsToSpawn = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BigPlayer") || other.CompareTag("SmallPlayer"))
        {
            SpawnObjects();
            gameObject.SetActive(false);
        }
    }

    private void SpawnObjects()
    {
        foreach (GameObject obj in objectsToSpawn)
        { 
            Instantiate(obj, obj.transform.position, obj.transform.rotation);
        }
    }
}
