# Kerry Test Project

This assignment will try to give you a task that would lightly touch on what we do on a daily basis.

The assignment will consists for the following steps:

##  Creating a REST API

The test folder contains 2 sub-folders:
- DbWorker
- RestApi

### DbWorker

DbWorker is not a real worker, it is just another rest api in front of a basic Sqlite database.
The data directory contains the sql instructions and sample data.

### RestApi

This folder contans a project webapi project template already scaffolded.
This is where most of your work should take place.

Your mission is, to create a RESTful API that talks to the DbWorker and acts as a kind of front-end or proxy api.

## Requirements/Tasks

You need to do the following tasks:

### DbWorker
- Add a new column to the database to capture Email address (mandatory) and PhoneNumber (optional).
- Update the DbWorker project to use the new database fields
- Update the openapi.yml file of the DbWorker Project to reflect the new fields.

### RestApi
- Write a proxy API for DbWorker
- Write an openapi.yaml file for this api
- Write xunit test for your API cover 100% of the controllers and at least 70% of the overall codebase.

### Deployment

Our preferred way of deploying our code is using containers.

- Create a docker-compose.yml file that will be used to deploy and build your solution on a linux server.
- Containers names to use:
    - DbWorker => `database-layer`
    - RestApi  =>  `api-layer`
-  `api-layer` container should be accessible on port `8787` from anywhere
- `database-layer` container must not be accesible outside of the docker network
- use `mcr.microsoft.com/dotnet/sdk:8.0-alpine` docker image from Microsoft to build your containers
- use `alpine:3.21.3` base image for your final containers
- ensure your containers are not running as root
- The sqlite database file must not be bundled into the containers.
- Ensure only the final artifacts are packaged into the containers, your database file and  your code is not.
- Your database file should be placed in the main folder of this repository.
- Update this file with instructions on how to build and deploy/run your solution.

## How to submit your code.

To submit your code upload it to a publicly available github repository.


Happy Coding!