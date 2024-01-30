using UnityEngine;
using UnityEngine.Events;

public class InGameEvent : MonoBehaviour
{
    [SerializeField] public UnityEvent OnGoal;
    [SerializeField] public UnityEvent OnStart;
}