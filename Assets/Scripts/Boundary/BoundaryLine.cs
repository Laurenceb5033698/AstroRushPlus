using UnityEngine;
using System.Collections;

public class BoundaryLine : MonoBehaviour {

    public GameObject ship;
    private LineRenderer line;

    public bool drawLine = false;
    public bool ZORX;

    private const int SBOUND = 600;
    private const int HBOUND = SBOUND + 70;
    private Vector3 lineStartPos = new Vector3(0, 500, 0);
    private Vector3 lineEndPos = new Vector3(0, 500, 0);

    // Use this for initialization
    void Start()
    {
        line = transform.gameObject.GetComponent<LineRenderer>();
        line.SetWidth(0.2f, 0.2f);

        line.SetPosition(0, lineStartPos);
        line.SetPosition(1, lineEndPos);

        drawLine = false;
    }
   
    void Update() // Update is called once per frame
    {
        if (drawLine) //draws a line on the z border
        { 
            if (ZORX) drawboundaryZ();
            else drawboundaryX();
        }
    }
    private void drawboundaryZ()
    {
        int zside = 1;
        if (ship.transform.position.z < 0)
            zside = -1;

        lineStartPos = new Vector3(ship.transform.position.x - 40, 0, zside * HBOUND);
        lineEndPos = lineStartPos + new Vector3(80, 0, 0);

        if (lineStartPos.x < -HBOUND)
            lineStartPos.x = -HBOUND;
        else if (lineEndPos.x > HBOUND)
            lineEndPos.x = HBOUND;

        line.SetPosition(0, lineStartPos);
        line.SetPosition(1, lineEndPos);
    }
    private void drawboundaryX()
    {
        int xside = 1;
        if (ship.transform.position.x < 0)
            xside = -1;

        lineStartPos = new Vector3(xside * HBOUND, 0, ship.transform.position.z - 40);
        lineEndPos = lineStartPos + new Vector3(0, 0, 80);

        if (lineStartPos.z < -HBOUND)
            lineStartPos.z = -HBOUND;
        else if (lineEndPos.z > HBOUND)
            lineEndPos.z = HBOUND;

        line.SetPosition(0, lineStartPos);
        line.SetPosition(1, lineEndPos);
    }

    public bool drawstate
    {
        get { return drawLine; }
        set { drawLine = value; }
    }


}
