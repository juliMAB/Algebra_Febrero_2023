using UnityEngine;
using EjerciciosAlgebra;

public class ExampleClass : MonoBehaviour
{
    public enum nums
    {
        uno, //suma
        dos, //resta
        tres, //producto cruz
        cuatro, //producto cruz
        cinco, //lerp
        seis, //el maximo de cada componente
        siete, // project
        ocho, //perpendicular del centro
        nueve, //lerp uncampled
        diez
    }
    public nums Ejercicio;
    public Color VectorColor;
    public Vector3 A;
    public Vector3 B;
    private Vector3 C;
    public Vec3 A1;
    public Vec3 B1;
    Vec3 C1 = new Vec3(0, 0, 0);


    private float aux;
    void Start()
    {
        
        VectorDebugger.EnableCoordinates();
        VectorDebugger.EnableEditorView();
        VectorDebugger.AddVector(A, Color.white, "A1");
        VectorDebugger.AddVector(B, Color.black ,"B1");
        VectorDebugger.AddVector(C, VectorColor,"C1");
    }
    void Update()
    {
        A1 = new Vec3(A.x, A.y, A.z);
        B1 = new Vec3(B.x, B.y, B.z);
        switch (Ejercicio)
        {
            case nums.uno:
                C1 = A1 + B1;
                break;
            case nums.dos:
                C1 = B1 - A1;
                break;
            case nums.tres:
                C1.x = A1.x * B1.x;
                C1.y = A1.y * B1.y;
                C1.z = A1.z * B1.z;
                break;
            case nums.cuatro:
                C1 = Vec3.Cross(B1, A1);
                break;
            case nums.cinco:
                aux += Time.deltaTime;
                if ((double)aux>1)
                {
                    aux = 1f;
                }
                C1 = Vec3.Lerp(A1, B1, aux);
                if ((double)aux!=1)
                {
                    break;
                }
                aux = 0;
                break;
            case nums.seis:
                C1 = Vec3.Max(A1, B1);
                break;
            case nums.siete:
                C1 = Vec3.Project(A1, B1);
                break;
            case nums.ocho: 
                //es una linea a la mitad. para cortar una soga.
                //la perpendicular del centro.
                C1 = (A1 + B1).normalized * Vec3.Distance(A1, B1) ;
                break;
            case nums.nueve:
                C1 = Vec3.Reflect(A1, B1.normalized);
                break;
            case nums.diez:
                aux += Time.deltaTime;
                if ((double)aux > 10.0f)
                    aux = 10f;
                C1 = Vec3.LerpUnclamped(B1, A1, aux);
                if ((double) aux != 10.0f)
                {
                    break;
                }
                aux = 0;
                break;
        }
        A = new Vector3(A1.x, A1.y, A1.z);
        B = new Vector3(B1.x, B1.y, B1.z);
        C = new Vector3(C1.x, C1.y, C1.z); 
        Debug.DrawLine(Vector3.zero, A, Color.white);
        Debug.DrawLine(Vector3.zero, B, Color.black);
        Debug.DrawLine(Vector3.zero, C, VectorColor);
        //Vector3Debugger.UpdatePosition("A1",A);
       VectorDebugger.UpdatePosition("A1", A);
       VectorDebugger.UpdatePosition("B1",B);
       VectorDebugger.UpdatePosition("C1", C);

    }
}
