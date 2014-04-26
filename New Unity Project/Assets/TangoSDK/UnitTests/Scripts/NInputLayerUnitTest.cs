// To actually perform the unit test, uncomment this line, and copy the functions at the bottom into the FingerGestures class.
//#define DO_UNIT_TEST

using UnityEngine;
using System.Collections;

public class NInputLayerUnitTest : MonoBehaviour
{
#if DO_UNIT_TEST
	bool pressDown = false;
	bool pressUp = false;
	bool stationaryPressBegin = false;
	bool stationaryPress = false;
	bool stationaryPressEnd = false;
	bool pressMoveBegin = false;
	bool pressMove = false;
	bool pressMoveEnd = false;
	bool longPress = false;
	bool pressDragBegin = false;
	bool pressDragMove = false;
	bool pressDragEnd = false;
	bool shortPress = false;
	bool doublePress = false;
	bool pressScreenSwipe = false;
	bool pinchBegin = false;
	bool pinchMove = false;
	bool pinchEnd = false;
	bool rotationBegin = false;
	bool rotationMove = false;
	bool rotationEnd = false;
	bool twoFingerDragBegin = false;
	bool twoFingerDragMove = false;
	bool twoFingerDragEnd = false;
	bool twoFingerPress = false;
	bool twoFingerSwipe = false;
	bool twoFingerLongPress = false;
	
	void Start ()
	{
		NInputLayer.OnPressDown += ReceivePressDown;
		NInputLayer.OnPressUp += ReceivePressUp;
		NInputLayer.OnStationaryPressBegin += ReceiveStationaryPressBegin;
		NInputLayer.OnStationaryPress += ReceiveStationaryPress;
		NInputLayer.OnStationaryPressEnd += ReceiveStationaryPressEnd;
		NInputLayer.OnPressMoveBegin += ReceivePressMoveBegin;
		NInputLayer.OnPressMove += ReceivePressMove;
		NInputLayer.OnPressMoveEnd += ReceivePressMoveEnd;
		NInputLayer.OnLongPress += ReceiveLongPress;
		NInputLayer.OnPressDragBegin += ReceivePressDragBegin;
		NInputLayer.OnPressDragMove += ReceivePressDragMove;
		NInputLayer.OnPressDragEnd += ReceivePressDragEnd;
		NInputLayer.OnShortPress += ReceiveShortPress;
		NInputLayer.OnDoublePress += ReceiveDoublePress;
		NInputLayer.OnSwipe += ReceivePressScreenSwipe;
		NInputLayer.OnPinchBegin += ReceivePinchBegin;
		NInputLayer.OnPinchMove += ReceivePinchMove;
		NInputLayer.OnPinchEnd += ReceivePinchEnd;
		NInputLayer.OnRotationBegin += ReceiveRotationBegin;
		NInputLayer.OnRotationMove += ReceiveRotationMove;
		NInputLayer.OnRotationEnd += ReceiveRotationEnd;
		NInputLayer.OnTwoFingerDragBegin += ReceiveTwoFingerDragBegin;
		NInputLayer.OnTwoFingerDragMove += ReceiveTwoFingerDragMove;
		NInputLayer.OnTwoFingerDragEnd += ReceiveTwoFingerDragEnd;
		NInputLayer.OnTwoFingerPress += ReceiveTwoFingerPress;
		NInputLayer.OnTwoFingerSwipe += ReceiveTwoFingerSwipe;
		NInputLayer.OnTwoFingerLongPress += ReceiveTwoFingerLongPress;
		
		// Actual tests
		FingerGestures.TestOnFingerDown( 0, Vector2.zero );
		if( !pressDown )
			Debug.LogError( "TestOnFingerDown FAILED!" );
		
		FingerGestures.TestOnFingerUp( 0, Vector2.zero, 0f );
		if( !pressUp )
			Debug.LogError( "TestOnFingerUp FAILED!" );
		
		FingerGestures.TestOnFingerStationaryBegin( 0, Vector2.zero );
		if( !stationaryPressBegin )
			Debug.LogError( "TestOnFingerStationaryBegin FAILED!" );
		
		FingerGestures.TestOnFingerStationary( 0, Vector2.zero, 0f );
		if( !stationaryPress )
			Debug.LogError( "TestOnFingerStationary FAILED!" );
		
		FingerGestures.TestOnFingerStationaryEnd( 0, Vector2.zero, 0f );
		if( !stationaryPressEnd )
			Debug.LogError( "TestOnFingerStationaryEnd FAILED!" );
		
		FingerGestures.TestOnFingerMoveBegin( 0, Vector2.zero );
		if( !pressMoveBegin )
			Debug.LogError( "TestOnFingerMoveBegin FAILED!" );
		
		FingerGestures.TestOnFingerMove( 0, Vector2.zero );
		if( !pressMove )
			Debug.LogError( "TestOnFingerMove FAILED!" );
		
		FingerGestures.TestOnFingerMoveEnd( 0, Vector2.zero );
		if( !pressMoveEnd )
			Debug.LogError( "TestOnFingerMoveEnd FAILED!" );
		
		FingerGestures.TestOnFingerLongPress( 0, Vector2.zero );
		if( !longPress )
			Debug.LogError( "TestOnFingerLongPress FAILED!" );
		
		FingerGestures.TestOnFingerDragBegin( 0, Vector2.zero, Vector2.zero );
		if( !pressDragBegin )
			Debug.LogError( "TestOnFingerDragBegin FAILED!" );
		
		FingerGestures.TestOnFingerDragMove( 0, Vector2.zero, Vector2.zero );
		if( !pressDragMove )
			Debug.LogError( "TestOnFingerDragMove FAILED!" );
		
		FingerGestures.TestOnFingerDragEnd( 0, Vector2.zero );
		if( !pressDragEnd )
			Debug.LogError( "TestOnFingerDragEnd FAILED!" );
		
		FingerGestures.TestOnFingerPress( 0, Vector2.zero );
		if( !shortPress )
			Debug.LogError( "TestOnFingerPress FAILED!" );
		
		FingerGestures.TestOnFingerDoublePress( 0, Vector2.zero );
		if( !doublePress )
			Debug.LogError( "TestOnFingerDoublePress FAILED!" );
		
		FingerGestures.TestOnFingerSwipe( 0, Vector2.zero, FingerGestures.SwipeDirection.None, 0f );
		if( !pressScreenSwipe )
			Debug.LogError( "TestOnFingerSwipe FAILED!" );
		
		FingerGestures.TestOnPinchBegin( Vector2.zero, Vector2.zero );
		if( !pinchBegin )
			Debug.LogError( "TestOnPinchBegin FAILED!" );
		
		FingerGestures.TestOnPinchMove( Vector2.zero, Vector2.zero, 0f );
		if( !pinchMove )
			Debug.LogError( "TestOnPinchMove FAILED!" );
		
		FingerGestures.TestOnPinchEnd( Vector2.zero, Vector2.zero );
		if( !pinchEnd )
			Debug.LogError( "TestOnPinchEnd FAILED!" );
		
		FingerGestures.TestOnRotationBegin( Vector2.zero, Vector2.zero );
		if( !rotationBegin )
			Debug.LogError( "TestOnRotationBegin FAILED!" );
		
		FingerGestures.TestOnRotationMove( Vector2.zero, Vector2.zero, 0f );
		if( !rotationMove )
			Debug.LogError( "TestOnRotationMove FAILED!" );
		
		FingerGestures.TestOnRotationEnd( Vector2.zero, Vector2.zero, 0f );
		if( !rotationEnd )
			Debug.LogError( "TestOnRotationEnd FAILED!" );
		
		FingerGestures.TestOnTwoFingerDragBegin( Vector2.zero, Vector2.zero );
		if( !twoFingerDragBegin )
			Debug.LogError( "TestOnTwoFingerDragBegin FAILED!" );
		
		FingerGestures.TestOnTwoFingerDragMove( Vector2.zero, Vector2.zero );
		if( !twoFingerDragMove )
			Debug.LogError( "TestOnTwoFingerDragMove FAILED!" );
		
		FingerGestures.TestOnTwoFingerDragEnd( Vector2.zero );
		if( !twoFingerDragEnd )
			Debug.LogError( "TestOnTwoFingerDragEnd FAILED!" );
		
		FingerGestures.TestOnTwoFingerPress( Vector2.zero );
		if( !twoFingerPress )
			Debug.LogError( "TestOnTwoFingerPress FAILED!" );
		
		FingerGestures.TestOnTwoFingerSwipe( Vector2.zero, FingerGestures.SwipeDirection.None, 0f );
		if( !twoFingerSwipe )
			Debug.LogError( "TestOnTwoFingerSwipe FAILED!" );
		
		FingerGestures.TestOnTwoFingerLongPress( Vector2.zero );
		if( !twoFingerLongPress )
			Debug.LogError( "TestOnTwoFingerLongPress FAILED!" );
		
		Debug.Log("ALL TESTS PASSED");
	}
	
	// Callbacks
	private void ReceivePressDown( int fingerIndex, Vector2 position )
	{
		pressDown = true;
		Debug.Log( "ReceivePressDown" );
	}
	private void ReceivePressUp( int fingerIndex, Vector2 position, float timeHeldDown )
	{
		pressUp = true;
		Debug.Log( "ReceivePressUp" );
	}	
	private void ReceiveStationaryPressBegin( int fingerIndex, Vector2 position )
	{
		stationaryPressBegin = true;
		Debug.Log( "ReceiveStationaryPressBegin" );
	}
	private void ReceiveStationaryPress( int fingerIndex, Vector2 position, float elapsedTime )
	{
		stationaryPress = true;
		Debug.Log( "ReceiveStationaryPress" );
	}
	private void ReceiveStationaryPressEnd( int fingerIndex, Vector2 position, float elapsedTime )
	{
		stationaryPressEnd = true;
		Debug.Log( "ReceiveStationaryPressEnd" );
	}
	private void ReceivePressMoveBegin( int fingerIndex, Vector2 position )
	{
		pressMoveBegin = true;
		Debug.Log( "ReceivePressMoveBegin" );
	}	
	private void ReceivePressMove( int fingerIndex, Vector2 position )
	{
		pressMove = true;
		Debug.Log( "ReceivePressMove" );
	}
	private void ReceivePressMoveEnd( int fingerIndex, Vector2 position )
	{
		pressMoveEnd = true;
		Debug.Log( "ReceivePressMoveEnd" );
	}
	private void ReceiveLongPress( int fingerIndex, Vector2 position )
	{
		longPress = true;
		Debug.Log( "ReceiveLongPress" );
	}
	private void ReceivePressDragBegin( int fingerIndex, Vector2 position, Vector2 startPosition )
	{
		pressDragBegin = true;
		Debug.Log( "ReceivePressDragBegin" );
	}
	private void ReceivePressDragMove( int fingerIndex, Vector2 position, Vector2 startPosition )
	{
		pressDragMove = true;
		Debug.Log( "ReceivePressDragMove" );
	}
	private void ReceivePressDragEnd( int fingerIndex, Vector2 fingerPos )
	{
		pressDragEnd = true;
		Debug.Log( "ReceivePressDragEnd" );
	}
	private void ReceiveShortPress( int fingerIndex, Vector2 position )
	{
		shortPress = true;
		Debug.Log( "ReceiveShortPress" );
	}
	private void ReceiveDoublePress( int fingerIndex, Vector2 position )
	{
		doublePress = true;
		Debug.Log( "ReceiveDoublePress" );
	}
	private void ReceivePressScreenSwipe( int fingerIndex, Vector2 startPos, NInputLayer.eDirection direction, float velocity )
	{
		pressScreenSwipe = true;
		Debug.Log( "ReceivePressScreenSwipe" );
	}
	private void ReceivePinchBegin( Vector2 fingerPos1, Vector2 fingerPos2 )
	{
		pinchBegin = true;
		Debug.Log( "ReceivePinchBegin" );
	}
	private void ReceivePinchMove( Vector2 fingerPos1, Vector2 fingerPos2, float delta )
	{
		pinchMove = true;
		Debug.Log( "ReceivePinchMove" );
	}
	private void ReceivePinchEnd( Vector2 fingerPos1, Vector2 fingerPos2 )
	{
		pinchEnd = true;
		Debug.Log( "ReceivePinchEnd" );
	}
	private void ReceiveRotationBegin( Vector2 fingerPos1, Vector2 fingerPos2 )
	{
		rotationBegin = true;
		Debug.Log( "ReceiveRotationBegin" );
	}
	private void ReceiveRotationMove( Vector2 fingerPos1, Vector2 fingerPos2, float rotationAngleDelta )
	{
		rotationMove = true;
		Debug.Log( "ReceiveRotationMove" );
	}
	private void ReceiveRotationEnd( Vector2 fingerPos1, Vector2 fingerPos2, float totalRotationAngle )
	{
		rotationEnd = true;
		Debug.Log( "ReceiveRotationEnd" );
	}
	private void ReceiveTwoFingerDragBegin( Vector2 fingerPos, Vector2 startPos )
	{
		twoFingerDragBegin = true;
		Debug.Log( "ReceiveTwoFingerDragBegin" );
	}
	private void ReceiveTwoFingerDragMove( Vector2 fingerPos, Vector2 delta )
	{
		twoFingerDragMove = true;
		Debug.Log( "ReceiveTwoFingerDragMove" );
	}
	private void ReceiveTwoFingerDragEnd( Vector2 fingerPos )
	{
		twoFingerDragEnd = true;
		Debug.Log( "ReceiveTwoFingerDragEnd" );
	}
	private void ReceiveTwoFingerPress( Vector2 fingerPos )
	{
		twoFingerPress = true;
		Debug.Log( "ReceiveTwoFingerPress" );
	}
	private void ReceiveTwoFingerSwipe( Vector2 startPos, NInputLayer.eDirection direction, float velocity )
	{
		twoFingerSwipe = true;
		Debug.Log( "ReceiveTwoFingerSwipe" );
	}
	private void ReceiveTwoFingerLongPress( Vector2 fingerPos )
	{
		twoFingerLongPress = true;
		Debug.Log( "ReceiveTwoFingerLongPress" );
	}
#endif
}

// YUCK - Only way to unit test NInputLayer with FingerGestures is to add these into the FingerGestures class for testing.
/*
	public static void TestOnFingerDown( int fingerIndex, Vector2 fingerPos )
    {
        if( OnFingerDown != null )
            OnFingerDown( fingerIndex, fingerPos );
    }

    public static void TestOnFingerUp( int fingerIndex, Vector2 fingerPos, float timeHeldDown )
    {
        if( OnFingerUp != null )
            OnFingerUp( fingerIndex, fingerPos, timeHeldDown );
    }

    public static void TestOnFingerStationaryBegin( int fingerIndex, Vector2 fingerPos )
    {
        if( OnFingerStationaryBegin != null )
            OnFingerStationaryBegin( fingerIndex, fingerPos );
    }

    public static void TestOnFingerStationary( int fingerIndex, Vector2 fingerPos, float elapsedTime )
    {
        if( OnFingerStationary != null )
            OnFingerStationary( fingerIndex, fingerPos, elapsedTime );
    }

    public static void TestOnFingerStationaryEnd( int fingerIndex, Vector2 fingerPos, float elapsedTime )
    {
        if( OnFingerStationaryEnd != null )
            OnFingerStationaryEnd( fingerIndex, fingerPos, elapsedTime );
    }

    public static void TestOnFingerMoveBegin( int fingerIndex, Vector2 fingerPos )
    {
        if( OnFingerMoveBegin != null )
            OnFingerMoveBegin( fingerIndex, fingerPos );
    }

    public static void TestOnFingerMove( int fingerIndex, Vector2 fingerPos )
    {
        if( OnFingerMove != null )
            OnFingerMove( fingerIndex, fingerPos );
    }

    public static void TestOnFingerMoveEnd( int fingerIndex, Vector2 fingerPos )
    {
        if( OnFingerMoveEnd != null )
            OnFingerMoveEnd( fingerIndex, fingerPos );
    }

    public static void TestOnFingerLongPress( int fingerIndex, Vector2 fingerPos )
    {
        if( OnFingerLongPress != null )
            OnFingerLongPress( fingerIndex, fingerPos );
    }

    public static void TestOnFingerDragBegin( int fingerIndex, Vector2 fingerPos, Vector2 startPos )
    {
        if( OnFingerDragBegin != null )
            OnFingerDragBegin( fingerIndex, fingerPos, startPos );
    }

    public static void TestOnFingerDragMove( int fingerIndex, Vector2 fingerPos, Vector2 delta )
    {
        if( OnFingerDragMove != null )
            OnFingerDragMove( fingerIndex, fingerPos, delta );
    }

    public static void TestOnFingerDragEnd( int fingerIndex, Vector2 fingerPos )
    {
        if( OnFingerDragEnd != null )
            OnFingerDragEnd( fingerIndex, fingerPos );
    }

    public static void TestOnFingerPress( int fingerIndex, Vector2 fingerPos )
    {
        if( OnFingerTap != null )
            OnFingerTap( fingerIndex, fingerPos );
    }

    public static void TestOnFingerDoublePress( int fingerIndex, Vector2 fingerPos )
    {
        if( OnFingerDoubleTap != null )
            OnFingerDoubleTap( fingerIndex, fingerPos );
    }

    public static void TestOnFingerSwipe( int fingerIndex, Vector2 startPos, SwipeDirection direction, float velocity )
    {
        if( OnFingerSwipe != null )
            OnFingerSwipe( fingerIndex, startPos, direction, velocity );
    }

    public static void TestOnPinchBegin( Vector2 fingerPos1, Vector2 fingerPos2 )
    {
        if( OnPinchBegin != null )
            OnPinchBegin( fingerPos1, fingerPos2 );
    }

    public static void TestOnPinchMove( Vector2 fingerPos1, Vector2 fingerPos2, float delta )
    {
        if( OnPinchMove != null )
            OnPinchMove( fingerPos1, fingerPos2, delta );
    }

    public static void TestOnPinchEnd( Vector2 fingerPos1, Vector2 fingerPos2 )
    {
        if( OnPinchEnd != null )
            OnPinchEnd( fingerPos1, fingerPos2 );
    }

    public static void TestOnRotationBegin( Vector2 fingerPos1, Vector2 fingerPos2 )
    {
        if( OnRotationBegin != null )
            OnRotationBegin( fingerPos1, fingerPos2 );
    }

    public static void TestOnRotationMove( Vector2 fingerPos1, Vector2 fingerPos2, float rotationAngleDelta )
    {
        if( OnRotationMove != null )
            OnRotationMove( fingerPos1, fingerPos2, rotationAngleDelta );
    }

    public static void TestOnRotationEnd( Vector2 fingerPos1, Vector2 fingerPos2, float totalRotationAngle )
    {
        if( OnRotationEnd != null )
            OnRotationEnd( fingerPos1, fingerPos2, totalRotationAngle );
    }

    public static void TestOnTwoFingerDragBegin( Vector2 fingerPos, Vector2 startPos )
    {
        if( OnTwoFingerDragBegin != null )
            OnTwoFingerDragBegin( fingerPos, startPos );
    }

    public static void TestOnTwoFingerDragMove( Vector2 fingerPos, Vector2 delta )
    {
        if( OnTwoFingerDragMove != null )
            OnTwoFingerDragMove( fingerPos, delta );
    }

    public static void TestOnTwoFingerDragEnd( Vector2 fingerPos )
    {
        if( OnTwoFingerDragEnd != null )
            OnTwoFingerDragEnd( fingerPos );
    }

    public static void TestOnTwoFingerPress( Vector2 fingerPos )
    {
        if( OnTwoFingerTap != null )
            OnTwoFingerTap( fingerPos );
    }

    public static void TestOnTwoFingerSwipe( Vector2 startPos, SwipeDirection direction, float velocity )
    {
        if( OnTwoFingerSwipe != null )
            OnTwoFingerSwipe( startPos, direction, velocity );
    }
    
    public static void TestOnTwoFingerLongPress( Vector2 fingerPos )
    {
        if( OnTwoFingerLongPress != null )
            OnTwoFingerLongPress( fingerPos );
    }
*/