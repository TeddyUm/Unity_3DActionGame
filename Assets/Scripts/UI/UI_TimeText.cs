using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TimeText : MonoBehaviour
{
    private Text text;
    private float alpha;

    void Start()
    {
        alpha = 1;
        text = GetComponent<Text>();
    }
    void Update()
    {
        alpha -= Time.deltaTime;
        transform.Translate(new Vector3(0, 1, 0));
        text.color = new Color(1, 1, 0.1f, alpha);

        Invoke("DestroyObj", 1.0f);
    }

    void DestroyObj()
    {
        Destroy(gameObject);
    }
}
