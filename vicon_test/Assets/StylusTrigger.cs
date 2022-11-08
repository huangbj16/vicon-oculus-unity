using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class StylusTrigger : MonoBehaviour
{
    public bool isExperimentMode;
    public GameObject button;
    public GameObject[] actuators;
    public GameObject[] answerButtons;
    public VisualEffect[] visualEffects;
    public TcpSender sender;
    
    private DataCollection dataCollection;
    private bool isTrialTriggered = false;
    private bool isCounting = false;
    private float count_timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        dataCollection = new DataCollection();
        // deactivate actuator objects
        if (isExperimentMode)
        {
            for (int i = 0; i < actuators.Length; i++)
            {
                actuators[i].SetActive(false);
                answerButtons[i].SetActive(false);
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isCounting)
        {
            count_timer += Time.deltaTime;
            if (count_timer > 1.5)
            {
                isCounting = false;
                count_timer = 0;
                for(int i = 0; i < actuators.Length; i++)
                {
                    actuators[i].SetActive(true);
                    answerButtons[i].SetActive(true);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Enter! collider = " + other.name);
        if (!isExperimentMode)
        {
            for (int i = 0; i < actuators.Length; i++)
            {
                if (other.gameObject.Equals(actuators[i]))
                {
                    //trigger freestyle effect
                    if (sender.isConnected)
                    {
                        sender.SendData(i.ToString());
                    }
                    visualEffects[i].Play();
                }
            }
        }
        else
        {// in experiment mode
            if (button != null && other.gameObject.Equals(button))
            {
                // trigger new trial
                isTrialTriggered = true;
                isCounting = true;
                //  show visual and haptic effect
                DataCollection.VisualHapticPair trial = dataCollection.GetNextTrial();
                if (trial != null)
                {
                    Debug.Log("button is clicked, trial = " + trial.ToString());
                    if (sender.isConnected)
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
                    if (other.gameObject.Equals(answerButtons[i]))
                    {
                        if (isTrialTriggered)
                        {
                            // record user response
                            isTrialTriggered = false;
                            Debug.Log("record user response = " + i.ToString());
                            dataCollection.ResultUpdate(i);
                            // deactivate actuator objects
                            for (int j = 0; j < actuators.Length; j++)
                            {
                                actuators[j].SetActive(false);
                                answerButtons[j].SetActive(false);
                            }
                        }
                    }
                }
            }
        }
    }

    private void OnApplicationQuit()
    {
        dataCollection.SaveToFile();
    }


}
