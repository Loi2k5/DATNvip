using UnityEngine;

public class Conform : MonoBehaviour
{
    public void OnConfirmUpgrade()
    {
        FindObjectOfType<GameManager>().ResetEnergy();
    }

}
