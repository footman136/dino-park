using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DinoPark
{
    public class LogicTree : MonoBehaviour
    {
        private static List<LogicTree> allTrees = new List<LogicTree>();
        public static List<LogicTree> AllTrees { get { return allTrees; } }
    
        void Awake()
        {
            allTrees.Add(this);
        }
        // Start is called before the first frame update
        void Start()
        {
            
        }
    
        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
