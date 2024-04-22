using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSurfaceAlignment : MonoBehaviour
{
    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask groundLayerMask;
    public GameObject groundRayObj;
    private Vector2 defaultNormalizedRayCast = new(0, 1f);
    private Quaternion defaultNormalizedRotation = new(0, 0, 0, 1f);
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (rb.velocity.sqrMagnitude > 0.01f)
            SurfaceAlignment();
    }


    private void SurfaceAlignment()
    {
        // RaycastHit2D rayCastHit = Physics2D.Raycast(groundRayObj.transform.position, -Vector2.up);
        // Debug.DrawRay(groundRayObj.transform.position, -Vector2.up * rayCastHit.distance, Color.red);

        RaycastHit2D rayCastHit = Physics2D.Raycast(groundRayObj.transform.position, -Vector2.up, float.PositiveInfinity, groundLayerMask);
        groundDistance = rayCastHit.distance;
        Debug.DrawRay(groundRayObj.transform.position, -Vector2.up * rayCastHit.distance, Color.red);
        Debug.DrawRay(groundRayObj.transform.position, rayCastHit.normal, Color.blue);

        // Debug.Log(rayCastHit.normal.normalized + " * " + rayCastHit.distance + "*" + transform.rotation);

        //  if (Input.GetButton("Vertical"))

        if (rayCastHit.distance > 1f)
            transform.rotation = Quaternion.FromToRotation(transform.up, Vector2.up) * transform.rotation;

        //check surface alignment only if near surface, to avoid aligning in air
        if (rayCastHit.collider != null && rayCastHit.distance < 1f)
        {
            if (rayCastHit.normal.normalized != defaultNormalizedRayCast || transform.rotation != defaultNormalizedRotation)
            {
                // if (rayCastHit.distance > 0.05f)

                // Vector3 rotateTo = new Vector3(1f,1f,0.5f);
                //Debug.Log(rayCastHit.normal.x + "/" + rayCastHit.normal.y + " ? " + rayCastHit.normal.normalized);
                //Quaternion targetRotateTo = Quaternion.Euler(0,0,20f);
                //Debug.Log("rotating quater = " + Quaternion.LookRotation(rayCastHit.normal));
                // transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rayCastHit.normal), Time.deltaTime * 5.0f);

                // Quaternion rotationRayCast = Quaternion.LookRotation(Vector3.up, rayCastHit.normal);
                // transform.rotation = rotationRayCast;

                // Quaternion targetLocation = Quaternion.FromToRotation(transform.up, rayCastHit.normal) * transform.rotation;
                // transform.rotation = Quaternion.Slerp(transform.rotation, targetLocation, Time.deltaTime  * 3f );
                // transform.rotation = Quaternion.FromToRotation(transform.up, rayCastHit.normal) * transform.rotation;
                transform.rotation = Quaternion.FromToRotation(Vector2.up, rayCastHit.normal);

                //groundRayObj.transform.rotation = Quaternion.FromToRotation(transform.up, rayCastHit.normal) * transform.rotation;
            }
        }
    }

}
