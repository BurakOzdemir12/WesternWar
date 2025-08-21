using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class MainMenuUiManager : MonoBehaviour
{
    [Header("UI Elements")] [Tooltip("Panel for the market UI")] [SerializeField]
    GameObject marketPanel;

    [Space] [Tooltip("Layer mask for building detection")] [SerializeField]
    LayerMask buildingLayer;

    [SerializeField] private string building;
    [Space] [SerializeField] private Camera raycastCamera;
    private Vector3 _clickedScreenPos;
    [Header("Panels")] [SerializeField] GameObject[] panels;



    private void Start()
    {
        marketPanel.SetActive(false);
    }
    private void Update()
    {

        // if (Input.GetMouseButtonDown(0)) // veya Input System callback
        // {
        //     Vector2 screenPos = Input.mousePosition;
        //     Ray ray = raycastCamera.ScreenPointToRay(screenPos);
        //     if (Physics.Raycast(ray, out var hit, Mathf.Infinity, buildingLayer))
        //     {
        //         if (hit.collider.CompareTag("Building"))
        //             marketPanel.SetActive(true);
        //     }
        // }
    }
    public void OpenOnly(GameObject target)
    {
        
        foreach (var p in panels) p.SetActive(p == target);
    }

    public void CloseMarketPanel()
    {
        marketPanel.SetActive(false);
    }

}