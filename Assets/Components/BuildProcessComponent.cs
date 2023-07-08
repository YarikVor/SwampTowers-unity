using UnityEngine;

public class BuildProcessComponent : MonoBehaviour
{
    private GameObject _spawningTower;

    public void Init(float delaySeconds, GameObject tower)
    {
        GetComponent<Animator>().speed = 1 / delaySeconds;

        _spawningTower = tower;
        _spawningTower.SetActive(false);

        Destroy(gameObject, delaySeconds);
    }

    private void OnDestroy()
    {
        _spawningTower?.SetActive(true);
    }
}
