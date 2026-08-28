using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatterableBehaviour : MonoBehaviour
{
   public GameObject shatteredVersion;

   public void Shatter()
   {
        var obj = Instantiate(shatteredVersion, transform.position, transform.rotation);
        obj.transform.localScale = transform.localScale;
        Destroy(gameObject);
   }
}
