using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggStats : MonoBehaviour
{
    [SerializeField, Tooltip("孵蛋的时间")]
    public float hatchTime = 30f;
    
    [SerializeField, Tooltip("蛋的营养")]
    public float eggFood = 50f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
