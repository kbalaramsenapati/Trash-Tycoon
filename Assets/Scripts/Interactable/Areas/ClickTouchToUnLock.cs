using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ClickTouchToUnLock : MonoBehaviour
{
    public UnityEvent unityEvent;

    private void OnMouseDown()
    {
        unityEvent.Invoke();
    }
}
