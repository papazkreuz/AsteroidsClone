using UnityEngine;
using UnityEngine.UI;

public class LivesDisplay : MonoBehaviour
{
    [SerializeField] private Image _liveImagePrefab;
    private int _livesDisplayedCount;

    public void UpdateLivesCount(int count)
    {
        _livesDisplayedCount = transform.childCount;

        if (_livesDisplayedCount > count)
        {
            for (int i = 0; i < _livesDisplayedCount - count; i++)
            {
                Destroy(transform.GetChild(_livesDisplayedCount - i - 1).gameObject);
            }
        }
        else
        {
            for (int i = 0; i < count - _livesDisplayedCount; i++)
            {
                Instantiate(_liveImagePrefab, transform);
            }
        }
    }
}
