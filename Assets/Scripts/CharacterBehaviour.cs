using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour
{
    private Animator m_animator;
    private Rigidbody m_rigidbody;
    private GameManager gm;

    [SerializeField] private bool running;
    [SerializeField] private float runSpeed = 3, swerveSpeed = 2;
    private int maxStack = 20;
    [SerializeField] private Transform stackStart;
    [SerializeField] private List<GameObject> stacks = new List<GameObject>();


    void Start()
    {
        GetMaxStack();
        stacks.Clear();
        stacks = new List<GameObject>();
        m_animator = GetComponent<Animator>();
        m_rigidbody = GetComponent<Rigidbody>();
        gm = FindObjectOfType<GameManager>();
    }
    public void GetMaxStack()
    {
        maxStack = 20 + SavedDataHandler.Instance.stackUpgrade;
    }
    void Update()
    {
        m_animator.SetFloat("StackEffect", (float)stacks.Count / (float)maxStack);
        

        if (running)
        {
            //m_rigidbody.velocity = Vector3.forward * Time.deltaTime * speed;
            transform.position += Vector3.forward * Time.deltaTime * (runSpeed * (1 + (float)stacks.Count / (float)maxStack));
        }
        if (SwerveInput.swerveLeft)
        {
            transform.position += Vector3.left * Time.deltaTime * swerveSpeed;
        }
        else if (SwerveInput.swerveRight)
        {
            transform.position += Vector3.right * Time.deltaTime * swerveSpeed;
        }
        var pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, -2.0f, 2.0f);
        transform.position = pos;

    }
    public void StartRun()
    {
        running = true;
        m_animator.SetTrigger("Start");
    }
    public void StopRun()
    {
        running = false;
        m_animator.SetTrigger("Stop");
    }
    public void Dance()
    {
        m_animator.SetTrigger("Dance");
    }
    public Transform GetLastStack(GameObject stack)
    {
        Transform lastObject;

        if (stacks.Count == 0) { lastObject = stackStart; }

        else if (stacks.Count > maxStack) return null;

        else lastObject = stacks.Last<GameObject>().transform;

        stacks.Add(stack);
        gm.StackAdded(((float)stacks.Count / (float)maxStack));

        Debug.Log("Stack added to: " + lastObject.name + " Stack Count Is: " + stacks.Count);
        //Debug.Log("Stack size is: " + stacks.Count);
        gm.StackAdded(((float)stacks.Count / (float)maxStack));
        return lastObject;
    }
    public void DropStacks(GameObject stack)
    {
        if(stacks.Count == 0) return;
        int number = 0;

        if (stack != null) number = stacks.IndexOf(stack);

        Debug.Log("Stack dropped from: " + number);

        for (int i = number; i < stacks.Count; i++)
        {
            stacks[i].GetComponent<StackObject>().Dropped();
            Debug.Log("Stack Count Is: " + stacks.Count);
        }
        stacks.RemoveRange(number, stacks.Count - number);
        gm.StackAdded(((float)stacks.Count / (float)maxStack));
    }
}
