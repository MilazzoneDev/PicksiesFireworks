using UnityEngine;
using System.Collections;

public class BurstFireworkBundle : FireworkBundle {

	public BurstFireworkBundle(GameObject fireworkObject, string type):base(fireworkObject,type)
	{
	}

	public override GameObject Generate(Color firstColor, Color secondColor)
	{
		if(firstColor == Color.clear){ firstColor = Color.white;}
		if(secondColor == Color.clear) { secondColor = Color.white;}

		//change color
		GameObject firework = getNextAvailable();
		ParticleSystem smoke = firework.GetComponent<ParticleSystem>().subEmitters.death0;
		ParticleSystem stage1 = firework.GetComponent<ParticleSystem>().subEmitters.death1;
		ParticleSystem stage2 = smoke.subEmitters.birth0;

		stage1.startColor = firstColor;
		stage2.startColor = secondColor;

		return firework;
	}
}
