Task("Unit-Test")
    .Does(() =>
    {

    });

Task("UI-Test")
    .Does(() =>
    {

    });

BuildParameters.Tasks.TestTask.IsDependentOn("Unit-Test").IsDependentOn("UI-Test");