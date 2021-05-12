using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTD : MonoBehaviour
{
    #region Variables
    public Transform player;
    public float smooth = .3f;
    public float distance = 7f;
    public float height;
    private Vector3 velocity = Vector3.zero;
    #endregion

    #region Methods
    private void Update()
    {
        Vector3 position = new Vector3();
        position.x = player.position.x;
        position.z = player.position.z - distance;
        position.y = player.position.y + height;
        transform.position = Vector3.SmoothDamp(transform.position, position, ref velocity, smooth);
    }
    #endregion
}
