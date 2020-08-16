using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventComponent : MonoBehaviour
{
    public UnityEvent Callback;

    public void Call()
    {
        Callback?.Invoke();
    }
}
