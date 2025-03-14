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

## **4. Deployment Instructions**

### Prerequisites
- Install Docker and Docker Compose on your system.

### Steps to Build and Run
1. Clone this repository:
   ```bash
   git clone 
   cd KerryTestAssignment
   ```

2. Place your SQLite database file (`database.sqlite`) in the root directory of this repository.

3. Build and start containers using Docker Compose:
   ```bash
   docker compose up --build
   ```

4. Access the APIs:
   - RestApi: `http://localhost:8787`
     Example endpoint: `http://localhost:8787/api/addresses`
   
5. Verify functionality using tools like Postman or curl.

## Issue
- Getting an error when trying to test it through docker, due to lack of time I could not invest more time to the assignment. 
