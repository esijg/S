static public var pitchLevel : float;
if (pitchLevel < 0.1) {
	pitchLevel=0.1;
}
if (pitchLevel > 3) {
	pitchLevel=3;
}
audio.pitch = pitchLevel;
scale = pitchLevel*0.5;
transform.localScale += Vector3(scale, scale, scale);
