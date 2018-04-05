using System;
using System.IO;
using UnityEngine;

public class Logger {

	public static Logger logger;
	private static string logFolderPath = null;
	private static string logFilePath = null;

	//This constructor will call the init function
	//Should only be called once in your code
	public Logger() {
		this.init();
	}

	public static Logger getInstance() {
		if (logger == null) {
			logger = new Logger();
			if (Application.platform != RuntimePlatform.WebGLPlayer) {
				if (logFolderPath == null) {
					logFolderPath = Directory.GetCurrentDirectory() + "/Logs";
					logFilePath = logFolderPath + "/BuildLog.txt"; 
				}
				if (Debug.isDebugBuild) {
					try {
						File.Delete(logFilePath); //delete previous log file
					} catch (Exception) {
						Directory.CreateDirectory(logFolderPath);
						File.Delete(logFilePath); //delete previous log file
					}
				}
				logger.init();
			}
		}
		return logger;
	}

	//Can be called as many time as you want in your code (as it will still construct the logger but won't call the init function
	//public Logger(bool b) {} //This constructor won't call the init function

	public void logCustom(string n, string type) {
		printToFile(generateTimestamp() + " [" + type.ToUpper() + "]: " + n + "\n");
	}

	//Designates informational messages that highlight the progress of the application at noticeable level.
	public void info(string n) {
		printToFile(generateTimestamp() + " [INFO]: " + n + "\n");
	}

	//Designates fine-grained informational events that are most useful to debug an application.
	public void debug(string n) {
		printToFile(generateTimestamp() + " [DEBUG]: " + n + "\n");
	}

	//Designates potentially harmful situations.
	public void warn(string n) {
		printToFile(generateTimestamp() + " [WARN]: " + n + "\n");
	}

	//Designates error events that might still allow the application to continue running.
	public void error(string n) {
		printToFile(generateTimestamp() + " [ERROR]: " + n + "\n");
	}

	//Designates finer-grained informational events than the DEBUG.
	public void trace(string n) {
		printToFile(generateTimestamp() + " [TRACE]: " + n + "\n");
	}

	//TEST case logging
	public void test(string n) {
		printToFile(generateTimestamp() + " [TEST]: " + n + "\n");
	}

	private void init() {
		printToFile("-------------------- INITIALIZE LOGGER ---------------------\n");
		printToFile(generateTimestamp() + ": Logger initialized\n");
	}

	private void printToFile(string n) {
		if (Application.platform != RuntimePlatform.WebGLPlayer) {
			if (logFolderPath == null) {
				logFolderPath = Directory.GetCurrentDirectory() + "/Logs";
				logFilePath = logFolderPath + "/BuildLog.txt"; 
			}
			try {
				File.AppendAllText (logFilePath, n);
			} catch (Exception) {
				Directory.CreateDirectory (logFolderPath);
				File.AppendAllText (logFilePath, n);
			}
		}
	}

	private string generateTimestamp() {
		return DateTime.Now.ToString("O");
	}
}