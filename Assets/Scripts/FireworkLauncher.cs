using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FireworkLauncher : MonoBehaviour {

	public int posDivider = 5;

	public GameObject launcher;

	public Slider autoLauncher;
	public float secondsPerFirework = 0;


	// Use this for initialization
	void Start () {
		float autoFireworksPerMinute = autoLauncher.value;
		float autoFireworksPerSecond = autoFireworksPerMinute/60.0f;

		secondsPerFirework = 1/autoFireworksPerSecond;
	}
	
	// Update is called once per frame
	void UpdateAutoLauncher() {
		float autoFireworksPerMinute = autoLauncher.value;
		float autoFireworksPerSecond = autoFireworksPerMinute/60.0f;

		secondsPerFirework = 1/autoFireworksPerSecond;
	}


	public void Launch(GameObject firework)
	{
		//move to a location
		if(firework != null)
		{
			firework.transform.position = launcher.transform.position;
			int Displace = (int)((Random.value * launcher.transform.localScale.z) - (launcher.transform.localScale.z/2));
			Displace /= 5;
			Vector3 DisplacePos = new Vector3(0,0,(float)Displace*posDivider);
			firework.transform.position += DisplacePos;

			firework.GetComponent<ParticleSystem>().Play();
		}
	}

}
