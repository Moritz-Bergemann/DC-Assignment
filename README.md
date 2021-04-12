# DC-Assignment
Distributed Computing assignment

## TODO
- **Authenticator**
- **ServiceProvider**
  - Test DataModel
  - Test controllers overall
  - Add exception handling (likely just FindPrimesBetween)
  - Add authentication
  - 3 input types to single input type that contains a list of numbers (and have the API verify the number of inputs is right, either on registry or at service itself)

- **Registry**
  - Add authentication
  - The registry has an additional business logic before providing the service. Every client should 
be authenticated before the service invocation. So, the registry expects a valid token with 
every service call. The registry calls the validate function of the Authentication service and if 
validated the service is provided. Otherwise, the following JSON output is sent:
```
{
“Status”: “Denied”
“Reason”: “Authentication Error”
}
```
- **Client**


## Questions
- Should a user be allowed to log in multiple times?
- What should login return if the login fails? (Currently returning token as -1)
