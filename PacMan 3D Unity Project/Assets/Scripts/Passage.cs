using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Passage : MonoBehaviour
{
    [SerializeField]
    private Transform connection;
    
    private void OnTriggerEnter(Collider other)
    {
        Vector3 position = connection.position;
        position.y = other.transform.position.y;
        other.transform.position = position;
    }
}
