using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoDestroy : MonoBehaviour {
    public string tagName;
    void Awake() {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tagName);
        if (objects.Length > 1) {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }
}