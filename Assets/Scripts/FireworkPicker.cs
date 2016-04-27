using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FireworkLauncher))]
[RequireComponent (typeof(TwitchParser))]
public class FireworkPicker : MonoBehaviour {

	[Header("Firework Prefabs")]
	public GameObject basic;
	public GameObject unity;
	public GameObject trail;
	public GameObject sparkle;
	public GameObject burst;
	public GameObject bloom;

	private const string basicText = "basic";
	private const string unityText = "unity";
	private const string trailText = "trail";
	private const string sparkleText = "sparkle";
	private const string burstText = "burst";
	private const string bloomText = "bloom";

	//firework bundles
	private ArrayList basicBundle;
	private ArrayList unityBundle;
	private ArrayList trailBundle;
	private ArrayList sparkleBundle;
	private ArrayList burstBundle;
	private ArrayList bloomBundle;

	private TwitchParser commands;
	private FireworkLauncher launcher;

	void Start ()
	{
		commands = this.GetComponent<TwitchParser>();
		launcher = this.GetComponent<FireworkLauncher>();

		commands.recievedParseEvent.AddListener(OnMessage);

		basicBundle = new ArrayList();
		unityBundle = new ArrayList();
		trailBundle = new ArrayList();
		sparkleBundle = new ArrayList();
		burstBundle = new ArrayList();
		bloomBundle = new ArrayList();
	}

	void OnMessage(UserMessage cmd)
	{
		if(cmd.command.Equals("!fire")||cmd.command.Equals("!Fire"))
		{
			Fire(cmd);
		}
	}

	void Fire(UserMessage cmd)
	{
		if(cmd.arg != null)
		{
			//find colors if entered
			Color newColor = ColorPicker.pickColor(cmd.arg);
			Color firstColor = Color.clear;
			Color secondColor = Color.clear;

			if(newColor != Color.clear)
			{
				firstColor = newColor;
				if(cmd.args != null)
				{
					if(cmd.args.Length > 1)
					{
						secondColor = ColorPicker.pickColor(cmd.args[1]);
					}
				}
			}
			else
			{
				//check for the colors in the other argument
				if(cmd.args != null)
				{
					if(cmd.args.Length > 2)
					{
						secondColor = ColorPicker.pickColor(cmd.args[2]);
					}
					if(cmd.args.Length > 1)
					{
						firstColor = ColorPicker.pickColor(cmd.args[1]);
					}
				}
			}

			GameObject firework;
			switch(cmd.arg)
			{
				case unityText:
					firework = GetFirework(unityText);
					break;
				case trailText:
					firework = GenerateTrailFirework(firstColor, secondColor);
					break;
				case sparkleText:
					firework = GenerateSparkleFirework(firstColor,secondColor);
					break;
				case burstText:
					firework = GenerateBurstFirework(firstColor,secondColor);
					break;
				case bloomText:
					firework = GenerateBloomFirework(firstColor,secondColor);
					break;
				default:
					firework = GenerateBasicFirework(firstColor,secondColor);
					break;
			}
			launcher.Launch(firework);
		}
		else
		{
			//fire a random basic firework
			launcher.Launch(GenerateBasicFirework(ColorPicker.pickColor("random"),ColorPicker.pickColor("random")));
		}
	}

	GameObject GenerateTrailFirework(Color firstColor, Color secondColor)
	{
		bool defaultColor = false;
		if(firstColor == Color.clear) { defaultColor = true; firstColor = Color.white; }
		if(secondColor == Color.clear) { secondColor = ColorPicker.pickColor("ab5323"); }

		GameObject firework = GetFirework(trailText);
		//change color
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

	GameObject GenerateSparkleFirework(Color firstColor, Color secondColor)
	{
		if(firstColor == Color.clear){ firstColor = Color.white;}
		if(secondColor == Color.clear) { secondColor = Color.white;}

		GameObject firework = GetFirework(sparkleText);
		//change color
		ParticleSystem child = firework.GetComponent<ParticleSystem>().subEmitters.death1;
		ParticleSystem child2 = child.GetComponent<ParticleSystem>().subEmitters.death0;

		ParticleSystem.ColorOverLifetimeModule lifetime = child2.GetComponent<ParticleSystem>().colorOverLifetime;
		Gradient grad = new Gradient();

		grad.SetKeys( new GradientColorKey[] { new GradientColorKey(firstColor, 0.0f), new GradientColorKey(secondColor, 1.0f)}, 
			new GradientAlphaKey[] { new GradientAlphaKey(1.0f,0.0f), new GradientAlphaKey(1.0f,(111.0f/255.0f))});

		lifetime.color = new ParticleSystem.MinMaxGradient(grad);

		return firework;

	}

	GameObject GenerateBurstFirework(Color firstColor, Color secondColor)
	{
		if(firstColor == Color.clear){ firstColor = Color.white;}
		if(secondColor == Color.clear) { secondColor = Color.white;}

		GameObject firework = GetFirework(burstText);

		//change color
		ParticleSystem smoke = firework.GetComponent<ParticleSystem>().subEmitters.death0;
		ParticleSystem stage1 = firework.GetComponent<ParticleSystem>().subEmitters.death1;
		ParticleSystem stage2 = smoke.subEmitters.birth0;

		stage1.startColor = firstColor;
		stage2.startColor = secondColor;

		return firework;
	}

	GameObject GenerateBloomFirework(Color firstColor, Color secondColor)
	{
		bool defaultColor = false;
		if(firstColor == Color.clear) { defaultColor = true; firstColor = Color.white; }
		if(secondColor == Color.clear) { secondColor = ColorPicker.pickColor("ab5323"); }

		GameObject firework = GetFirework(bloomText);
		//change color
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

	//generates a basic firework
	GameObject GenerateBasicFirework(Color firstColor, Color secondColor)
	{
		if(firstColor == Color.clear) { firstColor = Color.red; }
		if(secondColor == Color.clear) { secondColor = Color.white; }

		//basic firework
		//fire a firework with color
		GameObject firework = GetFirework(basicText);
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

	GameObject GetFirework(string type)
	{
		if(type.Equals(basicText))
		{
			return (GameObject)basicBundle[getNextAvailable(basicBundle, type)];
		}
		else if(type.Equals(unityText))
		{
			return (GameObject)unityBundle[getNextAvailable(unityBundle, type)];
		}
		else if(type.Equals(trailText))
		{
			return (GameObject)trailBundle[getNextAvailable(trailBundle, type)];
		}
		else if(type.Equals(sparkleText))
		{
			return (GameObject)sparkleBundle[getNextAvailable(sparkleBundle, type)];
		}
		else if(type.Equals(burstText))
		{
			return (GameObject)burstBundle[getNextAvailable(burstBundle, type)];
		}
		else if(type.Equals(bloomText))
		{
			return (GameObject)bloomBundle[getNextAvailable(bloomBundle, type)];
		}
		else
		{
			return null;
		}
	}

	int getNextAvailable(ArrayList bundle, string type)
	{
		for(int i=0; i<bundle.Count;i++)
		{
			GameObject emitter = (GameObject)bundle[i];
			if(!emitter.GetComponent<ParticleSystem>().IsAlive(true))
			{
				return i;
			}
		}
		//no available emitters were found
		bundle.Add(createNewEmitter(type));
		return bundle.Count-1;
	}

	GameObject createNewEmitter(string type)
	{
		GameObject newFirework;

		if(type.Equals(unityText))
		{
			newFirework = Instantiate(unity);
		}
		else if(type.Equals(trailText))
		{
			newFirework = Instantiate(trail);
		}
		else if(type.Equals(sparkleText))
		{
			newFirework = Instantiate(sparkle);
		}
		else if(type.Equals(burstText))
		{
			newFirework = Instantiate(burst);
		}
		else if(type.Equals(bloomText))
		{
			newFirework = Instantiate(bloom);
		}
		else
		{
			//else return a basic
			newFirework = Instantiate(basic);
		}
		return newFirework;
	}
}
