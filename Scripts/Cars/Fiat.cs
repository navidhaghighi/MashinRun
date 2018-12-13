using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fiat : Car {

	#region monobehaviour_events
	// Use this for initialization
	void Awake () {
		virtualGood = MashinRunAssets.fiatCar;
	}
	#endregion
}
