using Doinject;
using UnityEngine;

public class Application : MonoBehaviour, IInjectableComponent
{
    public IContext Context { get; set; }

    [Inject]
    public void Construct(IContext context)
    {
        Context = context;
    }

    [OnInjected]
    public void OnInjected()
    {
    }

    [RuntimeInitializeOnLoadMethod]
    public static void InitializeApplication()
    {
        UnityEngine.Application.targetFrameRate = 60;
    }
}
