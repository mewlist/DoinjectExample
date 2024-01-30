using UnityEngine;

public class ResultUI : MonoBehaviour
{
    public void Show()
    {
        GetComponent<Animator>().SetTrigger("OnGoal");
    }
}
