var audioClip1 : AudioClip;
var audioClip2 : AudioClip;
var audioClip3 : AudioClip;

function OnTriggerEnter(other : Collider)
{
	if (other.tag == "Player")
	{
		Camera.main.SendMessage("fadeOut");
		yield WaitForSeconds(20.0);
//		var clipNr = 0;

/*		while (!clipNr == 3)
		{
		    if (!audio.isPlaying)
		    {
			    if (clipNr == 0)
			    {
			        audio.clip = audioClip1;
			        audio.Play();
			    }

			    if (clipNr == 1)
			    {
			        audio.clip = audioClip2;
			        audio.Play();
			    }

			    if (clipNr == 2)
			    {
			        audio.clip = audioClip3;
			        audio.Play();
			    }

		        clipNr += 1;
	    	}
	    } */

	    Application.Quit();
	}
}