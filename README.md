# Chekout.com Backend Payment Service Test
A payment API using a MongoDB as storage and connects to a mock CKO Bank API

## Decisions/Considerations/Assumptions:
 - API to be REST - Could be GRPC for better contracts but decided for REST to have wider reach
 - Dumb Auth Service to be mocked in REST package
 - Validation is done by the MS API controller but it could live in Payment.logic if REST was to swich out for something else.


 ## Auth
 All requests expect a Authorisation bearer token its hard coded to only accept as its only a stub service

        Authorization: Bearer 1234"

# Local Docker MongoDB

       docker run --name checkout-mongo -d mongo


# TODOS:
 - TESTS
 - HTTPS?