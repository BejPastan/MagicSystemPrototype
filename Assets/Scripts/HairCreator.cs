using UnityEngine;
using System;

public class HairCreator : MonoBehaviour
{
    [SerializeField]
    StrandData[] strandData = new StrandData[0];
    [SerializeField]
    int StrandLength;
    [SerializeField]
    int strandCount;
    [SerializeField]
    Transform[] hairRoots;
    [SerializeField]
    GameObject hairPrefab;

    private void OnValidate()
    {
        if (strandData.Length > strandCount)
        {
            for (int i = strandCount; i > strandData.Length; i--)
            {
                RemoveStrand();
            }
        }
        else if (strandData.Length < strandCount)
        {
            for (int i = strandCount; i < strandData.Length; i--)
            {
                CreateNewStrand();
            }
        }
        else if(hairRoots.Length == strandCount)
        {
            RecalcRootPos();
        }
    }

    private void CreateNewStrand()
    {
        Array.Resize(ref strandData, strandData.Length + 1);
        Array.Resize(ref hairRoots, hairRoots.Length + 1);
        hairRoots[strandData.Length - 1] = new GameObject("HairRoot").transform;
        hairRoots[strandData.Length - 1].SetParent(transform);
        hairRoots[strandData.Length - 1].localPosition = new Vector3(0, 1, 0);

        strandData[strandData.Length - 1] = new StrandData(hairPrefab, Color.black, hairRoots[strandData.Length - 1]);

        RecalcRootPos();
        strandData[strandData.Length - 1].ChangeLenght(StrandLength);
    }

    private void RemoveStrand()
    {
        strandData[strandData.Length - 1].RemoveStrand();
        RecalcRootPos();
    }

    private void RecalcRootPos()
    {
        float rotation = 360 / strandCount;
        rotation = rotation * Mathf.Deg2Rad;

        for(int i = 0; i < strandCount; i++)
        {
            hairRoots[i].rotation = new Quaternion(0, rotation * i, 0, 0);
        }
    }
}
