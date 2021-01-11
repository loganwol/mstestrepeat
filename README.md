# MSTest.Repeat

## Summary
If you have written an integration test using MSTest, and are looking for an easy and efficent way to run tests in a loop (stress/performance), running tests either locally or in Azure Pipelines or plan to do so soon, you can now with a small change. Enable you're tests to run in a loop in Azure Pipelines or on you're local machine without having to write loops in you're tests! 

## How to
Add a reference to MSTest.Repeat nuget package in your test solution. Set the Windows environment variable NumberofIterations to the number of iterations you want the test to run. If this variable is not set, the behavior even when the attributes are added will remain the same as it is today, i.e. run the test without a loop.


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

## Benefit
Many take the approach of running a test multiple times by making a copy of an existing test and putting it in a for loop. In some cases that might be ok, but look closely at your test implementation. Do you have a TestInitialize or ClassInitialize implementation and possibly a corresponding Cleanup implementation as well. If yes then using the Attribute is an accurate representation of how the code functionality under test would behave than a simple loop. In the simple loop implementation, the initialization and cleanup only happens once with the loop happening n times, when we want to be testing the initialization, the functionality and the cleanup all in a loop.
I ran into a couple of cases at work where teams used the simple loop approach and didn't catch any problems but switching to the attribute method, caught long standing stress problems right away.

### References
* [Xunit.Repeat in Nuget](https://github.com/MarcolinoPT/Xunit.Repeat). Code was borrowed from here adding the variation of reading from Enviornment variable as customization.
* [Creating custom MSTest Attributes](https://www.meziantou.net/mstest-v2-customize-test-execution.htm)
