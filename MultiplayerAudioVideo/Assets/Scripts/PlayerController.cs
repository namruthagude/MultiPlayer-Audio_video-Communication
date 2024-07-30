using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Agora_RTC_Plugin.API_Example.Examples.Basic.JoinChannelAudio;
using Agora_RTC_Plugin.API_Example.Examples.Basic.JoinChannelVideo;

public class PlayerController : NetworkBehaviour
{
    [SerializeField]
    private float _speed = 5f; // Speed should be a float for proper movement calculations
    private Vector3 networkPos;
    private float lerpRate = 5f; // Rate at which the remote player interpolates to the correct position

    void Start()
    {
        networkPos = transform.position;
    }

    void Update()
    {
        // Ensure only the local player can control its own character
        if (Object.HasInputAuthority)
        {
            HandleMovement();
        }
        else
        {
            SmoothMove();
        }
    }

    private void HandleMovement()
    {
        // Handle continuous movement using Input.GetKey
        float moveHorizontal = 0f;
        float moveVertical = 0f;

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            moveVertical = 1f;
        }
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            moveVertical = -1f;
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            moveHorizontal = -1f;
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            moveHorizontal = 1f;
        }

        Vector2 movement = new Vector2(moveHorizontal, moveVertical).normalized * _speed * Time.deltaTime;
        transform.Translate(movement);
    }

    private void SmoothMove()
    {
        transform.position = Vector3.Lerp(transform.position, networkPos, Time.deltaTime * lerpRate);
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            Vector3 movement = new Vector3(data.movementInput.x, data.movementInput.y, 0) * _speed * Runner.DeltaTime;
            transform.Translate(movement);

            // Send position data to other players
            networkPos = transform.position;
        }
    }

    public override void Render()
    {
        if (!Object.HasInputAuthority)
        {
            SmoothMove();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Object.HasInputAuthority && collision.gameObject.CompareTag("Player"))
        {
            JoinChannelAudio.Instance.JoinChannel();
            JoinChannelVideo.Instance.JoinChannel();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (Object.HasInputAuthority && collision.gameObject.CompareTag("Player"))
        {
            JoinChannelAudio.Instance.LeaveChannel();
            JoinChannelVideo.Instance.LeaveChannel();
        }
    }
}

public struct NetworkInputData : INetworkInput
{
    public Vector2 movementInput;
}
