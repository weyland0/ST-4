using Stateless;

namespace BugPro;

public enum BugState
{
    NewDefect,
    DefectTriage,
    Fixing,
    NotReproducible,
    DeferredNoTime,
    NeedsSeparateDecision,
    OtherProductProblem,
    NeedMoreInfo,
    Returned,
    Closed,
    Reopened
}

public enum BugTrigger
{
    SubmitToTriage,
    StartFixing,
    CloseAsNotABug,
    CloseAsWontFix,
    CloseAsDuplicate,
    MarkNotReproducible,
    DeferNoTime,
    RequireSeparateDecision,
    MarkOtherProduct,
    RequestMoreInfo,
    ReturnToTriage,
    ProblemSolved,
    ReturnAfterFailedFix,
    ConfirmNotReproducibleOk,
    RejectNotReproducible,
    CompleteReturnToTriage,
    Reopen,
    EnterTriageAfterReopen
}

public sealed class Bug
{
    private BugState _state;
    private readonly StateMachine<BugState, BugTrigger> _machine;

    public Bug(BugState initialState = BugState.NewDefect)
    {
        _state = initialState;
        _machine = new StateMachine<BugState, BugTrigger>(() => _state, s => _state = s);

        _machine.Configure(BugState.NewDefect)
            .Permit(BugTrigger.SubmitToTriage, BugState.DefectTriage);

        _machine.Configure(BugState.DefectTriage)
            .Permit(BugTrigger.StartFixing, BugState.Fixing)
            .Permit(BugTrigger.CloseAsNotABug, BugState.Closed)
            .Permit(BugTrigger.CloseAsWontFix, BugState.Closed)
            .Permit(BugTrigger.CloseAsDuplicate, BugState.Closed)
            .Permit(BugTrigger.MarkNotReproducible, BugState.NotReproducible)
            .Permit(BugTrigger.DeferNoTime, BugState.DeferredNoTime)
            .Permit(BugTrigger.RequireSeparateDecision, BugState.NeedsSeparateDecision)
            .Permit(BugTrigger.MarkOtherProduct, BugState.OtherProductProblem)
            .Permit(BugTrigger.RequestMoreInfo, BugState.NeedMoreInfo);

        void ConfigureParkingLot(BugState parked)
        {
            _machine.Configure(parked)
                .Permit(BugTrigger.ReturnToTriage, BugState.DefectTriage)
                .Permit(BugTrigger.StartFixing, BugState.Fixing);
        }

        ConfigureParkingLot(BugState.DeferredNoTime);
        ConfigureParkingLot(BugState.NeedsSeparateDecision);
        ConfigureParkingLot(BugState.OtherProductProblem);
        ConfigureParkingLot(BugState.NeedMoreInfo);

        _machine.Configure(BugState.Fixing)
            .Permit(BugTrigger.MarkNotReproducible, BugState.NotReproducible)
            .Permit(BugTrigger.RequestMoreInfo, BugState.NeedMoreInfo)
            .Permit(BugTrigger.ProblemSolved, BugState.Closed)
            .Permit(BugTrigger.ReturnAfterFailedFix, BugState.Returned);

        _machine.Configure(BugState.NotReproducible)
            .Permit(BugTrigger.ConfirmNotReproducibleOk, BugState.Closed)
            .Permit(BugTrigger.RejectNotReproducible, BugState.Returned);

        _machine.Configure(BugState.Returned)
            .Permit(BugTrigger.CompleteReturnToTriage, BugState.DefectTriage);

        _machine.Configure(BugState.Closed)
            .Permit(BugTrigger.Reopen, BugState.Reopened);

        _machine.Configure(BugState.Reopened)
            .Permit(BugTrigger.EnterTriageAfterReopen, BugState.DefectTriage);
    }

    public BugState State => _machine.State;

    public bool CanFire(BugTrigger trigger) => _machine.CanFire(trigger);

    public void Fire(BugTrigger trigger) => _machine.Fire(trigger);

    public static string Describe(BugState state) => state switch
    {
        BugState.NewDefect => "НОВЫЙ ДЕФЕКТ",
        BugState.DefectTriage => "РАЗБОР ДЕФЕКТОВ",
        BugState.Fixing => "ИСПРАВЛЕНИЕ",
        BugState.NotReproducible => "НЕ ВОСПРОИЗВОДИМО",
        BugState.DeferredNoTime => "НЕТ ВРЕМЕНИ \u0421\u0415\u0419\u0427\u0410\u0421",
        BugState.NeedsSeparateDecision => "НУЖНО ОТДЕЛЬНОЕ РЕШЕНИЕ",
        BugState.OtherProductProblem => "ПРОБЛЕМА ДРУГОГО ПРОДУКТА",
        BugState.NeedMoreInfo => "НУЖНО БОЛЬШЕ ИНФОРМАЦИИ",
        BugState.Returned => "ВОЗВРАТ",
        BugState.Closed => "ЗАКРЫТИЕ",
        BugState.Reopened => "ПЕРЕОТКРЫТИЕ",
        _ => state.ToString()
    };

    public string StateDescription => Describe(State);
}

internal static class Program
{
    private static void Main()
    {
        var bug = new Bug();

        Console.WriteLine("=== Демонстрация workflow баг-репорта (Stateless) ===");
        Console.WriteLine($"Старт: {bug.StateDescription} ({bug.State})");

        bug.Fire(BugTrigger.SubmitToTriage);
        Console.WriteLine($"После разбора очереди: {bug.StateDescription}");

        bug.Fire(BugTrigger.RequestMoreInfo);
        Console.WriteLine($"Запрошена информация: {bug.StateDescription}");

        bug.Fire(BugTrigger.ReturnToTriage);
        Console.WriteLine($"Вернулись в разбор: {bug.StateDescription}");

        bug.Fire(BugTrigger.StartFixing);
        Console.WriteLine($"В работе у разработчика: {bug.StateDescription}");

        bug.Fire(BugTrigger.RequestMoreInfo);
        Console.WriteLine($"Из исправления снова нужна информация: {bug.StateDescription}");

        bug.Fire(BugTrigger.StartFixing);
        Console.WriteLine($"Снова в исправлении: {bug.StateDescription}");

        bug.Fire(BugTrigger.ReturnAfterFailedFix);
        Console.WriteLine($"Возврат тестировщика (не решено): {bug.StateDescription}");

        bug.Fire(BugTrigger.CompleteReturnToTriage);
        Console.WriteLine($"После возврата снова разбор: {bug.StateDescription}");

        bug.Fire(BugTrigger.StartFixing);
        Console.WriteLine($"Снова в работе у разработчика: {bug.StateDescription}");

        bug.Fire(BugTrigger.ProblemSolved);
        Console.WriteLine($"Закрыт как исправленный: {bug.StateDescription}");

        bug.Fire(BugTrigger.Reopen);
        Console.WriteLine($"Переоткрыт: {bug.StateDescription}");

        bug.Fire(BugTrigger.EnterTriageAfterReopen);
        Console.WriteLine($"Снова на разборе: {bug.StateDescription}");

        Console.WriteLine();
        Console.WriteLine("Доступные триггеры из текущего состояния:");
        foreach (BugTrigger t in Enum.GetValues<BugTrigger>())
        {
            if (bug.CanFire(t))
                Console.WriteLine($"  - {t}");
        }
    }
}
