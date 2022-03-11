# Gifter

Gifter is an API built using .Net 5 with `UserProfile`, `Post`, and `Comment` resources that implements full CRUD functionality. 
It was built using [Visual Studio 2022 Community Edition](https://visualstudio.microsoft.com/vs/community/). 

The [project](https://github.com/NewForce-at-Mountwest/bangazon-inc/blob/master/book-3-web-api/chapters/GIFTER.md) comes from the NewForce back end curriculum

🔴 This has not been tested in previous versions of Visual Studio.

Testing the API functionality can be done with the built-in testing tool Swagger UI when running the project in Debug mode or without 
just make sure to use a URL like `https://localhost:{port}/swagger/index.html` in your preferred browser. Swagger is really helpful for seeing which endpoints to use.
Otherwise you can use [Postman](https://www.postman.com/downloads/) to test the API if you're into that.

## Seed Data

Select the database you want to use in SQL Server Object Explorer then right-click on the db and select `New Query...`

[Database Setup SQL](https://github.com/NewForce-at-Mountwest/bangazon-inc/blob/master/book-3-web-api/chapters/sql/Gifter.sql) - Run this query first

```sql
-- Create some extra users
SET IDENTITY_INSERT [UserProfile] ON
INSERT INTO [UserProfile]
  ([Id], [Name], [Email], [ImageUrl], [Bio], [DateCreated])
VALUES 
  (3, 'Andy Birt', 'andyb@email.com', null, null, '03-11-2022'),
  (4, 'Steve Powers', 'stevep@email.com', null, null, '03-11-2022'),
  (5, 'Jordan Twyman', 'jordant@email.com', null, null, '03-11-2022'),
  (6, 'Aki Endo', 'akie@email.com', null, null, '03-11-2022'),
  (7, 'Heaven Burdette', 'heavenb@email.com', null, null, '03-11-2022')
SET IDENTITY_INSERT [UserProfile] OFF

-- Create some extra comments
SET IDENTITY_INSERT [Comment] ON
INSERT INTO [Comment]
  ([Id], [UserProfileId], [PostId], [Message])
VALUES
  (2, 3, 2, 'No you stop!'),
  (3, 7, 3, 'I hope you like wearing red paint... or blood... we'),
  (4, 4, 4, 'Animals should be the leaders of humanity'),
  (5, 6, 4, 'Pandas are the leaders of humanity'),
  (6, 5, 5, 'HAHA yep'),
  (7, 4, 5, 'I love the fact this was posted on 4/20'),
  (8, 3, 2, 'He''s not going to...'),
  (9, 5, 2, 'I''m not doing anything wrong myb'),
  (10, 6, 1, 'What?'),
  (11, 7, 1, 'Huh?'),
  (12, 5, 1, 'Hmm...'),
  (13, 4, 1, 'What''s going on?')
SET IDENTITY_INSERT [Comment] OFF
```

## Dependencies

🟢 .NET 5.0

🟢 Microsoft.Data.SqlClient v4.1.0

This is for using [ADO.NET](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/ado-net-overview) which allows using SQL commands as a string that can be executed
against database using C#/.NET. 

## Suggestions

If you feel like this can be more helpful you can let me know. Submit a PR! I'll do what I can to make this easier to understand and follow.
