using UnityEngine;

public class IngredientTimer : MonoBehaviour
{
    [SerializeField] private float timeToDestroy;
    private float _time;
    private void Update()
    {
        _time += Time.deltaTime;
        if (_time > timeToDestroy)
        {
            Destroy(gameObject);
        }
    }
}
