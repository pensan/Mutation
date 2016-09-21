using UnityEngine;
using System.Collections;

public class CoroutineWithData
{
    public Coroutine coroutine { get; private set; }
    public object data;
    private IEnumerator target;
    public event System.Action<object> OnFinish;

    public bool IsFinished
    {
        get;
        private set;
    }

    public CoroutineWithData(MonoBehaviour owner, IEnumerator target, System.Action<object> OnFinishMethod = null)
    {
        this.target = target;
        this.coroutine = owner.StartCoroutine(Run());

        if (OnFinishMethod != null)
            OnFinish += OnFinishMethod;

        IsFinished = false;
    }

    private IEnumerator Run()
    {
        while (target.MoveNext())
        {
            data = target.Current;
            yield return data;
        }

        if (OnFinish != null)
            OnFinish(data);

        IsFinished = true;
    }
}
