# Lib and Coffee

> There's more to software development than code.

You have to verify the system works from end to end.  At some point in the dev cycle you will need to plug it all together to make sure it works as expected.  

Too much of my career has been focused on ensuring everything works all the time *on my system*.  As I've grown as a developer, my career has gone through several phases.

##  Phase I - Manually Verify

Change a line of code, then manually verify it worked as expected and didn't break anything else.  Keep the code on your system fully operating with each of the following:

1. Database
2. API
3. Web Services
4. UI

But, a problem quickly arises:  Can you truly verify everyting all the time?

## Phase II - Automatically Verify

Enter automated integration testing.  Here's few flavors of integration tests I've written:

1.  SQL Client Database Tests
2.  REST API Tests
3.  Selenium/E2E Tests
4.  Generative Data LOAD Tests
5.  PERF Repetition Tests

It never ceases to amaze me how much there is to do with automated testing.

These have been great to gain greater confidence in the quality of software.  But, there's a catch.

Integration testing by definition is more computationally expensive, time extensive, and generally more involved than UNIT testing.  Putting your integration tests into a CI/CD pipeline also proves to be tricky and can be *brittle*.  

## Phase III - Focus on the UNIT

During the next phase in my career, I turned my attention to UNIT testing.  UNIT testing requires direct inheritance of the production code.  This allows the tests to be decoupled from external concerns such as HTTP connections, database connections, file system access and the like.

I can write what seems like 100s of UNIT tests for every integration test.

Great!  Now, I can write tests and shorten the feedback loop.  I want to know if I've broken some requirement as soon as possible in the software development cycle.

Even here there's a *catch* though.  External dependencies have to be **mocked** out.  Through mocking, you can tell the system to do something *instead* of actually doing the external work.  But, this process has proved to be pain.  The test code is more complicated, takes longer to write, and is harder to maintain.  

## Phase IV - Lib It!

Let me just say that one thing I look forward to everyday is what I call "Lib and Coffee".  It's the type of coding that just requires my thoughts, my code, and probably a cup of coffee.  I just have the problem at hand, the knowledge in my mind, and the code on the screen.  

I startup watchers that run unit tests on file save.  I write a test failure, write the code, then watch the test turn green.  I can be in this zone for hours on end, and at the end, it feels very satisfying to deliver a quality product that is fully tested.  Very nice!


From a functional programming perspective, the `Lib` is the library where you put your purely functional code.  The functions are transparent in that they declare the names, their input types, and their output types.  External dependencies are handled some where else.   

This is the truly *zen* mode of coding!