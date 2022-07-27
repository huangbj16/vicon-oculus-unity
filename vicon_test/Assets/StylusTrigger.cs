using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class StylusTrigger : MonoBehaviour
{
    public GameObject button;
    public GameObject[] actuators;
    public VisualEffect[] visualEffects;
    public TcpSender sender;
   private DataCollection dataCollection;

    // Start is called before the first frame update
    void Start()
    {
        dataCollection = new DataCollection();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Enter! collider = " + other.name);
        if (button != null && other.gameObject.Equals(button))
        {
            // trigger new trial
            DataCollection.VisualHapticPair trial = dataCollection.GetNextTrial();
            Debug.Log("button is clicked, trial = " + trial.ToString());
            if (trial != null)
            {
                
                if(sender.isConnected)
                {
                    sender.SendData(trial.h.ToString());
                }
                visualEffects[trial.v].Play();
                
            }
        }
        else
        {
            for (int i = 0; i < actuators.Length; i++)
            {
                if (other.gameObject.Equals(actuators[i]))
                {
                    //either trigger freestyle effect
                    /*
                    if (sender.isConnected)
                    {
                        sender.SendData(i.ToString());
                    }
                    visualEffects[i].Play();
                    */
                    // record user response
                    Debug.Log("record user response = "+i.ToString());
                    dataCollection.ResultUpdate(i);
                }
            }
        }
    }

    private void OnApplicationQuit()
    {
        dataCollection.SaveToFile();
    }


}
