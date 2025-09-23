using UnityEngine;

public class TornadoSpawnerButton : MonoBehaviour
{
    [SerializeField] private TornadoSpawner tornadoSpawner;
    [SerializeField] private float decreaseAmount = 1f; // mỗi lần bấm giảm bao nhiêu giây
    private bool firstPress = true;

    public void OnButtonClick()
    {
        if (tornadoSpawner == null) return;

        if (firstPress)
        {
            // lần đầu thì bật spawner
            tornadoSpawner.SetActive(true);
            firstPress = false;
        }
        else
        {
            // những lần sau giảm thời gian spawn
            tornadoSpawner.DecreaseSpawnInterval(decreaseAmount);
        }
    }
}
