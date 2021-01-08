# MSTest.Repeat

## Summary
If you have written an integration test using either MSTest and are currently running tests in Azure Pipelines or plan to do so soon, you can now with a little change enable you're tests to run in a loop in Azure Pipelines or on you're local machine without having to write loops in you're tests! 

In order to accomplish that you need to use the RepeatTestMethod for MSTest, when you're ready to run tests in a loop, set the Windows environment variable NumberofIterations to the number of iterations you want the test to run in. If this variable is not set, the behavior even when the attributes are added will remain the same as it is today, i.e. run the test without a loop.

## Where to start
To you're existing integration test project, please add the MSTest.Repeat Nuget package to you're project. Once you've done that you're ready for the next step.

### Add RepeatTestMethod Attribute to use with MSTest
In your Test code file, for the test method you want to change, make the following change:

```csharp
    [TestMethod]
    public void MyTestMethod()
    {
        .... // Your test code.
    }
```
to

```csharp
    [RepeatTestMethod]
    public void MyTestMethod()
    {
        ... // Your test code.
    }
```

## My code is ready, what's next.
Now if you are ready to run your test in a loop, from a command line window, run the following command 

```cmd
    setx NumberofIterations n
```
where n is an integer value representing the number of iterations you want to run tests on. 

### References
* [Xunit.Repeat in Nuget](https://github.com/MarcolinoPT/Xunit.Repeat). Code was borrowed from here adding the variation of reading from Enviornment variable as customization.
* [Creating custom MSTest Attributes](https://www.meziantou.net/mstest-v2-customize-test-execution.htm)
