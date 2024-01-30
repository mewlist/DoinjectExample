using Doinject;
using UnityEngine;

public class SceneManagementInstaller : BindingInstallerComponent
{
    [SerializeField] SceneManagement sceneManagement;

    public override void Install(DIContainer container, IContextArg contextArg)
    {
        base.Install(container, contextArg);
        // Bind your dependencies here
        container.BindFromInstance(sceneManagement);
    }
}
