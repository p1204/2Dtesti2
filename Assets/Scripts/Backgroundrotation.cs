using UnityEngine;
using System.Collections;

public class Backgroundrotation : MonoBehaviour {
    public float turnRate;
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, turnRate) * Time.deltaTime);
    }
}
