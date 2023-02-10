using System.Collections.Generic;
using UnityEngine;
using EjerciciosPlanos;

public class MyCamaraClass : MonoBehaviour
{
    #region EXPOSED_FIELDS

    [SerializeField] private string layerToUse;

    public float Near;

    public float Far;

    [Range(0.1f, 179f)] public float FieldOfView;
    #endregion

    #region PRIVATE_FIELDS

    //lo que va a ser mi plano de adelante y el de atras.
    private GameObject frontal;
    private GameObject atras;

    //el punto medio entre esos planos.
    private Vec3 middle;

    //los bordes de los plano frontal y atras.
    private GameObject[] puntosFrontales = new GameObject[4];
    private GameObject[] puntosAtras = new GameObject[4];

    // los planos verdaderos que van a formar mi frustrum culling.
    private MyPlane[] planos = new MyPlane[6];

    private List<GameObject> SceneObjects = new List<GameObject>();

    #endregion

    #region UNITY_CALLS
    private void OnValidate()
    {
        if (Near < 0)
        {
            Near = 0;
        }

        if (Near > Far)
        {
            Far = Near;
        }
    }

    void Start()
    {

        SetInitialPlanes();

        SetPoints();

        SetObjectsTests();
    }

    void Update()
    {

        DatosUpdate();

        UpdatePlanes();

        Inside();

        SeePlanes();
        
    }
    #endregion

    #region PRIVATE_METHODS
    void SetInitialPlanes()
    {
        frontal = new GameObject("frontal");
        atras = new GameObject("atras");

        frontal.transform.parent = this.transform;
        atras.transform.parent = this.transform;

        frontal.transform.eulerAngles = new Vector3(90,0,0);
        atras.transform.eulerAngles = new Vector3(270,0,0);
    }

    void SetPoints()
    {
        //aca le pongo nombre a cada punto para que no se me compique tanto la cosa y los instancio.
        for (int i = 0; i < 4; i++)
        {
            puntosFrontales[i] = new GameObject("PF" + i);
            puntosFrontales[i].transform.parent = frontal.transform;
            puntosAtras[i]     = new GameObject("PA" + i);
            puntosAtras[i]    .transform.parent = atras.transform;
        }
        //aca los posiciono en los bordes de mis "planos".
        var localScale = frontal.transform.localScale;
        puntosFrontales[0].transform.localPosition = Vector3.zero + (new Vector3(0, 0, localScale.y).normalized * 10 / 2);
        puntosFrontales[0].transform.localPosition += (new Vector3(localScale.x, 0, 0).normalized * 10 / 2);
        puntosFrontales[1].transform.localPosition = Vector3.zero + (new Vector3(0, 0, localScale.y).normalized * 10 / 2);
        puntosFrontales[1].transform.localPosition -= (new Vector3(localScale.x, 0, 0).normalized * 10 / 2);
        puntosFrontales[2].transform.localPosition = Vector3.zero - (new Vector3(0, 0, localScale.y).normalized * 10 / 2);
        puntosFrontales[2].transform.localPosition -= (new Vector3(localScale.x, 0, 0).normalized * 10 / 2);
        puntosFrontales[3].transform.localPosition = Vector3.zero - (new Vector3(0, 0, localScale.y).normalized * 10 / 2);
        puntosFrontales[3].transform.localPosition += (new Vector3(localScale.x, 0, 0).normalized * 10 / 2);
        var scale = atras.transform.localScale;
        puntosAtras[0].transform.localPosition = Vector3.zero + (new Vector3(0, 0, scale.y).normalized * 10 / 2);
        puntosAtras[0].transform.localPosition += (new Vector3(scale.x, 0, 0).normalized * 10 / 2);
        puntosAtras[1].transform.localPosition = Vector3.zero + (new Vector3(0, 0, scale.y).normalized * 10 / 2);
        puntosAtras[1].transform.localPosition -= (new Vector3(scale.x, 0, 0).normalized * 10 / 2);
        puntosAtras[2].transform.localPosition = Vector3.zero - (new Vector3(0, 0, scale.y).normalized * 10 / 2);
        puntosAtras[2].transform.localPosition -= (new Vector3(scale.x, 0, 0).normalized * 10 / 2);
        puntosAtras[3].transform.localPosition = Vector3.zero - (new Vector3(0, 0, scale.y).normalized * 10 / 2);
        puntosAtras[3].transform.localPosition += (new Vector3(scale.x, 0, 0).normalized * 10 / 2);
    }

    void SetObjectsTests()
    {
        //agrego todos los objetos de mi escena a este arraid y despues los meto en una lista si tienen el Layer "TEST".
        GameObject[] sceneGameObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (var t in sceneGameObjects)
        {
            if (t.layer == LayerMask.NameToLayer(layerToUse))
            {
                SceneObjects.Add(t);
            }
        }
    }

    void Inside()
    {
        if (SceneObjects.Count != 0)
        {
            foreach (var t in SceneObjects)
            {
                MeshRenderer meshRenderer = t.GetComponent<MeshRenderer>();

                if (meshRenderer != null)
                {
                    if (Pertenece(t))
                    {
                        meshRenderer.enabled = true;
                    }
                    else
                    {
                        meshRenderer.enabled = false;
                    }
                }
            }
        }
    }

    void DatosUpdate()
    {
        //esto es mas que nada los datos de la posicion del plano central y el lejano y que tan ampleado esta con el field of view.
        frontal.transform.localPosition =  Vector3.forward * Near;
        atras.transform.localPosition =  Vector3.forward * Far;
        middle = (Vec3)(((atras.transform.position - frontal.transform.position))/2+ frontal.transform.position);
        frontal.transform.localScale = atras.transform.localScale / (Vector3.Distance(atras.transform.position, transform.position)/Vector3.Distance(frontal.transform.position,transform.position));
        atras.transform.localScale = Vector3.one * FieldOfView*0.1f;
    }
    void UpdatePlanes()
    {
        //aca updateo todos los datos de vec3 y myplanes.
        //el punto medio.
        Vec3 a = (middle - (Vec3)frontal.transform.position);
        //los planos frontal y tracero.
        planos[0].SetNormalAndPosition(a.normalized, (Vec3)frontal.transform.position);
        planos[1].SetNormalAndPosition((middle - (Vec3)atras.transform.position).normalized, (Vec3)atras.transform.position);
        //aca todos los puntos de los bordes de mis planos frontal y tracero.
        Vec3 PA0 = (Vec3)puntosAtras[0].transform.position;
        Vec3 PA1 = (Vec3)puntosAtras[1].transform.position;
        Vec3 PA2 = (Vec3)puntosAtras[2].transform.position;
        Vec3 PA3 = (Vec3)puntosAtras[3].transform.position;
        Vec3 PF0 = (Vec3)puntosFrontales[0].transform.position;
        Vec3 PF2 = (Vec3)puntosFrontales[2].transform.position;
        //y aca actualizo mis otros planos de los costados con 3 puntos de los actualizados recien arriba.
        //PF = Plano Frontal.
        planos[2].Set3Points(PF2, PA1, PA2);
        planos[3].Set3Points(PF2, PA1, PA0);
        planos[4].Set3Points(PF0, PA3, PA0);
        planos[5].Set3Points(PF0, PA3, PA2);
        //si los planos no estan mirando al centro, se giran, asi todos estan mirando hacia adentro.
        for (int i = 2; i < planos.Length; i++)
        {
            if (!planos[i].GetSide(middle))
                planos[i].Flip();
        }
    }

    void SeePlanes()
    {
        DrawPlane(planos[0].normal, frontal.transform.position);
        DrawPlane(planos[1].normal,atras.transform.position);
        Vector3 PA0 = puntosAtras[0].transform.position;
        Vector3 PA1 = puntosAtras[1].transform.position;
        Vector3 PA2 = puntosAtras[2].transform.position;
        Vector3 PA3 = puntosAtras[3].transform.position;
        Vector3 PF0 = puntosFrontales[0].transform.position;
        Vector3 PF1 = puntosFrontales[1].transform.position;
        Vector3 PF2 = puntosFrontales[2].transform.position;
        Vector3 PF3 = puntosFrontales[3].transform.position;
        //frontal.
        Debug.DrawLine(PF0, PF1, Color.red);
        Debug.DrawLine(PF0, PF3, Color.red);
        Debug.DrawLine(PF2, PF1, Color.red);
        Debug.DrawLine(PF2, PF3, Color.red);
        //atras.
        Debug.DrawLine(PA0, PA1, Color.red);
        Debug.DrawLine(PA0, PA3, Color.red);
        Debug.DrawLine(PA2, PA1, Color.red);
        Debug.DrawLine(PA2, PA3, Color.red);
        //conectores.
        Debug.DrawLine(PF3, PA0, Color.red);
        Debug.DrawLine(PF0, PA3, Color.red);
        Debug.DrawLine(PF1, PA2, Color.red);
        Debug.DrawLine(PF2, PA1, Color.red);


    }
    void DrawPlane(Vector3 normal, Vector3 position)
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

        Debug.DrawLine(corner0, corner2, Color.green);
        Debug.DrawLine(corner1, corner3, Color.green);
        Debug.DrawLine(corner0, corner1, Color.green);
        Debug.DrawLine(corner1, corner2, Color.green);
        Debug.DrawLine(corner2, corner3, Color.green);
        Debug.DrawLine(corner3, corner0, Color.green);
        Debug.DrawRay(position, normal, Color.blue);
    }
    bool Pertenece(GameObject go)
    {
        Mesh a = go.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices;
        vertices = a.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            if (Pertenece((Vec3)vertices[i] + (Vec3)go.transform.position))
                return true;
        }
        return false;
    }
    bool Pertenece(Vec3 v3)
    {
        bool bl = planos[0].GetSide(v3);
        for (int i = 1; i < planos.Length; i++)
        {
            if (bl != planos[i].GetSide(v3))
            {
                return false;
            }
        }

        return true;
    }
    #endregion
}
