using UnityEngine;
using System.Collections;

// basic touch manipulation:
// single touch -> drag move in camera plane
// double touch drag -> rotate/scale (scale can be uniform across all three axes)
// rotate is in the plane of the camera

public class TouchAxisRemoteRotateStatic: BBSimpleTouchableObject {

	public bool allowRotateX = true;
	public bool allowRotateY = true;
	public float factorY = 1;
	public float factorX = 1;
	
	public GameObject PanoReference;
	
	private Transform saveParent;
	private Vector3 movement;
	
	protected GameObject pivot;
		
	public override void handleSingleTouch(iPhoneTouch touch) 
	{
		//if (!allowDrag) return;
		
	//	print ("this is touch " + touch.phase);
		if(touch.phase == iPhoneTouchPhase.Ended) {
			print ("done");
		}
			
		// // we want to drag our object

		movement = touchMovementVector(touch);
	//	print ("This is movement " + movement);
//		if (movement.sqrMagnitude > 0.01) {
		this.startPivot(gameObject.transform.position); 
		if (allowRotateX) {	
		//	pivot.transform.Translate(0, movement.y, 0,Space.World);			
			PanoReference.transform.Rotate(movement.y * factorY, 0, 0,Space.World);
		}
		if (allowRotateY) {	
		//	pivot.transform.Translate(movement.x, 0, 0,Space.World);
			PanoReference.transform.Rotate(0, movement.x * factorX, 0,Space.World);
		}
			this.endPivot();			
//		}
	}

	
	virtual protected void startPivot(Vector3 pivotPosition)
	{			
		if (pivot == null) {
			pivot = new GameObject();
			pivot.name = "BBBasicTouchManipulation Pivot";
			pivot.transform.position = pivotPosition;		
		}	

		saveParent = gameObject.transform.parent;
		gameObject.transform.parent = null;
		pivot.transform.parent = saveParent;
		gameObject.transform.parent = pivot.transform;
	}

	virtual protected void endPivot()
	{
		gameObject.transform.parent = saveParent;		
		pivot.transform.parent = null;	
		Destroy(pivot);	
	}

	public Vector3 touchMovementVector(iPhoneTouch touch) 
	{
		float zDistanceFromCamera = Vector3.Distance(renderingCamera.transform.position,gameObject.transform.position);

		Vector3 screenPosition = new Vector3(touch.position.x,touch.position.y,zDistanceFromCamera);
		Vector3 lastScreenPosition = new Vector3(touch.position.x - touch.deltaPosition.x,touch.position.y - touch.deltaPosition.y,zDistanceFromCamera);

		Vector3 cameraWorldPosition = this.renderingCamera.ScreenToWorldPoint(screenPosition);
		Vector3 lastCameraWorldPosition = this.renderingCamera.ScreenToWorldPoint(lastScreenPosition);

		return cameraWorldPosition - lastCameraWorldPosition;
	}


}


