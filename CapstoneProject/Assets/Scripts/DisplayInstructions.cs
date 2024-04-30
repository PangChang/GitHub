using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DisplayInstructions : MonoBehaviour
{
    public GameObject helpCanvasObj;
    public static bool isHelping;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isHelping)
            {
                helpInfo();
            }
            else
            {
                stopHelpInfo();
            }
        }

    }


    public void helpInfo()
    {
        helpCanvasObj.SetActive(true);
        isHelping = true;
    }
    public void stopHelpInfo()
    {
        helpCanvasObj.SetActive(false);
        isHelping = false;
    }

}

