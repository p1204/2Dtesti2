using UnityEngine;
using System.Collections;

//https://www.youtube.com/watch?v=m-J5sCRipA0&index=6&list=PLPV2KyIb3jR42oVBU6K2DIL6Y22Ry9J1c

public class armRotation : MonoBehaviour {
    public float rotationOffset = -90f;
    public float rotationZ;

    // Update is called once per frame
    void Update () {
        /*//Debug.DrawLine(laserGunPoint.position, Input.mousePosition, Color.red);
        Vector3 difference = Input.mousePosition - transform.position;
        Debug.DrawLine(Input.mousePosition, transform.position, Color.blue);
        difference.Normalize();

        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ + rotationOffset);
        */
        rotationZ = Mathf.Atan2(Input.mousePosition.x, Input.mousePosition.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, (rotationZ + rotationOffset) *2);
	}
}
