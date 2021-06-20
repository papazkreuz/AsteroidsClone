using UnityEngine;

public class SpawnZone : MonoBehaviour
{
    private int _objectsInZoneCount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _objectsInZoneCount++;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _objectsInZoneCount--;
    }

    private void Start()
    {
        _objectsInZoneCount = 0;
    }

    public bool isReadyToSpawn => _objectsInZoneCount == 0;
}
