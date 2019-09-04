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
    
    private const string ROOT_PLAYER_NAME = "Players"; 
    private const string ROOT_DINO_NAME = "Dinos"; 
    private const string ROOT_PLANT_NAME = "Plants"; 
    private const string ROOT_EGG_NAME = "Eggs"; 
      
    public Transform RootPlayers { set; get; }
    public Transform RootDinos { set; get; }
    public Transform RootPlants { set; get; }
    public Transform RootEggs { set; get; }

    private void Awake()
    {
      if (instance != null && instance != this)
      {
        Destroy(gameObject);
        return;
      }

      instance = this;
      var go = new GameObject();
      RootPlayers = go.transform;
      RootPlayers.parent = transform;
      RootPlayers.name = ROOT_PLAYER_NAME;
       go = new GameObject();
      RootDinos = go.transform;
      RootDinos.parent = transform;
      RootDinos.name = ROOT_DINO_NAME;
      go = new GameObject();
      RootPlants = go.transform;
      RootPlants.parent = transform;
      RootPlants.name = ROOT_PLANT_NAME;
      go = new GameObject();
      RootEggs = go.transform;
      RootEggs.parent = transform;
      RootEggs.name = ROOT_EGG_NAME;
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
      RootPlayers.name = ROOT_PLAYER_NAME + " (" + RootPlayers.childCount + ")";
      RootDinos.name = ROOT_DINO_NAME + " (" + RootDinos.childCount + ")";
      RootPlants.name = ROOT_PLANT_NAME + " (" + RootPlants.childCount + ")";
      RootEggs.name = ROOT_EGG_NAME + " (" + RootEggs.childCount + ")";
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