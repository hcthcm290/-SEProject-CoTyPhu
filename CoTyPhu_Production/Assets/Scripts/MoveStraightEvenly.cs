using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStraightEvenly : MonoBehaviour
{
    // Do not move in X-axis
    public bool lockX = false;
    // Do not move in Y-axis
    public bool lockY = false;
    // Do not move in Z-axis
    public bool lockZ = false;

    // When set, automatically start moving
    public Vector3 Target
    {
        get
        {
            return _target;
        }
        set
        {
            _reachedTarget = false;
            _target = value;
        }
    }
    [SerializeField]
    private Vector3 _target;
    private bool _reachedTarget = false;
    public float speed = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_reachedTarget)
            return;

        Vector3 pos = transform.position;

        Vector3 direction = Target - pos;
        float dirX = direction.x;
        float dirY = direction.y;
        float dirZ = direction.z;

        //Debug.Log(direction);

        if (lockX)
            dirX = 0;

        if (lockY)
            dirY = 0;

        if (lockZ)
            dirZ = 0;

        Vector3 moveDir = (new Vector3(dirX, dirY, dirZ));

        if (moveDir.magnitude < speed * Time.deltaTime)
        {
            transform.position = new Vector3(
                lockX ? pos.x : Target.x,
                lockY ? pos.y : Target.y,
                lockZ ? pos.z : Target.z);
            _reachedTarget = true;
            return;
        }

        Vector3 newPos = pos + moveDir.normalized * (speed * Time.deltaTime);

        transform.position = newPos;
    }
    // Start moving towards target
    public void StartMove(Vector3 target)
    {
        Target = target;
    }
}
