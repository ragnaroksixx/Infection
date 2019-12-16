using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScene : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(11);
        SaveLoad.Continue();
    }

}
