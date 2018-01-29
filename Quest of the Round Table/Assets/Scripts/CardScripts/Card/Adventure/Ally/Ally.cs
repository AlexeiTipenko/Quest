using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : Adventure {
  protected int battlePoints, bonusBattlePoints, bidPoints, bonusBidPoints;

  public getBattlePoints() {
    return battlePoints;
  }

  public getBidPoints() {
    return bidPoints;
  }

}
