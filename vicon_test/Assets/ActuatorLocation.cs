using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActuatorLocation : MonoBehaviour
{
    public Transform elbowTransform;
    public Transform wristTransform;
    public Transform[] actuators;
    public float[] tArray;
    public int actuatorsCount;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(actuatorsCount == 0)
        {
            return;
        }

        for(int i = 0; i < actuatorsCount; i++)
        {
            actuators[i].localPosition = Vector3.Lerp(wristTransform.position, elbowTransform.position, tArray[i]);
            actuators[i].localRotation = Quaternion.Lerp(wristTransform.rotation, elbowTransform.rotation, tArray[i]);
        }
    }
}
