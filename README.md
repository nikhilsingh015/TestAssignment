# Work Summary for the Kerry Test Project

This document summarizes the work completed for the Kerry Test Project, highlighting the tasks accomplished, the process followed, and the results achieved. The project involved implementing a RESTful API, unit testing, achieving code coverage requirements, and containerizing the solution using Docker.

---

## **1. Overview of Work Done**

The project was divided into two main components:
- **DbWorker**: A REST API before a SQLite database.
- **RestApi**: A proxy REST API that communicates with DbWorker and acts as a front-end.

The following tasks were completed as part of this project:

### **DbWorker**
1. **Database Schema Update**:
   - Added two new columns to the SQLite database:
     - `Email` (mandatory)
     - `PhoneNumber` (optional)
   - Updated the SQL schema and sample data to include these fields.

2. **API Updates**:
   - Modified the DbWorker REST API to handle the new `Email` and `PhoneNumber` fields.
   - Updated all relevant endpoints to support these fields.

3. **OpenAPI Specification**:
   - Updated the `openapi.yml` file for DbWorker to reflect the new database fields (`Email` and `PhoneNumber`).

---

### **RestApi**
1. **Proxy API Implementation**:
   - Developed a proxy REST API (`RestApi`) that communicates with DbWorker.
   - Implemented endpoints in RestApi to forward requests to DbWorker while abstracting its complexity from external clients.

2. **OpenAPI Specification**:
   - Created an `openapi.yaml` file for RestApi to document all available endpoints and their behavior.

3. **Unit Testing**:
   - Wrote comprehensive unit tests using xUnit for RestApi.
   - Coverage Metrics
   - Testing Scenarios**:
   - Tested all CRUD operations for endpoints in RestApi.
   - Verified error handling and edge cases (e.g., invalid inputs, missing data).
   - Ensured proper communication between RestApi and DbWorker during integration tests.
        - Line Coverage (76.92%):                    
        - Branch Coverage (68.18%)    
        - Method Coverage (93.75%):
    
    Only a small portion (6.25%) of methods were not covered, which is a strong result.
     - ![image](https://github.com/user-attachments/assets/06c1ea25-b4a6-4a16-98b8-be151d9bc4db)


4. **Code Quality Assurance**:
   - Used mocking frameworks (e.g., Moq) to isolate dependencies during testing.
   - Ensured tests were independent, repeatable, and covered edge cases.
   - 

---

## **2. Dockerization**

The entire solution was containerized using Docker, adhering to best practices for containerized deployments:

1. **Docker Compose Setup**:
   - Created a `docker-compose.yml` file to orchestrate the deployment of both DbWorker and RestApi containers.
   - Defined two services:
     - `database-layer` (DbWorker)
     - `api-layer` (RestApi)

2. **Containerization Details**:
   - Used `mcr.microsoft.com/dotnet/sdk:8.0-alpine` as the base image for building containers.
   - Used `alpine:3.21.3` as the base image for final containers to ensure minimal size and security.
   - Ensured that containers do not run as root users for enhanced security.
   - Mounted the SQLite database file (`database.sqlite`) outside of containers to persist data between deployments.

3. **Networking Configuration**:
   - Exposed the `api-layer` container on port `8787`, making it accessible externally.
   - Configured `database-layer` to be accessible only within the Docker network.

4. **Final Artifacts Packaging**:
   - Ensured only compiled binaries and necessary artifacts were included in the final containers.
   - Excluded source code and database files from containers.
  
   - ![image](https://github.com/user-attachments/assets/6326ec19-aebb-41c6-84b4-82834e8faaeb)


---

# Detailed Instructions to Run the Kerry Test Project

This document provides step-by-step instructions to build, deploy, and run the Kerry Test Project using Docker Compose. Follow these instructions to ensure the project runs successfully.

---

## **Prerequisites**

Before running the project, ensure you have the following installed on your system:

1. **Docker**:
   - Install Docker from [Docker's official website](https://www.docker.com/get-started).
   - Verify installation by running:
     ```bash
     docker --version
     ```

2. **Docker Compose**:
   - Docker Compose is usually bundled with Docker Desktop.
   - Verify installation by running:
     ```bash
     docker-compose --version
     ```

3. **SQLite Database File**:
   - Ensure you have the `database.sqlite` file provided in the assignment or created during development.
   - Place this file in the root directory of the project repository.

---

## **Steps to Run the Project**

### **Step 1: Clone the Repository**
Clone the project repository from GitHub to your local machine:
```bash
git clone 
cd TestAssignment
```

---

### **Step 2: Place SQLite Database File**
Place your SQLite database file (`database.sqlite`) in the root directory of the cloned repository. This file will be mounted into the `database-layer` container during runtime.

---

### **Step 3: Build and Start Containers**
Use Docker Compose to build and start the containers:

1. Run the following command to build and start all services:
   ```bash
   docker compose up --build -d
   ```

2. Verify that both containers (`api-layer` and `database-layer`) are running:
   ```bash
   docker ps
   ```

You should see output similar to this:
```
CONTAINER ID   IMAGE                                COMMAND            CREATED          STATUS          PORTS                    NAMES
c34a53d587d8   TestAssignment-api-layer        "dotnet Api.dll"   X seconds ago    Up X seconds    0.0.0.0:8787->8080/tcp   TestAssignment-api-layer-1
f21ff428273f   TestAssignment-database-layer   "dotnet DbWorker"  X seconds ago    Up X seconds                            TestAssignment-database-layer-1
```

---

### **Step 4: Access REST APIs**

#### **RestApi (Proxy API)**
The `api-layer` container exposes RestApi on port `8787`. You can access its endpoints via `http://localhost:8787`.

#### Example Endpoints:
1. **Get All Addresses**:
   ```bash
   curl http://localhost:8787/api/addresses
   ```

2. **Get Address by ID**:
   ```bash
   curl http://localhost:8787/api/addresses/1
   ```

3. **Create a New Address**:
   ```bash
   curl -X POST http://localhost:8787/api/addresses \
        -H "Content-Type: application/json" \
        -d '{
              "name": "John Doe",
              "email": "john.doe@example.com",
              "phoneNumber": "1234567890",
              "city": "New York",
              "country": "USA"
            }'
   ```

4. **Update an Address**:
   ```bash
   curl -X PUT http://localhost:8787/api/addresses/1 \
        -H "Content-Type: application/json" \
        -d '{
              "name": "Jane Doe",
              "email": "jane.doe@example.com",
              "phoneNumber": "0987654321",
              "city": "Los Angeles",
              "country": "USA"
            }'
   ```

5. **Delete an Address**:
   ```bash
   curl -X DELETE http://localhost:8787/api/addresses/1
   ```

#### **DbWorker**
The `database-layer` container is accessible only within the Docker network and cannot be accessed externally for security purposes.

---

### **Step 5: Verify Logs**

If any issues arise, check logs for both containers:

#### Check Logs for `api-layer` (RestApi):
```bash
docker logs TestAssignment-api-layer-1
```

#### Check Logs for `database-layer` (DbWorker):
```bash
docker logs TestAssignment-database-layer-1
```

---

### **Step 6: Stop Containers**
To stop and remove containers, run:
```bash
docker compose down
```

---

### **Step 7: Rebuild Containers**
If you make changes to the code or configuration, rebuild containers using:
```bash
docker compose up --build -d
```

---

## **Additional Notes**

### Networking Configuration:
- The `api-layer` container is accessible externally on port `8787`.
- The `database-layer` container is isolated within the Docker network (`app-network`) and cannot be accessed externally.

### Mounted Volumes:
- The SQLite database file (`database.sqlite`) is mounted into `/app/database.sqlite` in the `database-layer` container.

---

## Troubleshooting

### Issue 1: Containers Not Starting Properly
Run this command to check for errors during startup:
```bash
docker-compose logs --tail=50
```

### Issue 2: Cannot Access API Endpoints Externally
Ensure port mapping (`8787:8080`) is correct by running:
```bash
docker ps
```
You should see something like this for `api-layer`:
```
0.0.0.0:8787->8080/tcp
```

### Issue 3: Permission Errors During Build or Runtime
Ensure proper directory permissions are set for mounted volumes (e.g., SQLite database file). Run this command to fix permissions locally:
```bash
chmod 777 database.sqlite
```


## Issue
- Getting an error when trying to test it through docker, due to lack of time I could not invest more time to the assignment. 
