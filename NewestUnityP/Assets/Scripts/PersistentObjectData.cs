using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PersistentObjectData
{
    public string id;
    public Vector3 pPosition;
    public Quaternion pRotation;
    public Vector3 pVelocity;
    public Vector3 pAngularVerlocity;
}

[Serializable] 
public class PersistentObjectSerializationWrapper
{
    [SerializeField]
    public List<PersistentObjectData> Data = new List<PersistentObjectData>();
}
