using UnityEngine;
using System.Collections;

public class BasicFireworkBundle : FireworkBundle {

	public BasicFireworkBundle(GameObject fireworkObject, string type):base(fireworkObject,type)
	{
		
	}

	public override GameObject Generate(Color firstColor, Color secondColor)
	{
		GameObject firework = getNextAvailable();

		if(firstColor == Color.clear) { firstColor = Color.red; }
		if(secondColor == Color.clear) { secondColor = Color.white; }

		//change color
		ParticleSystem child = firework.GetComponent<ParticleSystem>().subEmitters.death0;
		child.startColor = firstColor;

		ParticleSystem.ColorOverLifetimeModule lifetime = child.GetComponent<ParticleSystem>().colorOverLifetime;
		Gradient grad = new Gradient();

		grad.SetKeys( new GradientColorKey[] { new GradientColorKey(firstColor, 0.0f), new GradientColorKey(secondColor, 1.0f)}, 
			new GradientAlphaKey[] { new GradientAlphaKey(1.0f,0.0f), new GradientAlphaKey(1.0f,1.0f)});

		lifetime.color = new ParticleSystem.MinMaxGradient(grad);

		return firework;
	}
}
