using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class DelayObject : MonoBehaviour
{
    float duration;
    FutureTask<bool> taskOnComplete;

    private void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0)
        {
            taskOnComplete.Complete(true);
            Destroy(this.gameObject, 0.1f);
        }
    }

    public Future<bool> Init(float duration)
    {
        this.duration = duration;
        taskOnComplete = new FutureTask<bool>();
        return taskOnComplete.GetFuture();
    }
}
