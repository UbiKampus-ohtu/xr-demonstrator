using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBoxColliders : MonoBehaviour
{

    public CollidersList CollidersList = new CollidersList();

    void Start()
    {
       TextAsset asset = Resources.Load("collider_export") as TextAsset;
       if (asset != null) 
       {
           CollidersList = JsonUtility.FromJson<CollidersList>(asset.text);
           
           foreach(Colliders Colliders in CollidersList.Colliders) 
           {
               BoxCollider bc = gameObject.AddComponent(typeof(BoxCollider)) as BoxCollider;
               bc.center = new Vector3(Colliders.origin.x, Colliders.origin.y, Colliders.origin.z);
               bc.size = new Vector3(Colliders.dimensions.width, Colliders.dimensions.depth, Colliders.dimensions.height);
           }
       } 
       else
       {
           Debug.Log("Asset tyhjä, json filua ei loydy tms");
       }
    }


}
