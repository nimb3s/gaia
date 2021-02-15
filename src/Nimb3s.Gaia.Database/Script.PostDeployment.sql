/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

begin transaction mergetypes;
    begin try
        print 'adding lookups';
        :r .\Scripts\Post-Deploy\Types\types-runner.sql

        
    end try
    begin catch
        print 'rb transaction'
        select error_procedure() 'errorProcedure',
            error_line() 'errorLine',
            error_message() 'errorMessage',
            error_number() 'errorNumber',
            error_severity() 'errorSeverity',
            error_state() 'errorState';

        if @@trancount > 0
            rollback transaction;
    end catch
if @@trancount > 0
begin
    commit transaction mergetypes;
    print 'transaction cmitted';
end
go