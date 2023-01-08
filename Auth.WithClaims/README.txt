Based on the videos:
ASP.NET Core Authentication Schemas (.NET 7 Minimal Apis C#) - https://www.youtube.com/watch?v=N_zVCCpnjXM
ASP.NET Core Authorization (.NET 7 Minimal Apis C#) - https://www.youtube.com/watch?v=5NUCPJTbLWY
ASP.NET Core External Authentication (OAuth, .NET 7 Minimal Apis C#) - https://www.youtube.com/watch?v=PUXpfr1LzPE

ASP.NET Core authorization tutorial with asp.net core c# policy authorization example. 
How to use claims to facilitate authorization.

Run the API and test using the following endpoints in order:

GET /europe/locals/login      => to acquire a login cookie for european citizen
GET /europe/locals			  => 200 if local european passport/cookie
     						     401 if no cookie
    						     403 if no valid claims in cookie
                            
GET /america/locals/login     => to acquire a login cookie for local american
GET /america/locals			  => 200 if local american passport/cookie
     						     401 if no cookie
    						     403 if no valid claims in cookie
 						   
GET /america/visitors/login   => to acquire a login cookie for visitor to America
GET /america/visitors		  => 200 if visitor to America passport/cookie
						         401 if no cookie
						         403 if no valid claims in cookie

                                   
GET /america/president/login  => to acquire a login cookie for president of America using OAuth2
                                 Note: must be previously logged-in as local (/america/locals/login)
GET /america/president		  => 200 if president of America passport/cookie
						         401 if no cookie
						         403 if no valid claims in cookie