//made with help from https://github.com/Grahnz/TwitchIRC-Unity

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Random=UnityEngine.Random;

[RequireComponent(typeof(TwitchIRCReader))]
public class TwitchParser : MonoBehaviour 
{
	private TwitchIRCReader IRC;
	private char[] splitters = {' ',':'};

	[Header("Joining Channels")]
	//used for joining channels
	public GameObject inputField;

	public Text joinedText;
	public Image joinedImage;
	private string newJoinedText;
	private bool titleChanged;

	[Header("Chat Window")]
	//used for chat window
	public int maxMessages = 50;
	private int numMessages = 0;
	public ScrollRect chatRect;
	public Text chatText;

	//default titles for chat
	private string noChannelText = "No Channel";
	private string notConnectedText = "Channel Not Connected";

	[Header("Bot Listening")]
	//bot listener properties
	public InputField BotInput;
	public Toggle BotToggle;

	//private 
	private TwitchIRCReader joined;

	//used to send messages to the game
	public class ParsedMsgEvent : UnityEngine.Events.UnityEvent<UserMessage>{ }
	public ParsedMsgEvent recievedParseEvent = new ParsedMsgEvent();

	// Use this for initialization
	void Start () 
	{
		IRC = this.GetComponent<TwitchIRCReader>();
		IRC.messageRecievedEvent.AddListener(OnMessageRecieved);

		joined = this.GetComponent<TwitchIRCReader>();
		joined.JoinedChannelEvent.AddListener(JoinedChannel);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(titleChanged)
		{
			if(joinedText != null)
			{
				joinedText.text = newJoinedText;
				titleChanged = false;
			}
			if(joinedImage != null)
			{
				if(newJoinedText.Equals(noChannelText) || newJoinedText.Equals(notConnectedText))
				{
					joinedImage.enabled = false;
				}
				else
				{
					joinedImage.enabled = true;
				}
			}
		}

	}

	void JoinChannel ()
	{
		string channel = inputField.GetComponent<InputField>().text;
		this.GetComponent<TwitchIRCReader>().SendJoin(channel);
		newJoinedText = notConnectedText;
		if(channel.Equals(""))
		{
			newJoinedText = noChannelText;
		}
		titleChanged = true;
		ClearChat();
	}

	void JoinedChannel (string channel)
	{
		Debug.Log("Joined a channel: " + channel);
		newJoinedText = channel + "'s chat";
		titleChanged = true;
	}
	
	void OnMessageRecieved(string msg)
	{
		UserMessage newMessage = Parse (msg);

		if(newMessage != null)
		{
			//is this a user we are listening for?
			if(!BotToggle.isOn||(BotToggle.isOn && newMessage.user.Equals(BotInput.text.ToLower())))
			{
				recievedParseEvent.Invoke(newMessage);
			}
		}

		UpdateChat(msg);

	}

	void UpdateChat(string msg)
	{
		if(msg.Contains("PRIVMSG #"))
		{
			int msgStart = msg.IndexOf("PRIVMSG #");
			//removes the "Privmsg # channelname :" from the message
			string message = msg.Substring(msgStart + IRC.channelName.Length + 11);
			string user = msg.Substring(1, msg.IndexOf('!')-1);

			if(numMessages >= maxMessages)
			{
				int secondMessageStart = chatText.text.IndexOf("\n\n") + 2;
				chatText.text = chatText.text.Substring(secondMessageStart);
				numMessages--;
			}

			Random.seed = user.Length + (int)user[0] + (int)user[user.Length - 1];
			Color32 c = new Color(Random.Range(0.25f,0.65f),Random.Range(0.25f,0.65f),Random.Range(0.25f,0.65f));
			string nameColor = "#" + c.r.ToString("X2")+c.g.ToString("X2")+c.b.ToString("X2");

			if(message.StartsWith("\u0001ACTION"))
			{
				message = message.Substring(8);
				chatText.text += "<color="+nameColor+"><b>"+user + " " + message + "</b></color>\n\n";
			}
			else
			{
				chatText.text += "<color="+nameColor+"><b>"+user+"</b></color>"+": " + message + "\n\n";
			}
			numMessages++;
			Random.seed = System.Environment.TickCount;
		}
	}

	void ClearChat()
	{
		chatText.text = "";
		numMessages = 0;
	}

	//parses a message (returns null if not a command)
	UserMessage Parse(string msg)
	{
		var separatedMessage = msg.Split(splitters);
		var nameSeparated = separatedMessage[1].Split('!')[0];
		UserMessage pMsg;
		if(separatedMessage[5].StartsWith("!"))
		{
			if(separatedMessage.Length > 6)
			{
				if(separatedMessage.Length > 7)
				{
					string[] arguments = new string[separatedMessage.Length-6];
					Array.Copy(separatedMessage,6,arguments,0,separatedMessage.Length-6);
					pMsg = new UserMessage(nameSeparated,separatedMessage[5],arguments);
				}
				else
				{
					pMsg = new UserMessage(nameSeparated,separatedMessage[5],separatedMessage[6]);
				}
			}
			else
			{
				pMsg = new UserMessage(nameSeparated,separatedMessage[5]);
			}

			return pMsg;
		}

		return null;
	}


}
