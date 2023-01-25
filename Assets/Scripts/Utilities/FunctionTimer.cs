using System;
using UnityEngine;

public class FunctionTimer
{
   private class MonoBehaviourHook : MonoBehaviour
    {
        public Action onUpdate;

        private void Update()
        {
            onUpdate?.Invoke();
        }
    }

    public static FunctionTimer Create(Action action, float timer, bool runOnce = false)
    {
        GameObject gameObject = new GameObject("FunctionTimer", typeof(MonoBehaviourHook));
        FunctionTimer functionTimer = new FunctionTimer(action, timer, gameObject, runOnce);
        gameObject.GetComponent<MonoBehaviourHook>().onUpdate = functionTimer.Update;

        return functionTimer;
    }

    private GameObject gameObject;
    private Action action;
    private float timerMax;
    private float timer;
    private bool isDestroyed;
    private bool runOnce;

    public FunctionTimer(Action action, float timer, GameObject gameObject, bool runOnce)
    {
        this.action = action;
        this.timerMax = timer;
        this.timer = timer;
        this.isDestroyed = false;
        this.gameObject = gameObject;
        this.runOnce = runOnce;
    }

    public void Update()
    {
        if(!isDestroyed)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                action();
                if(!runOnce)
                {
                    DestroySelf();
                }
                else
                {
                    timer += timerMax;
                }
            }
        }
    }

    private void DestroySelf()
    {
        isDestroyed = true;
        UnityEngine.Object.Destroy(gameObject);
    }
}
