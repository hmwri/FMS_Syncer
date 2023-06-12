using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Syncer : MonoBehaviour
{
    [SerializeField] private RealtimeCommunication realtimeCommunication;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (realtimeCommunication != null && realtimeCommunication.OpponentData != null)
        {
            //Debug.Log(realtimeCommunication.OpponentData.position);
            transform.position = realtimeCommunication.OpponentData.position;
        }
    }
}
