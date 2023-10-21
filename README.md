<p align="center">
  <h1 align="center">PersonalFinances-backend</h1>
  <h3 align="center"></h3>
</p>
Backend of a personal finance management web application built with Microsoft .NET SDK targeting .NET 6.0. Ensuring security through JSON Web Tokens (JWT) for user authentication and data protection.

See the frontend
[PersonalFinances-frontend](https://github.com/pedrojosawczuk/PersonalFinances-frontend)

## How to Use?

⚠️ Ensure you have .NET 6.0 installed ⚠️

### Running the Program

Clone the repository to your local machine:
```sh
git clone https://github.com/pedrojosawczuk/PersonalFinances-backend.git
```
Navigate to the local repository folder:
```sh
cd PersonalFinances
```

### Database

To initialize a MariaDB instance, use the following command:

```sh
docker-compose up
```

The database is located in db.sql.

### Backend

To compile and run the code, use the following command:

```sh
dotnet run --project PersonalFinances/PersonalFinances.csproj
```

## JSON Data Communication

### Authorization with Token in JSON Header
To secure API endpoints, token-based authorization is employed. The authentication token must be included in the header of the JSON request. This token serves as proof of the user's identity and authorization to access protected resources. It is typically passed in the Authorization header of the HTTP request.

Here's an example of how to include the token in the request header:

```http
POST {{URL}}/Transaction/
Content-Type: application/json
Authorization: {{JWT}}
```

```json
{
   "description": "Electric Bill",
   "value": 5.0,
   "type": "I",
   "date": "2023-04-10",
   "fkCategory": 3
}
```
More examples in the project's test folder.