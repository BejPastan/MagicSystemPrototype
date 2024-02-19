using UnityEngine;
using System;
using Unity.VisualScripting;

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
    [SerializeField]
    float prefabRadius;

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
        hairRoots[id].localPosition = Vector3.zero;
        hairRoots[id].AddComponent<Rigidbody>().useGravity = false;
        hairRoots[id].GetComponent<Rigidbody>().isKinematic = true;


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
            strandData[^1].RemoveStrand();
            Array.Resize(ref strandData, strandData.Length - 1);
            DestroyImmediate(hairRoots[^1].gameObject);
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
        //get radius of prefab
        prefabRadius = hairPrefab.GetComponent<Renderer>().bounds.extents.x;
        
        Debug.Log("RecalcRootPos");
        if(strandCount == 0)
        {
            return;
        }
        float rotation = 300 / strandCount;
        Debug.Log("Rotation: " + rotation);

        for(int i = 0; i < strandCount; i++)
        {
            //get position of HairRoot
            Vector3 rootPos = -hairRoots[i].localPosition;
            //calculate new position of HairRoot based on rotation and prefab radius
            rootPos.x += Mathf.Sin(((rotation * i) + 30) * Mathf.Deg2Rad) * prefabRadius*2;
            rootPos.z += Mathf.Cos(((rotation * i) + 30) * Mathf.Deg2Rad) * prefabRadius*2;
            strandData[i].ChangePosition(rootPos);
        }
    }
}
