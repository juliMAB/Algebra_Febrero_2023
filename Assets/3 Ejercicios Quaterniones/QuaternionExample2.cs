using EjerciciosQuaternion;
using UnityEngine;

public class QuaternionExample2 : MonoBehaviour
{
    public Transform objRefBase;
    public Transform objRefEnd;

    public Transform objToLerp1;
    public Transform objToLerp2;
    public Transform objToLerp3;
    public Transform objToLerp4;

    public Transform objToLerp5;
    public Transform objToLerp6;
    public Transform objToLerp7;
    public Transform objToLerp8;

    public float maxTime;
    public float minTime;


    bool up = true;

    float timer = 0;

    private void Update()
    {

        if (up)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer -= Time.deltaTime;
        }

        objToLerp1.rotation = MyQuaternion.Lerp(objRefBase.rotation, objRefEnd.rotation, timer);

        objToLerp2.rotation = MyQuaternion.LerpUnclamped(objRefBase.rotation, objRefEnd.rotation, timer);

        objToLerp3.rotation = MyQuaternion.Slerp(objRefBase.rotation, objRefEnd.rotation, timer);
        
        objToLerp4.rotation = MyQuaternion.SlerpUnclamped(objRefBase.rotation, objRefEnd.rotation, timer);


        objToLerp5.rotation = Quaternion.Lerp(objRefBase.rotation, objRefEnd.rotation, timer);

        objToLerp6.rotation = Quaternion.LerpUnclamped(objRefBase.rotation, objRefEnd.rotation, timer);

        objToLerp7.rotation = Quaternion.Slerp(objRefBase.rotation, objRefEnd.rotation, timer);

        objToLerp8.rotation = Quaternion.SlerpUnclamped(objRefBase.rotation, objRefEnd.rotation, timer);

        if (timer< minTime)
        {
            up = true;
        }
        else if (timer> maxTime)
        {
            up = false;
        }


    }
}
