#region namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion
public class Beetle : Car  {
	#region monobehaviour_events
	// Use this for initialization
	void Awake () {
		virtualGood = MashinRunAssets.beetleCar;
	}
	#endregion
}
