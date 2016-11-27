using UnityEngine;
using System.Collections;

public class SolarPanel : MonoBehaviour
{

    public GameObject Base;
    public GameObject p1;
    public GameObject p2;
    public GameObject p3;
    public GameObject p4;
    public GameObject p5;
    public GameObject p6;

    public bool toggle = false;
    private bool open = true;
    private const float speed = 0.5f;

    private float minimum = 1F;
    private float maximum = 0F;
    private float t = 1.0f;

    void Update()
    {
        if (toggle) UpdateSolarPanel();
    }

    private void UpdateSolarPanel()
    {

        float temp = Mathf.Lerp(minimum, maximum, t);
        p1.transform.eulerAngles = new Vector3(temp * -90, 0, 0);
        p2.transform.eulerAngles = new Vector3(temp * 90, 0, 0);
        p3.transform.eulerAngles = new Vector3(temp * -90, 0, 0);
        p4.transform.eulerAngles = new Vector3(temp * 90, 0, 0);
        p5.transform.eulerAngles = new Vector3(temp * -90, 0, 0);
        p6.transform.eulerAngles = new Vector3(temp * 90, 0, 0);

        if (t <= 1f)
        {
            t += speed * Time.deltaTime;
        }
        else
        {
            float swap = maximum;
            maximum = minimum;
            minimum = swap;
            t = 0.0f;

            toggle = false;
            open = !open;
        }
    }

    public void ToggleState()
    {
        toggle = true;
    }
}