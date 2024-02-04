using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cinemachine;
using Doinject;
using Mew.Core.Assets;
using Mew.Core.Extensions;
using TMPro;
using UnityEngine;

public class InGame : MonoBehaviour, IInjectableComponent
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private List<UnifiedScene> stages;

    private IContext Context { get; set; }
    private IFactory<Character> CharacterFactory { get; set; }
    private CinemachineClearShot CinemachineClearShot { get; set; }
    private InGameEvent InGameEvent { get; set; }
    private SceneManagement SceneManagement { get; set; }
    private IFactory<ResultUI> ResultUIFactory { get; set; }
    private InGameArg InGameArg { get; set; }

    [Inject]
    public void Construct(
        IContext context,
        IFactory<Character> characterFactory,
        CinemachineClearShot cinemachineClearShot,
        InGameEvent inGameEvent,
        [Optional] SceneManagement sceneManagement,
        [Optional] BlackoutCurtain blackoutCurtain,
        IFactory<ResultUI> resultUIFactory,
        IContextArg arg)
    {
        Context = context;
        CharacterFactory = characterFactory;
        CinemachineClearShot = cinemachineClearShot;
        InGameEvent = inGameEvent;
        SceneManagement = sceneManagement;
        ResultUIFactory = resultUIFactory;
        InGameArg = arg as InGameArg;

        if (InGameArg is not null)
            titleText.text = $"Stage {InGameArg?.StageIndex + 1}";

        if (blackoutCurtain) blackoutCurtain.FadeIn().Forget();
    }

    [OnInjected]
    public async Task OnInjected()
    {
        if (InGameArg is not null)
            await Context.SceneContextLoader.LoadAsync(stages[InGameArg.StageIndex], active: true);

        InGameEvent.OnGoal.AddListener(() =>
        {
            ReturnToStageSelect().Forget();
        });

        var character = await CharacterFactory.CreateAsync();
        CinemachineClearShot.LookAt = character.transform;
        CinemachineClearShot.Follow = character.transform;
        character.StartAction();
    }

    private async Task ReturnToStageSelect()
    {
        await ResultUIFactory.CreateAsync();

        await Task.Delay(TimeSpan.FromSeconds(4f));
        if (SceneManagement) await SceneManagement.LoadStageSelect();
    }
}
