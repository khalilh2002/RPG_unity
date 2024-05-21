using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;
    public float lifetime = 1f;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.Play("ThunderAnim"); // Replace with your animation name
        }
        // Destroy the thunderbolt after its lifetime
        Destroy(gameObject, lifetime);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
