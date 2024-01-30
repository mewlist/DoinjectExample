using Doinject;
using UnityEngine;

[RequireComponent(typeof(CharacterAction))]
public class Character : MonoBehaviour, IInjectableComponent
{
    private CharacterAction CharacterAction { get; set; }
    private InGameEvent InGameEvent { get; set; }

    [Inject]
    public void Construct(InGameEvent inGameEvent)
    {
        CharacterAction = GetComponent<CharacterAction>();
        InGameEvent = inGameEvent;
    }

    public void StartAction()
    {
        CharacterAction.InputEnabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Respawn"))
        {
            var spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
            CharacterAction.Teleport(spawnPoint.transform.position);
        }

        if (other.gameObject.CompareTag("Finish"))
        {
            CharacterAction.InputEnabled = false;
            InGameEvent.OnGoal.Invoke();
        }
    }
}