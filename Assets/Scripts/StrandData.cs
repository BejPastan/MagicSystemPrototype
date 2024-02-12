using System;
using UnityEngine;

[Serializable]
public struct StrandData
{
    private GameObject hairPrefab;
    public int length;
    private Color color;
    private Transform[] hairTransforms;

    //length getter
    public int Length
    {
        get { return length; }
    }

    public StrandData(GameObject hairPrefab, Color color, Transform root)
    {
        this.hairPrefab = hairPrefab;
        this.color = color;
        length = 0;
        hairTransforms = new Transform[0];
        hairTransforms[length] = root;
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
        while (lenghtChange>0)
        {
            CreateHairPart(length);
            Debug.Log("Lenght Change: " + lenghtChange);
            //lenghtChange set closer to 0
            lenghtChange--;
            length++;
        }
    }

    private void CreateHairPart(int id)
    {
        hairTransforms[id] = GameObject.Instantiate(hairPrefab, hairTransforms[id - 1]).transform;
        hairTransforms[id].localPosition = new Vector3(0, 0, 0);
        hairTransforms[id].localRotation = new Quaternion(0, 0, 0, 0);
        float scale = hairTransforms[id].localScale.x;
        hairTransforms[id].Translate(Vector3.forward * scale);
        hairTransforms[id].GetComponent<Renderer>().material.color = color;
        hairTransforms[id].SetParent(hairTransforms[id].parent.parent);
    }
}