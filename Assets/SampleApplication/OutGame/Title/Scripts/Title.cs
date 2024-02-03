using System.Threading.Tasks;
using Doinject;
using Mew.Core.Extensions;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour, IInjectableComponent
{
    [SerializeField] private Button startButton;

    private SceneManagement SceneManagement { get; set; }

    [Inject]
    public void Construct(SceneManagement sceneManagement)
    {
        SceneManagement = sceneManagement;
        ObserveUIEvents();
    }

    private void ObserveUIEvents()
    {
        startButton.OnClickAsObservable()
            .Subscribe(_ => LoadStageSelectScene().Forget())
            .AddTo(this);
    }

    private async Task LoadStageSelectScene()
    {
        await SceneManagement.LoadStageSelect();
    }
}
