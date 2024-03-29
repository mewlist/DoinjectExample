using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doinject;
using Mew.Core.Extensions;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour, IInjectableComponent
{
    [SerializeField] List<Button> selectButtons;

    private SceneManagement SceneManagement { get; set; }

    [Inject]
    public void Construct(IContext context, SceneManagement sceneManagement)
    {
        SceneManagement = sceneManagement;
    }

    [OnInjected]
    public void OnInjected()
    {
        selectButtons
            .Select((x, i) => x.OnClickAsObservable().Select(_ => i))
            .Merge()
            .Take(1)
            .Subscribe(i => LoadStage(i).Forget())
            .AddTo(this);
    }

    private async Task LoadStage(int stageIndex)
    {
        // Pass InGameArg to InGame scene.
        await SceneManagement.LoadInGame(new InGameArg { StageIndex = stageIndex });
    }
}