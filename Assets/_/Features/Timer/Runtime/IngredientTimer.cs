using UnityEngine;

public class IngredientTimer : MonoBehaviour
{
    [SerializeField] private float timeToDestroy;
    [SerializeField] private ParticleSystem VFXActiveToDespawn;
    [SerializeField] private GameObject DesactivateRendu;
    private float _time;
    private float _timeToDestroy;
    private void Start()
    {
        _time = 0;
        _timeToDestroy = timeToDestroy + 1;
    }
    private void Update()
    {
        _time += Time.deltaTime;
        if (_time > timeToDestroy)
        {
            VFXActiveToDespawn.Play();
            DesactivateRendu.gameObject.SetActive(false);
            if (_time > _timeToDestroy)
                Destroy(gameObject);
            //Destroy(gameObject);
        }
    }
}
