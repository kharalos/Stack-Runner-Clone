using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackObject : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    private Rigidbody m_rigidbody;
    private BoxCollider m_collider;

    [SerializeField]
    private bool inStack;
    private bool canPick;

    public bool InStack => inStack;
    private void Start()
    {
        canPick = true;
        m_rigidbody = GetComponent<Rigidbody>();
        m_collider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        if (inStack)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, target.position.z + .75f);
            transform.position = Vector3.Lerp(transform.position, target.position + (Vector3.forward * .75f), Time.deltaTime * 10);
        }
    }

    public void PickedUp(Transform newtarget)
    {
        if (newtarget == null) return;
        canPick = false;
        inStack = true;
        target = newtarget;
        m_rigidbody.isKinematic = true;
        m_collider.isTrigger = true;
    }

    public void Dropped()
    {
        inStack = false;
        target = null;
        m_rigidbody.isKinematic = false;
        m_collider.isTrigger = false;
        m_rigidbody.AddForce(new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)).normalized * 3, ForceMode.VelocityChange);
        CancelInvoke("PickDelay");
        Invoke("PickDelay", 1f);
    }
    private void PickDelay()
    {
        canPick = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!inStack && canPick)
        {
            if (other.CompareTag("Player"))
            {
                PickedUp(other.GetComponent<CharacterBehaviour>().GetLastStack(this.gameObject));
                //Debug.Log("Picked up by the player.");
            }
            if (other.CompareTag("Stack") && other.GetComponent<StackObject>().InStack)
            {
                PickedUp(FindObjectOfType<CharacterBehaviour>().GetLastStack(this.gameObject));
                //Debug.Log(string.Format("Picked up by {0}", other.gameObject.name));
            } 
        }
    }
}
