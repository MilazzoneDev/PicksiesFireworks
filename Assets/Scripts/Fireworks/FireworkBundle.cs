using UnityEngine;
using System.Collections;

public class FireworkBundle {

	public static GameObject FireworkParent;

	private string fireworkName;
	private GameObject prefab;
	private ArrayList bundle;

	public string FireworkName
	{
		get { return fireworkName; }
		private set { fireworkName = value; }
	}

	public FireworkBundle(GameObject fireworkObject, string type)
	{
		bundle = new ArrayList();

		FireworkName = type;
		prefab = fireworkObject;
	}

	public virtual GameObject Generate(Color firstColor, Color secondColor)
	{
		return getNextAvailable();
	}

	protected GameObject getNextAvailable()
	{
		for(int i=0; i<bundle.Count;i++)
		{
			GameObject emitter = (GameObject)bundle[i];
			ParticleSystem particles = emitter.GetComponent<ParticleSystem>();
			if(!particles.IsAlive(true))
			{
				if(particles.subEmitters.death0 != null)
				{
					if(particles.subEmitters.death0.IsAlive(true))
					{
						continue;
					}
				}
				if(particles.subEmitters.death1 != null)
				{
					if(particles.subEmitters.death1.IsAlive(true))
					{
						continue;
					}
				}

				return (GameObject)bundle[i];
			}
		}
		//no available emitters were found
		bundle.Add(createNewEmitter());
		return (GameObject)bundle[bundle.Count-1];
	}

	GameObject createNewEmitter()
	{
		GameObject newFirework;

		newFirework = GameObject.Instantiate(prefab);

		if(FireworkParent != null)
		{
			newFirework.transform.SetParent(FireworkParent.transform);
		}

		return newFirework;
	}


}
