# Dualog.Shared
This is a shared project that was primarily made for eCatch. We then figured out we could reuse some code for the portal, myEcatch. 
This project contains code for parsing Ers messages from NAF (North Atlantic Format) messages to understandable objects, and it also contains lists of harbours, fish species etc (located in /ReferenceTables).

# NuGet
We use NuGet as a package host. When pushing to master, it will increment the build number and publish the newest version of the package on the nuget feed.

In order to consume this pacakge, just add the Dualog.Shared package as you do for any other package.

https://www.nuget.org/packages/Dualog.Shared/
