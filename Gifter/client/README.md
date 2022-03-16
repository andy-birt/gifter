# The Gifter React Client

Navigate to this directory in the terminal and run `npm start` to get the client running and interact with the Gifter API.

Since there is currently no authentication you will default to posting as a user with an Id of 3 which, if you used the provided API seed data, is me...

Also you will want to run the API using the Gifter option. Otherwise, you could try editing the proxy setting in `package.json` if you know what you're doing.

```json
{
  "name": "client",
  // When Swagger loads after running the api 
  // in debug mode look for that port number and 
  // try it here, that will be your IIS Server port number
  "proxy": "https://localhost:{YOUR_IIS_SERVER_PORT}",
  "version": "0.1.0",
  "private": true,
  // rest of settings...
}
```