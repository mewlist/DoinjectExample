using Doinject;
using Doinject.Assets;
using Mew.Core.Assets;
using UnityEngine;

public class InGameUIInstaller : BindingInstallerComponent
{
    [SerializeField] private PrefabAssetReference resultUIPrefabAssetReference;
    [SerializeField] private Transform uiRootTransform;

    public override void Install(DIContainer container, IContextArg contextArg)
    {
        container.BindPrefabAssetReference<ResultUI>(resultUIPrefabAssetReference)
            .Under(uiRootTransform, worldPositionStays: false)
            .AsFactory();
    }
}
