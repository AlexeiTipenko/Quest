using System;

[Serializable]
public class Giant : Foe {

	public static int frequency = 2;

	public Giant() : base ("Giant", 40) {
		
	}
}
