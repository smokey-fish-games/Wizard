using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.UI;

public class DeveloperConsole : MonoBehaviour
{
    public static DeveloperConsole instance;

    SortedDictionary<string, Command> commandList = new SortedDictionary<string, Command>();

    public Image devconsole;
    public InputField devconsoleIn;
    public TextMeshProUGUI textBox;

    static string COMMAND_TEXT_COLOR = "#3498db";
    static string WARN_TEXT_COLOR    = "#f39c12";
    static string ERROR_TEXT_COLOR   = "#e74c3c";
    static string INFO_TEXT_COLOR    = "#ecf0f1";

    public enum LEVEL { INFO, ERROR, WARNING, COMMAND};

    bool showingConsole = false;

    public int MAX_MESSAGES = 20;
    int currentMessages = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            // setup text box etc
            clear(new string[0]);
            closeConsole(new string[0]);
            RegisterCommand("close", "Closes the developer console.", closeConsole);
            RegisterCommand("clear", "clears the developer console window.", clear);
            RegisterCommand("help", " (command) Displays help for a particular command or lists all commands available.", help);
        }
        else
        {
            Debug.LogError("Error: Two developer console instances have started!");
        }
    }

    bool closeConsole(string[] noop)
    {
        clear(new string[0]);
        showingConsole = false;
        devconsoleIn.DeactivateInputField();
        devconsole.enabled = false;
        for (int i = 0; i < devconsole.transform.childCount; i++)
        {
            devconsole.transform.GetChild(i).gameObject.SetActive(false);
        }

        return true;
    }

    bool showConsole(string[] noop)
    {
        devconsole.enabled = true;
        for (int i = 0; i < devconsole.transform.childCount; i++)
        {
            devconsole.transform.GetChild(i).gameObject.SetActive(true);
        }
        devconsoleIn.Select();
        devconsoleIn.ActivateInputField();

        return true;
    }

    bool clear(string[] noops)
    {
        textBox.text = "";
        currentMessages = 0;
        return true;
    }

    bool help(string[] noop)
    {
        if(noop == null)
        {
            printHelp("");
            return true;
        }
        switch(noop.Length)
        {
            case 0:
                printHelp("");
                return true;

            case 1:
                string com = noop[0].Trim();
                if (com == "")
                {
                    printHelp("");
                }
                else
                {
                    printHelp(com);
                }
                return true;

            default:
                return false;
        }        
    }

    private void Update()
    {
        if(Input.GetKeyDown("tab"))
        {
            if(showingConsole)
            {
                closeConsole(new string[0]);
            }
            else 
            {
                showConsole(new string[0]);
            }
        }
    }

    public static string[] parseArgs(string fullString)
    {
        fullString = fullString.Trim();
        string[] starting = fullString.Split(' ');

        string[] presend = new string[starting.Length];
        bool foundbad = false;
        int presendi = 0;
        for (int i = 0; i < starting.Length; i++)
        {
            if(starting[i].Trim() != "")
            {
                presend[presendi] = starting[i].Trim();
                presendi++;
            }
            else
            {
                foundbad = true;
            }
        }
        if (foundbad)
        {
            string[] toreturnnew = new string[presendi];
            Array.Copy(presend, 0, toreturnnew, 0, toreturnnew.Length);

            return toreturnnew;
        }
        else
        {
            return presend;
        }
    }

    
    public void DevConsoleRunCommand(string wholestring)
    {
        if (wholestring.Trim() != "")
        {
            String[] parsed = parseArgs(wholestring);
            string command = parsed[0];
            string[] parms = new string[parsed.Length - 1];
            Array.Copy(parsed, 1, parms, 0, parms.Length);

            writeMessage("> " + wholestring, LEVEL.COMMAND);

            runCommand(command, parms);
        }
        devconsoleIn.text = "";
        devconsoleIn.Select();
        devconsoleIn.ActivateInputField();
    }

    public bool RegisterCommand(string command, string helptext, Func<string[], bool> call)
    {
        command = command.Trim();
        if (command == null || command == "")
        {
            Debug.LogError("Command to add cannot be empty '" + command + "'!");
            return false;
        }

        if(call == null)
        {
            Debug.LogError("Command "+command+"cannot have a empty function!");
            return false;
        }
        
        if(commandList.ContainsKey(command))
        {
            Debug.LogError("Command " + command +" already registered!");
            return false;
        }
        Command toAdd = new Command(command, helptext, call);
        commandList.Add(command, toAdd);

        //sort the list!
        

        return true;
    }

    public bool RemoveCommand(string command)
    {
        command = command.Trim();
        if (command == null || command == "")
        {
            Debug.LogError("Command to remove cannot be empty '" + command + "'!");
            return false;
        }

        if (commandList.ContainsKey(command))
        {
            Debug.LogError("Command " + command + " is not registered!");
            return false;
        }

        return commandList.Remove(command); ;
    }

    public void runCommand(string command, string[] parameters)
    {
        bool output = true;
        if(!commandList.ContainsKey(command))
        {
            writeError("Unknown command " + command);
            output = false;
        }
        else
        {
            output = commandList[command].method(parameters);
            if(!output)
            {
                printHelp(command);
            }
        }
    }

    public void writeMessage(string message)
    {
        writeMessage(message, LEVEL.INFO);
    }

    public void writeError(string message)
    {
        writeMessage(message, LEVEL.ERROR);
    }

    public void writeWarning(string message)
    {
        writeMessage(message, LEVEL.WARNING);
    }

    public void writeMessage(string message, LEVEL l)
    {
        string curText = textBox.text;
        if (currentMessages >= MAX_MESSAGES)
        {
            // Need to delete a line at start
            curText = curText.Substring(curText.IndexOf('\n', 1));
        }
        else
        {
            currentMessages++;
        }
        switch (l)
        {
            case LEVEL.ERROR:
                curText += "<color=" + ERROR_TEXT_COLOR + ">" + message + "</color>";
                break;
            
            case LEVEL.WARNING:
                curText += "<color=" + WARN_TEXT_COLOR + ">" + message + "</color>";
                break;

            case LEVEL.COMMAND:
                curText += "<color="+ COMMAND_TEXT_COLOR + ">" + message + "</color>";
                break;

            case LEVEL.INFO:
            default:
                curText += "<color=" + INFO_TEXT_COLOR + ">" + message + "</color>";
                break;
        }
        textBox.text = curText + "\n";
    }

    public void printHelp(string command)
    {
        if(command == "")
        {
            writeMessage("Available Commands () = optional parameter [] = required parameter | = choose one parameter");
            List<string> commands = commandList.Keys.ToList();
            for (int i = 0; i < commands.Count; i++)
            {
                string toPrint = " - " + commandList[commands[i]].command + ": " + commandList[commands[i]].helptext;
                writeMessage(toPrint);
            }
        }
        else
        {
            // Specific help
            if (commandList.ContainsKey(command))
            {
                writeMessage(commandList[command].command + ": " + commandList[command].helptext);
            }
            else
            {
                writeError("Unknown command " + command);
            }
        }
    }
}

public class Command
{
    public string command;
    public string helptext;
    public Func<string[], bool> method;

    public Command(string com, string help, Func<string[], bool> met)
    {
        command = com;
        helptext = help;
        method = met;
    }
}