﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
//using OpenNI;

public class UserDetector : MonoBehaviour {

//	[SerializeField] OpenNIUserTracker tracker;
	[SerializeField] UserDetectHandler handler;
	[SerializeField] float ComeInTreshod = -2000;
	[SerializeField] float stopAbstractTreshod = - 800;

	public enum State
	{
		None,
		Pass,
		Come,
		Stop,
	}

	State m_state = State.None;
	State state {
		get {
			return m_state;
		}
		set {
			if ( m_state != value )
			{
				m_state = value;
				if ( value == State.Pass )
					Pass();
				if ( value == State.Come )
					Come();
				if ( value == State.Stop )
					Stop();
				if ( value == State.None )
					Leave();
			}
		}
	}

	void LateUpdate()
	{
		UpdateUserInfo ();
	}
		
	Vector3 closetPosition;
	int temID;

	void UpdateUserInfo()
	{
		if ( state == State.None ) 
			return;
		
		//detect closest position
//		if ( tracker.AllUsers.Count <= 0 ) 
//			return;
//		
//		closetPosition.z = -99999f;
//		foreach( int id in tracker.AllUsers )
//		{
//			Vector3 pos = tracker.GetUserCenterOfMass( id );
//			if ( Mathf.Abs( pos.z ) < Mathf.Abs( closetPosition.z ) )
//				closetPosition = pos;
//		}
		if (ZigInput.Instance.TrackedUsers.Values.Count <= 0)
			return;
		
		closetPosition.z = -99999f;
		foreach (ZigTrackedUser currentUser in ZigInput.Instance.TrackedUsers.Values)
		{
			Vector3 pos = currentUser.Position;
			if ( Mathf.Abs( pos.z ) < Mathf.Abs( closetPosition.z ) && pos !=  Vector3.zero )
				closetPosition = pos;
		}

		if ( closetPosition.magnitude > 0 )
				Debug.Log(" distance " + closetPosition );

		switch( state )
		{
		case State.Pass:
			if ( Mathf.Abs( closetPosition.z ) < Mathf.Abs ( ComeInTreshod ))
			{
				state = State.Come;
			}
			break;
		case State.Come:
			if ( Math.Abs( closetPosition.z ) > Mathf.Abs( ComeInTreshod ))
			{
				state = State.Pass;
			}else if (  Math.Abs( closetPosition.z ) < Mathf.Abs( stopAbstractTreshod ) )
			{
				state = State.Stop;
			}
		break;
		case State.Stop:
			break;
		}
	}

//	IEnumerator RecordOriPosition()
//	{
//		while ( oriPosition.Equals(Vector3.zero) ) {
//			yield return null;
//			oriPosition = tracker.GetUserCenterOfMass (temID);
//		}
//	}

//	public bool positionCloser()
//	{
//		Vector3 move = closetPosition - oriPosition;
//
//		Debug.Log ("Check closer" + move);
//		if (move.z > comeRelativeTreshod || closetPosition.z > stopAbstractTreshod )
//			return true;
//
//		return false;
////		if (positionList.Count < cachePositionNum)
////			return false;
////		for (int i = 0; i < positionList.Count - 1 ; ++i) {
////			if (!(positionList [i].z < positionList [i + 1].z))
////				return false;
////		}
////		return true;
//	}
//
//	public bool positionUnmove()
//	{
//		Debug.Log ("CheckUnmove " + closetPosition);
//		if (closetPosition.z > stopAbstractTreshod)
//			return true;
//		return false;
////		if (positionList.Count < cachePositionNum)
////			return false;
////		for (int i = 0; i < positionList.Count - 1 ; ++i) {
////			if ( ! ( (positionList [i] - positionList [i + 1]).magnitude < 0.1f  ) ) 
////				return false;
////		}
////		return true;
//	}

//	public void UserDetected( NewUserEventArgs e )
//	{
//		if (state == State.None) {
//			state = State.Pass;
//			Pass();
//		}
//
//	}

//	public void AllUsersLost( UserLostEventArgs e )
//	{
//		state = State.None;
//	}

//	public void UserLost( UserLostEventArgs e )
//	{
//		if (  ZigInput.Instance.TrackedUsers.Values.Count <= 0 )
//			state = State.None;
//	}


	void Zig_UserFound(ZigTrackedUser user) {
		if (state == State.None) {
			state = State.Pass;
			Pass();
		}
	}

	void Zig_UserLost(ZigTrackedUser user) {
		if (  ZigInput.Instance.TrackedUsers.Values.Count <= 0 )
			state = State.None;
	}

	public void Pass()
	{
		state = State.Pass;
		if ( handler != null )
			handler.UserPass ();
	}

	public void Come()
	{
		state = State.Come;
		if ( handler != null )
			handler.UserCome ();
	}

	public void Stop()
	{
		state = State.Stop;
		if ( handler != null )
			handler.UserStop ();
	}

	public void Leave()
	{
		state = State.None;
		if (handler != null)
			handler.UserLeave ();
	}
		
}
