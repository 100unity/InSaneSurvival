using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    private bool mIsOpen = false;
    private GameObject obj;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            mIsOpen = !mIsOpen;
            GetComponent<Animator>().SetBool("open", mIsOpen);
            Destroy(gameObject.GetComponent<BoxCollider>());
            Destroy(this);
        }
    }
}
