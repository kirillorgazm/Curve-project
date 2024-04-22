using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBranch : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayerMask;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // if (!Physics2D.OverlapCircle(transform.position, 2f, groundLayerMask))
        //     transform.Translate(Vector3.down * 5f * Time.deltaTime);
    }
}
