using UnityEngine;
using System.Collections;

public class CoroutineWithData
{
    public Coroutine coroutine { get; private set; }
    public object data;
    private IEnumerator target;
    public event System.Action<object> OnFinish;

    public CoroutineWithData(MonoBehaviour owner, IEnumerator target, System.Action<object> OnFinishMethod = null)
    {
        this.target = target;
        this.coroutine = owner.StartCoroutine(Run());

        if (OnFinishMethod != null)
            OnFinish += OnFinishMethod;
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
    }
}
