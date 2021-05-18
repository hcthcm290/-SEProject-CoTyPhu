using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Event Card Draw Animation
/// Assumes no parent game object
/// </summary>
public class EventCardDraw : MonoBehaviour
{
    public Transform targetTransform;
    public float duration = 1f;
    
    public Vector3 initialPos;
    public Quaternion initialRot;
    public Vector3 initialScale;

    public Vector3 targetPos;
    public Quaternion targetRot;
    public Vector3 targetScale;

    public float acumulatedTime = 0f;
    [SerializeField]
    private bool active;
    public bool Active
    {
        get => active;
        set
        {
            bool prev = active;
            if (value && !prev)
                Reset();
            active = value;
            if (!value && prev)
                PerformOnTargetReached();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    private void Reset()
    {
        initialPos = transform.position;
        initialRot = transform.rotation;
        initialScale = transform.localScale;

        targetPos = targetTransform.position;
        targetRot = targetTransform.rotation;
        targetScale = targetTransform.localScale;

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
            transform.position = targetPos;
            transform.rotation = targetRot;
            transform.localScale = targetScale;
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

        /// Calculate new Scale ////////////////////////////////////////////
        Vector3 initialScaleWeight = initialScale;
        initialScaleWeight.Scale(new Vector3(remaining, remaining, remaining));

        Vector3 targetScaleWeight = targetScale;
        targetScaleWeight.Scale(new Vector3(progress, progress, progress));

        Vector3 newScale = initialScaleWeight + targetScaleWeight;
        //////////////////////////////////////////////////////////////////////

        transform.position = newPos;
        transform.rotation = newRot;
        transform.localScale = newScale;
    }

    /// <summary>
    /// Event triggers when the target reaches destination
    /// and stop moving. 
    /// </summary>
    List<IAction> OnTargetReached = new List<IAction>();
    public void ListenTargetReached(IAction action)
    {
        OnTargetReached.Add(action);
    }
    // note: Clears OnTargetReached whenever Target is reached
    // note: Allow Action to reregister in PerformAction()
    // Action: { ...; moveComponent.ListenTargetReached(this); ...; }
    private void PerformOnTargetReached()
    {
        List<IAction> temp = OnTargetReached;
        OnTargetReached = new List<IAction>();

        foreach (IAction item in temp)
            item.PerformAction();
    }
}
