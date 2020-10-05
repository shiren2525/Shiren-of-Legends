using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        bool result = false;
        //System.Action<bool> onFinished = i_result => result = i_result;
        WaitProcess(i_result => result = i_result);
    }

    IEnumerator WaitProcess(Action<bool> action)
    {
        yield return new WaitForSeconds(3.0f);

        // 何かこの処理に対して"結果"というものがあるとして….
        bool result = true;

        action(result);
    }

}