using MAEngine;
using System;

public class ForgingModule : BasicModule
{
    public void StartModule()
    {

    }

    public override void Initialise()
    {
        InitializeFields();
        InitializeForgingActions();
        InitializeResultsActions();
        InitializeSmeltingActions();
    }

    private void InitializeFields()
    {
        _actions = new Actions();
    }

    private void InitializeForgingActions()
    {
        ForgingActions forgingActions =
            _di.Resolve<ForgingActions>();
        _actions.Add(forgingActions);
    }

    private void InitializeResultsActions()
    {
        ResultsActions resultsActions =
            _di.Resolve<ResultsActions>();
        _actions.Add(resultsActions);
    }
    
    private void InitializeSmeltingActions()
    {
        SmeltingActions smeltingActions =
            _di.Resolve<SmeltingActions>();
        _actions.Add(smeltingActions);
    }
}
