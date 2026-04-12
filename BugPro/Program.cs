namespace BugPro;

internal static class Program
{
    private static void Main()
    {
        var bug = new Bug();

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
