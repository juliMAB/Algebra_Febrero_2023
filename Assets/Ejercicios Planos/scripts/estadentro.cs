using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class estadentro : MonoBehaviour
{
    enum pos
    {
        piso,
        frontal,
        atras,
        derecha,
        izquierda,
        techo
    }

    public Plane[] room = new Plane[6];
    private readonly Vector3[] point = new Vector3[2];
    public GameObject punto;
    public List<GameObject> objs2 = new List<GameObject>();
    public Transform[] room2Tr = new Transform[6];
    public Plane[] room2 = new Plane[6];
    void Start()
    {
        point[0] = new Vector3(5, 10, -5);
        point[1] = new Vector3(-5, 0, 5);
        SetRoom();
        SetRoom2();
    }
    void SetRoom()
    {
        room[0] = new Plane(new Vector3(0, -1, 0), point[0]);
        room[1] = new Plane(new Vector3(-1, 0, 0), point[0]);
        room[2] = new Plane(new Vector3(0, 0, 1), point[0]);
        room[3] = new Plane(new Vector3(0, 1, 0), point[1]);
        room[4] = new Plane(new Vector3(1, 0, 0), point[1]);
        room[5] = new Plane(new Vector3(0, 0, -1), point[1]);
    }

    void cargarObjs()
    {
        //GameObject go = GameObject.FindWithTag();
        //objs2.Add(go);
        //go.GetInstanceID()
    }
    void SetRoom2()
    {
        room2[0] = new Plane(new Vector3(0, 1, 0), room2Tr[0].position);
        room2[1] = new Plane(new Vector3(0, 0, 1), room2Tr[1].position);
        room2[2] = new Plane(new Vector3(0, 0, -1), room2Tr[2].position);
        room2[3] = new Plane(new Vector3(-1, 0, 0), room2Tr[3].position);
        room2[4] = new Plane(new Vector3(1, 0, 0), room2Tr[4].position);
        room2[5] = new Plane(new Vector3(0, -1, 0), room2Tr[5].position);
    }

    void Update()
    {
        SetRoom2();
        punto.GetComponent<MeshRenderer>().forceRenderingOff = !CheckAllVertexInRoom(punto);

        //DrawPlane(new Vector3(0, -1, 0), point[0]); 
        //DrawPlane(new Vector3(-1, 0, 0), point[0]); 
        //DrawPlane(new Vector3(0, 0, 1), point[0]); 
        //DrawPlane(new Vector3(0, 1, 0), point[1]); 
        //DrawPlane(new Vector3(1, 0, 0), point[1]); 
        //DrawPlane(new Vector3(0, 0, -1), point[1]); 
    }
    private bool Pertenece(Vector3 v3)
    {
        bool bl = room2[0].GetSide(v3);
        for (int i = 1; i < 6; i++)
        {
            if (bl!=room2[i].GetSide(v3))
            {
                return false;
            }
        }

        return true;
    }

    private Vector3 centerRoom()
    {
        Vector3 punto = ((room2Tr[5].transform.position - room2Tr[0].transform.position) / 2) + room2Tr[0].transform.position;
        Debug.DrawLine(punto, punto+Vector3.down,Color.red);
        return punto;   
    }
    private bool CheckAllVertexInRoom(GameObject go)
    {
    Vector3 a  = Physics.ClosestPoint(centerRoom(), go.GetComponent<Collider>(), go.transform.position, go.transform.rotation);
        Debug.DrawLine(a,a + Vector3.down, Color.red);
        if (Pertenece(a))
        {
            return true;
        }
                
        //Debug.DrawRay(go.transform.TransformPoint(t), Vector3.one, Color.red);

        return false;
    }
    void DrawPlane( Vector3 normal, Vector3 position)
    {

        Vector3 v3;

        if (normal.normalized != Vector3.forward)
            v3 = Vector3.Cross(normal, Vector3.forward).normalized * normal.magnitude;
        else
            v3 = Vector3.Cross(normal, Vector3.up).normalized * normal.magnitude;

        var corner0 = position + v3;
        var corner2 = position - v3;
        var q = Quaternion.AngleAxis(90.0f, normal);
        v3 = q * v3;
        var corner1 = position + v3;
        var corner3 = position - v3;

        //Debug.DrawLine(corner0, corner2, Color.green);
        //Debug.DrawLine(corner1, corner3, Color.green);
        //Debug.DrawLine(corner0, corner1, Color.green);
        //Debug.DrawLine(corner1, corner2, Color.green);
        //Debug.DrawLine(corner2, corner3, Color.green);
        //Debug.DrawLine(corner3, corner0, Color.green);
        Debug.DrawRay(position, normal, Color.red);
    }
}
