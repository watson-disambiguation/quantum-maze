using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;

using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player player;
    
    [SerializeField]
    private TileManager tileManager;

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private Light2D visionLight;

    [SerializeField]
    private float speed = 10f;

    private Vector2 velocity;
    private float rotation;
    private float mapWidth;
    private float mapHeight;

    private void Awake()
    {
        if (player == null)
        {
            player = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if(cam == null)
        {
            cam = FindObjectOfType<Camera>();
        }
        if(visionLight == null)
        {
            visionLight = FindObjectOfType<UnityEngine.Rendering.Universal.Light2D>();
        }
        int x = tileManager.Width();
        if(x % 2 == 1)
        {
            x++;
        }
        int y = tileManager.Height();
        if (y % 2 == 1)
        {
            y++;
        }
        transform.position = new Vector3(x, y, 0);

        mapWidth = tileManager.Width() * 2;
        mapHeight = tileManager.Height() * 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseManager.Instance.paused)
        {
            velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            rotation = Vector2.SignedAngle(Vector2.up,(cam.ScreenToWorldPoint(Input.mousePosition)-transform.position));
        }
    }
    private void FixedUpdate()
    {
        if (!PauseManager.Instance.paused)
        {
            visionLight.transform.localEulerAngles = new Vector3(0, 0, rotation);
            float x = transform.position.x;

            float y = transform.position.y;
            if (x > mapWidth - 1)
            {
                x -= mapWidth;
            }
            else if (x < -1)
            {
                x += mapWidth;
            }

            if (y > mapHeight - 1)
            {
                y -= mapHeight;
            }
            else if (y < -1)
            {
                y += mapHeight;
            }

            transform.position = new Vector3(x, y, 0);
            rb.velocity = velocity * speed;
        }
    }

    public float getRotation()
    {
        return rotation;
    }
}
