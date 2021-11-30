# storefront
Product list to customers which consumed by the vendor's service. 
using .Net Core and Angular template in Visual Studio 2019.
installed Microsoft.AspNet.WebApi.Client NuGet package to use HttpContent.

Configuration:
pull the application use visual studio 2019 or latest version to open the project file.
open 2 developer powershell windows and build .net core application and angular (npm install).
run both apps seperately. 
angular app will run on http://localhost:4200
command line:
Core:
dotnet build

dotnet run

Angular:

ng g s services\product

ng g m models\product

ng g c components/products --module app

ng s

Unit Testing done with:
1. Karma/ Jasmin
