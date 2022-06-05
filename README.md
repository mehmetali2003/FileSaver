
1. DB Exceptions & Solutions
	1.1 Login failed for user 'NT AUTHORITY\SYSTEM'. 
	   Solution: 'NT AUTHORITY\SYSTEM' user must be defined under the FileDB database. 
	   Related solution url: https://stackoverflow.com/questions/24822076/sql-server-login-error-login-failed-for-user-nt-authority-system

	1.2 Membership roles	for user 'NT AUTHORITY\SYSTEM'
			db_accessadmin
			db_datareader
			db_datawriter


2. IIS Exceptions & Solutions
	2.1 Exception: System.Data.SqlClient.SqlException: Login failed for user 'IIS APPPOOL\DefaultAppPool'.
	   Solution: DefaultAppPool\Advance Settings\Identity: LocalSysyem
	   

3. FileDB named database must be created under localhost DB server.

4. All DB objects, data and user creation script file is script.sql
   
   