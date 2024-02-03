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

    [OnInjected]
    public void OnInjected()
    {
        Spawn();
    }

    public void StartAction()
    {
        CharacterAction.InputEnabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Respawn"))
        {
            Spawn();
        }

        if (other.gameObject.CompareTag("Finish"))
        {
            CharacterAction.InputEnabled = false;
            InGameEvent.OnGoal.Invoke();
        }
    }

    private void Spawn()
    {
        var spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
        if (spawnPoint) CharacterAction.Teleport(spawnPoint.transform.position);
    }
}