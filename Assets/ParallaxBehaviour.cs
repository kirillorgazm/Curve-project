using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBehaviour : MonoBehaviour
{

    [SerializeField] Transform followingTarget;
    [SerializeField, Range(0f, 1f)] float parallaxFactor = 0.1f;
    [SerializeField] bool disableVerticalParallax;

    Vector3 targetPrevPos;
    // Start is called before the first frame update
    void Start()
    {
        if (!followingTarget)
            followingTarget = Camera.main.transform;

        targetPrevPos = followingTarget.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 delta = followingTarget.position - targetPrevPos;
        
        if (disableVerticalParallax)
            delta.y = 0;

        targetPrevPos = followingTarget.position;
        transform.position += delta * parallaxFactor;

    }
}
