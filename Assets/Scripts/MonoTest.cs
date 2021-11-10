using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MonoTest : MonoBehaviour
{
    private async UniTaskVoid Start()
    {
        await UniTask.RunOnThreadPool(SharedState.ModifyState);
    }
}

public class SharedState
{
    public static int sum;
    private static object LockObj = new object();

    public static async UniTask ModifyState()
    {
        //lock (LockObj)
        {
            for (int i = 0; i < 90_000_000; i++)
            {
                sum += 1;
            }
        }

        Debug.Log($"Sum: {sum}");
    }
}