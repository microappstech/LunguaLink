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

### When using differente instance of dbcontext in same transaction 
    - should add parameter TransactionScopeAsyncFlowOption.Enabled in init of transaction 
    - Use manually disposing after compliting transaction


#### The diff between Suppress and Required and requiresNew in TrasactionOption
    - Required : indique que la m�thode doit utiliser une transaction existante ou en cr�er une nouvelle si n�cessaire, 
    - RequiresNew indique que la m�thode doit utiliser une nouvelle transaction m�me si une transaction existante est en cours, 
    - Suppress indique que la m�thode doit s'ex�cuter sans transaction, m�me si une transaction existe d�j�

 # Transaction Errors
    - async and await methods inside transaction should enabled asyncflow 




# Helps 
- to use dynamic link only by passing predicate as *string* you have to use NameeSpace : ~System.Linq.Dynamic.Core~ 
- 

# Add mail service 
- turn on 2-step verification 
- create a password app 
- then use genereted passord to connect to stmp server and send mails
- 