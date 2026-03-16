Healthcare Appointment Scheduling System

This project implements a Microservices-based Healthcare Appointment System built with ASP.NET Core Web API.It allows patients to schedule appointments with healthcare providers, manage their appointments.

It consists of two independent services:

PatientService – Handles user registration, authentication, and user management.

AppointmentService – Handles appointment scheduling between patients and doctors.

Both services communicate securely using JWT authentication and HTTP-based service-to-service communication.

| Service            | Responsibility                               |
| ------------------ | -------------------------------------------- |
| PatientService     | User registration, login, JWT authentication |
| AppointmentService | Appointment booking and management           |


Technologies Used

ASP.NET Core Web API

Entity Framework Core

JWT Authentication

BCrypt Password Hashing

Microservices Architecture

HttpClient for service communication

Dependency Injection

Clean Architecture principles

Repository Pattern

Unit Testing (xUnit + Moq)


Project Structure

PatientService
   ├── API
   │   └── Controllers
   ├── Application
   │   ├── Commands
   │   ├── DTOs
   │   └── Handlers
   ├── Core
   │   ├── Entities
   │   └── Repositories
   └── Infrastructure
       ├── Data
       ├── Repositories
       └── Services

AppointmentService
   ├── API
   ├── Application
   ├── Core
   └── Infrastructure

Features

PatientService

1.Register User

- Registers Doctors or Patients.

POST /api/Auth/register

Request Body:
{
  "username": "doctor@gmail.com",
  "password": "password123",
  "role": "Doctor"
}

Response:
{
  "message": "User registered successfully",
  "userId": "GUID"
}

2.Login

POST /api/Auth/login

Request:

{
  "username": "doctor@gmail.com",
  "password": "password123"
}

Response:

{
  "message": "User logged in successfully",
  "token": "JWT_TOKEN"
}

Get All Users (Doctor Only)
GET /api/Auth/AllRegisteredUsers

Authorization:

Bearer Token Required
Role: Doctor

Response:

[
  {
    "id": "GUID",
    "username": "doctor@gmail.com",
    "role": "Doctor"
  }
]

Service Communication

PatientService communicates with AppointmentServiceClient using HttpClient.

Running the Application

Requirements

.NET 8 SDK

Visual Studio 2022


If you face any challenges while running the solution, please follow the steps below:

Download the source code from the Git repository.

Create a new folder in the root of the C or D drive. Name the folder using a short name like "repo".

Rename the project folder if necessary by removing “-master” from the project name.

Open the solution in Visual Studio. Right-click on the solution and select Open in Terminal.

Run the restore command:

dotnet restore

Clean the solution.

Delete the bin and obj folders in all projects.

Rebuild the solution.

If the build is successful, the solution should run correctly. If the build fails, unload the Test Project and try building the solution again.

Please follow the same steps for the AppointmentService as well.

If you encounter additional issues, try the following fixes:

Remove the Kafka package from the Infrastructure project if it causes dependency issues.

If the JWT token DLL is not loading properly, remove the JWT reference from the project and add the Shared project reference again. Then rebuild the solution.

I hope these steps help you run the solution successfully.

Note:
Sometimes the Test Project may be blocked by the system. In that case, allow or unblock the repository path because it might be marked as quarantined. After allowing it, try rebuilding the entire solution again.