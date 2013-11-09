#pragma strict
 
var float y_deg;
var boolean mouth_was_open = false;
var boolean mouth_is_open = false;
var cameraObject : GameObject;
var move_direction = 0;

function Matrix(){
	this.rows = new Array();
}

static function MoveOverTime( theTransform : Transform, d : Vector3, t : float )		    {
	    var rate : float = 1.0/t;
	    var index : float = 0.0;
	    var startPosition : Vector3 = theTransform.position;
	    var endPosition : Vector3 = startPosition + d;
	    while( index < 0.0 )
	    {
	    theTransform.position = Vector3.Lerp( startPosition, endPosition, index );
	    index += rate * Time.deltaTime;
	    yield;
	    }
	    theTransform.position = endPosition;
	    }
//can I make the matrix const and pass by reference?
static function move_controller(gameObject : GameObject, direction : int, matrix){
	    
	
		
	

function Start () {
	var matrix = new Matrix();
	m.rows[0] = new Array( /*row 1*/
	//more rows for whole grid
	cameraObject  = GameObject.Find("CameraLeft");
}

function Update () {
	//check to see if mouth is open
	if(mouth_was_open != mouth_is_open){
		y_deg = cameraObject.transform.eulerAngles.y;
		Debug.Log(y_deg);
		if(y_deg > 45 && y_deg <= 135){
			move_direction = 1;
		}else if(y_deg > 135 && y_deg <= 225){
			move_direction = 8;
		}else if(y_deg > 225 && y_deg <= 315){
			move_direction = 4;
		}else if(y_deg > 315 || y_deg <= 45){
			move_direction = 2;
		}
		mouth_was_open = !mouth_was_open;
		move_controller(gameObject, move_direction, matrix);
	}
	/*var a = gameObject.transform.position.x;
	var b = gameObject.transform.position.y;
	var c = gameObject.transform.position.z;
	transform.Translate(Vector3(Mathf.Sin(y_deg), 0, Mathf.Cos(y_deg)) * Time.deltaTime);
	Debug.Log( (a - gameObject.transform.position.x) + " " + (b - gameObject.transform.position.y) + " " + (c - gameObject.transform.position.z));*/
}