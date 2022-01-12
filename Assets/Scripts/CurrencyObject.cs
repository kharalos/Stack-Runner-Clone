using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<GameManager>().CollectCurrency(transform.position);
            Debug.Log("Coin picked up by the player.");
            Destroy(gameObject);
        }
        if (other.CompareTag("Stack") && other.GetComponent<StackObject>().InStack)
        {
            FindObjectOfType<GameManager>().CollectCurrency(transform.position);
            Debug.Log(string.Format("Coin picked up by {0}", other.gameObject.name));
            Destroy(gameObject);
        }
    }
}
