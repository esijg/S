public var clipStorage:AudioClip[];

function Update()
{
	if (audio.enabled)
	{
		if (!audio.isPlaying)
		{
			var audioClipNum:int = Random.Range(0,(clipStorage.Length));
			audio.clip = clipStorage[audioClipNum];
	        audio.Play();
		}
	}
}