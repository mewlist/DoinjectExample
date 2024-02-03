using Doinject;
using Doinject.Assets;
using Mew.Core.Assets;
using UnityEngine;

[CreateAssetMenu(menuName = "Doinject/Installers/SceneManagementInstaller", fileName = "SceneManagementInstaller", order = 0)]
public class SceneManagementInstaller : BindingInstallerScriptableObject
{
    [SerializeField] private PrefabAssetReference sceneManagementPrefab;

    public override void Install(DIContainer container, IContextArg contextArg)
    {
        container
            .BindPrefabAssetReference<SceneManagement>(sceneManagementPrefab)
            .AsSingleton();
    }
}
