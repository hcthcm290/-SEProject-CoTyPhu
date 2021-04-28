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
    // the amount of time needed to confirm the dice is no longer moving.
    const float inactiveConfirmDelay = 0.5f;
    float inactiveDuration;

    public delegate void DiceResult(int result);
    public event DiceResult ReceiveResult;

    private void Update()
    {
        diceVelocity = rb.velocity;

        if (rolled && rb.angularVelocity == Vector3.zero && rb.velocity == Vector3.zero)
        {
            Vector3 smallestAngleVector = new Vector3();
            float smallestAngle = 180;

            foreach (var vector in ListCrossVectors)
            {
                var rotation = transform.rotation;
                var csna = rotation * vector;
                var dg = Vector3.Angle(new Vector3(0, 1, 0), rotation * vector);
                if (Vector3.Angle(new Vector3(0, 1, 0), rotation * vector) < smallestAngle)
                {
                    smallestAngle = Vector3.Angle(new Vector3(0, 1, 0), rotation * vector);
                    smallestAngleVector = vector;
                }
            }
            inactiveDuration += Time.deltaTime;
            if (inactiveDuration > inactiveConfirmDelay)
            {
                rolled = false;
                Debug.Log("result: " + (ListCrossVectors.IndexOf(smallestAngleVector) + 1).ToString());
                ReceiveResult.Invoke(ListCrossVectors.IndexOf(smallestAngleVector) + 1);
            }
        }
        else
            inactiveDuration = 0;
    }

    public void Roll()
    {
        Vector3 torque = new Vector3();
        torque.x = UnityEngine.Random.Range(0, 500);
        torque.y = UnityEngine.Random.Range(0, 500);
        torque.z = UnityEngine.Random.Range(0, 500);

        transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        transform.rotation = Quaternion.identity;
        rb.velocity = transform.up * 6;
        rb.AddTorque(torque);

        rolled = true;

        
    }
}
