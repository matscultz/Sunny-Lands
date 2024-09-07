using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure_Chest_Default : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenChest()
    {
        animator.SetTrigger("isOpen");
    }
}
