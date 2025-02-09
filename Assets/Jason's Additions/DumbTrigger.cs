using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbTrigger : MonoBehaviour
{
    public GameObject HidenArea;

    private void Awake()
    {
        HidenArea.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        HidenArea.gameObject.SetActive(true);
    }
}
