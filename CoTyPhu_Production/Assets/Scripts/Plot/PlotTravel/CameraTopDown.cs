using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTopDown : MonoBehaviour
{
    public Transform targetTransform;
    public Camera targetCamera;
    public float duration = 1f;

    public Vector3 initialPos;
    public Quaternion initialRot;
    public float initialSize;

    public Vector3 targetPos;
    public Quaternion targetRot;
    public float targetSize;

    public float acumulatedTime = 0f;
    [SerializeField]
    private bool active;
    [SerializeField]
    int count;
    public bool Active
    {
        get => active;
        set
        {
            if (value)
                count++;
            else
                count--;
            bool prev = active;
            active = value;
            if (!value && prev)
                PerformOnTargetReached();
            if (!value && count > 0)
            {
                active = true;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;
        if (targetTransform == null)
            targetTransform = targetCamera.transform;

        targetPos = transform.position;
        targetRot = transform.rotation;

        initialSize = targetCamera.orthographicSize;
        initialPos = targetTransform.position;
        initialRot = targetTransform.rotation;

        acumulatedTime = 0;
    }

    private void Rearm()
    {
        Vector3 temp1 = initialPos;
        initialPos = targetPos;
        targetPos = temp1;

        Quaternion temp2 = initialRot;
        initialRot = targetRot;
        targetRot = temp2;

        float temp3 = initialSize;
        initialSize = targetSize;
        targetSize = temp3;

        acumulatedTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Active)
            return;
        acumulatedTime += Time.deltaTime;

        if (acumulatedTime > duration)
        {
            targetTransform.position = targetPos;
            targetTransform.rotation = targetRot;
            targetCamera.orthographicSize = targetSize;
            Active = false;
            return;
        }

        float progress = acumulatedTime / duration;
        float remaining = 1 - progress;


        /// Calculate new Position ////////////////////////////////////////////
        Vector3 initialPosWeight = initialPos;
        initialPosWeight.Scale(new Vector3(remaining, remaining, remaining));

        Vector3 targetPosWeight = targetPos;
        targetPosWeight.Scale(new Vector3(progress, progress, progress));

        Vector3 newPos = initialPosWeight + targetPosWeight;
        //////////////////////////////////////////////////////////////////////


        /// Calculate new Rotation ////////////////////////////////////////////
        float deltaDegree = Quaternion.Angle(initialRot, targetRot);

        float rotation = deltaDegree * progress;

        Quaternion newRot = Quaternion.RotateTowards(
            initialRot, targetRot, rotation);
        //////////////////////////////////////////////////////////////////////

        /// Calculate new Size ///////////////////////////////////////////////
        float newSize = progress * targetSize + remaining * initialSize;
        //////////////////////////////////////////////////////////////////////

        targetTransform.position = newPos;
        targetTransform.rotation = newRot;
        targetCamera.orthographicSize = newSize;
    }
    #region Event
    /// <summary>
    /// Event triggers when the target reaches destination
    /// and stop moving. 
    /// </summary>
    List<List<IAction>> OnTargetReached = new List<List<IAction>>();
    public void ListenTargetReached(IAction action)
    {
        while (OnTargetReached.Count <= count - 1)
            OnTargetReached.Add(new List<IAction>());
        OnTargetReached[count - 1].Add(action);
    }
    // note: Clears OnTargetReached whenever Target is reached
    // note: Allow Action to reregister in PerformAction()
    // Action: { ...; Active = true; moveComponent.ListenTargetReached(this); ...; }
    private void PerformOnTargetReached()
    {
        if (OnTargetReached.Count > 0)
        {
            foreach (IAction item in OnTargetReached[0])
                item.PerformAction();

            OnTargetReached.RemoveAt(0);
        }
        Rearm();
    }
    #endregion

    #region Singleton
    public CameraTopDown()
    {
        Locator.MarkInstance(this);
    }
    public static CameraTopDown GetInstance()
    {
        return Locator.GetInstance<CameraTopDown>();
    }
    #endregion
}
