using Doinject;
using TMPro;
using UnityEngine;

public class ResultUI : MonoBehaviour, IInjectableComponent
{
    [SerializeField] TMP_Text stageNameText;

    [Inject]
    public void Construct(IContextArg contextArg)
    {
        if (contextArg is InGameArg inGameContextArg)
            stageNameText.text = $"Stage {inGameContextArg.StageIndex + 1} Clear!";
        GetComponent<Animator>().SetTrigger("OnGoal");
    }
}
