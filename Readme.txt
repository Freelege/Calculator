
This Solution are composed of two projects: Calculator and CalculationService, along with two tests projects.

1. CalculationService is a AWS lambda which is deployed with API Gateway, so that user can invoke the lambda from URL directly.

It will take three URL query parameters:
a. operand ---- 0:add, 1:sub, 2:mul, 3:div
b. num1 ---- the first operator number (decimal)
c. num2 ---- the second operator number (decimal)

example:
https://8mp50eo4sd.execute-api.ap-southeast-2.amazonaws.com/prod?operand=2&num1=100.8&num2=80.2

If the environmentName in appSetting.json is set as "Development", the service would use hard coded parameters to call Calculator lambda to get the result.

should get result: 8084.16

2. Calculator is a simple AWS lambda, and it will be invoked by CalculationService from inside the service.

3. The result along with the input parameters are logged in cloud watch, and also write to console.