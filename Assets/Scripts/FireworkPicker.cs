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
	public GameObject retro;
	public GameObject rainbow;
	public GameObject FireworkParent;

	private const string basicText = "basic";

	//firework bundles
	private ArrayList bundles;

	private TwitchParser commands;
	private FireworkLauncher launcher;

	//used for streamer inputs
	bool streamerButton = false;

	//used for auto fireworks
	private float timeSinceLast = 0.0f;

	void Start ()
	{
		commands = this.GetComponent<TwitchParser>();
		commands.recievedParseEvent.AddListener(OnMessage);

		launcher = this.GetComponent<FireworkLauncher>();
		FireworkBundle.FireworkParent = FireworkParent;

		bundles = new ArrayList();
		bundles.Add(new BasicFireworkBundle(basic,basicText));
		bundles.Add(new UnityFireworkBundle(unity,"unity"));
		bundles.Add(new TrailFireworkBundle(trail,"trail"));
		bundles.Add(new SparkleFireworkBundle(sparkle,"sparkle"));
		bundles.Add(new BurstFireworkBundle(burst,"burst"));
		bundles.Add(new BloomFireworkBundle(bloom,"bloom"));
		bundles.Add(new RetroFireworkBundle(retro,"8bit"));
		bundles.Add(new RainbowFireworkBundle(rainbow,"rainbow"));
	}

	void Update()
	{
		float dt = Time.deltaTime;
		int bundleID = -1;

		if(Input.GetAxis("Fire1")>0) { bundleID = 0; }
		else if(Input.GetAxis("Fire2")>0) { bundleID = 1; }
		else if(Input.GetAxis("Fire3")>0) { bundleID = 2; }
		else if(Input.GetAxis("Fire4")>0) { bundleID = 3; }
		else if(Input.GetAxis("Fire5")>0) { bundleID = 4; }
		else if(Input.GetAxis("Fire6")>0) { bundleID = 5; }
		else if(Input.GetAxis("Fire7")>0) { bundleID = 6; }
		else if(Input.GetAxis("Fire8")>0) { bundleID = 7; }
		else { streamerButton = true; }

		if(bundleID >= 0 && streamerButton && bundleID < bundles.Count)
		{
			string type = ((FireworkBundle)bundles[bundleID]).FireworkName;
			UserMessage streamerCommand = new UserMessage("streamer","!fire", new string[] {type,"random","random"});
			OnMessage(streamerCommand);
			streamerButton = false;
		}
		//auto launch fireworks
		timeSinceLast += dt;
		if(timeSinceLast > launcher.secondsPerFirework)
		{
			FireRandomFirework();
			timeSinceLast = 0;
		}
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
					if(cmd.args.Length > 1) { secondColor = ColorPicker.pickColor(cmd.args[1]); }
				}
			}
			else
			{
				//check for the colors in the other argument
				if(cmd.args != null)
				{
					if(cmd.args.Length > 2) { secondColor = ColorPicker.pickColor(cmd.args[2]); }
					if(cmd.args.Length > 1) { firstColor = ColorPicker.pickColor(cmd.args[1]); }
				}
			}

			FireFirework(cmd.arg,firstColor,secondColor);
		}
		else
		{
			//fire a random basic firework
			FireFirework(basicText,ColorPicker.pickColor("random"),ColorPicker.pickColor("random"));
		}
	}

	void FireRandomFirework()
	{
		Color firstColor = ColorPicker.pickColor("random");
		Color secondColor = ColorPicker.pickColor("random");
		string randomBundleName = ((FireworkBundle)bundles[Random.Range(0,bundles.Count)]).FireworkName;

		FireFirework(randomBundleName,firstColor,secondColor);
	}

	void FireFirework(string type, Color firstColor, Color SecondColor)
	{
		FireworkBundle curBundle = (FireworkBundle)bundles[0];
		for(int i = 0; i < bundles.Count; i++)
		{
			if(((FireworkBundle)bundles[i]).FireworkName.Equals(type))
			{
				curBundle = (FireworkBundle)bundles[i];
				break;
			}
		}
		launcher.Launch(curBundle.Generate(firstColor,SecondColor));
	}
}