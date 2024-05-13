# Common Errors 
 ### SqlException: Cannot open database "LanguaDb" requested by the login. The login failed. Login failed for user 'IIS APPPOOL\langua
    USE [master]
    GO
    CREATE LOGIN [IIS APPPOOL\langua] FROM WINDOWS WITH DEFAULT_DATABASE=[master]
    GO
    USE [LanguaDb]
    GO
    CREATE USER [IIS APPPOOL\langua] FOR LOGIN [IIS APPPOOL\langua]
    GO
    ALTER ROLE [db_owner] ADD MEMBER [IIS APPPOOL\langua]
    GO





# Helps 
- to use dynamic link only by passing predicate as *string* you have to use NameeSpace : ~System.Linq.Dynamic.Core~ 
- 

# Add mail service 
- turn on 2-step verification 
- create a password app 
- then use genereted passord to connect to stmp server and send mails
- 