﻿using UnityEngine;
    
public class PlayerSync : MonoBehaviour
{
	Vector3 lastPosition;
	Quaternion lastRotation;
  Vector3 lastSpineRotation;
  Transform myTransform;
  NetworkView thisNetworkView;
  Vector3 targetSpineRotation;
	[SerializeField] float rotThreshold = 5f;
  [SerializeField] private Character characterBase = null;
  private NetworkManager networkManager = null;  
  public bool IsMine = false;

  private void Start ()
	{
    thisNetworkView = GetComponent<NetworkView>();
    networkManager = FindObjectOfType<NetworkManager>();
    myTransform = transform;
  }  
	
	private void Update ()
	{
    if (networkManager.HasInternet && networkManager.IsConnectToMasterServer)
    {
      if (IsMine)
      {
        SendMovement();
      }
      else
      {
        ApplyMovement();
      }
    }
	}
	
	private void SendMovement()
	{
    if (Vector2.Angle(characterBase.SpineBoneJoystickAngle, lastSpineRotation) >= rotThreshold)
    {
      lastSpineRotation = characterBase.SpineBoneJoystickAngle;
      thisNetworkView.RPC("UpdateMovement", RPCMode.OthersBuffered, myTransform.position, myTransform.rotation, characterBase.SpineBoneJoystickAngle);      
    }
  }
	
	private void ApplyMovement ()
	{
		characterBase.SpineBoneNetworkAngle = Vector3.Lerp(characterBase.SpineBoneJoystickAngle, targetSpineRotation, 0.5f);
  }
	
	[RPC]
	private void UpdateMovement (Vector3 newPosition, Quaternion newRotation, Vector3 newSpineRotation)
	{
		targetSpineRotation = newSpineRotation;    
	}

  public void TryNetworkShoot(bool hasTarget, bool toHead)
  {
    if (networkManager.HasInternet && networkManager.IsConnectToMasterServer)
      thisNetworkView.RPC("NetworkShoot", RPCMode.OthersBuffered, hasTarget, toHead);
  }

  [RPC]
  private void NetworkShoot(bool hasTarget, bool toHead)
  {
    characterBase.NetworkTryShoot(hasTarget, toHead);
  }
}