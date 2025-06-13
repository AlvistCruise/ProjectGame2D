using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMDontDestroyOnLoad : MonoBehaviour
{
    public BGMDontDestroyOnLoad Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Destroy(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
