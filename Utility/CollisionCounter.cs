using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ContactCounter : MonoBehaviour
{
    private int _contactCount = 0;

    void OnCollisionEnter(Collision collision)
    {
        _contactCount++;    
    }
    void OnCollisionExit(Collision collision)
    {
        _contactCount--;    
    }

    public int GetContactCount() { return _contactCount; }
    public bool HasContacts() { return _contactCount > 0; }
}
