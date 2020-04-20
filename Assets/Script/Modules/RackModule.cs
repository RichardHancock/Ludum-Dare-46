using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RackModule : Pickup
{
    [System.Serializable]
    public enum ModuleType
    {
        HardDrive,
        Compute,
        Core
    }

    public ModuleType Type;

    protected int RawOutput;

    protected int Age = 0;

    private bool Active = false;
    public bool Animated = false;
    protected bool AnimationFinished = false;
    protected Vector3 AnimationStartPoint;
    protected Vector3 AnimationEndPoint;
    private float animationTimePos = 0.0f;

    public int FailureThresholdMin;
    public int FailureThresholdMax;
    public int FailurePoint;

    public AnimationCurve ModuleAnimationCurve;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RunAnimation();
    }

    public void SetAnimationPoints(Vector3 start, Vector3 finish)
    {
        AnimationStartPoint = start;
        AnimationEndPoint = finish;
    }

    protected virtual void RunAnimation()
    {
        if (!AnimationFinished && Active && Animated)
        {
            animationTimePos += Time.deltaTime;
            transform.position = Vector3.Lerp(AnimationStartPoint, AnimationEndPoint, ModuleAnimationCurve.Evaluate(animationTimePos));
        }
    }

    public void ActivateModule()
    {
        if (Active)
            return;

        InvokeRepeating("AgeModule", 1.0f, 10.0f);

        if (!Animated)
            AnimationFinished = true;

        Active = true;

        Debug.Log(Active);
    }

    public void ResetModule()
    {
        Active = false;
        Age = 0;
        AnimationFinished = false;
        animationTimePos = 0.0f;
    }

    private void AgeModule()
    {
        Age += 1;
    }

    public bool LocalFailCheck()
    {
        if (!Active)
            return false;

        if ((Random.Range(FailureThresholdMin, FailureThresholdMax) + Age) > FailurePoint)
        {
            //TODO Display Failure and Test Algorithm
            Debug.Log(gameObject.name + "Failed");
            CancelInvoke();
            Active = false;
            return true;
        }

        return false;
    }

    public virtual int GetOutput()
    {
        //TODO (Maybe Done)have output get affected by failures
        if (!Active)
            return 0;

        return RawOutput;
    }
}
