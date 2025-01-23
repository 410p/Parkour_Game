using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class CreidtStart : MonoBehaviour
{
    [SerializeField] Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator.SetBool("Credit_Down", true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
