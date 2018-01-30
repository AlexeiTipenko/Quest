using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : Story {
  private int stagesNum; //TODO; shouldn't be protected?
  private enum dominantFoe {};
  private List<Stage> stagestList;

  public int getNumShieldsWon() {
    //TODO; return stagesNum + total foes/tests played
    return 0;
  }
}
