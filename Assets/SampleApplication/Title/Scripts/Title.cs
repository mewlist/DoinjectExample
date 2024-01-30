using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Doinject;
using Mew.Core.Assets;
using Mew.Core.TaskHelpers;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour, IInjectableComponent
{
    [SerializeField] private UnifiedScene backgroundScene = default;
    [SerializeField] private Button startButton;

    private SceneManagement SceneManagement { get; set; }
    private SceneContextLoader SceneContextLoader { get; set; }

    [Inject]
    public void Construct(SceneManagement sceneManagement, SceneContextLoader sceneContextLoader)
    {
        SceneManagement = sceneManagement;
        SceneContextLoader = sceneContextLoader;

        ObserveUIEvents();
    }

    public async Task OnInjected()
    {
        await LoadBackground();
    }

    private void ObserveUIEvents()
    {
        startButton.OnClickAsObservable()
            .Subscribe(_ => LoadStageSelectScene().Forget())
            .AddTo(this);
    }

    private async UniTask LoadBackground()
    {
        await SceneContextLoader.LoadAsync(backgroundScene, active: true);
    }

    private async UniTask LoadStageSelectScene()
    {
        await SceneManagement.LoadStageSelect();
    }
}
