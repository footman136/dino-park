using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DinoPark;

namespace LowPolyAnimalPack
{
  public class AnimalManager : MonoBehaviour
  {
    [SerializeField]
    private bool peaceTime;
    public bool PeaceTime
    {
      get
      {
        return peaceTime;
      }
      set
      {
        SwitchPeaceTime(value);
      }
    }

    private static AnimalManager instance;
    public static AnimalManager Instance
    {
      get
      {
        return instance;
      }
    }

    public enum ANIMAL_TYPE
    {
        PLAYER = 0,
        EGG = 1,
        TREE = 2,
        BRACHIO = 3,
        TREX = 4,
        COUNT = 5,
    };
    
    
    private string [] ROOT_NAMES = {
      "Players", "Eggs", "Tree", "Brachio", "TRex", 
    };

    public Transform[] Roots;
    private void Awake()
    {
      if (instance != null && instance != this)
      {
        Destroy(gameObject);
        return;
      }

      instance = this;

      Roots = new Transform[(int)ANIMAL_TYPE.COUNT];
      for (int i = 0; i < (int)ANIMAL_TYPE.COUNT; ++i)
      {
        var go = new GameObject();
        Roots[i] = go.transform;
        Roots[i].parent = transform;
        Roots[i].name = ROOT_NAMES[i];
      }
    }

    private void Start()
    {
      if (peaceTime)
      {
        Debug.Log("AnimalManager: Peacetime is enabled, all animals are non-agressive.");
        SwitchPeaceTime(true);
      }
    }

    private const float TIME_DELAY = 1f; 
    private float _lastTime = 0;
    void Update()
    {
      _lastTime += Time.deltaTime;
      if (_lastTime < TIME_DELAY)
      {
        return;
      }
      _lastTime = 0;
      
      // 刷新个体的数量
      for (int i = 0; i < (int) ANIMAL_TYPE.COUNT; ++i)
      {
        Roots[i].name = ROOT_NAMES[i] + " (" + Roots[i].childCount + ")";
      }
    }

    public void SwitchPeaceTime(bool enabled)
    {
      if (enabled == peaceTime)
      {
        return;
      }

      peaceTime = enabled;

      Debug.Log(string.Format("AnimalManager: Peace time is now {0}.", enabled ? "On" : "Off"));
			foreach (WanderScript animal in WanderScript.AllAnimals)
      {
        animal.SetPeaceTime(enabled);
      }
      foreach (var animal in DinoBehaviour.AllAnimals)
      {
        animal.Value.SetPeaceTime(enabled);
      }
    }

    public void Nuke()
    {
			foreach (WanderScript animal in WanderScript.AllAnimals)
      {
        animal.Die();
      }
      foreach (var animal in DinoBehaviour.AllAnimals)
      {
        animal.Value.Die();
      }
    }
  }
}