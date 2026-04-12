using BugPro;

namespace BugTests;

[TestClass]
public sealed class UnitTest1
{
    [TestMethod]
    public void InitialState_IsNewDefect()
    {
        var bug = new Bug();
        Assert.AreEqual(BugState.NewDefect, bug.State);
    }

    [TestMethod]
    public void SubmitToTriage_TransitionsToDefectTriage()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.SubmitToTriage);
        Assert.AreEqual(BugState.DefectTriage, bug.State);
    }

    [TestMethod]
    public void FromTriage_StartFixing_GoesToFixing()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.SubmitToTriage);
        bug.Fire(BugTrigger.StartFixing);
        Assert.AreEqual(BugState.Fixing, bug.State);
    }

    [TestMethod]
    public void FromTriage_CloseAsNotABug_GoesToClosed()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.SubmitToTriage);
        bug.Fire(BugTrigger.CloseAsNotABug);
        Assert.AreEqual(BugState.Closed, bug.State);
    }

    [TestMethod]
    public void FromTriage_CloseAsWontFix_GoesToClosed()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.SubmitToTriage);
        bug.Fire(BugTrigger.CloseAsWontFix);
        Assert.AreEqual(BugState.Closed, bug.State);
    }

    [TestMethod]
    public void FromTriage_CloseAsDuplicate_GoesToClosed()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.SubmitToTriage);
        bug.Fire(BugTrigger.CloseAsDuplicate);
        Assert.AreEqual(BugState.Closed, bug.State);
    }

    [TestMethod]
    public void FromTriage_MarkNotReproducible_GoesToNotReproducible()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.SubmitToTriage);
        bug.Fire(BugTrigger.MarkNotReproducible);
        Assert.AreEqual(BugState.NotReproducible, bug.State);
    }

    [TestMethod]
    public void FromTriage_DeferNoTime_GoesToDeferredNoTime()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.SubmitToTriage);
        bug.Fire(BugTrigger.DeferNoTime);
        Assert.AreEqual(BugState.DeferredNoTime, bug.State);
    }

    [TestMethod]
    public void FromTriage_RequireSeparateDecision_GoesToNeedsSeparateDecision()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.SubmitToTriage);
        bug.Fire(BugTrigger.RequireSeparateDecision);
        Assert.AreEqual(BugState.NeedsSeparateDecision, bug.State);
    }

    [TestMethod]
    public void FromTriage_MarkOtherProduct_GoesToOtherProductProblem()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.SubmitToTriage);
        bug.Fire(BugTrigger.MarkOtherProduct);
        Assert.AreEqual(BugState.OtherProductProblem, bug.State);
    }

    [TestMethod]
    public void FromTriage_RequestMoreInfo_GoesToNeedMoreInfo()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.SubmitToTriage);
        bug.Fire(BugTrigger.RequestMoreInfo);
        Assert.AreEqual(BugState.NeedMoreInfo, bug.State);
    }

    [TestMethod]
    public void FromDeferredNoTime_ReturnToTriage_GoesToDefectTriage()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.SubmitToTriage);
        bug.Fire(BugTrigger.DeferNoTime);
        bug.Fire(BugTrigger.ReturnToTriage);
        Assert.AreEqual(BugState.DefectTriage, bug.State);
    }

    [TestMethod]
    public void FromDeferredNoTime_StartFixing_GoesToFixing()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.SubmitToTriage);
        bug.Fire(BugTrigger.DeferNoTime);
        bug.Fire(BugTrigger.StartFixing);
        Assert.AreEqual(BugState.Fixing, bug.State);
    }

    [TestMethod]
    public void FromFixing_RequestMoreInfo_GoesToNeedMoreInfo()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.SubmitToTriage);
        bug.Fire(BugTrigger.StartFixing);
        bug.Fire(BugTrigger.RequestMoreInfo);
        Assert.AreEqual(BugState.NeedMoreInfo, bug.State);
    }

    [TestMethod]
    public void FromFixing_ProblemSolved_GoesToClosed()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.SubmitToTriage);
        bug.Fire(BugTrigger.StartFixing);
        bug.Fire(BugTrigger.ProblemSolved);
        Assert.AreEqual(BugState.Closed, bug.State);
    }

    [TestMethod]
    public void FromFixing_ReturnAfterFailedFix_GoesToReturned()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.SubmitToTriage);
        bug.Fire(BugTrigger.StartFixing);
        bug.Fire(BugTrigger.ReturnAfterFailedFix);
        Assert.AreEqual(BugState.Returned, bug.State);
    }

    [TestMethod]
    public void FromReturned_CompleteReturnToTriage_GoesToDefectTriage()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.SubmitToTriage);
        bug.Fire(BugTrigger.StartFixing);
        bug.Fire(BugTrigger.ReturnAfterFailedFix);
        bug.Fire(BugTrigger.CompleteReturnToTriage);
        Assert.AreEqual(BugState.DefectTriage, bug.State);
    }

    [TestMethod]
    public void FromClosed_Reopen_GoesToReopened()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.SubmitToTriage);
        bug.Fire(BugTrigger.CloseAsNotABug);
        bug.Fire(BugTrigger.Reopen);
        Assert.AreEqual(BugState.Reopened, bug.State);
    }

    [TestMethod]
    public void FromReopened_EnterTriageAfterReopen_GoesToDefectTriage()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.SubmitToTriage);
        bug.Fire(BugTrigger.CloseAsNotABug);
        bug.Fire(BugTrigger.Reopen);
        bug.Fire(BugTrigger.EnterTriageAfterReopen);
        Assert.AreEqual(BugState.DefectTriage, bug.State);
    }

    [TestMethod]
    public void FromNotReproducible_ConfirmNotReproducibleOk_GoesToClosed()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.SubmitToTriage);
        bug.Fire(BugTrigger.MarkNotReproducible);
        bug.Fire(BugTrigger.ConfirmNotReproducibleOk);
        Assert.AreEqual(BugState.Closed, bug.State);
    }

    [TestMethod]
    public void FromNotReproducible_RejectNotReproducible_GoesToReturned()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.SubmitToTriage);
        bug.Fire(BugTrigger.MarkNotReproducible);
        bug.Fire(BugTrigger.RejectNotReproducible);
        Assert.AreEqual(BugState.Returned, bug.State);
    }

    [TestMethod]
    public void Describe_ReturnsExpectedRussianLabel_ForNewDefect()
    {
        Assert.AreEqual("НОВЫЙ ДЕФЕКТ", Bug.Describe(BugState.NewDefect));
    }

    [TestMethod]
    public void StateDescription_MatchesDescribe_OfCurrentState()
    {
        var bug = new Bug();
        Assert.AreEqual(Bug.Describe(bug.State), bug.StateDescription);
        bug.Fire(BugTrigger.SubmitToTriage);
        Assert.AreEqual(Bug.Describe(bug.State), bug.StateDescription);
    }

    [TestMethod]
    public void CanFire_FromNewDefect_AllowsOnlySubmitToTriage()
    {
        var bug = new Bug();
        Assert.IsTrue(bug.CanFire(BugTrigger.SubmitToTriage));
        Assert.IsFalse(bug.CanFire(BugTrigger.StartFixing));
        Assert.IsFalse(bug.CanFire(BugTrigger.Reopen));
    }

    [TestMethod]
    public void Fire_FromNewDefect_StartFixing_ThrowsInvalidOperationException_FromStateless()
    {
        var bug = new Bug();
        var ex = Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(BugTrigger.StartFixing));
        StringAssert.Contains(ex.Message, "permitted", StringComparison.OrdinalIgnoreCase);
    }

    [TestMethod]
    public void Fire_FromNewDefect_ProblemSolved_ThrowsInvalidOperationException_FromStateless()
    {
        var bug = new Bug();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(BugTrigger.ProblemSolved));
    }

    [TestMethod]
    public void Fire_FromDefectTriage_ProblemSolved_ThrowsInvalidOperationException_FromStateless()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.SubmitToTriage);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(BugTrigger.ProblemSolved));
    }

    [TestMethod]
    public void Fire_FromClosed_StartFixing_ThrowsInvalidOperationException_FromStateless()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.SubmitToTriage);
        bug.Fire(BugTrigger.CloseAsDuplicate);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(BugTrigger.StartFixing));
    }

    [TestMethod]
    public void Fire_FromReturned_StartFixing_ThrowsInvalidOperationException_FromStateless()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.SubmitToTriage);
        bug.Fire(BugTrigger.StartFixing);
        bug.Fire(BugTrigger.ReturnAfterFailedFix);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(BugTrigger.StartFixing));
    }

    [TestMethod]
    public void Fire_FromReopened_SubmitToTriage_ThrowsInvalidOperationException_FromStateless()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.SubmitToTriage);
        bug.Fire(BugTrigger.CloseAsWontFix);
        bug.Fire(BugTrigger.Reopen);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(BugTrigger.SubmitToTriage));
    }

    [TestMethod]
    public void Fire_FromNotReproducible_Reopen_ThrowsInvalidOperationException_FromStateless()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.SubmitToTriage);
        bug.Fire(BugTrigger.MarkNotReproducible);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(BugTrigger.Reopen));
    }
}
