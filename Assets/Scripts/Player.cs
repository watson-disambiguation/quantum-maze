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
    private Rigidbody rb;

    [SerializeField]
    private Light2D visionLight;

    [SerializeField] private float speed = 100f, rotationSpeed = 10f, playerHeight = 1f;

    [SerializeField] private Crosshair crosshair;

    private Vector3 velocity;
    private float rotation;
    private float mapWidth;
    private float mapHeight;

    private Camera mainCamera;
    void Awake()
    {
        if (player == null)
        {
            player = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (mainCamera == null)
        {
            mainCamera = FindObjectOfType<Camera>();
        }
        if(visionLight == null)
        {
            visionLight = FindObjectOfType<UnityEngine.Rendering.Universal.Light2D>();
        }

        if (crosshair == null)
        {
            crosshair = FindObjectOfType<Crosshair>();
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
        transform.position = new Vector3(x, 0, y);

        mapWidth = tileManager.Width() * 2;
        mapHeight = tileManager.Height() * 2;
    }
    

    // Update is called once per frame
    void Update()
    {
        if (!PauseManager.Instance.paused  && !TextManager.instance.dialogueOpen)
        {
            //Control view direction
            rotation += rotationSpeed * Input.GetAxisRaw("Mouse X") * Time.deltaTime;
            if (rotation < 0)
            {
                rotation += 360;
            }
            else if (rotation > 360)
            {
                rotation -= 360;
            }

            float angleRadians = rotation * Mathf.Deg2Rad;
            Vector3 forward = new(Mathf.Sin(angleRadians), 0, Mathf.Cos(angleRadians));
            Vector3 side = Vector3.Cross(Vector3.up,forward);
            velocity = forward * Input.GetAxisRaw("Vertical") + side * Input.GetAxisRaw("Horizontal");
            velocity.Normalize();
            
            RaycastInteractable();
        }
    }
    private void FixedUpdate()
    {
        if (!PauseManager.Instance.paused && !TextManager.instance.dialogueOpen)
        {
            //loop the player o they stay in the map
            float x = transform.position.x;

            float y = transform.position.z;
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

            transform.position = new Vector3(x, playerHeight, y);
            rb.velocity = speed * velocity;
            transform.rotation = Quaternion.AngleAxis(rotation,Vector3.up);
        }

        
    }

    public float getRotation()
    {
        return rotation;
    }
    
    private void RaycastInteractable()
    {

        Vector3 origin = mainCamera.transform.position;
        Vector3 lookDirection = mainCamera.transform.forward;
        if (Physics.Raycast(origin, lookDirection, out RaycastHit hit))
        {
            bool hitInteractable =
                hit.transform.gameObject.TryGetComponent<Interactable>(out Interactable interactable);
            crosshair.SetOnTarget(hitInteractable);
            if(hitInteractable)
            {
                
            }
        }
        
    }
}
