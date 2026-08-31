using System;
using UnityEngine;

public class AnimEvents : MonoBehaviour
{
    public event Action OnStepLanded;

    private void OnStep()
    {
        OnStepLanded?.Invoke();
    }
}
