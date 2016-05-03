using UnityEngine;
using System.Collections;

public class SparkleFireworkBundle : FireworkBundle {

	public SparkleFireworkBundle(GameObject fireworkObject, string type):base(fireworkObject,type)
	{
	}

	public override GameObject Generate(Color firstColor, Color secondColor)
	{
		if(firstColor == Color.clear){ firstColor = Color.white;}
		if(secondColor == Color.clear) { secondColor = Color.white;}

		//change color
		GameObject firework = getNextAvailable();
		ParticleSystem child = firework.GetComponent<ParticleSystem>().subEmitters.death1;
		ParticleSystem child2 = child.GetComponent<ParticleSystem>().subEmitters.death0;

		ParticleSystem.ColorOverLifetimeModule lifetime = child2.GetComponent<ParticleSystem>().colorOverLifetime;
		Gradient grad = new Gradient();

		grad.SetKeys( new GradientColorKey[] { new GradientColorKey(firstColor, 0.0f), new GradientColorKey(secondColor, 1.0f)}, 
			new GradientAlphaKey[] { new GradientAlphaKey(1.0f,0.0f), new GradientAlphaKey(1.0f,(111.0f/255.0f))});

		lifetime.color = new ParticleSystem.MinMaxGradient(grad);

		return firework;
	}
}
