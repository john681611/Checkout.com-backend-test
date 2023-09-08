# Chekout.com Backend Payment Service Test
A payment API using a MongoDB as storage and connects to a mock CKO Bank API

## Decisions/Considerations/Assumptions:
 - API to be REST - Could be GRPC for better contracts but decided for REST to have wider reach
 - Dumb Auth Service to be mocked in REST package


 ## Auth
 All requests expect a Authorisation bearer token its hard coded to only accept as its only a stub service

        Authorization: Bearer 1234"

# Local Docker MongoDB

       docker run --name checkout-mongo -d mongo


# TODOS:
 - BANK API Client
 - BANK MOCK API
 - Service Interfaces
 - Validation of Requests
 - TESTS