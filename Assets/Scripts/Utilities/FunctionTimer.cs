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

    public static FunctionTimer CreateRandom(Action action, float timerMin, float timerMax, bool runOnce = false)
    {
        GameObject gameObject = new GameObject("FunctionTimerRandom", typeof(MonoBehaviourHook));
        FunctionTimer functionTimer = new FunctionTimer(action, timerMin, timerMax, gameObject, runOnce);
        gameObject.GetComponent<MonoBehaviourHook>().onUpdate = functionTimer.Update;

        return functionTimer;
    }

    private GameObject gameObject;
    private Action action;
    private float timerMin;
    private float timerMax;
    private float timer;
    private bool isDestroyed;
    private bool runOnce;
    private bool randomizeTimer;

    public FunctionTimer(Action action, float timer, GameObject gameObject, bool runOnce)
    {
        this.action = action;
        this.timerMax = timer;
        this.timer = timer;
        this.isDestroyed = false;
        this.gameObject = gameObject;
        this.runOnce = runOnce;

        timerMin = 0;
        randomizeTimer = false;
    }

    public FunctionTimer(Action action, float timerMin, float timerMax, GameObject gameObject, bool runOnce)
    {
        this.action = action;
        this.timerMax = timerMax;
        this.timer = timerMax;
        this.isDestroyed = false;
        this.gameObject = gameObject;
        this.runOnce = runOnce;

        this.timerMin = timerMin;
        randomizeTimer = true;
    }

    public void Update()
    {
        if(!isDestroyed)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                action();
                if(runOnce)
                {
                    DestroySelf();
                }
                else
                {
                    if(randomizeTimer)
                    {
                        timer += UnityEngine.Random.Range(timerMin, timerMax);
                    }
                    else
                    {
                        timer += timerMax;
                    }                    
                }
            }
        }
    }

    private void DestroySelf()
    {
        isDestroyed = true;
        UnityEngine.Object.Destroy(gameObject);
    }

    public float GetTimer() => timer;

    public void SetTimer(float timer, bool resetImmediately = false)
    {
        timerMax = timer;
        randomizeTimer = false;

        if (resetImmediately)
        {
            this.timer = timer;
        }
    }

    public void SetTimerByRandomRange(float timerMin, float timerMax)
    {
        this.timerMax = UnityEngine.Random.Range(timerMin, timerMax);
        randomizeTimer = true;
    }
}
