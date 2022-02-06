using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PersistentObject : MonoBehaviour
{
    [SerializeField] [HideInInspector] private string id;
    [NonSerialized] private Rigidbody rigidBody;

    private PersistentObjectData data;    

    public string ID => id;
    public PersistentObjectData Data => data;

    private void Awake()
    {
        if (TryGetComponent(out Rigidbody rigidBody))
        {
            this.rigidBody = rigidBody; 
        }
    }

    private void Start()
    {
        data = new PersistentObjectData();
        PersistenceManager.RegisterPersistentObject(this);
    }

    public void SaveState()
    {
        data.id = id;
        data.pPosition = transform.position;
        data.pRotation = transform.rotation;

        if (rigidBody != null)
        {
            data.pVelocity = rigidBody.velocity;
            data.pAngularVerlocity = rigidBody.angularVelocity;
        }
    }

    public void RestoreState(PersistentObjectData dataContainer)
    {
        transform.position = dataContainer.pPosition;
        transform.rotation = dataContainer.pRotation;

        if (rigidBody != null)
        {
            rigidBody.velocity = dataContainer.pVelocity;
            rigidBody.angularVelocity = dataContainer.pAngularVerlocity;
        }
    }

    private void OnValidate()
    {
        if (id == null)
            id = Guid.NewGuid().ToString();
    }
}
