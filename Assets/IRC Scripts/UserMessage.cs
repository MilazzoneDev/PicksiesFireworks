using UnityEngine;
using System.Collections;

public class UserMessage {

	public string user;
	public string command;
	public string arg;
	public string[] args;

	public UserMessage(string userName, string commandSent)
	{
		user = userName;
		command = commandSent;
	}

	public UserMessage(string userName, string commandSent, string arg1)
	{
		user = userName;
		command = commandSent;
		arg = arg1;
		args = new string[] {arg1};
	}

	public UserMessage(string userName, string commandSent, string[] arguments)
	{
		user = userName;
		command = commandSent;
		arg = arguments[0];
		args = new string[arguments.Length];
		args = (string[])arguments.Clone();
	}

}
