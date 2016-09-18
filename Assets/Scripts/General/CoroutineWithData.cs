﻿using UnityEngine;
using System.Collections;

public class CoroutineWithData
{
    public Coroutine coroutine { get; private set; }
    public System.Object data;
    private IEnumerator target;
    public CoroutineWithData(MonoBehaviour owner, IEnumerator target)
    {
        this.target = target;
        this.coroutine = owner.StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        while (target.MoveNext())
        {
            data = target.Current;
            yield return data;
        }
    }
}
