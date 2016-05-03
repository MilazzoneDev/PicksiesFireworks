using UnityEngine;
using System.Collections;

public class BloomFireworkBundle : FireworkBundle {

	public BloomFireworkBundle(GameObject fireworkObject, string type):base(fireworkObject,type)
	{
	}

	public override GameObject Generate(Color firstColor, Color secondColor)
	{
		bool defaultColor = false;
		if(firstColor == Color.clear) { defaultColor = true; firstColor = Color.white; }
		if(secondColor == Color.clear) { secondColor = ColorPicker.pickColor("ab5323"); }

		//change color
		GameObject firework = getNextAvailable();
		ParticleSystem child = firework.GetComponent<ParticleSystem>().subEmitters.death0;
		ParticleSystem trail = child.GetComponent<ParticleSystem>().subEmitters.birth0;

		ParticleSystem.ColorOverLifetimeModule lifetime = trail.GetComponent<ParticleSystem>().colorOverLifetime;
		Gradient grad = new Gradient();

		if(!defaultColor)
		{
			grad.SetKeys( new GradientColorKey[] { new GradientColorKey(firstColor, 0.0f), new GradientColorKey(secondColor, 1.0f)}, 
				new GradientAlphaKey[] { new GradientAlphaKey(1.0f,0.0f), new GradientAlphaKey(1.0f,1.0f)});
		}
		else
		{
			grad.SetKeys( new GradientColorKey[] { new GradientColorKey(firstColor, 0.0f), new GradientColorKey(ColorPicker.pickColor("ffd974"),0.224f), new GradientColorKey(ColorPicker.pickColor("ee8038"),0.519f), new GradientColorKey(secondColor, 1.0f)}, 
				new GradientAlphaKey[] { new GradientAlphaKey(1.0f,0.0f), new GradientAlphaKey(1.0f,1.0f)});
		}

		lifetime.color = new ParticleSystem.MinMaxGradient(grad);

		return firework;
	}
}
