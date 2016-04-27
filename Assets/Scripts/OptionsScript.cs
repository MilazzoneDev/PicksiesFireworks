using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionsScript : MonoBehaviour {

	[Header("Cam Positions")]
	public Vector3 chatPos;
	public Vector3 withoutChatPos;
	public Vector3 viewPos;
	public Vector3 UpPos;
	[Header("Cam Rotations")]
	public Vector3 normalRot;
	public Vector3 upRot;

	[Header("screens")]
	public GameObject optionsMenu;
	public GameObject chatWindow;
	public GameObject InstructionsStart;
	public GameObject InstructionsPlay;
	public GameObject InstructionsBot;

	[Header("Settings")]
	public Toggle chatToggle;
	public Toggle optionsButtonToggle;
	public Image chromaImage;
	public InputField chromaInput;

	[Header("objects")]
	public GameObject optionsButton;
	public GameObject launcher;
	public Camera cam;

	private bool optionsButtonActive;
	private bool chatActive;


	// Use this for initialization
	void Start () {
		CloseMenu(); //in case it's already open
		HideInstructions();

		ShowMenu();
		ToggleChat();
		ToggleOptions();
		cam.transform.position = chatPos;
		cam.transform.rotation = Quaternion.Euler(normalRot);
		cam.clearFlags = CameraClearFlags.Color;
		cam.backgroundColor = Color.black;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetAxis("ChatCam")>0)
		{
			if(chatActive)
			{
				cam.transform.position = chatPos;
			}
			else
			{
				cam.transform.position = withoutChatPos;
			}

			cam.transform.rotation = Quaternion.Euler(normalRot);
			SetChat(true);
			ViewLauncher(true);
			cam.clearFlags = CameraClearFlags.Color;
			cam.backgroundColor = Color.black;

		}
		else if(Input.GetAxis("ViewCam")>0)
		{
			cam.transform.position = viewPos;
			cam.transform.rotation = Quaternion.Euler(normalRot);
			SetChat(false);
			ViewLauncher(true);
			cam.clearFlags = CameraClearFlags.Color;
			cam.backgroundColor = Color.black;
		}
		else if(Input.GetAxis("ChromaCam")>0)
		{
			cam.transform.position = viewPos;
			cam.transform.rotation = Quaternion.Euler(normalRot);
			SetChat(false);
			ViewLauncher(false);
			cam.clearFlags = CameraClearFlags.Color;
			cam.backgroundColor = Color.black;
		}
		else if(Input.GetAxis("UpCam")>0)
		{
			cam.transform.position = UpPos;
			cam.transform.rotation = Quaternion.Euler(upRot);
			SetChat(false);
			ViewLauncher(false);
			cam.clearFlags = CameraClearFlags.Color;
			cam.backgroundColor = Color.black;
		}

		if(Input.GetAxis("OpenMenu")>0)
		{
			ShowMenu();
		}
	}

	void ShowMenu()
	{
		HideChat();
		HideOptions();
		ShowLauncher (false);

		//do last to make sure options and chat are hidden
		if(optionsMenu != null)
		{
			optionsMenu.SetActive(true);
		}
	}

	void CloseMenu()
	{
		if(optionsMenu != null)
		{
			optionsMenu.SetActive(false);
		}
		SetChat(chatActive);
		SetOptionsButton(optionsButtonActive);
		ShowLauncher (true);
	}

	//
	//variable menu toggles
	//

	void SetOptionsButton(bool setTo)
	{
		if(optionsButton != null && optionsButtonToggle != null)
		{
			if(!optionsMenu.activeSelf)
			{
				optionsButton.SetActive(setTo);
				optionsButtonToggle.isOn = setTo;
			}
			if(optionsMenu.activeSelf)
			{
				optionsButtonToggle.isOn = setTo;
			}
		}
	}

	void ToggleOptions()
	{
		SetOptionsButton(!optionsButtonActive);
	}

	void ChangeOptions()
	{
		if(optionsButton != null)
		{
			optionsButtonActive = optionsButtonToggle.isOn;
		}
	}

	void HideOptions()
	{
		if(optionsButton != null)
		{
			optionsButton.SetActive(false);
		}
	}

	void SetChat(bool setActive)
	{
		if(chatWindow != null && chatToggle != null)
		{
			if(!optionsMenu.activeSelf)
			{
				chatWindow.SetActive(setActive);
				chatToggle.isOn = setActive;
			}
			if(optionsMenu.activeSelf)
			{
				chatToggle.isOn = setActive;
			}
		}
	}

	void ToggleChat()
	{
		SetChat(!chatActive);
	}

	void ChangeChat()
	{
		if(chatWindow != null)
		{
			chatActive = chatToggle.isOn;
		}

		if(cam.transform.position == chatPos)
		{
			if(!chatActive)
			{
				cam.transform.position = withoutChatPos;
			}
		}
		else if(cam.transform.position == withoutChatPos)
		{
			if(chatActive)
			{
				cam.transform.position = chatPos;
			}
		}
	}

	void HideChat()
	{
		if(chatWindow != null)
		{
			chatWindow.SetActive(false);
		}
	}

	void ViewLauncher(bool setAcitve)
	{
		if(launcher != null)
		{
			launcher.GetComponent<MeshRenderer>().enabled = setAcitve;
		}
	}

	void ShowLauncher(bool setActive)
	{
		if (launcher != null) 
		{
			launcher.SetActive (setActive);
		}
	}

	//
	//instruction screens
	//

	void HideMenu(bool shouldHide)
	{
		if(optionsMenu != null)
		{
			optionsMenu.SetActive(!shouldHide);
		}
	}

	void ViewStart()
	{
		HideMenu (true);
		if (InstructionsStart != null) 
		{
			InstructionsStart.SetActive (true);
		}
	}

	void ViewPlay()
	{
		HideMenu (true);
		if (InstructionsPlay != null) 
		{
			InstructionsPlay.SetActive (true);
		}
	}

	void ViewBot()
	{
		HideMenu (true);
		if (InstructionsBot != null) 
		{
			InstructionsBot.SetActive (true);
		}
	}

	void HideInstructionsFromOptions()
	{
		HideInstructions ();
		HideMenu (false);
	}

	void HideInstructions()
	{
		if (InstructionsStart != null) 
		{
			InstructionsStart.SetActive (false);
		}
		if (InstructionsPlay != null) 
		{
			InstructionsPlay.SetActive (false);
		}
		if (InstructionsBot != null) 
		{
			InstructionsBot.SetActive (false);
		}
	}

	void ChangeChroma()
	{
		Color newColor = Color.clear;
		if(chromaInput.text != "")
		{
			newColor = ColorPicker.pickColor(chromaInput.text);

		}
		if(newColor == Color.clear)
		{
			newColor = Color.black;
		}

		chromaImage.color = newColor;
		cam.backgroundColor = newColor;

	}

}
