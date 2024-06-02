# RDStationAPI
 RDStation Marketing API Integration<br/>
 NOTE: This API integration is not fully ready for production, i'm still working on it, so i don't recommend it's use unless you really know what you're doing.
 <br/><br/>
 DISCLAIMER: I didn't create this API Integration template. 
 I got it from one of the integrations that i found on the company that i work at, and my coworker who made the other integration got it from Michael's Vivet repo https://github.com/vivet/GoogleApi.<br/>
 Since I really liked the way that this API Integration was made, i decided to use it as well. <br/>
 It's somewhat different, and simpler, compared to Michaels's GoogleApi because my coworker made a lot of changes and i based my integration on his, but all of the credit for creating this template goes to Michael and the people who worked on it.<br/>
 Also i got to give credit to my coworker, Lucas Lana, because he is the one that made the integration that i based mine upon. And he also taught me a lot, thanks Lucas!<br/>
 I checked Michael's GoogleApi repo and it had a MIT License, so i figured it was ok for me to use it, modify and publish it on mine as well.
 <br/><br/>
 <br/><br/>
 Setup:
 <br/><br/>
 I created a class called Credentials that is used to pass the RDStation credentials to the HttpEngine. I did it this way because I didn't want to keep rewriting the AppSettings.json every time the access_token changed.<br/>
 For now, it's being serialized directly from the JSON to a instance of credentials. It's not ideal, but I had to make it functional as soon as possible. I intend to improve on this.<br/>
 Just keep it in mind that you need to call the API "RenewAccessToken" because the "access_token" expires every 24 hours from the moment it's generated.<br/>
 https://developers.rdstation.com/reference/atualizar-access-token
 <br/><br/>
 IMPORTANT: You'll need to dispose of all the API calls if the token changes, otherwise the HttpEngine wont be set again and it will kepp using the old token.
 <br/><br/>
 <br/><br/>
 To use this API integration it's fairly simple:
 <br/><br/>
 First you create a instance of the request you need.<br/>
 Second you declare the corresponding response equals to RDStationApi."ApiCall".Query() or .QueryAsync(), passing the request and, optionally, the HttpEngineOptions.
 <br/><br/>
 (The HttpEngineOptions has the Boolean property ThrowOnInvalidRequest, default is true.)
 <br/><br/>
 Ex:<br/>
 ```c#
 CreateContactRequest createContactRequest = new(){ parameters... };
 CreateContactResponse createContactResponse = RDStationApi.CreateContact.Query(contactRequest, httpEngineOptions);
 ```
