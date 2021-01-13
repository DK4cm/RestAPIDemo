# RestAPIDemo
A REST API Demo for coffee shop ordering system

Do not use in Production!!!!!!

Do not use in Production!!!!!!

Do not use in Production!!!!!!

1. It is just a demo project,so the security is not good enough in particle use.
2. The SQL statement is hardcore so that it may cause SQL injection problem.
3. The API is not vertify the user in a secure way.
4. The API use post method to process user login data which is plain text and cause security leak.
5. It is not a standarded MVC project since it just use controller function to handle restful api request.
6. In Global.asax, "GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();" should be add to avoid the system response xml data instead of JSON 
