# Chekout.com Backend Payment Service Test
A payment API using a MongoDB as storage and connects to a mock CKO Bank API acording to this spec: https://c.smartrecruiters.com/sr-company-attachments-prod-aws-dc5/63594668c67784587b6c5a3f/fe7584e2-e8df-46f0-ab14-446b6df44411?r=s3-eu-central-1

## Pre-requisites
- Dotnet 7.0.306
- Docker
- Docker Compose

## Supporting Infra
The app needs a MongoDB and Mock back instance to work. A docker compose has been provided to spool these up.

       docker compose up

## Run local
As a dotnet app running up just uses a CLI. Enviroment variables have been provided for you (Assuming you are running the Supporting infa)

       souce .env
       cd Payment.REST
       dotnet run


## Docker
It can run on docker with the `Dockerfile`` provided although the `.env` file may not be suitable networking wise.

## Enviroment Variables
Fairly self explanatory. Make sure its set otherwise the API will crash on first request. `.env` is there already for you

       MONGO_CONNECTION="mongodb://admin:admin@localhost:27017/"
       MONGO_DB="checkout"
       CKO_BANK_URL="http://localhost:5251"
       CKO_BANK_API_KEY="4321"

## Swagger
Swagger should be setup and running on http://localhost:5059/swagger/index.html

## Auth
Auth is mocked for now and hard coded to use `Authorisation: Bearer 1234` Its just stubbed as a Proper app would call a Auth Service and the token would of come from an inital validaiton within that auth service


## Testing
Everything is tested with xunit and Moq. I found the MongoDB Mocking the hardest due to the extentions used (Moq can't handel them so you gotta go digging)

       dotnet test

### Manual testing
If you want to try out the app you can control the status the bank returns using the `Referance` field
- NO MONEY -> INSUFFICENT FUNDS
- REJECT ME -> REJECTED
- Anything else -> Approved

## Design

![Design](./ReadmeAssets/Design.png "Design")


### Decisions/Considerations/Assumptions:

 - Mongo DB chosen because I've worked with it before and its nice and simple for this applicaiton. Probabbly doesn't scale well without Indexes and even then you could move to DynamoDB/Cassandra for better performance with current data)
 - API to be REST - Could be GRPC for better contracts but decided for REST to have wider reach
 - Dumb Auth Service to be mocked in REST package
 - Validation is done by the MS API controller but it could live in Payment.logic if REST was to swich out for something else. (Still tested)
 - Packages seperated to allow re-use in future (Swich out REST for GRPC or SOAP (if ya old school))
 - Didn't bother implementing secondry banks but it would be more client and a swich based on identifying info
 - Assume that in relaity there would be much more validation and ways of identifying banks and users (via auth)
 - Potential case of MongoDB failing and payments being made and not saved. In this case I'd probabbly cache and either dump onto a external Queue servie to be dealth with and respond with the payment status (Assuming there isn't laws and cancel paymet methods already)
 - Avoid storing card details so I only store the record of transation to minimise value of DB.


### Cloud Infra setup
This would probabbly run on a K8s infra with the MongoDB probabbly being hosted on Atlas on in whatever infra we are using. Deployed and PR testing with Github Actions and infra deployed using Terraform.

## Stuff done with more time:

- Propper HTTPS
- Mongo Encrypt at rest
- Metrics
- Probably a bunch of stuff I'll think of right after submission. Unfortunatly I'll forget it before being asked about it :P