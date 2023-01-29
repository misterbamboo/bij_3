using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallAnimations : MonoBehaviour
{

    public Animator AnimRefObj;

    // Start is called before the first frame update
    void Start()
    {
        AnimRefObj = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimRefObj.Play("Sheep_Idle");   //use the name of your animation clip in the quotes.
    }
}
