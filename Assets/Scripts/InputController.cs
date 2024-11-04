using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

public class InputController : MonoBehaviour
{
    public static InputController Instance;
    public static event Action<GameObject> OnCardClick;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
            Instance = this;
    }

    void Update()
    {
        if(Time.timeScale == 0)
            return;

        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            Vector3 inputPos;

            if (Input.GetMouseButtonDown(0))
                inputPos = Input.mousePosition;
            else if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                    inputPos = Input.GetTouch(0).position;
                else
                    return;
            }
            else
                return;

            Ray ray = Camera.main.ScreenPointToRay(inputPos);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                if (hit.collider == null)
                    return;

                hit.collider.transform.parent.GetComponent<IClickable>().OnClick();
                OnCardClick?.Invoke(hit.collider.transform.parent.gameObject);
            }
        }
    }
}
