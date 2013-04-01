var floatup;
var floatAmount = 0.3;
function Start(){
    floatup = false;
}
function Update(){
    if(floatup)
        floatingup();
    else if(!floatup)
        floatingdown();
}
function floatingup(){
    transform.position.y += floatAmount * Time.deltaTime;
    yield WaitForSeconds(1);
    floatup = false;
}
function floatingdown(){
    transform.position.y -= floatAmount * Time.deltaTime;;
    yield WaitForSeconds(1);
    floatup = true;
}