using UnityEngine;
using System.Collections;

public class RetroFireworkBundle : FireworkBundle {

	public RetroFireworkBundle(GameObject fireworkObject, string type):base(fireworkObject,type)
	{
	}

	public override GameObject Generate(Color firstColor, Color secondColor)
	{
		if(firstColor == Color.clear) { firstColor = Color.red; }
		if(secondColor == Color.clear) { secondColor = Color.yellow; }

		GameObject firework = getNextAvailable();
		ParticleSystem child = firework.GetComponent<ParticleSystem>().subEmitters.death0;
		ParticleSystem child2 = child.subEmitters.birth0;

		ParticleSystem.ColorOverLifetimeModule lifetime = child2.colorOverLifetime;
		Gradient grad = new Gradient();

		grad.SetKeys( new GradientColorKey[] { new GradientColorKey(firstColor, 0.0f), new GradientColorKey(secondColor, 1.0f)}, 
			new GradientAlphaKey[] { new GradientAlphaKey(1.0f,0.0f), new GradientAlphaKey(1.0f,1.0f)});

		lifetime.color = new ParticleSystem.MinMaxGradient(grad);

		return firework;
	}
}
