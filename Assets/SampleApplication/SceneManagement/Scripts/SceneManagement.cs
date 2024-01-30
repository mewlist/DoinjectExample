using System.Threading.Tasks;
using Doinject;
using Mew.Core.Assets;
using Mew.Core.Tasks;
using UnityEngine;

public class SceneManagement : MonoBehaviour, IInjectableComponent
{
    [SerializeField] private UnifiedScene stageSelectScene = default;
    [SerializeField] private UnifiedScene titleScene = default;
    [SerializeField] private UnifiedScene inGameScene = default;

    private SceneContextLoader RootSceneContextLoader { get; set; }
    private BlackoutCurtain BlackoutCurtain { get; set; }

    [Inject]
    public void Construct(
        SceneContextLoader sceneContextLoader,
        [Optional] BlackoutCurtain blackoutCurtain)
    {
        TaskQueue.Start(destroyCancellationToken);
        RootSceneContextLoader = sceneContextLoader;
        BlackoutCurtain = blackoutCurtain;
    }

    public async Task OnInjected()
    {
        // Load initial scene
        await LoadTitle();
    }

    public async Task LoadTitle()
        => await Transit(titleScene);

    public async Task LoadStageSelect()
        => await Transit(stageSelectScene);
    public async Task LoadInGame(InGameArg inGameArg)
        => await Transit(inGameScene, inGameArg);

    private TaskQueue TaskQueue { get; } = new(TaskQueueLimitType.SwapLast, maxSize: 2);
    // Load main scene by using SceneContextLoader.
    // scene will be created as a child context of EntryPoint context.
    private async Task Transit(UnifiedScene to, IContextArg contextArg = null)
    {
        await TaskQueue.EnqueueAsync(async ct =>
        {
            if (BlackoutCurtain) await BlackoutCurtain.FadeOut();
            await RootSceneContextLoader.UnloadAllScenesAsync();
            await RootSceneContextLoader.LoadAsync(to, active: true, contextArg);
            if (BlackoutCurtain) await BlackoutCurtain.FadeIn();
        });
    }
}