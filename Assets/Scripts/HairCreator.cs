using UnityEngine;
using System;

public class HairCreator : MonoBehaviour
{
    [SerializeField]
    StrandData[] strandData = new StrandData[0];
    [SerializeField]
    int StrandLength = 0;
    [SerializeField]
    int strandCount = 0;
    [SerializeField]
    Transform[] hairRoots;
    [SerializeField]
    GameObject hairPrefab;

    private void OnValidate()
    {
        if (strandData.Length > strandCount)
        {
            Debug.Log("RemoveStrand");
            RemoveStrands(strandData.Length - strandCount);
        }
        else if (strandData.Length < strandCount)
        {
            Debug.Log("CreateNewStrand");
            CreateNewStrands(strandCount - strandData.Length);
        }
        else if(hairRoots.Length == strandCount)
        {
            Debug.Log("RecalcRootPos");
            RecalcRootPos();
        }
    }

    private void CreateNewStrand()
    {
        int id = strandData.Length;
        Array.Resize(ref strandData, id+1);
        Array.Resize(ref hairRoots, id+1);
        hairRoots[id] = new GameObject("HairRoot").transform;
        hairRoots[id].SetParent(transform);
        hairRoots[id].localPosition = new Vector3(0, 1, 0);
        
        strandData[id] = new StrandData(hairPrefab, Color.black, hairRoots[id]);

        
        strandData[id].ChangeLenght(StrandLength);
    }

    public void CreateNewStrands(int number)
    {
        for (int i = 0; i < number; i++)
        {
            CreateNewStrand();
        }
        RecalcRootPos();
    }

    private void RemoveStrand()
    {
        UnityEditor.EditorApplication.delayCall += () =>
        {
            strandData[strandData.Length - 1].RemoveStrand();
            Array.Resize(ref strandData, strandData.Length - 1);
            DestroyImmediate(hairRoots[hairRoots.Length-1].gameObject);
            Array.Resize(ref hairRoots, hairRoots.Length - 1);
        };
    }

    public void RemoveStrands(int number)
    {
        for(int i = 0; i < number; i++)
        {
            RemoveStrand();
        }
        RecalcRootPos();
    }

    private void RecalcRootPos()
    {
        Debug.Log("RecalcRootPos");
        if(strandCount == 0)
        {
            return;
        }
        float rotation = 360 / strandCount;
        rotation = rotation * Mathf.Deg2Rad;
        Debug.Log("Rotation: " + rotation);

        for(int i = 0; i < strandCount; i++)
        {
            hairRoots[i].rotation = new Quaternion(0, rotation * i, 0, 0);
        }
    }
}
