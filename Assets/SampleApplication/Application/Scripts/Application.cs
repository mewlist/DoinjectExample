using Doinject;
using UnityEngine;

public class Application : MonoBehaviour, IInjectableComponent
{
    public IContext Context { get; set; }

    [Inject]
    public async void Construct(IContext context)
    {
        Context = context;
    }

    public void OnInjected()
    {
    }

    [RuntimeInitializeOnLoadMethod]
    public static void InitializeApplication()
    {
        UnityEngine.Application.targetFrameRate = 60;
    }
}
