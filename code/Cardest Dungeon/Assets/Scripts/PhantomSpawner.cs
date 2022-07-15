using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhantomSpawner : MonoBehaviour, IDungeonSwapMessage
{
    [SerializeField]
    private GameObject phantomPrefab;

    private GameObject phantomInstance;
    
    // Start is called before the first frame update
    void Start()
    {
        TrySpawningPhantom();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDungeonSwap()
    {
        TrySpawningPhantom();
    }

    private void TrySpawningPhantom()
    {
        if (phantomInstance == null)
        {
            phantomInstance = Instantiate(phantomPrefab, Vector3.zero, Quaternion.identity);
            phantomInstance.transform.SetParent(transform, false);
        }
    }
}
