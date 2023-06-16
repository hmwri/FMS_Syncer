using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Syncer : MonoBehaviour
{
    [SerializeField] private RealtimeCommunication realtimeCommunication;
    [SerializeField] private Transform head;
    [SerializeField] private Transform pelvis;
    [SerializeField] private Transform rightHand;
    [SerializeField] private Transform leftHand;

    [SerializeField] private Transform parent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (realtimeCommunication != null && realtimeCommunication.OpponentData != null)
        {
            parent.position = realtimeCommunication.OpponentData.parentPosition;
            parent.rotation = realtimeCommunication.OpponentData.parentRotation;
            //Debug.Log(realtimeCommunication.OpponentData.position);
            head.position = realtimeCommunication.OpponentData.headPosition;
            head.rotation = realtimeCommunication.OpponentData.headRotation;
            pelvis.position = realtimeCommunication.OpponentData.pelvisPosition;
            rightHand.position = realtimeCommunication.OpponentData.rightHandPosition;
            leftHand.position = realtimeCommunication.OpponentData.headPosition;
        }
    }
}
