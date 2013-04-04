var currentWeapon = 0; 
var numWeapons = 1; 

function Awake () 
{
	SelectWeapon(0);
}

function Update () 
{	
	if ( !Input.GetButton("Fire1"))
	{
		if (Input.GetAxis("Mouse ScrollWheel") < 0)
		{
			if (currentWeapon + 1 <= numWeapons)
			{
				currentWeapon++;
			}
	
			else 
			{
				currentWeapon = 0;
			}
	
	        SelectWeapon(currentWeapon);
			
			}
	
		else if (Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			if (currentWeapon - 1 >= 0)
			{
				currentWeapon--;
			}
	
			else 
			{
				currentWeapon = numWeapons;
			}
	
			SelectWeapon(currentWeapon);
	
		}
	}
	if (Input.GetKeyDown("1"))
	{
        SelectWeapon(0);
    }   

    else if (Input.GetKeyDown("2"))
    {
        SelectWeapon(1);
    }
}

 

function SelectWeapon (index : int) 
{

    for (var i=0;i<transform.childCount;i++)

    {

        // Activate the selected weapon

        if (i == index)

            transform.GetChild(i).gameObject.SetActive(true);

        // Deactivate all other weapons

        else

            transform.GetChild(i).gameObject.SetActive(false);

    }

}