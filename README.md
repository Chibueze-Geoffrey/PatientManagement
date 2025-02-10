# **Patient Management API**

## **Project Description**
The **Patient Management API** is a web service that provides CRUD (Create, Read, Update, Delete) functionality for managing patient records. The API follows a **layered architecture**, implementing best practices such as:

- **Unit of Work & Generic Repository Pattern** for database interactions
- **DTOs (Data Transfer Objects)** for structured data transfer
- **AutoMapper** for mapping entities to DTOs
- **Middleware for centralized error handling**
- **Logging service integration** for tracking API responses
- **SQLite as a lightweight database**
- **Docker for easy deployment**
- **xUnit Testing** for unit tests
- **Swagger for API Documentation**

This API allows soft deletion, restoration, and permanent deletion of patient records, ensuring robust patient data management.

---

## **Thought Process & Key Design Decisions**

### **1. Layered Architecture**
- **Why?** To separate concerns and ensure maintainability.
- **How?**  
  - **API Layer**: Handles HTTP requests & responses.
  - **Application Layer**: Contains business logic and DTOs.
  - **Domain Layer**: Defines entities and enums.
  - **Infrastructure Layer**: Handles database operations.
  - **Common Layer**: Stores reusable extensions and utilities.

### **2. Unit of Work & Generic Repository Pattern**
- **Why?** To manage database transactions efficiently and keep the code DRY.
- **How?**  
  - `IUnitOfWork` ensures all database operations are completed before committing.  
  - `IRepository<T>` provides a **generic** interface for database operations.

### **3. AutoMapper for DTOs**
- **Why?** To simplify object mapping between **Entities** and **DTOs**.  
- **How?**  
  - `AutoMapperProfile.cs` maps `Patient` to `PatientDto`.  
  - Avoids writing repetitive manual mapping logic.

### **4. Middleware for Exception Handling**
- **Why?** To ensure centralized error handling.  
- **How?**  
  - `ExceptionMiddleware` captures errors and logs them.  
  - Returns a **consistent error response format** to clients.

### **5. xUnit Testing**
- **Why?** To validate business logic.  
- **How?**  
  - Uses `Moq` to **mock dependencies** like repositories.  
  - Ensures `PatientService` correctly implements CRUD logic.

### **6. SQLite for Data Persistence**
- **Why?** SQLite is lightweight and easy to use for local development.  
- **How?**  
  - Migrations are handled via **Entity Framework Core**.

### **7. Docker for Deployment**
- **Why?** To containerize the application and make it easy to deploy anywhere.  
- **How?**  
  - `Dockerfile` is used to **build** and **run** the application in a container.  

---

## **API Endpoints**

### **1. Create a Patient**
- **Method:** `POST`
- **Endpoint:** `/api/patient`

### **2. Get a Patient by ID**
- **Method:** `GET`
- **Endpoint:** `/api/patient/{id}`

### **3. Get All Patients**
- **Method:** `GET`
- **Endpoint:** `/api/patient`

### **4. Update a Patient**
- **Method:** `PUT`
- **Endpoint:** `/api/patient/update/{id}`

### **5. Soft Delete a Patient**
- **Method:** `DELETE`
- **Endpoint:** `/api/patient/SoftDeletePatient/{id}`

### **6. Restore a Soft Deleted Patient**
- **Method:** `PUT`
- **Endpoint:** `/api/patient/restore/{id}`

### **7. Permanently Delete a Patient**
- **Method:** `DELETE`
- **Endpoint:** `/api/patient/permanent/{id}`

---

## **Setup Instructions**

### **1. Clone the Repository**
```sh
git clone https://github.com/yourusername/PatientManagementAPI.git
cd PatientManagementAPI
```

### **2. Install Dependencies**
```sh
dotnet restore
```

### **3. Run the API**
```sh
dotnet run
```

### **4. Run Tests**
```sh
dotnet test
```

---

## **Technology Stack**
- **.NET 8** (C#)
- **Entity Framework Core (SQLite)**
- **AutoMapper** for DTO mapping
- **Middleware for error handling**
- **xUnit for unit testing**
- **Docker** for containerization
- **Swagger (Swashbuckle)** for API documentation

---

## **Conclusion**
This API is designed with **scalability, maintainability, and best practices** in mind. With **soft delete, restore, and permanent deletion features**, it ensures proper patient data management.


