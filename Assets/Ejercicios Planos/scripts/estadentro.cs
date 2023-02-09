using UnityEngine;

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
    public GameObject[] ObjectsToCheck;
    public Transform[] room2Tr = new Transform[6];
    public Plane[] room2 = new Plane[6];

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
        for (int i = 0; i < ObjectsToCheck.Length; i++)
            ObjectsToCheck[i].GetComponent<MeshRenderer>().forceRenderingOff = !CheckAllVertexInRoom(ObjectsToCheck[i]);
        
    }
    private bool Pertenece(Vector3 v3)
    {
        bool bl = room2[0].GetSide(v3);
        for (int i = 1; i < 6; i++)
            if (bl!=room2[i].GetSide(v3))
                return false;

        return true;
    }
    bool CheckAllVertexInRoom(GameObject go)
    {
        Mesh a = go.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices;
        vertices = a.vertices;
        for (int i = 0; i < vertices.Length; i++)
            if (Pertenece(vertices[i] + go.transform.position))
                return true;
        
        return false;
    }
}
