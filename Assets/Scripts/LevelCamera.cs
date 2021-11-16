using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

public class LevelCamera : MonoBehaviour
{
    [SerializeField] private int length;
    [SerializeField] private float cameraSpeed = 1;
    [SerializeField] private float mapSizeX = 10;

    private Camera camera;
    private HeroController hero;

    private float cameraMinX;
    private float cameraMaxX;

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
        hero = FindObjectOfType<HeroController>();

        RefreshCameraBounds();
    }

    void RefreshCameraBounds()
    {
        cameraMinX = 0;
        cameraMaxX = 0;
        
        var vertExtent = camera.orthographicSize;
        var horzExtent = vertExtent * Screen.width / Screen.height;

        cameraMinX = horzExtent;
        cameraMaxX = length - horzExtent;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraPos = camera.transform.position;

        if (Mathf.Abs(cameraPos.x - hero.Position.x) < 0.05f)
            return;

        cameraPos.x = Mathf.MoveTowards(cameraPos.x, hero.Position.x, cameraSpeed);

        //clamp within level
        cameraPos.x = Mathf.Clamp(cameraPos.x, cameraMinX, cameraMaxX);

        camera.transform.position = cameraPos;
    }
}
