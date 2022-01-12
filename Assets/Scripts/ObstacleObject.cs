using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleObject : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.forward, 5f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<CharacterBehaviour>().DropStacks(null);
            Debug.Log("Obstacle Hit");
        }
        if (other.CompareTag("Stack") && other.GetComponent<StackObject>().InStack)
        {
            FindObjectOfType<CharacterBehaviour>().DropStacks(other.gameObject);
            Debug.Log(string.Format(("Obstacle Hit: {0}"),other.gameObject.name));
        }
    }
}
