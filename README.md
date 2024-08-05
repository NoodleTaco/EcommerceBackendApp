# E-Commerce Backend Microservices

This project is a comprehensive backend solution for an e-commerce application, built using C# ASP.NET Core and following a microservices architecture. It consists of three main services: User Service, Product Service, and Order Service, each responsible for handling specific aspects of the e-commerce platform.


## Architecture Overview

The project follows a microservices architecture, with each service operating independently and communicating via HTTP requests. Each service is structured into three modules:

- **API**: Contains controllers, DTOs, mappers, and the Program.cs file.
- **Infrastructure**: Handles system and database operations, including service and repository implementations and the Database Context.
- **Core**: Houses models, interfaces, and other shared entities used across the API and Infrastructure modules.

## Services

### User Service

Responsible for user management, including registration, authentication, and profile management.

Key features:
- User registration and login
- JWT token generation for authentication
- User profile management

### Product Service

Handles all product-related operations.

Key features:
- Product creation, retrieval, update, and deletion
- Inventory management
- Price information

### Order Service

Manages the order process from creation to fulfillment.

Key features:
- Order creation and management
- Integration with Product Service for inventory updates
- Order item management

## Key Features

- RESTful API design
- JWT-based authentication
- Microservices architecture
- Inter-service communication
- CRUD operations for users, products, and orders
- Paginated GET requests
- Error handling and validation

## Technology Stack

- C# ASP.NET Core
- Entity Framework Core
- SQL Server
- JWT for authentication
- RESTful API principles

## Getting Started

1. Clone the repository
2. Ensure you have .NET Core SDK installed
3. Set up SQL Server and update connection strings in `appsettings.json` for each service
4. Navigate to each service directory and run: 

dotnet restore
dotnet ef database update
dotnet run

5. The services will be available at:
- User Service: http://localhost:5000
- Product Service: http://localhost:5001
- Order Service: http://localhost:5002

## API Endpoints

### User Service
- POST /api/user/register - Register a new user
- POST /api/user/login - User login
- GET /api/user - Get all users (Authorized)
- PUT /api/user/email - Update user email (Authorized)

### Product Service
- GET /api/product - Get all products (Authorized)
- GET /api/product/{id} - Get product by ID (Authorized)
- POST /api/product - Create a new product (Authorized)
- PUT /api/product/{id} - Update a product (Authorized)
- DELETE /api/product/{id} - Delete a product (Authorized)

### Order Service
- GET /api/order - Get all orders (Authorized)
- GET /api/order/{id} - Get order by ID (Authorized)
- POST /api/order - Create a new order (Authorized)
- PUT /api/order/{id} - Update an order (Authorized)
- DELETE /api/order/{id} - Delete an order (Authorized)
- POST /api/order/{orderId}/items - Add item to an order (Authorized)
- DELETE /api/order/{orderId}/items/{itemId} - Remove item from an order (Authorized)

## Authentication

The system uses JWT (JSON Web Tokens) for authentication. The User Service generates tokens upon successful login, which are then used to authenticate requests to protected endpoints across all services.

## Database

Each service maintains its own SQL Server database, created and managed through Entity Framework Core migrations.

## Inter-Service Communication

Services communicate with each other through HTTP requests. For example, the Order Service communicates with the Product Service to check and update product availability when processing orders.

## Error Handling

The application implements comprehensive error handling, including input validation, proper HTTP status codes, and detailed error messages.

