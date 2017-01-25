using UnityEngine;
using System.Collections;

public class ObjectiveManager : MonoBehaviour {

    [SerializeField]
    private GameObject distance;
    [SerializeField]
    private GameObject pointer;

    [SerializeField]
    private GameObject gen1;
    [SerializeField]
    private GameObject gen2;
    [SerializeField]
    private GameObject warpGate;

    private GameObject newTarget;
    private int targetIndex;

    private bool[] objectivesCompleted = new bool[4]; 

	// Use this for initialization
	void Start () {

        for (int i = 0; i < objectivesCompleted.Length; i++)
        {
            objectivesCompleted[i] = false;
        }
        newTarget = transform.gameObject; // scene manager position	
	}
	
	// Update is called once per frame
	void Update () 
    {
        UpdateOnceMessages();
        UpdateTarget();
    }

    private void UpdateTarget()
    {
        targetIndex = 0;

        if (gen1.GetComponent<Generator>().GetShieldStatus() && gen2.GetComponent<Generator>().GetShieldStatus())
            targetIndex = (Vector3.Distance(pointer.transform.position, gen1.transform.position) < Vector3.Distance(pointer.transform.position, gen2.transform.position)) ? 1 : 2;
        else
        {
            if (gen1.GetComponent<Generator>().GetShieldStatus()) targetIndex = 1;
            else if (gen2.GetComponent<Generator>().GetShieldStatus()) targetIndex = 2;
            else targetIndex = 3;
        }

        switch (targetIndex)
        {
            case 1: newTarget = gen1; break;
            case 2: newTarget = gen2; break;
            case 3: newTarget = warpGate; break;
        }

        pointer.GetComponent<Pointer>().SetNewTarget(newTarget);
        distance.GetComponent<DistanceDisplay>().SetNewTarget(newTarget);
    }

    private void UpdateOnceMessages()
    {
        // toggle messages
        if (objectivesCompleted[0] == false)
        {
            objectivesCompleted[0] = !gen1.GetComponent<Generator>().GetShieldStatus();
            if (objectivesCompleted[0] == true) transform.GetComponent<UI>().SetMessage(3);
        }

        if (objectivesCompleted[1] == false)
        {
            objectivesCompleted[1] = !gen2.GetComponent<Generator>().GetShieldStatus();
            if (objectivesCompleted[1] == true) transform.GetComponent<UI>().SetMessage(3);
        }

        if (objectivesCompleted[2] == false)
        {
            objectivesCompleted[2] = warpGate.GetComponent<WarpGate>().GetState();
            if (objectivesCompleted[2] == true) transform.GetComponent<UI>().SetMessage(4);
        }

        if (objectivesCompleted[3] == false)
        {
            objectivesCompleted[3] = warpGate.GetComponent<WarpGate>().GetShipCrossState();
            if (objectivesCompleted[3] == true) transform.GetComponent<UI>().SetMessage(0);
        }


    }
}
