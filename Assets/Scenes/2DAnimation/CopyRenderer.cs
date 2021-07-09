using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class CopyRenderer : MonoBehaviour
{
    public float interval = 0;
    float time = 0;
    Dictionary<GameObject, GameObject> relation=new Dictionary<GameObject, GameObject>();   
    // Start is called before the first frame update
    void Start()
    {
        var clone = Instantiate(gameObject);
        Destroy(clone.GetComponent<CopyRenderer>());
        Dictionary<string, GameObject> dic = new Dictionary<string, GameObject>();
        foreach(var i in this.GetComponentsInChildren<Transform>(true))
        {
            dic.Add(i.name, i.gameObject);
        }
        //SpriteRendererはMonoBehaviourを継承していない
        foreach (Renderer i in this.GetComponentsInChildren<Renderer>(true))
        {
            i.enabled = false;
        }

        foreach (MonoBehaviour i in clone.GetComponentsInChildren<MonoBehaviour>(true))
        {
            //Debug.Log(i.GetType(),(i as MonoBehaviour)?.gameObject);
            if (i is SpriteSkin) continue;
            Destroy(i);
        }
        foreach(Collider2D i in clone.GetComponentsInChildren<Collider2D>(true))
        {
           // Debug.Log(i.GetType(), (i as Collider2D)?.gameObject);
            Destroy(i);
        }
        foreach (DistanceJoint2D i in clone.GetComponentsInChildren<DistanceJoint2D>(true))
        {
            // Debug.Log(i.GetType(), (i as Collider2D)?.gameObject);
            Destroy(i);
        }
        foreach (Rigidbody2D i in clone.GetComponentsInChildren<Rigidbody2D>(true))
        {
            // Debug.Log(i.GetType(), (i as Collider2D)?.gameObject);
            Destroy(i);
        }


        foreach (var i in clone.GetComponentsInChildren<Transform>(true))
        {
            if (!dic.ContainsKey(i.name)) continue;
            relation.Add(dic[i.name], i.gameObject);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(time>= interval)
        {

            foreach (var i in relation)
            {
                i.Value.transform.position = i.Key.transform.position;
                i.Value.transform.localScale = i.Key.transform.localScale;
                i.Value.transform.rotation = i.Key.transform.rotation;
            }
            time = 0;
        }
        time += Time.deltaTime;
    }
}
