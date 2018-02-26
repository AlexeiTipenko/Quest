using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchForTheQuestingBeast : Quest {

	public static int frequency = 1;

	public SearchForTheQuestingBeast() : base ("Search for the Questing Beast", 4) {
		Logger.getInstance ().info ("Initializing the Search for the Questing Beast card");
	}

}
