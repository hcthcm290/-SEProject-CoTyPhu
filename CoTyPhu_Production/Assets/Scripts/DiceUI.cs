using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DiceUI : MonoBehaviour
{
    [SerializeField] List<Vector3> ListCrossVectors;
    
    int result;
    [SerializeField] Rigidbody rb;
    public Vector3 diceVelocity;

    bool rolled = false;

    private void Update()
    {
        diceVelocity = rb.velocity;

        if (rolled && rb.angularVelocity == Vector3.zero && rb.velocity == Vector3.zero )
        {
            Vector3 smallestAngleVector = new Vector3();
            float smallestAngle = 180;

            foreach (var vector in ListCrossVectors)
            {
                var rotation = transform.rotation;
                var csna = rotation * vector;
                var dg = Vector3.Angle(new Vector3(0, 1, 0), rotation * vector);
                if (Vector3.Angle(new Vector3(0, 1, 0), rotation*vector) < smallestAngle)
                {
                    smallestAngle = Vector3.Angle(new Vector3(0, 1, 0), rotation * vector);
                    smallestAngleVector = vector;
                }
            }
            Debug.Log("result: " + (ListCrossVectors.IndexOf(smallestAngleVector) + 1).ToString());
            rolled = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            float dirX = UnityEngine.Random.Range(0, 500);
            float dirY = UnityEngine.Random.Range(0, 500);
            float dirZ = UnityEngine.Random.Range(0, 500);

            transform.position = new Vector3(transform.position.x, 2, transform.position.z);
            transform.rotation = Quaternion.identity;
            rb.AddForce(transform.up * 500);
            rb.AddTorque(dirX, dirY, dirZ);

            rolled = true;
        }
    }
}
