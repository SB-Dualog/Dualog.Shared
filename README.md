# Dualog.Shared
This is a shared project that was primarily made for eCatch. We then figured out we could reuse some code for the portal, myEcatch. 
This project contains code for parsing Ers messages from NAF (North Atlantic Format) messages to understandable objects, and it also contains lists of harbours, fish species etc (located in /ReferenceTables).

# MyGet
We use MyGet as a package host. When pushing to master, it will increment the build number and publish the newest version of the package on our private myget feed.
In order to consume this pacakge, you must add https://www.myget.org/F/dualog/auth/f2f157ec-2210-48b2-9f95-784c06cd9bda/api/v3/index.json as a NuGet package feed inside VS, or add it as a package source for your build if you are using TeamCity or some other build service/server.
