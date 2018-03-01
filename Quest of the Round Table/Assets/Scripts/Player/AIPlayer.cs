using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : Player {
    
    readonly Strategy strategy;

    public AIPlayer(string name, Strategy strategy) : base(name) {
        this.strategy = strategy;
        strategy.SetStrategyOwner(this);
    }

    public Strategy GetStrategy() {
        return strategy;
    }

}
