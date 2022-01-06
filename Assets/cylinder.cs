using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]




public class cylinder : Photon.MonoBehaviour
{
    public float timer;

    private void Update()
    {
        timer -= Time.unscaledDeltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);
        }

    }

    private Vector3 selfPos;

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            selfPos = (Vector3)stream.ReceiveNext();
        }
    }
}