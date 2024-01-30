using Doinject;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterInstaller", menuName = "Doinject/Installers/CharacterInstaller")]
public class CharacterInstaller : BindingInstallerScriptableObject
{
    [SerializeField] private CharacterAction characterPrefab;
    public override void Install(DIContainer container, IContextArg contextArg)
    {
        // Bind your dependencies here
        container
            .BindPrefab<Character>(characterPrefab)
            .AsFactory();
    }
}
