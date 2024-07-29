using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Agora_RTC_Plugin.API_Example.Examples.Basic.JoinChannelAudio;
using Agora_RTC_Plugin.API_Example.Examples.Basic.JoinChannelVideo;

public class PlayerController : MonoBehaviourPun, IPunObservable
{
    [SerializeField]
    private float _speed = 5f; // Speed should be a float for proper movement calculations
    private Vector3 networkPos;
    private float lerpRate = 5f; // Rate at which the remote player interpolates to the correct position

    void Start()
    {
        networkPos = transform.position;
        PhotonView photon = GetComponent<PhotonView>();
        if( photon != null)
        {
            photon.ObservedComponents.Add(this);
        }
    }

    void Update()
    {
        Debug.Log("Network pos " +  networkPos);
        // Ensure only the local player can control its own character
        if (photonView.IsMine)
        {
            //Debug.Log("Handling movement");
            HandleMovement();
        }
        else
        {
            //Debug.Log("Moving Smoothly");
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            Debug.Log("Sending Data");
            // Send position data to other players
            stream.SendNext(transform.position);
        }
        else
        {
            Debug.Log("Receiving Data");
            // Receive position data from other players
            networkPos = (Vector3)stream.ReceiveNext();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(photonView.IsMine && collision.gameObject.CompareTag("Player"))
        {
            JoinChannelAudio.Instance.JoinChannel();
            JoinChannelVideo.Instance.JoinChannel();
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (photonView.IsMine && collision.gameObject.CompareTag("Player"))
        {
            JoinChannelAudio.Instance.LeaveChannel();
            JoinChannelVideo.Instance.LeaveChannel();
        }
    }
}
