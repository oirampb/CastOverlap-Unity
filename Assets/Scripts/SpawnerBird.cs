using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBird : MonoBehaviour
{

    void Update()
    {
        _time += Time.deltaTime;
        if (_time >= _timeToSpawn)
        {
            Instantiate(_birdPrefab);
            _time = 0;
        }
    }
    [SerializeField]
    private GameObject _birdPrefab = null;
    [SerializeField]
    private float _timeToSpawn = 3;
    private float _time;
}
