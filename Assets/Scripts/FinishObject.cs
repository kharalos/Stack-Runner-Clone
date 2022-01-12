using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<GameManager>().GameOver();
        }
        if (other.CompareTag("Stack") && other.GetComponent<StackObject>().InStack)
        {
            FindObjectOfType<GameManager>().Collect();
        }
    }
}
