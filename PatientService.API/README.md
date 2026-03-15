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
