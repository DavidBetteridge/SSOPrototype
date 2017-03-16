# SSOPrototype

## Getting Started

1. Load the solution and start both web sites running

2. Register a user on the first web site "Product 1"

3. Then register a different user on the second web site "Product 2"

4. On the Product A site,  once logged in,  click the Registered Applications button and ensure that the URL is correct for Product B

5. On the Product B site,  once logged in,  click the Authorised Applications button and ensure that the URL is correct for Product A

6. Logout of Product B

7. Return to Product A,  and click the button Labelled Product B

8. This will take you to a page on the Product B site,  asking you which user to would like to link with.  Enter the username and password.

9. You should now be logged into the site as that user.  Click Logout and return to site A

10. Click the Product B again,  this time you should be taken straight into Product B

## Security

This is based on the OAuth "Authorization Code" grant type.  With a couple of important differences!

1. The scope instead of being set to the name of the resource the client wishes to access,  is set to the user's name in product A

2. In the final stage (Access Token Request) the server also passed the ID of the logged in user to the other server for additional verification

* It is (very) important that ClientSecret is not exposed.

## Not Implemented

* The access token does not currently expire


  