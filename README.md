Decongester V.1, is a test solution for WinningTemp candidate test

Test URL: https://github.com/winningtemp/candidate-test/tree/master/backend



Note: The test solution is re-written completely, it is a data driven (to support the maintinance and extention) and not code driven application with Razor Pages as Test Harness UI,


Instuctions to run/test/examine the solution:

1:- Open the Visual Studio Solution .sln file 

2:- Update the Nuget packages using package manager / package manager console ( PM> Update-Package)

3:- Change the connection string in appsettings.json ( please do not change any other information because it contains a fall-back configuration for the application)

4:- Create the database with the name provided in connection string

5:- Run Update-Database command from the Package Manager Console

6:- Once migrations are applied, Import and run test data script from /Decongestor/DataAccess/TestData/

7:- Once test data is imported, Run the application to test/examine

8:- Solution also contains unit tests project for the application.
