using MAEngine;

public class ForgingModule : BasicModule
{
    public void StartModule()
    {

    }

    public override void Initialise()
    {
        InitializeFields();
        InitializeForgingActions();
    }

    private void InitializeFields()
    {
        _actions = new Actions();
    }

    private void InitializeForgingActions()
    {
        ForgingActions forgingActions = _di.Resolve<ForgingActions>();
        _actions.Add(forgingActions);
    }
}
