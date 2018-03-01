public class AIPlayer : Player {
    
    readonly AbstractAI strategy;

    public AIPlayer(string name, AbstractAI strategy) : base(name) {
        this.strategy = strategy;
        strategy.SetStrategyOwner(this);
    }

    public AbstractAI GetStrategy() {
        return strategy;
    }

}
