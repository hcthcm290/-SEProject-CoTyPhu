using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicEffect : MonoBehaviour
{
    public Vector3 initialPos;
    public Vector3 pivotPos;
    public Vector3 targetPos;

    public float MaxTime = 5.0f;
    public float TimePassed = 0f;
    float accel;

    private Vector3 pivotToTarget;
    private Vector3 pivotToInitial;

    private bool hasStart = false;
    public IAction onComplete;

    public bool HasStart
    {
        get => hasStart; set
        {
            if (hasStart == false && value == true)
                Init();
            bool prev = hasStart;
            hasStart = value;
            if (prev == true && value == false)
                onComplete?.PerformAction();
                
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void Init()
    {
        initialPos = transform.position;
        accel = 1.0f / MaxTime / MaxTime;

        pivotToInitial = initialPos - pivotPos;
        pivotToTarget = targetPos - pivotPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (!HasStart)
            return;
        TimePassed += Time.deltaTime;
        if (TimePassed >= MaxTime)
        {
            HasStart = false;
            TimePassed = MaxTime;
        }

        float alpha = accel * TimePassed * TimePassed;
        float beta;
        {
            float temp = MaxTime - TimePassed;
            beta = accel * temp * temp;
        }

        Vector3 newPos = pivotPos + pivotToInitial * beta + pivotToTarget * alpha;
        transform.position = newPos;
    }
}
