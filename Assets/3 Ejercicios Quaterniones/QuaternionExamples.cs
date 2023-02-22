using EjerciciosQuaternion;
using UnityEngine;
using EjerciciosVec3;
using UnityEditor;
using System.Reflection;
public class QuaternionExamples : MonoBehaviour
{
    public Transform obj1;
    public Transform obj2;
    public Transform obj3;
    public Transform obj4;
    
    
    public Quaternion UnityQuaternion1;
    public Quaternion UnityQuaternion2;

    public MyQuaternion myQuaternion1;
    public MyQuaternion myQuaternion2;

    public float angle;

    public Vector3 axis;

    void ReGetComponent()
    {
        UnityQuaternion1 = obj1.rotation;
        UnityQuaternion2 = obj2.rotation;

        myQuaternion1 = obj3.rotation;
        myQuaternion2 = obj4.rotation;
    }

    void DebugData(string unity, string my)
    {
        Debug.Log("Result unity: " + unity);
        Debug.Log("Result MyQuaternion: " + my);
    }

    public void MyStaticAngle()
    {
        ReGetComponent();
        float angle1 = Quaternion.Angle(UnityQuaternion1, UnityQuaternion2);
        float angle2 = MyQuaternion.Angle(myQuaternion1, myQuaternion2);
        Debug.ClearDeveloperConsole();
        Debug.Log("-----Angle-----");
        DebugData(angle1.ToString(), angle2.ToString());
        Debug.Log("---------------");
    }

    public void MyStaticAngleAxis()
    {
        ReGetComponent();
        Quaternion dot1 = Quaternion.AngleAxis(angle, axis);
        MyQuaternion dot2 = MyQuaternion.AngleAxis(angle, axis);
        Debug.ClearDeveloperConsole();
        Debug.Log("---AngleAxis---");
        DebugData(dot1.ToString(), dot2.ToString());
        Debug.Log("---------------");
    }

    public void MyStaticDot()
    {
        ReGetComponent();
        float dot1 = Quaternion.Dot(UnityQuaternion1, UnityQuaternion2);
        float dot2 = MyQuaternion.Dot(myQuaternion1, myQuaternion2);
        Debug.ClearDeveloperConsole();
        Debug.Log("------Dot------");
        DebugData(dot1.ToString(), dot2.ToString());
        Debug.Log("---------------");
    }

    public void MyStaticEquals()
    {
        ReGetComponent();
        bool res1 = Quaternion.Equals(UnityQuaternion1, UnityQuaternion2);
        bool res2 = MyQuaternion.Equals(myQuaternion1,myQuaternion2);
        Debug.ClearDeveloperConsole();
        Debug.Log("-----Equals-----");
        DebugData(res1.ToString(), res2.ToString());
        Debug.Log("---------------");
    }

    public void MyStaticEuler()
    {
        ReGetComponent();
        Quaternion   res1 = Quaternion  .Euler(UnityQuaternion1.eulerAngles);
        MyQuaternion res2 = MyQuaternion.Euler(myQuaternion1.eulerAngles);
        Debug.ClearDeveloperConsole();
        Debug.Log("-----Euler-----");
        DebugData(res1.ToString(), res2.ToString());
        Debug.Log("---------------");
    }

    public void MyStaticFromToRotation()
    {
        ReGetComponent();
        Quaternion res1 = Quaternion.FromToRotation(obj1.position, obj2.position);
        MyQuaternion res2 = MyQuaternion.FromToRotation(obj1.position, obj2.position);
        Debug.ClearDeveloperConsole();
        Debug.Log("-FromToRotation-");
        DebugData(res1.ToString(), res2.ToString());
        Debug.Log("---------------");
    }

    public void MyStaticIdentity()
    {
        ReGetComponent();
        Quaternion res1 = Quaternion.identity;
        MyQuaternion res2 = MyQuaternion.Identity;
        Debug.ClearDeveloperConsole();
        Debug.Log("---Identity---");
        DebugData(res1.ToString(), res2.ToString());
        Debug.Log("---------------");
    }

    public void MyStaticInverse()
    {
        ReGetComponent();
        Quaternion res1 = Quaternion.Inverse(UnityQuaternion1);
        MyQuaternion res2 = MyQuaternion.Inverse(myQuaternion1);
        Debug.ClearDeveloperConsole();
        Debug.Log("----Inverse----");
        DebugData(res1.ToString(), res2.ToString());
        Debug.Log("---------------");
    }

    public void MyStaticNormalize()
    {
        ReGetComponent();
        Quaternion res1 = Quaternion.Normalize(UnityQuaternion1);
        MyQuaternion res2 = MyQuaternion.Normalize(myQuaternion1);
        Debug.ClearDeveloperConsole();
        Debug.Log("---Normalize---");
        DebugData(res1.ToString(), res2.ToString());
        Debug.Log("---------------");
    }

    public void MyEulerAngles()
    {
        ReGetComponent();
        
        Debug.ClearDeveloperConsole();
        Debug.Log("----EulerAngles----");
        DebugData(UnityQuaternion1.eulerAngles.ToString(), myQuaternion1.eulerAngles.ToString());
        Debug.Log("---------------");
    }
    public void MyStaticRotateTowards()
    {
        ReGetComponent();
        Quaternion res1 = Quaternion.RotateTowards(obj1.rotation,obj2.rotation, float.MaxValue);
        MyQuaternion res2 = MyQuaternion.RotateTowards(obj3.rotation, obj4.rotation, float.MaxValue);
        Debug.ClearDeveloperConsole();
        Debug.Log("-RotateTowards-");
        DebugData(res1.ToString(), res2.ToString());
        Debug.Log("---------------");
    }
}

[CustomEditor(typeof(QuaternionExamples))]
public class QuaternionExampleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        QuaternionExamples target = (QuaternionExamples)this.target;

        MethodInfo[] a = (typeof(QuaternionExamples)).GetMethods();
        
        for (int i = 0; i < a.Length; i++)
        {
            if (!a[i].Name.StartsWith("My"))
                continue;
            if (GUILayout.Button(a[i].Name))
            {
                a[i].Invoke(target, null);
            }
        }

    }
}
