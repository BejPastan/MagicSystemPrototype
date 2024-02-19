using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public struct StrandData
{
    private readonly GameObject hairPrefab;
    private int length;
    private Transform[] hairTransforms;
    private Color color;

    //length getter
    public readonly int Length
    {
        get { return length; }
    }

    public StrandData(GameObject hairPrefab, Color color, Transform root)
    {
        this.hairPrefab = hairPrefab;
        length = 1;
        hairTransforms = new Transform[length];
        hairTransforms[0] = root;
        this.color = color;
    }

    public void ChangeLenght(int lenghtChange)
    {
        while (lenghtChange < 0)
        {
            GameObject.Destroy(hairTransforms[length].gameObject);
            lenghtChange++;
            length--;
        }
        Array.Resize(ref hairTransforms, length + lenghtChange);
        Debug.Log(length + " hairTransforms.Length");
        while (lenghtChange>0)
        {
            length++;
            CreateHairPart(length-1);
            //lenghtChange set closer to 0
            lenghtChange--;
        }
    }

    private readonly void CreateHairPart(int id)
    {
        Debug.Log(id + " hair part ID");
        hairTransforms[id] = GameObject.Instantiate(hairPrefab, hairTransforms[id - 1]).transform;
        hairTransforms[id].SetLocalPositionAndRotation(new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
        float scale = hairTransforms[id].localScale.x;
        hairTransforms[id].Translate(Vector3.up * scale);
        hairTransforms[id].GetComponent<Renderer>().material.color = color;
        hairTransforms[id].SetParent(hairTransforms[0].parent);
        hairTransforms[id].GetComponent<Joint>().connectedBody = hairTransforms[id - 1].GetComponent<Rigidbody>();
        hairTransforms[id].transform.localScale = Vector3.one*scale;
    }

    public readonly void RemoveStrand()
    {
        for (int i = 1; i < hairTransforms.Length; i++)
        {
            GameObject.DestroyImmediate(hairTransforms[i].gameObject);
        }
    }

    //move strand to new position
    public void ChangePosition(Vector3 positionChange)
    {
        foreach (Transform hairTransform in hairTransforms)
        {
            hairTransform.position += positionChange;
        }
    }

    //changeRotation of strand around Y axis
    public void ChangeRotation(float rotationChange)
    {
        foreach (Transform hairTransform in hairTransforms)
        {
            hairTransform.Rotate(Vector3.up, rotationChange);
        }
    }
}