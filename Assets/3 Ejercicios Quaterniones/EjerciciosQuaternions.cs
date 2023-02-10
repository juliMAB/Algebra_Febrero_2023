using UnityEngine;
using EjerciciosAlgebra;
using System.Collections.Generic;

public class EjerciciosQuaternions : MonoBehaviour
{
    public enum Ejercicio
    {
        Uno,
        Dos,
        Tres,
    }
    public Ejercicio ejercicio;
    public float angle;


    // Start is called before the first frame update
    void Start()
    {
        VectorDebugger.EnableCoordinates();
        VectorDebugger.EnableEditorView();
        VectorDebugger.AddVector(new Vector3(10f, 0.0f, 0.0f), Color.red, "1");
        List<Vector3> positions1 = new List<Vector3>();
        positions1.Add(new Vector3(10f, 0.0f, 0.0f));
        positions1.Add(new Vector3(10f, 10f, 0.0f));
        positions1.Add(new Vector3(20f, 10f, 0.0f));
        VectorDebugger.AddVectorsSecuence(positions1, false, Color.blue, "2");
        List<Vector3> positions2 = new List<Vector3>();
        positions2.Add(new Vector3(10f, 0.0f, 0.0f));
        positions2.Add(new Vector3(10f, 10f, 0.0f));
        positions2.Add(new Vector3(20f, 10f, 0.0f));
        positions2.Add(new Vector3(20f, 20f, 0.0f));
        VectorDebugger.AddVectorsSecuence(positions2, false, Color.yellow, "3");
    }

    private void FixedUpdate()
    {
        TurnOffAll();
        switch (ejercicio)
        {
            case Ejercicio.Uno:
                TurnOn("1"); //rotate again a point. only one vector.
                List<Vector3> newPositions1 = new List<Vector3>();
                for (int index = 0; index < VectorDebugger.GetVectorsPositions("1").Count; ++index)
                    newPositions1.Add((Quaternion.Euler(new Vector3(0.0f, angle, 0.0f)) * VectorDebugger.GetVectorsPositions("1")[index]));
                VectorDebugger.UpdatePositionsSecuence("1", newPositions1);
                break;
            case Ejercicio.Dos: //same whit multipls vectors.
                TurnOn("2");
                List<Vector3> newPositions2 = new List<Vector3>();
                for (int index = 0; index < VectorDebugger.GetVectorsPositions("2").Count; ++index)
                    newPositions2.Add((Quaternion.Euler(new Vector3(0.0f, angle, 0.0f)) * VectorDebugger.GetVectorsPositions("2")[index]));
                VectorDebugger.UpdatePositionsSecuence("2", newPositions2);
                break;
            case Ejercicio.Tres: //only rotate the vectors 0 and 2.
                TurnOn("3");
                List<Vector3> newPositions3 = new List<Vector3>();
                newPositions3.Add(VectorDebugger.GetVectorsPositions("3")[0]);
                newPositions3.Add((Quaternion.Euler(new Vector3(angle, angle, 0.0f)) * VectorDebugger.GetVectorsPositions("3")[1]));
                newPositions3.Add(VectorDebugger.GetVectorsPositions("3")[2]);
                newPositions3.Add((Quaternion.Euler(new Vector3(-angle, -angle, 0.0f)) * VectorDebugger.GetVectorsPositions("3")[3]));
                newPositions3.Add(VectorDebugger.GetVectorsPositions("3")[4]);
                VectorDebugger.UpdatePositionsSecuence("3", newPositions3);
                break;
        }
    }

    void TurnOffAll()
    {
       TurnOff("1");
       TurnOff("2");
       TurnOff("3");
    }

    void TurnOff(string id)
    {
        VectorDebugger.TurnOffVector(id);
        VectorDebugger.DisableEditorView(id);
    }

    private void TurnOn(string id)
    {
        VectorDebugger.TurnOnVector(id);
        VectorDebugger.EnableEditorView(id);
    }
}
